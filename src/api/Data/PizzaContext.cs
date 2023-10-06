﻿using Microsoft.EntityFrameworkCore;

namespace Example.Api.Data
{
	public class PizzaContext : DbContext
	{
        public PizzaContext(DbContextOptions<PizzaContext> options) : base(options)
        {
            
        }

        public DbSet<Pizza> Pizzas { get; set; }
    }
}
