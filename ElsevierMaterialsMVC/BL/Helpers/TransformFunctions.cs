using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IniCore.Web.Mvc.Html;
using System.Text;

namespace ElsevierMaterialsMVC.BL.Helpers
{
    public static class TransformFunctions
    {
        public static string RemoveLast(this string text, string character)
        {
            if (text.Length < 1) return text;
            return text.Remove(text.ToString().LastIndexOf(character), character.Length);
        }
    }
}