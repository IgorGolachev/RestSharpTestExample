using Newtonsoft.Json;
using FluentAssertions;

namespace core.Util
{
    public sealed class AssertionHelper
    {
        public void AssertResponsesByPayLoad<T>(string expectedJson, string actualJson) where T : class
        {
            var expected = JsonConvert.DeserializeObject<T>(expectedJson);
            var actual = JsonConvert.DeserializeObject<T>(actualJson);
            actual.Should().BeEquivalentTo(expected);
        }
    }
}