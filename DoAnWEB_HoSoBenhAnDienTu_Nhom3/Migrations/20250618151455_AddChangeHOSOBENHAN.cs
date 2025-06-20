using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeHOSOBENHAN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECS6QdTNgMcM+jFkKXs31vrQWbQFCa+WOt1SnK05dMtvpQXLTzIHn697C9o0bJqcQg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMkBIxLqkGtHI/AalAiwbHKEJ7SvoFbdTIqw9WoBTtYUxmDsf7iZECbaJD7G+WD8oA==");
        }
    }
}
