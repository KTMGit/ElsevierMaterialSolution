using ElsevierMaterials.EF.Common.Models;
using System.Data.Entity.Infrastructure;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
namespace ElsevierMaterials.EF.MaterialsContext
{
    public class MyMaterialsDbConfiguration : DbConfiguration// MyBaseDbConfiguration
    {
         public MyMaterialsDbConfiguration()
        {
            SetProviderServices(
              SqlProviderServices.ProviderInvariantName,
              SqlProviderServices.Instance);
            SetDefaultConnectionFactory(new SqlConnectionFactory(
                ConfigurationManager.ConnectionStrings["ems_connection"].ConnectionString));
        }
    }
}
