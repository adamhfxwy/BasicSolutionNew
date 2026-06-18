using FakeItEasy;
using FluentAssertions;
using LanTian.Solution.Core.CommonHelper;
using LanTian.Solution.Core.Domain.INpgSqlService;
using LanTian.Solution.Core.DTO.Identity;
using LanTian.Solution.Core.ParameterModel.ChangeModel.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebAPI;
using WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Shared;

namespace IdentityTest.Controllers.MainController
{
    public class TestMainShould
    {
        private readonly WebAPI.Controllers.Identity.MainController _controller;
        private readonly IEmployeeService _fakeEmployeeService;
        private readonly IMenuService _fakeMenuService;
        private readonly IMobilePermissionsService _fakeMobilePermissionsService;
        private readonly JwtParam _fakeJwtParam;
        private readonly IRoleService _fakeRoleService;
        private readonly IMemoryCacheHelper _fakeMemoryCacheHelper;
        public TestMainShould()
        {
            _fakeEmployeeService = A.Fake<IEmployeeService>();
            _fakeMenuService = A.Fake<IMenuService>();
            _fakeMobilePermissionsService = A.Fake<IMobilePermissionsService>();
            _fakeRoleService = A.Fake<IRoleService>();
            _fakeMemoryCacheHelper = A.Fake<IMemoryCacheHelper>();
            _fakeJwtParam= new JwtParam 
            {
                ValidateAudience = false,    
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuerSigningKey= "bwNfxPbPbn2K8qy8xq8LjURDvCO+QQ7EioxNIw7q7M8=",
                ValidAudience="",
                ValidIssuer= "Lantianhuanjing",
                ValidLifetime= 9999,

            };
            _controller = new WebAPI.Controllers.Identity.MainController(
               _fakeMemoryCacheHelper,
               _fakeRoleService,
               _fakeJwtParam,
               _fakeMobilePermissionsService,
               _fakeMenuService,
              _fakeEmployeeService
          );        
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task LoginAsync_ReturnsSuccess_WhenSuccess(
                LoginModel model,
                EmployeeDTO emp,
                RoleDTO role,
                CancellationToken cancellationToken = default            
            )
        {
            EmployeeDTO user = new EmployeeDTO 
            {
                UserName=emp.UserName,
                DepartmentId=emp.DepartmentId,
                DepartmentName=emp.DepartmentName,
                Age=emp.Age,
                Cellphone= model.Cellphone,
                Id=emp.Id,
                CreateTime=emp.CreateTime,
                LoginPermissions=4,
                Permissions=emp.Permissions,
                Remark=emp.Remark,
                RoleId=emp.RoleId,
                RoleName=emp.RoleName,
                Sex=emp.Sex,
                ShiftId=emp.ShiftId,
                ShiftName=emp.ShiftName,
                Status=emp.Status,
                RealName=emp.RealName
            };
            // Arrange
            A.CallTo(() => _fakeEmployeeService.CheckLoginAsync(A<string>.Ignored, A<string>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(new Tuple<bool, string, EmployeeDTO>(true, "success", user)));
            A.CallTo(() => _fakeRoleService.GetRoleByIdAsync(A<long>.Ignored))
                .Returns(Task.FromResult(role));
            // Act
            var result = await _controller.Login(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JObject.FromObject(okResult.Value);// 或定义为具体的类型
            var actualUser = response["User"].ToObject<EmployeeDTO>();
            var token = response["Token"].ToString();
            var userId = Convert.ToInt64(response["UserId"]);
            var status = response["Status"].ToString();

            // Assert
            Assert.NotNull(response);
            actualUser.Should().BeEquivalentTo(user);
            token.Should().NotBeNullOrEmpty();
            userId.Should().Be(user.Id);
            status.Should().Be("Ok");
            // 验证服务层是否被调用
            A.CallTo(() => _fakeEmployeeService.CheckLoginAsync(A<string>.Ignored, A<string>.Ignored,A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }



    }
}
