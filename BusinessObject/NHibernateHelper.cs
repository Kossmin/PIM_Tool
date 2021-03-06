using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class NHibernateHelper
    {

        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    InitializeSessionFactory(); return _sessionFactory;
            }
        }

        private static void InitializeSessionFactory()
        {
            _sessionFactory = Fluently.Configure()			
            .Database(MsSqlConfiguration.MsSql2008.ConnectionString(
            //@"Data Source=(localdb)\ProjectsV13;Initial Catalog=PIMDemoDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False").ShowSql())
            @"Data Source=DESKTOP-OO0EKGH;Initial Catalog=PIM_Tool;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False").ShowSql())
         .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Project>())
         .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Group>())
         .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Employee>())
         .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, false))
         .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
