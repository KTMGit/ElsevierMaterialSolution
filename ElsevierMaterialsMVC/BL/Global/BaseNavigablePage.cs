using ElsevierMaterials.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class BaseNavigablePage : INavigable, IPageDataContainer {
        public bool IsActive { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string PageId { get; set; }

        public string ElementId { get; set; }

        private string _NavigableName;
        public string NavigableName {
            get {
                //if (!(String.IsNullOrEmpty(this.PageId) && string.IsNullOrEmpty(this.ElementId))) {
                //    return Ktm.Web.Mvc.Helpers.TranslationHelper.GetTranslation(PageId, ElementId);
                //}
                return _NavigableName;
            }

            set {
                _NavigableName = value;
            }
        }

        public bool ReplaceLast { get; set; }

        public object PageData { get; set; }

        public string NavigableID { get; set; }

        public bool IsVisible { get; set; }

       public BaseNavigablePage(bool _isVisible=true)
        {
            IsVisible = _isVisible;
        }

    }
}