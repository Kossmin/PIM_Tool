using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Project_Employee
    {
        public virtual Project Project { get; set; }
        public virtual Employee Employee { get; set; }

        public override bool Equals(object obj)
        {
            var emp = (Project_Employee)obj;
            return emp.Project.ID == Project.ID && emp.Employee.ID == Employee.ID;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Project.ID, Employee.ID).GetHashCode(); ;
        }

        public virtual void AddProject(Project project)
        {
            Project = project;
            project.ProjectEmployees.Add(this);
        }
    }

    class Project_EmployeeMapping : ClassMap<Project_Employee>
    {
        public Project_EmployeeMapping()
        {
            Table("ProjectEmployee");
            CompositeId()
                .KeyReference(x => x.Employee)
                .KeyReference(x => x.Project);
            
        }
    }
}
