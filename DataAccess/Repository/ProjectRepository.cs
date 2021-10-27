using BusinessObject;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataAccess.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        public void Add(Project project, IEnumerable<int> empIds)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using(var transactionScope = new TransactionScope())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        project.AddEmployee(session.CreateCriteria<Employee>().List<Employee>().Where(x => empIds.Contains(x.ID)));
                        session.Save(project);
                        transaction.Commit();
                    }
                    transactionScope.Complete();
                }
            }
        }

        public void Delete(IEnumerable<int> id)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using(var transactionScope = new TransactionScope())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        var temp = session.Query<Project>().Where(x => id.Contains(x.ID)).ToList();
                        foreach (var item in temp)
                        {
                            session.Delete(item);
                        }
                        tx.Commit();
                    }
                    transactionScope.Complete();
                }
            }
        }

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
                        var temp = Enum.Parse(typeof(Project.ProjectStatus), pageModel.Status);
                        projectList = session.CreateCriteria<Project>().List<Project>().Where(x=>x.Status.ToString() == temp).ToList<Project>();
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
        {
            IList<Project> projectList;
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    if (String.IsNullOrWhiteSpace(searchString) && String.IsNullOrWhiteSpace(status))
                    {
                        projectList = session.CreateCriteria<Project>().List<Project>();
                    }
                    else if (String.IsNullOrWhiteSpace(searchString))
                    {
                        var temp = Enum.Parse(typeof(Project.ProjectStatus), status);
                        projectList = session.CreateCriteria<Project>().List<Project>().Where(x => x.Status.ToString() == temp).ToList<Project>();
                    }
                    else
                    {
                        projectList = session.CreateCriteria<Project>().List<Project>().Where(x => x.ProjectName.Contains(searchString)).ToList<Project>();
                    }
                }
            }
            return projectList.Count();
        }



        public List<Project> GetProjects(IEnumerable<int> id)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    return session.Query<Project>().Where(x => id.Contains(x.ID)).ToList();
                }
            }
        }


        public void Update(Project project, IEnumerable<int> empIds)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using(var transactionScope = new TransactionScope())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        var tmpProject = session.Query<Project>().FirstOrDefault(x => x.ID == project.ID);
                        var tmpEmpList = session.Query<Employee>().Where(x => empIds.Contains(x.ID)).ToList();
                        if (tmpProject != null)
                        {
                            tmpProject.ProjectName = project.ProjectName;
                            tmpProject.StartDate = project.StartDate;
                            tmpProject.EndDate = project.EndDate;
                            tmpProject.Status = project.Status;
                            tmpProject.Customer = project.Customer;
                            tmpProject.Group = project.Group;
                            tmpProject.Version = project.Version;
                            tmpProject.AddEmployee(tmpEmpList);
                        }
                        session.Update(tmpProject);
                        tx.Commit();
                    }
                    transactionScope.Complete();
                }
            }
        }

        private Project SearchByProjectNumber(string projectNumber)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    return session.Query<Project>().FirstOrDefault(x => x.ProjectNumber == projectNumber);
                }
            }
        }
    }
}
