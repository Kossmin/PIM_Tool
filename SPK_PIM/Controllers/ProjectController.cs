using BusinessObject;
using DataAccess.Repository;
using SPK_PIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SPK_PIM.Controllers
{
    public class ProjectController : Controller
    {
        IProjectRepository _projectRepository = new ProjectRepository();

        public ActionResult Index(string _status, string _searchString, string _sortingKind, int _numberOfRows = 5, int _pageIndex = 1)
        {
            IndexPageModel indexPage = new IndexPageModel() {
                Status = _status,
                _SearchString = _searchString,
                _PageIndex = _pageIndex,
                _SortingKind = _sortingKind,
                 _NumberOfRows = _numberOfRows
            };

            var EntityState = new SelectList(Enum.GetValues(typeof(ProjectObject.ProjectStatus)).Cast<ProjectObject.ProjectStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            ViewBag._status = EntityState;

            indexPage._Projects =  _projectRepository.GetAllProjectObject(indexPage.Status, indexPage._SearchString, indexPage._PageIndex, indexPage._NumberOfRows, indexPage._SortingKind);
            indexPage._MaxPage = _projectRepository.GetMaxPageNumber(indexPage.Status, indexPage._SearchString);
            return View(indexPage);
        }

        public ActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new IndexPageModel());
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
        public ActionResult Create(IndexPageModel indexPage, string returnUrl=null)
        {
            if (ModelState.IsValid)
            {
                _projectRepository.Add(indexPage._Project);
                if (returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            List<int> tmp = new List<int>();
            tmp.Add(id);
            var model = _projectRepository.GetProjects(tmp)[0];
            IndexPageModel indexPageModel = new IndexPageModel { _Project = model };
            ViewBag.Details = true;
            return View("Create", indexPageModel);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
        public ActionResult Edit(IndexPageModel indexModel, string returnUrl)
        {
            _projectRepository.Update(indexModel._Project);
            if(returnUrl == null)
            {
                return RedirectToAction("Index");
            }
            return Redirect(returnUrl);
        }
    }
}