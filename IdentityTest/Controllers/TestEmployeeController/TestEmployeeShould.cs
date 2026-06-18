
using AutoFixture.Xunit2;
using FakeItEasy;
using LanTian.Solution.Core.CommonHelper;
using LanTian.Solution.Core.Domain.ICommonService;
using LanTian.Solution.Core.Domain.INpgSqlService;
using LanTian.Solution.Core.Domain.NpgSqlEntities.Identity;
using LanTian.Solution.Core.DTO.Identity;
using WebAPI.Models;
using LanTian.Solution.Core.Infrastructure.NpgSqlService;
using LanTian.Solution.Core.ParameterModel.ChangeModel.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Testing.Shared;
using static LanTian.Solution.Core.EnumAndConstent.Enums.LanTianEnum;
using LanTian.Solution.Core.ParameterModel.QueryModel.Identity;
using LanTian.Solution.Core.EnumAndConstent;

namespace LanTian.WisdomParkTests.Controllers.TestEmployeeController
{
    public class TestEmployeeShould
    {
        private readonly WebAPI.Controllers.Identity.EmployeeController _controller;
        private readonly IEmployeeService _fakeEmployeeService;
        private readonly IMemoryCacheHelper _fakeMemoryCacheHelper;
        private readonly IConfiguration _fakeConfig;
        private readonly HttpClient _fakeHttpClient;
        public TestEmployeeShould()
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
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task AddEmployeeAsync_ReturnsSuccess_WhenValidModel(
                 AddEditEmployeeModel model
            )
        {
            // Arrange
            //model.Cellphone = "123";
            A.CallTo(() => _fakeEmployeeService.AddEmployeeAsync(A<AddEditEmployeeModel>.Ignored, A<CancellationToken>.Ignored))
             .Returns(Task.FromResult(new Tuple<long, string>(2, "success")));

            // Act
            var result = await _controller.AddEmployeeAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody<long>;
            // Assert
            Assert.NotNull(response);
            Assert.Equal("success", response.Msg);
            Assert.Equal("Ok", response.Status);
            Assert.Equal(2, (long)response.Data);


            // 验证服务层是否被调用
            A.CallTo(() => _fakeEmployeeService.AddEmployeeAsync(model, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task EditEmployeeAsync_ReturnsSuccess_WhenSuccess(AddEditEmployeeModel model)
        {
            // Arrange
            var existingEmployee = new EmployeeDTO
            {
                Id = 1,
                RoleId = 1
            };

            A.CallTo(() => _fakeEmployeeService.GetEmployeeByIdAsync(A<long>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(existingEmployee));
            A.CallTo(() => _fakeEmployeeService.EditEmployeeAsync(A<AddEditEmployeeModel>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(new Tuple<bool, string>(true, "success")));

            // Act
            var result = await _controller.EditEmployeeAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody;
            // Assert
            Assert.Equal("Ok", response.Status);
            Assert.Equal("success", response.Msg);
            // 验证服务层是否被调用
            A.CallTo(() => _fakeEmployeeService.EditEmployeeAsync(model, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 删除成功
        /// </summary>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task DeleteEmployeeAsync_ReturnsSuccess_WhenSuccess(GetByIdModel model)
        {
            // Arrange
            var existingEmployee = new EmployeeDTO
            {
                Id = 1,
                RoleId = 1
            };
            A.CallTo(() => _fakeEmployeeService.GetEmployeeByIdAsync(A<long>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(existingEmployee));
            A.CallTo(() => _fakeEmployeeService.RemoveEmployeeAsync(A<long>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(new Tuple<bool, string>(true, "success")));
            // Act
            var result = await _controller.RemoveEmployeeAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody;
            // Assert
            Assert.Equal("Ok", response.Status);
            Assert.Equal("success", response.Msg);
            // 验证服务层是否被调用
            A.CallTo(() => _fakeEmployeeService.RemoveEmployeeAsync(model.Id, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task GetEmployeeByIdAsync_ReturnsOk_WhenSuccess(
            GetByIdModel model)
        {
            // Arrange
            var fakeEmployee = new EmployeeDTO
            {
                Id = model.Id,
                RealName = "Test Employee",
                Cellphone = "12345678901",
                RoleId = 1
            };
            A.CallTo(() => _fakeEmployeeService.GetEmployeeByIdAsync(model.Id, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(fakeEmployee));

            // Act
            var result = await _controller.GetEmployeeByIdAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody<EmployeeDTO>;

            // Assert
            Assert.Equal("Ok", response.Status);
            Assert.NotNull(response.Data);
            Assert.Equal(model.Id, response.Data.Id);

            A.CallTo(() => _fakeEmployeeService.GetEmployeeByIdAsync(model.Id, A<CancellationToken>.Ignored))
             .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task GetEmployeeAsync_WhenSuccess(EmployeeQueryModel query)
        {
            // Arrange
            var fakeEmployee = new Pagination<EmployeeDTO>
            {
                Code = 1,
                List = new List<EmployeeDTO>
                {
                    new EmployeeDTO
                    {
                        Id = 1,
                        RealName = "Test Employee",
                        Cellphone = "12345678901",
                        RoleId = 1                    
                    },
                    new EmployeeDTO
                    {
                        Id = 2,
                        RealName = "Test Employee1",
                        Cellphone = "12345678922",
                        RoleId = 1
                    },
                },             
            };
            A.CallTo(() => _fakeEmployeeService.GetEmployeeAsync(query, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(fakeEmployee));
            // Act
            var result = await _controller.GetEmployeeAsync(query);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody<EmployeeDTO[]>;

            // Assert

            Assert.Equal("Ok", response.Status);
            Assert.NotNull(response.Data);
            Assert.Equal(fakeEmployee.List.Count, response.TotalCount);
            A.CallTo(() => _fakeEmployeeService.GetEmployeeAsync(query, A<CancellationToken>.Ignored))
             .MustHaveHappenedOnceExactly();
        }
    }
}
