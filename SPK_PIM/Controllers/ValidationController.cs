using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SPK_PIM.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : Controller
    {
        IProjectRepository _projectRepository = new ProjectRepository();
        // GET: Validation
        public JsonResult Is_Valid(string projectNumber)
        {
            var tmpProject = _projectRepository.SearchByProjectNumber(projectNumber);
            if(tmpProject == null)
            {
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
    }
}