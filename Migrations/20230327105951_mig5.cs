using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistantEdu.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonScore_Lessons_PassedLessonId",
                table: "LessonScore");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonScore_SubjectSubscription_SubjectSubscriptionId",
                table: "LessonScore");

            migrationBuilder.DropForeignKey(
                name: "FK_Query_Quizs_QuizId",
                table: "Query");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizs_Lessons_LessonId",
                table: "Quizs");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizScores_LessonScore_LessonScoreId",
                table: "QuizScores");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizScores_Quizs_RepliedQuizId",
                table: "QuizScores");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSubscription_StudentProfiles_StudentProfileId",
                table: "SubjectSubscription");

            migrationBuilder.DropIndex(
                name: "IX_QuizScores_RepliedQuizId",
                table: "QuizScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quizs",
                table: "Quizs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonScore",
                table: "LessonScore");

            migrationBuilder.DropIndex(
                name: "IX_LessonScore_PassedLessonId",
                table: "LessonScore");

            migrationBuilder.RenameTable(
                name: "Quizs",
                newName: "Quizzes");

            migrationBuilder.RenameTable(
                name: "LessonScore",
                newName: "LessonScores");

            migrationBuilder.RenameColumn(
                name: "RepliedQuizId",
                table: "QuizScores",
                newName: "Score");

            migrationBuilder.RenameIndex(
                name: "IX_Quizs_LessonId",
                table: "Quizzes",
                newName: "IX_Quizzes_LessonId");

            migrationBuilder.RenameColumn(
                name: "PassedLessonId",
                table: "LessonScores",
                newName: "LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_LessonScore_SubjectSubscriptionId",
                table: "LessonScores",
                newName: "IX_LessonScores_SubjectSubscriptionId");

            migrationBuilder.AlterColumn<int>(
                name: "StudentProfileId",
                table: "SubjectSubscription",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LessonScoreId",
                table: "QuizScores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "QuizScores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "QuizScores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "QuizScores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StudentProfileId",
                table: "QuizScores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Query",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QType",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectSubscriptionId",
                table: "LessonScores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonScores",
                table: "LessonScores",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QueryReplied",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryId = table.Column<int>(type: "int", nullable: false),
                    isReplied = table.Column<bool>(type: "bit", nullable: false),
                    isCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuizScoreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryReplied", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryReplied_QuizScores_QuizScoreId",
                        column: x => x.QuizScoreId,
                        principalTable: "QuizScores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Replied",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReplyId = table.Column<int>(type: "int", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false),
                    QueryRepliedId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replied", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Replied_QueryReplied_QueryRepliedId",
                        column: x => x.QueryRepliedId,
                        principalTable: "QueryReplied",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizScores_StudentProfileId",
                table: "QuizScores",
                column: "StudentProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryReplied_QuizScoreId",
                table: "QueryReplied",
                column: "QuizScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Replied_QueryRepliedId",
                table: "Replied",
                column: "QueryRepliedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonScores_SubjectSubscription_SubjectSubscriptionId",
                table: "LessonScores",
                column: "SubjectSubscriptionId",
                principalTable: "SubjectSubscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Query_Quizzes_QuizId",
                table: "Query",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizScores_LessonScores_LessonScoreId",
                table: "QuizScores",
                column: "LessonScoreId",
                principalTable: "LessonScores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizScores_StudentProfiles_StudentProfileId",
                table: "QuizScores",
                column: "StudentProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Lessons_LessonId",
                table: "Quizzes",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSubscription_StudentProfiles_StudentProfileId",
                table: "SubjectSubscription",
                column: "StudentProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonScores_SubjectSubscription_SubjectSubscriptionId",
                table: "LessonScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Query_Quizzes_QuizId",
                table: "Query");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizScores_LessonScores_LessonScoreId",
                table: "QuizScores");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizScores_StudentProfiles_StudentProfileId",
                table: "QuizScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Lessons_LessonId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSubscription_StudentProfiles_StudentProfileId",
                table: "SubjectSubscription");

            migrationBuilder.DropTable(
                name: "Replied");

            migrationBuilder.DropTable(
                name: "QueryReplied");

            migrationBuilder.DropIndex(
                name: "IX_QuizScores_StudentProfileId",
                table: "QuizScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonScores",
                table: "LessonScores");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "QuizScores");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "QuizScores");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "QuizScores");

            migrationBuilder.DropColumn(
                name: "StudentProfileId",
                table: "QuizScores");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Query");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "QType",
                table: "Quizzes");

            migrationBuilder.RenameTable(
                name: "Quizzes",
                newName: "Quizs");

            migrationBuilder.RenameTable(
                name: "LessonScores",
                newName: "LessonScore");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "QuizScores",
                newName: "RepliedQuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_LessonId",
                table: "Quizs",
                newName: "IX_Quizs_LessonId");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                table: "LessonScore",
                newName: "PassedLessonId");

            migrationBuilder.RenameIndex(
                name: "IX_LessonScores_SubjectSubscriptionId",
                table: "LessonScore",
                newName: "IX_LessonScore_SubjectSubscriptionId");

            migrationBuilder.AlterColumn<int>(
                name: "StudentProfileId",
                table: "SubjectSubscription",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LessonScoreId",
                table: "QuizScores",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Lessons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "Quizs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectSubscriptionId",
                table: "LessonScore",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizs",
                table: "Quizs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonScore",
                table: "LessonScore",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QuizScores_RepliedQuizId",
                table: "QuizScores",
                column: "RepliedQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonScore_PassedLessonId",
                table: "LessonScore",
                column: "PassedLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonScore_Lessons_PassedLessonId",
                table: "LessonScore",
                column: "PassedLessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonScore_SubjectSubscription_SubjectSubscriptionId",
                table: "LessonScore",
                column: "SubjectSubscriptionId",
                principalTable: "SubjectSubscription",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Query_Quizs_QuizId",
                table: "Query",
                column: "QuizId",
                principalTable: "Quizs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizs_Lessons_LessonId",
                table: "Quizs",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizScores_LessonScore_LessonScoreId",
                table: "QuizScores",
                column: "LessonScoreId",
                principalTable: "LessonScore",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizScores_Quizs_RepliedQuizId",
                table: "QuizScores",
                column: "RepliedQuizId",
                principalTable: "Quizs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSubscription_StudentProfiles_StudentProfileId",
                table: "SubjectSubscription",
                column: "StudentProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id");
        }
    }
}
