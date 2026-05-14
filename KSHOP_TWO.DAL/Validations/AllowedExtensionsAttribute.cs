using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Validations
{
    
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        string[] _extension = { ".jpg", ".webp" };
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extension.Contains(extension))
                {
                    return new ValidationResult($"Allowed Extension : {string.Join(", ", _extension)}");
                }
               
                  
                
            }
            return ValidationResult.Success;
        }
    }
}
