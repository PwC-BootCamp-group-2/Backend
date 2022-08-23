using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace spacemeet.Models
{
    public class Individual
    {
        public int Id { get; set; }
        public string userName { get; set; } = string.Empty;

        public string gender { get; set; } = string.Empty;
        public string companyName { get; set; } = string.Empty;
        [ForeignKey("User")]
        public string UserId {get; set;}

    }
}