using RentCarSys.Application.Models;

namespace RentCarSys.Domain.Interfaces
{
    public interface IReservasRepository
    {
        Task<List<Reserva>> ObterTodasReservasAsync();
        Task<Reserva> ObterReservaPorIdAsync(int reservaId);
        Task AdicionarReservaAsync(Reserva reserva);
        Task AtualizarReservaAsync(Reserva reserva);
        Task ExcluirReservaAsync(Reserva reserva);
    }
}
