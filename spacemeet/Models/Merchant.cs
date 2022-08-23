using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace spacemeet.Models
{
    public class Merchant
    {
        public int Id { get; set; }
        public string adminName { get; set; } = string.Empty;
        public string validId { get; set; } = string.Empty;
        [ForeignKey("User")]
        public string UserId {get; set;}

    }
}