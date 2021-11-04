

using Autofac.Extras.Moq;
using DataAccess.Repository;
using Xunit;
using BusinessLogic.BusinessLogic;
using BusinessObject;
using System.Collections.Generic;
using System;
using Moq;

namespace BusinessLogic.Test
{
    public class BusinessServiceTest
    {
        [Fact]
        public void GetProjects_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IEmployeeRepository>()
                    .Setup(x=>x.GetEmployees())
                    .Returns(GetSampleEmployees());

                var business = mock.Create<BusinessService>();

                var expected = GetSampleEmployees();

                var actual = business.GetEmployees();

                Assert.Equal(expected.Count, actual.Count);
            }
        }

        [Fact]
        public void AddNewProject_ValidCall()
        {
            var project = new Project();
            var employeeId = new List<int> { 1, 5 ,6 };
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IProjectRepository>()
                    .Setup(x => x.Add(project, employeeId));
                var business = mock.Create<BusinessService>();

                business.AddNewProject(project, employeeId);
                mock.Mock<IProjectRepository>()
                    .Verify(x => x.Add(project, employeeId), Times.Exactly(1));
            }
        }

        [Fact]
        public void SearchProjectById()
        {
            using(var mock = AutoMock.GetLoose())
            {
                var output = new List<Project> {
                    new Project
                    {
                        ID = 1,
                        Customer = "None",
                        Employees = GetSampleEmployees(),
                        ProjectName = "Pim1",
                        ProjectNumber = "234",
                        Status = Project.ProjectStatus.NEW,
                        StartDate = new DateTime(2021, 12, 12),
                        Version = 1
                    }
                };
                mock.Mock<IProjectRepository>()
                    .Setup(x => x.GetProjects(new List<int> { 1 }))
                    .Returns(output);
                var business = mock.Create<BusinessService>();

                var expected = output[0];

                var actual = business.SearchProjectById(1);

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void CountMaxPage_ValidCall()
        {
            using(var mock = AutoMock.GetLoose())
            {
                mock.Mock<IProjectRepository>()
                    .Setup(x => x.GetNumberOfRecords("tmp", "tmp"))
                    .Returns(6);
                var business = mock.Create<BusinessService>();

                var expected = 2;

                var actual = business.CountMaxPage("tmp", "tmp", 5);

                Assert.Equal(expected, actual);
            }
        }


        private List<Employee> GetSampleEmployees()
        {
            List<Employee> output = new List<Employee>
            {
                new Employee
                {
                    ID = 1,
                    BirthDate = DateTime.Now,
                    FirstName= "Kim",
                    LastName = "Son",
                    Visa = "SPK",
                    Version = 1
                },
                new Employee
                {
                    ID = 2,
                    BirthDate = DateTime.Now,
                    FirstName= "Kim",
                    LastName = "Khanh",
                    Visa = "SSS",
                    Version = 1
                }
            };
            return output;
        }

        private List<Project> GetSampleProjects()
        {
            List<Project> output = new List<Project>
            {
                new Project
                {
                    ID = 1,
                    Customer = "None",
                    Employees = GetSampleEmployees(),
                    ProjectName = "Pim1",
                    ProjectNumber = "234",
                    Status = Project.ProjectStatus.NEW,
                    StartDate = new DateTime(2021, 12, 12),
                    Version = 1
                },
                new Project
                {
                    ID = 2,
                    Customer = "Null",
                    Employees = GetSampleEmployees(),
                    ProjectName = "Pim2",
                    ProjectNumber = "2341",
                    Status = Project.ProjectStatus.NEW,
                    StartDate = new DateTime(2021, 12, 1),
                    Version = 1
                }
            };
            return output;
        }
    }
}
