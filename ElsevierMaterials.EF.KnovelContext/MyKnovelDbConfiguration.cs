using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Configuration;
using ElsevierMaterials.EF.Common.Models;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace ElsevierMaterials.EF.KnovelContext
{
   public class MyKnovelDbConfiguration: DbConfiguration //: MyBaseDbConfiguration
    {
        public MyKnovelDbConfiguration()
        {

            SetProviderServices(
                SqlProviderServices.ProviderInvariantName,
                SqlProviderServices.Instance);
            SetDefaultConnectionFactory(new SqlConnectionFactory(
                ConfigurationManager.ConnectionStrings["KnovelDbConnection"].ConnectionString));
        }
    }
}
