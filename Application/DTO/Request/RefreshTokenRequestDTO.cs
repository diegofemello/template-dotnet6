using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class RefreshTokenRequestDTO
    {
        [Display(Name = "Refresh Token")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string RefreshToken { get; set; }
    }
}
