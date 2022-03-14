using Bogus;
using Events.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Data
{
    public class SeedData
    {
        public static async Task InitializeAcync(IServiceProvider services)
        {

            using var db = new EventsAPIContext(services.GetRequiredService<DbContextOptions<EventsAPIContext>>());

            if (await db.CodeEvent.AnyAsync()) return;

            var faker = new Faker("sv");
            var events = new List<CodeEvent>();

            for (int i = 0; i < 50; i++)
            {
                events.Add(new CodeEvent
                {
                    Name = faker.Company.CompanySuffix() + faker.Random.Word(),
                    EventDate = DateTime.Now.AddDays(faker.Random.Int(-20, 20)),
                    Location = new Location()
                    {
                        Address = faker.Address.StreetAddress(),
                        CityTown = faker.Address.City(),
                        StateProvince = faker.Address.State(),
                        PostalCode = faker.Address.ZipCode(),
                        Country = faker.Address.Country()
                    },
                    Length = 1,
                    Lectures = new Lecture[]
                    {
                            new Lecture
                            {
                              Title = faker.Commerce.ProductName(),
                              Level = 100,
                              Speaker = new Speaker
                              {
                                FirstName = faker.Name.FirstName(),
                                LastName = faker.Name.LastName(),
                                BlogUrl = faker.Internet.Url(),
                                Company = faker.Company.CompanyName(),
                                CompanyUrl = faker.Internet.Url(),
                                GitHub = faker.Internet.Url()
                              }
                          },
                            new Lecture
                            {
                              Title = faker.Commerce.ProductName(),
                              Level = 100,
                              Speaker = new Speaker
                              {
                                FirstName = faker.Name.FirstName(),
                                LastName = faker.Name.LastName(),
                                BlogUrl = faker.Internet.Url(),
                                Company = faker.Company.CompanyName(),
                                CompanyUrl = faker.Internet.Url(),
                                GitHub = faker.Internet.Url()
                              }
                            }
                    }
                });

            }

            db.AddRange(events);
            await db.SaveChangesAsync();

        }
    }

}
