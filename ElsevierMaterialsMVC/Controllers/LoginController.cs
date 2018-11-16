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

        public ActionResult Index()
        {
            string access_token = generate_token("http://dev.knewknovel.com");
            //string access_token = "1542106524-312825-456-4ff6e6f8260190de0cdaba71c14bf1a0";
            bool isOK = verify_token(access_token, "http://dev.knewknovel.com");

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

        public bool verify_token(string token, string uri)
        {
            //$access_token="$timestamp-$user_id-$org_id-$signatureHash";
            //token = "1539697713-312825-11-44bb67e6fb6ffe6e21981944683d5c72";
            //uri = "http://dev.knewknovel.com";
            bool isVerifiedSignature = false;
            bool isVerifiedTimestamp = false;
            string[] tokenParts = token.Split('-');
            string timestamp = tokenParts[0];
            string user_id = tokenParts[1];
            string org_id = tokenParts[2];
            string signature = tokenParts[3];
            string shared_secret = "KNOVEL+EMT";
            string valueVerified = timestamp + "-" + user_id + "-" + org_id + "-" + uri + "-" + shared_secret;
            string signatureHashVerified = ComputeSha256Hash(valueVerified).Substring(0, 32);
            isVerifiedSignature = signatureHashVerified.Equals(signature, StringComparison.Ordinal);
            isVerifiedTimestamp = getTimestamp(Int32.Parse(timestamp)) > 300 ? false : true;
            return isVerifiedSignature & isVerifiedTimestamp;
        }

        public string generate_token(string uri)
        {
            //uri = "http://dev.knewknovel.com";
            Int32 timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            int user_id = 312825;
            int org_id = 456;
            string shared_secret = "KNOVEL+EMT";
            string value = timestamp + "-" + user_id + "-" + org_id + "-" + uri + "-" + shared_secret;
            /*$signatureHash - string holding hexadecimal encoding of SHA 256 hash.*/
            string signatureHash = ComputeSha256Hash(value).Substring(0, 32);
            string access_token = timestamp + "-" + user_id + "-" + org_id + "-" + signatureHash;
            return access_token;
        }
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

        public Int32 getTimestamp(Int32 timestamp)
        {
            Int32 timestampNow = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return (timestampNow - timestamp);
        }


    }
}
