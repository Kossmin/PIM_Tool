using BusinessObject;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        public bool Add(Project project)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                
                using (var transaction = session.BeginTransaction())
                {
                    var emp = session;
                    session.Save(project);
                    transaction.Commit();
                }
            }
            return true;
        }

        public bool Delete(IEnumerable<int> id)
        => ProjectDAO.Instance.Delete(id);

        public List<Project> GetAllProjectObject(PageModel pageModel)
        {
            IList<Project> projectList;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    if(String.IsNullOrWhiteSpace(pageModel.SearchString) && String.IsNullOrWhiteSpace(pageModel.Status))
                    {
                        projectList = session.CreateCriteria<Project>().List<Project>();
                    }else if (String.IsNullOrWhiteSpace(pageModel.SearchString))
                    {
                        projectList = session.CreateCriteria<Project>().List<Project>().Where(x=>x.Status.ToString() == pageModel.Status).ToList<Project>();
                    }
                    else
                    {
                        projectList = session.CreateCriteria<Project>().List<Project>().Where(x => x.ProjectName.Contains(pageModel.SearchString)).ToList<Project>();
                    }
                }
            }

            

            var key = "";
            if (pageModel.SortingKind != null)
            {
                pageModel.SortingKind = pageModel.SortingKind.ToLower();

                if (pageModel.SortingKind.Contains("projectnumber"))
                {
                    key = "projectnumber";
                }
                else if (pageModel.SortingKind.Contains("status"))
                {
                    key = "status";
                }
                else if (pageModel.SortingKind.Contains("customer"))
                {
                    key = "customer";
                }
                else if (pageModel.SortingKind.Contains("startdate"))
                {
                    key = "startdate";
                }
                else if (pageModel.SortingKind.Contains("projectname"))
                {
                    key = "projectname";
                }
            }

            switch (key)
            {
                case "projectnumber":
                    projectList = projectList.OrderBy(p => p.ProjectNumber).ToList();
                    break;
                case "status":
                    projectList = projectList.OrderBy(p => p.Status).ToList();
                    break;
                case "customer":
                    projectList = projectList.OrderBy(p => p.Customer).ToList();
                    break;
                case "startdate":
                    projectList = projectList.OrderBy(p => p.StartDate).ToList();
                    break;
                case "projectname":
                    projectList = projectList.OrderBy(p => p.ProjectName).ToList();
                    break;
                default:
                    projectList = projectList.OrderBy(p => p.ID).ToList();
                    break;
            }

            if (pageModel.SortingKind != null && !pageModel.SortingKind.Contains("up"))
            {
                projectList.Reverse();
            }

            List<Project> resultList = new List<Project>();
            int max;
            if (pageModel.PageIndex * pageModel.NumberOfRow < projectList.Count) max = pageModel.PageIndex * pageModel.NumberOfRow;
            else max = projectList.Count;
            for (int i = (pageModel.PageIndex - 1) * pageModel.NumberOfRow; i < max; i++)
            {
                resultList.Add(projectList.ElementAt(i));
            }
            return resultList;
        }

        public int GetMaxPageNumber(string status, string searchString)
        => ProjectDAO.Instance.GetMaxPageNumber(status, searchString);

        public List<Project> GetProjects(IEnumerable<int> id)
        => ProjectDAO.Instance.SearchByID(id);


        public void Update(Project project)
        => ProjectDAO.Instance.Update(project);
    }
}
