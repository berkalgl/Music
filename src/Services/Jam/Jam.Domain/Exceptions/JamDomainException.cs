using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jam.Domain.Exceptions
{
    public class JamDomainException : Exception
    {
        public JamDomainException()
        { }

        public JamDomainException(string message)
            : base(message)
        { }

        public JamDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
