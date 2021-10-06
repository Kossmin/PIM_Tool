using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ProjectObject
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectName { get; set; }
        public string Customer { get; set; }
        public ProjectStatus? Status { get; set; } = ProjectStatus.NEW;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Version { get; set; }

        public enum ProjectStatus
        {
            NEW,//new
            PLA,//planned
            INP,//in progress
            FIN//finish
        }
    }
}
