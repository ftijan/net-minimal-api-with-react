using Example.Api;
using Example.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

const string CorsAllowOrigins = "CorsAllowOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzaContext>(options => options.UseInMemoryDatabase("PizzaDB"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{ 
		Title = "Pizzas API",
		Description = "Pizza pizza",
		Version = "v1"
	});
});

builder.Services.AddCors(options => 
{
	options.AddPolicy(name: CorsAllowOrigins, builder =>
	{
		builder.WithOrigins("http://localhost/"); // Allow localhost only
	});
});

var app = builder.Build();

app.SeedData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(setup => 
	{
		setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Pizzas API V1");
	});
}

app.UseCors(CorsAllowOrigins);

app.MapGet("/", () => "Hello World!");

app.MapGet("/pizzas", async(PizzaContext db) => await db.Pizzas.ToListAsync());

app.MapPost("/pizzas", async (PizzaContext db, Pizza pizza) => 
{
	await db.Pizzas.AddAsync(pizza);
	await db.SaveChangesAsync();
	return Results.Created($"/pizzas/{pizza.Id}", pizza);
});

app.MapPut("/pizzas/{id}", async (PizzaContext db, Pizza updatePizza, int id) => 
{
	var pizzaItem = await db.Pizzas.FindAsync(id);
	if (pizzaItem == null) return Results.NotFound();

	pizzaItem.Name = updatePizza.Name;
	pizzaItem.Description = updatePizza.Description;
	await db.SaveChangesAsync();
	return Results.NoContent();
});

app.MapDelete("/pizzas/{id}", async (PizzaContext db, int id) => 
{
	var todo = await db.Pizzas.FindAsync(id);
	if(todo is null) return Results.NotFound();

	db.Pizzas.Remove(todo);
	await db.SaveChangesAsync();
	return Results.Ok();
});

app.Run();

