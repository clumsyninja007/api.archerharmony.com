using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.archerharmony.com.Migrations.Notkace;

public partial class NotkaceInitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ASSET",
            columns: table => new
            {
                ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                ASSET_TYPE_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false, defaultValueSql: "0"),
                NAME = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                ASSET_DATA_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                OWNER_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                LOCATION_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                MODIFIED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                CREATED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                MAPPED_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                ASSET_CLASS_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                ARCHIVE = table.Column<string>(type: "enum('PENDING','COMPLETED','')", nullable: true),
                ARCHIVE_DATE = table.Column<DateTime>(nullable: true),
                ARCHIVE_REASON = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                ASSET_STATUS_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ASSET", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "HD_PRIORITY",
            columns: table => new
            {
                ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                HD_QUEUE_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false, defaultValueSql: "0"),
                NAME = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                ORDINAL = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false, defaultValueSql: "0"),
                COLOR = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                ESCALATION_MINUTES = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                USE_BUSINESS_HOURS_FOR_ESCALATION = table.Column<byte>(type: "tinyint(1) unsigned", nullable: true, defaultValueSql: "0"),
                IS_SLA_ENABLED = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                RESOLUTION_DUE_DATE_MINUTES = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                USE_BUSINESS_HOURS_FOR_SLA = table.Column<byte>(type: "tinyint(1) unsigned", nullable: true, defaultValueSql: "0"),
                SLA_NOTIFICATION_RECURRENCE_MINUTES = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HD_PRIORITY", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "HD_STATUS",
            columns: table => new
            {
                ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                HD_QUEUE_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false, defaultValueSql: "0"),
                NAME = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                ORDINAL = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false, defaultValueSql: "0"),
                STATE = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HD_STATUS", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "USER",
            columns: table => new
            {
                ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                USER_NAME = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                PASSWORD = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                EMAIL = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                BUDGET_CODE = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                DOMAIN = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                FULL_NAME = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                MODIFIED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                CREATED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                LDAP_IMPORTED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                PERMISSIONS = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                LDAP_UID = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                HOME_PHONE = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                WORK_PHONE = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                MOBILE_PHONE = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                PAGER_PHONE = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                ROLE_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                LINKED_APPLIANCE_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                PATH = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                LEVEL = table.Column<int>(type: "int(11)", nullable: true),
                MANAGER_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                LOCATION_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                LOCALE_BROWSER_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                HD_DEFAULT_QUEUE_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                HD_DEFAULT_VIEW = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                API_ENABLED = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                SECURITY_NOTIFICATIONS = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                SALES_NOTIFICATIONS = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                PRIMARY_DEVICE_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                DEVICE_COUNT = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                IS_ARCHIVED = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                ARCHIVED_DATE = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                ARCHIVED_BY = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                _2FA_REQUIRED = table.Column<sbyte>(name: "2FA_REQUIRED", type: "tinyint(2)", nullable: true, defaultValueSql: "0"),
                _2FA_CONFIGURED = table.Column<sbyte>(name: "2FA_CONFIGURED", type: "tinyint(2)", nullable: true, defaultValueSql: "0"),
                _2FA_SECRET = table.Column<byte[]>(name: "2FA_SECRET", type: "tinyblob", nullable: true),
                _2FA_CUTOFF_DATE = table.Column<DateTimeOffset>(name: "2FA_CUTOFF_DATE", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                THEME = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_USER", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "HD_TICKET",
            columns: table => new
            {
                ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                TITLE = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                SUMMARY = table.Column<string>(unicode: false, nullable: true),
                HD_PRIORITY_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                HD_IMPACT_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                MODIFIED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                CREATED = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                OWNER_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                SUBMITTER_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                HD_STATUS_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                HD_QUEUE_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false, defaultValueSql: "0"),
                HD_CATEGORY_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                CC_LIST = table.Column<string>(unicode: false, nullable: false),
                ESCALATED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                CUSTOM_FIELD_VALUE0 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE1 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE2 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE3 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE4 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE5 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE6 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE7 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE8 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE9 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE10 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE11 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE12 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE13 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE14 = table.Column<string>(unicode: false, nullable: true),
                CUSTOM_FIELD_VALUE15 = table.Column<string>(unicode: false, nullable: true),
                DUE_DATE = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                IS_MANUAL_DUE_DATE = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                SLA_NOTIFIED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                TIME_OPENED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                TIME_CLOSED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                TIME_STALLED = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                MACHINE_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                SATISFACTION_RATING = table.Column<int>(type: "int(11)", nullable: true),
                SATISFACTION_COMMENT = table.Column<string>(unicode: false, nullable: true),
                RESOLUTION = table.Column<string>(unicode: false, nullable: true),
                ASSET_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                PARENT_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                IS_PARENT = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                APPROVER_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                APPROVE_STATE = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                APPROVAL = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                APPROVAL_NOTE = table.Column<string>(unicode: false, nullable: true),
                SERVICE_TICKET_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                HD_SERVICE_STATUS_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                HD_USE_PROCESS_STATUS = table.Column<sbyte>(type: "tinyint(4)", nullable: true, defaultValueSql: "0")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HD_TICKET", x => x.ID);
                table.ForeignKey(
                    name: "FK_HD_TICKET_ASSET_ASSET_ID",
                    column: x => x.ASSET_ID,
                    principalTable: "ASSET",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_HD_TICKET_HD_PRIORITY_HD_PRIORITY_ID",
                    column: x => x.HD_PRIORITY_ID,
                    principalTable: "HD_PRIORITY",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_HD_TICKET_HD_STATUS_HD_STATUS_ID",
                    column: x => x.HD_STATUS_ID,
                    principalTable: "HD_STATUS",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_HD_TICKET_ASSET_MACHINE_ID",
                    column: x => x.MACHINE_ID,
                    principalTable: "ASSET",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_HD_TICKET_USER_OWNER_ID",
                    column: x => x.OWNER_ID,
                    principalTable: "USER",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_HD_TICKET_USER_SUBMITTER_ID",
                    column: x => x.SUBMITTER_ID,
                    principalTable: "USER",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "HD_TICKET_CHANGE",
            columns: table => new
            {
                ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                HD_TICKET_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                TIMESTAMP = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                USER_ID = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "0"),
                COMMENT = table.Column<string>(type: "mediumtext", nullable: true),
                COMMENT_LOC = table.Column<string>(type: "mediumtext", nullable: true),
                DESCRIPTION = table.Column<string>(type: "mediumtext", nullable: true),
                OWNERS_ONLY_DESCRIPTION = table.Column<string>(type: "mediumtext", nullable: true),
                LOCALIZED_DESCRIPTION = table.Column<string>(type: "mediumtext", nullable: true),
                LOCALIZED_OWNERS_ONLY_DESCRIPTION = table.Column<string>(type: "mediumtext", nullable: true),
                MAILED = table.Column<byte>(type: "tinyint(1) unsigned", nullable: true, defaultValueSql: "0"),
                MAILED_TIMESTAMP = table.Column<DateTime>(nullable: true),
                MAILER_SESSION = table.Column<uint>(type: "int(11) unsigned", nullable: true, defaultValueSql: "0"),
                NOTIFY_USERS = table.Column<string>(unicode: false, nullable: true),
                VIA_EMAIL = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                OWNERS_ONLY = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "0"),
                RESOLUTION_CHANGED = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "0"),
                SYSTEM_COMMENT = table.Column<byte>(type: "tinyint(1) unsigned", nullable: true, defaultValueSql: "0"),
                TICKET_DATA_CHANGE = table.Column<byte>(type: "tinyint(1) unsigned", nullable: true, defaultValueSql: "0")
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
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "INDEX_CLASS_ID",
            table: "ASSET",
            column: "ASSET_CLASS_ID");

        migrationBuilder.CreateIndex(
            name: "INDEX_ASSET_STATUS_ID",
            table: "ASSET",
            column: "ASSET_STATUS_ID");

        migrationBuilder.CreateIndex(
            name: "INDEX_NAME",
            table: "ASSET",
            column: "NAME");

        migrationBuilder.CreateIndex(
            name: "INDEX_OWNER_ID",
            table: "ASSET",
            column: "OWNER_ID");

        migrationBuilder.CreateIndex(
            name: "INDEX_ARCHIVE",
            table: "ASSET",
            columns: new[] { "ARCHIVE", "ARCHIVE_DATE" });

        migrationBuilder.CreateIndex(
            name: "INDEX_TYPE_DATA_ID",
            table: "ASSET",
            columns: new[] { "ASSET_TYPE_ID", "ASSET_DATA_ID" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "INDEX_TYPE_MAPPED_ID",
            table: "ASSET",
            columns: new[] { "ASSET_TYPE_ID", "MAPPED_ID" });

        migrationBuilder.CreateIndex(
            name: "INDEX_TYPE_NAME",
            table: "ASSET",
            columns: new[] { "ASSET_TYPE_ID", "NAME" });

        migrationBuilder.CreateIndex(
            name: "HD_QUEUE_IDX",
            table: "HD_PRIORITY",
            column: "HD_QUEUE_ID");

        migrationBuilder.CreateIndex(
            name: "HD_QUEUE_IDX",
            table: "HD_STATUS",
            column: "HD_QUEUE_ID");

        migrationBuilder.CreateIndex(
            name: "IX_HD_TICKET_ASSET_ID",
            table: "HD_TICKET",
            column: "ASSET_ID");

        migrationBuilder.CreateIndex(
            name: "HD_CATEGORY_IDX",
            table: "HD_TICKET",
            column: "HD_CATEGORY_ID");

        migrationBuilder.CreateIndex(
            name: "HD_IMPACT_IDX",
            table: "HD_TICKET",
            column: "HD_IMPACT_ID");

        migrationBuilder.CreateIndex(
            name: "HD_PRIORITY_IDX",
            table: "HD_TICKET",
            column: "HD_PRIORITY_ID");

        migrationBuilder.CreateIndex(
            name: "HD_QUEUE_IDX",
            table: "HD_TICKET",
            column: "HD_QUEUE_ID");

        migrationBuilder.CreateIndex(
            name: "HD_STATUS_IDX",
            table: "HD_TICKET",
            column: "HD_STATUS_ID");

        migrationBuilder.CreateIndex(
            name: "MACHINE_IDX",
            table: "HD_TICKET",
            column: "MACHINE_ID");

        migrationBuilder.CreateIndex(
            name: "PARENT",
            table: "HD_TICKET",
            column: "PARENT_ID");

        migrationBuilder.CreateIndex(
            name: "IX_HD_TICKET_SUBMITTER_ID",
            table: "HD_TICKET",
            column: "SUBMITTER_ID");

        migrationBuilder.CreateIndex(
            name: "OWNER_STATUS",
            table: "HD_TICKET",
            columns: new[] { "OWNER_ID", "HD_STATUS_ID" });

        migrationBuilder.CreateIndex(
            name: "HD_TICKET_IDX",
            table: "HD_TICKET_CHANGE",
            column: "HD_TICKET_ID");

        migrationBuilder.CreateIndex(
            name: "MAILED",
            table: "HD_TICKET_CHANGE",
            column: "MAILED");

        migrationBuilder.CreateIndex(
            name: "MAILER_SESSION",
            table: "HD_TICKET_CHANGE",
            column: "MAILER_SESSION");

        migrationBuilder.CreateIndex(
            name: "USER_IDX",
            table: "HD_TICKET_CHANGE",
            column: "USER_ID");

        migrationBuilder.CreateIndex(
            name: "MAILED_MAILER_SESSION_IDX",
            table: "HD_TICKET_CHANGE",
            columns: new[] { "MAILED", "MAILER_SESSION" });

        migrationBuilder.CreateIndex(
            name: "VIA_EMAIL_TIMESTAMP_IDX",
            table: "HD_TICKET_CHANGE",
            columns: new[] { "VIA_EMAIL", "TIMESTAMP" });

        migrationBuilder.CreateIndex(
            name: "IDX_LDAP_UID",
            table: "USER",
            column: "LDAP_UID");

        migrationBuilder.CreateIndex(
            name: "IDX_PARENT",
            table: "USER",
            column: "MANAGER_ID");

        migrationBuilder.CreateIndex(
            name: "IDX_PATH",
            table: "USER",
            column: "PATH");

        migrationBuilder.CreateIndex(
            name: "IDX_NAME",
            table: "USER",
            column: "USER_NAME");
    }

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