using ElsevierMaterialsMVC.Models.Login;
using IniCore.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace ElsevierMaterialsMVC.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        //public ActionResult Index()
        //{
        //    return View();
        //}


        //[HttpPost()]
        //public ActionResult Login(string username, string password) {
        //    string usr = WebConfigurationManager.AppSettings["username"];
        //    string passw = WebConfigurationManager.AppSettings["password"];
        //    if (usr == username && passw == password){
        //        Session["username"] = usr;
        //        return Json(new { success = true });

        //    }
        //    else return Json(new { success = false });
        //}
        private int clockDriftAllowedDiffInSeconds = 300;//5 minutes, to prevent replay attack 
        public ActionResult Index()
        {
            bool isOK = false;
            string access_token = "";
            //?accesstoken="<unix_time_second> - <currentuserID> - <organizationId> -<signatureHash>" 
            //<signatureHash> = sha256(“<unix_time_second> - <currentuserID> - <organizationId> - “<shared_secrete>”)
            //access_token = generate_token(uri);           
            string path = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            //string path = "http://dev.knewknovel.com/emt/?access_token=1543301462-1893563-7775-9aec47147f12d569c704ed43064aaf3e";

            if (path.Contains("access_token="))
            {
                access_token = Request.QueryString["access_token"].ToString();
                //access_token = "1543301462-1893563-7775-9aec47147f12d569c704ed43064aaf3e";
                isOK = verify_token(access_token);
            }
            else
            {
                ViewBag.ErrorToken = "Token not verified in: " + path;
            }


            if (isOK)
            {
                string usr = WebConfigurationManager.AppSettings["username"];
                string passw = WebConfigurationManager.AppSettings["password"];
                Session["username"] = usr;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }


        [HttpPost()]
        public ActionResult Login(string username, string password)
        {
            string usr = WebConfigurationManager.AppSettings["username"];
            string passw = WebConfigurationManager.AppSettings["password"];

            if (usr == username && passw == password)
            {
                Session["username"] = usr;
                return Json(new { success = true });
            }
            else return Json(new { success = false });
        }

        public bool verify_token(string token)
        {
            bool isVerifiedSignature = false;
            bool isVerifiedTimestamp = true;
            string[] tokenParts = token.Split('-');
            if (tokenParts.Length != 4)
            {
                return false;
            }
            string timestamp = tokenParts[0];
            string user_id = tokenParts[1];
            string org_id = tokenParts[2];
            string signature = tokenParts[3];
            string shared_secret = "KNOVEL+EMT";
            Int32 tokenTimestamp;
            if (!Int32.TryParse(timestamp, out tokenTimestamp))
            {
                return false;//invalid timestamp format
            }
            string valueVerified = timestamp + "-" + user_id + "-" + org_id + "-" + shared_secret;
            string signatureHashVerified = ComputeSha256Hash(valueVerified).Substring(0, 32);
            isVerifiedSignature = signatureHashVerified.Equals(signature, StringComparison.Ordinal);
            //isVerifiedTimestamp = getTimestamp(tokenTimestamp) > clockDriftAllowedDiffInSeconds ? false : true;            
            return isVerifiedSignature & isVerifiedTimestamp;
        }

        //public string generate_token(string uri)
        //{            
        //    Int32 timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        //    int user_id = 1893563;
        //    int org_id = 7775;
        //    string shared_secret = "KNOVEL+EMT";
        //    string value = timestamp + "-" + user_id + "-" + org_id + "-" + shared_secret;
        //    /*$signatureHash - string holding hexadecimal encoding of SHA 256 hash.*/
        //    string signatureHash = ComputeSha256Hash(value).Substring(0, 32);
        //    string access_token = timestamp + "-" + user_id + "-" + org_id + "-" + signatureHash;
        //    return access_token;
        //}
        static string ComputeSha256Hash(string value)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public Int32 getTimestamp(Int32 tokenTimestamp)
        {
            Int32 timestampNow = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return (timestampNow - tokenTimestamp);
        }


    }
}
