using NLog;
using NUnit.Framework;

namespace core.Util
{
    public sealed class Logger
    {
        public static NLog.Logger Log =>
            LogManager.GetLogger(TestContext.CurrentContext.Test.FullName);
    }
}