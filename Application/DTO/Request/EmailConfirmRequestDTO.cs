using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class EmailConfirmRequestDTO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("00000000-0000-0000-0000-000000000000"), Display(Name = "Id de usuário")]
        public Guid UserUid { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("9a934959"), Display(Name = "Token")]
        [MinLength(5, ErrorMessage = "Token inválido")]
        public string Token { get; set; }
    }
}
