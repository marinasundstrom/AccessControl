using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppService.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessLists",
                columns: table => new
                {
                    AccessListId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLists", x => x.AccessListId);
                });

            migrationBuilder.CreateTable(
                name: "AccessZones",
                columns: table => new
                {
                    AccessZoneId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessZones", x => x.AccessZoneId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identitiets",
                columns: table => new
                {
                    IdentityId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsInactive = table.Column<bool>(nullable: false),
                    ValidFrom = table.Column<TimeSpan>(nullable: true),
                    ValidThru = table.Column<TimeSpan>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identitiets", x => x.IdentityId);
                });

            migrationBuilder.CreateTable(
                name: "AccessLogs",
                columns: table => new
                {
                    AccessLogId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AccessZoneId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLogs", x => x.AccessLogId);
                    table.ForeignKey(
                        name: "FK_AccessLogs_AccessZones_AccessZoneId",
                        column: x => x.AccessZoneId,
                        principalTable: "AccessZones",
                        principalColumn: "AccessZoneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccessPoints",
                columns: table => new
                {
                    AccessPointId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    AccessTime = table.Column<TimeSpan>(nullable: false),
                    AccessListId = table.Column<Guid>(nullable: true),
                    AccessZoneId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPoints", x => x.AccessPointId);
                    table.ForeignKey(
                        name: "FK_AccessPoints_AccessLists_AccessListId",
                        column: x => x.AccessListId,
                        principalTable: "AccessLists",
                        principalColumn: "AccessListId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessPoints_AccessZones_AccessZoneId",
                        column: x => x.AccessZoneId,
                        principalTable: "AccessZones",
                        principalColumn: "AccessZoneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    CredentialId = table.Column<Guid>(nullable: false),
                    IdentityId = table.Column<Guid>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    CardType = table.Column<int>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true),
                    Pin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.CredentialId);
                    table.ForeignKey(
                        name: "FK_Credentials_Identitiets_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identitiets",
                        principalColumn: "IdentityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentityAccessList",
                columns: table => new
                {
                    IdentityId = table.Column<Guid>(nullable: false),
                    AccessListId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityAccessList", x => new { x.IdentityId, x.AccessListId });
                    table.ForeignKey(
                        name: "FK_IdentityAccessList_AccessLists_AccessListId",
                        column: x => x.AccessListId,
                        principalTable: "AccessLists",
                        principalColumn: "AccessListId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentityAccessList_Identitiets_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identitiets",
                        principalColumn: "IdentityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessLogEntries",
                columns: table => new
                {
                    AccessLogEntryId = table.Column<Guid>(nullable: false),
                    AccessLogId = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    AccessPointId = table.Column<Guid>(nullable: true),
                    IdentityId = table.Column<Guid>(nullable: true),
                    Event = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLogEntries", x => x.AccessLogEntryId);
                    table.ForeignKey(
                        name: "FK_AccessLogEntries_AccessLogs_AccessLogId",
                        column: x => x.AccessLogId,
                        principalTable: "AccessLogs",
                        principalColumn: "AccessLogId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessLogEntries_AccessPoints_AccessPointId",
                        column: x => x.AccessPointId,
                        principalTable: "AccessPoints",
                        principalColumn: "AccessPointId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessLogEntries_Identitiets_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identitiets",
                        principalColumn: "IdentityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogEntries_AccessLogId",
                table: "AccessLogEntries",
                column: "AccessLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogEntries_AccessPointId",
                table: "AccessLogEntries",
                column: "AccessPointId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogEntries_IdentityId",
                table: "AccessLogEntries",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_AccessZoneId",
                table: "AccessLogs",
                column: "AccessZoneId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_AccessListId",
                table: "AccessPoints",
                column: "AccessListId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_AccessZoneId",
                table: "AccessPoints",
                column: "AccessZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_IdentityId",
                table: "Credentials",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAccessList_AccessListId",
                table: "IdentityAccessList",
                column: "AccessListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessLogEntries");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "IdentityAccessList");

            migrationBuilder.DropTable(
                name: "AccessLogs");

            migrationBuilder.DropTable(
                name: "AccessPoints");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Identitiets");

            migrationBuilder.DropTable(
                name: "AccessLists");

            migrationBuilder.DropTable(
                name: "AccessZones");
        }
    }
}
