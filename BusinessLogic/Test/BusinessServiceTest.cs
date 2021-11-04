

using Autofac.Extras.Moq;
using DataAccess.Repository;
using Xunit;
using BusinessLogic.BusinessLogic;
using BusinessObject;
using System.Collections.Generic;
using System;

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
        public void Get

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
    }
}
