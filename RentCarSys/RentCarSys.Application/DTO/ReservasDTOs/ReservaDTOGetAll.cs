using RentCarSys.Application.DTO.ClienteDTOs;
using RentCarSys.Application.DTO.VeiculosDTOs;
using System.ComponentModel.DataAnnotations;

namespace RentCarSys.Application.DTO.ReservasDTOs
{
    public class ReservaDTOGetAll
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A data da reserva é obrigatório!")]
        public string DataReserva { get; set; }
        public long ValorLocacao { get; set; }

        [Required(ErrorMessage = "A data de retirada é obrigatório!")]
        public string DataRetirada { get; set; }

        [Required(ErrorMessage = "A data de entrega é obrigatório!")]
        public string DataEntrega { get; set; }

        public List<ClienteDTOGetAll> Cliente { get; set; }

        public List<VeiculoDTOGetAll> Veiculo { get; set; }
    }
}
