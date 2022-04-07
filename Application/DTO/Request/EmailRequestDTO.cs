using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class EmailRequestDTO
    {
        [DefaultValue("email@email.com"), Display(Name = "E-Mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "Não foi informado um {0} válido.")]
        public string Email { get; set; }
    }
}
