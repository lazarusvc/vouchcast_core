using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VC_IMS.Migrations.Identity
{
    /// <inheritdoc />
    public partial class Identity_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notify");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "push_subscriptions",
                schema: "notify",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    P256dh = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Auth = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSeenUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_push_subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_audit_logs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Entity = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    OldValuesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValuesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ip = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ExtraJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_audit_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_conversations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UserAId = table.Column<int>(type: "int", nullable: true),
                    UserBId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_conversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_email_deadletter",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    To = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    BodyText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadersJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FailedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_email_deadletter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_email_outbox",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    To = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Cc = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Bcc = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    BodyText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadersJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SentUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextAttemptUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastError = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_email_outbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_notification_prefs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    InAppEnabled = table.Column<bool>(type: "bit", nullable: false),
                    EmailEnabled = table.Column<bool>(type: "bit", nullable: false),
                    DigestEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_notification_prefs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_notifications",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seen = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_policies",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_public_endpoints",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Page = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Regex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 100),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_public_endpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_roles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_session_logs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LoginUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSeenUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoutUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ip = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_session_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VC_conversation_members",
                schema: "dbo",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JoinedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastReadMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastReadUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_conversation_members", x => new { x.ConversationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_VC_conversation_members_VC_conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalSchema: "dbo",
                        principalTable: "VC_conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VC_messages",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderUserId = table.Column<int>(type: "int", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VC_messages_VC_conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalSchema: "dbo",
                        principalTable: "VC_conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VC_endpoint_policy_assignments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Page = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Regex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    PolicyName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 100),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_endpoint_policy_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VC_endpoint_policy_assignments_VC_policies_PolicyId",
                        column: x => x.PolicyId,
                        principalSchema: "dbo",
                        principalTable: "VC_policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VC_policy_claims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorizationPolicyEntityId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_policy_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VC_policy_claims_VC_policies_AuthorizationPolicyEntityId",
                        column: x => x.AuthorizationPolicyEntityId,
                        principalSchema: "dbo",
                        principalTable: "VC_policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VC_policy_roles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorizationPolicyEntityId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_policy_roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VC_policy_roles_VC_policies_AuthorizationPolicyEntityId",
                        column: x => x.AuthorizationPolicyEntityId,
                        principalSchema: "dbo",
                        principalTable: "VC_policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VC_policy_roles_VC_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "VC_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VC_role_claims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_role_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VC_role_claims_VC_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "VC_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VC_user_claims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_user_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VC_user_claims_VC_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "VC_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VC_user_logins",
                schema: "dbo",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_user_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_VC_user_logins_VC_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "VC_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VC_user_roles",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_user_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_VC_user_roles_VC_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "VC_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VC_user_roles_VC_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "VC_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VC_user_tokens",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VC_user_tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_VC_user_tokens_VC_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "VC_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_push_subscriptions_Endpoint",
                schema: "notify",
                table: "push_subscriptions",
                column: "Endpoint",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_push_subscriptions_UserId",
                schema: "notify",
                table: "push_subscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VC_audit_logs_Entity_EntityId_Utc",
                schema: "dbo",
                table: "VC_audit_logs",
                columns: new[] { "Entity", "EntityId", "Utc" });

            migrationBuilder.CreateIndex(
                name: "IX_VC_audit_logs_UserId_Utc",
                schema: "dbo",
                table: "VC_audit_logs",
                columns: new[] { "UserId", "Utc" });

            migrationBuilder.CreateIndex(
                name: "IX_VC_conversation_members_UserId_ConversationId",
                schema: "dbo",
                table: "VC_conversation_members",
                columns: new[] { "UserId", "ConversationId" });

            migrationBuilder.CreateIndex(
                name: "IX_VC_conversations_Type_UserAId_UserBId",
                schema: "dbo",
                table: "VC_conversations",
                columns: new[] { "Type", "UserAId", "UserBId" },
                unique: true,
                filter: "([Type] = 1)");

            migrationBuilder.CreateIndex(
                name: "IX_VC_email_outbox_CreatedUtc",
                schema: "dbo",
                table: "VC_email_outbox",
                column: "CreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_VC_email_outbox_SentUtc_NextAttemptUtc",
                schema: "dbo",
                table: "VC_email_outbox",
                columns: new[] { "SentUtc", "NextAttemptUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_VC_endpoint_policy_assignments_MatchType_Area_Controller_Action_Page_Path_Regex_PolicyId_IsEnabled",
                schema: "dbo",
                table: "VC_endpoint_policy_assignments",
                columns: new[] { "MatchType", "Area", "Controller", "Action", "Page", "Path", "Regex", "PolicyId", "IsEnabled" });

            migrationBuilder.CreateIndex(
                name: "IX_VC_endpoint_policy_assignments_PolicyId",
                schema: "dbo",
                table: "VC_endpoint_policy_assignments",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_VC_messages_ConversationId_CreatedUtc",
                schema: "dbo",
                table: "VC_messages",
                columns: new[] { "ConversationId", "CreatedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_VC_notification_prefs_UserId_Type",
                schema: "dbo",
                table: "VC_notification_prefs",
                columns: new[] { "UserId", "Type" },
                unique: true,
                filter: "[Type] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VC_notifications_CreatedUtc",
                schema: "dbo",
                table: "VC_notifications",
                column: "CreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_VC_notifications_UserId_Seen_CreatedUtc",
                schema: "dbo",
                table: "VC_notifications",
                columns: new[] { "UserId", "Seen", "CreatedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_VC_policies_Name",
                schema: "dbo",
                table: "VC_policies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VC_policy_claims_AuthorizationPolicyEntityId",
                schema: "dbo",
                table: "VC_policy_claims",
                column: "AuthorizationPolicyEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_VC_policy_roles_AuthorizationPolicyEntityId_RoleId",
                schema: "dbo",
                table: "VC_policy_roles",
                columns: new[] { "AuthorizationPolicyEntityId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VC_policy_roles_RoleId",
                schema: "dbo",
                table: "VC_policy_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_VC_public_endpoints_MatchType_Area_Controller_Action_Page_Path_Regex_IsEnabled",
                schema: "dbo",
                table: "VC_public_endpoints",
                columns: new[] { "MatchType", "Area", "Controller", "Action", "Page", "Path", "Regex", "IsEnabled" });

            migrationBuilder.CreateIndex(
                name: "IX_VC_role_claims_RoleId",
                schema: "dbo",
                table: "VC_role_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "VC_roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VC_session_logs_LastSeenUtc",
                schema: "dbo",
                table: "VC_session_logs",
                column: "LastSeenUtc");

            migrationBuilder.CreateIndex(
                name: "IX_VC_session_logs_UserId_LoginUtc",
                schema: "dbo",
                table: "VC_session_logs",
                columns: new[] { "UserId", "LoginUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_VC_session_logs_UserId_SessionId",
                schema: "dbo",
                table: "VC_session_logs",
                columns: new[] { "UserId", "SessionId" });

            migrationBuilder.CreateIndex(
                name: "IX_VC_user_claims_UserId",
                schema: "dbo",
                table: "VC_user_claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VC_user_logins_UserId",
                schema: "dbo",
                table: "VC_user_logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VC_user_roles_RoleId",
                schema: "dbo",
                table: "VC_user_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "VC_users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "VC_users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "push_subscriptions",
                schema: "notify");

            migrationBuilder.DropTable(
                name: "VC_audit_logs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_conversation_members",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_email_deadletter",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_email_outbox",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_endpoint_policy_assignments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_messages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_notification_prefs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_notifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_policy_claims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_policy_roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_public_endpoints",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_role_claims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_session_logs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_user_claims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_user_logins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_user_roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_user_tokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_conversations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_policies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VC_users",
                schema: "dbo");
        }
    }
}
