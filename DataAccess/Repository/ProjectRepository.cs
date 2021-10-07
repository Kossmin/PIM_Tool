﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        public bool Add(ProjectObject project)
        => ProjectDAO.Instance.Add(project);

        public bool Delete(int id)
        => ProjectDAO.Instance.Delete(id);

        public List<ProjectObject> GetAllProjectObject(string status, string searchString, int pageIndex, int numberOfRow, string sortingKind)
            => ProjectDAO.Instance.GetProjectObjects(status, searchString, pageIndex, numberOfRow, sortingKind);

        public int GetMaxPageNumber(string status, string searchString)
        => ProjectDAO.Instance.GetMaxPageNumber(status, searchString);

        public ProjectObject GetProject(int id)
        => ProjectDAO.Instance.SearchByID(id);

        public bool Update(ProjectObject project)
        {
            throw new NotImplementedException();
        }
    }
}