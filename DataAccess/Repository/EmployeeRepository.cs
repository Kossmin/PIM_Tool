using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public IList<Employee> GetEmployees()
        {
            using(var session = NHibernateHelper.OpenSession())
            {
                using(var tx = session.BeginTransaction())
                {
                    return session.CreateCriteria<Employee>().List<Employee>();
                }
            }
        }

        public IList<Employee> GetEmployeesInProject(int projectId)
        {
            IList<Employee> emp = new List<Employee>();
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var project = session.CreateCriteria<Project>().List<Project>().First(x => x.ID == projectId);
                    emp = session.CreateCriteria<Employee>().List<Employee>().Where(x => x.Projects.Contains(project)).ToList<Employee>();
                }
            }
            return emp;
        }

        public IList<int> GetEmployeesIDInProject(int projectId)
        {
            IList<int> emp = new List<int>();
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var project = session.CreateCriteria<Project>().List<Project>().First(x => x.ID == projectId);
                    emp = session.CreateCriteria<Employee>().List<Employee>().Where(x => x.Projects.Contains(project)).Select(x=>x.ID).ToList();
                }
            }
            return emp;
        }

        //public IList<Employee> GetEmployeesNotInProject(int projectId)
        //{
        //    IList<Employee> emp = new List<Employee>();
        //    using (var session = NHibernateHelper.OpenSession())
        //    {
        //        using (var tx = session.BeginTransaction())
        //        {
        //            var proj = session.CreateCriteria<Project>().List<Project>().Where(x => x.ID == projectId).FirstOrDefault();
        //            if (proj != null)
        //            {
        //                var projectEmployees = session.CreateCriteria<Project_Employee>().List<Project_Employee>().Where(x => x.Project.ID != proj.ID).ToList();

        //                foreach (var item in projectEmployees)
        //                {
        //                    emp.Add(item.Employee);
        //                }
        //            }
        //        }
        //    }
        //    return emp;
        //}
    }
}
