using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parkman.Migrations
{
    /// <inheritdoc />
    public partial class PairingApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PairingPassword",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompanyApproved",
                table: "PersonProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PairingPassword",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "IsCompanyApproved",
                table: "PersonProfiles");
        }
    }
}
