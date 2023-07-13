using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inputs
{
    public class CreateAddressInput
    {
        public int AddressId { get;  set; }
        public string StreetAdress { get;  set; }
        public string ZipCode { get;  set; }
        public int CityId { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
    }
}
