using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using core.Enum;
using core.Util;
using core.TestData;
using core.Common.Feature;
using NUnit.Framework;
using core.Common.Request.Transfer;

[assembly:LevelOfParallelism(3)]

namespace core
{
    
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public sealed class TestExample
    {
        DataTable data;
        readonly Dictionary<int, bool> testData = new Dictionary<int, bool>();

        private static readonly string parameter1 = "Some Value";
        private static readonly string parameter2 = "Some Value";

        [OneTimeSetUp]
        public void GetTestData()
        {
            var SqlParameters = new Dictionary<string, object>
            {
                { "@parameter1", parameter1 },
                { "@parameter2", parameter2 }
            };

            data = new DBHelper().ExecuteSQLQueryWithResult(QueryStorage.QUERY_2, SqlParameters);

            foreach (DataRow dr in data.Rows)
            {
                if (dr["someField1"].Equals(true) && dr["someField2"].Equals("smth"))
                    testData.Add(Convert.ToInt32(dr["id"]), false);
            }

            if (testData.Count.Equals(0))
                throw new NoTestDataFoundException("No items found");
        }

        private int GetTestData(Dictionary<int, bool> data)
        {
            int id = default;

            lock (data)
            {
                id = data.First(x => x.Value.Equals(false)).Key;
                data[id] = true;

                if (id == default)
                    throw new Exception("No suitable id found");
                
                return id;
            }
        }

        [Test]
        [Category("Integration")]
        [TestCaseSource(typeof(TransferCases), "TestCases")]
        public void TestExample1(TransferCreation initialRequestData)
        {
            initialRequestData.ItemsToTransfer = new List<int>() { GetTestData(testData) };

            var transfer = new Transfer();
            transfer.Initialize(initialRequestData)
                .Create()
                .SetRequestNumber()
                .ValidateResponse(FlowActions.CREATE)
                .Accept()
                .ValidateResponse(FlowActions.ACCEPT);
        }
    }
}