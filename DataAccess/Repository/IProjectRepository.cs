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
        void Add(Project project, IEnumerable<int> empIds);
        void Update(Project project, IEnumerable<int> empIds);
        int GetMaxPageNumber(string status, string searchString);
        void Delete(IEnumerable<int> id);
        List<Project> GetProjects(IEnumerable<int> ids);
    }
}
