using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.BLL.Services
{
    public class ServiceResponse<T>
    {
        public ServiceResponse() { }

        public ServiceResponse(T data, string message = "", bool isSuccess = true)
        {
            Data = data;
            Message = message;
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public string? JwtToken { get; set; }
    }
}

