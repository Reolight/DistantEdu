using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistantEdu.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSubscription_Subjects_SubscribedSubjectId",
                table: "SubjectSubscription");

            migrationBuilder.DropIndex(
                name: "IX_SubjectSubscription_SubscribedSubjectId",
                table: "SubjectSubscription");

            migrationBuilder.RenameColumn(
                name: "SubscribedSubjectId",
                table: "SubjectSubscription",
                newName: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "SubjectSubscription",
                newName: "SubscribedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSubscription_SubscribedSubjectId",
                table: "SubjectSubscription",
                column: "SubscribedSubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSubscription_Subjects_SubscribedSubjectId",
                table: "SubjectSubscription",
                column: "SubscribedSubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
