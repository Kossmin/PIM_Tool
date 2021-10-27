using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPK_PIM
{
    public class EndDateAttribute : ValidationAttribute
    {
        private string _startDate;
        public EndDateAttribute(string startDate)
        {
            _startDate = startDate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_startDate);
            var startDate = (DateTime)property.GetValue(validationContext.ObjectInstance);
            if(DateTime.Compare((DateTime)value, startDate) < 0)
            {
                var errorMessage = Resources.Resources.DateTime;
                return new ValidationResult(errorMessage);
            }
            
            return ValidationResult.Success;
        }
    }
}