using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Migrations
{
    /// <inheritdoc />
    public partial class removeRQChiDinh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFrZf9QfMGDznDKzU8HBLwg5+VLVV2qLSJSI0ag2+IoDFKbA6abMsd1svI767uxBvQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFVI7cyBzjrpPzk4Z+L+gY1F0yz5N8thSGOkf5YEr6n7XcZTLOceDhUTyWTCfMI9Bg==");
        }
    }
}
