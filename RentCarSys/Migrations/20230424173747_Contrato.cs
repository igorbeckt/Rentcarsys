using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarSys.Migrations
{
    /// <inheritdoc />
    public partial class Contrato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    ContratoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaId = table.Column<int>(type: "int", nullable: false),
                    DataReserva = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataRetirada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEntrega = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorLocacao = table.Column<double>(type: "float", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    NomeCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<long>(type: "bigint", nullable: false),
                    VeiculoId = table.Column<int>(type: "int", nullable: false),
                    Placa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormaPagamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parcelas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.ContratoId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contratos");
        }
    }
}
