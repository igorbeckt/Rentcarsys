using AutoMapper;
using Localdorateste.Models;

namespace RentCarSys.Application.DTO.AutoMapper
{
    public class EntitiesDTOMappingProfile : Profile
    {
        public EntitiesDTOMappingProfile()
        {
            CreateMap<Cliente, ClienteDTO>().ReverseMap();
            CreateMap<Cliente, ClienteDTOCREATE>();
            CreateMap<Cliente, ClienteDTOGetAll>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NomeCompleto, opt => opt.MapFrom(src => src.NomeCompleto))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF));

            CreateMap<Veiculo, VeiculoDTO>().ReverseMap();
            CreateMap<Veiculo, VeiculoDTOCREATE>();
            CreateMap<Veiculo, VeiculoDTOGetAll>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Modelo, opt => opt.MapFrom(src => src.Modelo))
                .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa))
                .ForMember(dest => dest.Marca, opt => opt.MapFrom(src => src.Marca));

            CreateMap<Reserva, ReservaDTO>().ReverseMap();
        }
    }
}
