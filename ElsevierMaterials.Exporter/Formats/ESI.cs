using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ElsevierMaterials.Exporter.Models;
using Ionic.Zip;


namespace ElsevierMaterials.Exporter.Formats
{
    public class ESI
    {

        /// <summary>
        /// Fills the XML file new.
        /// </summary>
        /// <returns></returns>
        public static XDocument FillXmlFileNew(string name, IList<Models.ExportPropertyGeneral> properties)
        {        
       
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", null), null);           
            XElement MTXML = new XElement("MTXML");
            xml.Add(MTXML);
            XElement Records = new XElement("Records");
            MTXML.Add(Records);

          
            //KtmDataContext db = new KtmDataContext();
            //string baseElement = (from u in db.mats where u.vk_key == export.VkKey select u.BaseElement).FirstOrDefault();
            //TODO:+Testirati za materijale, base element nemamo info

            Records.SetAttributeValue("FolderName", "");
            XElement Record = new XElement("Record");
            Records.Add(Record);
            Record.SetAttributeValue("ID", (name + "-Table1"));
            XElement datasets = new XElement("Datasets");
            Record.Add(datasets);
            XElement dataset = new XElement("Dataset");
            datasets.Add(dataset);
            dataset.SetAttributeValue("IDref", "Material_type");
            XElement str = new XElement("String");
            dataset.Add(str);
            XElement val = new XElement("Value");
            val.Value = "T";
            str.Add(val);
            XElement Models = new XElement("Models");
            Record.Add(Models);
            AddChemicalNew(properties, Models, "");
            AddStressNew(properties, Models);      

            return xml;
        }
        /// <summary>
        /// Adds the stress new.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <param name="Models">The models.</param>
        private static void AddStressNew(IList<Models.ExportPropertyGeneral> properties, XElement Models)
        {
            XElement stressModel = new XElement("Model");
            stressModel.SetAttributeValue("IDref", "Stress");
            XElement SubModels = new XElement("SubModels");
            stressModel.Add(SubModels);
            XElement ModelInSubModel = new XElement("Model");
            SubModels.Add(ModelInSubModel);
            ModelInSubModel.SetAttributeValue("Active", "true");
            ModelInSubModel.SetAttributeValue("IDref", "Stress_Elasto_Plastic");

            bool add = false;
            if (properties.Where(p => p.Type == PropertyTypeEnum.PhysicalModulusOfElasticity).Any())
            {
                XElement DatasetYoung = new XElement("Dataset");
                DatasetYoung.SetAttributeValue("IDref", "Youngs_modulus");
                XElement FloatYoung = new XElement("Float");
                XElement CurveYoung = new XElement("Curve");
                CurveYoung.SetAttributeValue("Label", "F(T)");
                XElement DataAxisYoung = new XElement("DataAxis");
                XElement ValueListYoung = new XElement("ValueList");
                XElement ValueListYoungT = new XElement("ValueList");
                foreach (var property in properties.Where(p => p.Type == PropertyTypeEnum.PhysicalModulusOfElasticity))
                {
                    XElement Value = new XElement("Value");
                    Value.Value = (double.Parse(property.Value) * 1000).ToString();
                    ValueListYoung.Add(Value);
                    XElement ValueT = new XElement("Value");
                    ValueT.Value = property.Temperature.ToString();
                    ValueListYoungT.Add(ValueT);
                }
                XElement UnitsYoung = new XElement("Units");
                XElement UnitYoung = new XElement("Unit");
                UnitYoung.SetAttributeValue("Name", "MPa");
                UnitsYoung.Add(UnitYoung);

                XElement ParameterAxisYoung = new XElement("ParameterAxis");
                ParameterAxisYoung.SetAttributeValue("IDref", "TEMP");
                if (properties.Where(p => p.Type == PropertyTypeEnum.PhysicalModulusOfElasticity ).Any())
                {
                    ParameterAxisYoung.Add(ValueListYoungT);
                    CurveYoung.Add(DataAxisYoung);
                    CurveYoung.Add(ParameterAxisYoung);
                    DataAxisYoung.Add(ValueListYoung);
                    DataAxisYoung.Add(UnitsYoung);

                    FloatYoung.Add(CurveYoung);
                    DatasetYoung.Add(FloatYoung);
                    ModelInSubModel.Add(DatasetYoung);
                    XElement UnitsYoungT = new XElement("Units");
                    XElement UnitYoungT = new XElement("Unit");
                    UnitYoungT.SetAttributeValue("Name", "C");
                    UnitsYoungT.Add(UnitYoungT);
                    ParameterAxisYoung.Add(UnitsYoungT);

                    add = true;
                }
            }      

            // Yield stress
            if (properties.Where(p => p.Type == PropertyTypeEnum.MechanicalYield ).Any())
            {
                XElement DatasetYield = new XElement("Dataset");
                DatasetYield.SetAttributeValue("IDref", "Yield_stress");
                XElement FloatYield = new XElement("Float");
                XElement ConstantYield = new XElement("Constant");
                XElement ValueYield = new XElement("Value");
                Models.ExportPropertyGeneral elem = properties.Where(p => p.Type == PropertyTypeEnum.MechanicalYield ).FirstOrDefault();
                ValueYield.Value = elem.Value;
                XElement UnitsYield = new XElement("Units");
                XElement UnitYield = new XElement("Unit");
                UnitYield.SetAttributeValue("Name", "MPa");
                UnitsYield.Add(UnitYield);
                ConstantYield.Add(ValueYield);
                ConstantYield.Add(UnitsYield);
                FloatYield.Add(ConstantYield);
                DatasetYield.Add(FloatYield);
                ModelInSubModel.Add(DatasetYield);
                add = true;
            }

            // tensile stress
            if (properties.Where(p => p.Type == PropertyTypeEnum.MechanicalTensile ).Any())
            {
                XElement DatasetTensile = new XElement("Dataset");
                DatasetTensile.SetAttributeValue("IDref", "Tensile_stress");
                XElement FloatTensile = new XElement("Float");
                XElement ConstantTensile = new XElement("Constant");
                XElement ValueTensile = new XElement("Value");
                Models.ExportPropertyGeneral elemTensile = properties.Where(p => p.Type == PropertyTypeEnum.MechanicalTensile ).FirstOrDefault();
                ValueTensile.Value = elemTensile.Value;
                XElement UnitsTensile = new XElement("Units");
                XElement UnitTensile = new XElement("Unit");
                UnitTensile.SetAttributeValue("Name", "MPa");
                UnitsTensile.Add(UnitTensile);
                ConstantTensile.Add(ValueTensile);
                ConstantTensile.Add(UnitsTensile);
                FloatTensile.Add(ConstantTensile);
                DatasetTensile.Add(FloatTensile);
                ModelInSubModel.Add(DatasetTensile);
                add = true;
            }

            // Elongation
            if (properties.Where(p => p.Type == PropertyTypeEnum.MechanicalElongation ).Any())
            {
                XElement DatasetElongation = new XElement("Dataset");
                DatasetElongation.SetAttributeValue("IDref", "Elongation");
                XElement FloatElongation = new XElement("Float");
                XElement ConstantElongation = new XElement("Constant");
                XElement ValueElongation = new XElement("Value");
                Models.ExportPropertyGeneral elemElongation = properties.Where(p => p.Type == PropertyTypeEnum.MechanicalElongation ).FirstOrDefault();
                ValueElongation.Value = elemElongation.Value;
                XElement UnitsElongation = new XElement("Units");
                XElement UnitElongation = new XElement("Unit");
                UnitsElongation.Add(UnitElongation);

                ConstantElongation.Add(ValueElongation);
                ConstantElongation.Add(UnitsElongation);
                FloatElongation.Add(ConstantElongation);
                DatasetElongation.Add(FloatElongation);
                ModelInSubModel.Add(DatasetElongation);
                add = true;
            }


            // Submodels
            XElement sepSubmodels = new XElement("SubModels");
            XElement sepModelSecant = new XElement("Model");
            sepModelSecant.SetAttributeValue("Active", "true");
            sepModelSecant.SetAttributeValue("IDref", "Stress_Elasto_Plastic_Secant");
            // sepSubmodels.Add(sepModelSecant);

            // termal expansion
            if (properties.Where(p => p.Type == PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion ).Any())
            {
                XElement DatasetThermal = new XElement("Dataset");
                sepModelSecant.Add(DatasetThermal);
                DatasetThermal.SetAttributeValue("IDref", "Thermal_expansion_secant");
                XElement FloatThermal = new XElement("Float");
                XElement CurveThermal = new XElement("Curve");
                CurveThermal.SetAttributeValue("Label", "F(T)");
                XElement DataAxisThermal = new XElement("DataAxis");
                XElement ValueListThermal = new XElement("ValueList");
                XElement ValueListThermalT = new XElement("ValueList");
                foreach (var property in properties.Where(p => p.Type == PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion ))
                {
                    XElement ValueThermal = new XElement("Value");
                    //ValueThermal.Value = property.Value;
                    ValueThermal.Value = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.###e-00}", double.Parse(property.Value) / 1000000);
                    ValueListThermal.Add(ValueThermal);
                    XElement ValueThermalT = new XElement("Value");
                    ValueThermalT.Value = property.Temperature.ToString();
                    ValueListThermalT.Add(ValueThermalT);
                }
                XElement UnitsThermal = new XElement("Units");
                XElement UnitThermal = new XElement("Unit");
                UnitThermal.SetAttributeValue("Name", "1/K");
                UnitsThermal.Add(UnitThermal);

                XElement UnitsThermalAxis = new XElement("Units");
                XElement UnitThermalAxis = new XElement("Unit");
                UnitThermalAxis.SetAttributeValue("Name", "C");
                UnitsThermalAxis.Add(UnitThermalAxis);


                XElement ParameterAxisThermal = new XElement("ParameterAxis");
                ParameterAxisThermal.SetAttributeValue("IDref", "TEMP");

                if (properties.Where(p => p.Type == PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion ).Any())
                {
                    ParameterAxisThermal.Add(ValueListThermalT);
                    ParameterAxisThermal.Add(UnitsThermalAxis);
                    CurveThermal.Add(DataAxisThermal);
                    CurveThermal.Add(ParameterAxisThermal);
                    DataAxisThermal.Add(ValueListThermal);
                    DataAxisThermal.Add(UnitsThermal);


                    FloatThermal.Add(CurveThermal);
                    DatasetThermal.Add(FloatThermal);
                    sepSubmodels.Add(sepModelSecant);
                    add = true;
                }
                XElement refTempDataset = new XElement("Dataset");
                sepModelSecant.Add(refTempDataset);
                refTempDataset.SetAttributeValue("IDref", "Thermal_expansion_secant_reference_temperature");
                XElement refTempFloat = new XElement("Float");
                refTempDataset.Add(refTempFloat);
                XElement refTempConstant = new XElement("Constant");
                refTempFloat.Add(refTempConstant);
                XElement refTempValue = new XElement("Value");
                refTempConstant.Add(refTempValue);
                refTempValue.Value = "20.0";
                XElement refTempUnitsThermalAxis = new XElement("Units");
                XElement refTempUnitThermalAxis = new XElement("Unit");
                refTempConstant.Add(refTempUnitsThermalAxis);
                refTempUnitThermalAxis.SetAttributeValue("Name", "C");
                refTempUnitsThermalAxis.Add(refTempUnitThermalAxis);
            }

            // Isotropic
            XElement sepModelIsotropic = new XElement("Model");
            sepModelIsotropic.SetAttributeValue("Active", "true");
            sepModelIsotropic.SetAttributeValue("IDref", "Stress_Elasto_Plastic_Isotropic");
            XElement sepModelIsotropicSubmodels = new XElement("SubModels");
            sepModelIsotropic.Add(sepModelIsotropicSubmodels);
            if (properties.Where(p => p.Type == PropertyTypeEnum.StressStrain ).Any())
            {
                XElement ssModel = new XElement("Model");
                ssModel.SetAttributeValue("Active", "true");
                ssModel.SetAttributeValue("IDref", "Stress_Elasto_Plastic_Isotropic_Table");
                sepModelIsotropicSubmodels.Add(ssModel);
                XElement ssDataset = new XElement("Dataset");
                ssModel.Add(ssDataset);
                ssDataset.SetAttributeValue("IDref", "MatmlProp_HARDENING_TABLE");
                XElement ssFloat = new XElement("Float");
                ssDataset.Add(ssFloat);
                XElement ssConstant = new XElement("Constant");
                ssFloat.Add(ssConstant);
                XElement refTempValue = new XElement("Value");
                ssConstant.Add(refTempValue);
                refTempValue.Value = "1";
                sepSubmodels.Add(sepModelIsotropic);
                add = true;
            }

            if (add)
            {
                Models.Add(stressModel);
                ModelInSubModel.Add(sepSubmodels);
            }
        }
        /// <summary>
        /// Adds the stress strain new.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <param name="Model">The model.</param>
        private static void AddStressStrainNew(IList<Models.ExportPropertyGeneral> properties, XElement Model)
        {
            if (properties.Where(p => p.Type == PropertyTypeEnum.StressStrain).Any())
            {
                XElement DatasetElongation = new XElement("Dataset");
                DatasetElongation.SetAttributeValue("IDref", "ATT_true_stress");
                XElement Float = new XElement("Float");
                XElement Curve = new XElement("Curve");
                Curve.SetAttributeValue("Label", "F(strain)");
                XElement DataAxis = new XElement("DataAxis");
                XElement ValueList = new XElement("ValueList");
                IList<Value> coordinates;
                coordinates = properties.Where(p => p.Type == PropertyTypeEnum.StressStrain).Select(u => u.Values).FirstOrDefault().ToList();
                foreach (var item in coordinates)
                {
                    XElement Value = new XElement("Value");
                    Value.Value = item.Y.ToString();
                    ValueList.Add(Value);
                }
                DataAxis.Add(ValueList);
                XElement Units = new XElement("Units");
                XElement Unit = new XElement("Unit");
                Unit.SetAttributeValue("Name", "MPa");
                Units.Add(Unit);
                DataAxis.Add(Units);
                Curve.Add(DataAxis);
                XElement ParameterAxis = new XElement("ParameterAxis");
                ParameterAxis.SetAttributeValue("IDref", "ATT_true_strain");
                ValueList = new XElement("ValueList");
                foreach (var item in coordinates)
                {
                    XElement Value = new XElement("Value");
                    Value.Value = item.X.ToString();
                    ValueList.Add(Value);
                }
                ParameterAxis.Add(ValueList);
                Curve.Add(ParameterAxis);
                Float.Add(Curve);
                DatasetElongation.Add(Float);
                Model.Add(DatasetElongation);
            }
        }
        /// <summary>
        /// Adds the chemical new.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <param name="Models">The models.</param>
        private static void AddChemicalNew(IList<Models.ExportPropertyGeneral> properties, XElement Models, string baseElement)
        {
            if (properties.Where(p => p.Type == PropertyTypeEnum.ChemicalCompositions ).Any())
            {
                XElement Model = new XElement("Model");
                Model.SetAttributeValue("IDref", "Composition");
                XElement SubModels = new XElement("SubModels");
                XElement ModelInSubModel = new XElement("Model");
                ModelInSubModel.SetAttributeValue("Active", "true");
                //KtmDataContext db = new KtmDataContext();              
                ModelInSubModel.SetAttributeValue("IDref", baseElement + "_Model");

                ExportPropertyGeneral property = properties.Where(p => p.Type == PropertyTypeEnum.ChemicalCompositions ).FirstOrDefault();
           

                foreach (var item in property.Values)
                {
                    XElement DatasetSi = new XElement("Dataset");
                    DatasetSi.SetAttributeValue("IDref", item.X.Replace("Composition: ", "") + "_weight_content");
                    XElement FloatSi = new XElement("Float");
                    XElement ConstantSi = new XElement("Constant");
                    XElement ValueSi = new XElement("Value");
                    ValueSi.Value = item.Y.ToString();
                    ConstantSi.Add(ValueSi);
                    FloatSi.Add(ConstantSi);
                    DatasetSi.Add(FloatSi);
                    ModelInSubModel.Add(DatasetSi); 
                }
                   
       
                SubModels.Add(ModelInSubModel);
                Model.Add(SubModels);
                Models.Add(Model);
            }
        }
        /// <summary>
        /// Adds the stress strain.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <param name="Model">The model.</param>
        private static void AddStressStrain(IList<Models.ExportPropertyGeneral> properties, XElement Model)
        {
            if (properties.Where(p => p.Type == PropertyTypeEnum.StressStrain).Any())
            {
                XElement DatasetElongation = new XElement("Dataset");
                DatasetElongation.SetAttributeValue("IDref", "ATT_true_stress");
                XElement Float = new XElement("Float");
                XElement Curve = new XElement("Curve");
                Curve.SetAttributeValue("Label", "F(strain)");
                XElement DataAxis = new XElement("DataAxis");
                XElement ValueList = new XElement("ValueList");
                IList<Value> coordinates;
                coordinates = properties.Where(p => p.Type == PropertyTypeEnum.StressStrain).Select(u => u.Values).FirstOrDefault().ToList();
                foreach (var item in coordinates)
                {
                    XElement Value = new XElement("Value");
                    Value.Value = item.Y.ToString();
                    ValueList.Add(Value);
                }
                DataAxis.Add(ValueList);
                XElement Units = new XElement("Units");
                XElement Unit = new XElement("Unit");
                Unit.SetAttributeValue("Name", "MPa");
                Units.Add(Unit);
                DataAxis.Add(Units);
                Curve.Add(DataAxis);
                XElement ParameterAxis = new XElement("ParameterAxis");
                ParameterAxis.SetAttributeValue("IDref", "ATT_true_strain");
                ValueList = new XElement("ValueList");
                foreach (var item in coordinates)
                {
                    XElement Value = new XElement("Value");
                    Value.Value = item.X.ToString();
                    ValueList.Add(Value);
                }
                ParameterAxis.Add(ValueList);
                Curve.Add(ParameterAxis);
                Float.Add(Curve);
                DatasetElongation.Add(Float);
                Model.Add(DatasetElongation);
            }
        }
        /// <summary>
        /// Adds the mechnical new.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <param name="Models">The models.</param>
        private static void AddMechnicalNew(IList<Models.ExportPropertyGeneral> properties, XElement Models)
        {
            if (properties.Where(p => p.Type == PropertyTypeEnum.MechanicalElongation || p.Type == PropertyTypeEnum.MechanicalTensile || p.Type == PropertyTypeEnum.MechanicalYield || p.Type == PropertyTypeEnum.StressStrain).Any())
            {
                XElement Model = new XElement("Model");
                Model.SetAttributeValue("IDref", "MOD_KT_MechProp");
                ExportMechanicalYieldNew(properties, Model);
                ExportMechanicalTensileNew(properties, Model);
                ExportMechanicalElongationNew(properties, Model);
                AddStressStrain(properties, Model);
                Models.Add(Model);
            }
        }
        /// <summary>
        /// Exports the mechanical elongation new.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <param name="Model">The model.</param>
        private static void ExportMechanicalElongationNew(IList<Models.ExportPropertyGeneral> properties, XElement Model)
        {
            if (properties.Where(p => p.Type == PropertyTypeEnum.MechanicalElongation).Any())
            {
                XElement DatasetElongation = new XElement("Dataset");
                DatasetElongation.SetAttributeValue("IDref", "ATT_elongation");
                XElement FloatElongation = new XElement("Float");
                XElement ConstantElongation = new XElement("Constant");
                XElement ValueElongation = new XElement("Value");
                Models.ExportPropertyGeneral elemElongation = properties.Where(p => p.Type == PropertyTypeEnum.MechanicalElongation).FirstOrDefault();
                ValueElongation.Value = elemElongation.Value;

                ConstantElongation.Add(ValueElongation);
                FloatElongation.Add(ConstantElongation);
                DatasetElongation.Add(FloatElongation);
                Model.Add(DatasetElongation);
            }
        }
        /// <summary>
        /// Exports the mechanical tensile new.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <param name="Model">The model.</param>
        private static void ExportMechanicalTensileNew(IList<Models.ExportPropertyGeneral> properties, XElement Model)
        {
            if (properties.Where(p => p.Type == PropertyTypeEnum.MechanicalTensile).Any())
            {
                XElement DatasetTensile = new XElement("Dataset");
                DatasetTensile.SetAttributeValue("IDref", "ATT_max_tensile_stress");
                XElement FloatTensile = new XElement("Float");
                XElement ConstantTensile = new XElement("Constant");
                XElement ValueTensile = new XElement("Value");
                Models.ExportPropertyGeneral elemTensile = properties.Where(p => p.Type == PropertyTypeEnum.MechanicalTensile).FirstOrDefault();
                ValueTensile.Value = elemTensile.Value;
                XElement UnitsTensile = new XElement("Units");
                XElement UnitTensile = new XElement("Unit");
                UnitTensile.SetAttributeValue("Name", "MPa");
                UnitsTensile.Add(UnitTensile);
                ConstantTensile.Add(ValueTensile);
                ConstantTensile.Add(UnitsTensile);
                FloatTensile.Add(ConstantTensile);
                DatasetTensile.Add(FloatTensile);
                Model.Add(DatasetTensile);
            }
        }
        /// <summary>
        /// Exports the mechanical yield new.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <param name="Model">The model.</param>
        private static void ExportMechanicalYieldNew(IList<Models.ExportPropertyGeneral> properties, XElement Model)
        {
            if (properties.Where(p => p.Type == PropertyTypeEnum.MechanicalYield).Any())
            {
                XElement DatasetYield = new XElement("Dataset");
                DatasetYield.SetAttributeValue("IDref", "ATT_Yield_stress_Rp02");
                XElement FloatYield = new XElement("Float");
                XElement ConstantYield = new XElement("Constant");
                XElement ValueYield = new XElement("Value");
                Models.ExportPropertyGeneral elem = properties.Where(p => p.Type == PropertyTypeEnum.MechanicalYield).FirstOrDefault();
                ValueYield.Value = elem.Value;
                XElement UnitsYield = new XElement("Units");
                XElement UnitYield = new XElement("Unit");
                UnitYield.SetAttributeValue("Name", "MPa");
                UnitsYield.Add(UnitYield);
                ConstantYield.Add(ValueYield);
                ConstantYield.Add(UnitsYield);
                FloatYield.Add(ConstantYield);
                DatasetYield.Add(FloatYield);
                Model.Add(DatasetYield);
            }
        }
        /// <summary>
        /// Adds the physical new.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <param name="Models">The models.</param>
        private static void AddPhysicalNew(IList<Models.ExportPropertyGeneral> properties, XElement Models)
        {
            if (properties.Where(p => p.Type == PropertyTypeEnum.PhysicalModulusOfElasticity || p.Type == PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion).Any())
            {
                XElement Model = new XElement("Model");
                Model.SetAttributeValue("IDref", "MOD_KTM_PhysProp");

                XElement DatasetYoung = new XElement("Dataset");
                DatasetYoung.SetAttributeValue("IDref", "ATT_Youngs_modulus");
                XElement FloatYoung = new XElement("Float");
                XElement CurveYoung = new XElement("Curve");
                CurveYoung.SetAttributeValue("Label", "F(T)");
                XElement DataAxisYoung = new XElement("DataAxis");
                XElement ValueListYoung = new XElement("ValueList");
                XElement ValueListYoungT = new XElement("ValueList");
                foreach (var property in properties.Where(p => p.Type == PropertyTypeEnum.PhysicalModulusOfElasticity))
                {
                    XElement Value = new XElement("Value");
                    Value.Value = property.Value;
                    ValueListYoung.Add(Value);
                    XElement ValueT = new XElement("Value");
                    ValueT.Value = property.Temperature.ToString();
                    ValueListYoungT.Add(ValueT);
                }
                XElement UnitsYoung = new XElement("Units");
                XElement UnitYoung = new XElement("Unit");
                UnitYoung.SetAttributeValue("Name", "GPa");
                UnitsYoung.Add(UnitYoung);
                XElement ParameterAxisYoung = new XElement("ParameterAxis");
                ParameterAxisYoung.SetAttributeValue("IDref", "TEMP");
                if (properties.Where(p => p.Type == PropertyTypeEnum.PhysicalModulusOfElasticity).Any())
                {
                    ParameterAxisYoung.Add(ValueListYoungT);
                    CurveYoung.Add(DataAxisYoung);
                    CurveYoung.Add(ParameterAxisYoung);
                    DataAxisYoung.Add(ValueListYoung);
                    DataAxisYoung.Add(UnitsYoung);

                    FloatYoung.Add(CurveYoung);
                    DatasetYoung.Add(FloatYoung);
                    Model.Add(DatasetYoung);
                }
                XElement DatasetThermal = new XElement("Dataset");
                DatasetThermal.SetAttributeValue("IDref", "Thermal_expansion_secant");
                XElement FloatThermal = new XElement("Float");
                XElement CurveThermal = new XElement("Curve");
                CurveThermal.SetAttributeValue("Label", "F(T)");
                XElement DataAxisThermal = new XElement("DataAxis");
                XElement ValueListThermal = new XElement("ValueList");
                XElement ValueListThermalT = new XElement("ValueList");
                foreach (var property in properties.Where(p => p.Type == PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion))
                {
                    XElement ValueThermal = new XElement("Value");                  
                    ValueThermal.Value = property.Value;
                    ValueListThermal.Add(ValueThermal);
                    XElement ValueThermalT = new XElement("Value");
                    ValueThermalT.Value = property.Temperature.ToString();
                    ValueListThermalT.Add(ValueThermalT);
                }
                XElement UnitsThermal = new XElement("Units");
                XElement UnitThermal = new XElement("Unit");
                UnitThermal.SetAttributeValue("Name", "C");
                UnitsThermal.Add(UnitThermal);
                XElement ParameterAxisThermal = new XElement("ParameterAxis");
                ParameterAxisThermal.SetAttributeValue("IDref", "TEMP");
                if (properties.Where(p => p.Type == PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion).Any())
                {
                    ParameterAxisThermal.Add(ValueListThermalT);
                    CurveThermal.Add(DataAxisThermal);
                    CurveThermal.Add(ParameterAxisThermal);
                    DataAxisThermal.Add(ValueListThermal);
                    DataAxisThermal.Add(UnitsThermal);


                    FloatThermal.Add(CurveThermal);
                    DatasetThermal.Add(FloatThermal);
                    Model.Add(DatasetThermal);
                }      
                Models.Add(Model);
            }
        }
        
        public static void AddEntryForSS1(ZipFile zip, IList<Models.ExportPropertyGeneral> properties)
        {            
            if (properties.Where(p => p.Type == PropertyTypeEnum.StressStrain).Any())
            {
                var memoryStream = new MemoryStream();
                var writer = new StreamWriter(memoryStream);
                string dat = "STRESS_UNIT 4" + Environment.NewLine;
                int curveCounter = 1;

                IList<ExportPropertyGeneral> proeprties = properties.Where(p => p.Type == PropertyTypeEnum.StressStrain).ToList();

                foreach (var ss in proeprties)
                {

                    dat += "CURVE " + curveCounter + Environment.NewLine;
                    dat += "POINTS " + ss.Values.Count + Environment.NewLine;
                    dat += "TEMPERATURE 2 " + ss.Temperature + "." + Environment.NewLine;
                    foreach (var temp in ss.Values)
                    {
                        dat += temp.X + " " + temp.Y + Environment.NewLine;

                    }
                    dat += Environment.NewLine;
                    curveCounter = curveCounter + 1;
                }

                writer.Write(dat);
                writer.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);
                zip.AddEntry("ss1.dat", memoryStream);
            }
        }


    }
}