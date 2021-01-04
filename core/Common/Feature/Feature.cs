using RestSharp;
using core.Util;

namespace core.Common.Feature
{
    public abstract class Feature
    {
        public RestHelper RestClient => new RestHelper();
        public IRestResponse RestResponse { get; set; }
        public DBHelper DBHelper => new DBHelper();
        public AssertionHelper AssertionHelper => new AssertionHelper();
        public FileHelper FileHelper => new FileHelper();
    }
}