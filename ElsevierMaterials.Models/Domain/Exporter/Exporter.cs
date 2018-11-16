
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Models.Domain.Export
{
    public class Exporter
    {
        public IList<ExportType> ExportTypes { get; set; }
        public IList<Material> Materials { get; set; }
        public IList<TMPropertyTypeEnum> Properties { get; set; }

        public Exporter() {

            Materials = new List<Material>();
            Properties = new List<TMPropertyTypeEnum>();

            ExportTypes = new List<ExportType>();

            ExportTypes.Add(FillExportType(ExportTypeEnum.Radioss));
            ExportTypes.Add(FillExportType(ExportTypeEnum.Abaqus));

            ExportTypes.Add(FillExportType(ExportTypeEnum.SolidEdge));
            ExportTypes.Add(FillExportType(ExportTypeEnum.SolidWorks));

            ExportTypes.Add(FillExportType(ExportTypeEnum.Esi));
            ExportTypes.Add(FillExportType(ExportTypeEnum.ESIPamCrash));

            ExportTypes.Add(FillExportType(ExportTypeEnum.ANSYS));

            ExportTypes.Add(FillExportType(ExportTypeEnum.Siemens));

            ExportTypes.Add(FillExportType(ExportTypeEnum.NASTRAN));
            ExportTypes.Add(FillExportType(ExportTypeEnum.LsDyna));


            ExportTypes.Add(FillExportType(ExportTypeEnum.FEMAP));
            ExportTypes.Add(FillExportType(ExportTypeEnum.PTCCreo));

            ExportTypes.Add(FillExportType(ExportTypeEnum.Excel));
            ExportTypes.Add(FillExportType(ExportTypeEnum.AutodeskNastran));

            Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
            Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
            Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
            Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
            Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
            Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);
            Properties.Add(TMPropertyTypeEnum.MechanicalYield);
            Properties.Add(TMPropertyTypeEnum.MechanicalTensile);
            Properties.Add(TMPropertyTypeEnum.MechanicalElongation);
 
        }

        public void FillAllPropertiesForExport()
        {
            Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
            Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
            Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
            Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
            Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
            Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);          
            //Properties.Add(PropertyTypeEnum.FatigueStrengthCoefficient);
            //Properties.Add(PropertyTypeEnum.FatigueStrengthExponent);
            //Properties.Add(PropertyTypeEnum.FatigueDuctilityExponent);
            //Properties.Add(PropertyTypeEnum.FatigueDuctilityCoefficient);
            //Properties.Add(PropertyTypeEnum.FatigueCyclicStrengthExponent);
            //Properties.Add(PropertyTypeEnum.FatigueCyclicStrengthCoefficient);
            //Properties.Add(PropertyTypeEnum.FatigueNf_maxENDiagram);
            //Properties.Add(PropertyTypeEnum.FatigueA);
            //Properties.Add(PropertyTypeEnum.FatigueB);
            //Properties.Add(PropertyTypeEnum.FatigueNf_maxSNDiagram);
            //Properties.Add(PropertyTypeEnum.FatigueLimit);
            //Properties.Add(PropertyTypeEnum.StressStrain);
            //Properties.Add(PropertyTypeEnum.PlasticStrainStress);
            Properties.Add(TMPropertyTypeEnum.MechanicalYield);
            Properties.Add(TMPropertyTypeEnum.MechanicalTensile);
            Properties.Add(TMPropertyTypeEnum.MechanicalElongation);
            //Properties.Add(PropertyTypeEnum.FatigueStressPoints);
         //   Properties.Add(PropertyTypeEnum.ChemicalCompositions);
        }
        
        public ExportType FillExportType(ExportTypeEnum type)
        {

        ExportType typeObj = new ExportType();
        typeObj.ExportTypeId = type;


        switch (type)
        {
            case ExportTypeEnum.All:
                typeObj.IdString = "allTabHead";
                typeObj.Title = "All";
                typeObj.Href = "#allTab";
                typeObj.IdKey = "all";

                break;
            case ExportTypeEnum.Radioss:
                typeObj.IdString = "altairTabHead";
                typeObj.Title = "OptiStruct";
                typeObj.Href = "#altairTab";
                typeObj.IdKey = "altair";
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalYield);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalTensile);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueStrengthCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueStrengthExponent);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueDuctilityExponent);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueDuctilityCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueCyclicStrengthExponent);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueCyclicStrengthCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueNf_maxENDiagram);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueA);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueB);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueNf_maxSNDiagram);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueLimit);
                typeObj.Properties.Add(TMPropertyTypeEnum.StressStrain);
                break;
            case ExportTypeEnum.Abaqus:
                typeObj.IdString = "abaqusTabHead";
                typeObj.Title = "Abaqus";
                typeObj.Href = "#abaqusTab";
                typeObj.IdKey = "abaqus";

                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PlasticStrainStress);


                break;
            case ExportTypeEnum.Excel:
                typeObj.IdString = "ExcelTabHead";
                typeObj.Title = "Excel";
                typeObj.Href = "#ExcelTab";
                typeObj.IdKey = "Excel";


                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalElongation);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalYield);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalTensile);

                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);


                break;

            case ExportTypeEnum.KTMXml:
                typeObj.IdString = "KTMXmlTabHead";
                typeObj.Title = "KTM XML";
                typeObj.Href = "#KTMXmlTab";
                typeObj.IdKey = "KTMXml";
                break;
            case ExportTypeEnum.FEMAP:
                typeObj.IdString = "FEMAPTabHead";
                typeObj.Title = "FEMAP";
                typeObj.Href = "#FEMAPTab";
                typeObj.IdKey = "FEMAP";

                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);           
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);    

                break;
            case ExportTypeEnum.SolidWorks:
                typeObj.IdString = "solidWorksTabHead";
                typeObj.Title = "SolidWorks";
                typeObj.Href = "#solidWorksTab";
                typeObj.IdKey = "solidWorks";

                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalYield);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalTensile);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);


                break;
            case ExportTypeEnum.SolidEdge:
                typeObj.IdString = "solidEdgeTabHead";
                typeObj.Title = "SolidEdge";
                typeObj.Href = "#solidEdgeTab";
                typeObj.IdKey = "solidEdge";

                 typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalYield);
                 typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalTensile);
                 typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalElongation);
                 typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                 typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                 typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
                 typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);
                 typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                 typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);

                break;
            case ExportTypeEnum.Esi:
                typeObj.IdString = "esiTabHead";
                typeObj.Title = "ESI ProCAST";
                typeObj.Href = "#esiTab";
                typeObj.IdKey = "esi";

                typeObj.Properties.Add(TMPropertyTypeEnum.StressStrain);
                typeObj.Properties.Add(TMPropertyTypeEnum.ChemicalCompositions);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalElongation);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalYield);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalTensile);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);


                break;
            case ExportTypeEnum.ESIPamCrash:
                typeObj.IdString = "esiPamCrashTabHead";
                typeObj.Title = "ESI Pam-Crash";
                typeObj.Href = "#esiPamCrashTab";
                typeObj.IdKey = "esiPamCrash";

                 typeObj.Properties.Add(TMPropertyTypeEnum.StressStrain);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);

                break;
            case ExportTypeEnum.EsiVPS:
                typeObj.IdString = "esiVpsTabHead";
                typeObj.Title = "ESI VPS";
                typeObj.Href = "#esiVpsTab";
                typeObj.IdKey = "esiVps";
                break;
            case ExportTypeEnum.ANSYS:
                typeObj.IdString = "ansysTabHead";
                typeObj.Title = "ANSYS";
                typeObj.Href = "#ansysTab";
                typeObj.IdKey = "ansys";
                
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PlasticStrainStress);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueCyclicStrengthExponent);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueCyclicStrengthCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueStrengthCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueStrengthExponent);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueDuctilityExponent);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueDuctilityCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueStressPoints);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalTensile);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalYield);

                break;
            case ExportTypeEnum.Siemens:
                typeObj.IdString = "siemensTabHead";
                typeObj.Title = "Siemens NX";
                typeObj.Href = "#siemensTab";
                typeObj.IdKey = "siemens";

                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalTensile);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalYield);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueCyclicStrengthExponent);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueCyclicStrengthCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueStrengthCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueStrengthExponent);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueDuctilityExponent);
                typeObj.Properties.Add(TMPropertyTypeEnum.FatigueDuctilityCoefficient);


                break;
            case ExportTypeEnum.LsDyna:
                typeObj.IdString = "lsDynaTabHead";
                typeObj.Title = "LS-DYNA";
                typeObj.Href = "#lsDynaTab";
                typeObj.IdKey = "lsDyna";

                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);         
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PlasticStrainStress);
                
                break;
            case ExportTypeEnum.NASTRAN:
                typeObj.IdString = "nastranTabHead";
                typeObj.Title = "MSC Nastran";
                typeObj.Href = "#nastranTab";
                typeObj.IdKey = "nastran";

                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                
                break;
            case ExportTypeEnum.AutodeskNastran:
                typeObj.IdString = "autodeskNastranTabHead";
                typeObj.Title = "Autodesk Nastran";
                typeObj.Href = "#autodeskNastranTab";
                typeObj.IdKey = "autodeskNastran";
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);
                typeObj.Properties.Add(TMPropertyTypeEnum.MechanicalTensile);
                break;
            case ExportTypeEnum.PTCCreo:

                typeObj.IdString = "ptcCreoTabHead";
                typeObj.Title = "PTC Creo";
                typeObj.Href = "#ptcCreoTab";
                typeObj.IdKey = "ptcCreo";

                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalModulusOfElasticity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalPoissonCoefficient);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalDensity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalThermalConductivity);
                typeObj.Properties.Add(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);
                break;
        }
                return typeObj;

        }
        
        
        
    }



}
