using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Open.ManifestToolkit.API.Migrations
{
    public partial class Alter_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environment_Settings_SettingId",
                table: "Environment");

            migrationBuilder.DropForeignKey(
                name: "FK_SceInstance_Settings_SettingId",
                table: "SceInstance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SceInstance",
                table: "SceInstance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Environment",
                table: "Environment");

            migrationBuilder.RenameTable(
                name: "SceInstance",
                newName: "SceInstances");

            migrationBuilder.RenameTable(
                name: "Environment",
                newName: "Environments");

            migrationBuilder.RenameIndex(
                name: "IX_SceInstance_SettingId",
                table: "SceInstances",
                newName: "IX_SceInstances_SettingId");

            migrationBuilder.RenameIndex(
                name: "IX_Environment_SettingId",
                table: "Environments",
                newName: "IX_Environments_SettingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SceInstances",
                table: "SceInstances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Environments",
                table: "Environments",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Settings_SettingId",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_SceInstances_Settings_SettingId",
                table: "SceInstances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SceInstances",
                table: "SceInstances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Environments",
                table: "Environments");

            migrationBuilder.RenameTable(
                name: "SceInstances",
                newName: "SceInstance");

            migrationBuilder.RenameTable(
                name: "Environments",
                newName: "Environment");

            migrationBuilder.RenameIndex(
                name: "IX_SceInstances_SettingId",
                table: "SceInstance",
                newName: "IX_SceInstance_SettingId");

            migrationBuilder.RenameIndex(
                name: "IX_Environments_SettingId",
                table: "Environment",
                newName: "IX_Environment_SettingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SceInstance",
                table: "SceInstance",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Environment",
                table: "Environment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Environment_Settings_SettingId",
                table: "Environment",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SceInstance_Settings_SettingId",
                table: "SceInstance",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
