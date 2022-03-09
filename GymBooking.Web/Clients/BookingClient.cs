using GymBooking.Web.Models.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GymBooking.Web.Clients
{
    public class BookingClient : BaseClient
    {
        public BookingClient(HttpClient httpClient) : base(httpClient, new Uri(""), "application/json")
        { }

       
    }
}
