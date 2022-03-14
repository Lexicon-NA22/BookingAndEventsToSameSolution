namespace Events.Core.Repositories
{
    public interface IUnitOfWork
    {
        IEventRepository EventRepo { get; }

        Task CompleteAsync();
    }
}