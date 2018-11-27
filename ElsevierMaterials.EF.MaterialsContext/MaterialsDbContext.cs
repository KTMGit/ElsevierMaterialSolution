using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.EF.ModelsMisc.Maps;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterials.Models.Domain.PropertyGroups;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace ElsevierMaterials.EF.MaterialsContext
{
    public class MaterialsDbContext : BaseDbContext
    {
        public MaterialsDbContext()
            : base("emt") // production address
        //: base("kms_test") // test address
        {
            //TypeCorrectors.Add(new MovieCorrector());
            //TypeCorrectors.Add(new EntryCorrector());

            //TypeValidators.Add(new MovieValidator());
            //TypeValidators.Add(new PostValidator());
        }

        public DbSet<PropertyWithConvertedValues> PropertiesWithConvertedValues { get; set; }
        public DbSet<PropertyConversionFactorAndUnit> PropertiesConversionFactorsAndUnits { get; set; }

        public DbSet<Source> Sources { get; set; }
        public DbSet<Tree> Tree { get; set; }
        public DbSet<PrefferedNames> PrefferedNames { get; set; }
        public DbSet<BaseProperty> BaseProperties { get; set; }
        public DbSet<PropertyDescription> PropertiesDescriptions { get; set; }
        public DbSet<EquivalentProperty> EquivalentProperties { get; set; }
        public DbSet<MaterialProperty> MaterialProperties { get; set; }
        public DbSet<PropertyGroup> PropertyGroups { get; set; }
        public DbSet<RecordLink> RecordLinks { get; set; }
        //public DbSet<GroupCondition> GroupConditions { get; set; }
        public DbSet<GroupMaterialCondition> GroupConditionsFirst { get; set; }
        public DbSet<GroupTestCondition> GroupConditionsSecond { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Material> MaterialsView { get; set; }
        public DbSet<MaterialTaxonomy> MaterialTaxonomy { get; set; }
        public DbSet<FullTextSearch> FullTextSearch { get; set; }
        public DbSet<PropertyUnit> PropertyUnits { get; set; }
        public DbSet<AdvSearchResults> AdvSearchResults { get; set; }
        public DbSet<EquivalentMaterial> EquivalentMaterials { get; set; }
        public DbSet<TreeCount> TreeCounts { get; set; }
        public DbSet<TaxonomyTreeCount> TaxonomyTreeCounts { get; set; }
        public DbSet<GroupChemicalProperty> GroupChemicalProperties { get; set; }
        public DbSet<GroupChemicalPropertyAll> GroupChemicalPropertiyAlls { get; set; }
        public DbSet<DiagramPoint> DiagramPoints { get; set; }
        public DbSet<Citation> Citations { get; set; }
        public DbSet<AdvSearchPropertyConditions> AdvSearchPropertyConditions { get; set; }
        public DbSet<AdvSearchMultipointDataView> AdvSearchMultipointDataView { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations
                .Add(new SourceMap());
            modelBuilder.Configurations
               .Add(new TreeMap());
            modelBuilder.Configurations
              .Add(new PrefferedNamesMap());
            modelBuilder.Configurations
                .Add(new BasePropertyMap());
            modelBuilder.Configurations
                .Add(new EquivalentPropertyViewMap());
            modelBuilder.Configurations
                .Add(new MaterialPropertyMap());
            modelBuilder.Configurations
                .Add(new MaterialTaxonomyMap());
            modelBuilder.Configurations
                .Add(new RecordLinkMap());
            modelBuilder.Configurations
                .Add(new UnitMap());
            modelBuilder.Configurations
                .Add(new FullTextSearchMap());
            modelBuilder.Configurations
                  .Add(new MaterialViewMap());
            modelBuilder.Configurations
                .Add(new PropertyUnitsViewMap());
            modelBuilder.Configurations
                .Add(new AdvSearchResultViewMap());
            modelBuilder.Configurations
                .Add(new PropertyWithConvertedValuesMap());
            modelBuilder.Configurations
                .Add(new PropertiesConversionFactorsMap());
            modelBuilder.Configurations
            .Add(new PropertyGroupMap());
            //modelBuilder.Configurations
            //  .Add(new GroupConditionMap());
            modelBuilder.Configurations
                .Add(new GroupMaterialConditionMap());
            modelBuilder.Configurations
                .Add(new GroupTestConditionMap());
            modelBuilder.Configurations
                .Add(new EquivalentMaterialMap());
            modelBuilder.Configurations
                .Add(new TreeCountMap());
            modelBuilder.Configurations
                .Add(new TaxonomyTreeCountMap());
            modelBuilder.Configurations
                .Add(new PropertyDescriptionMap());
            modelBuilder.Configurations
                .Add(new GroupChemicalPropertyMap());
            modelBuilder.Configurations
                .Add(new GroupChemicalPropertyAllMap());
            modelBuilder.Configurations
                .Add(new DiagramPointMap());
            modelBuilder.Configurations
                .Add(new CitationMap());
            modelBuilder.Configurations
                .Add(new AdvSearchPropertyConditionsMap());
            modelBuilder.Configurations
                .Add(new AdvSearchMultipointDataViewMap());
        }

        public override int SaveChanges()
        {
            // demo purpose: false is by the default
            ValidationExceptionFromValidationErrorsEnabled = false;

            if (!Configuration.ValidateOnSaveEnabled)
            { Configuration.ValidateOnSaveEnabled = true; }

            return base.SaveChanges();
        }

        //protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        //{
        //    return base.ShouldValidateEntity(entityEntry) || // only Added or Modified
        //        (entityEntry.State == EntityState.Deleted &&
        //            entityEntry.Entity is Post);
        //}
    }
}
