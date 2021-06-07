using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Models
{
    public class RegistrationInput
    {
        public string FullName { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string State { get; set; }
    }
}
