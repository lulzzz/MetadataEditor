﻿using System.Web;
using System.Web.Optimization;

namespace Kartverket.MetadataEditor
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/expressive.annotations.validate.js",
                "~/Scripts/globalize.js",
                "~/Scripts/globalize.culture.nb-NO.js",
                "~/Scripts/globalize-custom.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                "~/Scripts/bootstrap-filestyle.js",
                "~/Scripts/geonorge-editor.js",
                "~/Scripts/jquery.cookie.js"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/core.css",
                "~/Content/themes/base/datepicker.css",
                "~/Content/themes/base/theme.css"));
              
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/common.css",
                "~/Content/navbar.css",
                "~/Content/searchbar.css",
                "~/Content/site.css"));
        }
    }
}
