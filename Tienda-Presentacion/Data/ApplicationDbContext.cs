using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace Tienda_Presentacion.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(EntityTypeBuilder =>
            {
                EntityTypeBuilder.ToTable("Usuarios");

                EntityTypeBuilder.Property(u => u.Empleado_Id).HasDefaultValue(0);
            });
        }
    }
    public class Usuario : IdentityUser
    {
        public long Empleado_Id { get; set; }
    }
}
