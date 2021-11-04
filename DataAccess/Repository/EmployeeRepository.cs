using BusinessObject;
using NHibernate.Criterion;
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
                    var project = session.CreateCriteria<Project>().Add(Expression.Eq(nameof(Project.ID), projectId)).List<Project>().First();
                    emp = session.CreateCriteria<Employee>().Add(Expression.In(nameof(Employee.ID), project.Employees.ToArray())).List<Employee>();
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
                    var project = session.CreateCriteria<Project>().Add(Expression.Eq(nameof(Project.ID), projectId)).List<Project>().First();
                    var employeeIdList = new List<int>();
                    foreach (var item in project.Employees)
                    {
                        employeeIdList.Add(item.ID);
                    }
                    emp = session.CreateCriteria<Employee>().Add(Expression.In(nameof(Employee.ID), employeeIdList)).List<Employee>().Select(x=>x.ID).ToList();
                }
            }
            return emp;
        }

    }
}
