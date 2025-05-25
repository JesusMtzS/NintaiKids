using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nintai_kids.Models
{
    /// <summary>
    ///     Exception thrown when a function call is invalid
    /// </summary>
    internal class InvalidFunctionCallException : Exception
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="InvalidFunctionCallException" /> with the provided message
        /// </summary>
        public InvalidFunctionCallException(string message) : base(message)
        {
        }
    }
}
