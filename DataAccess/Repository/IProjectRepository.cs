using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProjectRepository
    {
        List<ProjectObject> GetAllProjectObject(string status, string searchString, int pageIndex, int numberOfRow, string sortingKind);
        bool Add(ProjectObject project);
        void Update(ProjectObject project);
        int GetMaxPageNumber(string status, string searchString);
        bool Delete(IEnumerable<int> id);
        List<ProjectObject> GetProjects(IEnumerable<int> ids);
    }
}
