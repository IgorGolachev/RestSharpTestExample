using core.Common.Request.Transfer;
using System.Collections;

namespace core.TestData
{
    public sealed class TransferCases
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TransferCreation()
                {
                };
            }
        }
    }
}