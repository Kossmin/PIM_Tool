using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IEmployeeRepository
    {
        IList<Employee> GetEmployees();
        IList<Employee> GetEmployeesInProject(int projId);
        //IList<Employee> GetEmployeesNotInProject(int projId);
    }
}
