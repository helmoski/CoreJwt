using System;

namespace CoreJwt
{
    public class TamperingException : Exception
    {
        private const string DEFAULT_MSG = "JWT shows signs of tampering.";

        public TamperingException() : base(DEFAULT_MSG)
        {

        }
    }
}
