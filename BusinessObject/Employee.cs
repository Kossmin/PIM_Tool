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

        public virtual IList<Project> Projects { get; set; }
        public virtual Group Group { get; set; }
    }
}
