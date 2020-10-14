using Microsoft.EntityFrameworkCore.Migrations;

namespace writely.Migrations
{
    public partial class SpecifyJournalFKForEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccountActive",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccountActive",
                table: "AspNetUsers");
        }
    }
}
