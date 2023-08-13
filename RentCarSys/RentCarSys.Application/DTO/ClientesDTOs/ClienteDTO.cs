using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RentCarSys.Application.DTO.ClienteDTOs
{
    public class ClienteDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório!")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O E-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "Insira um e-mail válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O RG é obrigatório!")]
        [Range(0, 9999999, ErrorMessage = "O RG deve conter 7 dígitos!")]
        public long RG { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório!")]
        [Range(0, 99999999999, ErrorMessage = "O CPF deve conter 11 dígitos!")]
        public long CPF { get; set; }
    }
}
