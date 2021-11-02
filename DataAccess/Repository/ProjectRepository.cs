using BusinessObject;
using DataAccess.CustomException;
using DataAccess.Model;
using NHibernate.Criterion;
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
                        if (SearchByProjectNumber(project.ProjectNumber) != null)
                        {
                            throw new DuplicateProjectNumberException();
                        }
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
                            //item.DeleteEmployee();
                            if (checkCanBeRemoved(item))
                            {
                                session.Delete(item);
                            }
                        }
                        tx.Commit();
                    }
                    transactionScope.Complete();
                }
            }
        }

        public bool checkCanBeRemoved(Project project)
        {
            using(var session = NHibernateHelper.OpenSession())
            {
                var checkedProject = session.CreateCriteria<Project>().List<Project>().FirstOrDefault(x => x.ID == project.ID);
                var status = (Project.ProjectStatus)0;
                if (checkedProject.Status == status)
                {
                    return true;
                }
                else
                {
                    return false;
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
                    
                    if(pageModel.IsAcsending == true)
                    {
                        if (String.IsNullOrWhiteSpace(pageModel.SearchString) && String.IsNullOrWhiteSpace(pageModel.Status))
                        {
                            projectList = session.CreateCriteria<Project>()
                                 .SetFirstResult(pageModel.NumberOfRow * (pageModel.PageIndex - 1))
                                 .SetMaxResults(pageModel.NumberOfRow)
                                 .AddOrder(Order.Asc("Status"))
                                 .List<Project>();
                                
                        }
                        else if (String.IsNullOrWhiteSpace(pageModel.SearchString))
                        {
                            var searchString = Enum.Parse(typeof(Project.ProjectStatus), pageModel.Status);
                            projectList = session.CreateCriteria<Project>().List<Project>()
                                .Where(x => x.Status.ToString() == searchString)
                                .OrderBy(x => x.ID)
                                .Skip(pageModel.NumberOfRow * (pageModel.PageIndex - 1))
                                .Take(pageModel.NumberOfRow)
                                .ToList();
                        }
                        else
                        {
                            projectList = session.CreateCriteria<Project>().List<Project>()
                                .Where(x => x.ProjectName.Contains(pageModel.SearchString) && x.ProjectName.Contains(pageModel.SearchString) && x.Customer.Contains(pageModel.SearchString))
                                .OrderBy(x => x.ID)
                                .Skip(pageModel.NumberOfRow * (pageModel.PageIndex - 1))
                                .Take(pageModel.NumberOfRow)
                                .ToList<Project>();
                        }
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(pageModel.SearchString) && String.IsNullOrWhiteSpace(pageModel.Status))
                        {
                            projectList = session.CreateCriteria<Project>().List<Project>()
                                .OrderByDescending(x => x.ID)
                                .Skip(pageModel.NumberOfRow * (pageModel.PageIndex - 1))
                                .Take(pageModel.NumberOfRow)
                                .ToList();
                        }
                        else if (String.IsNullOrWhiteSpace(pageModel.SearchString))
                        {
                            var searchString = Enum.Parse(typeof(Project.ProjectStatus), pageModel.Status);
                            projectList = session.CreateCriteria<Project>().List<Project>().Where(x => x.Status.ToString() == searchString)
                                .OrderByDescending(x => x.ID)
                                .Skip(pageModel.NumberOfRow * (pageModel.PageIndex - 1))
                                .Take(pageModel.NumberOfRow)
                                .ToList();
                        }
                        else
                        {
                            projectList = session.CreateCriteria<Project>().List<Project>()
                                .Where(x => x.ProjectName.Contains(pageModel.SearchString) && x.ProjectName.Contains(pageModel.SearchString) && x.Customer.Contains(pageModel.SearchString))
                                .OrderByDescending(x => x.ID)
                                .Skip(pageModel.NumberOfRow * (pageModel.PageIndex - 1))
                                .Take(pageModel.NumberOfRow)
                                .ToList();
                        }
                    }
                }
            }


            switch (pageModel.SortingKind)
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

            if (pageModel.IsAcsending == false)
            {
                projectList = projectList.Reverse().ToList();
            }

            return projectList.ToList();
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
                //using (var tx = session.BeginTransaction())
                //{
                    return session.Query<Project>().Where(x => id.Contains(x.ID)).ToList();
                //}
            }
        }


        public bool Update(Project project, IEnumerable<int> empIds)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using(var transactionScope = new TransactionScope())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        var tmpProject = session.Query<Project>().FirstOrDefault(x => x.ID == project.ID && x.Version == project.Version);
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
                            tmpProject.UpdateEmployee(tmpEmpList);
                        }
                        else
                        {
                            return false;
                        }
                        session.Update(tmpProject);
                        tx.Commit();
                    }
                    transactionScope.Complete();
                    return true;
                }
            }
        }

        public Project SearchByProjectNumber(string projectNumber)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session.Query<Project>().FirstOrDefault(x => x.ProjectNumber == projectNumber);
            }
        }
    }
}
