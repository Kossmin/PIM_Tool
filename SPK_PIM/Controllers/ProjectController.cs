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
                _Status = _status,
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

            indexPage._Projects =  _projectRepository.GetAllProjectObject(indexPage._Status, indexPage._SearchString, indexPage._PageIndex, indexPage._NumberOfRows, indexPage._SortingKind);
            indexPage._MaxPage = _projectRepository.GetMaxPageNumber(indexPage._Status, indexPage._SearchString);
            return View(indexPage);
        }

        public ActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Create(IndexPageModel indexPage, string returnUrl=null)
        {
            if (ModelState.IsValid)
            {
                _projectRepository.Add(indexPage._Project);
            }
            return Redirect(returnUrl);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Delete(int id, string returnUrl = null)
        {
            if(_projectRepository.GetProject(id)!=null) return View(_projectRepository.GetProject(id));
            else
            {
                throw new Exception();
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string returnUrl = null)
        {
            _projectRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}