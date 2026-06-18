using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solution.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lan_tian_employee_role_relation");

            migrationBuilder.CreateTable(
                name: "T_EmployeeRoleRelation",
                columns: table => new
                {
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_EmployeeRoleRelation", x => new { x.EmployeeId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_T_EmployeeRoleRelation_T_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "T_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_EmployeeRoleRelation_T_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "T_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_EmployeeRoleRelation_RoleId",
                table: "T_EmployeeRoleRelation",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_EmployeeRoleRelation");

            migrationBuilder.CreateTable(
                name: "lan_tian_employee_role_relation",
                columns: table => new
                {
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lan_tian_employee_role_relation", x => new { x.employee_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_lan_tian_employee_role_relation_T_Employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "T_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lan_tian_employee_role_relation_T_Role_role_id",
                        column: x => x.role_id,
                        principalTable: "T_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lan_tian_employee_role_relation_role_id",
                table: "lan_tian_employee_role_relation",
                column: "role_id");
        }
    }
}
