﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.CustomAttribute
{
    class ValidLengthAttribute : StringLengthAttribute
    {
        private int _maximumLength;
        public ValidLengthAttribute(int maximumLength) :base(maximumLength)
        {
            _maximumLength = maximumLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var temp = value.ToString().Length;
            if(temp > _maximumLength)
            {
                var errorMessage = String.Format(Resources.Resources.ValidLength, _maximumLength.ToString());
                return new ValidationResult(errorMessage);
            }
            return base.IsValid(value, validationContext);
        }

    }
}
