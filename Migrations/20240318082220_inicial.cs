using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotoSpain_API.Migrations
{
    /// <inheritdoc />
    public partial class inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bonoloto_res",
                columns: table => new
                {
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    combinacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    complementario = table.Column<int>(type: "int", nullable: false),
                    reintegro = table.Column<int>(type: "int", nullable: false),
                    bote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bonoloto_res", x => x.fecha);
                });

            migrationBuilder.CreateTable(
                name: "botes_res",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cuantia = table.Column<int>(type: "int", nullable: false),
                    sorteo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    jornada = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_botes_res", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "eurodreams_res",
                columns: table => new
                {
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    combinacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    suenho = table.Column<int>(type: "int", nullable: false),
                    bote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eurodreams_res", x => x.fecha);
                });

            migrationBuilder.CreateTable(
                name: "euromillones_res",
                columns: table => new
                {
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    combinacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estrellas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_euromillones_res", x => x.fecha);
                });

            migrationBuilder.CreateTable(
                name: "gordo_res",
                columns: table => new
                {
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    combinacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    clave = table.Column<int>(type: "int", nullable: false),
                    bote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gordo_res", x => x.fecha);
                });

            migrationBuilder.CreateTable(
                name: "lototurf_res",
                columns: table => new
                {
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    jornada = table.Column<int>(type: "int", nullable: false),
                    combinacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    caballo = table.Column<int>(type: "int", nullable: false),
                    reintegro = table.Column<int>(type: "int", nullable: false),
                    bote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lototurf_res", x => x.fecha);
                });

            migrationBuilder.CreateTable(
                name: "nacional_res",
                columns: table => new
                {
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    combinacion1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    combinacion2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reintegros = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombreExtra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nacional_res", x => x.fecha);
                });

            migrationBuilder.CreateTable(
                name: "primitiva_res",
                columns: table => new
                {
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    combinacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    complementario = table.Column<int>(type: "int", nullable: false),
                    reintegro = table.Column<int>(type: "int", nullable: false),
                    bote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_primitiva_res", x => x.fecha);
                });

            migrationBuilder.CreateTable(
                name: "quiniela_res",
                columns: table => new
                {
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    jornada = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quiniela_res", x => x.fecha);
                });

            migrationBuilder.CreateTable(
                name: "quinigol_res",
                columns: table => new
                {
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    jornada = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quinigol_res", x => x.fecha);
                });

            migrationBuilder.CreateTable(
                name: "quintuple_res",
                columns: table => new
                {
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    combinacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    jornada = table.Column<int>(type: "int", nullable: false),
                    bote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quintuple_res", x => x.fecha);
                });

            migrationBuilder.CreateTable(
                name: "Partido",
                columns: table => new
                {
                    local = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    visitante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    restultado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quinielafecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quinigolfecha = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partido", x => x.local);
                    table.ForeignKey(
                        name: "FK_Partido_quiniela_res_Quinielafecha",
                        column: x => x.Quinielafecha,
                        principalTable: "quiniela_res",
                        principalColumn: "fecha");
                    table.ForeignKey(
                        name: "FK_Partido_quinigol_res_Quinigolfecha",
                        column: x => x.Quinigolfecha,
                        principalTable: "quinigol_res",
                        principalColumn: "fecha");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partido_Quinielafecha",
                table: "Partido",
                column: "Quinielafecha");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_Quinigolfecha",
                table: "Partido",
                column: "Quinigolfecha");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bonoloto_res");

            migrationBuilder.DropTable(
                name: "botes_res");

            migrationBuilder.DropTable(
                name: "eurodreams_res");

            migrationBuilder.DropTable(
                name: "euromillones_res");

            migrationBuilder.DropTable(
                name: "gordo_res");

            migrationBuilder.DropTable(
                name: "lototurf_res");

            migrationBuilder.DropTable(
                name: "nacional_res");

            migrationBuilder.DropTable(
                name: "Partido");

            migrationBuilder.DropTable(
                name: "primitiva_res");

            migrationBuilder.DropTable(
                name: "quintuple_res");

            migrationBuilder.DropTable(
                name: "quiniela_res");

            migrationBuilder.DropTable(
                name: "quinigol_res");
        }
    }
}
