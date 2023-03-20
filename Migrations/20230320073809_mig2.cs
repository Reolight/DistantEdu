using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistantEdu.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentProfiles_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Quizs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizs_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubjectSubscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscribedSubjectId = table.Column<int>(type: "int", nullable: false),
                    StudentProfileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectSubscription_StudentProfiles_StudentProfileId",
                        column: x => x.StudentProfileId,
                        principalTable: "StudentProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubjectSubscription_Subjects_SubscribedSubjectId",
                        column: x => x.SubscribedSubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Query",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Query", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Query_Quizs_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LessonScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassedLessonId = table.Column<int>(type: "int", nullable: false),
                    IsPassed = table.Column<bool>(type: "bit", nullable: false),
                    SubjectSubscriptionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonScore_Lessons_PassedLessonId",
                        column: x => x.PassedLessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonScore_SubjectSubscription_SubjectSubscriptionId",
                        column: x => x.SubjectSubscriptionId,
                        principalTable: "SubjectSubscription",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reply",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QueryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reply_Query_QueryId",
                        column: x => x.QueryId,
                        principalTable: "Query",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuizScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepliedQuizId = table.Column<int>(type: "int", nullable: false),
                    LessonScoreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizScores_LessonScore_LessonScoreId",
                        column: x => x.LessonScoreId,
                        principalTable: "LessonScore",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizScores_Quizs_RepliedQuizId",
                        column: x => x.RepliedQuizId,
                        principalTable: "Quizs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubjectId",
                table: "Lessons",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonScore_PassedLessonId",
                table: "LessonScore",
                column: "PassedLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonScore_SubjectSubscriptionId",
                table: "LessonScore",
                column: "SubjectSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Query_QuizId",
                table: "Query",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizs_LessonId",
                table: "Quizs",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizScores_LessonScoreId",
                table: "QuizScores",
                column: "LessonScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizScores_RepliedQuizId",
                table: "QuizScores",
                column: "RepliedQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Reply_QueryId",
                table: "Reply",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProfiles_SubjectId",
                table: "StudentProfiles",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSubscription_StudentProfileId",
                table: "SubjectSubscription",
                column: "StudentProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSubscription_SubscribedSubjectId",
                table: "SubjectSubscription",
                column: "SubscribedSubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_StudentProfiles_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId",
                principalTable: "StudentProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_StudentProfiles_ProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "QuizScores");

            migrationBuilder.DropTable(
                name: "Reply");

            migrationBuilder.DropTable(
                name: "LessonScore");

            migrationBuilder.DropTable(
                name: "Query");

            migrationBuilder.DropTable(
                name: "SubjectSubscription");

            migrationBuilder.DropTable(
                name: "Quizs");

            migrationBuilder.DropTable(
                name: "StudentProfiles");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "AspNetUsers");
        }
    }
}
