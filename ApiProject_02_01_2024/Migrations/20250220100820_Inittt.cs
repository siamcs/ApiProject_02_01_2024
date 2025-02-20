﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProject_02_01_2024.Migrations
{
    /// <inheritdoc />
    public partial class Inittt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Designations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Designations");
        }
    }
}
