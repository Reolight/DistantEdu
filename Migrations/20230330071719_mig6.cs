using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistantEdu.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Query_Quizzes_QuizId",
                table: "Query");

            migrationBuilder.DropForeignKey(
                name: "FK_QueryReplied_QuizScores_QuizScoreId",
                table: "QueryReplied");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizScores_StudentProfiles_StudentProfileId",
                table: "QuizScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Replied_QueryReplied_QueryRepliedId",
                table: "Replied");

            migrationBuilder.DropForeignKey(
                name: "FK_Reply_Query_QueryId",
                table: "Reply");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProfiles_Subjects_SubjectId",
                table: "StudentProfiles");

            migrationBuilder.DropIndex(
                name: "IX_StudentProfiles_SubjectId",
                table: "StudentProfiles");

            migrationBuilder.DropIndex(
                name: "IX_QuizScores_StudentProfileId",
                table: "QuizScores");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "StudentProfileId",
                table: "QuizScores");

            migrationBuilder.AlterColumn<int>(
                name: "QueryId",
                table: "Reply",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QueryRepliedId",
                table: "Replied",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "StartTime",
                table: "QuizScores",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "QuizScores",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "EndTime",
                table: "QuizScores",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "isCorrect",
                table: "QueryReplied",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "QuizScoreId",
                table: "QueryReplied",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "Query",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSubscription_SubjectId",
                table: "SubjectSubscription",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Replied_ReplyId",
                table: "Replied",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizScores_QuizId",
                table: "QuizScores",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryReplied_QueryId",
                table: "QueryReplied",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonScores_LessonId",
                table: "LessonScores",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonScores_Lessons_LessonId",
                table: "LessonScores",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Query_Quizzes_QuizId",
                table: "Query",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QueryReplied_Query_QueryId",
                table: "QueryReplied",
                column: "QueryId",
                principalTable: "Query",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QueryReplied_QuizScores_QuizScoreId",
                table: "QueryReplied",
                column: "QuizScoreId",
                principalTable: "QuizScores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizScores_Quizzes_QuizId",
                table: "QuizScores",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replied_QueryReplied_QueryRepliedId",
                table: "Replied",
                column: "QueryRepliedId",
                principalTable: "QueryReplied",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replied_Reply_ReplyId",
                table: "Replied",
                column: "ReplyId",
                principalTable: "Reply",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_Query_QueryId",
                table: "Reply",
                column: "QueryId",
                principalTable: "Query",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSubscription_Subjects_SubjectId",
                table: "SubjectSubscription",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonScores_Lessons_LessonId",
                table: "LessonScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Query_Quizzes_QuizId",
                table: "Query");

            migrationBuilder.DropForeignKey(
                name: "FK_QueryReplied_Query_QueryId",
                table: "QueryReplied");

            migrationBuilder.DropForeignKey(
                name: "FK_QueryReplied_QuizScores_QuizScoreId",
                table: "QueryReplied");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizScores_Quizzes_QuizId",
                table: "QuizScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Replied_QueryReplied_QueryRepliedId",
                table: "Replied");

            migrationBuilder.DropForeignKey(
                name: "FK_Replied_Reply_ReplyId",
                table: "Replied");

            migrationBuilder.DropForeignKey(
                name: "FK_Reply_Query_QueryId",
                table: "Reply");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSubscription_Subjects_SubjectId",
                table: "SubjectSubscription");

            migrationBuilder.DropIndex(
                name: "IX_SubjectSubscription_SubjectId",
                table: "SubjectSubscription");

            migrationBuilder.DropIndex(
                name: "IX_Replied_ReplyId",
                table: "Replied");

            migrationBuilder.DropIndex(
                name: "IX_QuizScores_QuizId",
                table: "QuizScores");

            migrationBuilder.DropIndex(
                name: "IX_QueryReplied_QueryId",
                table: "QueryReplied");

            migrationBuilder.DropIndex(
                name: "IX_LessonScores_LessonId",
                table: "LessonScores");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "StudentProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QueryId",
                table: "Reply",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "QueryRepliedId",
                table: "Replied",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "QuizScores",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "QuizScores",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "QuizScores",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentProfileId",
                table: "QuizScores",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isCorrect",
                table: "QueryReplied",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "QuizScoreId",
                table: "QueryReplied",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "Query",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProfiles_SubjectId",
                table: "StudentProfiles",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizScores_StudentProfileId",
                table: "QuizScores",
                column: "StudentProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Query_Quizzes_QuizId",
                table: "Query",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QueryReplied_QuizScores_QuizScoreId",
                table: "QueryReplied",
                column: "QuizScoreId",
                principalTable: "QuizScores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizScores_StudentProfiles_StudentProfileId",
                table: "QuizScores",
                column: "StudentProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Replied_QueryReplied_QueryRepliedId",
                table: "Replied",
                column: "QueryRepliedId",
                principalTable: "QueryReplied",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_Query_QueryId",
                table: "Reply",
                column: "QueryId",
                principalTable: "Query",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProfiles_Subjects_SubjectId",
                table: "StudentProfiles",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }
    }
}
