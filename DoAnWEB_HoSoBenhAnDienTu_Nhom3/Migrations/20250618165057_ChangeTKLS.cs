using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTKLS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFVI7cyBzjrpPzk4Z+L+gY1F0yz5N8thSGOkf5YEr6n7XcZTLOceDhUTyWTCfMI9Bg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECS6QdTNgMcM+jFkKXs31vrQWbQFCa+WOt1SnK05dMtvpQXLTzIHn697C9o0bJqcQg==");
        }
    }
}
