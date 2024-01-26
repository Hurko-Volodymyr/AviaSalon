using AviationSalon.Core.Abstractions;
using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviationSalon.Infrastructure
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(ApplicationDbContext context, IServiceProvider serviceProvider, ILogger<DbInitializer> logger)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }


        public async Task InitializeAsync()
        {
            _logger.LogInformation("Initializing the database...");

            await _context.Database.MigrateAsync();

            _logger.LogInformation("Database migration completed.");

            var seedWeaponDataTask = SeedWeaponDataAsync();
            var seedAircraftDataTask = SeedAircraftDataAsync();

            await Task.WhenAll(seedWeaponDataTask, seedAircraftDataTask);

            _logger.LogInformation("Database seeding completed.");
        }

        private async Task SeedWeaponDataAsync()
        {
            _logger.LogInformation("-------------------SeedWeaponDataAsync HERE-------------------");

            var weaponsExist = _context.Weapons.Any();
            _logger.LogInformation($"Check for existing weapons: {weaponsExist}");
            if (!_context.Weapons.Any())
            {
                    try
                    {
                        var weaponRepository = _serviceProvider.GetRequiredService<IRepository<WeaponEntity>>();
                        var weapons = new List<WeaponEntity>
                        {
                            new WeaponEntity
                            {
                                Name = "AIM-120 AMRAAM",
                                Type = WeaponType.AirToAir,
                                GuidedSystem = GuidedSystemType.ActiveRadar,
                                Range = 180000,
                                FirePower = 150,
                            },
                            new WeaponEntity
                            {
                                Name = "AGM-88 HARM",
                                Type = WeaponType.AntiRadiation,
                                GuidedSystem = GuidedSystemType.PassiveRadar,
                                Range = 150000,
                                FirePower = 250
                            },
                            new WeaponEntity
                            {
                                Name = "R-77 (AA-12 Adder)",
                                Type = WeaponType.AirToAir,
                                GuidedSystem = GuidedSystemType.ActiveRadar,
                                Range = 190000,
                                FirePower = 120,    
                            },
                            new WeaponEntity
                            {
                                Name = "AIM-9 Sidewinder",
                                Type = WeaponType.AirToAir,
                                GuidedSystem = GuidedSystemType.Infrared,
                                Range = 22000,
                                FirePower = 60
                            },
                            new WeaponEntity
                            {
                                Name = "R-73",
                                Type = WeaponType.AirToAir,
                                GuidedSystem = GuidedSystemType.Infrared,
                                Range = 30000,
                                FirePower = 50
                            },
                            new WeaponEntity
                            {
                                Name = "GBU-12 Paveway II",
                                Type = WeaponType.AirToGround,
                                GuidedSystem = GuidedSystemType.Laser,
                                Range = 12000,
                                FirePower = 250,
                            },
                        };

                        foreach (var weapon in weapons)
                        {
                            await weaponRepository.AddAsync(weapon);
                            _logger.LogInformation(weapon.Name);
                        }

                        await _context.SaveChangesAsync();

                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Error in SeedWeaponDataAsync: {ex.Message}");
                        throw;
                    };                
            }
            else 
            {
                _logger.LogInformation("No weapon was added");
            }
        }
        private async Task SeedAircraftDataAsync()
        {
            _logger.LogInformation("-------------------SeedAircraftDataAsync HERE-------------------");
            if (!_context.Aircrafts.Any())
            {
                    try
                    {
                        var aircraftRepository = _serviceProvider.GetRequiredService<IRepository<AircraftEntity>>();
                        var aircrafts = new List<AircraftEntity>
                        {
                            new AircraftEntity
                            {
                                Model = "MiG-29",
                                Range = 1430000,
                                MaxHeight = 18013,
                                Role = Role.Fighter,
                                MaxWeaponsCapacity = 6,
                                 ImageFileName = "aircrafts/mig-29.jpg"
                            },
                            new AircraftEntity
                            {
                                Model = "Su-27",
                                Range = 3300000,
                                MaxHeight = 20000,
                                Role = Role.Fighter,
                                MaxWeaponsCapacity = 8,
                                 ImageFileName = "aircrafts/su-27.jpg"
                            },
                            new AircraftEntity
                            {
                                Model = "Su-24",
                                Range = 1850000,
                                MaxHeight = 11000,
                                Role = Role.Bomber,
                                MaxWeaponsCapacity = 12,
                                 ImageFileName = "aircrafts/su-24.jpg"
                            },
                            new AircraftEntity
                            {
                                Model = "Su-25",
                                Range = 750000,
                                MaxHeight = 5000,
                                Role = Role.CloseAirSupport,
                                MaxWeaponsCapacity = 10,
                                 ImageFileName = "aircrafts/su-25.jpg"
                            },
                            new AircraftEntity
                            {
                                Model = "F-16",
                                Range = 4220000,
                                MaxHeight = 15240,
                                Role = Role.Multirole,
                                MaxWeaponsCapacity = 8,
                                 ImageFileName = "aircrafts/f-16.jpg"
                            },
                        };                  


                        foreach (var aircraft in aircrafts)
                        {
                            await aircraftRepository.AddAsync(aircraft);
                            _logger.LogInformation(aircraft.Model);
                        }

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
               
            }
        }
    }

}
