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
        IProjectRepository _projectRepository = new ProjectRepository();
        IEmployeeRepository _employeeRepository = new EmployeeRepository();

        public ActionResult Index(string _status, string _searchString, string _sortingKind, int _numberOfRows = 5, int _pageIndex = 1, bool isRemoved = false)
        {
            ViewBag.acceptLanguage = Request.Headers.Get("Accept-Language").Split(',')[0];

            if (_status == "null")
            {
                _status = null;
            }
            if(_searchString == "null")
            {
                _searchString = null;
            }
            IndexPageModel indexPage = new IndexPageModel() {
                Status = _status,
                _SearchString = _searchString,
                _PageIndex = _pageIndex,
                _SortingKind = _sortingKind,
                 _NumberOfRows = _numberOfRows
            };

            var EntityState = new SelectList(Enum.GetValues(typeof(Project.ProjectStatus)).Cast<Project.ProjectStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            ViewBag._status = EntityState;

            if (isRemoved)
            {
                ViewBag.isRemoved = true;
            }

            indexPage._Projects =  _projectRepository.GetAllProjectObject(new PageModel { SearchString = _searchString, NumberOfRow = _numberOfRows, PageIndex = _pageIndex, SortingKind = _sortingKind, Status = _status});
            var maxPage = _projectRepository.GetMaxPageNumber(indexPage.Status, indexPage._SearchString);
            indexPage._MaxPage = ( maxPage == 0) ? 1 : maxPage;
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
                _projectRepository.Add(indexPage._Project, projectEmployees);
            }
            catch (DuplicateProjectNumberException e)
            {
                ModelState.AddModelError("_Project.ProjectNumber", e.Message);
            }
            if (!ModelState.IsValid)
            {
                indexPage.Members = _employeeRepository.GetEmployees();
                return View(indexPage);
            }
            //if(_projectRepository.SearchByProjectNumber(indexPage._Project.ProjectNumber) == null)
            //{
            //    _projectRepository.Add(indexPage._Project, projectEmployees);
            //}
            
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
            List<int> tmp = new List<int>();
            tmp.Add(id);
            var model = _projectRepository.GetProjects(tmp).FirstOrDefault();
            if(model == null)
            {
                return RedirectToAction("Index", new { isRemoved = true });
            }
            var tmpEmp = _employeeRepository.GetEmployeesIDInProject(id).ToList<int>();
            IndexPageModel indexPageModel = new IndexPageModel { _Project = model, Members = _employeeRepository.GetEmployees() };
            if (checkConcurrent)
            {
                ViewBag.Concurrent = true;
            }
            ViewBag.Details = true;
            ViewData["SelectedEmployee"] = tmpEmp;
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
            if(!_projectRepository.Update(indexModel._Project, projectEmployees))
            {
                return RedirectToAction("Index", new { id = indexModel._Project.ID, checkConcurrent = true });
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