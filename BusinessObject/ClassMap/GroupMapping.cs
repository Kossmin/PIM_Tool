using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ClassMap
{
    public class GroupMapping : ClassMap<Group>
    {
        public GroupMapping()
        {
            Id(x => x.ID);
            Map(x => x.Version).Length(10);
            References<Employee>(x => x.Employee).Column("LeaderID").Cascade.None().Unique();
            HasMany<Project>(x => x.projectObjects).Cascade.All().KeyColumn("GroupID");
            Table("Groups");
        }
    }
}
