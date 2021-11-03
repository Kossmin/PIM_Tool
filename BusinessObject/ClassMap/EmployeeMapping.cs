using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ClassMap
{
    public class EmployeeMapping : ClassMap<Employee>
    {
        public EmployeeMapping()
        {
            Id(x => x.ID);
            Map(x => x.Visa).Not.Nullable().CustomSqlType("Char(3)");
            Map(x => x.FirstName).Not.Nullable().CustomSqlType("nvarchar(50)");
            Map(x => x.LastName).Not.Nullable().CustomSqlType("nvarchar(50)");
            Map(x => x.BirthDate).Not.Nullable();
            Version(x => x.Version).Not.Nullable().Length(10);

            HasManyToMany(x => x.Projects)
                .Cascade.SaveUpdate()
                .Access.Property()
                .Inverse()
                .Table("ProjectEmployees");
            HasOne(x => x.Group).Cascade.All();

            Table("Employees");
        }
    }
}
