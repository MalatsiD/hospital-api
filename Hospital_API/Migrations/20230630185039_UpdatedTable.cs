using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_API.Migrations
{
    public partial class UpdatedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "PharmaceuticalCategories",
                newName: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_HospitalId",
                table: "Vendors",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Hospitals_HospitalId",
                table: "Vendors",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Hospitals_HospitalId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_HospitalId",
                table: "Vendors");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PharmaceuticalCategories",
                newName: "Email");
        }
    }
}
