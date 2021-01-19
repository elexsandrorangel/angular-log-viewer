using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LogViewer.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "logviewer");

            migrationBuilder.CreateTable(
                name: "access_logs",
                schema: "logviewer",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    remote_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    remote_user = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    request_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    request_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    http_status = table.Column<int>(type: "integer", nullable: false),
                    bytes_sent = table.Column<int>(type: "integer", nullable: true),
                    http_referer = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    user_agent = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    gzip_ratio = table.Column<float>(type: "real", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("access_logs_pkey", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_logs",
                schema: "logviewer");
        }
    }
}
