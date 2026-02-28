using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ERDv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_takeover_requests_payments_PaymentId",
                table: "takeover_requests");

            migrationBuilder.DropIndex(
                name: "IX_takeover_requests_PaymentId",
                table: "takeover_requests");

            migrationBuilder.DropIndex(
                name: "IX_payments_ProviderRef",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "users");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "takeover_requests");

            migrationBuilder.DropColumn(
                name: "fee_amount",
                table: "takeover_requests");

            migrationBuilder.DropColumn(
                name: "fee_currency",
                table: "takeover_requests");

            migrationBuilder.AddColumn<short>(
                name: "RoleId",
                table: "users",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeePaid",
                table: "takeover_requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PaidAt",
                table: "takeover_requests",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PaidAt",
                table: "payments",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PointPurchaseId",
                table: "payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BuyerLastReadAt",
                table: "conversations",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ResponsibleLastReadAt",
                table: "conversations",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "broker_applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReviewedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsActivationPaid = table.Column<bool>(type: "bit", nullable: false),
                    ActivationPaidAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_broker_applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_broker_applications_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_boosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartsAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndsAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_boosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_listing_boosts_listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_listing_boosts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "moderation_reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    FlagsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuggestionsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Decision = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReviewedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReviewOutcome = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moderation_reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_moderation_reports_listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "point_ledger",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Delta = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RefType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsMonthlyBucket = table.Column<bool>(type: "bit", nullable: false),
                    BalanceMonthlyAfter = table.Column<int>(type: "int", nullable: false),
                    BalancePermanentAfter = table.Column<int>(type: "int", nullable: false),
                    Bucket = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MonthKey = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    TxType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_point_ledger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "point_packages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceCurrency = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_point_packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_points",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MonthlyPoints = table.Column<int>(type: "int", nullable: false),
                    MonthlyMonthKey = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PermanentPoints = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_points_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "point_purchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceCurrency = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_point_purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_point_purchases_payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_point_purchases_point_packages_PointPackageId",
                        column: x => x.PointPackageId,
                        principalTable: "point_packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_point_purchases_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<short>(type: "smallint", nullable: false),
                    PermissionId = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_role_permissions_permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permissions_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_takeover_requests_IsFeePaid",
                table: "takeover_requests",
                column: "IsFeePaid");

            migrationBuilder.CreateIndex(
                name: "IX_payments_PointPurchaseId",
                table: "payments",
                column: "PointPurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_Provider_ProviderRef",
                table: "payments",
                columns: new[] { "Provider", "ProviderRef" });

            migrationBuilder.CreateIndex(
                name: "IX_conversations_LastMessageAt",
                table: "conversations",
                column: "LastMessageAt");

            migrationBuilder.CreateIndex(
                name: "IX_broker_applications_IsDeleted",
                table: "broker_applications",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_broker_applications_Status",
                table: "broker_applications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_broker_applications_UserId",
                table: "broker_applications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_listing_boosts_IsDeleted",
                table: "listing_boosts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_listing_boosts_ListingId",
                table: "listing_boosts",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_listing_boosts_ListingId_StartsAt_EndsAt",
                table: "listing_boosts",
                columns: new[] { "ListingId", "StartsAt", "EndsAt" });

            migrationBuilder.CreateIndex(
                name: "IX_listing_boosts_UserId",
                table: "listing_boosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_moderation_reports_CreatedAt",
                table: "moderation_reports",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_moderation_reports_Decision",
                table: "moderation_reports",
                column: "Decision");

            migrationBuilder.CreateIndex(
                name: "IX_moderation_reports_IsDeleted",
                table: "moderation_reports",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_moderation_reports_ListingId",
                table: "moderation_reports",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_moderation_reports_ReviewedByAdminId",
                table: "moderation_reports",
                column: "ReviewedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_Code",
                table: "permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permissions_IsDeleted",
                table: "permissions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_point_ledger_Bucket_MonthKey",
                table: "point_ledger",
                columns: new[] { "Bucket", "MonthKey" });

            migrationBuilder.CreateIndex(
                name: "IX_point_ledger_IsDeleted",
                table: "point_ledger",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_point_ledger_TxType",
                table: "point_ledger",
                column: "TxType");

            migrationBuilder.CreateIndex(
                name: "IX_point_ledger_UserId",
                table: "point_ledger",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_point_ledger_UserId_CreatedAt",
                table: "point_ledger",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_point_packages_IsActive",
                table: "point_packages",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_point_packages_IsDeleted",
                table: "point_packages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_point_purchases_IsDeleted",
                table: "point_purchases",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_point_purchases_PaymentId",
                table: "point_purchases",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_point_purchases_PointPackageId",
                table: "point_purchases",
                column: "PointPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_point_purchases_Status",
                table: "point_purchases",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_point_purchases_UserId",
                table: "point_purchases",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_IsDeleted",
                table: "role_permissions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_PermissionId",
                table: "role_permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_RoleId",
                table: "role_permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_RoleId_PermissionId",
                table: "role_permissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_IsDeleted",
                table: "roles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_roles_Name",
                table: "roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_points_IsDeleted",
                table: "user_points",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_user_points_UserId",
                table: "user_points",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_users_roles_RoleId",
                table: "users",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_roles_RoleId",
                table: "users");

            migrationBuilder.DropTable(
                name: "broker_applications");

            migrationBuilder.DropTable(
                name: "listing_boosts");

            migrationBuilder.DropTable(
                name: "moderation_reports");

            migrationBuilder.DropTable(
                name: "point_ledger");

            migrationBuilder.DropTable(
                name: "point_purchases");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "user_points");

            migrationBuilder.DropTable(
                name: "point_packages");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropIndex(
                name: "IX_users_RoleId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_takeover_requests_IsFeePaid",
                table: "takeover_requests");

            migrationBuilder.DropIndex(
                name: "IX_payments_PointPurchaseId",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_Provider_ProviderRef",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_conversations_LastMessageAt",
                table: "conversations");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "IsFeePaid",
                table: "takeover_requests");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "takeover_requests");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "PointPurchaseId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "BuyerLastReadAt",
                table: "conversations");

            migrationBuilder.DropColumn(
                name: "ResponsibleLastReadAt",
                table: "conversations");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId",
                table: "takeover_requests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "fee_amount",
                table: "takeover_requests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "fee_currency",
                table: "takeover_requests",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_takeover_requests_PaymentId",
                table: "takeover_requests",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_ProviderRef",
                table: "payments",
                column: "ProviderRef");

            migrationBuilder.AddForeignKey(
                name: "FK_takeover_requests_payments_PaymentId",
                table: "takeover_requests",
                column: "PaymentId",
                principalTable: "payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
