using System.ComponentModel.DataAnnotations;

namespace desafio_tecnico.ViewModels;

public class CreateDepartamentViewModel
{
    [Required(ErrorMessage = "O nome do departamento é obrigatório")]
    [Display(Name = "Nome do Departamento")]
    [MaxLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Gerente")]
    public int? ManagerId { get; set; }

    [Display(Name = "Departamento Superior")]
    public int? HigherDepartamentId { get; set; }
}
