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
        public virtual int ProjectID { get; set; }
        public virtual int EmployeeID { get; set; }

        public override bool Equals(object obj)
        {
            var emp = (Project_Employee)obj;
            return emp.ProjectID == ProjectID && emp.EmployeeID == EmployeeID;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(ProjectID, EmployeeID).GetHashCode(); ;
        }

    }

    class Project_EmployeeMapping : ClassMap<Project_Employee>
    {
        public Project_EmployeeMapping()
        {
            Table("ProjectEmployee");
            CompositeId()
                .KeyProperty(x => x.EmployeeID)
                .KeyProperty(x => x.ProjectID);
            //References<int>(x => x.ProjectID).ForeignKey("ProjectID");
            //References<int>(x => x.EmployeeID).ForeignKey("EmployeeID");
        }
    }
}
