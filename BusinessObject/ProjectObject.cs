using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ProjectObject
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        [Display(Name ="Number")]
        public string ProjectNumber { get; set; }
        [Display(Name ="Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Customer")]
        public string Customer { get; set; }
        [Display(Name = "Status")]
        public ProjectStatus? Status { get; set; } = ProjectStatus.NEW;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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
