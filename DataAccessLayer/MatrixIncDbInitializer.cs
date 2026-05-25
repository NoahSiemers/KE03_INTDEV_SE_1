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
                new Product { Name = "Boormachine GBM 10 RE", Description = "Boormachine GBM 10 RE (Snelspanboorhouder 1 - 10 mm)", Price = 100.99m, MainImageUrl = "/images/products/Bosch.jfif" },
                new Product { Name = "JMV spaanplaatschroeven", Description = "JMV spaanplaatschroeven 4.0x16mm met verzonken kop - per 200 stuks (VK4016)", Price = 9.99m, MainImageUrl = "/images/products/Schroef.webp" },
                new Product { Name = "vidaXL Schep", Description = "vidaXL Schep Zwart 68,5 cm Krachtig gecoat staal en massief hout", Price = 24.99m, MainImageUrl = "/images/products/Schep.jpg" },
                new Product { Name = "Bosch PST 650 Decoupeerzaag", Description = "Bosch PST 650 Decoupeerzaag - op snoer - 500 W", Price = 59.99m, MainImageUrl = "/images/products/Zaag.jpg" },
                new Product { Name = "Kreator combinatietang", Description = "Kreator combinatietang basic 150mm met PVC handvat (KRT602001)", Price = 14.99m, MainImageUrl = "/images/products/Tang.jpg" },
                new Product { Name = "BGS Steekringsleutel 36 mm", Description = "BGS 1086 | Steekringsleutel | 36 mm", Price = 19.99m, MainImageUrl = "/images/products/Steekringsleutel.jpg" }
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
                new ProductImage { Product = products[0], ImageUrl = "/images/products/Bosch.jfif", AltText = "Boormachine GBM 10 RE hoofdafbeelding"},
                new ProductImage { Product = products[0], ImageUrl = "/images/products/Bosch1.jpg", AltText = "Boormachine GBM 10 RE extra afbeelding 1"},
                new ProductImage { Product = products[0], ImageUrl = "/images/products/Bosch2.jpg", AltText = "Boormachine GBM 10 RE extra afbeelding 2"},
                new ProductImage { Product = products[1], ImageUrl = "/images/products/Schroef.webp", AltText = "JMV spaanplaatschroeven hoofdafbeelding"},
                new ProductImage { Product = products[2], ImageUrl = "/images/products/Schep.jpg", AltText = "vidaXL Schep hoofdafbeelding"},
                new ProductImage { Product = products[3], ImageUrl = "/images/products/Zaag.jpg", AltText = "Bosch PST 650 Decoupeerzaag hoofdafbeelding"},
                new ProductImage { Product = products[4], ImageUrl = "/images/products/Tang.jpg", AltText = "Kreator combinatietang hoofdafbeelding"},
                new ProductImage { Product = products[5], ImageUrl = "/images/products/Steekringsleutel.jpg", AltText = "BGS Steekringsleutel hoofdafbeelding"}
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
