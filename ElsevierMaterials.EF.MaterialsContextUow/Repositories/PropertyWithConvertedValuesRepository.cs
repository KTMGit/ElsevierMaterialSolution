using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.Models;
using System.Data.Entity;
using System.Linq;
using ElsevierMaterials.Models.Domain.Property;

namespace ElsevierMaterials.EF.MaterialsContextUow.Repositories
{
    //TODO: Napravi interfejs za ProeprtyRepository
    public class PropertyWithConvertedValuesRepository : BaseRepository<PropertyWithConvertedValues>, IPropertyWithConvertedValuesRepository
    {
        public PropertyWithConvertedValuesRepository(DbContext dbContext)
            : base(dbContext)
        {
        }
                     

        public Property GetPropertyInfoForMaterialForSelectedMetric(string name, int materialId, int subgroupId, int rowId, int propertyId, int valueId, int type, string unitName)
        {   
            switch (type)
            {
                case 1:
                    return FillPropertyOriginValueAndUnit(name, materialId, subgroupId, rowId, propertyId, valueId, unitName);
                case 2:
                    return FillPropertyDefaultValueAndUnit(name, materialId, subgroupId, rowId, propertyId, valueId);
                case 3:
                    return FillPropertyUsCustomaryValueAndUnit(name, materialId, subgroupId, rowId, propertyId, valueId);
                default:
                    return null;
            }
        }

        private Property FillPropertyUsCustomaryValueAndUnit(string name, int materialId, int subgroupId, int rowId, int propertyId, int valueId)
        {



            //TODO: DefaultUnitId
            Property prop = new Property()
            {
                PropertyId = propertyId,
                // SourcePropertyId = 0, SourceId = 1, DefaultUnitId = 1,                
                MaterialId = materialId,
                SubgroupId = subgroupId,
                RowId = rowId,
                ValueId = valueId

            };



            PropertyWithConvertedValues property = DataSet.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.RowId == rowId && m.PropertyId == propertyId && m.ValueId == valueId).FirstOrDefault();

            prop.PropertyName = property.PropertyName;
            prop.OrigUnit = property.UsUnit;


            if (property.UsValue == null && property.UsValueMax == null && property.UsValueMin == null)
            {
                if (!string.IsNullOrEmpty(property.OrigValueText))
                {
                    prop.OrigValue = property.OrigValueText;

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.AdditionalCondition + " ;" + property.Temperature.ToString() + "°C";
                    }

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature == null)
                    {
                        prop.OrigValueText = property.AdditionalCondition;
                    }

                    if (string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.Temperature.ToString() + "°C";
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(property.OrigValueText))
                {

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.OrigValueText + " ;" + property.AdditionalCondition + " ;" + property.Temperature.ToString() + "°C";
                    }

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature == null)
                    {
                        prop.OrigValueText = property.OrigValueText + " ;" + property.AdditionalCondition;
                    }

                    if (string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.OrigValueText + " ;" + property.Temperature.ToString() + "°C";
                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.AdditionalCondition + " ;" + property.Temperature.ToString() + "°C";
                    }

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature == null)
                    {
                        prop.OrigValueText = property.AdditionalCondition;
                    }

                    if (string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.Temperature.ToString() + "°C";
                    }

                }

                if (property.UsValue == null)
                {
                    if (property.UsValueMin != null && property.UsValueMax != null)
                    {
                        prop.OrigValue = ((double)property.UsValueMin).ToString("0.####") + "-" + ((double)property.UsValueMax).ToString("0.##");
                    }

                    if (property.UsValueMin != null && property.UsValueMax == null)
                    {
                        prop.OrigValue = "&GreaterEqual;" + ((double)property.UsValueMin).ToString("0.####");
                    }

                    if (property.UsValueMax != null && property.UsValueMin == null)
                    {
                        prop.OrigValue = "&le;" + ((double)property.UsValueMax).ToString("0.####");
                    }

                    if (property.UsValueMax == null && property.UsValueMin == null)
                    {
                        prop.OrigValue = "";
                    }
                }
                else
                {
                    prop.OrigValue = property.UsValue.ToString();
                }
            }

            if (property.UsValue == null)
            {
                if (property.UsValueMin != null && property.UsValueMax != null)
                {
                    prop.ConvValue = ((double)property.UsValueMin).ToString("0.####") + "-" + ((double)property.UsValueMax).ToString("0.##");
                }
                if (property.UsValueMin != null && property.UsValueMax == null)
                {
                    prop.ConvValue = "&le;" + ((double)property.UsValueMin).ToString("0.####");
                }
                if (property.UsValueMax != null && property.UsValueMin == null)
                {
                    prop.ConvValue = "&GreaterEqual;" + ((double)property.UsValueMax).ToString("0.####");
                }
                if (property.UsValueMax == null && property.UsValueMin == null)
                {
                    prop.ConvValue = "";
                }
            }
            else
            {
                prop.ConvValue = property.ConvValue.ToString();
            }

            return prop;
        }

        private Property FillPropertyDefaultValueAndUnit(string name, int materialId, int subgroupId, int rowId, int propertyId, int valueId)
        {
         

            //TODO: DefaultUnitId
            Property prop = new Property()
            {
                PropertyId = propertyId,
                // SourcePropertyId = 0, SourceId = 1, DefaultUnitId = 1,                
                MaterialId = materialId,
                SubgroupId = subgroupId,
                RowId = rowId,
                ValueId = valueId

            };

       

            PropertyWithConvertedValues property = DataSet.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.RowId == rowId && m.PropertyId == propertyId && m.ValueId == valueId).FirstOrDefault();

            prop.PropertyName = property.PropertyName;
            prop.OrigUnit = property.DefaultUnit;


            if (property.ConvValue == null && property.ConvValueMax == null && property.ConvValueMin == null)
            {
                if (!string.IsNullOrEmpty(property.OrigValueText))
                {
                    prop.OrigValue = property.OrigValueText;

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.AdditionalCondition + " ;" + property.Temperature.ToString() + "°C";
                    }

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature == null)
                    {
                        prop.OrigValueText = property.AdditionalCondition;
                    }

                    if (string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.Temperature.ToString() + "°C";
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(property.OrigValueText))
                {

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.OrigValueText + " ;" + property.AdditionalCondition + " ;" + property.Temperature.ToString() + "°C";
                    }

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature == null)
                    {
                        prop.OrigValueText = property.OrigValueText + " ;" + property.AdditionalCondition;
                    }

                    if (string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.OrigValueText + " ;" + property.Temperature.ToString() + "°C";
                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.AdditionalCondition + " ;" + property.Temperature.ToString() + "°C";
                    }

                    if (!string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature == null)
                    {
                        prop.OrigValueText = property.AdditionalCondition;
                    }

                    if (string.IsNullOrEmpty(property.AdditionalCondition) && property.Temperature != null)
                    {
                        prop.OrigValueText = property.Temperature.ToString() + "°C";
                    }

                }

                if (property.ConvValue == null)
                {
                    if (property.ConvValueMin != null && property.ConvValueMax != null)
                    {
                        prop.OrigValue = ((double)property.ConvValueMin).ToString("0.####") + "-" + ((double)property.ConvValueMax).ToString("0.##");
                    }

                    if (property.ConvValueMin != null && property.ConvValueMax == null)
                    {
                        prop.OrigValue = "&GreaterEqual;" + ((double)property.ConvValueMin).ToString("0.####");
                    }

                    if (property.ConvValueMax != null && property.ConvValueMin == null)
                    {
                        prop.OrigValue = "&le;" + ((double)property.ConvValueMax).ToString("0.####");
                    }

                    if (property.ConvValueMax == null && property.ConvValueMin == null)
                    {
                        prop.OrigValue = "";
                    }
                }
                else
                {
                    prop.OrigValue = property.ConvValue.ToString();
                }
            }

            if (property.ConvValue == null)
            {
                if (property.ConvValueMin != null && property.ConvValueMax != null)
                {
                    prop.ConvValue = ((double)property.ConvValueMin).ToString("0.####") + "-" + ((double)property.ConvValueMax).ToString("0.##");
                }
                if (property.ConvValueMin != null && property.ConvValueMax == null)
                {
                    prop.ConvValue = "&le;" + ((double)property.ConvValueMin).ToString("0.####");
                }
                if (property.ConvValueMax != null && property.ConvValueMin == null)
                {
                    prop.ConvValue = "&GreaterEqual;" + ((double)property.ConvValueMax).ToString("0.####");
                }
                if (property.ConvValueMax == null && property.ConvValueMin == null)
                {
                    prop.ConvValue = "";
                }
            }
            else
            {
                prop.ConvValue = property.ConvValue.ToString();
            }

            return prop;
        }

        private Property FillPropertyOriginValueAndUnit(string name, int materialId, int subgroupId, int rowId, int propertyId, int valueId, string unitName)
        {
            Property prop = new Property() { PropertyId = propertyId, SourcePropertyId = 0, SourceId = 1, DefaultUnitId = 1, MaterialId = materialId, SubgroupId = subgroupId, RowId = rowId, ValueId = valueId };

            prop.PropertyName = name;
            PropertyWithConvertedValues mProp = DataSet.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.RowId == rowId && m.PropertyId == propertyId && m.ValueId == valueId).FirstOrDefault();


            prop.OrigUnit = unitName;
            if (mProp.OrigValue == null && mProp.OrigValueMax == null && mProp.OrigValueMin == null)
            {
                if (!string.IsNullOrEmpty(mProp.OrigValueText))
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
                    if (!string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature == null) prop.OrigValueText = mProp.OrigValueText + " ;" + mProp.AdditionalCondition;
                    if (string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature != null) prop.OrigValueText = mProp.OrigValueText + " ;" + mProp.Temperature.ToString() + "°C";
                }
                else
                {

                    if (!string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature != null) prop.OrigValueText = mProp.AdditionalCondition + " ;" + mProp.Temperature.ToString() + "°C";
                    if (!string.IsNullOrEmpty(mProp.AdditionalCondition) && mProp.Temperature == null) prop.OrigValueText = mProp.AdditionalCondition;
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

