using System;

namespace CoreJwt
{
    public class ExpiredException : Exception
    {
        private const string DEFAULT_MSG = "JWT has expired.";

        public ExpiredException() : base(DEFAULT_MSG)
        {

        }
    }
}
