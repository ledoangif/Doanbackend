using System.Net;

namespace Doantour.Models
{
    public class ResponseFormat
    {


        #region Fields
        /// <summary>
        /// Thông tin mã code thành công
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Thông báo cho người dùng
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// ResponseData trả về
        /// </summary>
        public object? ResponseData { get; set; }

        #endregion

        #region Constructor
        public ResponseFormat() { }

        public ResponseFormat(HttpStatusCode statusCode, string? message, object? responseData)
        {
            StatusCode = statusCode;
            Message = message;
            ResponseData = responseData;

        }
        #endregion

    }
}

