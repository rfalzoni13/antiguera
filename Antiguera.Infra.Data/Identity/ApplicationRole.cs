using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace Antiguera.Infra.Data.Identity
{
    public class ApplicationRole : IdentityRole
    {
        [Required]
        public bool New { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}
