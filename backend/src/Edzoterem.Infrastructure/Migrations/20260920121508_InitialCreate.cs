using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edzoterem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BerletTipusok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nev = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Kategoria = table.Column<int>(type: "int", nullable: false),
                    Ar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    AlapertelmezettAlkalmak = table.Column<int>(type: "int", nullable: true),
                    Aktiv = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BerletTipusok", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Munkakorok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nev = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Munkakorok", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SzolgaltatasTipusok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nev = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AlapertelmezettAr = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Aktiv = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SzolgaltatasTipusok", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tagok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Vezeteknev = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Keresztnev = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SzuletesiDatum = table.Column<DateOnly>(type: "date", nullable: false),
                    Cim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegisztracioDatuma = table.Column<DateOnly>(type: "date", nullable: false),
                    VhKapcsolattartoNev = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VhKapcsolattartoTelefon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nem = table.Column<int>(type: "int", nullable: false),
                    Megjegyzes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aktiv = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tagok", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Dolgozok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Vezeteknev = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Keresztnev = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Becenev = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MunkakorId = table.Column<int>(type: "int", nullable: false),
                    SzemelyiSzam = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TajSzam = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adoszam = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SzuletesiDatum = table.Column<DateOnly>(type: "date", nullable: false),
                    Nem = table.Column<int>(type: "int", nullable: false),
                    FenykepUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PortfolioUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Oradij = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Megjegyzes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aktiv = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Letrehozva = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modositva = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dolgozok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dolgozok_Munkakorok_MunkakorId",
                        column: x => x.MunkakorId,
                        principalTable: "Munkakorok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Edzok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DolgozoId = table.Column<int>(type: "int", nullable: false),
                    Szakterulet = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Vegzettseg = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Oradij = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edzok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Edzok_Dolgozok_DolgozoId",
                        column: x => x.DolgozoId,
                        principalTable: "Dolgozok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Felhasznalok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Felhasznalonev = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JelszoHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Szerepkor = table.Column<int>(type: "int", nullable: false),
                    DolgozoId = table.Column<int>(type: "int", nullable: true),
                    TagId = table.Column<int>(type: "int", nullable: true),
                    Aktiv = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JelszoIdeiglenes = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UtolsoBejelentkezes = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Felhasznalok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Felhasznalok_Dolgozok_DolgozoId",
                        column: x => x.DolgozoId,
                        principalTable: "Dolgozok",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Felhasznalok_Tagok_TagId",
                        column: x => x.TagId,
                        principalTable: "Tagok",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Munkarendek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DolgozoId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    TolIdo = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    IgIdo = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    Megjegyzes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Munkarendek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Munkarendek_Dolgozok_DolgozoId",
                        column: x => x.DolgozoId,
                        principalTable: "Dolgozok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EdzoElerhetosegek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EdzoId = table.Column<int>(type: "int", nullable: false),
                    HetNapja = table.Column<int>(type: "int", nullable: false),
                    TolIdo = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    IgIdo = table.Column<TimeOnly>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdzoElerhetosegek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EdzoElerhetosegek_Edzok_EdzoId",
                        column: x => x.EdzoId,
                        principalTable: "Edzok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Berletek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    BerletTipusId = table.Column<int>(type: "int", nullable: false),
                    KezdoDatum = table.Column<DateOnly>(type: "date", nullable: false),
                    LejaratiDatum = table.Column<DateOnly>(type: "date", nullable: false),
                    ArFizetve = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    HatralevoAlkalmak = table.Column<int>(type: "int", nullable: true),
                    Statusz = table.Column<int>(type: "int", nullable: false),
                    LetrehozvaAltalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Berletek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Berletek_BerletTipusok_BerletTipusId",
                        column: x => x.BerletTipusId,
                        principalTable: "BerletTipusok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Berletek_Felhasznalok_LetrehozvaAltalId",
                        column: x => x.LetrehozvaAltalId,
                        principalTable: "Felhasznalok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Berletek_Tagok_TagId",
                        column: x => x.TagId,
                        principalTable: "Tagok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CsoportosFoglalkozasok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Megnevezes = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EdzoId = table.Column<int>(type: "int", nullable: false),
                    Idopont = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Helyszin = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxLetszam = table.Column<int>(type: "int", nullable: false),
                    Statusz = table.Column<int>(type: "int", nullable: false),
                    MeghirdetteId = table.Column<int>(type: "int", nullable: false),
                    JovahagytaId = table.Column<int>(type: "int", nullable: true),
                    JovahagyasDatuma = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LezarasDatuma = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ElutasitasIndoka = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsoportosFoglalkozasok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CsoportosFoglalkozasok_Edzok_EdzoId",
                        column: x => x.EdzoId,
                        principalTable: "Edzok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CsoportosFoglalkozasok_Felhasznalok_JovahagytaId",
                        column: x => x.JovahagytaId,
                        principalTable: "Felhasznalok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CsoportosFoglalkozasok_Felhasznalok_MeghirdetteId",
                        column: x => x.MeghirdetteId,
                        principalTable: "Felhasznalok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EgyeniFoglalkozasok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SzolgaltatasTipusId = table.Column<int>(type: "int", nullable: false),
                    DolgozoId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    Idopont = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ar = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Allapot = table.Column<int>(type: "int", nullable: false),
                    RogzitveAltalId = table.Column<int>(type: "int", nullable: false),
                    Megjegyzes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgyeniFoglalkozasok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EgyeniFoglalkozasok_Dolgozok_DolgozoId",
                        column: x => x.DolgozoId,
                        principalTable: "Dolgozok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EgyeniFoglalkozasok_Felhasznalok_RogzitveAltalId",
                        column: x => x.RogzitveAltalId,
                        principalTable: "Felhasznalok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EgyeniFoglalkozasok_SzolgaltatasTipusok_SzolgaltatasTipusId",
                        column: x => x.SzolgaltatasTipusId,
                        principalTable: "SzolgaltatasTipusok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EgyeniFoglalkozasok_Tagok_TagId",
                        column: x => x.TagId,
                        principalTable: "Tagok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CsoportosJelentkezesek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CsoportosFoglalkozasId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    JelentkezesDatuma = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RogzitveAltalId = table.Column<int>(type: "int", nullable: false),
                    JelenVolt = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsoportosJelentkezesek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CsoportosJelentkezesek_CsoportosFoglalkozasok_CsoportosFogla~",
                        column: x => x.CsoportosFoglalkozasId,
                        principalTable: "CsoportosFoglalkozasok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CsoportosJelentkezesek_Felhasznalok_RogzitveAltalId",
                        column: x => x.RogzitveAltalId,
                        principalTable: "Felhasznalok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CsoportosJelentkezesek_Tagok_TagId",
                        column: x => x.TagId,
                        principalTable: "Tagok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Berletek_BerletTipusId",
                table: "Berletek",
                column: "BerletTipusId");

            migrationBuilder.CreateIndex(
                name: "IX_Berletek_LetrehozvaAltalId",
                table: "Berletek",
                column: "LetrehozvaAltalId");

            migrationBuilder.CreateIndex(
                name: "IX_Berletek_TagId",
                table: "Berletek",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_CsoportosFoglalkozasok_EdzoId",
                table: "CsoportosFoglalkozasok",
                column: "EdzoId");

            migrationBuilder.CreateIndex(
                name: "IX_CsoportosFoglalkozasok_JovahagytaId",
                table: "CsoportosFoglalkozasok",
                column: "JovahagytaId");

            migrationBuilder.CreateIndex(
                name: "IX_CsoportosFoglalkozasok_MeghirdetteId",
                table: "CsoportosFoglalkozasok",
                column: "MeghirdetteId");

            migrationBuilder.CreateIndex(
                name: "IX_CsoportosJelentkezesek_CsoportosFoglalkozasId_TagId",
                table: "CsoportosJelentkezesek",
                columns: new[] { "CsoportosFoglalkozasId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CsoportosJelentkezesek_RogzitveAltalId",
                table: "CsoportosJelentkezesek",
                column: "RogzitveAltalId");

            migrationBuilder.CreateIndex(
                name: "IX_CsoportosJelentkezesek_TagId",
                table: "CsoportosJelentkezesek",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Dolgozok_MunkakorId",
                table: "Dolgozok",
                column: "MunkakorId");

            migrationBuilder.CreateIndex(
                name: "IX_EdzoElerhetosegek_EdzoId",
                table: "EdzoElerhetosegek",
                column: "EdzoId");

            migrationBuilder.CreateIndex(
                name: "IX_Edzok_DolgozoId",
                table: "Edzok",
                column: "DolgozoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EgyeniFoglalkozasok_DolgozoId",
                table: "EgyeniFoglalkozasok",
                column: "DolgozoId");

            migrationBuilder.CreateIndex(
                name: "IX_EgyeniFoglalkozasok_RogzitveAltalId",
                table: "EgyeniFoglalkozasok",
                column: "RogzitveAltalId");

            migrationBuilder.CreateIndex(
                name: "IX_EgyeniFoglalkozasok_SzolgaltatasTipusId",
                table: "EgyeniFoglalkozasok",
                column: "SzolgaltatasTipusId");

            migrationBuilder.CreateIndex(
                name: "IX_EgyeniFoglalkozasok_TagId",
                table: "EgyeniFoglalkozasok",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Felhasznalok_DolgozoId",
                table: "Felhasznalok",
                column: "DolgozoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Felhasznalok_Felhasznalonev",
                table: "Felhasznalok",
                column: "Felhasznalonev",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Felhasznalok_TagId",
                table: "Felhasznalok",
                column: "TagId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Munkarendek_DolgozoId",
                table: "Munkarendek",
                column: "DolgozoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Berletek");

            migrationBuilder.DropTable(
                name: "CsoportosJelentkezesek");

            migrationBuilder.DropTable(
                name: "EdzoElerhetosegek");

            migrationBuilder.DropTable(
                name: "EgyeniFoglalkozasok");

            migrationBuilder.DropTable(
                name: "Munkarendek");

            migrationBuilder.DropTable(
                name: "BerletTipusok");

            migrationBuilder.DropTable(
                name: "CsoportosFoglalkozasok");

            migrationBuilder.DropTable(
                name: "SzolgaltatasTipusok");

            migrationBuilder.DropTable(
                name: "Edzok");

            migrationBuilder.DropTable(
                name: "Felhasznalok");

            migrationBuilder.DropTable(
                name: "Dolgozok");

            migrationBuilder.DropTable(
                name: "Tagok");

            migrationBuilder.DropTable(
                name: "Munkakorok");
        }
    }
}
