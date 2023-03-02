using Internal.Services.Movements.ProxyClients;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internal.Services.Movements.IntegrationTests.ObjectMother
{
  internal class ExternalMovementsMother
  {
    public async Task<PagedMovements> GetMovements(int? pageNumber, int? pageSize, string? account, EnumMovementType? movementType, string? accountFrom, string? accountTo, decimal? amountFrom, decimal? amountTo)
    {
      var pagedMovements = new PagedMovements
      {
        PageSize = (int)pageSize,
        PageNumber = (int)pageNumber,
        Movements = getTestMovements((int)pageNumber, (int)pageSize, account, movementType, accountFrom, accountTo, amountFrom, amountTo)
      };
      return pagedMovements;
    }

    private List<Movement> getTestMovements(int pageNumber, int pageSize, string? account, EnumMovementType? movementType, string? accountFrom, string? accountTo, decimal? amountFrom, decimal? amountTo)
    {
      var movements = allTestMovements();
      
      if (!string.IsNullOrWhiteSpace(account))
      {
        movements = movements.Where(x => x.Account == account);
      }

      if (movementType.HasValue)
      {
        movements = movements.Where(x => x.MovementType == movementType);
      }

      if (!string.IsNullOrWhiteSpace(accountFrom))
      {
        movements = movements.Where(x => x.AccountFrom == accountFrom);
      }

      if (!string.IsNullOrWhiteSpace(accountTo))
      {
        movements = movements.Where(x => x.AccountTo == accountTo);
      }

      if (amountFrom.HasValue)
      {
        movements = movements.Where(x => x.Amount >= amountFrom.Value);
      }

      if (amountTo.HasValue)
      {
        movements = movements.Where(x => x.Amount <= amountTo.Value);
      }

      var skip = (pageNumber - 1) * pageSize;

      return movements.Skip(skip).Take(pageSize).ToList();
    }
  
    private IEnumerable<Movement> allTestMovements()
    {
      var movements = new List<Movement>();

      var customerAccount = "NL54FAKE0062046111";
      var customerNominatedAccount = "NL96NMFK0208212218";
      var interestAccount = "SystemFakeInterestAccount";
      var feeAccount = "SystemFakeFeeAccount";
      var taxAccount = "SystemFakeTaxAccount";
      var fiscalTransferAccount = "NL54FAKE0326806738";

      for (int i = 0; i < 10; i++)
      {
        var newMovements = new List<Movement>
        {
          // System interest movement
          new Movement
          {
            MovementId = i * 9 + 1000,
            Account = customerAccount,
            MovementType = EnumMovementType.Interest,
            Amount = (decimal)0.42 + i,
            AccountFrom = interestAccount,
            AccountTo = customerAccount
          },
          // System fee movement
          new Movement
          {
            MovementId = i * 9 + 1001,
            Account = customerAccount,
            MovementType = EnumMovementType.Fee,
            Amount = (decimal)-0.59 - i,
            AccountFrom = customerAccount,
            AccountTo = feeAccount
          },
          // System tax movement
          new Movement
          {
            MovementId = i * 9 + 1002,
            Account = customerAccount,
            MovementType = EnumMovementType.Tax,
            Amount = (decimal)-200.77 - i,
            AccountFrom = customerAccount,
            AccountTo = taxAccount
          },
          // Fiscal transfer movement
          new Movement
          {
            MovementId = i * 9 + 1003,
            Account = customerAccount,
            MovementType = EnumMovementType.Unknown,
            Amount = (decimal)17000 + i,
            AccountFrom = fiscalTransferAccount,
            AccountTo = customerAccount
          },
          // Incoming movement
          new Movement
          {
            MovementId = i * 9 + 1004,
            Account = customerAccount,
            MovementType = EnumMovementType.Interest,
            Amount = (decimal)500 + i,
            AccountFrom = customerNominatedAccount,
            AccountTo = customerAccount
          },
          // Outgoing movement
          new Movement
          {
            MovementId = i * 9 + 1005,
            Account = customerAccount,
            MovementType = EnumMovementType.Interest,
            Amount = (decimal)-700 - i,
            AccountFrom = customerAccount,
            AccountTo = customerNominatedAccount
          }
        };
        movements.AddRange(newMovements);
      }

      return movements;
    }
  }
}
