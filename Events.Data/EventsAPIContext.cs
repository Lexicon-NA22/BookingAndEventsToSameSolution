#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Events.Core.Entities;

namespace Events.Data
{
    public class EventsAPIContext : DbContext
    {
        public DbSet<CodeEvent> CodeEvent { get; set; }
        public DbSet<Lecture> Lecture { get; set; }
        public EventsAPIContext(DbContextOptions<EventsAPIContext> options)
            : base(options)
        {
        }

    }
}
