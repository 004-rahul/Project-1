using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // FullName is now a derived (not stored) property — replaced by first/last name + address.
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(name: "FirstName", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "LastName", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "AddressLine", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "City", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "State", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "PostalCode", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Country", table: "AspNetUsers", type: "nvarchar(max)", nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "FirstName", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "LastName", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "AddressLine", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "City", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "State", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "PostalCode", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "Country", table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
