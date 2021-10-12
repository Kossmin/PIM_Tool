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
        [Display(Name ="Number", ResourceType = typeof(Resources.Resources))]
        public string ProjectNumber { get; set; }
        [Display(Name ="ProjectName", ResourceType = typeof(Resources.Resources))]
        public string ProjectName { get; set; }
        [Display(Name = "Customer", ResourceType = typeof(Resources.Resources))]
        public string Customer { get; set; }
        [Display(Name = "Status", ResourceType = typeof(Resources.Resources))]
        public ProjectStatus? Status { get; set; } = ProjectStatus.NEW;

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "StartDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "EndDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date)]
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
