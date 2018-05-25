using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IsaksMusic.Migrations
{
    public partial class RemovedPlayCountFromSongModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayCount",
                table: "Songs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayCount",
                table: "Songs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
