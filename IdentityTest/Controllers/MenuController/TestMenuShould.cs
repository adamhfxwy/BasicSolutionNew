using FakeItEasy;
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

namespace IdentityTest.Controllers.MenuController
{
    public class TestMenuShould
    {
        private readonly WebAPI.Controllers.Identity.MenuController _controller;
        private readonly IMenuService _fakeMenuService;
        public TestMenuShould()
        {
            _fakeMenuService = A.Fake<IMenuService>();
            _controller = new WebAPI.Controllers.Identity.MenuController(
              _fakeMenuService
          );
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task AddMenuAsync_ReturnsSuccess_WhenValidModel(
                 AddEditMenuModel model
            )
        {
            // Arrange
            //model.Cellphone = "123";
            A.CallTo(() => _fakeMenuService.AddMenuAsync(A<AddEditMenuModel>.Ignored, A<CancellationToken>.Ignored))
             .Returns(Task.FromResult(new Tuple<long, string>(2, "success")));

            // Act
            var result = await _controller.AddMenuAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody;
            // Assert
            Assert.NotNull(response);
            Assert.Equal("success", response.Msg);
            Assert.Equal("Ok", response.Status);
            //Assert.Equal(2, (long)response.Data);


            // 验证服务层是否被调用
            A.CallTo(() => _fakeMenuService.AddMenuAsync(model, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task EditEmployeeAsync_ReturnsSuccess_WhenValidModel(AddEditMenuModel model)
        {
            // Arrange
            var existingMenu = new MenuDTO
            {
                Id = 1,
                MenuName = "Test"
            };

            A.CallTo(() => _fakeMenuService.GetMenusAndButtonByIdAsync(A<long>.Ignored))
                .Returns(Task.FromResult(existingMenu));
            A.CallTo(() => _fakeMenuService.EditMenuAsync(A<AddEditMenuModel>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(new Tuple<bool, string>(true, "success")));

            // Act
            var result = await _controller.EditMenuAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody;

            // Assert
            Assert.Equal("Ok", response.Status);
            Assert.Equal("success", response.Msg);
            // 验证服务层是否被调用
            A.CallTo(() => _fakeMenuService.EditMenuAsync(model, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 删除成功
        /// </summary>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task DeleteMenuAsync_ReturnsSuccess_WhenSuccess(
            GetByIdModel model
            )
        {
            // Arrange
            MenuDTO? existingMenu = new MenuDTO()
            {
                Id = model.Id,
                MenuName = "Test"
            };
            A.CallTo(() => _fakeMenuService.GetMenusAndButtonByIdAsync(A<long>.Ignored))
            .Returns(Task.FromResult(existingMenu));
            A.CallTo(() => _fakeMenuService.RemoveMenusAsync(A<long>.Ignored, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(new Tuple<bool, string>(true, "success")));
            // Act
            var result = await _controller.RemoveMenusAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody;
            // Assert
            Assert.Equal("Ok", response.Status);
            Assert.Equal("success", response.Msg);
            // 验证服务层是否被调用
            A.CallTo(() => _fakeMenuService.RemoveMenusAsync(model.Id, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task GetMenuByIdAsync_ReturnsOk_WhenMenuExists(
            GetByIdModel model)
        {
            // Arrange
            var fakeMenu = new MenuDTO
            {
                Id = model.Id,
                MenuName = "Test"
            };
            A.CallTo(() => _fakeMenuService.GetMenusAndButtonByIdAsync(model.Id))
                .Returns(Task.FromResult(fakeMenu));

            // Act
            var result = await _controller.GetMenusAndButtonByIdAsync(model);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody<MenuDTO>;
            // Assert

            Assert.Equal("Ok", response.Status);
            Assert.NotNull(response.Data);
            Assert.Equal(model.Id, response.Data.Id);

            A.CallTo(() => _fakeMenuService.GetMenusAndButtonByIdAsync(model.Id))
             .MustHaveHappenedOnceExactly();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task GetMenuAsync_ReturnsOk(int isFilter)
        {
            // Arrange
            var fakeMenu = new List<MenuDTO>
            {
                new MenuDTO
                    {
                        Id = 1,
                        MenuName = "Test Menu"
                    },
                    new MenuDTO
                    {
                        Id = 2,
                        MenuName = "Test Menu1"
                    }
            };
            A.CallTo(() => _fakeMenuService.GetAllMenusAndButtonAsync(A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(fakeMenu));
            // Act
            var result = await _controller.GetAllMenusAndButtonAsync(isFilter);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as ResponseBody<MenuTreeModel[]>;

            // Assert
            Assert.Equal("Ok", response.Status);
            Assert.NotNull(response.Data);
            Assert.Equal(fakeMenu.Count, response.TotalCount);
            A.CallTo(() => _fakeMenuService.GetAllMenusAndButtonAsync(A<CancellationToken>.Ignored))
             .MustHaveHappenedOnceExactly();
        }
    }
}
