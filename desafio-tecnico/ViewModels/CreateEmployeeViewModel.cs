using System.ComponentModel.DataAnnotations;

namespace desafio_tecnico.ViewModels;

public class CreateEmployeeViewModel
{
    [Required(ErrorMessage = "O nome do colaborador é obrigatório")]
    [Display(Name = "Nome do colaborador")]
    [MaxLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF do colaborador é obrigatório")]
    [Display(Name = "CPF do colaborador")]
    [MaxLength(11, ErrorMessage = "O CPF deve ter no máximo 11 caracteres")]
    public string CPF { get; set; } = string.Empty;

    [Display(Name = "RG do colaborador")]
    [MaxLength(20, ErrorMessage = "O RG deve ter no máximo 20 caracteres")]
    public string? Rg { get; set; } = string.Empty;

    [Display(Name = "Departamento do colaborador")]
    public int DepartmentId { get; set; }

}
