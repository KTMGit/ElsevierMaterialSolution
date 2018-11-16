using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.Comparison
{
    public interface IFactory
    {
        bool AddProperty(int sourceId);
    }
}