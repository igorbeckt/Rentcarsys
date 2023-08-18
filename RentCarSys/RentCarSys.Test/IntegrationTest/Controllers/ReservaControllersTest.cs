using Newtonsoft.Json;
using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.Models.Enums;
using RentCarSys.Application.Models;
using RentCarSys.Test.IntegrationTest.MockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RentCarSys.Application.DTO.ReservasDTOs;

namespace RentCarSys.Test.IntegrationTest.Controllers
{
    public class ReservaControllersTest
    {
        protected RentCarSysApplication application;

        public ReservaControllersTest()
        {
            application = new RentCarSysApplication();
        }

        [Fact]
        public async Task BuscarTodasReservas_Success()
        {
            var reserva1 = new Reserva
            {
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1",
                Cliente = new List<Cliente> 
                    { new Cliente { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online } 
                },
                Veiculo = new List<Veiculo>
                    { new Veiculo { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1"} 
                }
            };
            var reserva2 = new Reserva
            {
                Id = 2,
                DataReserva = "DataReserva2",
                ValorLocacao = 2,
                DataRetirada = "DataRetirada2",
                DataEntrega = "DataEntrega2",
                Cliente = new List<Cliente>
                    { new Cliente { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online } 
                },
                Veiculo = new List<Veiculo>
                    { new Veiculo { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2"} 
                }
            };

            List<Reserva> reservas = new List<Reserva>() { reserva1, reserva2 };

            await ReservaMockData.CreateReservas(application, reservas);

            var url = "reservas/buscarTodas";
            var reserva = application.CreateClient();

            var response = await reserva.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<ReservaDTOGetAll>>();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.DataReserva == "DataReserva1");
            Assert.Contains(result, c => c.DataReserva == "DataReserva2");

            await ReservaMockData.DeletarReservas(application, reservas);
        }

        [Fact]
        public async Task BuscarReservaPorId_Fail()
        {
            var reserva1 = new Reserva
            {
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1",
                Cliente = new List<Cliente>
                    { new Cliente { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online }
                },
                Veiculo = new List<Veiculo>
                    { new Veiculo { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1"}
                }            
            };

            List<Reserva> reservas = new List<Reserva>() { reserva1 };

            var url = "reserva/buscarPorId/1";
            var reserva = application.CreateClient();

            var response = await reserva.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("", errorResponse);
        }

        [Fact]
        public async Task BuscarReservaPorId_Sucess()
        {
            var reserva1 = new Reserva
            {
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1",
                Cliente = new List<Cliente>
                    { new Cliente { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online }
                },
                Veiculo = new List<Veiculo>
                    { new Veiculo { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1"}
                }
            };            

            List<Reserva> reservas = new List<Reserva>() { reserva1 };

            await ReservaMockData.CreateReservas(application, reservas);

            var url = "reservas/buscarPorId/1";
            var reserva = application.CreateClient();

            var response = await reserva.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ReservaDTO>();
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);

            await ReservaMockData.DeletarReservas(application, reservas);
        }        

        [Fact]
        public async Task CriarReserva_Fail()
        {
            var reserva1 = new Reserva
            {
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1",
                Cliente = new List<Cliente>
                    { new Cliente { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online }
                },
                Veiculo = new List<Veiculo>
                    { new Veiculo { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1"}
                }            
            };

            List<Reserva> reservas = new List<Reserva>() { reserva1 };

            var url = "reservas/cadastrar";
            var reserva = application.CreateClient();

            var reservaDTOCreate = new ReservaDTOCreate
            {
                ClienteId = 1,
                VeiculoId = 1,
                Id = 1,
                DataReserva = "",
                ValorLocacao = 0,
                DataRetirada = "",
                DataEntrega = "",
            };

            var content = new StringContent(JsonConvert.SerializeObject(reservaDTOCreate), Encoding.UTF8, "application/json");

            var response = await reserva.PostAsync(url, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("A data da reserva é obrigatório!", errorResponse);
            Assert.Contains("A data de retirada é obrigatório!", errorResponse);
            Assert.Contains("A data de entrega é obrigatório!", errorResponse);
        }

        [Fact]
        public async Task CriarReserva_Sucess()
        {
            var reserva1 = new Reserva
            {
                Id = 1,
                DataReserva = "Data1",
                ValorLocacao = 1,
                DataRetirada = "Data1",
                DataEntrega = "Data1",
                Cliente = new List<Cliente>
                    { new Cliente { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online }
                },
                Veiculo = new List<Veiculo>
                    { new Veiculo { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1"}
                }            
            };

            List<Reserva> reservas = new List<Reserva>() { reserva1 };
            await ReservaMockData.CreateReservas(application, reservas);

            var url = "reservas/cadastrar";
            var reserva = application.CreateClient();

            var reservaDTOCreate = new ReservaDTOCreate
            {
                ClienteId = 1,
                VeiculoId = 1,
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 0,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataRetirada1",
            };

            var content = new StringContent(JsonConvert.SerializeObject(reservaDTOCreate), Encoding.UTF8, "application/json");

            var response = await reserva.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var createdResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ReservaDTO>(createdResponse);

            Assert.NotNull(result);
            Assert.Equal(reservaDTOCreate.DataReserva, result.DataReserva);
            Assert.Equal(reservaDTOCreate.ValorLocacao, result.ValorLocacao);
            Assert.Equal(reservaDTOCreate.DataEntrega, result.DataEntrega);
            Assert.Equal(reservaDTOCreate.DataRetirada, result.DataRetirada);           

            await ReservaMockData.DeletarReservas(application, reservas);
        }

        [Fact]
        public async Task EditarReserva_Fail()
        {
            var reserva1 = new Reserva
            {
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1",
                Cliente = new List<Cliente>
                    { new Cliente { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online }
                },
                Veiculo = new List<Veiculo>
                    { new Veiculo { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1"}
                }            
            };

            List<Reserva> reservas = new List<Reserva>() { reserva1 };

            var url = "reservas/alterar/1";
            var reserva = application.CreateClient();

            var reservaDTOUpdate = new ReservaDTOUpdate
            {                
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 0,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataRetirada1",
            };

            var content = new StringContent(JsonConvert.SerializeObject(reservaDTOUpdate), Encoding.UTF8, "application/json");

            var response = await reserva.PutAsync(url, content);
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("", errorResponse);
        }

        [Fact]
        public async Task EditarReserva_Sucess()
        {
            var reserva1 = new Reserva
            {
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1",
                Cliente = new List<Cliente>
                    { new Cliente { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online }
                },
                Veiculo = new List<Veiculo>
                    { new Veiculo { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1"}
                }
            };

            List<Reserva> reservas = new List<Reserva>() { reserva1 };

            await ReservaMockData.CreateReservas(application, reservas);

            var url = "reservas/alterar/1";
            var reserva = application.CreateClient();

            var reservaDTOUpdate = new ReservaDTOUpdate
            {
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 0,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataRetirada1",
            };

            var content = new StringContent(JsonConvert.SerializeObject(reservaDTOUpdate), Encoding.UTF8, "application/json");


            var response = await reserva.PutAsync(url, content);

            response.EnsureSuccessStatusCode();

            var updatedResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ReservaDTO>(updatedResponse);

            Assert.NotNull(result);
            Assert.Equal(reservaDTOUpdate.DataReserva, result.DataReserva);
            Assert.Equal(reservaDTOUpdate.ValorLocacao, result.ValorLocacao);
            Assert.Equal(reservaDTOUpdate.DataRetirada, result.DataRetirada);
            Assert.Equal(reservaDTOUpdate.DataEntrega, result.DataEntrega);

            await ReservaMockData.DeletarReservas(application, reservas);
        }

        [Fact]
        public async Task ExcluirReservas_Fail()
        {
            var reserva1 = new Reserva
            {
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1",
                Cliente = new List<Cliente>
                    { new Cliente { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online }
                },
                Veiculo = new List<Veiculo>
                    { new Veiculo { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1"}
                }
            };            

            List<Reserva> reservas = new List<Reserva>() { reserva1 };
            

            await ReservaMockData.CreateReservas(application, reservas);

            var url = "reservas/alterar/1333";
            var reserva = application.CreateClient();

            var response = await reserva.DeleteAsync(url);
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("", errorResponse);
            
        }

        [Fact]
        public async Task ExcluirReservas_Success()
        {
            var reserva1 = new Reserva
            {
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1",
                Cliente = new List<Cliente>
                    { new Cliente { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online }
                },
                Veiculo = new List<Veiculo>
                    { new Veiculo { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1"}
                }
            };

            List<Reserva> reservas = new List<Reserva>() { reserva1 };

            await ReservaMockData.CreateReservas(application, reservas);

            var url = "reservas/excluir/1";
            var reserva = application.CreateClient();

            var response = await reserva.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ReservaDTODelete>();

            Assert.NotNull(result);

            reservas.Remove(reserva1);            
        }
    }
}
