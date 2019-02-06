using CellPhone.Models;
using System;
using System.Linq;

namespace CellPhone.Data
{
  public static class DbInitializer
  {
    public static void Initialize(PhoneContext context)
    {
      context.Database.EnsureCreated();

      // Look for any students.
      if (context.Phones.Any())
      {
        return;   // DB has been seeded
      }

      var phones = new Phone[]
      {
        new Phone{Model="Iphone 4",Brand="Apple",Memory=8, Rating = "C", Smart = "Y", ReleaseDate=DateTime.Parse("2005-09-01")},
        new Phone{Model="Iphone 5",Brand="Apple",Memory=16, Rating = "C", Smart = "Y", ReleaseDate=DateTime.Parse("2007-01-01")},
        new Phone{Model="Iphone 6",Brand="Apple",Memory=32, Rating = "B", Smart = "Y", ReleaseDate=DateTime.Parse("2012-06-01")},
        new Phone{Model="Iphone 10",Brand="Apple",Memory=64, Rating = "A", Smart = "Y", ReleaseDate=DateTime.Parse("2016-04-15")},
        new Phone{Model="Pivot",Brand="Nokia",Memory=16, Rating = "C", Smart = "Y", ReleaseDate=DateTime.Parse("2013-05-12")},
        new Phone{Model="Galaxy",Brand="Samsung",Memory=64, Rating = "B", Smart = "Y", ReleaseDate=DateTime.Parse("2012-08-15")},

      };
      foreach (Phone s in phones)
      {
        context.Phones.Add(s);
      }
      context.SaveChanges();

    }
  }
}