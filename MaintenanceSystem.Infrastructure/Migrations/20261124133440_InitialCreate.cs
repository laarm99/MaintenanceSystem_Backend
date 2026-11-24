using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaintenanceSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MNTSYS_AUD_AUDIT_LOG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ENTITY_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ENTITY_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ACTION = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CHANGES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USER_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ACTION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_AUD_AUDIT_LOG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_CAT_EQUIPMENT_TYPE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DEFAULT_FREQUENCY = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_CAT_EQUIPMENT_TYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_CAT_SITE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TIME_ZONE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_CAT_SITE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_IDM_ACCOUNT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOMAIN_USER_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FULL_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_IDM_ACCOUNT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_IDM_ROLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_IDM_ROLE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_CAT_PROCEDURE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CODE = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EQUIPMENT_TYPE_ID = table.Column<int>(type: "int", nullable: true),
                    ESTIMATED_DURATION_MINUTES = table.Column<int>(type: "int", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_CAT_PROCEDURE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_CAT_PROCEDURE_MNTSYS_CAT_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                        column: x => x.EQUIPMENT_TYPE_ID,
                        principalTable: "MNTSYS_CAT_EQUIPMENT_TYPE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_CAT_PLANT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SITE_ID = table.Column<int>(type: "int", nullable: false),
                    CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_CAT_PLANT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_CAT_PLANT_MNTSYS_CAT_SITE_SITE_ID",
                        column: x => x.SITE_ID,
                        principalTable: "MNTSYS_CAT_SITE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_MNT_PLAN",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SITE_ID = table.Column<int>(type: "int", nullable: false),
                    YEAR = table.Column<int>(type: "int", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PUBLISHED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PUBLISHED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NOTES = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_MNT_PLAN", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_PLAN_MNTSYS_CAT_SITE_SITE_ID",
                        column: x => x.SITE_ID,
                        principalTable: "MNTSYS_CAT_SITE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_IDM_ACCOUNT_ROLE",
                columns: table => new
                {
                    ACCOUNT_ID = table.Column<int>(type: "int", nullable: false),
                    ROLE_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_IDM_ACCOUNT_ROLE", x => new { x.ACCOUNT_ID, x.ROLE_ID });
                    table.ForeignKey(
                        name: "FK_MNTSYS_IDM_ACCOUNT_ROLE_MNTSYS_IDM_ACCOUNT_ACCOUNT_ID",
                        column: x => x.ACCOUNT_ID,
                        principalTable: "MNTSYS_IDM_ACCOUNT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MNTSYS_IDM_ACCOUNT_ROLE_MNTSYS_IDM_ROLE_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "MNTSYS_IDM_ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_CAT_PROCEDURE_CHECKLIST_ITEM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROCEDURE_ID = table.Column<int>(type: "int", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false),
                    IS_REQUIRED = table.Column<bool>(type: "bit", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_CAT_PROCEDURE_CHECKLIST_ITEM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_CAT_PROCEDURE_CHECKLIST_ITEM_MNTSYS_CAT_PROCEDURE_PROCEDURE_ID",
                        column: x => x.PROCEDURE_ID,
                        principalTable: "MNTSYS_CAT_PROCEDURE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_CAT_AREA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PLANT_ID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_CAT_AREA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_CAT_AREA_MNTSYS_CAT_PLANT_PLANT_ID",
                        column: x => x.PLANT_ID,
                        principalTable: "MNTSYS_CAT_PLANT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_CAT_NON_WORKING_DAY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SITE_ID = table.Column<int>(type: "int", nullable: false),
                    PLANT_ID = table.Column<int>(type: "int", nullable: true),
                    DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_CAT_NON_WORKING_DAY", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_CAT_NON_WORKING_DAY_MNTSYS_CAT_PLANT_PLANT_ID",
                        column: x => x.PLANT_ID,
                        principalTable: "MNTSYS_CAT_PLANT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MNTSYS_CAT_NON_WORKING_DAY_MNTSYS_CAT_SITE_SITE_ID",
                        column: x => x.SITE_ID,
                        principalTable: "MNTSYS_CAT_SITE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_MNT_EQUIPMENT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TAG = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EQUIPMENT_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    BRAND = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MODEL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SERIAL_NUMBER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PLANT_ID = table.Column<int>(type: "int", nullable: false),
                    AREA_ID = table.Column<int>(type: "int", nullable: true),
                    LOCATION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RESPONSIBLE_ACCOUNT_ID = table.Column<int>(type: "int", nullable: true),
                    PROCEDURE_ID = table.Column<int>(type: "int", nullable: true),
                    FREQUENCY = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CRITICALITY = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HAS_BACKUP = table.Column<bool>(type: "bit", nullable: false),
                    NOTES = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_MNT_EQUIPMENT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_EQUIPMENT_MNTSYS_CAT_AREA_AREA_ID",
                        column: x => x.AREA_ID,
                        principalTable: "MNTSYS_CAT_AREA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_EQUIPMENT_MNTSYS_CAT_EQUIPMENT_TYPE_EQUIPMENT_TYPE_ID",
                        column: x => x.EQUIPMENT_TYPE_ID,
                        principalTable: "MNTSYS_CAT_EQUIPMENT_TYPE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_EQUIPMENT_MNTSYS_CAT_PLANT_PLANT_ID",
                        column: x => x.PLANT_ID,
                        principalTable: "MNTSYS_CAT_PLANT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_EQUIPMENT_MNTSYS_CAT_PROCEDURE_PROCEDURE_ID",
                        column: x => x.PROCEDURE_ID,
                        principalTable: "MNTSYS_CAT_PROCEDURE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_EQUIPMENT_MNTSYS_IDM_ACCOUNT_RESPONSIBLE_ACCOUNT_ID",
                        column: x => x.RESPONSIBLE_ACCOUNT_ID,
                        principalTable: "MNTSYS_IDM_ACCOUNT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_MNT_ACTIVITY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAINTENANCE_PLAN_ID = table.Column<int>(type: "int", nullable: true),
                    EQUIPMENT_ID = table.Column<int>(type: "int", nullable: false),
                    TECHNICIAN_ACCOUNT_ID = table.Column<int>(type: "int", nullable: true),
                    ORIGINAL_SCHEDULED_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    SCHEDULED_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    STARTED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    COMPLETED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IS_UNPLANNED = table.Column<bool>(type: "bit", nullable: false),
                    TICKET_NUMBER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NOTES = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NOT_PERFORMED_REASON = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_MNT_ACTIVITY", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_ACTIVITY_MNTSYS_IDM_ACCOUNT_TECHNICIAN_ACCOUNT_ID",
                        column: x => x.TECHNICIAN_ACCOUNT_ID,
                        principalTable: "MNTSYS_IDM_ACCOUNT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_ACTIVITY_MNTSYS_MNT_EQUIPMENT_EQUIPMENT_ID",
                        column: x => x.EQUIPMENT_ID,
                        principalTable: "MNTSYS_MNT_EQUIPMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_ACTIVITY_MNTSYS_MNT_PLAN_MAINTENANCE_PLAN_ID",
                        column: x => x.MAINTENANCE_PLAN_ID,
                        principalTable: "MNTSYS_MNT_PLAN",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_MNT_CHECKLIST_RESULT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAINTENANCE_ACTIVITY_ID = table.Column<int>(type: "int", nullable: false),
                    PROCEDURE_CHECKLIST_ITEM_ID = table.Column<int>(type: "int", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IS_DONE = table.Column<bool>(type: "bit", nullable: false),
                    COMMENTS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_MNT_CHECKLIST_RESULT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_CHECKLIST_RESULT_MNTSYS_CAT_PROCEDURE_CHECKLIST_ITEM_PROCEDURE_CHECKLIST_ITEM_ID",
                        column: x => x.PROCEDURE_CHECKLIST_ITEM_ID,
                        principalTable: "MNTSYS_CAT_PROCEDURE_CHECKLIST_ITEM",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_CHECKLIST_RESULT_MNTSYS_MNT_ACTIVITY_MAINTENANCE_ACTIVITY_ID",
                        column: x => x.MAINTENANCE_ACTIVITY_ID,
                        principalTable: "MNTSYS_MNT_ACTIVITY",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_MNT_EVIDENCE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAINTENANCE_ACTIVITY_ID = table.Column<int>(type: "int", nullable: false),
                    FILE_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STORED_FILE_PATH = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CONTENT_TYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FILE_SIZE_BYTES = table.Column<long>(type: "bigint", nullable: false),
                    UPLOADED_BY_ACCOUNT_ID = table.Column<int>(type: "int", nullable: false),
                    UPLOADED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_MNT_EVIDENCE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_EVIDENCE_MNTSYS_IDM_ACCOUNT_UPLOADED_BY_ACCOUNT_ID",
                        column: x => x.UPLOADED_BY_ACCOUNT_ID,
                        principalTable: "MNTSYS_IDM_ACCOUNT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_EVIDENCE_MNTSYS_MNT_ACTIVITY_MAINTENANCE_ACTIVITY_ID",
                        column: x => x.MAINTENANCE_ACTIVITY_ID,
                        principalTable: "MNTSYS_MNT_ACTIVITY",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MNTSYS_MNT_RESCHEDULE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAINTENANCE_ACTIVITY_ID = table.Column<int>(type: "int", nullable: false),
                    PREVIOUS_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    NEW_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    REASON = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CHANGED_BY_ACCOUNT_ID = table.Column<int>(type: "int", nullable: false),
                    CHANGED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MNTSYS_MNT_RESCHEDULE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_RESCHEDULE_MNTSYS_IDM_ACCOUNT_CHANGED_BY_ACCOUNT_ID",
                        column: x => x.CHANGED_BY_ACCOUNT_ID,
                        principalTable: "MNTSYS_IDM_ACCOUNT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MNTSYS_MNT_RESCHEDULE_MNTSYS_MNT_ACTIVITY_MAINTENANCE_ACTIVITY_ID",
                        column: x => x.MAINTENANCE_ACTIVITY_ID,
                        principalTable: "MNTSYS_MNT_ACTIVITY",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_AUD_AUDIT_LOG_ACTION_DATE",
                table: "MNTSYS_AUD_AUDIT_LOG",
                column: "ACTION_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_AUD_AUDIT_LOG_ENTITY_NAME_ENTITY_ID",
                table: "MNTSYS_AUD_AUDIT_LOG",
                columns: new[] { "ENTITY_NAME", "ENTITY_ID" });

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_CAT_AREA_PLANT_ID_NAME",
                table: "MNTSYS_CAT_AREA",
                columns: new[] { "PLANT_ID", "NAME" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_CAT_EQUIPMENT_TYPE_CODE",
                table: "MNTSYS_CAT_EQUIPMENT_TYPE",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_CAT_NON_WORKING_DAY_PLANT_ID",
                table: "MNTSYS_CAT_NON_WORKING_DAY",
                column: "PLANT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_CAT_NON_WORKING_DAY_SITE_ID_PLANT_ID_DATE",
                table: "MNTSYS_CAT_NON_WORKING_DAY",
                columns: new[] { "SITE_ID", "PLANT_ID", "DATE" },
                unique: true,
                filter: "[PLANT_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_CAT_PLANT_SITE_ID_CODE",
                table: "MNTSYS_CAT_PLANT",
                columns: new[] { "SITE_ID", "CODE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_CAT_PROCEDURE_CODE",
                table: "MNTSYS_CAT_PROCEDURE",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_CAT_PROCEDURE_EQUIPMENT_TYPE_ID",
                table: "MNTSYS_CAT_PROCEDURE",
                column: "EQUIPMENT_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_CAT_PROCEDURE_CHECKLIST_ITEM_PROCEDURE_ID",
                table: "MNTSYS_CAT_PROCEDURE_CHECKLIST_ITEM",
                column: "PROCEDURE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_CAT_SITE_CODE",
                table: "MNTSYS_CAT_SITE",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_IDM_ACCOUNT_DOMAIN_USER_NAME",
                table: "MNTSYS_IDM_ACCOUNT",
                column: "DOMAIN_USER_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_IDM_ACCOUNT_ROLE_ROLE_ID",
                table: "MNTSYS_IDM_ACCOUNT_ROLE",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_IDM_ROLE_ROLE_NAME",
                table: "MNTSYS_IDM_ROLE",
                column: "ROLE_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_ACTIVITY_EQUIPMENT_ID_SCHEDULED_DATE",
                table: "MNTSYS_MNT_ACTIVITY",
                columns: new[] { "EQUIPMENT_ID", "SCHEDULED_DATE" });

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_ACTIVITY_MAINTENANCE_PLAN_ID",
                table: "MNTSYS_MNT_ACTIVITY",
                column: "MAINTENANCE_PLAN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_ACTIVITY_SCHEDULED_DATE",
                table: "MNTSYS_MNT_ACTIVITY",
                column: "SCHEDULED_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_ACTIVITY_STATUS",
                table: "MNTSYS_MNT_ACTIVITY",
                column: "STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_ACTIVITY_TECHNICIAN_ACCOUNT_ID",
                table: "MNTSYS_MNT_ACTIVITY",
                column: "TECHNICIAN_ACCOUNT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_CHECKLIST_RESULT_MAINTENANCE_ACTIVITY_ID",
                table: "MNTSYS_MNT_CHECKLIST_RESULT",
                column: "MAINTENANCE_ACTIVITY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_CHECKLIST_RESULT_PROCEDURE_CHECKLIST_ITEM_ID",
                table: "MNTSYS_MNT_CHECKLIST_RESULT",
                column: "PROCEDURE_CHECKLIST_ITEM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_EQUIPMENT_AREA_ID",
                table: "MNTSYS_MNT_EQUIPMENT",
                column: "AREA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_EQUIPMENT_EQUIPMENT_TYPE_ID",
                table: "MNTSYS_MNT_EQUIPMENT",
                column: "EQUIPMENT_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_EQUIPMENT_PLANT_ID",
                table: "MNTSYS_MNT_EQUIPMENT",
                column: "PLANT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_EQUIPMENT_PROCEDURE_ID",
                table: "MNTSYS_MNT_EQUIPMENT",
                column: "PROCEDURE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_EQUIPMENT_RESPONSIBLE_ACCOUNT_ID",
                table: "MNTSYS_MNT_EQUIPMENT",
                column: "RESPONSIBLE_ACCOUNT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_EQUIPMENT_SERIAL_NUMBER",
                table: "MNTSYS_MNT_EQUIPMENT",
                column: "SERIAL_NUMBER");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_EQUIPMENT_TAG",
                table: "MNTSYS_MNT_EQUIPMENT",
                column: "TAG",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_EVIDENCE_MAINTENANCE_ACTIVITY_ID",
                table: "MNTSYS_MNT_EVIDENCE",
                column: "MAINTENANCE_ACTIVITY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_EVIDENCE_UPLOADED_BY_ACCOUNT_ID",
                table: "MNTSYS_MNT_EVIDENCE",
                column: "UPLOADED_BY_ACCOUNT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_PLAN_SITE_ID_YEAR",
                table: "MNTSYS_MNT_PLAN",
                columns: new[] { "SITE_ID", "YEAR" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_RESCHEDULE_CHANGED_BY_ACCOUNT_ID",
                table: "MNTSYS_MNT_RESCHEDULE",
                column: "CHANGED_BY_ACCOUNT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MNTSYS_MNT_RESCHEDULE_MAINTENANCE_ACTIVITY_ID",
                table: "MNTSYS_MNT_RESCHEDULE",
                column: "MAINTENANCE_ACTIVITY_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MNTSYS_AUD_AUDIT_LOG");

            migrationBuilder.DropTable(
                name: "MNTSYS_CAT_NON_WORKING_DAY");

            migrationBuilder.DropTable(
                name: "MNTSYS_IDM_ACCOUNT_ROLE");

            migrationBuilder.DropTable(
                name: "MNTSYS_MNT_CHECKLIST_RESULT");

            migrationBuilder.DropTable(
                name: "MNTSYS_MNT_EVIDENCE");

            migrationBuilder.DropTable(
                name: "MNTSYS_MNT_RESCHEDULE");

            migrationBuilder.DropTable(
                name: "MNTSYS_IDM_ROLE");

            migrationBuilder.DropTable(
                name: "MNTSYS_CAT_PROCEDURE_CHECKLIST_ITEM");

            migrationBuilder.DropTable(
                name: "MNTSYS_MNT_ACTIVITY");

            migrationBuilder.DropTable(
                name: "MNTSYS_MNT_EQUIPMENT");

            migrationBuilder.DropTable(
                name: "MNTSYS_MNT_PLAN");

            migrationBuilder.DropTable(
                name: "MNTSYS_CAT_AREA");

            migrationBuilder.DropTable(
                name: "MNTSYS_CAT_PROCEDURE");

            migrationBuilder.DropTable(
                name: "MNTSYS_IDM_ACCOUNT");

            migrationBuilder.DropTable(
                name: "MNTSYS_CAT_PLANT");

            migrationBuilder.DropTable(
                name: "MNTSYS_CAT_EQUIPMENT_TYPE");

            migrationBuilder.DropTable(
                name: "MNTSYS_CAT_SITE");
        }
    }
}
