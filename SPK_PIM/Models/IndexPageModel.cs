using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPK_PIM.Models
{
    public class IndexPageModel
    {
        public string Status { get; set; }
        public string SearchString { get; set; }
        public int NumberOfRows { get; set; } = 5;
        public int PageIndex { get; set; } = 1;
        public string SortingKind { get; set; } = "ID";
        public List<Project> Projects { get; set; }
        public int MaxPage { get; set; }
        public Project Project { get; set; } = new Project();
        public IList<Employee> Members { get; set; }
        public bool Acsending { get; set; } = true;
    }
}