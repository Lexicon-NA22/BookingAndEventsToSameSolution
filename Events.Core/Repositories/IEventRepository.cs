using Events.Core.Entities;
using Events.Core.Paging;

namespace Events.Core.Repositories
{
    public interface IEventRepository
    {
        Task AddAsync(CodeEvent codeEvent);
        Task<PagingResult<CodeEvent>> GetAsync(bool includeLectures, Paging.PagingParams pagingParams);
        Task<CodeEvent> GetAsync(string name, bool includeLectures);
    }
}