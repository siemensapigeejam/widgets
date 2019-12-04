using Microsoft.EntityFrameworkCore.Migrations;

namespace DESPortal.Widgets.Infrastructure.Migrations
{
    public partial class AddedPropertyToDefineDefaultDashboard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultDashboard",
                table: "Dashboards",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefaultDashboard",
                table: "Dashboards");
        }
    }
}
