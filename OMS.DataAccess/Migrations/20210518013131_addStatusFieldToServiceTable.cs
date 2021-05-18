﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace OMS.DataAccess.Migrations
{
    public partial class addStatusFieldToServiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Service",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Service");
        }
    }
}
