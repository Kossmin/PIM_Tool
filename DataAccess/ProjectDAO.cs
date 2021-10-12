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
        private List<ProjectObject> _db = new List<ProjectObject>()
        {
            new ProjectObject{ID = 1, GroupID = 1, Customer="KimSon", ProjectNumber = "123", ProjectName="PIM", StartDate=DateTime.Parse("2/12/2021"), EndDate= DateTime.Parse("2-2-2021"), Version = 5,},
            new ProjectObject{ID = 2, GroupID = 2, Customer="KimSon", ProjectNumber = "321", ProjectName="PIM2", StartDate=DateTime.Parse("2-2-2021"), EndDate= DateTime.Parse("2-2-2021"), Version = 5,},
            new ProjectObject{ID = 3, GroupID = 1, Customer="KimSon", ProjectNumber = "132", ProjectName="PIM4", StartDate=DateTime.Parse("2-2-2021"), EndDate= DateTime.Parse("2-2-2021"), Version = 5,},
            new ProjectObject{ID = 4, GroupID = 1, Customer="KimSon", ProjectNumber = "132", ProjectName="PIM6", StartDate=DateTime.Parse("2-2-2021"), EndDate= DateTime.Parse("2-2-2021"), Version = 5,},
            new ProjectObject{ID = 5, GroupID = 1, Customer="KimSon", ProjectNumber = "132", ProjectName="PIM4", StartDate=DateTime.Parse("2-2-2021"), EndDate= DateTime.Parse("2-2-2021"), Version = 5,},
            new ProjectObject{ID = 6, GroupID = 1, Customer="KimSon", ProjectNumber = "132", ProjectName="PIM8", StartDate=DateTime.Parse("2-2-2021"), EndDate= DateTime.Parse("2-2-2021"), Version = 5,},
            new ProjectObject{ID = 7, GroupID = 1, Customer="KimSon", ProjectNumber = "132", ProjectName="PIM3", StartDate=DateTime.Parse("2-2-2021"), EndDate= DateTime.Parse("2-2-2021"), Version = 5,},
            new ProjectObject{ID = 8, GroupID = 1, Customer="KimSon", ProjectNumber = "132", ProjectName="PIM5", StartDate=DateTime.Parse("2-2-2021"), EndDate= DateTime.Parse("2-2-2021"), Version = 5,},

        };

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

        public List<ProjectObject> GetProjectObjects(string status, string searchString, int pageIndex, int numberOfRow, string sortingKind)
        {
            List<ProjectObject> projectList = new List<ProjectObject>();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                projectList = (from a in _db where a.ProjectName == searchString select a).ToList();
            }
            else
            {
                projectList = _db;
            }

            switch (status)
            {
                case "0":
                    projectList = (from a in projectList where a.Status == ProjectObject.ProjectStatus.NEW select a).ToList();
                    break;
                case "1":
                    projectList = (from a in projectList where a.Status == ProjectObject.ProjectStatus.PLA select a).ToList();

                    break;
                case "2":
                    projectList = (from a in projectList where a.Status == ProjectObject.ProjectStatus.INP select a).ToList();

                    break;
                case "3":
                    projectList = (from a in projectList where a.Status == ProjectObject.ProjectStatus.FIN select a).ToList();

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

            List<ProjectObject> resultList = new List<ProjectObject>();
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
            List<ProjectObject> projectList = new List<ProjectObject>();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                projectList = (from a in _db where a.ProjectName == searchString select a).ToList();
            }
            else
            {
                projectList = _db;
            }

            switch (status)
            {
                case "0":
                    projectList = (from a in projectList where a.Status == ProjectObject.ProjectStatus.NEW select a).ToList();
                    break;
                case "1":
                    projectList = (from a in projectList where a.Status == ProjectObject.ProjectStatus.PLA select a).ToList();

                    break;
                case "2":
                    projectList = (from a in projectList where a.Status == ProjectObject.ProjectStatus.INP select a).ToList();

                    break;
                case "3":
                    projectList = (from a in projectList where a.Status == ProjectObject.ProjectStatus.FIN select a).ToList();

                    break;
                default:
                    break;
            }
            return projectList.Count;
        }

        private ProjectObject SearchByProjectNumber(string projectNumber)
        {
            return _db.SingleOrDefault(p => p.ProjectNumber == projectNumber);
        }

        public List<ProjectObject> SearchByID(IEnumerable<int> ids)
        {
            return (from a in _db where ids.Contains(a.ID) select a).ToList();
        }

        public bool Add(ProjectObject project)
        {
            var proj = SearchByProjectNumber(project.ProjectNumber);
            if ( proj != null)
            {
                return false;
            }
            else
            {
                _db.Add(project);
                return true;
            }
        }

        public bool Delete(IEnumerable<int> ids)
        {
            var projects = _db.Where(p => ids.Contains(p.ID)).ToList();
            for(int i = 0; i < projects.Count; i++)
            {
                _db.Remove(projects[i]);
            }
            return true;
        }

        public void Update(ProjectObject project)
        {
            var tmpProject = _db.Where(p => p.ID == project.ID).FirstOrDefault();
            if(tmpProject != null)
            {
                tmpProject.ProjectName = project.ProjectName;
                tmpProject.StartDate = project.StartDate;
                tmpProject.EndDate = project.EndDate;
                tmpProject.Status = project.Status;
                tmpProject.Customer = project.Customer;
                tmpProject.GroupID = project.GroupID;
            }
        }
    }
}
