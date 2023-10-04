using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealtDataBFF.Models
{
    public interface IEncounterCollection
    {
        /// <summary>
        /// A collection of <see cref="IEncounter"/>.
        /// </summary>
        /// <value>A collection of <see cref="IEncounter"/>.</value>
        IEnumerable<IEncounter> items { get; }
    }
}
