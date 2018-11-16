using Api.Models.Plus;
using Api.Models.PLUS.MaterialDetails;
using Api.Models.CrossReference;
using Api.Models.StressStrain;
using ElsevierMaterials.Models;
using ElsevierMaterials.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using Api.Models;
using System.Net.Http;

namespace ElsevierMaterials.Services
{
    public class TMPlusService : IPlusService
    {
        public Api.Models.PLUS.MaterialDetails.Condition MechanicalConditionProperties(int materialId, int subgroupId, int conditionId)
        {

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            using (WebClient client = new WebClient())
            {
                // New code
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MechanicalPropertiesPlus/ConditionProperties?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MechanicalPropertiesPlus/ConditionProperties?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                Api.Models.PLUS.MaterialDetails.Condition mechanical = Deserialize<Api.Models.PLUS.MaterialDetails.Condition>(json, responsebody);

                return mechanical;
            }

        }
        public CrossReference GetCrossReference(int materialId)
        {

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            using (WebClient client = new WebClient())
            {
                // New code
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CrossReferencePLUS/Materials?SessionId=" + sessionId + "&MaterialId=" + materialId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CrossReferencePLUS/Materials?SessionId=" + sessionId + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                CrossReference cr = Deserialize<CrossReference>(json, responsebody);

                return cr;
            }

        }
        public PropertiesContainer GetManufactoringProcesses(int materialId, int subgroupId)
        {

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            using (WebClient client = new WebClient())
            {
                // New code
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/ManufacturingPLUS/Properties?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;
         
                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/ManufacturingPLUS/Properties?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                PropertiesContainer tab = Deserialize<PropertiesContainer>(json, responsebody);
                foreach (var item in tab.Model)
                {
                   
                }
                return tab;
            }

        }

        public IList<string> GetAllReferencesForSS(string sessionId, int materialId) {

            using (WebClient client = new WebClient()) {
                // New code
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/AllReferences?SessionId=" + sessionId + "&MaterialId=" + materialId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/AllReferences?SessionId=" + sessionId + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<string> allReferences = Deserialize<IList<string>>(json, responsebody);

                return allReferences;
            }

        }

        public IList<string> GetReferencesForSelectedConditionForSS(string sessionId, int materialId, int conditionId) {
            if (conditionId == 0) return null;
            using (WebClient client = new WebClient()) {
                // New code
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/SelectedReferences?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/SelectedReferences?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<string> selectedReferences = Deserialize<IList<string>>(json, responsebody);

                return selectedReferences;
            }
        }

        public StressStrainModel GetStressStrainFromPLUSService(int materialId, int subgroupId)
        {

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            using (WebClient client = new WebClient())
            {
                // New code
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/Conditions?SessionId=" + sessionId + "&MaterialId=" + materialId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/Conditions?SessionId=" + sessionId + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                StressStrainModel result = new StressStrainModel() { TestConditions = new List<StressStrainConditionModel>() };
                IList<StressStrainCondition> root = Deserialize<List<StressStrainCondition>>(json, responsebody);
                foreach (var item in root)
                {
                    StressStrainConditionModel ssItem = new StressStrainConditionModel()
                    {
                        comment = item.comment,
                        Condition = item.Condition,
                        engineering = item.engineering,
                        FormId = item.FormId,
                        HeatTreatmentId = item.HeatTreatmentId,
                        LinearCoefComment = item.LinearCoefComment,
                        No = item.No,
                        sr = item.sr,
                        test_temperature = item.test_temperature,
                        TestingType = item.TestingType
                    };
                    ssItem.SelectedReferences = GetReferencesForSelectedConditionForSS(sessionId, materialId, ssItem.No);
                    //ssItem.StressTemperatures = GetStressStrainTemperatures(sessionId, materialId, subgroupId, item.No);
                    result.TestConditions.Add(ssItem);
                }
                if (result.TestConditions.Count > 0)
                {
                    result.TestConditions[0].StressTemperatures = GetStressStrainTemperaturesPLUS(sessionId, materialId, subgroupId, result.TestConditions[0].No,2);
                  
                }

                result.AllReferences = GetAllReferencesForSS(sessionId, materialId);
                return result;
            }

        }


        public IList<StressStrainTemperature> GetStressStrainTemperaturesPLUS(string sessionId, int materialId, int subgroupId, int conditionId, int ssType)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/TemperaturesForCondition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId + "&StressStrainCategory=" + ssType;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/TemperaturesForCondition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId + "&StressStrainCategory=" + ssType;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                IList<StressStrainTemperature> result = new List<StressStrainTemperature>();

                IList<TemperatureResult> temperatures = Deserialize<List<TemperatureResult>>(json, responsebody);
                foreach (var item in temperatures)
                {
                    StressStrainTemperature ssT = new StressStrainTemperature()
                    {
                        LegendDesc = item.LegendDesc,
                        LegendDescText = item.LegendDescText,
                        RT = item.RT,
                        Temperature = item.Temperature,
                        TemperatureText = item.Temperature + "°C"
                    };

                    result.Add(ssT);
                }
                if (result.Count > 0)
                {
                  //  result[0].TrueDetails = GetStressStrainPLUSDetails(sessionId, materialId, subgroupId, conditionId, result[0].Temperature, 1);
                    result[0].EngineeringDetails = GetStressStrainPLUSDetails(sessionId, materialId, subgroupId, conditionId, result[0].Temperature, 2);
                }
                return result;
            }
        }

        public StressStrainDetails GetStressStrainPLUSDetails(string sessionId, int materialId, int subgroupId, int conditionId, double temperature, int ssType)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/PointsForTemperature?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&StressStrainCategory=" + ssType;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/PointsForTemperature?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&StressStrainCategory=" + ssType;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();


                IList<DiagramCoordinate> temperatures = Deserialize<IList<DiagramCoordinate>>(json, responsebody);
                StressStrainDetails result = new StressStrainDetails() { Points = temperatures };
                result.Diagram = GetStressStrainPLUSDiagram(sessionId, materialId, subgroupId, conditionId, temperature, ssType);
                return result;
            }
        }

        public ImageSource GetStressStrainPLUSDiagram(string sessionId, int materialId, int subgroupId, int conditionId, double temperature, int ssType)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/Diagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&StressStrainCategory=" + ssType;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "image/png");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrainPLUS/Diagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&StressStrainCategory=" + ssType;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "image/png");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    //responsebody = Encoding.UTF8.GetString(responsebytes);
                }

                string mimeType = "image/png";
                string base64 = Convert.ToBase64String(responsebytes);

                ImageSource img = new ImageSource();
                img.Source = string.Format("data:{0};base64,{1}", mimeType, base64);

                return img;
            }
        }

        public IList<Api.Models.FatiguePLUS.Condition> GetFatigueConditionsFromPLUSService(string sessionId, int materialId)
        {
            
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/FatiguePLUS/Conditions?SessionId=" + sessionId + "&MaterialId=" + materialId;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                int counter = 0;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/FatiguePLUS/Conditions?SessionId=" + sessionId + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.FatiguePLUS.Condition> listFatigue = Deserialize<IList<Api.Models.FatiguePLUS.Condition>>(json, responsebody);
                return listFatigue;
            }
        }

        public Api.Models.FatiguePLUS.ConditionDetails GetFatigueConditionDetailsFromPLUSService(string sessionId, int materialId, string condition)
        {
            
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/FatiguePLUS/ConditionDetails?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

             

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/FatiguePLUS/ConditionDetails?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                Api.Models.FatiguePLUS.ConditionDetails details = Deserialize<Api.Models.FatiguePLUS.ConditionDetails>(json, responsebody);
                return details;
            }
        }

        public ImageSource GetFatigueDiagramFromPLUSService(string sessionId, int materialId, string condition)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/FatiguePLUS/Diagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "image/png");
                client.Encoding = System.Text.Encoding.UTF8;



                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/FatiguePLUS/Diagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "image/png");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                string mimeType = "image/png";
                string base64 = Convert.ToBase64String(responsebytes);

                ImageSource img = new ImageSource();
                img.Source = string.Format("data:{0};base64,{1}", mimeType, base64);

                return img;
            }
        }

        public IList<Api.Models.PLUS.MultiPointData.DiagramType> GetMPDiagramTypesFromPLUSService(string sessionId, int materialId, int subgroupId)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MultiPointData/DiagramTypes?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;



                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MultiPointData/DiagramTypes?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.PLUS.MultiPointData.DiagramType> details = Deserialize<IList<Api.Models.PLUS.MultiPointData.DiagramType>>(json, responsebody);
                return details;
            }
        }

        public IList<Api.Models.PLUS.MultiPointData.Condition> GetMPConditionsForDiagramTypeFromPLUSService(string sessionId, int materialId, int subgroupId, int typeDiagram)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MultiPointData/ConditionsForDiagramType?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&DiagramTypeId=" + typeDiagram;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;



                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MultiPointData/ConditionsForDiagramType?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&DiagramTypeId=" + typeDiagram;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                
                IList<Api.Models.Mechanical.MechanicalCondition> mechCond = Deserialize<IList<Api.Models.Mechanical.MechanicalCondition>>(json, responsebody);
                IList<Api.Models.PLUS.MultiPointData.Condition> details = mechCond.Select(r => new Api.Models.PLUS.MultiPointData.Condition() { Name = r.Condition != null ? r.Condition : "", Id = r.no, Comment="" }).ToList();
                return details;
            }
        }

        public IList<Api.Models.PLUS.MultiPointData.DiagramLegend> GetMPLegendsForConditionFromPLUSService(string sessionId, int materialId, int subgroupId, int condition, int typeDiagram)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MultiPointData/DiagramLegendsForCondition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + condition + "&DiagramTypeId=" + typeDiagram;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;



                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MultiPointData/DiagramLegendsForCondition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + condition + "&DiagramTypeId=" + typeDiagram;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.PLUS.MultiPointData.DiagramLegend> details = Deserialize<IList<Api.Models.PLUS.MultiPointData.DiagramLegend>>(json, responsebody);
                return details;
            }
        }

        public Api.Models.PLUS.MultiPointData.TablePoints GetMPTablePointsFromPLUSService(string sessionId, int materialId, int subgroupId, int condition, int typeDiagram, int legendId)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MultiPointData/TablePoints?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + condition + "&DiagramTypeId=" + typeDiagram + "&LegendId=" + legendId;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;



                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MultiPointData/TablePoints?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + condition + "&DiagramTypeId=" + typeDiagram + "&LegendId=" + legendId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                Api.Models.PLUS.MultiPointData.TablePoints details = Deserialize<Api.Models.PLUS.MultiPointData.TablePoints>(json, responsebody);
                return details;
            }
        }

        public ImageSource GetMultipointDataDiagramFromPlusService(string sessionId, int materialId, int subgroupId, int conditionId, int typeDiagram) 
        {

            using (WebClient client = new WebClient())
            {
                // New code:

                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MultiPointData/Diagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId + "&DiagramTypeId=" + typeDiagram;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "image/png");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MultiPointData/Diagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId + "&DiagramTypeId=" + typeDiagram;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "image/png");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    //responsebody = Encoding.UTF8.GetString(responsebytes);
                }

                string mimeType = "image/png";
                string base64 = Convert.ToBase64String(responsebytes);

                ImageSource img = new ImageSource();
                img.Source = string.Format("data:{0};base64,{1}", mimeType, base64);

                return img;
            }
        }


        public Api.Models.CreepDataPLUS.Data GetCreepDataFromService(string sessionId, int materialId)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/Data?SessionId=" + sessionId + "&MaterialId=" + materialId;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepData/Conditions?SessionId=" + sessionId + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                Api.Models.CreepDataPLUS.Data data = Deserialize<Api.Models.CreepDataPLUS.Data>(json, responsebody);
                return data;
            }
        }

        public IList<Api.Models.CreepDataPLUS.CreepCondition> GetCreepDiagramConditions(string sessionId, int materialId) {
            using (WebClient client = new WebClient()) {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/DiagramsConditions?SessionId=" + sessionId + "&MaterialId=" + materialId;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/DiagramsConditions?SessionId=" + sessionId + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.CreepDataPLUS.CreepCondition> conditions = Deserialize<IList<Api.Models.CreepDataPLUS.CreepCondition>>(json, responsebody);
                return conditions;
            }
        }

        public IList<TemperatureItem> GetCreepConditionTemperatures(string sessionId, int materialId, int conditionId) {
            using (WebClient client = new WebClient()) {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/ConditionTemperatures?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/ConditionTemperatures?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.CreepDataPLUS.Temperature> temperatures = Deserialize<IList<Api.Models.CreepDataPLUS.Temperature>>(json, responsebody);
                IList<TemperatureItem> retVal = new List<TemperatureItem>();
                foreach (var item in temperatures) {
                    retVal.Add(new TemperatureItem { Value = item.TemperatureValue, Text = item.TemperatureValue + "°C" });
                }
                return retVal;
            }
        }

        public IList<TemperatureItem> GetCreepConditionTemperaturesIso(string sessionId, int materialId, int conditionId) {
            using (WebClient client = new WebClient()) {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/ConditionTemperaturesIso?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/ConditionTemperaturesIso?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.CreepDataPLUS.TemperatureIso> temperatures = Deserialize<IList<Api.Models.CreepDataPLUS.TemperatureIso>>(json, responsebody);
                IList<TemperatureItem> retVal = new List<TemperatureItem>();
                foreach (var item in temperatures) {
                    retVal.Add(new TemperatureItem { Value = item.Temperature, Text = item.Temperature + "°C"});
                }
                return retVal;
            }
        }

        public IList<Api.Models.CreepDataPLUS.Time> GetCreepTimesIso(string sessionId, int materialId, int conditionId, short temperature) {
            using (WebClient client = new WebClient()) {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/TimesIso?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/TimesIso?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.CreepDataPLUS.Time> times = Deserialize<IList<Api.Models.CreepDataPLUS.Time>>(json, responsebody);
                return times;
            }
        }

        public IList<Api.Models.CreepDataPLUS.StressPoint> GetCreepStresses(string sessionId, int materialId, int conditionId, short temperature) {
            using (WebClient client = new WebClient()) {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/Stresses?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/Stresses?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.CreepDataPLUS.StressPoint> stresses = Deserialize<IList<Api.Models.CreepDataPLUS.StressPoint>>(json, responsebody);
                return stresses;
            }
        }

        public IList<Api.Models.CreepDataPLUS.StressPointIso> GetCreepStressPointsIso(string sessionId, int materialId, int conditionId, short temperature, string unit, double value) {
            using (WebClient client = new WebClient()) {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/StressPointsIso?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&Time=" + value + "&Unit=" + unit;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/StressPointsIso?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&Time=" + value + "&Unit=" + unit;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.CreepDataPLUS.StressPointIso> points = Deserialize<IList<Api.Models.CreepDataPLUS.StressPointIso>>(json, responsebody);
                return points;
            }
        }

        public IList<Api.Models.CreepDataPLUS.StressPointIso> GetCreepStressPoints(string sessionId, int materialId, int conditionId, short temperature, double value) {
            using (WebClient client = new WebClient()) {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/StressPoints?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&Stress=" + value;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/StressPoints?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&Stress=" + value;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.CreepDataPLUS.StressPointIso> points = Deserialize<IList<Api.Models.CreepDataPLUS.StressPointIso>>(json, responsebody);
                return points;
            }
        }

        public ImageSource GetCreepDiagramIso(string sessionId, int materialId, int conditionId, short temperature) {

            using (WebClient client = new WebClient()) {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/DiagramIso?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "image/png");
                client.Encoding = System.Text.Encoding.UTF8;



                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/DiagramIso?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "image/png");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                string mimeType = "image/png";
                string base64 = Convert.ToBase64String(responsebytes);

                ImageSource img = new ImageSource();
                img.Source = string.Format("data:{0};base64,{1}", mimeType, base64);

                return img;
            }
        }

        public ImageSource GetCreepDiagram(string sessionId, int materialId, int conditionId, short temperature) {

            using (WebClient client = new WebClient()) {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/Diagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature;

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "image/png");
                client.Encoding = System.Text.Encoding.UTF8;



                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>") {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepDataPlus/Diagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "image/png");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                string mimeType = "image/png";
                string base64 = Convert.ToBase64String(responsebytes);

                ImageSource img = new ImageSource();
                img.Source = string.Format("data:{0};base64,{1}", mimeType, base64);

                return img;
            }
        }

        public PropertiesContainer GetPhysicalPropertiesPLUSFromService(string sessionId, int materialId)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/PhysicalPropertiesPLUS/PhysicalProperties?SessionId=" + sessionId + "&MaterialId=" + materialId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/PhysicalPropertiesPLUS/PhysicalProperties?SessionId=" + ssid + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                PropertiesContainer physical = Deserialize<Api.Models.Plus.PropertiesContainer>(json, responsebody);


                return physical;
            }

        }

        public ICollection<Material> GetMaterialSubgroupPLUSListFromService(string sessionId, int materialId, int sourceMaterialId)
        {
            using (WebClient client = new WebClient())
            {
                // New code
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/SearchPLUS/SubgroupList?SessionId=" + sessionId + "&MaterialId=" + sourceMaterialId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/SearchPLUS/SubgroupList?SessionId=" + ssid + "&MaterialId=" + sourceMaterialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<SubgroupPLUS> subgroup = Deserialize<List<SubgroupPLUS>>(json, responsebody);
                ICollection<Material> materialList = new List<Material>();
                foreach (var it in subgroup)
                {
                    Material m = new Material() { MaterialId = materialId, SourceId = 3, SourceMaterialId = sourceMaterialId, SubgroupId = it.key_num, Standard = it.standard, Specification = it.subgroup_prod, SourceText = "Total Materia PolyPLUS", NumEquivalency = (int)(it.Equivalency != null ? it.Equivalency : 0), NumProperties = (int)(it.Properties != null ? it.Properties : 0), NumProcessing = (int)(it.Processing != null ? it.Processing : 0) };
                    m.Manufacturer = "-";
                    if (m.Standard == "PROPRIETARY")
                    {
                        m.Manufacturer = m.Specification;
                        m.Standard = "-";
                        m.Specification = "PROPRIETARY";
                    }
                    materialList.Add(m);
                }
                return materialList;
            }

        }

        public PropertiesContainer GetMechanicalPLUSPropertiesFromService(string sessionId, int materialId, int subgroupId)
        {
            using (WebClient client = new WebClient())
            {
                // New code
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MechanicalPropertiesPlus/ConditionsWithProperties?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MechanicalPropertiesPlus/ConditionsWithProperties?SessionId=" + ssid + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                Api.Models.Plus.PropertiesContainer mechanical = Deserialize<Api.Models.Plus.PropertiesContainer>(json, responsebody);

                return mechanical;
            }

        }

        public IList<int> GetMaterialIdsForAdvSearchPropertiesFromServicePLUS(string sessionId, AdvSearchFiltersAll filters)
        {
            if (
                filters == null
                )
            {
                return new List<int>();
            }

            using (WebClient client = new WebClient())
            {

                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/tm/AdvancedSearch/MaterialsPLUS?SessionId=" + sessionId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/jsonBasic");
                client.Encoding = System.Text.Encoding.UTF8;

                string jsonData = JsonConvert.SerializeObject(filters);

                byte[] data = Encoding.UTF8.GetBytes(jsonData);


                byte[] responsebytes = client.UploadData(new Uri(url), "POST", data);
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;

                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/tm/AdvancedSearch/MaterialsPLUS?SessionId=" + sessionId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    data = Encoding.UTF8.GetBytes(jsonData);


                    responsebytes = client.UploadData(new Uri(url), "POST", data);
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }


                var json = new JsonMediaTypeFormatter();

                return Deserialize<IList<int>>(json, responsebody); ;

            }
        }

        public ICollection<Material> GetPLUSMaterialsSubgroupListFromService(string sessionId, IList<int> materials)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                //TODO: altair mora da se zameni sa els
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/SearchPLUS/MaterialsSubgroupList?SessionId=" + sessionId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/json");
                client.Encoding = System.Text.Encoding.UTF8;
                string apiUsername = WebConfigurationManager.AppSettings["apiUsername"];
                string apiPassword = WebConfigurationManager.AppSettings["apiPassword"];

                string jsonData = Serialize(materials);
                byte[] data = Encoding.UTF8.GetBytes(jsonData);




                byte[] responsebytes = client.UploadData(new Uri(url), "POST", data);
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Search/MaterialsSubgroupList?SessionId=" + sessionId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.UploadData(new Uri(url), "POST", data);
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                IList<SubgroupPLUS> subgroupsMaterials = Deserialize<IList<SubgroupPLUS>>(json, responsebody);


                ICollection<Material> materialsColection = new HashSet<Material>();
                foreach (var it in subgroupsMaterials)
                {
                    //TODO: MaterialId = materialId je potreban!!
                    Material m = new Material() { SourceId = 3, SourceMaterialId = it.vk_key, SubgroupId = it.key_num, Standard = it.standard, Specification = it.subgroup_prod, SourceText = "Total Materia PolyPLUS", NumEquivalency = (int)(it.Equivalency != null ? it.Equivalency : 0), NumProperties = (int)(it.Properties != null ? it.Properties : 0), NumProcessing = (int)(it.Processing != null ? it.Processing : 0) };
                    m.Manufacturer = "-";
                    if (m.Standard == "PROPRIETARY")
                    {
                        m.Manufacturer = m.Specification;
                        m.Standard = "-";
                        m.Specification = "PROPRIETARY";
                    }

                    materialsColection.Add(m);
                }

                return materialsColection;


            }
        }
        public string GetSessionFromService()
        {
            using (WebClient client = new WebClient())
            {
                // New code:

                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/login/auth";
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/json");
                client.Encoding = System.Text.Encoding.UTF8;
                string apiUsername = WebConfigurationManager.AppSettings["apiUsername"];
                string apiPassword = WebConfigurationManager.AppSettings["apiPassword"];
                string jsonData = "{\"username\":\"" + apiUsername + "\",\"password\":\"" + apiPassword + "\"}";

                byte[] data = Encoding.UTF8.GetBytes(jsonData);


                byte[] responsebytes = client.UploadData(new Uri(url), "POST", data);
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                IDictionary<string, object> returnValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(responsebody);
                string serviceResponse = "";
                if (returnValue.ContainsKey("Result"))
                {
                    IDictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(returnValue["Result"].ToString());
                    serviceResponse = result["@SessionId"];
                }
                else
                {
                    serviceResponse = "Not found";
                }
                return serviceResponse;
            }
        }
        
        T Deserialize<T>(MediaTypeFormatter formatter, string str) where T : class
        {
            // Write the serialized string to a memory stream.
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            // Deserialize to an object of type T
            return formatter.ReadFromStreamAsync(typeof(T), stream, null, null).Result as T;
        }

        public string Serialize<T>(T propertyContainer)
        {
            var formatter = new JsonMediaTypeFormatter();
            // Create a dummy HTTP Content.
            Stream stream = new MemoryStream();
            var content = new StreamContent(stream);
            /// Serialize the object.
            formatter.WriteToStreamAsync(typeof(T), propertyContainer, stream, content, null).Wait();
            // Read the serialized string.
            stream.Position = 0;
            return content.ReadAsStringAsync().Result;

        }

    }
}
