
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncountersByPatientSearch.Models
{
    public sealed class EncounterCollection : IEncounterCollection
    {
       
        public EncounterCollection(List<IEncounter> encounters)
        {
            this.items = encounters;
        }

        public IEnumerable<IEncounter> items { get; init; }
    }
}
