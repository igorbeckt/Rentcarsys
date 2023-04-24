using Localdorateste.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Localdorateste.Data
{
    public class Contexto : DbContext

    {


        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

    }
}

