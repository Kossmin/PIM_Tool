using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.CustomAttribute
{
    public class NotNullableAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return false;
            }
            return true;
        }
    }
}
