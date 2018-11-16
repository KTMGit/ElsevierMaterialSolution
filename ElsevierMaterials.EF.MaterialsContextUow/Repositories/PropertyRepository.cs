using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.Models;
using System.Data.Entity;
using System.Linq;

namespace ElsevierMaterials.EF.MaterialsContextUow.Repositories {
    public class PropertyRepository : BaseRepository<MaterialProperty>, IPropertyRepository {
        public PropertyRepository(DbContext dbContext)
            : base(dbContext) {
        }
        public Property GetPropertyInfoForMaterial(string name, int materialId, int subgroupId, int rowId, int propertyId, int valueId, string unitName) {


            Property prop = new Property() { PropertyId = propertyId, SourcePropertyId = 0, SourceId = 1, DefaultUnitId = 1, MaterialId = materialId, SubgroupId = subgroupId, RowId = rowId, ValueId = valueId };


            prop.PropertyName = name;
            MaterialProperty mProp = DataSet.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.RowId == rowId && m.PropertyId == propertyId && m.ValueId == valueId).FirstOrDefault();

            //prop.AdditionalCondition = mProp.AdditionalCondition;
        //   prop.Temperature = mProp.Temperature;
           
       //  prop.OrigValueText = mProp.OrigValueText;
            prop.OrigUnit = unitName;
            if (mProp.OrigValue == null && mProp.OrigValueMax == null && mProp.OrigValueMin == null)
            {
                if(!string.IsNullOrEmpty(mProp.OrigValueText))
                {
                    prop.OrigValue = mProp.OrigValueText;
                    if (!string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature != null) prop.OrigValueText = mProp.AdditionalCondition + " ;" + mProp.Temperature.ToString() + "°C";
                    if (!string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature == null) prop.OrigValueText = mProp.AdditionalCondition;
                    if (string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature != null) prop.OrigValueText = mProp.Temperature.ToString() + "°C";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(mProp.OrigValueText))
                {

                    if (!string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature != null) prop.OrigValueText = mProp.OrigValueText + " ;" + mProp.AdditionalCondition + " ;" + mProp.Temperature.ToString() + "°C";
                    if (!string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature == null) prop.OrigValueText =mProp.OrigValueText+ " ;" + mProp.AdditionalCondition;
                    if (string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature != null) prop.OrigValueText = mProp.OrigValueText + " ;" + mProp.Temperature.ToString() + "°C";
                }
                else
                {

                    if (!string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature != null) prop.OrigValueText = mProp.AdditionalCondition + " ;" + mProp.Temperature.ToString() + "°C";
                    if (!string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature == null) prop.OrigValueText =  mProp.AdditionalCondition;
                    if (string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature != null) prop.OrigValueText = mProp.Temperature.ToString() + "°C";
                }
                if (mProp.OrigValue == null)
                {
                    if (mProp.OrigValueMin != null && mProp.OrigValueMax != null) prop.OrigValue = ((double)mProp.OrigValueMin).ToString("0.####") + "-" + ((double)mProp.OrigValueMax).ToString("0.##");
                    if (mProp.OrigValueMin != null && mProp.OrigValueMax == null) prop.OrigValue = "&GreaterEqual;" + ((double)mProp.OrigValueMin).ToString("0.####");
                    if (mProp.OrigValueMax != null && mProp.OrigValueMin == null) prop.OrigValue = "&le;" + ((double)mProp.OrigValueMax).ToString("0.####");
                    if (mProp.OrigValueMax == null && mProp.OrigValueMin == null) prop.OrigValue = "";
                }
                else
                {
                    prop.OrigValue = mProp.OrigValue.ToString();
                }
            }
          
                if (mProp.ConvValue == null)
                {
                    if (mProp.ConvValueMin != null && mProp.ConvValueMax != null) prop.ConvValue = ((double)mProp.ConvValueMin).ToString("0.####") + "-" + ((double)mProp.ConvValueMax).ToString("0.##");
                    if (mProp.ConvValueMin != null && mProp.ConvValueMax == null) prop.ConvValue = "&le;" + ((double)mProp.ConvValueMin).ToString("0.####");
                    if (mProp.ConvValueMax != null && mProp.ConvValueMin == null) prop.ConvValue = "&GreaterEqual;" + ((double)mProp.ConvValueMax).ToString("0.####");
                    if (mProp.ConvValueMax == null && mProp.ConvValueMin == null) prop.ConvValue = "";
                }
                else
                {
                    prop.ConvValue = mProp.ConvValue.ToString();
                }
          
                return prop;
            }
        }
    }

