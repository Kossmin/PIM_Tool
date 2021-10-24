using BusinessObject;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProjectRepository
    {
        List<Project> GetAllProjectObject(PageModel pageModel);
        bool Add(Project project);
        void Update(Project project);
        int GetMaxPageNumber(string status, string searchString);
        bool Delete(IEnumerable<int> id);
        List<Project> GetProjects(IEnumerable<int> ids);
    }
}
