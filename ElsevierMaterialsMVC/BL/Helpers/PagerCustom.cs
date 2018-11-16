using IniCore.Web.Mvc.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Helpers
{
    public class PagerCustom
    {
        public IQueryable<T> PagerSearch<T>(PagerDescriptor pagerDesc, IQueryable<T> results, int recordCount)
        {

            int page1 = (int)Math.Ceiling((recordCount + 0.0) / pagerDesc.SelectedPageSize);
            if (pagerDesc.SelectedPage > page1)
            {
                pagerDesc.SelectedPage = page1;
            }
            pagerDesc.TotalRecordCount = recordCount;
            pagerDesc.TotalPageCount = page1;
            if (pagerDesc.SelectedPage > pagerDesc.TotalPageCount)
            {
                pagerDesc.SelectedPage = pagerDesc.TotalPageCount;
            }

            return results.Skip((pagerDesc.SelectedPage - 1) * pagerDesc.SelectedPageSize).Take(pagerDesc.SelectedPageSize);
        }
    }
}