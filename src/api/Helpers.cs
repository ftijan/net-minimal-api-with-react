using Example.Api.Data;

namespace Example.Api
{
	internal static class Helpers
	{
		internal static void SeedData(this WebApplication app)
		{
			var scope = app.Services.CreateScope();
			var db = scope.ServiceProvider.GetService<PizzaContext>();

			var pizzas = new List<Pizza>() 
			{ 
				new Pizza 
				{
					Id = 1,
					Name = "Margherita",
					Description = "Tomato sauce, mozzarella, and basil",
				},
				new Pizza 
				{
					Id = 2,
					Name = "Pepperoni",
					Description = "Tomato sauce, mozzarella, and pepperoni",
				},
				new Pizza 
				{
					Id = 3,
					Name = "Hawaiian",
					Description = "Tomato sauce, mozzarella, ham, and pineapple",
				}
			};			

			db.Pizzas.AddRange(pizzas);

			db.SaveChanges();
		}
	}
}
