using System.ComponentModel.DataAnnotations;

namespace RentCarSys.Application.DTO.ClienteDTOs
{
    public class ClienteDTOGetAll
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório!")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório!")]
        [Range(0, 99999999999, ErrorMessage = "O CPF deve conter 11 dígitos!")]
        public long CPF { get; set; }
    }
}
