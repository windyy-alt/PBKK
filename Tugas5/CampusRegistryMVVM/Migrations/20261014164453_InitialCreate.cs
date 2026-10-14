using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusRegistryMVVM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mahasiswa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nrp = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Nama = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Prodi = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Ipk = table.Column<decimal>(type: "TEXT", nullable: false),
                    JenisKelamin = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    TanggalLahir = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Alamat = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    NoTelepon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahasiswa", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mahasiswa_Nrp",
                table: "Mahasiswa",
                column: "Nrp",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mahasiswa");
        }
    }
}
