using AutoFixture.Xunit2;
using FakeItEasy;
using FluentAssertions;
using LanTian.Solution.Core.Domain.ICommonService;
using LanTian.Solution.Core.Domain.NpgSqlEntities.Identity;
using LanTian.Solution.Core.Infrastructure.NpgSqlService;
using LanTian.Solution.Core.ParameterModel.ChangeModel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Testing.Shared;
using static LanTian.Solution.Core.EnumAndConstent.Enums.LanTianEnum;

namespace LanTian.WisdomParkTests.Service.TestEmployeeService
{
    public class TestEmployeeShouldNot
    {
        [Theory]
        [AutoFakeItEasy]
        public async Task CreateUserAsync_When_Cellphone_Exists(
            [Frozen] IRepository<LanTianEmployee> repository,
            EmployeeService sut, string realName, string cellphone, LoginPermissionsEnum loginPermissionsEnum, SexEnum sex,
            CancellationToken cancellationToken)
        {
            // Arrange
            var model = new AddEditEmployeeModel
            {
                RealName = realName,
                Cellphone = cellphone,
                LoginPermissions = loginPermissionsEnum,
                Sex = sex
            };
            A.CallTo(() => repository.AnyAsync(A<Expression<Func<LanTianEmployee, bool>>>._, A<CancellationToken>._))
          .Returns(true);
            //Act
            var tuple = await sut.AddEmployeeAsync(model, cancellationToken);
            //Assert
            tuple.Item2.Should().BeSameAs("手机号不能重复");

            A.CallTo(() => repository.InsertAsync(A<LanTianEmployee>.Ignored,true, cancellationToken))
               .MustNotHaveHappened();
        }
    }
}
