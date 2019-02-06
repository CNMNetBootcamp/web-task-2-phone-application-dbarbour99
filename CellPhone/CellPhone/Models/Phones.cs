using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CellPhone.Models
{
  public class Phone
  {
    public int ID { get; set; }
    public string Model { get; set; }
    public string Brand { get; set; }
    public int Memory { get; set; }
    public string Rating { get; set; }
    public string Smart { get; set; }


    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Release Date")]
    public DateTime ReleaseDate { get; set; }

  }
}
