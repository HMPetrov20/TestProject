using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PractiseApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Addvalues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TestDouble",
                table: "TestModels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TestNumber",
                table: "TestModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestDouble",
                table: "TestModels");

            migrationBuilder.DropColumn(
                name: "TestNumber",
                table: "TestModels");
        }
    }
}
