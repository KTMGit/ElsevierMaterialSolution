
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Exporter.Formats
{
    public class ESIPAMCrash
    {

        private const string THMAT = "THMAT / ";
        private const string NAME = "NAME ";
        private const string MASS_DENSITY = "MASS_DENSITY    CONSTANT";
        private const string SPECIFIC_HEAT = "SPECIFIC_HEAT   CONSTANT";
        private const string CONDUCTIVITY = "CONDUCTIVITY    ISOTROPIC    CONSTANT";
        private const string END_THMAT = "END_THMAT";


        private const string MATER = "MATER / ";


        private const string VALUE101 = "       101";
        private const string VALUE212 = "     212";
        private const string ZERO = "       0.0";
        private const string FUNCT101 = "FUNCT /      101       0     1.0     1.0";
        private const string BARS = "NAME H1050;_Bars;_(tensile) ";
        private const string END = "                END";


        public static string FillESIPamCrash(int vkKey, string standard, string name, IList<ExportPropertyGeneral> properties)
        {
                       
            ExportPropertyGeneral propertyDensity = (from u in properties where u.Type == PropertyTypeEnum.PhysicalDensity select u).FirstOrDefault();
            ExportPropertyGeneral propertySpecificHeat = (from u in properties where u.Type == PropertyTypeEnum.PhysicalSpecificThermalCapacity select u).FirstOrDefault();
            ExportPropertyGeneral propertyThermalConductivity = (from u in properties where u.Type == PropertyTypeEnum.PhysicalThermalConductivity select u).FirstOrDefault();
            ExportPropertyGeneral propertyModulusOfElasticity = (from u in properties where u.Type == PropertyTypeEnum.PhysicalModulusOfElasticity select u).FirstOrDefault();
            ExportPropertyGeneral propertyPoisson = (from u in properties where u.Type == PropertyTypeEnum.PhysicalPoissonCoefficient select u).FirstOrDefault();
            ExportPropertyGeneral propertyStressStrain = (from u in properties where u.Type == PropertyTypeEnum.StressStrain select u).FirstOrDefault() ;

            string exportString = "";
            if (propertyThermalConductivity!=null || propertyDensity != null || propertySpecificHeat !=null)
            {
                //StringBuilder sb  = new s
                exportString = exportString + THMAT + vkKey.ToString().PadLeft(8) + System.Environment.NewLine;
                exportString = exportString + NAME + name.Replace(" ", "_") + "_" + standard + System.Environment.NewLine;


                if (propertyDensity != null)
                {
                  
                    exportString = exportString + MASS_DENSITY + System.Environment.NewLine + (String.Format(CultureInfo.InvariantCulture, "{0:0.###E+0}", Double.Parse(propertyDensity.Value) / 1000000)).PadLeft(10) + System.Environment.NewLine;
                }


                if (propertySpecificHeat != null)
                {
                    exportString = exportString + SPECIFIC_HEAT + System.Environment.NewLine + propertySpecificHeat.Value.PadLeft(10) + System.Environment.NewLine;
                }


                if (propertyThermalConductivity != null)
                {
                    exportString = exportString + CONDUCTIVITY + System.Environment.NewLine + propertyThermalConductivity.Value.PadLeft(10) + System.Environment.NewLine;
                }

                exportString = exportString + END_THMAT + System.Environment.NewLine;
                exportString = exportString + System.Environment.NewLine;
            }
         



            if (propertyModulusOfElasticity != null || propertyPoisson != null || propertyDensity != null || propertyStressStrain != null)
            {

                exportString = exportString + MATER + vkKey.ToString().PadLeft(8) + VALUE212 + (propertyDensity != null ? (String.Format(CultureInfo.InvariantCulture, "{0:0.###e+00}", Double.Parse(propertyDensity.Value) / 1000000)).PadLeft(16) : "0.0".PadLeft(16)) + System.Environment.NewLine;
                exportString = exportString + System.Environment.NewLine;

                exportString = exportString + NAME + name.Replace(" ", "_") + "_" + standard + System.Environment.NewLine;

                exportString = exportString + (propertyModulusOfElasticity != null ? Double.Parse(propertyModulusOfElasticity.Value) * 1000 + ".0" : "0.0".PadLeft(10)).PadLeft(10) + (propertyPoisson != null ? propertyPoisson.Value : "0.0").PadLeft(10) + (propertyStressStrain != null ? "CURVE".PadLeft(10) : "0.0").PadLeft(10) + "         0       0.0" + System.Environment.NewLine;
                exportString = exportString + System.Environment.NewLine;
                exportString = exportString + System.Environment.NewLine;
                exportString = exportString + System.Environment.NewLine;
                exportString = exportString + (propertyStressStrain != null ?  VALUE101: "       0") + System.Environment.NewLine;
                exportString = exportString +ZERO+ System.Environment.NewLine;

                if (propertyStressStrain != null)
                {


                    exportString = exportString + FUNCT101 + System.Environment.NewLine;

                    exportString = exportString + BARS + System.Environment.NewLine;
                    string val = "";
                    foreach (var item in propertyStressStrain.Values)
                    {
                        string x;
                        if (item.X == "0")
                        {
                            x = "0.0".PadLeft(32);
                        }
                        else
                        {
                            x = item.X.PadLeft(32);
                        }
                        val = val + x + (item.Y.ToString() + ".0").PadLeft(16) + System.Environment.NewLine;
                    }
                    exportString = exportString + val + END;
                }

            }
      
            return exportString;
        }

    }
}