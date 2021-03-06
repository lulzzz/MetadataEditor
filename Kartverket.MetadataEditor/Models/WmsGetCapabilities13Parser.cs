﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Kartverket.MetadataEditor.Models
{
    public class WmsGetCapabilities13Parser
    {
        private static XNamespace WMS = "http://www.opengis.net/wms";
        private static XNamespace INSPIRE = "http://inspire.ec.europa.eu/schemas/inspire_vs/1.0";
        private static XNamespace INSPIRE_COMMON = "http://inspire.ec.europa.eu/schemas/common/1.0";


        public WmsServiceViewModel Parse(XDocument getCapabilitiesXmlDocument)
        {
            WmsServiceViewModel serviceModel = new WmsServiceViewModel();

            XElement root = getCapabilitiesXmlDocument.Element(WMS + "WMS_Capabilities");

            serviceModel.SupportsEnglishGetCapabilities = CheckSupportForEnglishGetCapabilities(root);

            IEnumerable<XElement> layers =
                    from el in root.Elements(WMS + "Capability").Elements(WMS + "Layer").Elements(WMS + "Layer")
                    select el;

            List<WmsLayerViewModel> layerModels = new List<WmsLayerViewModel>();

            foreach (var layer in layers)
            {
                layerModels.AddRange(ParseLayerData(layer));
            }
            
            serviceModel.Layers = layerModels;

            return serviceModel;
        }

        private List<WmsLayerViewModel> ParseLayerData(XElement layer)
        {
            List<WmsLayerViewModel> parsedLayers = new List<WmsLayerViewModel>();

            var boundingBox = layer.Element(WMS + "EX_GeographicBoundingBox");
            var nameElement = layer.Element(WMS + "Name");
            var titleElement = layer.Element(WMS + "Title");
            var abstractElement = layer.Element(WMS + "Abstract");
            List<string> keywords = ParseKeywords(layer.Element(WMS + "KeywordList"));

            string name = nameElement != null ? nameElement.Value : null;

            IEnumerable<XElement> subLayers = from el in layer.Elements(WMS + "Layer") select el;

            parsedLayers.Add(new WmsLayerViewModel
            {
                Name = name,
                Title = titleElement != null ? titleElement.Value : null,
                Abstract = abstractElement != null ? abstractElement.Value : null,
                BoundingBoxEast = boundingBox != null ? boundingBox.Element(WMS + "eastBoundLongitude").Value : null,
                BoundingBoxWest = boundingBox != null ? boundingBox.Element(WMS + "westBoundLongitude").Value : null,
                BoundingBoxNorth = boundingBox != null ? boundingBox.Element(WMS + "northBoundLatitude").Value : null,
                BoundingBoxSouth = boundingBox != null ? boundingBox.Element(WMS + "southBoundLatitude").Value : null,
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

            XElement extendedCapabilities = root.Element(WMS + "Capability").Element(INSPIRE + "ExtendedCapabilities");
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
                from el in getCapabilitiesEnglish.Elements(WMS + "WMS_Capabilities").Elements(WMS + "Capability").Elements(WMS + "Layer").Elements(WMS + "Layer")
                select el;

            if (englishLayers != null)
            {
                foreach (var layer in englishLayers)
                {
                    var nameElement = layer.Element(WMS + "Name");
                    string name = nameElement != null ? nameElement.Value : null;

                    var titleElement = layer.Element(WMS + "Title");
                    string title = titleElement != null ? titleElement.Value : null;

                    var abstractElement = layer.Element(WMS + "Abstract");
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
                var keywordListValues = keywordListElement.Elements(WMS + "Keyword");
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