using Microsoft.AspNetCore.Builder;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace TeamSite.Models
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AADbContext context = app.ApplicationServices.GetRequiredService<AADbContext>();

            if (!context.Clients.Any())
            {
                context.AddRange(
                    new Client
                    {
                        EcoSureClientID = 1,
                        ClientName = "ABC Television Network"
                    },
                    new Client
                    {
                        EcoSureClientID = 2,
                        ClientName = "Applebee's"
                    },
                    new Client
                    {
                        EcoSureClientID = 4,
                        ClientName = "Arby's"
                    },
                    new Client
                    {
                        EcoSureClientID = 9,
                        ClientName = "Burger King"
                    },
                    new Client
                    {
                        ClientName = "Chick - fil - A, Inc."
                    },
                    new Client
                    {
                        ClientName = "Club Corp"
                    },
                    new Client
                    {
                        ClientName = "Coca - Cola Company"
                    },
                    new Client
                    {
                        ClientName = "Darden Restaurants, Inc."
                    },
                    new Client
                    {
                        ClientName = "EcoSure"
                    },
                    new Client
                    {
                        ClientName = "Eggland's Best"
                    },
                    new Client
                    {
                        ClientName = "General Mills, Inc."
                    },
                    new Client
                    {
                        ClientName = "Hard Rock Cafe"
                    },
                    new Client
                    {
                        ClientName = "Hardee's Food Systems, Inc."
                    },
                    new Client
                    {
                        ClientName = "International Dairy Queen, Inc. (IDQ)"
                    },
                    new Client
                    {
                        ClientName = "McDonalds corporation"
                    },
                    new Client
                    {
                        ClientName = "Select Restaurants, Inc."
                    },
                    new Client
                    {
                        ClientName = "Spirit Cruises, LLC"
                    },
                    new Client
                    {
                        ClientName = "ARAMARK"
                    },
                    new Client
                    {
                        ClientName = "Back Yard Burgers"
                    },
                    new Client
                    {
                        ClientName = "Brinker International Restaurants"
                    },
                    new Client
                    {
                        ClientName = "Bruegger's"
                    },
                    new Client
                    {
                        ClientName = "Chipotle Mexican Grill"
                    },
                    new Client
                    {
                        ClientName = "Fuddruckers"
                    },
                    new Client
                    {
                        ClientName = "Golden Corral"
                    },
                    new Client
                    {
                        ClientName = "HMSHost"
                    },
                    new Client
                    {
                        ClientName = "IHOP"
                    },
                    new Client
                    {
                        ClientName = "Krispy Kreme"
                    },
                    new Client
                    {
                        ClientName = "Pizza Hut"
                    },
                    new Client
                    {
                        ClientName = "Quiznos"
                    },
                    new Client
                    {
                        ClientName = "Ruby Tuesday Inc."
                    },
                    new Client
                    {
                        ClientName = "Starbucks"
                    },
                    new Client
                    {
                        ClientName = "Omni Hotels"
                    },
                    new Client
                    {
                        ClientName = "Whole Foods Market Inc."
                    },
                    new Client
                    {
                        ClientName = "Fairmont Hotels"
                    },
                    new Client
                    {
                        ClientName = "Wendy's"
                    },
                    new Client
                    {
                        ClientName = "Dunkin' Brands"
                    },
                    new Client
                    {
                        ClientName = "Togo's Eatery"
                    },
                    new Client
                    {
                        ClientName = "Carl's Jr."
                    },
                    new Client
                    {
                        ClientName = "Taco Bueno"
                    },
                    new Client
                    {
                        ClientName = "Red Lobster"
                    },
                    new Client
                    {
                        ClientName = "Popeyes Chicken &Biscuits"
                    },
                    new Client
                    {
                        ClientName = "Church's Chicken"
                    },
                    new Client
                    {
                        ClientName = "Little Caesars"
                    },
                    new Client
                    {
                        ClientName = "On the Border"
                    },
                    new Client
                    {
                        ClientName = "CBC Restaurant Corp."
                    },
                    new Client
                    {
                        ClientName = "Checkers Drive In Restaurants"
                    },
                    new Client
                    {
                        ClientName = "Outback Steakhouse Inc."
                    },
                    new Client
                    {
                        ClientName = "Fazoli's"
                    },
                    new Client
                    {
                        ClientName = "T.G.I.Friday's"
                    },
                    new Client
                    {
                        ClientName = "Hilton Hotels"
                    },
                    new Client
                    {
                        ClientName = "Hyatt"
                    },
                    new Client
                    {
                        ClientName = "Landry's Restaurants. Inc."
                    },
                    new Client
                    {
                        ClientName = "American Blue Ribbon Holdings"
                    },
                    new Client
                    {
                        ClientName = "Steak 'n Shake Co."
                    },
                    new Client
                    {
                        ClientName = "Luby's, Inc."
                    },
                    new Client
                    {
                        ClientName = "Hooters of America, LLC"
                    },
                    new Client
                    {
                        ClientName = "O'Charley's, Inc."
                    },
                    new Client
                    {
                        ClientName = "Dave & Buster's, Inc."
                    }
                );
                context.SaveChanges();
            }

            if (!context.Employees.Any())
            {
                context.AddRange(
                    new Employee
                    {
                        FirstName = "Steve",
                        LastName = "Steinberg",
                        Email = "Steven.Steinberg@ecolab.com",
                        Phone = "+1 (630) 305-2646",
                        Position = "Manager Operations"
                    },
                    new Employee
                    {
                        FirstName = "Amanda",
                        LastName = "Coulombe",
                        Email = "Amanda.Coulombe@ecolab.com",
                        Phone = "+1 (630) 305-1284",
                        Position = "Manager Operations"
                    },
                    new Employee
                    {
                        FirstName = "Eric",
                        LastName = "Hightower",
                        Email = "Eric.Hightower@ecolab.com",
                        Phone = "+1 (630) 305-2011",
                        Position = "Manager Operations"
                    },
                    new Employee
                    {
                        FirstName = "Steve",
                        LastName = "Hall",
                        Email = "Steve.Hall@ecolab.com",
                        Phone = "+1 (630) 305-1958",
                        Position = "Sr Manager Operations"
                    },
                    new Employee
                    {
                        FirstName = "Amy",
                        LastName = "Fumagalli",
                        Email = "Amy.Fumagalli@ecolab.com",
                        Phone = "+1 (630) 305-2010",
                        Position = "Operations Specialist",

                    },
                    new Employee
                    {
                        FirstName = "Maura",
                        LastName = "Mulligan",
                        Email = "Maura.Mulligan@ecolab.com",
                        Phone = "+1 (630) 305-2932",
                        Position = "Operations Specialist",
                    },
                    new Employee
                    {
                        FirstName = "Kelly",
                        LastName = "Moser",
                        Email = "Kelly.Moser@ecolab.com",
                        Phone = "+1 (630) 364-3525",
                        Position = "Operations Specialist",
                    },
                    new Employee
                    {
                        FirstName = "Alyssa",
                        LastName = "Krause",
                        Email = "alyssa.krause@ecolab.com",
                        Phone = "+1 (630) 305-1391",
                        Position = "Operations Specialist",
                    },
                    new Employee
                    {
                        FirstName = "Jennifer",
                        LastName = "Cheang",
                        Email = "Jennifer.Cheang@ecolab.com",
                        Phone = "+1 (630) 305-2674",
                        Position = "Account Manager",
                    },
                    new Employee
                    {
                        FirstName = "Emily",
                        LastName = "Naufel",
                        Email = "Emily.Naufel@ecolab.com",
                        Phone = "+1 (330) 285-2132",
                        Position = "Account Manager",
                    },
                    new Employee
                    {
                        FirstName = "Maggie",
                        LastName = "Niewinski",
                        Email = "Magdalena.Niewinski@ecolab.com",
                        Phone = "+1 (630) 305-2630",
                        Position = "Operations Specialist"
                    },
                    new Employee
                    {
                        FirstName = "Maryhelen",
                        LastName = "Harkis",
                        Email = "Maryhelen.Harkis@ecolab.com",
                        Phone = "+1 (630) 305-1358",
                        Position = "Account Manager"
                    },
                    new Employee
                    {
                        FirstName = "Ellie",
                        LastName = "Lynch",
                        Email = "Eleanor.Lynch@ecolab.com",
                        Phone = "+1 (630) 305-2539",
                        Position = "Account Manager"
                    },
                    new Employee
                    {
                        FirstName = "Kelly",
                        LastName = "Evola",
                        Email = "Kelly.Evola@ecolab.com",
                        Phone = "+1 (630) 305-2831",
                        Position = "Operations Specialist"
                    },
                    new Employee
                    {
                        FirstName = "Kristin",
                        LastName = "Price",
                        Email = "Kristin.Price@ecolab.com",
                        Phone = "+1 (630) 305-1810",
                        Position = "Account Manager"
                    },
                    new Employee
                    {
                        FirstName = "Liddy",
                        LastName = "Ziulkowski",
                        Email = "Elizabeth.Ziulkowski@ecolab.com",
                        Phone = "+1 (630) 305-1260",
                        Position = "Operations Specialist"
                    },
                    new Employee
                    {
                        FirstName = "Jason",
                        LastName = "Ochs",
                        Email = "jason.ochs@ecolab.com",
                        Phone = "+1 (630) 305-1200",
                        Position = "Operations Specialist"
                    },
                    new Employee
                    {
                        FirstName = "Maggie",
                        LastName = "Stopka",
                        Email = "Margaret.Stopka@ecolab.com",
                        Phone = "+1 (630) 305-2647",
                        Position = "Operations Specialist"
                    },
                    new Employee
                    {
                        FirstName = "Joe",
                        LastName = "Reeder",
                        Email = "joseph.reeder@ecolab.com",
                        Phone = "+1 (630) 364-3518",
                        Position = "Operations Specialist"
                    },
                    new Employee
                    {
                        FirstName = "Renee",
                        LastName = "Hurd",
                        Email = "Renee.Hurd@ecolab.com",
                        Phone = "+1 (630) 305-2075",
                        Position = "Operations Specialist"
                    },
                    new Employee
                    {
                        FirstName = "Victoria",
                        LastName = "Bodish",
                        Email = "Victoria.Bodish@ecolab.com",
                        Phone = "+1 (630) 364-3504",
                        Position = "Account Manager"
                    },
                    new Employee
                    {
                        FirstName = "Christine",
                        LastName = "Cassa",
                        Email = "Christine.Cassa@ecolab.com",
                        Phone = "+1 (630) 305-2284",
                        Position = "Account Manager"
                    },
                    new Employee
                    {
                        FirstName = "Mark",
                        LastName = "Koladycz",
                        Email = "Mark.Koladycz@ecolab.com",
                        Phone = "+1 (630) 305-2130",
                        Position = "Account Manager"
                    },
                    new Employee
                    {
                        FirstName = "Steph",
                        LastName = "Chentorycki",
                        Email = "Stephanie.Chentorycki@ecolab.com",
                        Phone = "+1 (630) 305-2808",
                        Position = "Account Manager"
                    },
                    new Employee
                    {
                        FirstName = "Fred",
                        LastName = "Lutz",
                        Email = "Karl.Lutz@ecolab.com",
                        Phone = "+1 (630) 305-2327",
                        Position = "Account Manager"
                    },
                    new Employee
                    {
                        FirstName = "Brian",
                        LastName = "Hanus",
                        Email = "Brian.Hanus@ecolab.com",
                        Phone = "+1 (630) 305-2942",
                        Position = "Associate IT Business Process Analyst"
                    },
                    new Employee
                    {
                        FirstName = "Andrew",
                        LastName = "Durtka",
                        Email = "Andrew.Durtka@ecolab.com",
                        Phone = "+1 (630) 305-2162",
                        Position = "Associate IT Business Process Analyst"
                    },
                    new Employee
                    {
                        FirstName = "Meera",
                        LastName = "Gupta",
                        Email = "Meera.Gupta@ecolab.com",
                        Phone = "+1 (630) 305-2162",
                        Position = "Senoir Associate IT Business Process Analyst"
                    },
                    new Employee
                    {
                        FirstName = "Mike",
                        LastName = "O'Brien",
                        Email = "Michael.O'Brien@ecolab.com",
                        Phone = "+1 (630) 305-2532",
                        Position = "IT Business Process Analyst"
                    },
                    new Employee
                    {
                        FirstName = "Ayesha",
                        LastName = "Kazmi",
                        Email = "ayesha.kazmi@ecolab.com",
                        Phone = "+1 (630) 305-2395",
                        Position = "IT Contingent"
                    },
                    new Employee
                    {
                        FirstName = "Naveed",
                        LastName = "Jagirdar",
                        Email = "naveed.jagirdar@ecolab.com",
                        Phone = "+1 (630) 364-3515",
                        Position = "IT Contingent"
                    },
                    new Employee
                    {
                        FirstName = "Anthony",
                        LastName = "Pilipauskas",
                        Email = "anthony.pilipauskas@ecolab.com",
                        Phone = "+1 (630) 305-2722",
                        Position = "IT Contingent"
                    }
                );
                context.SaveChanges();
            }

            if (!context.AccountTeams.Any())
            {
                context.AddRange(
                    new AccountTeam
                    {
                        TeamName = "Team SouthEast"
                    },
                    new AccountTeam
                    {
                        TeamName = "Team Mountain"
                    },
                    new AccountTeam
                    {
                        TeamName = "Team Great Lakes"
                    },
                    new AccountTeam
                    {
                        TeamName = "Team Central"
                    },
                    new AccountTeam
                    {
                        TeamName = "Team NorthEast"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
