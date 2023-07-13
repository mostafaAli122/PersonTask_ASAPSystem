using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inputs
{
    public class CreatePersonInput
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int AddressId { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }

    }
}
