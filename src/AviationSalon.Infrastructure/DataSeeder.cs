﻿using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AviationSalon.Infrastructure
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataSeeder> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public DataSeeder(ApplicationDbContext context, ILogger<DataSeeder> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task SeedDataAsync()
        {
            _logger.LogInformation("Seeding data...");

            await _context.Database.MigrateAsync();

            _logger.LogInformation("Removing existing aircrafts...");
            _context.Aircrafts.RemoveRange(_context.Aircrafts);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Existing aircrafts removed.");

            if (!_context.Customers.Any())
            {
                _logger.LogInformation("Seeding customers...");

                var customers = new List<CustomerEntity>
            {
                new CustomerEntity
                {
                    CustomerId = "Customer1@gmail.com",
                    Name = "Customer1",
                    ContactInformation = "Contact1"
                },
                new CustomerEntity
                {
                    CustomerId = "CustomerId2",
                    Name = "Customer2",
                    ContactInformation = "Contact2"
                }
            };

                _context.Customers.AddRange(customers);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Seeded {customers.Count} customers.");
            }
            else
            {
                _logger.LogInformation("Customers already seeded.");
            }

            _logger.LogInformation("Removing existing aircrafts...");
            _context.Aircrafts.RemoveRange(_context.Aircrafts);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Existing aircrafts removed.");

            if (!_context.Aircrafts.Any())
            {
                _logger.LogInformation("Seeding aircrafts...");

                var aircrafts = new List<AircraftEntity>
                {
                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "MiG-29",
                        Range = 1430000,
                        MaxHeight = 18013,
                        Role = Role.Fighter,
                        MaxWeaponsCapacity = 6,
                        ImageFileName = "aircrafts/mig-29.jpg"
                    },

                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "Su-27",
                        Range = 3300000,
                        MaxHeight = 20000,
                        Role = Role.Fighter,
                        MaxWeaponsCapacity = 8,
                        ImageFileName = "aircrafts/su-27.jpg"
                    },

                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "Su-24",
                        Range = 1850000,
                        MaxHeight = 11000,
                        Role = Role.Bomber,
                        MaxWeaponsCapacity = 12,
                        ImageFileName = "aircrafts/su-24.jpg"
                    },

                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "Su-25",
                        Range = 750000,
                        MaxHeight = 5000,
                        Role = Role.CloseAirSupport,
                        MaxWeaponsCapacity = 10,
                        ImageFileName = "aircrafts/su-25.jpg"
                    },

                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "F-16",
                        Range = 4220000,
                        MaxHeight = 15240,
                        Role = Role.Multirole,
                        MaxWeaponsCapacity = 8,
                        ImageFileName = "aircrafts/f-16.jpg"
                    },                  
                    
                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "L-39",
                        Range = 900,
                        MaxHeight = 11000,
                        Role = Role.Trainer,
                        MaxWeaponsCapacity = 3, 
                        ImageFileName = "aircrafts/l-39.jpg"
                    },

                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "MiG-29",
                        Range = 1430000,
                        MaxHeight = 18013,
                        Role = Role.Fighter,
                        MaxWeaponsCapacity = 6,
                        ImageFileName = "aircrafts/mig-29-1.jpg"
                    },

                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "Su-27",
                        Range = 3300000,
                        MaxHeight = 20000,
                        Role = Role.Fighter,
                        MaxWeaponsCapacity = 8,
                        ImageFileName = "aircrafts/su-27-1.jpg"
                    },

                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "Su-24",
                        Range = 1850000,
                        MaxHeight = 11000,
                        Role = Role.Bomber,
                        MaxWeaponsCapacity = 12,
                        ImageFileName = "aircrafts/su-24-1.jpg"
                    },

                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "Su-25",
                        Range = 750000,
                        MaxHeight = 5000,
                        Role = Role.CloseAirSupport,
                        MaxWeaponsCapacity = 10,
                        ImageFileName = "aircrafts/su-25-1.jpg"
                    },

                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "F-16",
                        Range = 4220000,
                        MaxHeight = 15240,
                        Role = Role.Multirole,
                        MaxWeaponsCapacity = 8,
                        ImageFileName = "aircrafts/f-16-1.jpg"
                    },

                    new AircraftEntity
                    {
                        AircraftId = Guid.NewGuid().ToString(),
                        Model = "L-39",
                        Range = 900,
                        MaxHeight = 11000,
                        Role = Role.Trainer,
                        MaxWeaponsCapacity = 3,
                        ImageFileName = "aircrafts/l-39-1.jpg"
                    },
                };

                _context.Aircrafts.AddRange(aircrafts);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Seeded {aircrafts.Count} aircrafts.");
            }
            else
            {
                _logger.LogInformation("Aircrafts already seeded.");
            }

            _logger.LogInformation("Removing existing weapons...");
            _context.Weapons.RemoveRange(_context.Weapons);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Existing weapons removed.");

            if (!_context.Weapons.Any())
            {
                _logger.LogInformation("Seeding weapons...");

                var weapons = new List<WeaponEntity>
                {
                    new WeaponEntity
                    {
                        WeaponId = Guid.NewGuid().ToString(),
                        Name = "Central Fuel Tank",
                        Type = WeaponType.AirToGround,
                        GuidedSystem = GuidedSystemType.None,
                        Range = 0, 
                        FirePower = 200 
                    },

                    new WeaponEntity
                    {
                        WeaponId = Guid.NewGuid().ToString(),
                        Name = "Wing Fuel Tank",
                        Type = WeaponType.AirToGround,
                        GuidedSystem = GuidedSystemType.None,
                        Range = 0, 
                        FirePower = 150 
                    },

                    new WeaponEntity
                    {
                        WeaponId = Guid.NewGuid().ToString(),
                        Name = "Conformal Fuel Tank",
                        Type = WeaponType.AirToGround,
                        GuidedSystem = GuidedSystemType.None,
                        Range = 0,
                        FirePower = 100
                    },

                    new WeaponEntity
                    {
                        WeaponId = Guid.NewGuid().ToString(),
                        Name = "AIM-120 AMRAAM",
                        Type = WeaponType.AirToAir,
                        GuidedSystem = GuidedSystemType.ActiveRadar,
                        Range = 180000,
                        FirePower = 150,
                    },

                    new WeaponEntity
                    {
                        WeaponId = Guid.NewGuid().ToString(),
                        Name = "AGM-88 HARM",
                        Type = WeaponType.AntiRadiation,
                        GuidedSystem = GuidedSystemType.PassiveRadar,
                        Range = 150000,
                        FirePower = 250,
                    },

                    new WeaponEntity
                    {
                        WeaponId = Guid.NewGuid().ToString(),
                        Name = "R-77 (AA-12 Adder)",
                        Type = WeaponType.AirToAir,
                        GuidedSystem = GuidedSystemType.ActiveRadar,
                        Range = 190000,
                        FirePower = 120,
                    },

                    new WeaponEntity
                    {
                        WeaponId = Guid.NewGuid().ToString(),
                        Name = "AIM-9 Sidewinder",
                        Type = WeaponType.AirToAir,
                        GuidedSystem = GuidedSystemType.Infrared,
                        Range = 22000,
                        FirePower = 60,
                    },

                    new WeaponEntity
                    {
                        WeaponId = Guid.NewGuid().ToString(),
                        Name = "R-73",
                        Type = WeaponType.AirToAir,
                        GuidedSystem = GuidedSystemType.Infrared,
                        Range = 30000,
                        FirePower = 50,
                    },

                    new WeaponEntity
                    {
                        WeaponId = Guid.NewGuid().ToString(),
                        Name = "GBU-12 Paveway II",
                        Type = WeaponType.AirToGround,
                        GuidedSystem = GuidedSystemType.Laser,
                        Range = 12000,
                        FirePower = 250,
                    },
                };

                _context.Weapons.AddRange(weapons);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Seeded {weapons.Count} weapons.");
            }
            else
            {
                _logger.LogInformation("Weapons already seeded.");
            }

            _logger.LogInformation("Data seeding completed.");
        }

        public async Task InitializeUsersAsync()
        {
            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Seeding users...");

                var user = new IdentityUser { UserName = "Customer1@gmail.com" };
                var result = await _userManager.CreateAsync(user, "P@ssw0rd123");

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created successfully.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Error creating user: {error.Description}");
                    }
                }
            }
            else
            {
                _logger.LogInformation("Users already seeded.");
            }
        }
    }
}
