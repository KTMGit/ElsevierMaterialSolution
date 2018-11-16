using ElsevierMaterials.Models.Domain.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.ComparisonDiagram
{

    public class PropertyD : IPropertyBasicInfo
    {
        public PropertyD()
        {
            Materials = new List<MaterialD>();
            StressTemperatures = new List<StressStrainTemperature>();
        }
        public string GroupName { get; set; }
        public GroupTypeEnum GroupId { get; set; }
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int SourceTypeId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double MaxYValue { get; set; }
        public double MaxXValue { get; set; }
        public double MinYValue { get; set; }
        public double MinXValue { get; set; }
        public IList<MaterialD> Materials { get; set; }
        public IList<StressStrainTemperature> StressTemperatures { get; set; }

        public string SelectedXName { get; set; }
        public IList<string> ListXNamesAll { get; set; }       

    }


    public enum GroupTypeEnum
    {
        Mechanical = 806,
        Physical = 807,
        Electrical = 808,
        Machinability = 809,
        Chemical = 139941,
        Applications = 139946,
        FatigueData = 139968,
        FractureData = 139992,
        CreepData = 139993,
        Thermal = 139994,
        Rheology = 551876,
        SurfaceProperties = 551875,
        EnvironmentalCharacteristics = 551874,
        SolutionProperties = 377375,
        OpticalProperties = 377374,
        MagneticProperties = 377373,
        HazardRelatedProperties = 377372,
        ChemicalProperties = 377371,
        EquationPlotter = 585218,
        ThermodynamicProperties = 609579,
        ProcessingProperties = 609580,
        CorrosionResistance = 609581,
        ApplicationCharacterisitcs = 609582,
        StressStrain = -1

    }
}
