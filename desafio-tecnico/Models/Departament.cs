using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desafio_tecnico.Models;

public class Departament
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Column("manager_id")]
    public int? ManagerId { get; set; }

    [ForeignKey("ManagerId")]
    public virtual Employee? Manager { get; set; }
    
    [Column("higher_departament_id")]
    public int? HigherDepartamentId { get; set; }

    [ForeignKey("HigherDepartamentId")]
    public virtual Departament? HigherDepartament { get; set; }

    // departamentos que tem esse como superior
    public virtual ICollection<Departament> SubDepartaments { get; set; } = new List<Departament>();

    // funcionarios desse departamento
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [Required]
    [Column("createdAt", TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("updatedAt", TypeName = "TIMESTAMP")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
