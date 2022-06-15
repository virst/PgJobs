using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PgJobs.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    last_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    this_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    next_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    interval = table.Column<string>(type: "text", nullable: true),
                    failures = table.Column<int>(type: "integer", nullable: false),
                    what = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "jobs_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_id = table.Column<int>(type: "integer", nullable: false),
                    date_beg = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    err_txt = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_jobs_logs_jobs_job_id",
                        column: x => x.job_id,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_jobs_logs_job_id",
                table: "jobs_logs",
                column: "job_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jobs_logs");

            migrationBuilder.DropTable(
                name: "jobs");
        }
    }
}
