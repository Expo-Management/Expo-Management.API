using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class KindEvents
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Primary { get; set; } = default!;
        public string Secondary { get; set; } = default!;
    }
}
