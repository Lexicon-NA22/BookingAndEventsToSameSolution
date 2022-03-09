using Events.Core.Dtos;
using GymBooking.Web.Models.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GymBooking.Web.Clients
{
    public class BookingClient : BaseClient, IBookingClient
    {
        public BookingClient(HttpClient httpClient) : base(httpClient, new Uri("https://localhost:5001"), "application/json")
        { }

        public async Task<IEnumerable<CodeEventDto>> GetAllAsync(CancellationToken token)
        {
            return await base.GetWithStreamsAsync<IEnumerable<CodeEventDto>>(token, "api/events?includeLectures=true");
        }

        public async Task<CodeEventDto> GetAsync(CancellationToken token, string name)
        {
            return await base.GetWithStreamsAsync<CodeEventDto>(token, $"api/events/{name}");
        }


    }
}
