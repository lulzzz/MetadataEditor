﻿using System.Configuration;
using System.IO;
using Kartverket.MetadataEditor.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kartverket.MetadataEditor.Util;
using Resources;

namespace Kartverket.MetadataEditor.Controllers
{
    public class MetadataController : Controller
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MetadataService _metadataService;

        public MetadataController()
        {
            _metadataService = new MetadataService();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            MetadataCreateViewModel model = new MetadataCreateViewModel
            {
                MetadataContactName = GetSecurityClaim("urn:oid:1.3.6.1.4.1.5923.1.1.1.6"),
                MetadataContactOrganization = GetSecurityClaim("organization"),
                MetadataContactEmail = GetSecurityClaim("urn:oid:1.2.840.113549.1.9.1"),
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(MetadataCreateViewModel model)
        {
            string organization = GetSecurityClaim("organization");
            model.MetadataContactOrganization = organization;
            if (ModelState.IsValid)
            {
                string username = GetUsername();
                string uuid = _metadataService.CreateMetadata(model, username);
                Log.Info(string.Format("Created new metadata: {0} [uuid = {1}] for user: {2} on behalf of {3} ", model.Title, uuid, username, organization));
                return RedirectToAction("Edit", new { uuid = uuid });
            }
            return View(model);
        }

        public ActionResult Index(MetadataMessages? message, string organization = "", string searchString = "", int offset = 1, int limit = 50)
        {
            ViewBag.StatusMessage =
                message == MetadataMessages.InvalidUuid ? Resources.UI.Error_InvalidUuid
                : "";

            MetadataIndexViewModel model = new MetadataIndexViewModel();
            
            if (User.Identity.IsAuthenticated)
            {
                string userOrganization = GetSecurityClaim("organization");
                string role = GetSecurityClaim("role");
                if (!string.IsNullOrWhiteSpace(role) && role.Equals("nd.metadata_admin"))
                {
                    model = _metadataService.SearchMetadata(organization, searchString, offset, limit);
                    model.UserIsAdmin = true;
                }
                else
                {
                    model = _metadataService.SearchMetadata(userOrganization, searchString, offset, limit);
                }

                model.UserOrganization = userOrganization;
            }

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            return View(model);
        }

        private string GetUsername()
        {
            return GetSecurityClaim("username");
        }

        private string GetSecurityClaim(string type)
        {
            string result = null;
            foreach (var claim in System.Security.Claims.ClaimsPrincipal.Current.Claims)
            {
                if (claim.Type == type && !string.IsNullOrWhiteSpace(claim.Value))
                {
                    result = claim.Value;
                    break;
                }
            }
            
            // bad hack, must fix BAAT
            if (!string.IsNullOrWhiteSpace(result) && type.Equals("organization") && result.Equals("Statens kartverk"))
            {
                result = "Kartverket";
            }

            return result;
        }


        [HttpGet]
        [Authorize]
        public ActionResult Edit(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return HttpNotFound();

            try
            {   
                MetadataViewModel model = _metadataService.GetMetadataModel(uuid);
                string role = GetSecurityClaim("role");
                if (HasAccessToMetadata(model))
                {
                    PrepareViewBagForEditing(model);
                    return View(model);
                }
                else
                {
                    TempData["failure"] = "Du har ikke tilgang til å redigere disse metadataene";
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                Log.Error("Error while getting metadata with uuid = " + uuid, e);
                TempData["failure"] = "Feilmelding: " + e.Message;
                return View("Error");
            }

            
        }


        private void PrepareViewBagForEditing(MetadataViewModel model)
        {
            ViewBag.TopicCategoryValues = new SelectList(GetListOfTopicCategories(), "Key", "Value", model.TopicCategory);
            ViewBag.SpatialRepresentationValues = new SelectList(GetListOfSpatialRepresentations(), "Key", "Value", model.SpatialRepresentation);
            ViewBag.UnitsOfDistributionValues = new SelectList(GetListOfUnitsOfDistribution(), "Key", "Value", model.UnitsOfDistribution);
            ViewBag.MaintenanceFrequencyValues = new SelectList(GetListOfMaintenanceFrequencyValues(), "Key", "Value", model.MaintenanceFrequency);
            ViewBag.StatusValues = new SelectList(GetListOfStatusValues(), "Key", "Value", model.Status);
            ViewBag.SecurityConstraintValues = new SelectList(GetListOfClassificationValues(), "Key", "Value", model.SecurityConstraints);
            ViewBag.UseConstraintValues = new SelectList(GetListOfRestrictionValues(), "Key", "Value", model.UseConstraints);
            ViewBag.AccessConstraintValues = new SelectList(GetListOfRestrictionValues(), "Key", "Value", model.AccessConstraints);
            ViewBag.CreateProductSheetUrl =
                System.Web.Configuration.WebConfigurationManager.AppSettings["ProductSheetGeneratorUrl"] + model.Uuid;
            ViewBag.ThumbnailUrl =
                System.Web.Configuration.WebConfigurationManager.AppSettings["EditorUrl"] + "thumbnails/";
            ViewBag.GeoNetworkViewUrl = GeoNetworkUtil.GetViewUrl(model.Uuid);
            ViewBag.GeoNetworkXmlDownloadUrl = GeoNetworkUtil.GetXmlDownloadUrl(model.Uuid);
            var seoUrl = new SeoUrl("", model.Title);
            ViewBag.KartkatalogViewUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["KartkatalogUrl"] + "Metadata/" + seoUrl.Title + "/" + model.Uuid; ;
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(string uuid, string action, MetadataViewModel model, string ignoreValidationError)
        {
            ViewBag.IsAdmin = "0";
            string role = GetSecurityClaim("role");
            if (!string.IsNullOrWhiteSpace(role) && role.Equals("nd.metadata_admin")) {
                ViewBag.IsAdmin = "1";
            }

            if (ignoreValidationError == "1" && ViewBag.IsAdmin == "1") 
            { 
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }
            }

            if (ModelState.IsValid)
            {
                if (action.Equals(UI.Button_SaveAsXml))
                {
                    Stream fileStream = _metadataService.SaveMetadataAsXml(model);
                    var fileStreamResult = new FileStreamResult(fileStream, "application/xml")
                    {
                        FileDownloadName = model.Title + "_" + uuid + ".xml"
                    };

                    return fileStreamResult;
                }
                else
                {
                    SaveMetadataToCswServer(model);
                    return RedirectToAction("Edit", new {uuid = model.Uuid});
                }
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                Log.Debug("Model for " + uuid + " is not valid: " + messages);
            }

            PrepareViewBagForEditing(model);
            return View(model);
        }

        private void SaveMetadataToCswServer(MetadataViewModel model)
        {
            try
            {
                _metadataService.SaveMetadataModel(model, GetUsername());
                TempData["success"] = UI.Metadata_Edit_Saved_Success;
            }
            catch (Exception e)
            {
                Log.Error("Error while editing metadata with uuid = " + model.Uuid, e);
                TempData["failure"] = String.Format(UI.Metadata_Edit_Saved_Failure, e.Message);
            }
        }

        public Dictionary<string, string> GetListOfTopicCategories()
        {
            return new Dictionary<string, string> 
            {
                {"farming", "Landbruk og havbruk"}, 
                {"biota", "Biologisk mangfold"},
                {"boundaries", "Administrative grenser"},
                {"climatologyMeteorologyAtmosphere","Klima, meteorologi og atomsfære"},
                {"economy","Økonomi"},
                {"elevation","Høydedata"},
                {"environment","Miljødata"},
                {"geoscientificInformation","Geovitenskapelig informasjon"},
                {"health","Helse"},
                {"imageryBaseMapsEarthCover","Basisdata"},
                {"intelligenceMilitary","Militære data"},
                {"inlandWaters","Innsjø og vassdrag"},
                {"location","Posisjonsdata"},
                {"oceans","Kyst og sjø"},
                {"planningCadastre","Plan og eiendom"},
                {"society","Samfunn"},
                {"structure","Konstruksjoner"},
                {"transportation","Transport"},
                {"utilitiesCommunication","Ledningsinformasjon"},
            };
        }

        public Dictionary<string, string> GetListOfUnitsOfDistribution()
        {
            return new Dictionary<string, string> 
            {
                {"kommunevis", "Kommunevis"}, 
                {"fylkesvis", "Fylkesvis"}, 
                {"landsfiler", "Landsfiler"}, 
                {"regional inndeling", "Regional inndeling"}, 
                {"kartbladvis", "Kartbladvis"}, 
            };
        }

        public Dictionary<string, string> GetListOfSpatialRepresentations()
        {
            return new Dictionary<string, string> 
            {
                {"vector", "Vektordata"}, 
                {"grid", "Rasterdata/grid"}, 
                {"textTable", "Teksttabell"}, 
                {"tin", "TIN-modell"}, 
                {"stereoModel", "Stereomodel"}, 
                {"video", "Video"}, 
            };
        }

        public Dictionary<string, string> GetListOfMaintenanceFrequencyValues()
        {
            return new Dictionary<string, string>
            {
                {"continual", "Kontinuerlig"},
                {"daily", "Daglig"},
                {"weekly", "Ukentlig"},
                {"fortnightly", "Annenhver uke"},
                {"monthly", "Månedlig"},
                {"quarterly", "Hvert kvartal"},
                {"biannually", "Hvert halvår"},
                {"annually", "Årlig"},
                {"asNeeded", "Etter behov"},
                {"irregular", "Ujevnt"},
                {"notPlanned", "Ikke planlagt"},
                {"unknown", "Ukjent"},
            };
        }

        public Dictionary<string, string> GetListOfStatusValues()
        {
            return new Dictionary<string, string>
            {
                {"completed", "Fullført"},
                {"historicalArchive", "Arkivert"},
                {"obsolete", "Utdatert"},
                {"onGoing", "Kontinuerlig oppdatert"},
                {"planned", "Planlagt"},
                {"required", "Må oppdateres"},
                {"underDevelopment", "Under arbeid"},
            };
        }

        public Dictionary<string, string> GetListOfClassificationValues()
        {
            return new Dictionary<string, string>
            {
                {"unclassified", "Ugradert"},
                {"restricted", "Begrenset"},
                {"confidential", "Konfidensielt"},
                {"secret", "Hemmelig"},
                {"topSecret", "Topp hemmelig"},
            };
        }

        public Dictionary<string, string> GetListOfRestrictionValues()
        {
            return new Dictionary<string, string>
            {
                {"otherRestrictions", "Andre restriksjoner"},    
                {"restricted", "Beskyttet"},
                {"copyright", "Kopibeskyttet"},
                {"license", "Lisens"},
                {"patent", "Patentert"},
                {"patentPending", "Påvente av patent"},
                {"trademark", "Registrert varemerke"},
            };
        }

        [Authorize]
        [OutputCache(Duration = 0)]
        public ActionResult UploadThumbnail(string uuid, bool scaleImage = false)    
        {
            string filename = null;
            var viewresult = Json(new {});
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];

                if (file.ContentType == "image/jpeg" || file.ContentType == "image/gif" || file.ContentType == "image/png")
                {

                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    filename = uuid + "_" + timestamp + "_" + file.FileName;
                    string fullPath = Server.MapPath("~/thumbnails/" + filename);
             
                    if (scaleImage)
                    {
                        var image = Image.FromStream(file.InputStream);
                        var newImage = ScaleImage(image, 180, 1000);
                        newImage.Save(fullPath);
                    }
                    else
                    {
                        file.SaveAs(fullPath);
                    }

                    viewresult = Json(new { status = "OK", filename = filename });
                }
                else 
                {
                    viewresult = Json(new { status = "ErrorWrongContent" });
                }
            }

            //for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
                viewresult.ContentType = "text/plain";

            return viewresult;
        }


        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult ConfirmDelete(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return HttpNotFound();

            MetadataViewModel model = _metadataService.GetMetadataModel(uuid);

            string role = GetSecurityClaim("role");
            if (HasAccessToMetadata(model))
            {
                return View(model);
            } else {
                return new HttpUnauthorizedResult();
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(string uuid)
        {
            MetadataViewModel model = _metadataService.GetMetadataModel(uuid);

            string role = GetSecurityClaim("role");
            if (HasAccessToMetadata(model))
            {
                _metadataService.DeleteMetadata(uuid, GetUsername());

                TempData["Message"] = "Metadata med uuid " + uuid + " ble slettet.";
                return RedirectToAction("Index");
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }

        private bool HasAccessToMetadata(MetadataViewModel model)
        {
            string organization = GetSecurityClaim("organization");
            string role = GetSecurityClaim("role");
            bool isAdmin = !string.IsNullOrWhiteSpace(role) && role.Equals("nd.metadata_admin");
            return isAdmin || model.HasAccess(organization);
        }

        [Authorize]
        public ActionResult RegisterData()
        {
            string organization = GetSecurityClaim("organization");
            ViewBag.RegisterOrganizationUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["RegistryUrl"] + "api/register/search/organisasjon/" + organization;
            return View();
        
        }

	}
    
    public enum MetadataMessages
    {
        InvalidUuid
    }
}