using System;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.PropertyGroups;

namespace ElsevierMaterials.Common.Interfaces
{
    public interface IMaterialsContextUow : IDisposable
    {
        IMaterialRepository Materials { get; }
        ISampleMaterialRepository SampleMaterials { get; }
        IRepository<Source> Sources { get; }
        IFullTextSearchRepository FullTextSearch { get; }
        IAdvSearchResultsRepository AdvSearchResults { get; }
        IClassificationRepository Classifications { get; }
        IRepository<PrefferedNames> PreferredNames { get; }
        ITreeRepository Trees { get; }
        IRepository<BaseProperty> BaseProperties { get; }
        IRepository<EquivalentProperty> EquivalentProperties { get; }
        IPropertyRepository Properties { get; }
        IRepository<PropertyGroup> PropertyGroups { get; }
        //IRepository<GroupCondition> GroupConditions { get; }
        IRepository<GroupMaterialCondition> GroupMaterialConditions { get; }
        IRepository<GroupTestCondition> GroupTestConditions { get; }
        IPropertyWithConvertedValuesRepository PropertiesWithConvertedValues { get; }
        IPropertyWithConversionFactorsAndUnitsRepository PropertiesWithConversionFactorsAndUnits { get; }
        IRepository<MaterialProperty> MaterialProperties { get; }
        IRepository<RecordLink> RecordLinks { get; }
        IRepository<Condition> Conditions { get; }
        IRepository<Unit> Units { get; }
        IRepository<PropertyUnit> PropertyUnits { get; }
        IRepository<EquivalentMaterial> EquivalentMaterials { get; }
        IRepository<TreeCount> TreeCounts { get; }
        IRepository<TaxonomyTreeCount> TaxonomyTreeCounts { get; }
        IRepository<PropertyDescription> PropertiesDescriptions { get; }
        IRepository<GroupChemicalProperty> GroupChemicalProperties { get; }
        IRepository<GroupChemicalPropertyAll> GroupChemicalPropertyAll { get; }
        IRepository<DiagramPoint> DiagramPoints { get; }
        IRepository<Citation> Citations { get; }
        IRepository<AdvSearchPropertyConditions> AdvSearchPropertyConditionsAll { get; }
        IRepository<AdvSearchMultipointDataView> AdvSearchMultipointDataViewAll { get; }
        IRepository<MaterialTaxonomy> MaterialTaxonomyAll { get; }

        IUowCommandResult SubmitChanges();

    }
}
