using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Restaurant.Core.Interfaces.IReposatory;
using Restaurant.Core.Models.Account;
using Restaurant.Infrastructure.Reposatory;
using Villa_API_Project.DataAccess.Data;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Villa_API_Project.DataAccess.Reposatory
{
    public class UnitOfWork:IUnitOfWork
    {
     

        public IAPPlicationUserReposatory User { get; private set; }

        public IRefreshTokenReposatory RefreshToken { get; private set; }

        public IRevokedTokensReposatory RevokedTokens { get; private set; }

        public IRestaurantRepo Restaurant { get; private set; }

        public IDishRepo Dish { get; private set; }

        private Context context;
        private UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _config;

        public UnitOfWork(Context context)
        {
            this.context = context;
          

            User = new ApplicationUserReposatory(context);
            RefreshToken = new RefreshTokenReposatory(context);

            RevokedTokens = new RevokedTokensReposatory(context);

            Restaurant = new RestaurantRepo(context);

            Dish = new DishRepo(context);
        }
        public void save()
        {
            context.SaveChanges();
        }
    }
}
