using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingApp.Migrations
{
    /// <inheritdoc />
    public partial class RestrictDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Exercises_ExerciseId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_TrainingPrograms_ProgramId",
                table: "Exercises");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Exercises_ExerciseId",
                table: "Activities",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_TrainingPrograms_ProgramId",
                table: "Exercises",
                column: "ProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Exercises_ExerciseId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_TrainingPrograms_ProgramId",
                table: "Exercises");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Exercises_ExerciseId",
                table: "Activities",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_TrainingPrograms_ProgramId",
                table: "Exercises",
                column: "ProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
