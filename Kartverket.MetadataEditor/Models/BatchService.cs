﻿using GeoNorgeAPI;
using Kartverket.MetadataEditor.Controllers;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartverket.MetadataEditor.Models
{
    public class BatchService
    {
        private MetadataService _metadataService;
        private static readonly ILog Log = LogManager.GetLogger(typeof(MvcApplication));

        public BatchService() 
        {
            _metadataService = new MetadataService();
        }

        public void Update(BatchData data, string username)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(data.dataField) && !string.IsNullOrWhiteSpace(data.dataValue)) 
                {

                    foreach (var md in data.MetaData)
                    {
                        bool metadataUpdated = false;

                        MetadataViewModel metadata = _metadataService.GetMetadataModel(md.Uuid);

                        //Metadatakontakt
                        if (data.dataField == "ContactMetadata_Organization")
                        {
                            metadata.ContactMetadata.Organization = data.dataValue;
                            metadataUpdated = true;
                        }
                        else if (data.dataField == "ContactMetadata_Name")
                        {
                            metadata.ContactMetadata.Name = data.dataValue;
                            metadataUpdated = true;
                        }
                        else if (data.dataField == "ContactMetadata_Email")
                        {
                            metadata.ContactMetadata.Email = data.dataValue;
                            metadataUpdated = true;
                        }
                        //Teknisk kontakt
                        else if (data.dataField == "ContactPublisher_Organization")
                        {
                            metadata.ContactPublisher.Organization = data.dataValue;
                            metadataUpdated = true;
                        }
                        else if (data.dataField == "ContactPublisher_Name")
                        {
                            metadata.ContactPublisher.Name = data.dataValue;
                            metadataUpdated = true;
                        }
                        else if (data.dataField == "ContactPublisher_Email")
                        {
                            metadata.ContactPublisher.Email = data.dataValue;
                            metadataUpdated = true;
                        }
                        //Faglig kontakt
                        else if (data.dataField == "ContactOwner_Organization")
                        {
                            metadata.ContactOwner.Organization = data.dataValue;
                            metadataUpdated = true;
                        }
                        else if (data.dataField == "ContactOwner_Name")
                        {
                            metadata.ContactOwner.Name = data.dataValue;
                            metadataUpdated = true;
                        }
                        else if (data.dataField == "ContactOwner_Email")
                        {
                            metadata.ContactOwner.Email = data.dataValue;
                            metadataUpdated = true;
                        }
                        //Oppdateringshyppighet
                        else if (data.dataField == "MaintenanceFrequency")
                        {
                            metadata.MaintenanceFrequency = data.dataValue;
                            metadataUpdated = true;
                        }

                        if (metadataUpdated) 
                        { 
                        _metadataService.SaveMetadataModel(metadata, username);
                        Log.Info("Batch update uuid: " + metadata.Uuid + ", " + data.dataField + ": " + data.dataValue);
                        }
                    }

                }
              
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

        }

        public void UpdateAll(BatchData data, string username, string organization)
        {
            try
            {
                MetadataIndexViewModel model = new MetadataIndexViewModel();
                int offset = 1;
                int limit = 50;
                model = _metadataService.SearchMetadata(organization, "", offset, limit);
                model.UserOrganization = organization;

                foreach (var item in model.MetadataItems)
                {
                    data.MetaData.Add(new MetaDataEntry { Uuid = item.Uuid });
                }

                Update(data, username);

                int numberOfRecordsMatched = model.TotalNumberOfRecords;
                int next = model.OffsetNext();

                while (next < numberOfRecordsMatched)
                {
                    data.MetaData = null; data.MetaData = new List<MetaDataEntry>();

                    model = _metadataService.SearchMetadata(organization, "", next, limit);
                    model.UserOrganization = organization;

                    foreach (var item in model.MetadataItems)
                    {
                        data.MetaData.Add(new MetaDataEntry { Uuid = item.Uuid });
                    }

                    Update(data, username);

                    next = model.OffsetNext();
                    if (next == 0) break;
                }

 
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }

    public class BatchData
    {
        public BatchData()
        {
            MetaData = new List<MetaDataEntry>();
        }
        public string dataField { get; set; }
        public string dataValue { get; set; }
        public List<MetaDataEntry> MetaData { get; set; }
    }
}