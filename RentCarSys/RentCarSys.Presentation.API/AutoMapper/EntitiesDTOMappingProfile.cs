using AutoMapper;
using RentCarSys.Application.DTOs.ClientesDTO;
using RentCarSys.Application.DTOs.ReservasDTO;
using RentCarSys.Application.DTOs.VeiculosDTO;
using RentCarSys.Application.Models;

namespace RentCarSys.Presentation.API.AutoMapper
{
    public class EntitiesDTOMappingProfile : Profile
    {
        public EntitiesDTOMappingProfile()
        {
            CreateMap<Cliente, ClienteDTO>().ReverseMap();
            CreateMap<Cliente, ClienteDTOCreate>();
            CreateMap<Cliente, ClienteDTOGetAll>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NomeCompleto, opt => opt.MapFrom(src => src.NomeCompleto))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF));

            CreateMap<Veiculo, VeiculoDTO>().ReverseMap();
            CreateMap<Veiculo, VeiculoDTOUpdate>();
            CreateMap<Veiculo, VeiculoDTOCreate>();
            CreateMap<Veiculo, VeiculoDTOGetAll>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Modelo, opt => opt.MapFrom(src => src.Modelo))
                .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa))
                .ForMember(dest => dest.Marca, opt => opt.MapFrom(src => src.Marca));

            CreateMap<Reserva, ReservaDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente.FirstOrDefault()))
            .ForMember(dest => dest.Veiculo, opt => opt.MapFrom(src => src.Veiculo.FirstOrDefault()));
            CreateMap<Reserva, ReservaDTOGetAll>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente))
            .ForMember(dest => dest.Veiculo, opt => opt.MapFrom(src => src.Veiculo));
            CreateMap<Reserva, ReservaDTOCreate>()
            .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.Cliente.FirstOrDefault().Id))
            .ForMember(dest => dest.VeiculoId, opt => opt.MapFrom(src => src.Veiculo.FirstOrDefault().Id));
            CreateMap<Reserva, ReservaDTOUpdate>();
            CreateMap<Reserva, ReservaDTODelete>();

        }
    }
}
