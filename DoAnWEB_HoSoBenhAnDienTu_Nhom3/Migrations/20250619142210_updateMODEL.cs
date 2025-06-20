using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Migrations
{
    /// <inheritdoc />
    public partial class updateMODEL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED+DmqwRPVNLlTWcV0AxBtMqXJq52l3XE73tIgPF8FYHssbjTMB4ORqM2UoWe9qRqg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFrZf9QfMGDznDKzU8HBLwg5+VLVV2qLSJSI0ag2+IoDFKbA6abMsd1svI767uxBvQ==");
        }
    }
}
