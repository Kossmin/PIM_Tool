using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace BusinessObject
{
    public class Project
    {
        //[MaxLength(19, ErrorMessageResourceName = "ValidLength19", ErrorMessageResourceType = typeof(Resources.Resources))]
        public virtual int ID { get; set; }
        
        //[MaxLength(19, ErrorMessageResourceName = "ValidLength19", ErrorMessageResourceType = typeof(Resources.Resources))]
        //public virtual int GroupID { get; set; }
       
        [Display(Name ="Number", ResourceType = typeof(Resources.Resources))]
        [MaxLength(4, ErrorMessageResourceName = "ValidLength4", ErrorMessageResourceType = typeof(Resources.Resources))]
        public virtual string ProjectNumber { get; set; }
        
        [Display(Name ="ProjectName", ResourceType = typeof(Resources.Resources))]
        [MaxLength(50, ErrorMessageResourceName = "ValidLength50", ErrorMessageResourceType = typeof(Resources.Resources))]
        public virtual string ProjectName { get; set; }
        
        [Display(Name = "Customer", ResourceType = typeof(Resources.Resources))]
        [MaxLength(50, ErrorMessageResourceName = "ValidLength50", ErrorMessageResourceType = typeof(Resources.Resources))]
        public virtual string Customer { get; set; }
        
        [Display(Name = "Status", ResourceType = typeof(Resources.Resources))]
        public virtual ProjectStatus? Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "StartDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date)]
        public virtual DateTime StartDate { get; set; }
       
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "EndDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date)]
        public virtual DateTime EndDate { get; set; }

        //[MaxLength(10, ErrorMessageResourceName = "ValidLength10", ErrorMessageResourceType = typeof(Resources.Resources))]
        public virtual int Version { get; set; }

        public virtual IList<Project_Employee> ProjectEmployees { get; set; }
        public virtual Group Group { get; set; }

        public enum ProjectStatus
        {
            NEW,//new
            PLA,//planned
            INP,//in progress
            FIN//finish
        }

        public Project()
        {
            ProjectEmployees = new List<Project_Employee>();
            Status = ProjectStatus.NEW;
            StartDate = DateTime.UtcNow;
            Version = 0;
        }

        public virtual void AddEmployyee(Project_Employee project_Employee)
        {
            project_Employee.ProjectID = this.ID;
            ProjectEmployees.Add(project_Employee);
        }

    }

    class ProjectMapping : ClassMapping<Project>
    {
        public ProjectMapping()
        {
            Id(x => x.ID);
            //Map(x => x.GroupID).Not.Nullable().Length(19);
            Map(x => x.ProjectNumber).Not.Nullable().Length(4);
            Map(x => x.ProjectName).Not.Nullable().Length(50);
            Map(x => x.Customer).Not.Nullable().Length(50);
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.Status).Not.Nullable().Length(3).CustomSqlType("char(3)");
            Property(x => x.Status);
            Property(x => x.Version,m => m.);
            HasMany(x => x.ProjectEmployees).KeyColumn("ProjectID").Inverse().Cascade.All();
            References<Group>(x => x.Group).Cascade.None().Column("GroupID");

            Property(x => x.ProjectNumber, m => {
                m.NotNullable(true);
                m.Length(19);
            });
            Property(x => x.ProjectName, m => {
                m.NotNullable(true);
                m.Length(19);
            });
            Property(x => x.Customer, m => {
                m.NotNullable(true);
                m.Length(19);
            });
            Property(x => x.StartDate, m => {
                m.NotNullable(true);
                m.Length(19);
            });
            Property(x => x.EndDate, m => {
                m.NotNullable(true);
                m.Length(19);
            });
            Property(x => x.Status, m => {
                m.NotNullable(true);
                m.Length(19);
            });
            Version(x => x.Version,);

            Table("Project");
        }
    }
}
