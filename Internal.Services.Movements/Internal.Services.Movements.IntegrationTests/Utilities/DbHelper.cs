using Internal.Services.Movements.Data.Contexts;

namespace Internal.Services.Movements.IntegrationTests.Utilities
{
    public class DbHelper
	{
		private readonly MovementsDataContext _movementsDb;

		public DbHelper(MovementsDataContext movementsDb)
		{
			_movementsDb = movementsDb;
		}

		public void InitializeDbForTests()
		{

		}
	}
}
