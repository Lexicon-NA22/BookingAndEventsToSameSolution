using Events.Core.Dtos;

namespace GymBooking.Web.Clients
{
    public interface IBookingClient
    {
        Task<IEnumerable<CodeEventDto>> GetAllAsync(CancellationToken token);
        Task<CodeEventDto> GetAsync(CancellationToken token, string name);
    }
}