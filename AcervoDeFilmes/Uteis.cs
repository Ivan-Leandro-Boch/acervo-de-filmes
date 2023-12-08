using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AcervoDeFilmes
{
    public class Uteis
    {
        public class MinimumElementsAttribute : ValidationAttribute
        {
            private readonly int minElements;

            public MinimumElementsAttribute(int minElements)
            {
                this.minElements = minElements;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var list = value as IList;

                var result = list?.Count >= minElements;

                return result
                    ? ValidationResult.Success
                    : new ValidationResult($"{validationContext.DisplayName} requer pelo menos {minElements} elemento" + (minElements > 1 ? "s" : string.Empty));
            }
        }
    }
}