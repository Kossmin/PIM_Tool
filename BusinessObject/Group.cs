using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Group
    {
        public virtual int ID { get; set; }
        //public virtual int LeaderID { get; set; }
        public virtual int Version { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual IList<Project> projectObjects { get; set; }

        public virtual void AssignLeader(Employee employee)
        {
            Employee = employee;
            employee.Group = this;
        }

        public override bool Equals(object obj)
        {
            return ID == ((Group)obj).ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

    }

    public class GroupMapping : ClassMap<Group>
    {
        public GroupMapping()
        {
            Id(x => x.ID);
            Map(x => x.Version).Length(10);
            References<Employee>(x => x.Employee).Column("LeaderID").Cascade.None().Unique();
            //HasOne(x => x.Employee).Constrained().Cascade.None();
            HasMany<Project>(x => x.projectObjects).Cascade.All().KeyColumn("GroupID");
            Table("Groups");
        }
    }
}
