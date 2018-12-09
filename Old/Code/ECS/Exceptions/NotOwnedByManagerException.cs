using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.Exceptions
{
    class NotOwnedByManagerException : Exception
    {
        public NotOwnedByManagerException(string message) : base(message) { }
    }
}
