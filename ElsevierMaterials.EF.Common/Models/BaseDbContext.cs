using ElsevierMaterials.EF.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace ElsevierMaterials.EF.Common.Models
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(string name)//, DbConfiguration configuration)
            : base(name)
        {
            Configuration.ValidateOnSaveEnabled = false;
           // DbConfiguration.SetConfiguration(configuration);
            Database.SetInitializer<BaseDbContext>(null);
        }

        protected bool ValidationExceptionFromValidationErrorsEnabled { get; set; }
        protected HashSet<ITypeCorrector> TypeCorrectors
        {
            get
            {
                if (_typeCorrectors == null)
                { _typeCorrectors = new HashSet<ITypeCorrector>(); }

                return (HashSet<ITypeCorrector>)_typeCorrectors;
            }
        }
        protected HashSet<ITypeValidator> TypeValidators
        {
            get
            {
                if (_typeValidators == null)
                { _typeValidators = new HashSet<ITypeValidator>(); }

                return (HashSet<ITypeValidator>)_typeValidators;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions
                .Remove<StoreGeneratedIdentityKeyConvention>();
            modelBuilder.Conventions
                .Remove<IdKeyDiscoveryConvention>();
            modelBuilder.Conventions
                .Remove<OneToManyCascadeDeleteConvention>();

            //// demo purpose: overriden by MovieMap :(
            //modelBuilder.Properties()
            //    .Where(p => p.Name.Equals("Id"))
            //    .Configure(p => p.IsKey());

            //modelBuilder.Build(Database.Connection);
            
        }

        public override int SaveChanges()
        {
            bool isSomethingCorrected = false;

            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                    e.State == EntityState.Modified))
            {
                var typeCorrector = TypeCorrectors
                    .Where(tc => tc.IsMatch(entry.Entity))
                    .FirstOrDefault();
                if (typeCorrector != null)
                {
                    isSomethingCorrected = true;
                    typeCorrector.CorrectEntity(entry.Entity, entry.State);
                }
            }

            if (!Configuration.AutoDetectChangesEnabled && isSomethingCorrected)
            { ChangeTracker.DetectChanges(); }

            if (ValidationExceptionFromValidationErrorsEnabled)
            { ThrowValidationExceptionFromValidationErrors(); }

            return base.SaveChanges();
        }

        protected override DbEntityValidationResult ValidateEntity(
            DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var result = base.ValidateEntity(entityEntry, items);

            {
                var typeValidator = TypeValidators
                    .Where(tv => tv.IsMatch(result))
                    .FirstOrDefault();
                if (typeValidator != null)
                { typeValidator.ValidateEntity(result); }
            }

            return result;
        }

        private void ThrowValidationExceptionFromValidationErrors()
        {
            var dbEntityValidationResults = GetValidationErrors().ToList();
            if (dbEntityValidationResults.Any())
            {
                var stringBuilder = new StringBuilder()
                    .AppendLine("Validation errors found in DbContext.Save() method:");
                dbEntityValidationResults
                    .ForEach(dbEntityValidationResult =>
                        dbEntityValidationResult.ValidationErrors.ToList()
                            .ForEach(dbValidationError =>
                                stringBuilder
                                    .AppendLine(String.Format("{0}: {1}",
                                        dbValidationError.PropertyName ?? "(no property)",
                                        dbValidationError.ErrorMessage ?? "(no message)"))));

                throw new DbEntityValidationException(
                    stringBuilder.ToString(), dbEntityValidationResults);
            }
        }

        private ICollection<ITypeCorrector> _typeCorrectors;
        private ICollection<ITypeValidator> _typeValidators;
    }
}
