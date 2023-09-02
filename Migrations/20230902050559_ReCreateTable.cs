using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Open.ManifestToolkit.API.Migrations
{
    public partial class ReCreateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Settings_SettingId",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_SceInstances_Settings_SettingId",
                table: "SceInstances");

            migrationBuilder.AlterColumn<string>(
                name: "SettingId",
                table: "SceInstances",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "SettingId",
                table: "Environments",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Settings_SettingId",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_SceInstances_Settings_SettingId",
                table: "SceInstances");

            migrationBuilder.AlterColumn<string>(
                name: "SettingId",
                table: "SceInstances",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SettingId",
                table: "Environments",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Settings_SettingId",
                table: "Environments",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SceInstances_Settings_SettingId",
                table: "SceInstances",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
