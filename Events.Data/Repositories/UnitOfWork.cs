using Events.Core.Repositories;
using Events.Data;

namespace Events.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private EventsAPIContext db;
        public IEventRepository EventRepo { get; private set; }

        public UnitOfWork(EventsAPIContext context)
        {
            this.db = context;
            EventRepo = new EventRepository(context);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}