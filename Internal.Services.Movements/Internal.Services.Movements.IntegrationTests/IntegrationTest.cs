using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using Internal.Services.Movements.IntegrationTests.Utilities;
using Internal.Services.Movements.ProxyClients;
using Internal.Services.Movements.Data.Contexts;
using Internal.Services.Movements.Business.Manager;
using Internal.Services.Movements.IntegrationTests.ObjectMother;
using FluentAssertions;

namespace Internal.Services.Movements.IntegrationTests
{
  public class IntegrationTest : IClassFixture<TestStartup<Program>>
  { 
    private readonly TestStartup<Program> _factory;
    private readonly HttpClient _client;
    private readonly ExternalMovementsMother _externalMovements;

    public IntegrationTest(TestStartup<Program> factory)
    {
      _factory = factory;
      _client = _factory.CreateClient();
      _externalMovements = new ExternalMovementsMother();
    }

    [Fact]
    public async Task GivenIncomingTransferFilterType_WhenCallingGetMovementsAsync_ThenValidIncomingTransferMovementsAreReturned()
    {
      //Arrange
      var pageNumber = 1;
      var pageSize = 20;
      var account = AccountHelper.CustomerAccount;
      var internalFilterType = Data.Models.Enums.EnumMovementType.Incoming;

      //Act
      var response = GetMovements(internalFilterType, pageNumber, pageSize, account, null, null, null, 0, null);

      //Assert
      var movements = (await response).Movements;
      movements.Should().OnlyContain(x => x.AccountFrom != account && x.AccountTo == account)
      .And.OnlyContain(x => x.Amount >= 0)
      .And.HaveCountLessOrEqualTo(pageSize);
    }

    [Fact]
    public async Task GivenFiscalTransferFilterType_WhenCallingGetMovementsAsync_ThenValidFiscalTransferAccountMovementsAreReturned()
    {
      //Arrange
      var pageNumber = 1;
      var pageSize = 20;
      var account = AccountHelper.CustomerAccount;
      var internalFilterType = Data.Models.Enums.EnumMovementType.FiscalTransfer;
      var movementType = AccountHelper.FiscalTransferAccount;

      //Act
      var response = GetMovements(internalFilterType, pageNumber, pageSize, account, null, movementType, null, null, null);

      //Assert
      var movements = (await response).Movements;
      movements.Should().OnlyContain(x => x.AccountFrom == AccountHelper.FiscalTransferAccount && x.AccountTo == account)
      .And.OnlyContain(x => x.Amount >= 0)
      .And.HaveCountLessOrEqualTo(pageSize);
    }

    [Fact]
    public async Task GivenInterestTransferFilterType_WhenCallingGetMovementsAsync_ThenValidInterestTransferMovementsAreReturned()
    {
      //Arrange
      var pageNumber = 1;
      var pageSize = 15;
      var account = AccountHelper.CustomerAccount;
      var internalFilterType = Data.Models.Enums.EnumMovementType.Incoming;

      //Act
      var response = GetMovements(internalFilterType, pageNumber, pageSize, account, null, null, null, 0, null);

      //Assert
      var movements = (await response).Movements;
      movements.Should().OnlyContain(x => x.AccountFrom != account && x.AccountTo == account)
      .And.OnlyContain(x => x.Amount >= 0)
      .And.HaveCountLessOrEqualTo(pageSize);
    }

    private Task<PagedMovements> GetMovements(Data.Models.Enums.EnumMovementType? filterType, int? pageNumber, int pageSize, string account, EnumMovementType? movementType, string? accountFrom, string? accountTo, decimal? amountFrom, decimal? amountTo)
    {
      _factory.MovementMock.Setup(x => x.GetMovementsAsync(pageNumber, pageSize, account, movementType, accountFrom, accountTo, amountFrom, amountTo))
        .Returns(_externalMovements.GetMovements(pageNumber, pageSize, account, movementType, accountFrom, accountTo, amountFrom, amountTo));
      
      MovementsDataContext database = DbHelper.PrepareDbForTest();

      MovementsManager _manager = new MovementsManager(_factory.MovementMock.Object, database);
      Task<PagedMovements> movements = _manager.GetMovements(1, filterType, 1, pageSize);

      return movements;
    }
  }
}