using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Open.ManifestToolkit.API.Migrations
{
    public partial class Delete_Settings_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Settings_SettingId",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_SceInstances_Settings_SettingId",
                table: "SceInstances");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_SceInstances_SettingId",
                table: "SceInstances");

            migrationBuilder.DropIndex(
                name: "IX_Environments_SettingId",
                table: "Environments");

            migrationBuilder.DropColumn(
                name: "SettingId",
                table: "SceInstances");

            migrationBuilder.DropColumn(
                name: "SettingId",
                table: "Environments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SettingId",
                table: "SceInstances",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SettingId",
                table: "Environments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SceInstances_SettingId",
                table: "SceInstances",
                column: "SettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_SettingId",
                table: "Environments",
                column: "SettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Settings_SettingId",
                table: "Environments",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SceInstances_Settings_SettingId",
                table: "SceInstances",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id");
        }
    }
}
