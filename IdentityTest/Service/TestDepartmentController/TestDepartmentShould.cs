using AutoFixture.Xunit2;
using FakeItEasy;
using FluentAssertions;
using LanTian.Solution.Core.Domain.ICommonService;
using LanTian.Solution.Core.Domain.NpgSqlEntities.Identity;
using LanTian.Solution.Core.EnumAndConstent;
using LanTian.Solution.Core.EnumAndConstent.SubEntitys;
using LanTian.Solution.Core.Infrastructure.NpgSqlService;
using LanTian.Solution.Core.Infrastructure.Utils;
using LanTian.Solution.Core.ParameterModel.ChangeModel.Identity;
using LanTian.Solution.Core.ParameterModel.QueryModel;
using LanTian.Solution.Core.ParameterModel.QueryModel.Identity;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Testing.Shared;

namespace IdentityTest.Service.TestDepartmentController
{
    public class TestDepartmentShould
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
            [Frozen] IRepository<LanTianDepartment> repository,
            DepartmentService sut,
             AddEditDepartmentModel model,
            CancellationToken cancellationToken
            )
        {
            // Arrange
            A.CallTo(() => repository.FindAsync(x => x.DepartmentName == model.DepartmentName, CancellationToken.None, null))
            .Returns(Task.FromResult<LanTianDepartment?>(null));
            //Act
            var tuple = await sut.AddDepartmentAsync(model, cancellationToken);
            //Assert
            tuple.Item2.Should().BeSameAs("success");
            A.CallTo(() => repository.InsertAsync(A<LanTianDepartment>.That.Matches(u => u.DepartmentName == model.DepartmentName), true, cancellationToken))
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
        [Frozen] IRepository<LanTianDepartment> repository,
        DepartmentService sut,
        LanTianDepartment existingEmp,
        AddEditDepartmentModel model,
        CancellationToken cancellationToken)
        {
            // Arrange
            A.CallTo(() => repository.FindAsync(model.Id.Value, null))
            .Returns(Task.FromResult(existingEmp));
            //Act
            var tuple = await sut.EditDepartmentAsync(model, cancellationToken);
            //Assert
            tuple.Item2.Should().BeSameAs("success");
            A.CallTo(() => repository.UpdateAsync(A<LanTianDepartment>.That.Matches(u => u.DepartmentName == model.DepartmentName), true, cancellationToken))
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
        public async Task GetDepartmentByIdAsync_WhenSuccess(
              [Frozen] IRepository<LanTianDepartment> repository,
              DepartmentService sut,
              LanTianDepartment existingEmp,
              long id)
        {
            //Arrange
            A.CallTo(() => repository.FindAsync(id, null))
                .Returns(existingEmp);
            //Act
            var obj = await sut.GetDepartmentByIdAsync(id);
            var objDTO = ToDTOUtils.ToDTO(existingEmp);
            // Assert
            obj.Should().BeEquivalentTo(objDTO);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="sut"></param>
        /// <param name="existingEmp"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Theory]
        [AutoFakeItEasy]
        public async Task GetDepartmentAsync_WhenSuccess_ShouldReturnExpectedData(
        [Frozen] IRepository<LanTianDepartment> repository,
        DepartmentService sut,
        DepartmentQueryModel query,
        Pagination<LanTianDepartment> existings)
        {
            // Arrange

            // 模拟仓储层方法返回分页数据
            existings = new Pagination<LanTianDepartment>
            {
                List = new List<LanTianDepartment>()
                {
                    new LanTianDepartment(departmentName:"Department A",remark:"dfs",departmentLeader:null ),
                    new LanTianDepartment(departmentName:"Department B",remark:"dfs",departmentLeader:null )
                },
                Total = 2,
                Code = 1,
            };
            A.CallTo(() => repository.GetListOfSignalTableAsync(
                    query,
                    A<Expression<Func<LanTianDepartment, bool>>>.Ignored,
                    A<Expression<Func<LanTianDepartment, object[]>>>.Ignored,
                    A<CancellationToken>.Ignored,
                    A<bool>.Ignored,
                    A<string[]>.Ignored))
                .Returns(Task.FromResult(existings));

            // Act
            var result = await sut.GetDepartmentAsync(query);

            // Assert
            // 验证返回值是否符合预期
            Assert.NotNull(result);
            Assert.Equal(existings.Total, result.Total);
            Assert.Equal(existings.Code, result.Code);

            // 验证 DTO 的正确性
            Assert.Equal(existings.List.Count, result.List.Count);
            for (int i = 0; i < existings.List.Count; i++)
            {
                Assert.Equal(existings.List[i].Id, result.List[i].Id);
                Assert.Equal(existings.List[i].DepartmentName, result.List[i].DepartmentName);
            }        
            A.CallTo(() => repository.GetListOfSignalTableAsync(
                    query,
                    A<Expression<Func<LanTianDepartment, bool>>>.Ignored,
                    A<Expression<Func<LanTianDepartment, object[]>>>.Ignored,
                    A<CancellationToken>.Ignored,
                    A<bool>.Ignored,
                    A<string[]>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

    }
}
