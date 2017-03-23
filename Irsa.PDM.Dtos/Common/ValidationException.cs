using System;

namespace Irsa.PDM.Dtos.Common
{
    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : base(message)
        {

        }
    }
}
