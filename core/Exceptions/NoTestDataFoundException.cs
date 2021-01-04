using NLog;
using System;

namespace core
{
    public class NoTestDataFoundException : Exception
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public NoTestDataFoundException(string message)
        {
            logger.Error(message);
        }
    }
}