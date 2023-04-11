using System;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using static Api.Helper.Enums;

namespace Api.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DBContextModel(
                serviceProvider.GetRequiredService<DbContextOptions<DBContextModel>>()))
            {
                if (context.Orders.Any())
                {
                    // DB has already been seeded
                    return;
                }

                context.Orders.AddRange(
                    new Order
                    {
                        OrderCode = "ORD001",
                        Status = OrderStatus.New,
                        DateCreated = DateTime.UtcNow.AddDays(-3)
                    },
                    new Order
                    {
                        OrderCode = "ORD002",
                        Status = OrderStatus.InProgress,
                        DateCreated = DateTime.UtcNow.AddDays(-2)
                    },
                    new Order
                    {
                        OrderCode = "ORD003",
                        Status = OrderStatus.InProgress,
                        DateCreated = DateTime.UtcNow.AddDays(-1)
                    },
                    new Order
                    {
                        OrderCode = "ORD004",
                        Status = OrderStatus.Delivered,
                        DateCreated = DateTime.UtcNow.AddDays(-5)
                    },
                    new Order
                    {
                        OrderCode = "ORD005",
                        Status = OrderStatus.Delivered,
                        DateCreated = DateTime.UtcNow.AddDays(-4)
                    },
                    new Order
                    {
                        OrderCode = "ORD006",
                        Status = OrderStatus.Cancelled,
                        DateCreated = DateTime.UtcNow.AddDays(-6)
                    },
                    new Order
                    {
                        OrderCode = "ORD007",
                        Status = OrderStatus.Cancelled,
                        DateCreated = DateTime.UtcNow.AddDays(-7)
                    },
                    new Order
                    {
                        OrderCode = "ORD008",
                        Status = OrderStatus.New,
                        DateCreated = DateTime.UtcNow.AddDays(-8)
                    },
                    new Order
                    {
                        OrderCode = "ORD009",
                        Status = OrderStatus.New,
                        DateCreated = DateTime.UtcNow.AddDays(-9)
                    },
                    new Order
                    {
                        OrderCode = "ORD010",
                        Status = OrderStatus.InProgress,
                        DateCreated = DateTime.UtcNow.AddDays(-10)
                    });

                context.SaveChanges();
            }
        }
    }
}
