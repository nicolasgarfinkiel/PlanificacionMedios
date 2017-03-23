using System.Collections.Generic;

namespace Irsa.PDM.Dtos.Common
{
    public class Result
    {
        public bool HasErrors { get; set; }
        public IList<string> Messages { get; set; }
    }
}
