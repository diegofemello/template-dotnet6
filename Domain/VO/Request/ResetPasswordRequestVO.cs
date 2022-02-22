using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.VO.Request
{
    public class ResetPasswordVO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("00000000-0000-0000-0000-000000000000"), Display(Name = "Id de usuário")]
        public Guid UserUid { get; set; }


        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("9a934959"), Display(Name = "Token")]
        [MinLength(5, ErrorMessage = "Token inválido")]
        public string Token { get; set; }


        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("MySecret@123"), Display(Name = "Senha")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("MySecret@123"), Display(Name = "Confirmação de Senha")]
        [DataType(DataType.Password), Compare("Nova Senha")]
        public string ConfirmNewPassword { get; set; }
    }
}
