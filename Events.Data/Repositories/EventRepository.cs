using Events.Core.Entities;
using Events.Core.Paging;
using Events.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Data.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventsAPIContext db;

        public EventRepository(EventsAPIContext db)
        {
            this.db = db;
        }

        public async Task<PagingResult<CodeEvent>> GetAsync(bool includeLectures, PagingParams pagingParams)
        {

            return includeLectures ? await PagingResult<CodeEvent>.CreateAsync(db.CodeEvent.Include(c => c.Location)
                                                                                            .Include(c => c.Lectures)
                                                                                            .OrderBy(c => c.Name), pagingParams) :
                                    
                                      await PagingResult<CodeEvent>.CreateAsync(db.CodeEvent.Include(c => c.Location)
                                                                                              .OrderBy(c => c.Name), pagingParams);       
                                                       
        }


        public async Task<CodeEvent> GetAsync(string name, bool includeLectures)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));

            var query = db.CodeEvent
                          .Include(c => c.Location)
                          .AsQueryable();

            if (includeLectures)
                query = query.Include(c => c.Lectures);

            return await query.FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task AddAsync(CodeEvent codeEvent)
        {
            await db.AddAsync(codeEvent);
        }
    }
}
