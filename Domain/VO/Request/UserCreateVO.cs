using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.VO.Request
{
    public class UserCreateVO
    {
        [DefaultValue("myusername"), Display(Name = "Login")]
        [Required(ErrorMessage = "É necessário informar um {0}!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string UserName { get; set; }

        [DefaultValue("MySecret@123"), Display(Name = "Senha")]
        [Required(ErrorMessage = "É necessário informar um {0}!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string Password { get; set; }

        [DefaultValue("Nome Completo"), Display(Name = "Nome Completo")]
        [StringLength(130, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string FullName { get; set; }

        [DefaultValue("email@email.com"), Display(Name = "E-Mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "Não foi informado um {0} válido.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string Email { get; set; }

        [DefaultValue("1"), Display(Name = "Id da organização")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int OrganizationId { get; set; }

        [DefaultValue("1"), Display(Name = "Id da usina")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int PowerPlantId { get; set; }
    }
}