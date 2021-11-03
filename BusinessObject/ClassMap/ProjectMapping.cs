using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ClassMap
{
    public class ProjectMapping : ClassMap<Project>
    {
        public ProjectMapping()
        {
            Id(x => x.ID);
            //Map(x => x.GroupID).Not.Nullable().Length(19);
            Map(x => x.ProjectNumber).Not.Nullable().Length(4);
            Map(x => x.ProjectName).Not.Nullable().Length(50);
            Map(x => x.Customer).Not.Nullable().Length(50);
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.Status).Not.Nullable().Length(3).CustomSqlType("char(3)");
            Version(x => x.Version);
            HasManyToMany(x => x.Employees)
                .Access.Property()
                .Cascade.SaveUpdate()
                .Table("ProjectEmployees");
            References<Group>(x => x.Group).Cascade.None().Column("GroupID");
            OptimisticLock.Version();
            Table("Project");
        }
    }
}
