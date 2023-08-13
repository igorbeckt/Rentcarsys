using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarSys.Test.UnitTest.MockData
{
    public static class ClienteMockData
    {
        public static List<Cliente> ClientesGetAll()
        {
            return new List<Cliente>
            {
                new Cliente { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912 },
                new Cliente { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911 }
            };
        }

        public static Cliente CLienteGetById() 
        {
            return new Cliente
            {
                Id = 1,
                NomeCompleto = "Cliente1",
                CPF = 12345678911
            };
        }

        public static ClienteDTOCreate ClienteCreate()
        {
            return new ClienteDTOCreate
            {
                NomeCompleto = "Novo Cliente",
                Email = "novo.cliente@example.com",
                RG = 1234567,
                CPF = 98765432100
            };
        }

        public static ClienteDTOUpdate ClienteDTOUpdate()
        
        {
            return new ClienteDTOUpdate
            {
                NomeCompleto = "Cliente Editado",
                Email = "cliente.editado@example.com",
                RG = 7654321,
                CPF = 98765432101
            };
        }

    }
}