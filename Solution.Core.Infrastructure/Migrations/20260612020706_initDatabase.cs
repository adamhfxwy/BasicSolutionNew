using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solution.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Department",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "id主键")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "部门名称"),
                    Remark = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "备注"),
                    LeaderId = table.Column<long>(type: "bigint", nullable: true, comment: "部门负责人id"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false, comment: "软删  1-未删除 2-已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Dictionary",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "id主键")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "键"),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "值"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "描述"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "类型  0-创建类型"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false, comment: "软删  1-未删除 2-已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Dictionary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Menu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "id主键")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "路由"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "菜单名称"),
                    ButtonName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "按钮名称"),
                    MenuName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "菜单名称"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true, comment: "父级id"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "描述"),
                    Component = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "组件"),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "图标"),
                    IsButton = table.Column<int>(type: "int", nullable: false, comment: "是否是按钮 1-否 2-是"),
                    MenuCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "菜单或按钮编码"),
                    Level = table.Column<int>(type: "int", nullable: false, comment: "层级"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false, comment: "软删  1-未删除 2-已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_MobilePermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "id主键")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "权限项名称"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true, comment: "父级id"),
                    Remark = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "备注"),
                    PermissionCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "权限项编码"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false, comment: "软删  1-未删除 2-已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MobilePermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_OperationLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "id主键")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<long>(type: "bigint", nullable: false, comment: "员工id"),
                    EmpName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "员工姓名"),
                    OperationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "操作项目的名称"),
                    ApiPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "接口地址"),
                    RequestMessage = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "请求参数"),
                    ResponseMessage = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "响应参数"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false, comment: "软删  1-未删除 2-已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OperationLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "id主键")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "角色（职位）名称"),
                    Remark = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "备注"),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "权限项"),
                    MobilePermissions = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "移动端权限项"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false, comment: "软删  1-未删除 2-已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_ShiftInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "id主键")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "班次名称"),
                    BeginTime = table.Column<TimeSpan>(type: "time", nullable: false, comment: "开始时间"),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false, comment: "结束时间"),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "备注"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false, comment: "软删  1-未删除 2-已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ShiftInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_UserInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "id主键")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "用户名称"),
                    Cellphone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "联系方式"),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "地址"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false, comment: "软删  1-未删除 2-已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UserInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_VersionApp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "id主键")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "app版本号"),
                    EditDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "更新时间"),
                    IsUpdate = table.Column<int>(type: "int", nullable: true, comment: "是否需要强制更新 0：不需要 1：需要"),
                    DataVersion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "数据库版本"),
                    Source = table.Column<int>(type: "int", nullable: true, comment: "类型 1:app"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false, comment: "软删  1-未删除 2-已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_VersionApp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Employee",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "id主键")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RealName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "真实姓名"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "用户名"),
                    Cellphone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "手机号"),
                    JobName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "岗位名称"),
                    ShiftId = table.Column<long>(type: "bigint", nullable: true, comment: "班次id"),
                    Sex = table.Column<int>(type: "int", nullable: false, comment: " 性别 1-男 2-女"),
                    Remark = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "备注"),
                    LoginPermissions = table.Column<int>(type: "int", nullable: false, comment: "登录权限 1-无权限 2-web权限  3-app权限 4-小程序权限  5-所有权限"),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "密码"),
                    PasswordSalt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "盐"),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true, comment: "部门id"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "员工状态 1-正常 2-离职"),
                    Age = table.Column<int>(type: "int", nullable: true, comment: "年龄"),
                    PhotoPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "照片地址"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    IsDeleted = table.Column<int>(type: "int", nullable: false, comment: "软删  1-未删除 2-已删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Employee_T_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "T_Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_Employee_T_ShiftInfo_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "T_ShiftInfo",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_T_Employee_DepartmentId",
                table: "T_Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Employee_ShiftId",
                table: "T_Employee",
                column: "ShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lan_tian_employee_role_relation");

            migrationBuilder.DropTable(
                name: "T_Dictionary");

            migrationBuilder.DropTable(
                name: "T_Menu");

            migrationBuilder.DropTable(
                name: "T_MobilePermissions");

            migrationBuilder.DropTable(
                name: "T_OperationLog");

            migrationBuilder.DropTable(
                name: "T_UserInfo");

            migrationBuilder.DropTable(
                name: "T_VersionApp");

            migrationBuilder.DropTable(
                name: "T_Employee");

            migrationBuilder.DropTable(
                name: "T_Role");

            migrationBuilder.DropTable(
                name: "T_Department");

            migrationBuilder.DropTable(
                name: "T_ShiftInfo");
        }
    }
}
