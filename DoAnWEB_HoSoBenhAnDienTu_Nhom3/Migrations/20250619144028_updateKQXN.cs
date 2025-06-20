using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Migrations
{
    /// <inheritdoc />
    public partial class updateKQXN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAvp53dOj2uqtlT55AiK3tCgYao2V5k9JV9v1hpZXoTwZVdkqwWlV8+CeanlF3pR5g==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED+DmqwRPVNLlTWcV0AxBtMqXJq52l3XE73tIgPF8FYHssbjTMB4ORqM2UoWe9qRqg==");
        }
    }
}
