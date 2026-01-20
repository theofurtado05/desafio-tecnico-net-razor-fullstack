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

    [Required]
    public int DepartmentId { get; set; }

    [ForeignKey("DepartmentId")]
    public virtual Departament? Departament { get; set; }

    // departamentos onde esse funcionario é gerente
    public virtual ICollection<Departament> ManagedDepartaments { get; set; } = new List<Departament>();

    [Required]
    [Column(TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "TIMESTAMP")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
   

}