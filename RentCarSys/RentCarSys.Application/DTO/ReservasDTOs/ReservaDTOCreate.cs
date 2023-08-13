using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RentCarSys.Application.DTO.ReservasDTOs
{
    public class ReservaDTOCreate
    {
        public int ClienteId { get; set; }
        public int VeiculoId { get; set; }

        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "A data da reserva é obrigatório!")]
        public string DataReserva { get; set; }
        public long ValorLocacao { get; set; }

        [Required(ErrorMessage = "A data de retirada é obrigatório!")]
        public string DataRetirada { get; set; }

        [Required(ErrorMessage = "A data de entrega é obrigatório!")]
        public string DataEntrega { get; set; }
    }
}
