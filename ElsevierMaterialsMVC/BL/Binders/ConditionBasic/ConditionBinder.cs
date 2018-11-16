using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Property;

namespace ElsevierMaterialsMVC.BL.Binders.ConditionBasic
{
    public class ConditionBinder
    {        
        public Condition FillCondition(int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient)
        {
            //TODO: Source stavi u Enum: 1- els, 2-TMMetals, 3-TMPlus
            switch (sourceId)
            {
                case 1:
                    //TODO: Zasto je condition za els prazan?
                    return null;
                case 2:
                    //TODO: ConditionId umesto RowId... ????
                    //Comment: Ovo je metoda za Physical, Mechanica, Chemical grupe
                    return _conditionTMMetalsBinder.FillCondition(subgroupId, sourceMaterialId, sourceId, propertyClient.GroupId, propertyClient.ConditionId, context);
                case 3:
                    //TODO: ConditionId umesto RowId... ????, kako se puni condition za Chemical PLUS?
                    //Comment: Ovo je metoda za Physical, Mechanica
                    return _conditionTMPlusBinder.FillConditionData(subgroupId, sourceMaterialId, sourceId, propertyClient.GroupId, propertyClient.ConditionId, context);
                default:
                    return null;
            }
        }

        public ConditionBinder()
        {          
            _conditionTMPlusBinder = new ConditionTMPlusBinder();
            _conditionTMMetalsBinder = new ConditionTMMetalsBinder();          
        }

        private ConditionTMPlusBinder _conditionTMPlusBinder;
        private ConditionTMMetalsBinder _conditionTMMetalsBinder;
    }
}