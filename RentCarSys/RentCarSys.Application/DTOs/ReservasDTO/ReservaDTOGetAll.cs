using RentCarSys.Application.DTOs.ClientesDTO;
using RentCarSys.Application.DTOs.VeiculosDTO;
using System.ComponentModel.DataAnnotations;

namespace RentCarSys.Application.DTOs.ReservasDTO;
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

