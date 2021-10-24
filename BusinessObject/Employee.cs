using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Employee
    {
        public virtual int ID { get; set; }
        public virtual string Visa { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual int Version { get; set; }

        public virtual IList<Project_Employee> ProjectEmployees { get; set; }
        public virtual Group Group { get; set; }

        public virtual void AddEmployyee(Project_Employee project_Employee)
        {
            project_Employee.Employee = this;
            ProjectEmployees.Add(project_Employee);
        }

        public virtual void AssignLeader(Group group)
        {
            Group = group;
            group.Employee = this;
        }
    }

    public class EmployeeMapping: ClassMap<Employee>
    {
        public EmployeeMapping()
        {
            //LazyLoad();
            Id(x => x.ID);
            Map(x => x.Visa).Not.Nullable().CustomSqlType("Char(3)");
            Map(x => x.FirstName).Not.Nullable().CustomSqlType("nvarchar(50)");
            Map(x => x.LastName).Not.Nullable().CustomSqlType("nvarchar(50)");
            Map(x => x.BirthDate).Not.Nullable();
            Map(x => x.Version).Not.Nullable().Length(10);

            HasMany(x => x.ProjectEmployees).KeyColumn("EmployeeID").Inverse().Cascade.All();
            HasOne(x => x.Group).Cascade.All();

            Table("Employees");
        }
    }
}
