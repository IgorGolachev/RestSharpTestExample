namespace core
{
    public static class QueryStorage
    {
        public const string QUERY_1 = @"SELECT something FROM [SomeView].View 
                                                    WHERE FullName = @parameter";

        public const string QUERY_2 = @"EXEC [db].[someStoredProcedure] @parameter";
    }
}