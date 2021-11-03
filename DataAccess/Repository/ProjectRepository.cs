using BusinessObject;
using DataAccess.CustomException;
using DataAccess.Model;
using NHibernate;
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
                        var duplicateProject = SearchByProjectNumber(project.ProjectNumber);
                        if (duplicateProject != null)
                        {
                            throw new DuplicateProjectNumberException();
                        }
                        project.AddEmployees(session.CreateCriteria<Employee>().Add(Expression.In(nameof(Project.ID), empIds.ToArray())).List<Employee>());
                        session.Save(project);
                        
                        transaction.Commit();

                        
                    }
                    transactionScope.Complete();
                }
            }
        }

        public void Delete(IEnumerable<int> projectIds)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using(var transactionScope = new TransactionScope())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        var deleteList = session.CreateCriteria<Project>()
                            .Add(Expression.In(nameof(Project.ID), projectIds.ToArray()))
                            .Add(Expression.Eq(nameof(Project.Status), Project.ProjectStatus.NEW))
                            .List();
                        foreach (var item in deleteList)
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
            ICriteria projectList;
            using (var session = NHibernateHelper.OpenSession())
            {
                projectList = session.CreateCriteria<Project>();
                var sortWord = "";
                

                switch (pageModel.SortingKind)
                {
                    case "projectnumber":
                        sortWord = nameof(Project.ProjectNumber);
                        break;
                    case "projectname":
                        sortWord = nameof(Project.ProjectName);
                        break;
                    case "status":
                        sortWord = nameof(Project.Status);
                        break;
                    case "customer":
                        sortWord = nameof(Project.Customer);
                        break;
                    case "startdate":
                        sortWord = nameof(Project.StartDate);
                        break;
                    default:
                        sortWord = nameof(Project.ProjectNumber);
                        break;
                }

                var orderDirection = new Order(sortWord, pageModel.IsAcsending);
                projectList.AddOrder(orderDirection);

                if (String.IsNullOrWhiteSpace(pageModel.SearchString) && String.IsNullOrWhiteSpace(pageModel.Status))
                {
                    projectList = projectList;

                }
                else if (String.IsNullOrWhiteSpace(pageModel.SearchString))
                {
                    var searchString = Enum.Parse(typeof(Project.ProjectStatus), pageModel.Status);
                    projectList = projectList
                        .Add(Expression.Eq(nameof(Project.Status), searchString));
                }
                else if (String.IsNullOrWhiteSpace(pageModel.Status))
                {
                    projectList = projectList
                        .Add(Expression.Or(Expression.Or(
                            Expression.Like(nameof(Project.ProjectNumber), "%" + pageModel.SearchString + "%"),
                            Expression.Like(nameof(Project.Customer), "%" + pageModel.SearchString + "%")
                            ), Expression.Like(nameof(Project.ProjectName), "%" + pageModel.SearchString + "%")));
                }
                else
                {
                    var searchString = Enum.Parse(typeof(Project.ProjectStatus), pageModel.Status);
                    projectList = projectList
                        .Add(Expression.Or(Expression.Or(
                            Expression.Like(nameof(Project.ProjectNumber), "%" + pageModel.SearchString + "%"),
                            Expression.Like(nameof(Project.Customer), "%" + pageModel.SearchString + "%")
                            ), Expression.Like(nameof(Project.ProjectName), "%" + pageModel.SearchString + "%")))
                        .Add(Expression.Eq(nameof(Project.Status), searchString));
                }
                if(pageModel.SortingKind == "max")
                {
                    return projectList.List<Project>().ToList();
                }
                projectList = projectList
                         .SetFirstResult(pageModel.NumberOfRow * (pageModel.PageIndex - 1))
                         .SetMaxResults(pageModel.NumberOfRow);

                return projectList.List<Project>().ToList();
            }

            
        }



        public int GetMaxPageNumber(string status, string searchString)
        {
            return GetAllProjectObject(new PageModel { Status = status, SearchString = searchString, SortingKind = "max" }).Count();
        }



        public List<Project> GetProjects(IEnumerable<int> id)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session.Query<Project>().Where(x => id.Contains(x.ID)).ToList();
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
                        var Project = session.Query<Project>().FirstOrDefault(x => x.ID == project.ID && x.Version == project.Version);
                        if (Project != null)
                        {
                            var EmpList = session.Query<Employee>().Where(x => empIds.Contains(x.ID)).ToList();
                            Project.ProjectName = project.ProjectName;
                            Project.StartDate = project.StartDate;
                            Project.EndDate = project.EndDate;
                            Project.Status = project.Status;
                            Project.Customer = project.Customer;
                            Project.Group = project.Group;
                            Project.Version = project.Version;
                            Project.UpdateEmployee(EmpList);
                        }
                        else
                        {
                            throw new Exception("Concurrent Update");
                        }
                        session.Update(Project);
                        tx.Commit();
                    }
                    transactionScope.Complete();
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
