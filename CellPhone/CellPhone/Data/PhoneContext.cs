using CellPhone.Models;
using Microsoft.EntityFrameworkCore;


namespace CellPhone.Data
{
  public class PhoneContext : DbContext
  {
    public PhoneContext(DbContextOptions<PhoneContext> options) : base(options)
    {
    }
    public DbSet<Phone> Phones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Phone>().ToTable("Phone");
    }

  }
}
