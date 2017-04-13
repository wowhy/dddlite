using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Example.WebApi.Models
{
    public class ChangePasswordInput
    {
        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string OldPassword { get; set; }
    }
}
