using Localdorateste.Models;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace RentCarSys.Application.DTO
{
    public class ReservaDTO
    {
        
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public int VeiculoId { get; set; }

        public ClienteDTOGetAll Cliente { get; set; }

        public VeiculoDTOGetAll Veiculo { get; set; }        

        [Required(ErrorMessage = "A data da reserva é obrigatório!")]
        public string DataReserva { get; set; }
        public long ValorLocacao { get; set; }

        [Required(ErrorMessage = "A data de retirada é obrigatório!")]
        public string DataRetirada { get; set; }

        [Required(ErrorMessage = "A data de entrega é obrigatório!")]
        public string DataEntrega { get; set; }
    }
}
