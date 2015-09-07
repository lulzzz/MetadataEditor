using System;
using System.Web.Http;
using System.Web.Mvc;
using Kartverket.MetadataEditor.Areas.HelpPage.ModelDescriptions;
using Kartverket.MetadataEditor.Areas.HelpPage.Models;

namespace Kartverket.MetadataEditor.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            //return View(Configuration.Services.GetApiExplorer().ApiDescriptions); 
            var apiExplorer = Configuration.Services.GetApiExplorer();
            for (int i = apiExplorer.ApiDescriptions.Count - 1; i >= 0; i--)
            {
                if (!apiExplorer.ApiDescriptions[i].ActionDescriptor.ControllerDescriptor.ControllerName.Contains("ApiMeta")) { apiExplorer.ApiDescriptions.RemoveAt(i); }
            }
            return View(apiExplorer.ApiDescriptions);

        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }
}