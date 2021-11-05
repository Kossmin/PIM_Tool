using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Extensions
{
    public static class EnumExtensions
    {
        public static string EnumToString(this Project.ProjectStatus me)
        {
            switch (me)
            {
                case Project.ProjectStatus.NEW:
                    return "New";
                case Project.ProjectStatus.PLA:
                    return "Planned";
                case Project.ProjectStatus.INP:
                    return "In progress";
                default:
                    return "Finished";
            }
        }
    }
}
