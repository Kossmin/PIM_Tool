using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using SPK_PIM;
using BusinessObject.CustomAttribute;

namespace BusinessObject
{
    public class Project
    {
        //[MaxLength(19, ErrorMessageResourceName = "ValidLength19", ErrorMessageResourceType = typeof(Resources.Resources))]
        public virtual int ID { get; set; }

        
        //[MaxLength(19, ErrorMessageResourceName = "ValidLength19", ErrorMessageResourceType = typeof(Resources.Resources))]
        //public virtual int GroupID { get; set; }

        [Display(Name ="Number", ResourceType = typeof(Resources.Resources))]
        [ValidLength(4)]
        [Required(ErrorMessage ="Needed")]
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
        [EndDate("StartDate")]
        public virtual DateTime EndDate { get; set; }

        //[MaxLength(10, ErrorMessageResourceName = "ValidLength10", ErrorMessageResourceType = typeof(Resources.Resources))]
        public virtual int Version { get; set; }

        public virtual IList<Employee> Employees { get; set; }
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
            Employees = new List<Employee>();
            Status = ProjectStatus.NEW;
            StartDate = DateTime.UtcNow;
            Version = 0;
        }

        public virtual void AddEmployees(IEnumerable<Employee> employee)
        {
            Employees = employee.ToList();
            foreach (var item in employee)
            {
                item.Projects.Add(this);
            }
        }
        public virtual void UpdateEmployee(IEnumerable<Employee> employee)
        {
            Employees = employee.ToList();
            foreach (var item in Employees)
            {
                item.Projects.Remove(this);
            }
            foreach (var item in employee)
            {
                item.Projects.Add(this);
            }
        }

        public virtual void DeleteEmployee()
        {
            Employees = new List<Employee>();
            foreach (var item in Employees)
            {
                item.Projects.Remove(this);
            }
        }

        //public override bool Equals(object obj)
        //{
        //    var tmpProject = (Project)obj;
        //    return ID ==  tmpProject.ID;
        //}


        //public override int GetHashCode()
        //{
        //    return Tuple.Create<int, string>(ID, ProjectNumber).GetHashCode();
        //}
    }

    class ProjectMapping : ClassMap<Project>
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
            Version(x => x.Version);
            HasManyToMany(x => x.Employees)
                .Access.Property()
                .Cascade.SaveUpdate()
                .Table("ProjectEmployees");
            References<Group>(x => x.Group).Cascade.None().Column("GroupID");
            OptimisticLock.Version();


            //Property(x => x.ProjectNumber, m => {
            //    m.NotNullable(true);
            //    m.Length(19);
            //});
            //Property(x => x.ProjectName, m => {
            //    m.NotNullable(true);
            //    m.Length(19);
            //});
            //Property(x => x.Customer, m => {
            //    m.NotNullable(true);
            //    m.Length(19);
            //});
            //Property(x => x.StartDate, m => {
            //    m.NotNullable(true);
            //    m.Length(19);
            //});
            //Property(x => x.EndDate, m => {
            //    m.NotNullable(true);
            //    m.Length(19);
            //});
            //Property(x => x.Status, m => {
            //    m.NotNullable(true);
            //    m.Length(19);
            //});
            //Version(x => x.Version);

            Table("Project");
        }
    }
}
