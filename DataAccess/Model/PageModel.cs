using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class PageModel
    {
        public string Status { get; set; }
        public string SearchString { get; set; }
        public string SortingKind { get; set; }
        public int PageIndex { get; set; }
        public int NumberOfRow { get; set; }
        public bool IsAcsending { get; set; } = true;
    }
}
