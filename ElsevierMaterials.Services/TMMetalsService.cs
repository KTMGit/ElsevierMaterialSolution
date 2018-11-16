using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using ElsevierMaterials.Models;
using ElsevierMaterials.Common.Interfaces;
using System.Net.Http.Formatting;
using System.IO;
using Api.Models;
using Api.Models.Plus;
using Api.Models.Metallography;
using Api.Models.Machinability;
using Api.Models.HeatTreatment;
using System.Web.Configuration;
using Api.Models.ChemicalComposition;
using Api.Models.StressStrain;
using System.Drawing;
using Api.Models.Mechanical;
using Api.Models.SubgroupList;
using ElsevierMaterials.Models.Domain.AdvancedSearch;

namespace ElsevierMaterials.Services
{
    public class TotalMateriaService : IService
    {
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

        public string GetSessionFromService()
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                //TODO: altair mora da se zameni sa els
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

        public string getMechanicalPropName(MechanicalPropertiesTMEnum id)
        {
            switch (id)
            {
                case MechanicalPropertiesTMEnum.YieldStress:
                    
                    return "yield stress";

                case MechanicalPropertiesTMEnum.TensileStress:
                    return "tensile stress";

                case MechanicalPropertiesTMEnum.Elongation:
                    return "elongation";

                case MechanicalPropertiesTMEnum.Impact:
                    return "impact";

                case MechanicalPropertiesTMEnum.Hardness:
                    return "hardness";

                case MechanicalPropertiesTMEnum.ReductionOfArea:
                    return "reduction of area";
                default:
                    return "";
            }
        }

        public class MaterialDetailsLngFilters
        {
            public int MaterialId { get; set; }
            public string LanguageId { get; set; }

        }
           
        public ICollection<Material> GetMetalsMaterialsSubgroupListFromService(string sessionId, IList<int> materials)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                //TODO: altair mora da se zameni sa els
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Search/MaterialsSubgroupList?SessionId=" + sessionId;
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
                IList<Subgroup> subgroupsMaterials = Deserialize<IList<Subgroup>>(json, responsebody);
                ICollection<Material> materialsColection = new HashSet<Material>();
                foreach (var subgroup in subgroupsMaterials)
                {
                    Material m = new Material();
                    m.SourceMaterialId = subgroup.MaterialId;
                    //TODO: Da li mi je neophodna ova informacija
                    //   m.MaterialId = materialId;
                    if (subgroup.SubgroupId == null)
                    {
                        m.Specification = subgroup.SubgroupDesc;
                    }
                    else
                    {
                        m.Specification = subgroup.SubgroupId;
                    }

                    m.Specification = m.Specification + " " + (!string.IsNullOrEmpty(subgroup.SubgroupYear) ? " (" + subgroup.SubgroupYear + ")<br />" : "").ToString();
                    if (subgroup.SubgroupDesc != null)
                    {
                        if (m.Specification.Trim()!=subgroup.SubgroupDesc.Trim())
                        {
                            m.Specification = m.Specification + " " + subgroup.SubgroupDesc;
                        }
                        
                    }

                    if (string.IsNullOrEmpty(m.Specification))
                    {
                        m.Specification = "-";
                    }

                    m.SourceText = "Total Materia Total Metals";
                    m.Standard = subgroup.Standard;
                    if (string.IsNullOrEmpty(m.Standard))
                    {
                        m.Standard = "-";
                    }
                    m.SourceId = 2;


                    int y = 0;
                    if (int.TryParse(subgroup.SubgroupId, out y))
                    {
                        m.SubgroupId = y;
                    }

                    m.Manufacturer = "-";

                    if (m.Standard == "PROPRIETARY")
                    {
                        m.Standard = "-";
                        m.Manufacturer = m.Specification;
                        m.Specification = "PROPRIETARY";
                    }
                    m.SubgroupId = subgroup.KeyNum;
                    m.NumProperties = subgroup.Properties != null ? (int)subgroup.Properties : 0;
                    m.NumEquivalency = subgroup.Equivalency != null ? (int)subgroup.Equivalency : 0;
                    m.NumProcessing = subgroup.Processing != null ? (int)subgroup.Processing : 0;
                    materialsColection.Add(m);
                }

                return materialsColection;


            }
        }

        public ICollection<Api.Models.MaterialCounters> GetMetalsMaterialsCountersFromService(string sessionId, int materialId)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/search/CountersForMaterial?SessionId=" + sessionId + "&MaterialId=" + materialId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/search/CountersForMaterial?SessionId=" + sessionId + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                IList<Api.Models.MaterialCounters> counters = Deserialize<IList<Api.Models.MaterialCounters>>(json, responsebody);               
             

                return counters;
            }

        }

        public ICollection<Material> GetMaterialSubgroupListFromService(string sessionId, int materialId, int sourceMaterialId)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/search/SubgroupList?SessionId=" + sessionId + "&MaterialId=" + sourceMaterialId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/search/SubgroupList?SessionId=" + sessionId + "&MaterialId=" + sourceMaterialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                IList<Subgroup> subgroupsMaterials = Deserialize<IList<Subgroup>>(json, responsebody);
                ICollection<Material> materialsColection = new HashSet<Material>();
                foreach (var subgroup in subgroupsMaterials)
                {
                    Material m = new Material();
                    m.SourceMaterialId = sourceMaterialId;
                    m.MaterialId = materialId;
                    //TODO: Da li mi je neophodna ova informacija
                    //   m.MaterialId = materialId;
                    if (subgroup.SubgroupId == null)
                    {
                        m.Specification = subgroup.SubgroupDesc;
                    }
                    else
                    {
                        m.Specification = subgroup.SubgroupId;
                    }
                    //m.Specification = (!string.IsNullOrEmpty(itemResult["@Name"].ToString()) ? itemResult["@Name"].ToString() + " " : "") + (!string.IsNullOrEmpty(itemResult["@Year"].ToString()) ? "(" + itemResult["@Year"].ToString() + ")<br />" : "").ToString();

                    m.Specification = m.Specification + " " + (!string.IsNullOrEmpty(subgroup.SubgroupYear) ? " (" + subgroup.SubgroupYear + ")<br />" : "").ToString();
                    if (subgroup.SubgroupDesc != null)
                    {
                        if (m.Specification.Trim() != subgroup.SubgroupDesc.Trim())
                        {
                            m.Specification = m.Specification + " " + subgroup.SubgroupDesc;
                        }
                    }

                    if (string.IsNullOrEmpty(m.Specification))
                    {
                        m.Specification = "-";
                    }

                    m.SourceText = "Total Materia Total Metals";
                    m.Standard = subgroup.Standard;
                    if (string.IsNullOrEmpty(m.Standard))
                    {
                        m.Standard = "-";
                    }
                    m.SourceId = 2;


                    int y = 0;
                    if (int.TryParse(subgroup.SubgroupId, out y))
                    {
                        m.SubgroupId = y;
                    }

                    m.Manufacturer = "-";

                    if (m.Standard == "PROPRIETARY")
                    {
                        m.Standard = "-";
                        m.Manufacturer = m.Specification;
                        m.Specification = "PROPRIETARY";
                    }

                    m.SubgroupId = subgroup.KeyNum;
                    m.NumProperties = subgroup.Properties != null ? (int)subgroup.Properties : 0;
                    m.NumEquivalency = subgroup.Equivalency != null ? (int)subgroup.Equivalency : 0;
                    m.NumProcessing = subgroup.Processing != null ? (int)subgroup.Processing : 0;
                    materialsColection.Add(m);
                }

                return materialsColection;
            }


        }

        public ICollection<Condition> GetPhysicalPropertiesFromService(string sessionId, int materialId)
        {
            using (WebClient client = new WebClient())
            {
                // New code:

                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/Physical?SessionId=" + sessionId + "&MaterialId=" + materialId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/json");
                client.Encoding = System.Text.Encoding.UTF8;
                int counter = 0;

                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/Physical?SessionId=" + ssid + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                IDictionary<string, object> returnValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(responsebody);

                ICollection<Condition> conds = new List<Condition>();
                if (returnValue.ContainsKey("Result"))
                {
                    IDictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(returnValue["Result"].ToString());
                    if (result.ContainsKey("PhysicalProperties"))
                    {
                        result = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["PhysicalProperties"].ToString());

                        if (result.ContainsKey("Typical"))
                        {

                            IDictionary<string, object> resultTypical = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["Typical"].ToString());

                            if (resultTypical.ContainsKey("PhysicalProperties"))
                            {
                                dynamic dynObj = JsonConvert.DeserializeObject(resultTypical["PhysicalProperties"].ToString());
                                Condition c = new Condition();
                                c.ConditionId = counter;
                                c.ConditionName = "";
                                //c.ConditionTemperature = "";
                                c.Properties = new List<Property>();
                                if (dynObj.GetType().Name == "JObject")
                                {
                                    IDictionary<string, object> itemResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(dynObj.ToString());
                                    if (itemResult.ContainsKey("PhysicalProperty"))
                                    {
                                        dynamic ress = JsonConvert.DeserializeObject(itemResult["PhysicalProperty"].ToString());
                                        if (ress.GetType().Name == "JObject")
                                        {
                                            int count = 0;
                                            AddJSonObjectToTypicalPhysical(ref count, ress, ref c);
                                        }
                                        else
                                        {
                                            int count = 0;
                                            foreach (var itemd in ress)
                                            {
                                                AddJSonObjectToTypicalPhysical(ref count, itemd, ref c);
                                                count = count + 1;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    foreach (var item in dynObj)
                                    {
                                        IDictionary<string, object> itemResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(item.ToString());

                                        if (itemResult.ContainsKey("PhysicalProperty"))
                                        {

                                            dynamic ress = JsonConvert.DeserializeObject(itemResult["PhysicalProperty"].ToString());
                                            if (ress.GetType().Name == "JObject")
                                            {
                                                int count = 0;
                                                AddJSonObjectToTypicalPhysical(ref count, ress, ref c);


                                            }
                                            else
                                            {
                                                foreach (var itemd in ress)
                                                {
                                                    int count = 0;
                                                    AddJSonObjectToTypicalPhysical(ref count, itemd, ref c);
                                                    count = count + 1;
                                                }
                                            }
                                        }

                                    }
                                    c.Properties = c.Properties.OrderBy(m => m.PropertyName).ThenBy(m => m.Temperature).ToList();
                                    conds.Add(c);
                                    counter++;
                                }
                            }
                        }
                        if (result.ContainsKey("Direct"))
                        {
                            IDictionary<string, object> resultTypical = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["Direct"].ToString());

                            if (resultTypical.ContainsKey("Condition"))
                            {
                                dynamic dynObj = JsonConvert.DeserializeObject(resultTypical["Condition"].ToString());




                                if (dynObj.GetType().Name == "JObject")
                                {
                                    Condition c = new Condition();


                                    c.Properties = new List<Property>();
                                    //    c.ConditionId = item["@ConditionId"];
                                    c.ConditionId = counter;
                                    c.ConditionName = "Direct: " + dynObj["@Name"];
                                    IDictionary<string, object> itemResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(dynObj.ToString());
                                    if (itemResult.ContainsKey("Temperature"))
                                    {

                                        dynamic ress = JsonConvert.DeserializeObject(itemResult["Temperature"].ToString());



                                        if (ress.GetType().Name == "JObject")
                                        {


                                            double res;
                                            double? temperatureRes = null;
                                            if (double.TryParse(ress["@Value"].ToString(), out res))
                                            {
                                                temperatureRes = res;
                                            }
                                            IDictionary<string, object> ip = new Dictionary<string, object>();
                                            for (int i = 0; i < Enum.GetNames(typeof(PhisicalPropertiesTMEnum)).Length; i++)
                                            {

                                                ip = JsonConvert.DeserializeObject<Dictionary<string, object>>(ress[((PhisicalPropertiesTMEnum)i).ToString()].ToString());
                                                if (ip["@Value"].ToString() != "-")
                                                {

                                                    Property p = new Property()
                                                    {
                                                        SourcePropertyId = int.Parse(ip["@TypeId"].ToString()),
                                                        PropertyName = ip["@Name"].ToString().ToLower(),
                                                        OrigUnit = ip["@Unit"].ToString().Replace("(", "").Replace(")", ""),
                                                        OrigValue = ip["@Value"].ToString(),
                                                        Temperature = temperatureRes
                                                    };
                                                    if (p.OrigValue != "")
                                                    {
                                                        c.Properties.Add(p);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (var itemd in ress)
                                            {
                                                double res;
                                                double? temperatureRes = null;
                                                if (double.TryParse(itemd["@Value"].ToString(), out res))
                                                {
                                                    temperatureRes = res;
                                                }

                                                IDictionary<string, object> ip = new Dictionary<string, object>();
                                                for (int i = 0; i < Enum.GetNames(typeof(PhisicalPropertiesTMEnum)).Length; i++)
                                                {
                                                    ip = JsonConvert.DeserializeObject<Dictionary<string, object>>(itemd[((PhisicalPropertiesTMEnum)i).ToString()].ToString());
                                                    if (ip["@Value"].ToString() != "-")
                                                    {
                                                        Property p = new Property()
                                                        {
                                                            SourcePropertyId = int.Parse(ip["@TypeId"].ToString()),
                                                            PropertyName = ip["@Name"].ToString().ToLower(),
                                                            OrigUnit = ip["@Unit"].ToString().Replace("(", "").Replace(")", ""),
                                                            OrigValue = ip["@Value"].ToString(),
                                                            Temperature = temperatureRes
                                                        };
                                                        if (p.OrigValue != "")
                                                        {
                                                            c.Properties.Add(p);
                                                        }
                                                        //c.Properties.Add(p);
                                                    }

                                                }

                                            }
                                        }
                                    }
                                    c.Properties = c.Properties.OrderBy(m => m.PropertyName).ThenBy(m => m.Temperature).ToList();
                                    conds.Add(c);
                                    counter++;






                                }

                                else
                                {

                                    foreach (var item in dynObj)
                                    {
                                        Condition c = new Condition();


                                        c.Properties = new List<Property>();
                                        //    c.ConditionId = item["@ConditionId"];
                                        c.ConditionId = counter;
                                        c.ConditionName = "Direct: " + item["@Name"];
                                        IDictionary<string, object> itemResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(item.ToString());
                                        if (itemResult.ContainsKey("Temperature"))
                                        {

                                            dynamic ress = JsonConvert.DeserializeObject(itemResult["Temperature"].ToString());



                                            if (ress.GetType().Name == "JObject")
                                            {


                                                double res;
                                                double? temperatureRes = null;
                                                if (double.TryParse(ress["@Value"].ToString(), out res))
                                                {
                                                    temperatureRes = res;
                                                }
                                                IDictionary<string, object> ip = new Dictionary<string, object>();
                                                for (int i = 0; i < Enum.GetNames(typeof(PhisicalPropertiesTMEnum)).Length; i++)
                                                {

                                                    ip = JsonConvert.DeserializeObject<Dictionary<string, object>>(ress[((PhisicalPropertiesTMEnum)i).ToString()].ToString());
                                                    if (ip["@Value"].ToString() != "-")
                                                    {

                                                        Property p = new Property()
                                                        {
                                                            SourcePropertyId = int.Parse(ip["@TypeId"].ToString()),
                                                            PropertyName = ip["@Name"].ToString().ToLower(),
                                                            OrigUnit = ip["@Unit"].ToString().Replace("(", "").Replace(")", ""),
                                                            OrigValue = ip["@Value"].ToString(),
                                                            Temperature = temperatureRes
                                                        };
                                                        if (p.OrigValue != "")
                                                        {
                                                            c.Properties.Add(p);
                                                        }
                                                        //c.Properties.Add(p);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (var itemd in ress)
                                                {
                                                    double res;
                                                    double? temperatureRes = null;
                                                    if (double.TryParse(itemd["@Value"].ToString(), out res))
                                                    {
                                                        temperatureRes = res;
                                                    }

                                                    IDictionary<string, object> ip = new Dictionary<string, object>();
                                                    for (int i = 0; i < Enum.GetNames(typeof(PhisicalPropertiesTMEnum)).Length; i++)
                                                    {
                                                        ip = JsonConvert.DeserializeObject<Dictionary<string, object>>(itemd[((PhisicalPropertiesTMEnum)i).ToString()].ToString());
                                                        if (ip["@Value"].ToString() != "-")
                                                        {
                                                            Property p = new Property()
                                                            {
                                                                SourcePropertyId = int.Parse(ip["@TypeId"].ToString()),
                                                                PropertyName = ip["@Name"].ToString().ToLower(),
                                                                OrigUnit = ip["@Unit"].ToString().Replace("(", "").Replace(")", ""),
                                                                OrigValue = ip["@Value"].ToString(),
                                                                Temperature = temperatureRes
                                                            };
                                                            if (p.OrigValue != "")
                                                            {
                                                                c.Properties.Add(p);
                                                            }
                                                            //c.Properties.Add(p);
                                                        }

                                                    }

                                                }
                                            }
                                        }
                                        c.Properties = c.Properties.OrderBy(m => m.PropertyName).ThenBy(m => m.Temperature).ToList();
                                        conds.Add(c);
                                        counter++;
                                    }




                                }
                            }
                        }

                    }
                    // serviceResponse = result["@SessionId"];
                }
                else
                {

                }
                return conds;
            }
        }
        private void AddJSonObjectToTypicalPhysical(ref int count, dynamic ress, ref Condition c)
        {
            string condTemp = "";
            Property p = new Property();
            p.SourcePropertyId = int.Parse(ress["@TypeId"].ToString());
            p.PropertyName = ress["@Name"].ToString().ToLower();
            p.OrigUnit = ress["@Unit"];
            p.OrigValue = ress["@Value"];
            p.ValueId = count;
            condTemp = ress["@Comment"];
            if (c.ConditionName == "")
            {
                c.ConditionName = "Typical: " + condTemp.Split('.')[0];
            }
            if (p.OrigValue != "")
            {
                c.Properties.Add(p);
            }
        }

        public MachinabilityModel GetMachinabilityPropertiesFromService(string sessionId, int materialId)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/Machinability?SessionId=" + sessionId + "&MaterialId=" + materialId;
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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/Machinability?SessionId=" + ssid + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                MachinabilityModel machinability = Deserialize<MachinabilityModel>(json, responsebody);
                machinability.AllReferences = GetAllReferencesFromService(sessionId, materialId, MaterialDetailType.Machinability);
                return machinability;
            }

        }       

        public ICollection<Condition> GetMechanicalRoomPropertiesFromService(string sessionId, int materialId, int subgroupId, int type = 1)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MechanicalProperties/RoomConditionsWithProperties?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MechanicalProperties/RoomConditionsWithProperties?SessionId=" + ssid + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                MechanicalRoom root = Deserialize<MechanicalRoom>(json, responsebody);

                ICollection<Condition> conds = new List<Condition>();
                foreach (var itemCond in root.Conditions)
                {
                    Condition c = new Condition()
                    {
                        ConditionId = itemCond.Id,
                        ConditionName = itemCond.Name,
                        Properties = new List<Property>()
                    };
                    foreach (var itemProp in root.Properties.Where(n => n.no == itemCond.Id))
                    {
                        Property p = new Property();

                        p.SourcePropertyId = itemProp.TypeId;

                        p.PropertyName = itemProp.PropertyName;

                        //     (itemProp.Approx != null)?itemProp.Approx.ToString(): (itemProp.Min !=null ? itemProp.Max);



                        if (itemProp.Approx == null)
                        {
                            if (itemProp.Min != null && itemProp.Max != null) p.OrigValue = ((double)itemProp.Min).ToString("0.####") + "-" + ((double)itemProp.Max).ToString("0.####");
                            if (itemProp.Min != null && itemProp.Max == null) p.OrigValue = "&GreaterEqual; " + ((double)itemProp.Min).ToString("0.####");
                            if (itemProp.Max != null && itemProp.Min == null) p.OrigValue = "&le; " + ((double)itemProp.Max).ToString("0.####");
                            if (itemProp.Max == null && itemProp.Min == null) p.OrigValue = "";
                        }
                        else
                        {
                            p.OrigValue = itemProp.Approx.ToString();
                        }
                        p.OrigUnit = itemProp.Unit;
                        if (p.OrigValue != "")
                        {
                            c.Properties.Add(p);
                        }
                        //c.Properties.Add(p);
                    }
                    c.Properties = c.Properties.OrderBy(m => m.PropertyName).ThenBy(m => m.Temperature).ToList();
                    conds.Add(c);
                }




                return conds;
            }
        }
        public ICollection<Condition> GetMechanicalHighLowPropertiesFromService(string sessionId, int materialId, int subgroupId, MechanicalGroupEnum group, int type = 1)
        {
            IList<ElevatedTemperaturesCondition> root1 = new List<ElevatedTemperaturesCondition>();
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MechanicalProperties/OtherTemperatureConditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&Group=" + (int)group + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MechanicalProperties/OtherTemperatureConditions?SessionId=" + ssid + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&Group=" + (int)group + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                root1 = Deserialize<IList<ElevatedTemperaturesCondition>>(json, responsebody);
            }
            ICollection<Condition> conds = new List<Condition>();
            MechanicalOtherTemperatureGroup mot = new MechanicalOtherTemperatureGroup();
            foreach (var item in root1)
            {
                Condition c = new Condition()
                {
                    ConditionId = item.no,
                    ConditionName = item.Condition,
                    Properties = new List<Property>()
                };


                using (WebClient client = new WebClient())
                {
                    // New code:
                    string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MechanicalProperties/OtherTemperatureProperties?SessionId=" + sessionId + "&ConditionId=" + item.no + "&Group=" + (int)group + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&DisplayUnit=" + type;
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
                        url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/MechanicalProperties/OtherTemperatureProperties?SessionId=" + ssid + "&ConditionId=" + item.no + "&Group=" + (int)group + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&DisplayUnit=" + type;
                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("Host", "localhost:8081");
                        client.Headers.Add("Accept", "application/jsonBasic");
                        client.Encoding = System.Text.Encoding.UTF8;

                        responsebytes = client.DownloadData(new Uri(url));
                        responsebody = Encoding.UTF8.GetString(responsebytes);
                    }
                    var json = new JsonMediaTypeFormatter();
                    mot = Deserialize<MechanicalOtherTemperatureGroup>(json, responsebody);
                }



                foreach (var temp in mot.Temperatures)
                {
                    foreach (var itemProp in temp.Results)
                    {
                        Property p = new Property();
                        p.Temperature = temp.Temperature;
                        p.SourcePropertyId = itemProp.TypeId;
                        p.PropertyName = itemProp.PropertyName;


                        if (itemProp.Approx == null)
                        {
                            if (itemProp.Min != null && itemProp.Max != null) p.OrigValue = ((double)itemProp.Min).ToString("0.####") + "-" + ((double)itemProp.Max).ToString("0.####");
                            if (itemProp.Min != null && itemProp.Max == null) p.OrigValue = "&GreaterEqual; " + ((double)itemProp.Min).ToString("0.####");
                            if (itemProp.Max != null && itemProp.Min == null) p.OrigValue = "&le; " + ((double)itemProp.Max).ToString("0.####");
                            if (itemProp.Max == null && itemProp.Min == null) p.OrigValue = "";
                        }
                        else
                        {
                            p.OrigValue = itemProp.Approx.ToString();
                        }
                        p.OrigUnit = itemProp.Unit;
                        if (p.OrigValue != "")
                        {
                            c.Properties.Add(p);
                        }
                    }
                }
                c.Properties = c.Properties.OrderBy(m => m.PropertyName).ThenBy(m => m.Temperature).ToList();

                conds.Add(c);



            }

            return conds;

        }

        public HeatTreatment GetHeatTreatmentFromService(string sessionId, int materialId, int subgroupId)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/HeatTreatment?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/HeatTreatment?SessionId=" + ssid + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }

                HeatTreatment returnValue = JsonConvert.DeserializeObject<HeatTreatment>(responsebody);
                if (returnValue.Comment == null)
                {
                    returnValue.Comment = "";
                }
                for (int i = 0; i < returnValue.Diagrams.Count; i++)
                {
                    returnValue.Diagrams[i].ImageName = WebConfigurationManager.AppSettings["picturePath"] + "/Content/static/heattreatment/" + returnValue.Diagrams[i].ImageName;
                }
                return returnValue;
            }
        }
               
        public EquivalencyModel GetCrossReferenceFromService(string sessionId, int materialId, int subgroupId)
        {
            EquivalencyModel model = new EquivalencyModel() { Equivalences = new List<CrossReferenceModel>() };
            using (WebClient client = new WebClient())
            {
                // New code:

                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CrossReference/Materials?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Host", "localhost:8081");
                client.Headers.Add("Accept", "application/json");
                client.Encoding = System.Text.Encoding.UTF8;


                byte[] responsebytes = client.DownloadData(new Uri(url));
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody == "<Result><Status>2</Status><ErrorDescriptor>WrongAuthentication</ErrorDescriptor></Result>")
                {
                    string ssid = GetSessionFromService();
                    System.Web.HttpContext.Current.Session["TotalMateriaSession"] = ssid;
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CrossReference/Materials?SessionId=" + ssid + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                IDictionary<string, object> returnValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(responsebody);

                if (returnValue.ContainsKey("Result"))
                {
                    IDictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(returnValue["Result"].ToString());
                    if (result.ContainsKey("CrossReferences"))
                    {
                        IDictionary<string, object> itemResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(result["CrossReferences"].ToString());
                        if (itemResult.ContainsKey("CrossReference"))
                        {
                            dynamic dynRes = JsonConvert.DeserializeObject(itemResult["CrossReference"].ToString());
                            if (dynRes.GetType().Name == "JObject")
                            {
                                CrossReferenceModel crm = new CrossReferenceModel();
                                crm.CountryStandard = dynRes["@CountryStandard"];
                                crm.EquivalenceCategory = dynRes["@EquivalencyCategory"];
                                crm.MaterialId = dynRes["@MaterialId"];
                                crm.MaterialName = dynRes["@Name"];
                                model.Equivalences.Add(crm);
                            }
                            else
                            {
                                foreach (var itemd in dynRes)
                                {
                                    CrossReferenceModel crm = new CrossReferenceModel();
                                    crm.CountryStandard = itemd["@CountryStandard"];
                                    crm.EquivalenceCategory = itemd["@EquivalencyCategory"];
                                    crm.MaterialId = itemd["@MaterialId"];
                                    crm.MaterialName = itemd["@Name"];
                                    model.Equivalences.Add(crm);
                                }
                            }
                        }
                    }
                }
                return model;
            }
        }

        public MetallographyModel GetMetallographyPropertiesFromService(string sessionId, int materialId, int subgroupId)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/MetallographyConditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/MetallographyConditions?SessionId=" + ssid + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                MetallographyModel model = new MetallographyModel() { MetConditions = new List<MetallographyConditionModel>() };
                IList<MetallographyCondition> root = Deserialize<List<MetallographyCondition>>(json, responsebody);
                ICollection<MetallographyConditionModel> conds = new List<MetallographyConditionModel>();

                foreach (var itemCond in root)
                {
                    MetallographyDetailModel metDetail = GetMetallographyConditionDetailFromService(sessionId, materialId, subgroupId, itemCond.no);
                    IList<string> selectedReferences = GetReferencesForSelectedConditionFromService(sessionId, materialId, itemCond.no, MaterialDetailType.Metallography);
                    model.MetConditions.Add(new MetallographyConditionModel()
                    {
                        ConditionId = itemCond.no,
                        Name = itemCond.Condition,
                        Details = metDetail,
                        SelectedReferences = selectedReferences
                    });
                }
                model.AllReferences = GetAllReferencesFromService(sessionId, materialId, MaterialDetailType.Metallography);
                return model;
            }
        }
        public MetallographyDetailModel GetMetallographyConditionDetailFromService(string sessionId, int materialId, int subgroupId, int conditionId)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/MetallographyConditionDetails?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/MetallographyConditionDetails?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId + "&ConditionId=" + conditionId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                MetallographyDetailModel root = Deserialize<MetallographyDetailModel>(json, responsebody);
                root.SelectedReferences = GetReferencesForSelectedConditionFromService(sessionId, materialId, conditionId, MaterialDetailType.Metallography);
                return root;
            }
        }

        public ICollection<Condition> GetChemicalCompositionFromService(string sessionId, int materialId, int subgroupId)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/ChemicalComposition/ChemicalComposition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/ChemicalComposition/ChemicalComposition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&SubgroupId=" + subgroupId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                ChemicalComposition root = Deserialize<ChemicalComposition>(json, responsebody);

                ICollection<Condition> conds = new List<Condition>();
                Condition c = new Condition()
                {
                    ConditionId = 0,
                    ConditionName = "",
                    Properties = new List<Property>()
                };
                int i = 0;
                foreach (var itemCond in root.CompositionList)
                {

                    Property p = new Property();
                    p.ValueId = i;
                    i++;
                    p.ChemicalIdentityId = itemCond.cc_crit;
                    //p.SourcePropertyId = itemProp.TypeId;

                    p.PropertyName = itemCond.cc_crit;

                    //     (itemProp.Approx != null)?itemProp.Approx.ToString(): (itemProp.Min !=null ? itemProp.Max);
                    if (itemCond.cc_aprox == null)
                    {
                        if (itemCond.cc_min != null && itemCond.cc_max != null)
                        {
                            p.OrigValue = (((double)itemCond.cc_min).ToString("0.##")) + "-" + (((double)itemCond.cc_max).ToString("0.##"));
                        }
                        if (itemCond.cc_min != null && itemCond.cc_max == null)
                        {
                            p.OrigValue = "&GreaterEqual;" + (((double)itemCond.cc_min).ToString("0.##"));
                        }
                        if (itemCond.cc_max != null && itemCond.cc_min == null)
                        {
                            p.OrigValue = "&le;" + (((double)itemCond.cc_max).ToString("0.##"));
                        }
                        if (itemCond.cc_max == null && itemCond.cc_min == null)
                        {
                            p.OrigValue = "";
                        }
                    }
                    else
                    {
                        p.OrigValue = ((Double)(itemCond.cc_aprox)).ToString("0.##");
                    }

                    p.OrigUnit = "%";

                    p.OrigValueText = !string.IsNullOrEmpty(itemCond.cc_text) ? itemCond.cc_text : "-";

                    if (p.OrigValue != "" || p.OrigValueText != "")
                    {
                        c.Properties.Add(p);
                    }
                    c.Properties = c.Properties.OrderBy(m => m.PropertyName).ToList();

                }

                conds.Add(c);


                return conds;

            }
        }

        public IList<ElsevierMaterials.Models.MaterialCondition> GetStressStrainMaterialConditionsFromService(string sessionId, int materialId, int type = 1)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/MaterialConditions?SessionId=" + sessionId + "&MaterialId=" + materialId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/MaterialConditions?SessionId=" + sessionId + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.MaterialCondition> root = Deserialize<List<Api.Models.MaterialCondition>>(json, responsebody);
                IList<ElsevierMaterials.Models.MaterialCondition> materialConditions = new List<ElsevierMaterials.Models.MaterialCondition>();

                foreach (var item in root)
                {
                    ElsevierMaterials.Models.MaterialCondition ssItem = new ElsevierMaterials.Models.MaterialCondition()
                    {
                        ConditionId = item.ConditionId,
                        Description = item.Description,
                        MaterialId = item.MaterialId
                    };
                    materialConditions.Add(ssItem);
                }
                return materialConditions;
            }
        }
        public StressStrainModel GetStressStrainTestConditionsWithDataFromService(string sessionId, int materialId, string conditionId, int type = 1)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/TestConditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/TestConditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                StressStrainModel result = new StressStrainModel() { TestConditions = new List<StressStrainConditionModel>() };


                IList<ElsevierMaterials.Models.StressStrainTestCondition> root = Deserialize<List<ElsevierMaterials.Models.StressStrainTestCondition>>(json, responsebody);
                foreach (var item in root)
                {
                    StressStrainConditionModel ssItem = new StressStrainConditionModel()
                    {
                        //comment = item.comment,
                        Condition = item.Description,
                        //engineering = item.engineering,
                        //FormId = item.FormId,
                        //HeatTreatmentId = item.HeatTreatmentId,
                        //LinearCoefComment = item.LinearCoefComment,
                        No = int.Parse(item.ConditionId),
                        //sr = item.sr,
                        //test_temperature = item.test_temperature,
                        //TestingType = item.TestingType
                    };
                    ssItem.SelectedReferences = GetReferencesForSelectedConditionFromService(sessionId, materialId, ssItem.No, MaterialDetailType.StressStrain);
                    result.TestConditions.Add(ssItem);
                }
                if (result.TestConditions.Count > 0)
                {
                    result.TestConditions[0].StressTemperatures = GetStressStrainTemperatures(sessionId, materialId, result.TestConditions[0].No, type);
                }
                result.AllReferences = GetAllReferencesFromService(sessionId, materialId, MaterialDetailType.StressStrain);

                return result;
            }
        }

        //service that returns only ddl for test condition
        public StressStrainModel GetStressStrainOnlyTestConditionsFromService(string sessionId, int materialId, string conditionId, int type = 1)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/TestConditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/TestConditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();
                StressStrainModel result = new StressStrainModel() { TestConditions = new List<StressStrainConditionModel>() };


                IList<ElsevierMaterials.Models.StressStrainTestCondition> root = Deserialize<List<ElsevierMaterials.Models.StressStrainTestCondition>>(json, responsebody);
                foreach (var item in root)
                {
                    StressStrainConditionModel ssItem = new StressStrainConditionModel()
                    {
                        //comment = item.comment,
                        Condition = item.Description,
                        //engineering = item.engineering,
                        //FormId = item.FormId,
                        //HeatTreatmentId = item.HeatTreatmentId,
                        //LinearCoefComment = item.LinearCoefComment,
                        No = int.Parse(item.ConditionId),
                        //sr = item.sr,
                        //test_temperature = item.test_temperature,
                        //TestingType = item.TestingType
                    };
                    ssItem.SelectedReferences = GetReferencesForSelectedConditionFromService(sessionId, materialId, ssItem.No, MaterialDetailType.StressStrain);
                    result.TestConditions.Add(ssItem);
                } 
                return result;
            }
        }
        public IList<StressStrainTemperature> GetStressStrainTemperatures(string sessionId, int materialId, int conditionId, int type = 1)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/TemperaturesForCondition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/TemperaturesForCondition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId;
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
                    result[0].TrueDetails = GetStressStrainDetails(sessionId, materialId, conditionId, result[0].Temperature, 1, type);
                    result[0].EngineeringDetails = GetStressStrainDetails(sessionId, materialId, conditionId, result[0].Temperature, 2, type);
                }
                return result;
            }
        }
        public StressStrainDetails GetStressStrainDetails(string sessionId, int materialId, int conditionId, double temperature, int ssType, int type)
        {

        

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/ConditionDetails?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&StressStrainCategory=" + ssType + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/ConditionDetails?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&StressStrainCategory=" + ssType + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();


                StressStrainConditionDetails temperatures = Deserialize<StressStrainConditionDetails>(json, responsebody);

                StressStrainConditionDiagram StressStrainDiagPoints = GetStressStrainConditionPointsForDiagram(sessionId, materialId, conditionId, temperature, ssType, type);
                StressStrainDetails result = new StressStrainDetails() { Comment = temperatures.Comment, Points = temperatures.Points, UnitType = type, PointsForDiagram= StressStrainDiagPoints};

                result.Diagram = GetStressStrainDiagram(sessionId, materialId, conditionId, temperature, ssType, type);
                return result;
            }
        }

        public StressStrainConditionDiagram GetStressStrainConditionPointsForDiagram(string sessionId, int materialId, int conditionId, double temperature, int ssType, int type)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/ConditionPointsForDiagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&StressStrainCategory=" + ssType + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/ConditionPointsForDiagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&StressStrainCategory=" + ssType + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                StressStrainConditionDiagram diagram = Deserialize<StressStrainConditionDiagram>(json, responsebody);
               
                return diagram;
            }
        }
        public ImageSource GetStressStrainDiagram(string sessionId, int materialId, int conditionId, double temperature, int ssType, int type)
        {

            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/Diagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&StressStrainCategory=" + ssType + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/StressStrain/Diagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&Temperature=" + temperature + "&StressStrainCategory=" + ssType + "&DisplayUnit=" + type;
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
        
        public IList<ElsevierMaterials.Models.MaterialCondition> GetFatigueMaterialConditionsFromService(string sessionId, int materialId, int fatigueCategory, int type)
        {
            using (WebClient client = new WebClient())
            {
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/MaterialConditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&FatigueCategory=" + fatigueCategory + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/MaterialConditions=" + sessionId + "&MaterialId=" + materialId + "&FatigueCategory=" + fatigueCategory + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<ElsevierMaterials.Models.MaterialCondition> listFatigue = Deserialize<IList<ElsevierMaterials.Models.MaterialCondition>>(json, responsebody);

                return listFatigue;
            }
        }
        public IList<TestCondition> GetFatigueTestConditionsFromService(string sessionId, int materialId, string conditionId, int fatigueCategory, int type)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/TestConditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&FatigueCategory=" + fatigueCategory + "&ConditionId=" + conditionId + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/TestConditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&FatigueCategory=" + fatigueCategory + "&ConditionId=" + conditionId + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<TestCondition> listFatigue = Deserialize<IList<TestCondition>>(json, responsebody);

                return listFatigue;
            }
        }
        public IList<Api.Models.Fatigue.FatigueCondition> GetFatigueConditionsFromService(string sessionId, int materialId, int fatigueCategory, int type)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/Conditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&FatigueCategory=" + fatigueCategory + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/Conditions?SessionId=" + sessionId + "&MaterialId=" + materialId + "&FatigueCategory=" + fatigueCategory + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.Fatigue.FatigueCondition> listFatigue = Deserialize<IList<Api.Models.Fatigue.FatigueCondition>>(json, responsebody);

                return listFatigue;
            }
        }
        public Api.Models.Fatigue.FatigueConditionDetails GetFatigueStrainLifeConditionDetailsFromService(string sessionId, int materialId, string condition, int type)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StrainLifeConditionDetails?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StrainLifeConditionDetails?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                Api.Models.Fatigue.FatigueConditionDetails conDetails = Deserialize<Api.Models.Fatigue.FatigueConditionDetails>(json, responsebody);
                return conDetails;
            }
        }
        public ImageSource GetFatigueStrainSNCurveDiagramFromService(string sessionId, int materialId, string condition, int type)
        {
            using (WebClient client = new WebClient())
            {
                ImageSource img = new ImageSource();
                try
                {
                    // New code:
                    string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StrainSNCurveDiagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;

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
                        url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StrainSNCurveDiagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;
                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("Host", "localhost:8081");
                        client.Headers.Add("Accept", "application/jsonBasic");
                        client.Encoding = System.Text.Encoding.UTF8;

                        responsebytes = client.DownloadData(new Uri(url));
                        responsebody = Encoding.UTF8.GetString(responsebytes);
                    }
                    var json = new JsonMediaTypeFormatter();

                    string mimeType = "image/png";
                    string base64 = Convert.ToBase64String(responsebytes);

                    img.Source = string.Format("data:{0};base64,{1}", mimeType, base64);

                }
                catch { img = new ImageSource(); }

                return img;
            }
        }

        public Api.Models.Fatigue.FatigueConditionDiagram GetFatigueStrainSNCurveDiagramPointsFromService(string sessionId, int materialId, string condition, int type)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StrainSNCurveDiagramPoints?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StrainSNCurveDiagramPoints?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }

                var json = new JsonMediaTypeFormatter();

                Api.Models.Fatigue.FatigueConditionDiagram conDetails = Deserialize<Api.Models.Fatigue.FatigueConditionDiagram>(json, responsebody);
                return conDetails;
            }            
        }
        public IList<Api.Models.Fatigue.StrainLifePoint> GetFatigueStrainSNCurveDataFromService(string sessionId, int materialId, string condition, int type = 1)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StrainSNCurveData?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StrainSNCurveData?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.Fatigue.StrainLifePoint> points = Deserialize<IList<Api.Models.Fatigue.StrainLifePoint>>(json, responsebody);
                return points;
            }
        }
        public Api.Models.Fatigue.FatigueConditionDetails GetFatigueStressLifeConditionDetailsFromService(string sessionId, int materialId, string condition, int type = 1)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StressLifeConditionDetails?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StressLifeConditionDetails?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                Api.Models.Fatigue.FatigueConditionDetails conDetails = Deserialize<Api.Models.Fatigue.FatigueConditionDetails>(json, responsebody);
                return conDetails;
            }
        }
        public ImageSource GetFatigueStressSNCurveDiagramFromService(string sessionId, int materialId, string condition, int type = 1)
        {
            using (WebClient client = new WebClient())
            {
                ImageSource img = new ImageSource();
                try
                {
                    string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StressSNCurveDiagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;

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
                        url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StressSNCurveDiagram?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;
                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("Host", "localhost:8081");
                        client.Headers.Add("Accept", "application/jsonBasic");
                        client.Encoding = System.Text.Encoding.UTF8;

                        responsebytes = client.DownloadData(new Uri(url));
                        responsebody = Encoding.UTF8.GetString(responsebytes);
                    }
                    var json = new JsonMediaTypeFormatter();

                    string mimeType = "image/png";
                    string base64 = Convert.ToBase64String(responsebytes);

                    img = new ImageSource();
                    img.Source = string.Format("data:{0};base64,{1}", mimeType, base64);
                }

                catch
                {
                    img = new ImageSource();
                }
                return img;
            }
        }

        public Api.Models.Fatigue.FatigueConditionDiagram GetFatigueStressSNCurveDiagramPointsFromService(string sessionId, int materialId, string condition, int type = 1)
        {
            using (WebClient client = new WebClient())
            {
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StressSNCurveDiagramPoints?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;

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
                        url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StressSNCurveDiagramPoints?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;
                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("Host", "localhost:8081");
                        client.Headers.Add("Accept", "application/jsonBasic");
                        client.Encoding = System.Text.Encoding.UTF8;

                        responsebytes = client.DownloadData(new Uri(url));
                        responsebody = Encoding.UTF8.GetString(responsebytes);
                    }
                    var json = new JsonMediaTypeFormatter();

                    Api.Models.Fatigue.FatigueConditionDiagram conDetails = Deserialize<Api.Models.Fatigue.FatigueConditionDiagram>(json, responsebody);
                    return conDetails;               
            }
        }
        public IList<Api.Models.Fatigue.StrainLifePoint> GetFatigueStressSNCurveDataFromService(string sessionId, int materialId, string condition, int type = 1)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StressSNCurveData?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Fatigue/StressSNCurveData?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + condition + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<Api.Models.Fatigue.StrainLifePoint> points = Deserialize<IList<Api.Models.Fatigue.StrainLifePoint>>(json, responsebody);
                return points;
            }
        }
    
        public IList<ElsevierMaterials.Models.MaterialCondition> GetCreepMaterialConditionsFromService(string sessionId, int materialId)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepData/MaterialConditions?SessionId=" + sessionId + "&MaterialId=" + materialId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepData/MaterialConditions?SessionId=" + sessionId + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<ElsevierMaterials.Models.MaterialCondition> conDetails = Deserialize<IList<ElsevierMaterials.Models.MaterialCondition>>(json, responsebody);
                return conDetails;
            }
        }
        public IList<TestCondition> GetCreepTestConditionsFromService(string sessionId, int materialId, string conditionId)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepData/TestConditions?SessionId=" + sessionId + "&ConditionId=" + conditionId + "&MaterialId=" + materialId;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepData/TestConditions?SessionId=" + sessionId + "&ConditionId=" + conditionId + "&MaterialId=" + materialId;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                IList<TestCondition> conDetails = Deserialize<IList<TestCondition>>(json, responsebody);
                return conDetails;
            }
        }
        public IList<CreepConditionModel> GetCreepDataConditionsFromService(string sessionId, int materialId)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepData/Conditions?SessionId=" + sessionId + "&MaterialId=" + materialId;

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

                IList<CreepConditionModel> conDetails = Deserialize<IList<CreepConditionModel>>(json, responsebody);
                return conDetails;
            }
        }
        public Api.Models.CreepData.CreepData GetCreepDataFromService(string sessionId, int materialId, int conditionId, int type = 1)
        {
            using (WebClient client = new WebClient())
            {
                // New code:
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepData/CreepData?SessionId=" + sessionId + "&MaterialId=" + materialId + "&ConditionId=" + conditionId + "&DisplayUnit=" + type;

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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/CreepData/CreepData?SessionId=" + sessionId + "&MaterialId=" + materialId + "&DisplayUnit=" + type;
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("Host", "localhost:8081");
                    client.Headers.Add("Accept", "application/jsonBasic");
                    client.Encoding = System.Text.Encoding.UTF8;

                    responsebytes = client.DownloadData(new Uri(url));
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }
                var json = new JsonMediaTypeFormatter();

                Api.Models.CreepData.CreepData conDetails = Deserialize<Api.Models.CreepData.CreepData>(json, responsebody);
                return conDetails;

            }
        }
                
        public IList<string> GetAllReferencesFromService(string sessionId, int materialId, MaterialDetailType productGroupType)
        {

            using (WebClient client = new WebClient())
            {
                // New code
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/AllReferences?SessionId=" + sessionId + "&MaterialId=" + materialId + "&PropertiesGroupType=" + productGroupType;
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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/AllReferences?SessionId=" + sessionId + "&MaterialId=" + materialId + "&PropertiesGroupType=" + productGroupType;
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
        public IList<string> GetReferencesForSelectedConditionFromService(string sessionId, int materialId, int conditionId, MaterialDetailType productGroupType)
        {
            if (conditionId == 0) return null;
            using (WebClient client = new WebClient())
            {
                // New code
                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/ReferencesForSelectedCondition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&PropertiesGroupType=" + productGroupType + "&ConditionId=" + conditionId;
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
                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/ReferencesForSelectedCondition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&PropertiesGroupType=" + productGroupType + "&ConditionId=" + conditionId;
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
        public IList<string> GetReferencesForSelectedConditionFatigueFromService(string sessionId, int materialId, string conditionId, MaterialDetailType productGroupType)
        {
            int condition;
            if (Int32.TryParse(conditionId.Split(';')[0], out condition))
            {
                using (WebClient client = new WebClient())
                {
                    // New code
                    string url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/ReferencesForSelectedCondition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&PropertiesGroupType=" + productGroupType + "&ConditionId=" + condition;
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
                        url = WebConfigurationManager.AppSettings["apiAddress"] + "/altair/Properties/ReferencesForSelectedCondition?SessionId=" + sessionId + "&MaterialId=" + materialId + "&PropertiesGroupType=" + productGroupType + "&ConditionId=" + condition;
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
            else
            {
                return null;
            }

        }

        public IList<int> GetMaterialIdsForAdvSearchPropertiesFromService(string sessionId, AdvSearchFiltersAll filters)
        {
            if (
                filters == null
                )
            {
                return new List<int>();
            }

            using (WebClient client = new WebClient())
            {

                string url = WebConfigurationManager.AppSettings["apiAddress"] + "/tm/AdvancedSearch/Materials?SessionId=" + sessionId;
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

                    url = WebConfigurationManager.AppSettings["apiAddress"] + "/tm/AdvancedSearch/Materials?SessionId=" + sessionId;
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




    }

    public enum PhisicalPropertiesTMEnum
    {
        Density = 0,
        ElecricalResistivity,
        MeanCoefficient,
        ModulOfElasticity,
        Poisson,
        ThermalCapacity,
        ThermalConductivity,
        DynamicModulusOfElasticity, 
        CompressiveModulusOfElasticity,
        ShearModulus, 
        MeltingTemperature
    }
    public enum MechanicalPropertiesTMEnum
    {
        YieldStress = 1,
        TensileStress,
        Elongation,
        ReductionOfArea,
        Impact,
        Hardness

    }

}