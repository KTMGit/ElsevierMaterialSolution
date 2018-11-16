using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.Models;
using ElsevierMaterials.EF.ModelsMisc.Maps;
namespace ElsevierMaterials.EF.KnovelContext
{
   //[DbConfigurationType("ElsevierMaterials.EF.KnovelContext.MyKnovelDbConfiguration,ElsevierMaterials.EF.KnovelContext")]
   public class KnovelDbContext : BaseDbContext
    {
       public KnovelDbContext()
            : base("KnovelDb")//,new MyKnovelDbConfiguration())
        {
            //TypeCorrectors.Add(new MovieCorrector());
            //TypeCorrectors.Add(new EntryCorrector());

            //TypeValidators.Add(new MovieValidator());
            //TypeValidators.Add(new PostValidator());
        }

        public DbSet<Material> Materials { get; set; }
        //public DbSet<Entry> Entries { get; set; }
        //public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.ComplexType<Distribution>();

            //modelBuilder.Configurations
            //    .Add(new MaterialMap());
            //modelBuilder.Configurations
            //    .Add(new EntryMap());
            //modelBuilder.Configurations
            //    .Add(new PostMap());
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
