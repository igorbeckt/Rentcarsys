using System.ComponentModel.DataAnnotations;

namespace RentCarSys.Application.DTO
{
    public class VeiculoDTOGetAll
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A placa é obrigatório!")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "A Placa deve conter 4 letras e 3 números!")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório!")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "A marca é obrigatório!")]
        public string Marca { get; set; }
    }
}
