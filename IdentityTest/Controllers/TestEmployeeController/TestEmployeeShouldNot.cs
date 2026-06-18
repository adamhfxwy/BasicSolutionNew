using FakeItEasy;
using LanTian.Solution.Core.CommonHelper;
using LanTian.Solution.Core.Domain.INpgSqlService;
using LanTian.Solution.Core.DTO.Identity;
using WebAPI.Models;
using LanTian.Solution.Core.ParameterModel.ChangeModel.Identity;
using LanTian.Solution.Core.propertyMgtWebAPI;
using LanTian.Solution.Core.propertyMgtWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanTian.Solution.Core.EnumAndConstent.Enums.LanTianEnum;

namespace LanTian.WisdomParkTests.Controllers.TestEmployeeController
{
    public class TestEmployeeShouldNot
    {
        private readonly WebAPI.Controllers.Identity.EmployeeController _controller;
        private readonly IEmployeeService _fakeEmployeeService;
        private readonly IMemoryCacheHelper _fakeMemoryCacheHelper;
        private readonly IConfiguration _fakeConfig;
        private readonly HttpClient _fakeHttpClient;
        public TestEmployeeShouldNot()
        {
            _fakeEmployeeService = A.Fake<IEmployeeService>();
            _fakeMemoryCacheHelper = A.Fake<IMemoryCacheHelper>();
            _fakeConfig = A.Fake<IConfiguration>();
            _fakeHttpClient = A.Fake<HttpClient>();

            _controller = new WebAPI.Controllers.Identity.EmployeeController(
                _fakeEmployeeService,
                _fakeMemoryCacheHelper,
                _fakeConfig,
                _fakeHttpClient
            );
        }
        [Fact]
        public async Task AddEmployeeAsync_ReturnsFailed_WhenInvalidRealNameModel()
        {
            // Arrange
            var model = new AddEditEmployeeModel
            {
                RealName = "",
                Cellphone = "12345678910",
                LoginPermissions = LoginPermissionsEnum.所有权限,
                Sex = SexEnum.男
            };
            // Act
            var result = await _controller.AddEmployeeAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JObject.FromObject(okResult.Value);

            // Assert

            Assert.Equal("姓名不可为空", response["Msg"]);
            // 验证服务层是否被调用
            A.CallTo(() => _fakeEmployeeService.AddEmployeeAsync(model, A<CancellationToken>.Ignored))
                .MustNotHaveHappened();
        }
        [Fact]
        public async Task EditEmployeeAsync_ReturnsFailed_WhenEmployeeNotFound()
        {
            // Arrange
            var model = new AddEditEmployeeModel { Id = 999 };

            A.CallTo(() => _fakeEmployeeService.GetEmployeeByIdAsync(A<long>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult<EmployeeDTO>(null));

            // Act
            var result = await _controller.EditEmployeeAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JObject.FromObject(okResult.Value);

            // Assert
            Assert.Contains($"id={model.Id}的员工不存在", response["Msg"].ToString());
            // 验证服务层是否被调用
            A.CallTo(() => _fakeEmployeeService.EditEmployeeAsync(model, A<CancellationToken>.Ignored))
                .MustNotHaveHappened();
        }
        [Fact]
        public async Task GetEmployeeByIdAsync_ReturnsFailed_WhenIdIsInvalid()
        {
            // Arrange
            var model = new GetByIdModel { Id = 0 };

            // Act
            var result = await _controller.GetEmployeeByIdAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JObject.FromObject(okResult.Value);
            // Assert
            Assert.Equal("请选择Id", response["Msg"].ToString());
           
        }
    }
}
