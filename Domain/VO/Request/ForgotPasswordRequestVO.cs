using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.VO.Request
{
    public class ForgotPasswordRequestVO
    {
        [DefaultValue("email@email.com"), Display(Name = "E-Mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "Não foi informado um {0} válido.")]
        public string Email { get; set; }

        public bool EmailSent { get; set; }
    }
}
