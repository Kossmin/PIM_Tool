using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Helpers;

namespace DataAccess
{
    public class ProjectDAO
    {

        private List<Project> _db;


        private static ProjectDAO instance = null;
        private ProjectDAO() { }
        public static ProjectDAO Instance
        {
            get
            {
              
                    if (instance == null)
                    {
                        instance = new ProjectDAO();
                    }
                    return instance;
            }
        }

        public List<Project> GetProjectObjects(string status, string searchString, int pageIndex, int numberOfRow, string sortingKind)
        {
            IList<Project> projectList;
            using(var session = NHibernateHelper.OpenSession())
            {
                using(var tx = session.BeginTransaction())
                {
                    projectList = session.CreateCriteria<Project>().List<Project>();
                    if(projectList == null)
                    {
                        projectList = new List<Project>();
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                projectList = (from a in projectList where a.ProjectName.ToLower().Contains(searchString.ToLower()) select a).ToList();
            }
            else
            {
                //projectList = _db;
            }

            switch (status)
            {
                case "0":
                    projectList = (from a in projectList where a.Status == Project.ProjectStatus.NEW select a).ToList();
                    break;
                case "1":
                    projectList = (from a in projectList where a.Status == Project.ProjectStatus.PLA select a).ToList();

                    break;
                case "2":
                    projectList = (from a in projectList where a.Status == Project.ProjectStatus.INP select a).ToList();

                    break;
                case "3":
                    projectList = (from a in projectList where a.Status == Project.ProjectStatus.FIN select a).ToList();

                    break;
                default:
                    break;
            }

            var key = "";
            if (sortingKind != null)
            {
                sortingKind = sortingKind.ToLower();

                if (sortingKind.Contains("projectnumber"))
                {
                    key = "projectnumber";
                }
                else if (sortingKind.Contains("status"))
                {
                    key = "status";
                }
                else if (sortingKind.Contains("customer"))
                {
                    key = "customer";
                }
                else if (sortingKind.Contains("startdate"))
                {
                    key = "startdate";
                }else if (sortingKind.Contains("projectname"))
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
                    projectList = projectList.OrderBy(p=>p.ID).ToList();
                    break;
            }

            if (sortingKind!=null &&!sortingKind.Contains("up"))
            {
                projectList.Reverse();
            }

            List<Project> resultList = new List<Project>();
            int max;
            if (pageIndex * numberOfRow < projectList.Count) max = pageIndex * numberOfRow;
            else max = projectList.Count;
            for (int i = (pageIndex-1)*numberOfRow; i<max; i++)
            {
                resultList.Add(projectList.ElementAt(i));
            }
            return resultList;
        }

        public int GetMaxPageNumber(string status, string searchString)
        {
            List<Project> projectList = new List<Project>();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                projectList = (from a in _db where a.ProjectName.ToLower().Contains(searchString.ToLower()) select a).ToList();
            }
            else
            {
                //projectList = _db;
            }

            switch (status)
            {
                case "0":
                    projectList = (from a in projectList where a.Status == Project.ProjectStatus.NEW select a).ToList();
                    break;
                case "1":
                    projectList = (from a in projectList where a.Status == Project.ProjectStatus.PLA select a).ToList();

                    break;
                case "2":
                    projectList = (from a in projectList where a.Status == Project.ProjectStatus.INP select a).ToList();

                    break;
                case "3":
                    projectList = (from a in projectList where a.Status == Project.ProjectStatus.FIN select a).ToList();

                    break;
                default:
                    break;
            }
            return projectList.Count;
        }

        private Project SearchByProjectNumber(string projectNumber)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using(var tx = session.BeginTransaction())
                {
                    return session.Query<Project>().FirstOrDefault(x => x.ProjectNumber == projectNumber);
                }
            }
        }

        public List<Project> SearchByID(IEnumerable<int> ids)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    return session.Query<Project>().Where(x => ids.Contains(x.ID)).ToList();
                }
            }
        }

        public bool Add(Project project)
        {
            //var proj = SearchByProjectNumber(project.ProjectNumber);
            //if ( proj != null)
            //{
            //    return false;
            //}
            //else
            //{
            //    _db.Add(project);
            //    return true;
            //}

            using (var session = NHibernateHelper.OpenSession())
            {

                using (var transaction = session.BeginTransaction())
                {
                    session.Save(project);
                    transaction.Commit();
                }
            }
            return true;

        }

        public bool Delete(IEnumerable<int> ids)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var temp =  session.Query<Project>().Where(x => ids.Contains(x.ID)).ToList();
                    foreach (var item in temp)
                    {
                        session.Delete(item);
                    }
                    tx.Commit();
                }
            }
            return true;
        }

        public void Update(Project project)
        {

            using (var session = NHibernateHelper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var tmpProject = session.Query<Project>().FirstOrDefault(x => x.ID == project.ID);
                    if (tmpProject != null)
                    {
                        tmpProject.ProjectName = project.ProjectName;
                        tmpProject.StartDate = project.StartDate;
                        tmpProject.EndDate = project.EndDate;
                        tmpProject.Status = project.Status;
                        tmpProject.Customer = project.Customer;
                        tmpProject.Group = project.Group;
                    }
                    tx.Commit();
                }
            }
        }
    }
}
