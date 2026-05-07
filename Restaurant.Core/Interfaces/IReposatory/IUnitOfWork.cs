using Restaurant.Core.Interfaces.IReposatory;

namespace Villa_API_Project.DataAccess.Reposatory.IReposatory
{
    public interface IUnitOfWork
    {


        public IAPPlicationUserReposatory User { get; }
        public IRefreshTokenReposatory RefreshToken { get;  }
        public IRevokedTokensReposatory RevokedTokens { get; }

        public IRestaurantRepo Restaurant { get; }
        public IDishRepo Dish { get; }

        void save();
    }
}
