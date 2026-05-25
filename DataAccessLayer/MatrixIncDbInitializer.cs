using DataAccessLayer.Models;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class MatrixIncDbInitializer
    {
        public static void Initialize(MatrixIncDbContext context)
        {
            // Look for any customers.
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            // TODO: Hier moet ik nog wat namen verzinnen die betrekking hebben op de matrix.
            // - Denk aan de m3 boutjes, moertjes en ringetjes.
            // - Denk aan namen van schepen
            // - Denk aan namen van vliegtuigen            
            var customers = new Customer[]
            {
                new Customer { Name = "Noah", Address = "123 Elm St" , Active=true},
                new Customer { Name = "Gebruiker 1", Address = "456 Oak St", Active = true },
                new Customer { Name = "Gebruiker 2", Address = "789 Pine St", Active = true }
            };
            context.Customers.AddRange(customers);

            var orders = new Order[]
            {
                new Order { Customer = customers[0], OrderDate = DateTime.Parse("2021-01-01")},
                new Order { Customer = customers[0], OrderDate = DateTime.Parse("2021-02-01")},
                new Order { Customer = customers[1], OrderDate = DateTime.Parse("2021-02-01")},
                new Order { Customer = customers[2], OrderDate = DateTime.Parse("2021-03-01")}
            };  
            context.Orders.AddRange(orders);

            var products = new Product[]
            {
                new Product { Name = "Nebuchadnezzar", Description = "Het schip waarop Neo voor het eerst de echte wereld leert kennen", Price = 10000.00m, MainImageUrl = "/images/products/nebuchadnezzar.jpg" },
                new Product { Name = "Jack-in Chair", Description = "Stoel met een rugsteun en metalen armen waarin mensen zitten om ingeplugd te worden in de Matrix via een kabel in de nekpoort", Price = 500.50m, MainImageUrl = "/images/products/jack-in-chair.jpg" },
                new Product { Name = "EMP (Electro-Magnetic Pulse) Device", Description = "Wapentuig op de schepen van Zion", Price = 129.99m, MainImageUrl = "/images/products/emp-device.jpg" }
            };
            context.Products.AddRange(products);

            var parts = new Part[]
            {
                new Part { Name = "Tandwiel", Description = "Overdracht van rotatie in bijvoorbeeld de motor of luikmechanismen"},
                new Part { Name = "M5 Boutje", Description = "Bevestiging van panelen, buizen of interne modules"},
                new Part { Name = "Hydraulische cilinder", Description = "Openen/sluiten van zware luchtsluizen of bewegende onderdelen"},
                new Part { Name = "Koelvloeistofpomp", Description = "Koeling van de motor of elektronische systemen."}
            };
            context.Parts.AddRange(parts);

            var productImages = new ProductImage[]
            {
                new ProductImage { Product = products[0], ImageUrl = "/images/products/nebuchadnezzar.jpg", AltText = "Nebuchadnezzar hoofdafbeelding"},
                new ProductImage { Product = products[0], ImageUrl = "/images/products/nebuchadnezzar-2.jpg", AltText = "Nebuchadnezzar extra afbeelding"},
                new ProductImage { Product = products[1], ImageUrl = "/images/products/jack-in-chair.jpg", AltText = "Jack-in Chair hoofdafbeelding"},
                new ProductImage { Product = products[2], ImageUrl = "/images/products/emp-device.jpg", AltText = "EMP Device hoofdafbeelding"}
            };
            context.ProductImages.AddRange(productImages);

            var productSpecifications = new ProductSpecification[]
            {
                new ProductSpecification { Product = products[0], SpecName = "Type", SpecValue = "Hovercraft"},
                new ProductSpecification { Product = products[0], SpecName = "Gebruik", SpecValue = "Transport"},
                new ProductSpecification { Product = products[1], SpecName = "Materiaal", SpecValue = "Metaal en leer"},
                new ProductSpecification { Product = products[2], SpecName = "Categorie", SpecValue = "Verdediging"}
            };
            context.ProductSpecifications.AddRange(productSpecifications);


            context.SaveChanges();

            context.Database.EnsureCreated();
        }
    }
}
