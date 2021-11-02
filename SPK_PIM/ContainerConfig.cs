﻿using Autofac;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPK_PIM
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ProjectRepository>().As<IProjectRepository>();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();


            return builder.Build();
        }
    }
}