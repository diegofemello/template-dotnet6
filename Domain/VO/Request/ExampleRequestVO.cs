using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.VO.Request
{
    public class ExampleRequestVO
    {
        [DefaultValue("Nome"), Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres")]
        public string Name { get; set; }
    }
}