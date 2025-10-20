using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectricBusinessCard.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElectroWorks_CategoriesWorks_CategoryId",
                table: "ElectroWorks");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectroWorks_CategoriesWorks_CategoryId",
                table: "ElectroWorks",
                column: "CategoryId",
                principalTable: "CategoriesWorks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElectroWorks_CategoriesWorks_CategoryId",
                table: "ElectroWorks");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectroWorks_CategoriesWorks_CategoryId",
                table: "ElectroWorks",
                column: "CategoryId",
                principalTable: "CategoriesWorks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
