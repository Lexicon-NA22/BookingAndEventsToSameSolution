using Microsoft.AspNetCore.Http;

namespace GymBooking.Web.Extensions
{
    public static class RequestExtensions
    {
        public static bool IsAjax(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
