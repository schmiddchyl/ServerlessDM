using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealtDataBFF.Models
{
    public interface ICode
    {
        /// <summary>
        /// Code value
        /// </summary>
        string code { get; }

        /// <summary>
        /// The name as stored in the system
        /// </summary>
        string systemName { get; }
    }
}
