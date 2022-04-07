using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class AuthRequestDTO
    {
        [DefaultValue("admin@teste.com"), Display(Name = "Login")]
        [Required(ErrorMessage = "É necessário informar um {0}!")]
        public string Login { get; set; }

        [DefaultValue("admin123"), Display(Name = "Senha")]
        [Required(ErrorMessage = "É necessário informar uma {0}!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string Password { get; set; }
    }
}