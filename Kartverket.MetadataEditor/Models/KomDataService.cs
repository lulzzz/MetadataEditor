﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kartverket.MetadataEditor.no.geonorge.ws;

namespace Kartverket.MetadataEditor.Models
{
    public class KomDataService
    {
        SokKomDataClient result;

        System.Collections.Specialized.NameValueCollection settings = System.Web.Configuration.WebConfigurationManager.AppSettings;
        string username;
        string password;

        Dictionary<string, string> fylker;

        public KomDataService()
        {
            username = settings["WebserviceGeonorgeUsername"];
            password = settings["WebserviceGeonorgePassword"];

            fylker = GetFylkesList();

            result = new SokKomDataClient();
        }

        public List<string> GetPlaces(double nordMin, double austMin, double nordMax, double austMax,int koordSysUt, int koordSysInn)
        {
            var kommuner = result.kommuneSok1(username, password, null, 0, "", koordSysUt, koordSysInn, nordMin, austMin, nordMax, austMax);
            List<string> places = new List<string>();

            foreach (var kommune in kommuner.resultat)
            {
                var kommunenavn = kommune.kNavn;
                var kommunenr = kommune.kommNr;

                var fylke = GetFylke(kommunenr);

                if (!string.IsNullOrEmpty(fylke) && !places.Contains(fylke))
                    places.Add(fylke);

                if (!places.Contains(kommunenavn))
                    places.Add(kommunenavn);

            }

            return places;

        }

        private string GetFylke(int kommunenr)
        {
            string fylke = kommunenr.ToString("0000");
            fylke = fylke.Substring(0, 2);

            string fylkesnavn = "";

            foreach (var code in fylker)
            {
                var codevalue = code.Key;
                if (codevalue == fylke)
                {
                    fylkesnavn = code.Value;
                    break;
                }
            }

            return fylkesnavn;
        }

        Dictionary<string,string> GetFylkesList()
        {
            string url = System.Web.Configuration.WebConfigurationManager.AppSettings["RegistryUrl"] + "api/subregister/sosi-kodelister/kartverket/fylkesnummer";
            System.Net.WebClient c = new System.Net.WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            var data = c.DownloadString(url);
            var response = Newtonsoft.Json.Linq.JObject.Parse(data);

            var codeList = response["containeditems"];

            Dictionary<string, string> fylks = new Dictionary<string, string>();

            foreach (var code in codeList)
            {
                if(!fylks.ContainsKey(code["codevalue"].ToString()))
                    fylks.Add(code["codevalue"].ToString(), code["label"].ToString());               
            }

            return fylks;
        }

    }
   
}