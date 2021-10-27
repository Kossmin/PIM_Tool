using BusinessObject;
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

        public ActionResult Index(string _status, string _searchString, string _sortingKind, int _numberOfRows = 5, int _pageIndex = 1)
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
            if (!ModelState.IsValid)
            {
                indexPage.Members = _employeeRepository.GetEmployees();
                return View(indexPage);
            }
            _projectRepository.Add(indexPage._Project, projectEmployees);
            if (returnUrl == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        public ActionResult Details(int id)
        {
            List<int> tmp = new List<int>();
            tmp.Add(id);
            var model = _projectRepository.GetProjects(tmp)[0];
            IndexPageModel indexPageModel = new IndexPageModel { _Project = model, Members = _employeeRepository.GetEmployees() };
            ViewBag.Details = true;
            return View("Create", indexPageModel);
        }

        public ActionResult Delete(IEnumerable<int> employeeIds, string returnUrl = null)
        {
            var projects = _projectRepository.GetProjects(employeeIds);
            return View(projects);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(IEnumerable<int> employeeIds, string returnUrl = null)
        {
            _projectRepository.Delete(employeeIds);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(IndexPageModel indexModel, IEnumerable<int> projectEmployees, string returnUrl)
        {
            _projectRepository.Update(indexModel._Project, projectEmployees);
            if(returnUrl == null)
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