using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notkace.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ASSET",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    ASSET_TYPE_ID = table.Column<long>(type: "bigint", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASSET", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HD_PRIORITY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ORDINAL = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_PRIORITY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HD_STATUS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ORDINAL = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_STATUS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    USER_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FULL_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ROLE_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HD_TICKET",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    TITLE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SUMMARY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HD_QUEUE_ID = table.Column<long>(type: "bigint", nullable: false),
                    CREATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HD_PRIORITY_ID = table.Column<long>(type: "bigint", nullable: true),
                    HD_STATUS_ID = table.Column<long>(type: "bigint", nullable: true),
                    OWNER_ID = table.Column<long>(type: "bigint", nullable: true),
                    SUBMITTER_ID = table.Column<long>(type: "bigint", nullable: true),
                    ASSET_ID = table.Column<long>(type: "bigint", nullable: true),
                    CUSTOM_FIELD_VALUE1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUSTOM_FIELD_VALUE2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUSTOM_FIELD_VALUE5 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_TICKET", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HD_TICKET_ASSET_ASSET_ID",
                        column: x => x.ASSET_ID,
                        principalTable: "ASSET",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HD_TICKET_HD_PRIORITY_HD_PRIORITY_ID",
                        column: x => x.HD_PRIORITY_ID,
                        principalTable: "HD_PRIORITY",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HD_TICKET_HD_STATUS_HD_STATUS_ID",
                        column: x => x.HD_STATUS_ID,
                        principalTable: "HD_STATUS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HD_TICKET_USER_OWNER_ID",
                        column: x => x.OWNER_ID,
                        principalTable: "USER",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HD_TICKET_USER_SUBMITTER_ID",
                        column: x => x.SUBMITTER_ID,
                        principalTable: "USER",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HD_TICKET_CHANGE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    HD_TICKET_ID = table.Column<long>(type: "bigint", nullable: false),
                    TIMESTAMP = table.Column<DateTime>(type: "datetime2", nullable: false),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    COMMENT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OWNERS_ONLY = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HD_TICKET_CHANGE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HD_TICKET_CHANGE_HD_TICKET_HD_TICKET_ID",
                        column: x => x.HD_TICKET_ID,
                        principalTable: "HD_TICKET",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HD_TICKET_CHANGE_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USER",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ASSET_NAME",
                table: "ASSET",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_HD_TICKET_ASSET_ID",
                table: "HD_TICKET",
                column: "ASSET_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HD_TICKET_HD_PRIORITY_ID",
                table: "HD_TICKET",
                column: "HD_PRIORITY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HD_TICKET_HD_QUEUE_ID",
                table: "HD_TICKET",
                column: "HD_QUEUE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HD_TICKET_HD_STATUS_ID",
                table: "HD_TICKET",
                column: "HD_STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HD_TICKET_OWNER_ID_HD_STATUS_ID",
                table: "HD_TICKET",
                columns: new[] { "OWNER_ID", "HD_STATUS_ID" });

            migrationBuilder.CreateIndex(
                name: "IX_HD_TICKET_SUBMITTER_ID",
                table: "HD_TICKET",
                column: "SUBMITTER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HD_TICKET_CHANGE_HD_TICKET_ID",
                table: "HD_TICKET_CHANGE",
                column: "HD_TICKET_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HD_TICKET_CHANGE_USER_ID",
                table: "HD_TICKET_CHANGE",
                column: "USER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HD_TICKET_CHANGE");

            migrationBuilder.DropTable(
                name: "HD_TICKET");

            migrationBuilder.DropTable(
                name: "ASSET");

            migrationBuilder.DropTable(
                name: "HD_PRIORITY");

            migrationBuilder.DropTable(
                name: "HD_STATUS");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
