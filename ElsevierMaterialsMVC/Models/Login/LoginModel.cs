using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.Models.Login {
    public class LoginModel {
        [Required(ErrorMessage = "Username required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string RedirectURL { get; set; }
    }
}