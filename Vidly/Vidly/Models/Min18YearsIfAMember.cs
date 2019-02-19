using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Min18YearsIfAMember:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (Customer) validationContext.ObjectInstance;
            if(model.MembershipTypeId == (byte) MembershipTypeEnum.Uknown || model.MembershipTypeId == (byte) MembershipTypeEnum.PayAsYouGo)
                return ValidationResult.Success;
            if(model.BirthDate == null)
                return new ValidationResult("The birthDateIsRequired");
            var age = DateTime.Now.Year - model.BirthDate.Value.Year;
            return (age >= 17) ? ValidationResult.Success: new ValidationResult("Member has to have min 18 years");
        }
    }
}