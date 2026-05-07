using Restaurant.Core.Interfaces.IReposatory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Data;
using Villa_API_Project.DataAccess.Reposatory;

namespace Restaurant.Infrastructure.Reposatory
{
    public class RestaurantRepo :Reposatory<Restaurant.Core.Entity.Restaurant.Restaurant>,IRestaurantRepo
    {
        Context Context;
        public RestaurantRepo(Context context) : base(context)
        {
            this.Context = context;

        }
    }
}
