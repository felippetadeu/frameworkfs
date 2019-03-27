using System.Net;

namespace Framework.HttpResult
{
    public class Result
    {
        public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; }
        public dynamic Value { get; set; }

        public Result(string message = "", HttpStatusCode code = HttpStatusCode.OK, dynamic value = null, dynamic ex = null, int? userId = null)
        {
            Code = code;
            Message = message;
            Value = value;

            if (Code == HttpStatusCode.InternalServerError)
            {
                SandboxError(ex, userId.Value, value);
            }
        }

        private void SandboxError(dynamic ex, int userId, dynamic value)
        {
            
        }
    }
}
