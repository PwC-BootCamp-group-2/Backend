using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace spacemeet.Models
{
    
    public class BookedDateModel
    {
       [Key]
       public DateTime Date { get; set; }
    }
}
