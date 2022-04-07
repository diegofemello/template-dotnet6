using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Id de usuário")]
        public Guid UserUid { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("9a934959"), Display(Name = "Token")]
        [MinLength(5, ErrorMessage = "Token inválido")]
        public string Token { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("MySecret@123"), Display(Name = "Nova senha")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
