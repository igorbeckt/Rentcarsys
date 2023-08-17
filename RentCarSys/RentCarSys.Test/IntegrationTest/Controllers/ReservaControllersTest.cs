using AutoMapper;
using Moq;
using RentCarSys.Application.Controllers;
using RentCarSys.Application.DTO.AutoMapper;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarSys.Test.IntegrationTest.Controllers
{
    public class ReservaControllersTest
    {
        protected Mock<IClientesRepository> clientesRepository = new Mock<IClientesRepository>();
        protected Mock<IVeiculosRepository> veiculosRepository = new Mock<IVeiculosRepository>();
        protected Mock<IReservasRepository> reservasRepository = new Mock<IReservasRepository>();
        protected Mock<ClienteService> clienteService;
        protected Mock<VeiculoService> veiculoService;
        protected Mock<ReservaService> reservaService;
        protected IMapper mapper;
        public readonly ReservaController reservaController;

        public ReservaControllersTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntitiesDTOMappingProfile());
            });
            mapper = config.CreateMapper();

            clienteService = new Mock<ClienteService>(clientesRepository.Object, mapper);
            veiculoService = new Mock<VeiculoService>(veiculosRepository.Object, mapper);
            reservaService = new Mock<ReservaService>(reservasRepository.Object, mapper);
            reservaController = new ReservaController(clienteService.Object, veiculoService.Object, reservaService.Object );
        }
    }
}
