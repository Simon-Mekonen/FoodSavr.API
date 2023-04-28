using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSavr.API.Migrations
{
    /// <inheritdoc />
    public partial class databaseupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_Measurement_MeasurementId",
                table: "RecipeIngredient");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "RecipeIngredient",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "MeasurementId",
                table: "RecipeIngredient",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_Measurement_MeasurementId",
                table: "RecipeIngredient",
                column: "MeasurementId",
                principalTable: "Measurement",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_Measurement_MeasurementId",
                table: "RecipeIngredient");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "RecipeIngredient",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MeasurementId",
                table: "RecipeIngredient",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_Measurement_MeasurementId",
                table: "RecipeIngredient",
                column: "MeasurementId",
                principalTable: "Measurement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
