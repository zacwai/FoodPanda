using System;

namespace Panda.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; } = "";

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrMsg { get; set; } = "";
    }
}
