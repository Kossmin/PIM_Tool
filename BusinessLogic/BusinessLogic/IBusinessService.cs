using BusinessObject;
using DataAccess.Model;
using System.Collections.Generic;

namespace BusinessLogic.BusinessLogic
{
    public interface IBusinessService
    {
        void AddNewProject(Project project, IEnumerable<int> projectEmployees);
        int CountMaxPage(string status, string searchString, int numberOfRows);
        int CountNumberOfRecords(string status, string searchString);
        void DeleteProjects(IEnumerable<int> projectIds);
        List<Project> GetDetailOfSelectedProjects(IEnumerable<int> projectIds);
        List<Employee> GetEmployees();
        List<int> GetEmployeesIDInProject(int projectId);
        List<Project> GetProjects(PageModel pageModel);
        Project SearchProjectById(int projecttId);
        void Update(IEnumerable<int> projectEmployees, Project project);
    }
}