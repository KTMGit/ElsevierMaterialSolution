using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.Models.Home {
    public class IndexModel {
        public string SearchText { get; set; }
        public int ClasificationTypeId { get; set; }
        public int ClasificationId { get; set; }

        public IndexModel()
        {
            SearchText = "";
        }

        public IndexModel(string text)
        {
            SearchText = text;
        }


        public IndexModel(int id, int typeId)
        {
            ClasificationId = id;
            ClasificationTypeId = typeId;
        }
    }
}