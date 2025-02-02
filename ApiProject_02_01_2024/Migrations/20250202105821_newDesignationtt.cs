using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProject_02_01_2024.Migrations
{
    /// <inheritdoc />
    public partial class newDesignationtt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Designations",
                columns: table => new
                {
                    DesignationAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesignationCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    DesignationName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LUser = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LIP = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LMAC = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.DesignationAutoId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Designations");
        }
    }
}
