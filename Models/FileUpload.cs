using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Models
{
    public class FileUpload
    {
        public string Id { get; set; }
        public IFormFile File { get; set; }
        public string Type { get; set; }
    }
}
