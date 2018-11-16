using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Exporter.Formats
{

    public class NXSiemens
    {
     
        private const string REVISION = "** Revision - 0";
        private const string UNITSYSTEM = "***** UNIT SYSTEM******";
        /// <summary>
        /// Fills the nx siemens.
        /// </summary>
        /// <returns></returns>
        public static XDocument FillNXSiemens(string standard, string name, bool isSteel, PlusGroup materialGroup, IList<ExportPropertyGeneral> properties)
        {          
        
            XDocument xml = new XDocument();   
            XElement MatML_Doc = new XElement("MatML_Doc");
            xml.Add(MatML_Doc);
            XElement Material = new XElement("Material");
            MatML_Doc.Add(Material);
            XElement BulkDetails = new XElement("BulkDetails");
            Material.Add(BulkDetails);
            XElement Name = new XElement("Name");
            Name.Value = name;
            BulkDetails.Add(Name);            
            XElement Class = new XElement("Class");
            XElement ClassName = new XElement("Name");
           
          
            if (isSteel)
            {
                ClassName.Value = "METAL";
            }
            else
            {
                //TODO:-Bice izmapirani Ceramics, COMPOSITES

                if (materialGroup == PlusGroup.Polymers)
                {
                    ClassName.Value = "POLYMERS";
                }
                //if (materialGroup == PlusGroup.Ceramics)
                //{
                //    ClassName.Value = "CERAMICS";
                //}
                //if (materialGroup == PlusGroup.Composites)
                //{
                //    ClassName.Value = "COMPOSITES";
                //}
                
            }
            Class.Add(ClassName);
            BulkDetails.Add(Class);           
            XElement Source = new XElement("Source");
            Source.SetAttributeValue("source", "");      
            BulkDetails.Add(Source);
            FillPropertyData(BulkDetails, properties);
            XElement Metadata = new XElement("Metadata");
            MatML_Doc.Add(Metadata);
            FillMetaData(Metadata);
            return xml;
        }
        /// <summary>
        /// Fills the property data.
        /// </summary>
        /// <param name="BulkDetails">The bulk details.</param>
        private static void FillPropertyData(XElement BulkDetails, IList<ExportPropertyGeneral> properties)
        {

            CreatePropertyData(BulkDetails, "IsotropicMaterial", "string", "Material Type");
            CreatePropertyData(BulkDetails, "3.0", "string", "Version");
            //zakucala metal
            CreatePropertyData(BulkDetails, "METAL", "string", "Category");
         
            string value;
            ExportPropertyGeneral property;
            //Physical Density
            if ((from u in properties where u.Type == PropertyTypeEnum.PhysicalDensity  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.PhysicalDensity select u).FirstOrDefault();
              value = (double.Parse(property.Value) / 1000000).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
              CreatePropertyData(BulkDetails, value, "exponential", "Mass Density (RHO)_6");
            }
            //Youngs modul
            if ((from u in properties where u.Type == PropertyTypeEnum.PhysicalModulusOfElasticity  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.PhysicalModulusOfElasticity select u).FirstOrDefault();
                value = (double.Parse(property.Value) * 1000000).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "Youngs Modulus (E)_31");
            }
            //Poisson ratio
            if ((from u in properties where u.Type == PropertyTypeEnum.PhysicalPoissonCoefficient  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.PhysicalPoissonCoefficient select u).FirstOrDefault();
                value = (double.Parse(property.Value)).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "PoissonsRatio");
            }
            //Yield Strength_32
            if ((from u in properties where u.Type == PropertyTypeEnum.MechanicalYield  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.MechanicalYield select u).FirstOrDefault();
                value = (double.Parse(property.Value) * 1000).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "Yield Strength_32");
            }
            //Ultimate Tensile Strength_33
            if ((from u in properties where u.Type == PropertyTypeEnum.MechanicalTensile  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.MechanicalTensile select u).FirstOrDefault();
                value = (double.Parse(property.Value) * 1000).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "Ultimate Tensile Strength_33");
            }
            //Fatigue Strength Coefficient_20
            if ((from u in properties where u.Type == PropertyTypeEnum.FatigueStrengthCoefficient  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.FatigueStrengthCoefficient select u).FirstOrDefault();
                value = (double.Parse(property.Value) * 1000).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "Fatigue Strength Coefficient_20");
            }
            //FatigueStrengthExp
            if ((from u in properties where u.Type == PropertyTypeEnum.FatigueStrengthExponent  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.FatigueStrengthExponent select u).FirstOrDefault();
                value = (double.Parse(property.Value)).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "FatigueStrengthExp");
            }
            //FatigueDuctCoeff
            if ((from u in properties where u.Type == PropertyTypeEnum.FatigueDuctilityCoefficient  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.FatigueDuctilityCoefficient select u).FirstOrDefault();

                value = (double.Parse(property.Value)).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "FatigueDuctCoeff");
            }
            //FatigueDuctExp
            if ((from u in properties where u.Type == PropertyTypeEnum.FatigueDuctilityExponent  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.FatigueDuctilityExponent select u).FirstOrDefault();
                value = (double.Parse(property.Value)).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "FatigueDuctExp");
            }
            //CyclicStrengthCoeff
            if ((from u in properties where u.Type == PropertyTypeEnum.FatigueCyclicStrengthExponent  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.FatigueCyclicStrengthExponent select u).FirstOrDefault();
                value = (double.Parse(property.Value) * 1000).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "CyclicStrengthCoeff");
            }
            //CyclicStrainHardeningExp
            if ((from u in properties where u.Type == PropertyTypeEnum.FatigueCyclicStrengthExponent  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.FatigueCyclicStrengthExponent select u).FirstOrDefault();
                value = (double.Parse(property.Value)).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "CyclicStrainHardeningExp");
            }
            //Thermal Expansion (A)_34
            if ((from u in properties where u.Type == PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion select u).FirstOrDefault();
                value = (double.Parse(property.Value) / 1000000).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "Thermal Expansion (A)_34");
            }
            //Thermal Conductivity (K)_35
            if ((from u in properties where u.Type == PropertyTypeEnum.PhysicalThermalConductivity  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.PhysicalThermalConductivity select u).FirstOrDefault();
                value = (double.Parse(property.Value) * 1000).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "Thermal Conductivity (K)_35");
            }
            //Specific Heat (CP)_23
            if ((from u in properties where u.Type == PropertyTypeEnum.PhysicalSpecificThermalCapacity  select u).Any())
            {
                property = (from u in properties where u.Type == PropertyTypeEnum.PhysicalSpecificThermalCapacity select u).FirstOrDefault();
                value = (double.Parse(property.Value) * 1000000).ToString("e6", System.Globalization.CultureInfo.InvariantCulture);
                CreatePropertyData(BulkDetails, value, "exponential", "Specific Heat (CP)_23");
            }
        }

        /// <summary>
        /// Creates the property data.
        /// </summary>
        /// <param name="BulkDetails">The bulk details.</param>
        /// <param name="dataValue">The data value.</param>
        /// <param name="dataFormat">The data format.</param>
        /// <param name="property">The property.</param>
        private static void CreatePropertyData(XElement BulkDetails, string dataValue, string dataFormat, string property)
        {
            XElement PropertyData = new XElement("PropertyData");
            PropertyData.SetAttributeValue("property", property);
            XElement Data = new XElement("Data");
            Data.SetAttributeValue("format", dataFormat);
            Data.Value = dataValue;
            PropertyData.Add(Data);            
            BulkDetails.Add(PropertyData);
        }
        /// <summary>
        /// Fills the meta data.
        /// </summary>
        /// <param name="Metadata">The metadata.</param>
        private static void FillMetaData(XElement Metadata)
        {
            IList<NXSiemensParameterDetail> ListOfParametersDetails = FillListOFParameterDetails();
    
            foreach (var item in ListOfParametersDetails)
            {
                   AddMetaDataXMLNode(Metadata, item);
            }
        }

        /// <summary>
        /// Adds the meta data XML node.
        /// </summary>
        /// <param name="Metadata">The metadata.</param>
        /// <param name="parameterDetail">The parameter detail.</param>
        private static void AddMetaDataXMLNode(XElement Metadata, NXSiemensParameterDetail parameterDetail)
        {
            XElement ParameterDetails = new XElement("PropertyDetails");           
            ParameterDetails.SetAttributeValue("id", parameterDetail.IdString);
            XElement Name = new XElement("Name");
            Name.Value = " " + parameterDetail.Name + " ";
            if (parameterDetail.Units != null && parameterDetail.Units.Count > 0)
            {
                XElement Units = new XElement("Units");
                Units.SetAttributeValue("name", parameterDetail.UnitsName);
                Units.SetAttributeValue("description", parameterDetail.UnitsDescription);
                foreach (var item in parameterDetail.Units)
                {
                    if (item != null)
                    {
                        XElement Unit = new XElement("Unit");
                        if (item.Attribite != null)
                        {
                            Unit.SetAttributeValue(item.Attribite, item.AttribiteValue);
                        }
                        XElement NameOfUnit = new XElement("Name");
                        NameOfUnit.Value = " " + item.Name + " ";
                        Unit.Add(NameOfUnit);
                        Units.Add(Unit);
                    }
                    else
                    {                    
                        XElement Unitless = new XElement("Unitless");
                        Units.Add(Unitless);   
                    }                    
                }
                    ParameterDetails.Add(Name);
                    ParameterDetails.Add(Units);               
               
            }
            else
            {
                    ParameterDetails.Add(Name);
                    XElement Unitless = new XElement("Unitless");
                    ParameterDetails.Add(Unitless);                
            }

            Metadata.Add(ParameterDetails);
        }

        /// <summary>
        /// Fills the list of parameter details.
        /// </summary>
        /// <returns></returns>
        private static IList<NXSiemensParameterDetail> FillListOFParameterDetails()
        {
            IList<NXSiemensParameterDetail> listOfParameters = new List<NXSiemensParameterDetail>();
            NXSiemensParameterDetail parameterDetail = null;
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.MechanicalYield, "Yield Strength_32", "Yield", new List<NXSiemensUnit> { new NXSiemensUnit() { Name = "mN/mm^2(kPa)" } }, "mN/mm^2(kPa)", "milliNewtons per millimeter squared");
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.MechanicalTensile, "Ultimate Tensile Strength_33","UltTensile", new List<NXSiemensUnit> { new NXSiemensUnit() { Name = "mN/mm^2(kPa)" } }, "mN/mm^2(kPa)", "milliNewtons per millimeter squared");
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.MechanicalTensile, "Thermal Expansion (A)_34", "ThermalExpansion", new List<NXSiemensUnit> { new NXSiemensUnit() { Name = "1/C" } }, "1/C", "Expansion coefficient per degree Celsius");
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalThermalConductivity, "Thermal Conductivity (K)_35", "ThermalConductivity", new List<NXSiemensUnit> { new NXSiemensUnit() { Name = "microW/mm-C" } }, "microW/mm-C", "microWatts per millimeter per degree Centrigade");
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "Youngs Modulus (E)_31", "YoungsModulus", new List<NXSiemensUnit> { new NXSiemensUnit() { Name = "mN/mm^2(kPa)" } }, "mN/mm^2(kPa)", "milliNewtons per millimeter squared");
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "Specific Heat (CP)_23", "SpecificHeat", new List<NXSiemensUnit> { new NXSiemensUnit() { Name = "microJ/kg-K" } }, "microJ/kg-K", "microJoules per kilogram per degree Kelvin");
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "Mass Density (RHO)_6", "MassDensity", new List<NXSiemensUnit> { new NXSiemensUnit() { Name = "kg/mm^3" } }, "kg/mm^3", "kilograms per millimeter cubed");
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "Material Type", "Material Type", new List<NXSiemensUnit> {null}, null, null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "Version", "Version", new List<NXSiemensUnit> { null }, null, null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "Category", "Category", new List<NXSiemensUnit> { null }, null, null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "PoissonsRatio", "PoissonsRatio", new List<NXSiemensUnit> { null }, null, null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "FatigueStrengthExp", "FatigueStrengthExp", new List<NXSiemensUnit> { null }, null, null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "FatigueDuctCoeff", "FatigueDuctCoeff", new List<NXSiemensUnit> { null }, null, null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "FatigueDuctExp", "FatigueDuctExp", new List<NXSiemensUnit> { null }, null, null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "CyclicStrainHardeningExp", "CyclicStrainHardeningExp", new List<NXSiemensUnit> { null }, null, null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "Fatigue Strength Coefficient_20", "FatigueStrengthCoeff", new List<NXSiemensUnit> { new NXSiemensUnit() { Name = "mN/mm^2(kPa)" } }, "mN/mm^2(kPa)", "milliNewtons per millimeter squared");
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(PropertyTypeEnum.PhysicalModulusOfElasticity, "CyclicStrengthCoeff", "CyclicStrengthCoeff", new List<NXSiemensUnit> { new NXSiemensUnit() {  Name = "mN/mm^2(kPa)" } }, "mN/mm^2(kPa)", "milliNewtons per millimeter squared");
            listOfParameters.Add(parameterDetail);
            return listOfParameters;

        }

        /// <summary>
        /// Creates the parameter detail.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="idPrefix">The identifier prefix.</param>
        /// <param name="Name">The name.</param>
        /// <param name="units">The units.</param>
        /// <param name="unitsName">Name of the units.</param>
        /// <param name="unitsDescription">The units description.</param>
        /// <returns></returns>
        private static NXSiemensParameterDetail CreateParameterDetail(PropertyTypeEnum type, string idPrefix,  string Name, IList<NXSiemensUnit> units, string unitsName, string unitsDescription)
        {
            NXSiemensParameterDetail parameterDetail = new NXSiemensParameterDetail();         
            parameterDetail.IdPrefix = idPrefix;
            parameterDetail.Type = type;
            parameterDetail.UnitsName = unitsName;
            parameterDetail.UnitsDescription = unitsDescription;
            parameterDetail.IdString = idPrefix;
            parameterDetail.Name = Name;
            if (units != null && units.Count > 0)
            {
                parameterDetail.Units = units;
            }
            return parameterDetail;
        }
    }
    /// <summary>
    /// Parameter details
    /// </summary>
    public class NXSiemensParameterDetail
    {
        public PropertyTypeEnum Type { get; set; }
        public int Id { get; set; }
        public string IdPrefix { get; set; }
        public string IdString { get; set; }
        public string Name { get; set; }
        public string UnitsName { get; set; }
        public string UnitsDescription { get; set; }
        public IList<NXSiemensUnit> Units { get; set; }

    }
    /// <summary>
    /// Unit
    /// </summary>
    public class NXSiemensUnit
    {
        public string Name { get; set; }
        public string Attribite { get; set; }
        public string AttribiteValue { get; set; }
    }

}


