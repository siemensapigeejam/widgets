using Microsoft.EntityFrameworkCore.Migrations;

namespace DESPortal.Widgets.Infrastructure.Migrations
{
    public partial class AddUserRoleToDashboardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "Dashboards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Dashboards");
        }
    }
}
