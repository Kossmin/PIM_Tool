using Autofac;
using BusinessObject;
using DataAccess.CustomException;
using DataAccess.Model;
using DataAccess.Repository;
using SPK_PIM.Helpers;
using SPK_PIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SPK_PIM.Controllers
{
    public class ProjectController : BaseController
    {
        private IProjectRepository _projectRepository;
        private IEmployeeRepository _employeeRepository;
        IContainer container = ContainerConfig.Configure();
        

        public ProjectController() 
        {
            _projectRepository = container.Resolve<IProjectRepository>();
            _employeeRepository = container.Resolve<IEmployeeRepository>();
        }

        public ActionResult Index(string _status, string _searchString, string _sortingKind, int _numberOfRows = 5, int _pageIndex = 1, bool isRemoved = false, bool _acsending = true)
        {
            ViewBag.acceptLanguage = Request.Headers.Get("Accept-Language").Split(',')[0];

            IndexPageModel indexPage = new IndexPageModel() {
                Status = _status,
                SearchString = _searchString,
                PageIndex = _pageIndex,
                SortingKind = _sortingKind,
                NumberOfRows = _numberOfRows,
                Acsending = _acsending,
            };

            var EntityState = new SelectList(Enum.GetValues(typeof(Project.ProjectStatus)).Cast<Project.ProjectStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            ViewBag._status = EntityState;
            ViewBag.isRemoved = isRemoved;

            var numberOfRecords = _projectRepository.GetNumberOfRecords(indexPage.Status, indexPage.SearchString);
            var maxPage = numberOfRecords / _numberOfRows;
            if(numberOfRecords % _numberOfRows != 0)
            {
                maxPage = maxPage + 1;
            }

            if (_pageIndex > maxPage)
            {
                indexPage.Projects = _projectRepository.GetAllProjectObject(new PageModel { SearchString = _searchString, NumberOfRow = _numberOfRows, PageIndex = maxPage, SortingKind = _sortingKind, Status = _status, IsAcsending = _acsending });
                indexPage.PageIndex = maxPage;
            }
            else
            {
                indexPage.Projects = _projectRepository.GetAllProjectObject(new PageModel { SearchString = _searchString, NumberOfRow = _numberOfRows, PageIndex = _pageIndex, SortingKind = _sortingKind, Status = _status, IsAcsending = _acsending });
            }


            indexPage.MaxPage = ( maxPage == 0) ? 1 : maxPage;
            return View(indexPage);
        }

        public ActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new IndexPageModel { Members = _employeeRepository.GetEmployees()});
        }

        
        public ActionResult BackToHome(string returnUrl)
        {
            if (returnUrl == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        [HttpPost]
        public ActionResult Create(IndexPageModel indexPage, IEnumerable<int> projectEmployees, string returnUrl=null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }
                try
                {
                    _projectRepository.Add(indexPage.Project, projectEmployees);
                }
                catch (DuplicateProjectNumberException e)
                {
                    ModelState.AddModelError("Project.ProjectNumber", e.Message);
                    throw new Exception();
                }
                
            }
            catch (Exception)
            {
                indexPage.Members = _employeeRepository.GetEmployees();
                return View(indexPage);
            }
            

            if (returnUrl == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        public ActionResult Details(int id, bool checkConcurrent = false, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            List<int> projectID = new List<int>() { id};
            var model = _projectRepository.GetProjects(projectID).FirstOrDefault();
            if(model == null)
            {
                return RedirectToAction("Index", new { isRemoved = true });
            }
            var empInProject = _employeeRepository.GetEmployeesIDInProject(id).ToList<int>();
            IndexPageModel indexPageModel = new IndexPageModel { Project = model, Members = _employeeRepository.GetEmployees() };
            if (checkConcurrent)
            {
                ViewBag.Concurrent = true;
            }
            ViewBag.Details = true;
            ViewData["SelectedEmployee"] = empInProject;
            return View("Create", indexPageModel);
        }

        public ActionResult DeleteOne(int id, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            List<int> projectId = new List<int>();
            projectId.Add(id);
            var projects = _projectRepository.GetProjects(projectId);
            return View("Delete", projects);
        }

        public ActionResult Delete(IEnumerable<int> projectIds, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            var projects = _projectRepository.GetProjects(projectIds);
            return View(projects);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(IEnumerable<int> projectIds, string returnUrl = null, int projectId = -1)
        {
            if(projectId != -1)
            {
                projectIds.ToList().Add(projectId);
            }
            _projectRepository.Delete(projectIds);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(IndexPageModel indexModel, IEnumerable<int> projectEmployees, string returnUrl = null)
        {
            try
            {
                _projectRepository.Update(indexModel.Project, projectEmployees);
            }
            catch (Exception)
            {
                return RedirectToAction("Details", new { id = indexModel.Project.ID, checkConcurrent = true });
                throw;
            }

            if (returnUrl == null)
            {
                return RedirectToAction("Index");
            }
            return Redirect(returnUrl);
        }

        public ActionResult SetCulture(string culture, string returnUrl = null)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            if(returnUrl == null){
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }
    }
}