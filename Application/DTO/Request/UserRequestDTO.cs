using Application.Utils;
using Domain.Model;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class UserCreateDTO
    {
        [DefaultValue("myusername"), Display(Name = "Login")]
        [Required(ErrorMessage = "É necessário informar um {0}!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string UserName { get; set; }

        [Display(Name = "Nome Completo")]
        [StringLength(130, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string FullName { get; set; }

        [DefaultValue("email@email.com"), Display(Name = "E-Mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "Não foi informado um {0} válido.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("MySecret@123"), Display(Name = "Senha")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string Password { get; set; }
    }

    public class UserUpdateDTO
    {
        [DefaultValue("myusername"), Display(Name = "Login")]
        [Required(ErrorMessage = "É necessário informar um {0}!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string UserName { get; set; }

        [Display(Name = "Nome Completo")]
        [StringLength(130, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string FullName { get; set; }

        [DefaultValue("email@email.com"), Display(Name = "E-Mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "Não foi informado um {0} válido.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string Email { get; set; }

        [DefaultValue("MySecret@123"), Display(Name = "Senha")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string Password { get; set; }

        [DefaultValue("Visitor"), Display(Name = "Permissão")]
        [EnumDataType(typeof(UserRole))]
        public UserRole? UserRole { get; set; }
    }
}