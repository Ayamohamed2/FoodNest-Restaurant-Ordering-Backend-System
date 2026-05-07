using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Restaurant.Core.Interfaces.IReposatory
{
    public interface IRestaurantRepo:IReposatory<Restaurant.Core.Entity.Restaurant.Restaurant>
    {
    }
}
