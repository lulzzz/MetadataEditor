﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Kartverket.MetadataEditor.Models
{
    public class WfsGetCapabilities11Parser
    {
        private static XNamespace WFS = "http://www.opengis.net/wfs";
        private static XNamespace INSPIRE = "http://inspire.ec.europa.eu/schemas/inspire_vs/1.0";
        private static XNamespace INSPIRE_COMMON = "http://inspire.ec.europa.eu/schemas/common/1.0";

        public WfsServiceViewModel Parse(XDocument getCapabilitiesXmlDocument)
        {
            WfsServiceViewModel serviceModel = new WfsServiceViewModel();

            XElement root = getCapabilitiesXmlDocument.Element(WFS + "WFS_Capabilities");

            //serviceModel.SupportsEnglishGetCapabilities = CheckSupportForEnglishGetCapabilities(root);

            IEnumerable<XElement> layers =
                    from el in root.Elements(WFS + "FeatureTypeList").Elements(WFS + "FeatureType")
                    select el;

            List<WfsLayerViewModel> layerModels = new List<WfsLayerViewModel>();

            foreach (var layer in layers)
            {
                layerModels.AddRange(ParseLayerData(layer));
            }

            serviceModel.Layers = layerModels;

            return serviceModel;
        }

        private List<WfsLayerViewModel> ParseLayerData(XElement layer)
        {
            List<WfsLayerViewModel> parsedLayers = new List<WfsLayerViewModel>();

            var boundingBox = layer.Element(WFS + "EX_GeographicBoundingBox");
            var nameElement = layer.Element(WFS + "Name");
            var titleElement = layer.Element(WFS + "Title");
            var abstractElement = layer.Element(WFS + "Abstract");
            List<string> keywords = ParseKeywords(layer.Element(WFS + "KeywordList"));

            string name = nameElement != null ? nameElement.Value : null;

            IEnumerable<XElement> subLayers = from el in layer.Elements(WFS + "FeatureType") select el;

            parsedLayers.Add(new WfsLayerViewModel
            {
                Name = name,
                Title = titleElement != null ? titleElement.Value : null,
                Abstract = abstractElement != null ? abstractElement.Value : null,
                BoundingBoxEast = boundingBox != null ? boundingBox.Element(WFS + "eastBoundLongitude").Value : null,
                BoundingBoxWest = boundingBox != null ? boundingBox.Element(WFS + "westBoundLongitude").Value : null,
                BoundingBoxNorth = boundingBox != null ? boundingBox.Element(WFS + "northBoundLatitude").Value : null,
                BoundingBoxSouth = boundingBox != null ? boundingBox.Element(WFS + "southBoundLatitude").Value : null,
                Keywords = keywords,
                IsGroupLayer = subLayers != null && subLayers.Count() > 0
            });

            if (subLayers != null && subLayers.Count() > 0)
            {
                foreach (var subLayer in subLayers)
                {
                    parsedLayers.AddRange(ParseLayerData(subLayer));
                }
            }

            return parsedLayers;
        }


        private bool CheckSupportForEnglishGetCapabilities(XElement root)
        {
            bool supportsEnglishGetCapablities = false;

            XElement extendedCapabilities = root.Element(WFS + "Capability").Element(INSPIRE + "ExtendedCapabilities");
            if (extendedCapabilities != null)
            {
                XElement supportedLanguages = extendedCapabilities.Element(INSPIRE_COMMON + "SupportedLanguages");
                if (supportedLanguages != null)
                {
                    XElement language = supportedLanguages.Element(INSPIRE_COMMON + "SupportedLanguage").Element(INSPIRE_COMMON + "Language");
                    if (language.Value == "eng")
                    {
                        supportsEnglishGetCapablities = true;
                    }
                }
            }
            return supportsEnglishGetCapablities;
        }

        public Dictionary<string, Dictionary<string, string>> ParseEnglishGetCapabilities(XDocument getCapabilitiesEnglish)
        {
            Dictionary<string, Dictionary<string, string>> englishData = new Dictionary<string, Dictionary<string, string>>();

            IEnumerable<XElement> englishLayers =
                from el in getCapabilitiesEnglish.Elements(WFS + "WFS_Capabilities").Elements(WFS + "Capability").Elements(WFS + "Layer").Elements(WFS + "Layer")
                select el;

            if (englishLayers != null)
            {
                foreach (var layer in englishLayers)
                {
                    var nameElement = layer.Element(WFS + "Name");
                    string name = nameElement != null ? nameElement.Value : null;

                    var titleElement = layer.Element(WFS + "Title");
                    string title = titleElement != null ? titleElement.Value : null;

                    var abstractElement = layer.Element(WFS + "Abstract");
                    string @abstract = abstractElement != null ? abstractElement.Value : null;

                    var values = new Dictionary<string, string>();
                    values.Add("title", title);
                    values.Add("abstract", @abstract);

                    englishData.Add(name, values);
                }
            }

            return englishData;
        }


        private static List<string> ParseKeywords(XElement keywordListElement)
        {
            List<string> keywords = new List<string>();
            if (keywordListElement != null)
            {
                var keywordListValues = keywordListElement.Elements(WFS + "Keyword");
                foreach (var keyword in keywordListValues)
                {
                    var keywordValue = keyword.Value;
                    if (!string.IsNullOrWhiteSpace(keywordValue))
                    {
                        keywords.Add(keywordValue);
                    }
                }
            }
            return keywords;
        }
    }
}