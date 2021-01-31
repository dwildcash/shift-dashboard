using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace shift_dashboard.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Delegates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    PublicKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Vote = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    Producedblocks = table.Column<int>(type: "integer", nullable: false),
                    Missedblocks = table.Column<int>(type: "integer", nullable: false),
                    Rate = table.Column<int>(type: "integer", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Approval = table.Column<double>(type: "double precision", nullable: false),
                    Productivity = table.Column<double>(type: "double precision", nullable: false),
                    PoolShare = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delegates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DelegateStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TotalVoters = table.Column<int>(type: "integer", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    TotalVotes = table.Column<long>(type: "bigint", nullable: false),
                    DelegateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegateStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DelegateStats_Delegates_DelegateId",
                        column: x => x.DelegateId,
                        principalTable: "Delegates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Index_Address",
                table: "Delegates",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Index_Date",
                table: "DelegateStats",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateStats_DelegateId",
                table: "DelegateStats",
                column: "DelegateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DelegateStats");

            migrationBuilder.DropTable(
                name: "Delegates");
        }
    }
}
