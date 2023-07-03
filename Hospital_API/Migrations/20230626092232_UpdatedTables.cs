using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_API.Migrations
{
    public partial class UpdatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdmitAilments_Ailment_AilmentId",
                table: "AdmitAilments");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAdmits_Ward_WardId",
                table: "PatientAdmits");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientTranfer_Ward_WardId",
                table: "PatientTranfer");

            migrationBuilder.DropForeignKey(
                name: "FK_Ward_Departments_DepartmentId",
                table: "Ward");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ward",
                table: "Ward");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ailment",
                table: "Ailment");

            migrationBuilder.RenameTable(
                name: "Ward",
                newName: "Wards");

            migrationBuilder.RenameTable(
                name: "Ailment",
                newName: "Ailments");

            migrationBuilder.RenameIndex(
                name: "IX_Ward_DepartmentId",
                table: "Wards",
                newName: "IX_Wards_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wards",
                table: "Wards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ailments",
                table: "Ailments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PharmaceuticalCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmaceuticalCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmaceuticalCategories_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pharmaceuticals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    PharmaceuticalCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmaceuticals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmaceuticals_PharmaceuticalCategories_PharmaceuticalCategoryId",
                        column: x => x.PharmaceuticalCategoryId,
                        principalTable: "PharmaceuticalCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PharmaceuticalCategories_VendorId",
                table: "PharmaceuticalCategories",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmaceuticals_PharmaceuticalCategoryId",
                table: "Pharmaceuticals",
                column: "PharmaceuticalCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdmitAilments_Ailments_AilmentId",
                table: "AdmitAilments",
                column: "AilmentId",
                principalTable: "Ailments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAdmits_Wards_WardId",
                table: "PatientAdmits",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTranfer_Wards_WardId",
                table: "PatientTranfer",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wards_Departments_DepartmentId",
                table: "Wards",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdmitAilments_Ailments_AilmentId",
                table: "AdmitAilments");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAdmits_Wards_WardId",
                table: "PatientAdmits");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientTranfer_Wards_WardId",
                table: "PatientTranfer");

            migrationBuilder.DropForeignKey(
                name: "FK_Wards_Departments_DepartmentId",
                table: "Wards");

            migrationBuilder.DropTable(
                name: "Pharmaceuticals");

            migrationBuilder.DropTable(
                name: "PharmaceuticalCategories");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wards",
                table: "Wards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ailments",
                table: "Ailments");

            migrationBuilder.RenameTable(
                name: "Wards",
                newName: "Ward");

            migrationBuilder.RenameTable(
                name: "Ailments",
                newName: "Ailment");

            migrationBuilder.RenameIndex(
                name: "IX_Wards_DepartmentId",
                table: "Ward",
                newName: "IX_Ward_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ward",
                table: "Ward",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ailment",
                table: "Ailment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdmitAilments_Ailment_AilmentId",
                table: "AdmitAilments",
                column: "AilmentId",
                principalTable: "Ailment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAdmits_Ward_WardId",
                table: "PatientAdmits",
                column: "WardId",
                principalTable: "Ward",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTranfer_Ward_WardId",
                table: "PatientTranfer",
                column: "WardId",
                principalTable: "Ward",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ward_Departments_DepartmentId",
                table: "Ward",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
