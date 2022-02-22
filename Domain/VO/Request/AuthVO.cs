using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.VO.Request
{
    public class AuthVO
    {
        [DefaultValue("mylogin"), Display(Name = "Login")]
        [Required(ErrorMessage = "É necessário informar um {0}!")]
        public string UserName { get; set; }

        [DefaultValue("mypassword"), Display(Name = "Senha")]
        [Required(ErrorMessage = "É necessário informar uma {0}!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string Password { get; set; }
    }
}