using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.EF.Common.Factory;
using ElsevierMaterials.EF.Common.Interfaces;
using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.EF.MaterialsContext;
using ElsevierMaterials.EF.MaterialsContextUow.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.PropertyGroups;

namespace ElsevierMaterials.EF.MaterialsContextUow
{
    public class MaterialsContextUow : IMaterialsContextUow, IDisposable
    {
        public MaterialsContextUow() :
            this(new List<DbContext> { new MaterialsDbContext() },
                  new RepositoryProvider()) { }


        public MaterialsContextUow(
            IList<DbContext> dbContexts,
            IRepositoryProvider repositoryProvider)
        {
            _dbContexts = dbContexts;
            ConfigureDbContexts();
            _repositoryProvider = repositoryProvider;
            _repositoryProvider.DbContext = _dbContexts[0];
        }

        public IMaterialRepository Materials
        {
            get
            {
                return _repositoryProvider.GetRepository<IMaterialRepository>(
                    dbContext => new MaterialRepository(_dbContexts[0]));
            }
        }

        public IClassificationRepository Classifications
        {
            get
            {
                return _repositoryProvider.GetRepository<IClassificationRepository>(
                    dbContext => new ClassificationRepository(_dbContexts[0]));
            }
        }
        public IRepository<PrefferedNames> PreferredNames
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<PrefferedNames>(_dbContexts[0]);
            }
        }

        public IRepository<EquivalentMaterial> EquivalentMaterials
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<EquivalentMaterial>(_dbContexts[0]);
            }
        }



        public IRepository<MaterialTaxonomy> MaterialTaxonomyAll
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<MaterialTaxonomy>(_dbContexts[0]);
            }
        }


        public IRepository<PropertyUnit> PropertyUnits
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<PropertyUnit>(_dbContexts[0]);
            }
        }

        public ITreeRepository Trees
        {
            get
            {
                return _repositoryProvider.GetRepository<ITreeRepository>(dbContext => new TreeRepository(_dbContexts[0]));
            }
        }

        public IRepository<BaseProperty> BaseProperties
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<BaseProperty>(_dbContexts[0]);
            }
        }


        public IRepository<PropertyDescription> PropertiesDescriptions
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<PropertyDescription>(_dbContexts[0]);
            }
        }

        public IRepository<EquivalentProperty> EquivalentProperties
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<EquivalentProperty>(_dbContexts[0]);
            }
        }
        public IRepository<MaterialProperty> MaterialProperties
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<MaterialProperty>(_dbContexts[0]);
            }
        }
        public IPropertyRepository Properties
        {
            get
            {
                return _repositoryProvider.GetRepository<IPropertyRepository>(dbContexts => new PropertyRepository(_dbContexts[0]));
            }
        }

        public IRepository<PropertyGroup> PropertyGroups
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<PropertyGroup>(_dbContexts[0]);
            }
        }


        public IRepository<GroupCondition> GroupConditions
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<GroupCondition>(_dbContexts[0]);
            }
        }



        public IRepository<GroupMaterialCondition> GroupMaterialConditions
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<GroupMaterialCondition>(_dbContexts[0]);
            }
        }
        public IRepository<GroupChemicalProperty> GroupChemicalProperties
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<GroupChemicalProperty>(_dbContexts[0]);
            }
        }

        public IRepository<GroupChemicalPropertyAll> GroupChemicalPropertyAll
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<GroupChemicalPropertyAll>(_dbContexts[0]);
            }
        }

        public IRepository<DiagramPoint> DiagramPoints
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<DiagramPoint>(_dbContexts[0]);
            }
        }

        public IRepository<Citation> Citations
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<Citation>(_dbContexts[0]);
            }
        }


        public IRepository<AdvSearchPropertyConditions> AdvSearchPropertyConditionsAll
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<AdvSearchPropertyConditions>(_dbContexts[0]);
            }
        }

        public IRepository<AdvSearchMultipointDataView> AdvSearchMultipointDataViewAll
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<AdvSearchMultipointDataView>(_dbContexts[0]);
            }
        }


        public IRepository<GroupTestCondition> GroupTestConditions
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<GroupTestCondition>(_dbContexts[0]);
            }
        }



        public IPropertyWithConvertedValuesRepository PropertiesWithConvertedValues
        {
            get
            {
                return _repositoryProvider.GetRepository<IPropertyWithConvertedValuesRepository>(dbContexts => new PropertyWithConvertedValuesRepository(_dbContexts[0]));
            }
        }

        public IPropertyWithConversionFactorsAndUnitsRepository PropertiesWithConversionFactorsAndUnits
        {
            get
            {
                return _repositoryProvider.GetRepository<IPropertyWithConversionFactorsAndUnitsRepository>(dbContexts => new PropertyConversionFactorAndUnitRepository(_dbContexts[0]));
            }
        }


        public ISampleMaterialRepository SampleMaterials
        {
            get
            {
                return _repositoryProvider.GetRepository<ISampleMaterialRepository>(
                     dbContext => new SampleMaterialRepository(_dbContexts[0]));

            }
        }

        public IRepository<TreeCount> TreeCounts
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<TreeCount>(_dbContexts[0]);
            }
        }

        public IRepository<TaxonomyTreeCount> TaxonomyTreeCounts
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<TaxonomyTreeCount>(_dbContexts[0]);
            }
        }




        public IFullTextSearchRepository FullTextSearch { get { return _repositoryProvider.GetRepository<IFullTextSearchRepository>(dbContexts => new FullTextSearchRepository(_dbContexts[0])); } }


        public IAdvSearchResultsRepository AdvSearchResults { get { return _repositoryProvider.GetRepository<IAdvSearchResultsRepository>(dbContexts => new AdvSearchResultsRepository(_dbContexts[0])); } }
        public IRepository<Source> Sources
        { get { return _repositoryProvider.GetEntityRepository<Source>(_dbContexts[0]); } }
        public IRepository<RecordLink> RecordLinks
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<RecordLink>(_dbContexts[0]);
            }
        }
        public IRepository<Condition> Conditions
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<Condition>(_dbContexts[0]);
            }
        }








        public IRepository<Unit> Units
        {
            get
            {
                return _repositoryProvider.GetEntityRepository<Unit>(_dbContexts[0]);
            }
        }

        public IUowCommandResult SubmitChanges()
        {
            var uowCommandResult = new UowCommandResult();

            try
            {
                //treba dodati i za dbContext[1]
                uowCommandResult.RecordsAffected = _dbContexts[0].SaveChanges();
                uowCommandResult.HasError = uowCommandResult.RecordsAffected < 1;
                uowCommandResult.Description = String.Format(
                    "{0} rows affected.",
                    uowCommandResult.RecordsAffected);
            }
            catch (DbEntityValidationException exception)
            {
                uowCommandResult.Description = "EfHandledError_DbEntityValidationException";
                uowCommandResult.Exception = exception;
                exception.EntityValidationErrors.ToList()
                    .ForEach(dbEntityValidationResult => dbEntityValidationResult.ValidationErrors.ToList()
                        .ForEach(dbValidationError =>
                            {
                                // next 2 lines: just in case
                                if (!uowCommandResult.ValidationErrors.ToList()
                                    .Any(crve => crve.Key == dbValidationError.PropertyName))
                                {
                                    uowCommandResult.ValidationErrors.Add(
                                        dbValidationError.PropertyName ?? "",
                                        dbValidationError.ErrorMessage ?? "");
                                }
                            }));
            }
            catch (DbUpdateException exception)
            {
                uowCommandResult.Description = "SqlServerHandledError_DbUpdateException";
                var theMostInnerException = (Exception)exception;
                while (theMostInnerException.InnerException != null)
                { theMostInnerException = theMostInnerException.InnerException; }
                uowCommandResult.Exception = theMostInnerException;
            }
            catch (Exception exception)
            {
                uowCommandResult.Description = "UnexpectedException";
                uowCommandResult.Exception = exception;
            }

            return uowCommandResult;
        }

        public void Dispose()
        {
            if (_dbContexts != null)
            {
                foreach (var dbContext in _dbContexts)
                { dbContext.Dispose(); }
                _dbContexts = null;
            }
        }

        private void ConfigureDbContexts()
        {
            foreach (var dbContext in _dbContexts)
            { dbContext.Configuration.ProxyCreationEnabled = false; }
        }

        private IList<DbContext> _dbContexts;
        private IRepositoryProvider _repositoryProvider;
    }
}
