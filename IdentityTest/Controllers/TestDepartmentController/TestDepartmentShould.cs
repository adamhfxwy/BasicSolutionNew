using FakeItEasy;
using LanTian.Solution.Core.CommonHelper;
using LanTian.Solution.Core.Domain.INpgSqlService;
using LanTian.Solution.Core.DTO.Identity;
using LanTian.Solution.Core.EnumAndConstent;
using LanTian.Solution.Core.ParameterModel.ChangeModel.Identity;
using LanTian.Solution.Core.ParameterModel.QueryModel.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Shared;

namespace IdentityTest.Controllers.TestDepartmentController
{
    public class TestDepartmentShould
    {
        private readonly WebAPI.Controllers.Identity.DepartmentController _controller;
        private readonly IDepartmentService _fakeDepartmentService;
        private readonly IEmployeeService _fakeEmployeeService;
        public TestDepartmentShould()
        {
            _fakeEmployeeService = A.Fake<IEmployeeService>();
            _fakeDepartmentService = A.Fake<IDepartmentService>();
            _controller = new WebAPI.Controllers.Identity.DepartmentController(
              _fakeEmployeeService,
              _fakeDepartmentService
          );
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task AddDepartmentAsync_ReturnsSuccess_WhenValidModel(
                 AddEditDepartmentModel model
            )
        {
            // Arrange
            //model.Cellphone = "123";
            A.CallTo(() => _fakeDepartmentService.AddDepartmentAsync(A<AddEditDepartmentModel>.Ignored, A<CancellationToken>.Ignored))
             .Returns(Task.FromResult(new Tuple<long, string>(2, "success")));

            // Act
            var result = await _controller.AddDepartmentAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody<long>;
            // Assert
            Assert.NotNull(response);
            Assert.Equal("success", response.Msg);
            Assert.Equal("Ok", response.Status);
            Assert.Equal(2, (long)response.Data);


            // 验证服务层是否被调用
            A.CallTo(() => _fakeDepartmentService.AddDepartmentAsync(model, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task EditEmployeeAsync_ReturnsSuccess_WhenValidModel(AddEditDepartmentModel model)
        {
            // Arrange
            var existingDepartment = new DepartmentDTO
            {
                Id = 1,
                DepartmentName = "Test"
            };

            A.CallTo(() => _fakeDepartmentService.GetDepartmentByIdAsync(A<long>.Ignored))
                .Returns(Task.FromResult(existingDepartment));
            A.CallTo(() => _fakeDepartmentService.EditDepartmentAsync(A<AddEditDepartmentModel>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(new Tuple<bool, string>(true, "success")));

            // Act
            var result = await _controller.EditDepartmentAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody;

            // Assert
            Assert.Equal("Ok", response.Status);
            Assert.Equal("success", response.Msg);
            // 验证服务层是否被调用
            A.CallTo(() => _fakeDepartmentService.EditDepartmentAsync(model, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 删除成功
        /// </summary>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task DeleteDepartmentAsync_ReturnsSuccess_WhenSuccess(
            GetByIdModel model
            )
        {
            // Arrange
            DepartmentDTO? existingDepartment = new DepartmentDTO()
            {
                Id = model.Id,
                DepartmentName ="Test"
            };
            A.CallTo(() => _fakeDepartmentService.GetDepartmentByIdAsync(A<long>.Ignored))
            .Returns(Task.FromResult(existingDepartment));
            A.CallTo(() => _fakeDepartmentService.RemoveDepartmentAsync(A<long>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(new Tuple<bool, string>(true, "success")));
            // Act
            var result = await _controller.RemoveDepartmentAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody;
            // Assert
            Assert.Equal("Ok", response.Status);
            Assert.Equal("success", response.Msg);
            // 验证服务层是否被调用
            A.CallTo(() => _fakeDepartmentService.RemoveDepartmentAsync(model.Id, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task GetDepartmentByIdAsync_ReturnsOk_WhenDepartmentExists(
            GetByIdModel model)
        {
            // Arrange
            var fakeDepartment = new DepartmentDTO
            {
                Id = model.Id,
                DepartmentName = "Test"
            };
            A.CallTo(() => _fakeDepartmentService.GetDepartmentByIdAsync(model.Id))
                .Returns(Task.FromResult(fakeDepartment));

            // Act
            var result = await _controller.GetDepartmentByIdAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody<DepartmentDTO>;
            // Assert

            Assert.Equal("Ok", response.Status);
            Assert.NotNull(response.Data);
            Assert.Equal(model.Id, response.Data.Id);

            A.CallTo(() => _fakeDepartmentService.GetDepartmentByIdAsync(model.Id))
             .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task GetDepartmentAsync_ReturnsOk(DepartmentQueryModel query)
        {
            // Arrange
            var fakeDepartment = new Pagination<DepartmentDTO>
            {
                Code = 1,
                List = new List<DepartmentDTO>
                {
                    new DepartmentDTO
                    {
                        Id = 1,
                        DepartmentName = "Test Department"
                    },
                    new DepartmentDTO
                    {
                        Id = 2,
                        DepartmentName = "Test Department1"
                    },
                },
            };
            A.CallTo(() => _fakeDepartmentService.GetDepartmentAsync(query, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(fakeDepartment));
            // Act
            var result = await _controller.GetDepartmentAsync(query);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody<DepartmentDTO[]>;

            // Assert
            Assert.Equal("Ok", response.Status);
            Assert.NotNull(response.Data);
            Assert.Equal(fakeDepartment.List.Count, response.TotalCount);
            A.CallTo(() => _fakeDepartmentService.GetDepartmentAsync(query, A<CancellationToken>.Ignored))
             .MustHaveHappenedOnceExactly();
        }
    }
}
