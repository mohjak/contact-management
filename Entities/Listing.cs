using System.Collections.Generic;

namespace Mohjak.ContactManagement.Entities
{
    public class Listing : BaseEntity
    {
        public IList<string> Listings { get; set; } = new List<string>();

        public IList<string> Fields { get; set; } = new List<string>();
    }
}
