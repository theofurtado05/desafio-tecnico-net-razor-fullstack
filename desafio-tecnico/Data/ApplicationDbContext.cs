using Microsoft.EntityFrameworkCore;
using desafio_tecnico.Models;

namespace desafio_tecnico.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Departament> Departaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(e => e.CPF)
                .HasColumnName("cpf")
                .HasColumnType("CHAR(11)")
                .IsRequired();
            
            entity.HasIndex(e => e.CPF)
                .IsUnique()
                .HasFilter("\"is_deleted\" = false OR \"is_deleted\" IS NULL");

            entity.Property(e => e.Rg)
                .HasColumnName("rg")
                .HasMaxLength(20);
            
            entity.HasIndex(e => e.Rg)
                .IsUnique()
                .HasFilter("\"is_deleted\" = false OR \"is_deleted\" IS NULL");

            entity.Property(e => e.DepartmentId)
                .HasColumnName("departament_id");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("createdAt")
                .HasColumnType("TIMESTAMP")
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updatedAt")
                .HasColumnType("TIMESTAMP")
                .IsRequired()
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasOne(e => e.Departament)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Departament>(entity =>
        {
            entity.ToTable("departaments", t =>
            {
                // higher_departament_id não pode ser igual a id
                t.HasCheckConstraint(
                    "chk_higher_departament_not_self",
                    "higher_departament_id IS NULL OR higher_departament_id <> id");
            });

            entity.HasKey(d => d.Id);
            entity.Property(d => d.Id).HasColumnName("id");

            entity.Property(d => d.Name)
                .HasColumnName("name")
                .HasMaxLength(150)
                .IsRequired();

            entity.HasIndex(d => d.Name)
                .IsUnique()
                .HasFilter("\"is_deleted\" = false OR \"is_deleted\" IS NULL");

            entity.Property(d => d.ManagerId)
                .HasColumnName("manager_id");

            entity.Property(d => d.HigherDepartamentId)
                .HasColumnName("higher_departament_id");

            entity.Property(d => d.CreatedAt)
                .HasColumnName("createdAt")
                .HasColumnType("TIMESTAMP")
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            entity.Property(d => d.UpdatedAt)
                .HasColumnName("updatedAt")
                .HasColumnType("TIMESTAMP")
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            entity.Property(d => d.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired()
                .HasDefaultValue(false);

            // FK para gerente (employee) - um colaborador só pode ser gerente de um departamento
            entity.HasOne(d => d.Manager)
                .WithOne(e => e.ManagedDepartament)
                .HasForeignKey<Departament>(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            // FK para departamento superior
            entity.HasOne(d => d.HigherDepartament)
                .WithMany(d => d.SubDepartaments)
                .HasForeignKey(d => d.HigherDepartamentId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}