using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Exceptions
{
    public class InvalidScoreException : Exception
    {
        public InvalidScoreException()
        {
        }

        public InvalidScoreException(string message) : base(message)
        {
        }

        public InvalidScoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
