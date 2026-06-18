using AutoFixture.Xunit2;
using FakeItEasy;
using FluentAssertions;
using Humanizer;
using LanTian.Solution.Core.Domain.ICommonService;
using LanTian.Solution.Core.Domain.INpgSqlService;
using LanTian.Solution.Core.Domain.NpgSqlEntities.Identity;
using LanTian.Solution.Core.DTO.Identity;
using LanTian.Solution.Core.EnumAndConstent;
using LanTian.Solution.Core.Infrastructure.NpgSqlService;
using LanTian.Solution.Core.Infrastructure.Utils;
using LanTian.Solution.Core.ParameterModel.ChangeModel.Identity;
using LanTian.Solution.Core.ParameterModel.QueryModel.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.CodeCoverage;
using Newtonsoft.Json.Linq;
using WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Testing.Shared;
using static LanTian.Solution.Core.EnumAndConstent.Enums.LanTianEnum;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LanTian.WisdomParkTests.Service.TestEmployeeService
{
    public class TestEmployeeShould
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="sut"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task AddEmployeeAsync_ReturnsSuccess_WhenValidModel(
            [Frozen] IRepository<LanTianEmployee> repository,
            EmployeeService sut,
             AddEditEmployeeModel model,
            CancellationToken cancellationToken
            )
        {
            // Arrange
            A.CallTo(() => repository.FindAsync(x => x.Cellphone == model.Cellphone, CancellationToken.None,null))
            .Returns(Task.FromResult<LanTianEmployee?>(null));
            //Act
            var tuple = await sut.AddEmployeeAsync(model, cancellationToken);
            //Assert
            tuple.Item2.Should().BeSameAs("success");
            A.CallTo(() => repository.InsertAsync(A<LanTianEmployee>.That.Matches(u => u.Cellphone == model.Cellphone && u.RealName==model.RealName
            && u.LoginPermissions == model.LoginPermissions && u.Sex==model.Sex),true, cancellationToken))
                .MustHaveHappenedOnceExactly();

        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="sut"></param>
        /// <param name="existingEmp"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task EditEmployeeAsync_ReturnsSuccess_WhenSuccess(
        [Frozen] IRepository<LanTianEmployee> repository,
        EmployeeService sut,
        LanTianEmployee existingEmp,
        AddEditEmployeeModel model,
        CancellationToken cancellationToken)
        {
            // Arrange
            A.CallTo(() => repository.FindAsync(model.Id.Value, null))
            .Returns(Task.FromResult(existingEmp));
            //Act
            var tuple = await sut.EditEmployeeAsync(model, cancellationToken);
            //Assert
            tuple.Item2.Should().BeSameAs("success");
            A.CallTo(() => repository.UpdateAsync(A<LanTianEmployee>.That.Matches(u => u.Cellphone == model.Cellphone && u.RealName == model.RealName
            && u.LoginPermissions == model.LoginPermissions && u.Sex == model.Sex), true, cancellationToken))
                .MustHaveHappenedOnceExactly();

        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="sut"></param>
        /// <param name="existingEmp"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task GetEmployeeByIdAsync_ReturnsOk_WhenSuccess(
              [Frozen] IRepository<LanTianEmployee> repository,
              EmployeeService sut,
              LanTianEmployee existingEmp,
              long id,
              CancellationToken cancellationToken)
        {
            //Arrange
            A.CallTo(() => repository.FindAsync(id, null))
                .Returns(existingEmp);
            //Act
            var emp = await sut.GetEmployeeByIdAsync(id, cancellationToken);
            var empDTO = ToDTOUtils.ToDTO(existingEmp);
            // Assert
            emp.Should().BeEquivalentTo(empDTO);
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="sut"></param>
        /// <param name="queryModel"></param>
        /// <param name="employees"></param>
        /// <returns></returns>
        //[Theory]
        //[AutoFakeItEasy]
        //public async Task GetEmployeeAsync_WhenCalled_ShouldReturnExpectedResult(
        //    [Frozen] IRepository<LanTianEmployee> repository,
        //    EmployeeService sut,
        //    EmployeeQueryModel queryModel,
        //    Pagination<LanTianEmployee> employees
        //        )
        //{
        //    // Arrange
        //    // 模拟仓储层返回的所有员工数据

        //    // 模拟仓储层方法返回分页数据
        //    employees = new Pagination<LanTianEmployee>
        //    {
        //        List = new List<LanTianEmployee>() 
        //        {

        //            new LanTianEmployee
        //            (
        //            realName: "Test Employee",
        //            cellphone: "12345678901",
        //            roleId: 1,
        //            sex: SexEnum.男,
        //            remark: "Test Remark",
        //            loginPermissions: LoginPermissionsEnum.所有权限,
        //            departmentId: 1,
        //            age: 30,
        //            photoPath: "/images/photo1.jpg",
        //            shiftId: 1
        //                ),
        //            new LanTianEmployee
        //            (
        //            realName: "Test Employee1",
        //            cellphone: "12345678922",
        //            roleId: 2,
        //            sex: SexEnum.女,
        //            remark: "Another Remark",
        //            loginPermissions: LoginPermissionsEnum.web权限,
        //            departmentId: 2,
        //            age: 25,
        //            photoPath: "/images/photo2.jpg",
        //            shiftId: 2
        //                )
        //        },            
        //        Total = 2,
        //        Code = 1,
        //    };
        //    A.CallTo(() => repository.GetAllAsync())
        //        .Returns(mockQueryable);
        //    queryModel.OrderBy = "createTime";
        //    // Act
        //    var result = await sut.GetEmployeeAsync(queryModel);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(1, result.Code); // Ensure the response code is correct

        //    // 验证返回的 DTO 列表
        //    var actualDtos = result.List;
        //    Assert.NotNull(actualDtos);

        //    // 验证 DTO 的内容

        //    for (int i = 0; i < employees.List.Count; i++)
        //    {
        //        Assert.Equal(employees.List[i].Id, actualDtos[i].Id);
        //        Assert.Equal(employees.List[i].Id, actualDtos[i].Id);
        //        Assert.Equal(employees.List[i].DepartmentId, actualDtos[i].DepartmentId);
        //        Assert.Equal(employees.List[i].Department.DepartmentName, actualDtos[i].DepartmentName);
        //        Assert.Equal(employees.List[i].Age, actualDtos[i].Age);
        //        Assert.Equal(employees.List[i].Cellphone, actualDtos[i].Cellphone);
        //        Assert.Equal((int)employees.List[i].LoginPermissions, actualDtos[i].LoginPermissions);
        //        Assert.Equal(employees.List[i].LoginPermissions.ToString(), actualDtos[i].LoginPermissionsStr);
        //        Assert.Equal((int)employees.List[i].Sex, actualDtos[i].Sex);
        //        Assert.Equal(employees.List[i].Sex.ToString(), actualDtos[i].SexStr);
        //        Assert.Equal(employees.List[i].PhotoPath, actualDtos[i].PhotoPath);
        //        Assert.Equal(employees.List[i].CreateTime, actualDtos[i].CreateTime);
        //        Assert.Equal(employees.List[i].RealName, actualDtos[i].RealName);
        //        Assert.Equal(employees.List[i].Remark, actualDtos[i].Remark);
        //        Assert.Equal(employees.List[i].RoleId, actualDtos[i].RoleId);
        //        Assert.Equal(employees.List[i].Role.RoleName, actualDtos[i].RoleName);
        //        Assert.Equal((int)employees.List[i].Status, actualDtos[i].Status);
        //        Assert.Equal(employees.List[i].Status.ToString(), actualDtos[i].StatusStr);
        //        Assert.Equal(employees.List[i].UserName, actualDtos[i].UserName);
        //        Assert.Equal(employees.List[i].ShiftId, actualDtos[i].ShiftId);
        //        Assert.Equal(employees.List[i].ShiftInfo.ShiftName, actualDtos[i].ShiftName);
        //    }
        //    // 验证方法调用次数
        //    A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
        //}

    }
}
