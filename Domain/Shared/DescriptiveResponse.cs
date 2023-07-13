using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    public class DescriptiveResponse<TResult>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public TResult Result { get; set; }
        public int ErrorCode { get; set; }
        public string Data { get; set; }
    }
}
