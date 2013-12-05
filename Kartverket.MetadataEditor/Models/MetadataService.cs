﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using www.opengis.net;
using GeoNorgeAPI;

namespace Kartverket.MetadataEditor.Models
{
    public class MetadataService
    {
        private GeoNorge _geoNorge;

        public MetadataService()
        {
            _geoNorge = new GeoNorge();
        }

        public List<MetadataItemViewModel> GetMyMetadata(string organizationName)
        {
            SearchResultsType results = _geoNorge.SearchWithOrganisationName(organizationName);

            var metadata = new List<MetadataItemViewModel>();

            if (results.Items != null)
            {
                foreach (var item in results.Items)
                {
                    RecordType record = (RecordType)item;

                    string title = null;
                    string uuid = null;
                    string organization = null;

                    for (int i = 0; i < record.ItemsElementName.Length; i++)
                    {
                        var name = record.ItemsElementName[i];
                        var value = record.Items[i].Text != null ? record.Items[i].Text[0] : null;

                        if (name == ItemsChoiceType24.title)
                            title = value;
                        else if (name == ItemsChoiceType24.identifier)
                            uuid = value;
                        else if (name == ItemsChoiceType24.creator)
                            organization = value;
                    }
                    metadata.Add(new MetadataItemViewModel { Title = title, Uuid = uuid, Organization = organization });
                }
            }

            return metadata;
        }


        public MetadataViewModel GetMetadataModel(string uuid)
        {
            SimpleMetadata metadata = _geoNorge.GetRecordByUuidSimple(uuid);

            return new MetadataViewModel() { 
                Uuid = metadata.Uuid,
                Title = metadata.Title,
                HierarchyLevel = metadata.HierarchyLevel,
                Abstract = metadata.Abstract,
                Purpose = metadata.Purpose,
                SupplementalDescription = metadata.SupplementalDescription
            };

        }

        public void SaveMetadataModel(MetadataViewModel model)
        {
            SimpleMetadata metadata = _geoNorge.GetRecordByUuidSimple(model.Uuid);

            metadata.Title = model.Title;
            metadata.Abstract = model.Abstract;
            metadata.Purpose = model.Purpose;
            metadata.SupplementalDescription = model.SupplementalDescription;

            _geoNorge.MetadataUpdate(metadata.GetMetadata());
        }

    }
}