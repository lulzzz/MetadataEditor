﻿using System.Runtime.CompilerServices;
using ExpressiveAnnotations.Attributes;
using GeoNorgeAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Kartverket.MetadataEditor.Models
{
    public class SimpleMetadataViewModel
    {

        public SimpleMetadataViewModel()
        {
            KeywordsPlace = new List<string>();
            KeywordsNationalInitiative = new List<string>();
            KeywordsNationalTheme = new List<string>();
        }

        public string Uuid { get; set; }
        public string HierarchyLevel { get; set; }
        public string ParentIdentifier { get; set; }

        [Required(ErrorMessage = null)]
        [Display(Name = "Metadata_Title", ResourceType = typeof(UI))]
        public string Title { get; set; }


        [Required(ErrorMessage = "Sammendrag er påkrevd")]
        [Display(Name = "Metadata_Abstract", ResourceType = typeof(UI))]
        public string Abstract { get; set; }

        public Contact ContactMetadata { get; set; }
        public Contact ContactPublisher { get; set; }
        public Contact ContactOwner { get; set; }


        /* dataset only */
        public string SupplementalDescription { get; set; }
        public string SpecificUsage { get; set; }  // bruksområde
              

        [Display(Name = "Metadata_DateUpdated", ResourceType = typeof(UI))]
        [DisplayFormat(NullDisplayText = "", ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateMetadataUpdated { get; set; }

        public string DistributionProtocol { get; set; }

        [Required(ErrorMessage = "Oppdateringshyppighet er påkrevd")]
        public string MaintenanceFrequency { get; set; }

        public List<String> KeywordsPlace { get; set; }
        public List<String> KeywordsNationalInitiative { get; set; }
        public List<String> KeywordsNationalTheme { get; set; }

        public string Published { get; set; }



        internal bool HasAccess(string organization)
        {
            bool hasAccess = false;

            if (ContactMetadata != null && ContactMetadata.Organization != null && ContactMetadata.Organization.ToLower().Equals(organization.ToLower()))
            {
                hasAccess = true;
            }

            return hasAccess;
        }


        public bool IsService()
        {
            return HierarchyLevel == "service";
        }

        public bool IsDataset()
        {
            return HierarchyLevel == "dataset";
        }

        public bool IsSoftware()
        {
            return HierarchyLevel == "software";
        }

        public bool IsDatasetSeries()
        {
            return HierarchyLevel == "series";
        }

        public bool IsDatasetOrSeriesOrSoftware()
        {
            return IsDataset() || IsDatasetSeries() || IsSoftware();
        }


        public string GetInnholdstypeCSS()
        {
            string t = "label-default";
            if (HierarchyLevel == "dataset") t = "label-success";
            else if (HierarchyLevel == "software") t = "label-warning";
            else if (HierarchyLevel == "service" && (!string.IsNullOrWhiteSpace(ParentIdentifier))) t = "label-info";
            else if (HierarchyLevel == "service") t = "label-info";
            else if (HierarchyLevel == "series") t = "label-primary";

            return t;
        }

        public string GetInnholdstype()
        {
            string t = HierarchyLevel;
            if (HierarchyLevel == "dataset") t = "Datasett";
            else if (HierarchyLevel == "software") t = "Programvare";
            else if (HierarchyLevel == "service" && (!string.IsNullOrWhiteSpace(ParentIdentifier)) && DistributionProtocol != null && DistributionProtocol.Contains("WFS")) t = "WFS-lag (Tjenestelag)";
            else if (HierarchyLevel == "service" && (!string.IsNullOrWhiteSpace(ParentIdentifier))) t = "WMS-lag (Tjenestelag)";
            else if (HierarchyLevel == "service") t = "Tjeneste";
            else if (HierarchyLevel == "series") t = "Datasettserie";

            return t;
        }

        internal List<SimpleKeyword> CreateKeywords(List<string> inputList, string prefix, string type = null, string thesaurus = null)
        {
            List<SimpleKeyword> output = new List<SimpleKeyword>();
            if (inputList != null)
            {
                foreach (var keyword in inputList)
                {
                    output.Add(new SimpleKeyword
                    {
                        Keyword = keyword,
                        Thesaurus = thesaurus,
                        Type = type
                    });
                }
            }
            return output;
        }


        internal List<SimpleKeyword> GetAllKeywords()
        {
            List<SimpleKeyword> allKeywords = new List<SimpleKeyword>();
            allKeywords.AddRange(CreateKeywords(KeywordsNationalInitiative, "NationalInitiative", null, SimpleKeyword.THESAURUS_NATIONAL_INITIATIVE));
            allKeywords.AddRange(CreateKeywords(KeywordsNationalTheme, "NationalTheme", null, SimpleKeyword.THESAURUS_NATIONAL_THEME));
            allKeywords.AddRange(CreateKeywords(KeywordsPlace, "Place", SimpleKeyword.TYPE_PLACE, null));
            return allKeywords;
        }

    }

}