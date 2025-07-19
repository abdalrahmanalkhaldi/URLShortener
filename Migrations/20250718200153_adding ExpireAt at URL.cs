using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShortenerApiApplication.Migrations
{
    /// <inheritdoc />
    public partial class addingExpireAtatURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Urls",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Urls");
        }
    }
}
