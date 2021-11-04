using BusinessObject;
using DataAccess.CustomException;
using DataAccess.Model;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessLogic
{
    public class BusinessService : IBusinessService
    {
        private IProjectRepository _projectRepository;
        private IEmployeeRepository _employeeRepository;

        public BusinessService(IProjectRepository projectRepository, IEmployeeRepository employeeRepository)
        {
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
        }

        public void AddNewProject(Project project, IEnumerable<int> projectEmployees)
        {
                _projectRepository.Add(project, projectEmployees);
        }


        public Project SearchProjectById(int projecttId)
        {
            List<int> projectID = new List<int>() { projecttId };
            return _projectRepository.GetProjects(projectID).FirstOrDefault();
        }

        public List<Project> GetDetailOfSelectedProjects(IEnumerable<int> projectIds)
        {
            return _projectRepository.GetProjects(projectIds);
        }


        public int CountNumberOfRecords(string status, string searchString)
        {
            return _projectRepository.GetNumberOfRecords(status, searchString);
        }

        public int CountMaxPage(string status, string searchString, int numberOfRows)
        {
            var numberOfRecords = CountNumberOfRecords(status, searchString);
            var maxPage = numberOfRecords / numberOfRows;
            if (numberOfRecords % numberOfRows != 0)
            {
                maxPage++;
            }
            return maxPage;
        }

        public void DeleteProjects(IEnumerable<int> projectIds)
        {
            _projectRepository.Delete(projectIds);
        }

        public void Update(IEnumerable<int> projectEmployees, Project project)
        {
            _projectRepository.Update(project, projectEmployees);
        }

        public List<Project> GetProjects(PageModel pageModel)
        {
            return _projectRepository.GetAllProjectObject(pageModel);
        }

        public List<Employee> GetEmployees()
        {
            return _employeeRepository.GetEmployees().ToList();
        }

        public List<int> GetEmployeesIDInProject(int projectId)
        {
            return _employeeRepository.GetEmployeesIDInProject(projectId).ToList<int>();
        }
    }
}
