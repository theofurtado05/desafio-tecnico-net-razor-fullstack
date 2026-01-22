using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desafio_tecnico.Models;

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    

    [Required]
    [MaxLength(11)]
    [Column(TypeName = "CHAR(11)")]
    public string CPF { get; set; } = string.Empty;

    [MaxLength(20)]
    [Column(TypeName = "VARCHAR(20)")]
    public string? Rg { get; set; } = string.Empty;


    [Column("departament_id")]
    public int? DepartmentId { get; set; }

    [ForeignKey("DepartmentId")]
    public virtual Departament? Departament { get; set; }

    // departamento onde esse funcionario é gerente (um colaborador só pode ser gerente de um departamento)
    public virtual Departament? ManagedDepartament { get; set; }

    [Required]
    [Column(TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

    [Required]
    [Column(TypeName = "TIMESTAMP")]
    public DateTime UpdatedAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
   

    [Required]
    [Column("is_deleted")]
    public bool? IsDeleted { get; set; } = false;


}