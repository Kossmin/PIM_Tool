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
        public virtual int LeaderID { get; set; }
        public virtual int Version { get; set; }

        public virtual EmployeeObject Employee { get; set; }
        public virtual IList<ProjectObject> projectObjects { get; set; }

        public virtual void AssignLeader(EmployeeObject employee)
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
            HasOne(x => x.Employee).Constrained().Cascade.None();
            HasMany<ProjectObject>(x => x.projectObjects).Cascade.All();
            Table("GroupObject");
        }
    }
}
