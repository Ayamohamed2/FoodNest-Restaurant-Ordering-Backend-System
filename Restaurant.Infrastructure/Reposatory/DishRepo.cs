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
    public class DishRepo :Reposatory<Restaurant.Core.Entity.Restaurant.Dish>,IDishRepo
    {
        Context Context;
        public DishRepo(Context context) : base(context)
        {
            this.Context = context;

        }
    }
}
