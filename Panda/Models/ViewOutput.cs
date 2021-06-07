using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Models
{
    public class ViewOutput
    {
        public string ErrMsg { get; set; } = "";
        public string WarningMsg { get; set; } = "";
        public string Role { get; set; } = "admin";
        public object Result { get; set; } = null;

        public ViewOutput(string _role)
        {
            Role = _role;
        }
        public void SetError (string error)
        {
            ErrMsg = error;
        }
        public void SetWarning(string warning)
        {
            WarningMsg = warning;
        }
        public void SetResult(object result)
        {
            Result = result;
        }
    }
}
