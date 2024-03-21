using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessPartner.Data.Migrations
{
    /// <inheritdoc />
    public partial class musclestrained : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MusclesTrained",
                table: "ExerciseLibrary",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MusclesTrained",
                table: "ExerciseLibrary");
        }
    }
}
