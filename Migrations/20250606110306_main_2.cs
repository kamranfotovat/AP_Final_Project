using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolestanProject.Migrations
{
    /// <inheritdoc />
    public partial class main_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Roles_Roles_ROLEid",
                table: "User_Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Roles_Users_USERid",
                table: "User_Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Roles",
                table: "User_Roles");

            migrationBuilder.DropIndex(
                name: "IX_User_Roles_ROLEid",
                table: "User_Roles");

            migrationBuilder.DropIndex(
                name: "IX_User_Roles_USERid",
                table: "User_Roles");

            migrationBuilder.DropColumn(
                name: "ROLEid",
                table: "User_Roles");

            migrationBuilder.RenameColumn(
                name: "USERid",
                table: "User_Roles",
                newName: "id_worthless");

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                table: "User_Roles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "id_worthless",
                table: "User_Roles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Roles",
                table: "User_Roles",
                column: "id_worthless");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_role_id",
                table: "User_Roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_user_id",
                table: "User_Roles",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Roles_Roles_role_id",
                table: "User_Roles",
                column: "role_id",
                principalTable: "Roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Roles_Users_user_id",
                table: "User_Roles",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Roles_Roles_role_id",
                table: "User_Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Roles_Users_user_id",
                table: "User_Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Roles",
                table: "User_Roles");

            migrationBuilder.DropIndex(
                name: "IX_User_Roles_role_id",
                table: "User_Roles");

            migrationBuilder.DropIndex(
                name: "IX_User_Roles_user_id",
                table: "User_Roles");

            migrationBuilder.RenameColumn(
                name: "id_worthless",
                table: "User_Roles",
                newName: "USERid");

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                table: "User_Roles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "USERid",
                table: "User_Roles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ROLEid",
                table: "User_Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Roles",
                table: "User_Roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_ROLEid",
                table: "User_Roles",
                column: "ROLEid");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_USERid",
                table: "User_Roles",
                column: "USERid");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Roles_Roles_ROLEid",
                table: "User_Roles",
                column: "ROLEid",
                principalTable: "Roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Roles_Users_USERid",
                table: "User_Roles",
                column: "USERid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
