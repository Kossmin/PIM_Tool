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
        public string _SearchString { get; set; }
        public int _NumberOfRows { get; set; } = 5;
        public int _PageIndex { get; set; } = 1;
        public string _SortingKind { get; set; } = "ID";
        public List<Project> _Projects { get; set; }
        public int _MaxPage { get; set; }
        public Project _Project { get; set; } = new Project();
        public IList<Employee> Members { get; set; }
        public bool _Acsending { get; set; } = true;
    }
}