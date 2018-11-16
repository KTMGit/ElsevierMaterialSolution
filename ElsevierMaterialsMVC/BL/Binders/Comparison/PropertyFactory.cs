using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.Comparison
{
    public class PropertyFactory : IFactory
    {
        
        public bool AddProperty(int sourceId)
        {
            switch (sourceId)
            {
                case 1:
                    break;
                case 2:
                    break;
            }
            return true;
        }

    }
}