using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VC_IMS.Migrations.Identity
{
    /// <inheritdoc />
    public partial class Identity_Init_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_push_subscriptions",
                schema: "notify",
                table: "push_subscriptions");

            migrationBuilder.RenameTable(
                name: "push_subscriptions",
                schema: "notify",
                newName: "VC_push_subscriptions",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_push_subscriptions_UserId",
                schema: "dbo",
                table: "VC_push_subscriptions",
                newName: "IX_VC_push_subscriptions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_push_subscriptions_Endpoint",
                schema: "dbo",
                table: "VC_push_subscriptions",
                newName: "IX_VC_push_subscriptions_Endpoint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VC_push_subscriptions",
                schema: "dbo",
                table: "VC_push_subscriptions",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VC_push_subscriptions",
                schema: "dbo",
                table: "VC_push_subscriptions");

            migrationBuilder.EnsureSchema(
                name: "notify");

            migrationBuilder.RenameTable(
                name: "VC_push_subscriptions",
                schema: "dbo",
                newName: "push_subscriptions",
                newSchema: "notify");

            migrationBuilder.RenameIndex(
                name: "IX_VC_push_subscriptions_UserId",
                schema: "notify",
                table: "push_subscriptions",
                newName: "IX_push_subscriptions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VC_push_subscriptions_Endpoint",
                schema: "notify",
                table: "push_subscriptions",
                newName: "IX_push_subscriptions_Endpoint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_push_subscriptions",
                schema: "notify",
                table: "push_subscriptions",
                column: "Id");
        }
    }
}
