﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kartverket.MetadataEditor.Models
{
    public class WfsServiceLayerViewModel
    {
        public MetadataViewModel Metadata { get; set; }

        [DefaultValue(typeof(List<WfsLayerViewModel>))]
        public List<WfsLayerViewModel> Layers { get; set; }

        public string WfsUrl { get; set; }

        public string GetCapabilitiesUrl()
        {
            if (WfsUrl != null)
            {
                if (!WfsUrl.EndsWith("?"))
                {
                    WfsUrl = WfsUrl + "?";
                }
                return WfsUrl + "service=wfs&request=GetCapabilities";
            }
            else
                return null;

        }
    }
}