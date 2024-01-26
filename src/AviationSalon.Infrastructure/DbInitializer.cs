using AviationSalon.Core.Abstractions;
using AviationSalon.Core.Abstractions.Repositories;
using AviationSalon.Core.Data.Entities;
using AviationSalon.Core.Data.Enums;
using AviationSalon.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        public DbInitializer(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeAsync()
        {
            await _context.Database.MigrateAsync();

            await SeedWeaponDataAsync();
            await SeedAircraftDataAsync();
        }
        private async Task SeedWeaponDataAsync()
        {
            if (!_context.Weapons.Any())
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    try
                    {
                        var weaponRepository = scopedServices.GetRequiredService<IRepository<WeaponEntity>>();
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

        private async Task SeedAircraftDataAsync()
        {
            if (!_context.Aircrafts.Any())
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    try
                    {
                        var aircraftRepository = scopedServices.GetRequiredService<IRepository<AircraftEntity>>();
                        var aircrafts = new List<AircraftEntity>
                        {
                            new AircraftEntity
                            {
                                Model = "MiG-29",
                                Range = 1430000,
                                MaxHeight = 18013,
                                Role = Role.Fighter,
                                MaxWeaponsCapacity = 6,
                            },
                            new AircraftEntity
                            {
                                Model = "Su-27",
                                Range = 3300000,
                                MaxHeight = 20000,
                                Role = Role.Fighter,
                                MaxWeaponsCapacity = 8,
                            },
                            new AircraftEntity
                            {
                                Model = "Su-24",
                                Range = 1850000,
                                MaxHeight = 11000,
                                Role = Role.Bomber,
                                MaxWeaponsCapacity = 12,
                            },
                            new AircraftEntity
                            {
                                Model = "Su-25",
                                Range = 750000,
                                MaxHeight = 5000,
                                Role = Role.CloseAirSupport,
                                MaxWeaponsCapacity = 10,
                            },
                            new AircraftEntity
                            {
                                Model = "F-16",
                                Range = 4220000,
                                MaxHeight = 15240,
                                Role = Role.Multirole,
                                MaxWeaponsCapacity = 8,
                            },
                        };                  


                        foreach (var aircraft in aircrafts)
                        {
                            await aircraftRepository.AddAsync(aircraft);
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

}
