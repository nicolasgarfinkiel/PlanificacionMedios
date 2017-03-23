using System.Collections.Generic;

namespace Irsa.PDM.Dtos.Common
{
    public class Response<T>
    {
        public Response()
        {
            Result = new Result() {HasErrors = false, Messages = new List<string>()};
        }

        public Result Result { get; set; }
        public T Data { get; set; }
    }
}
