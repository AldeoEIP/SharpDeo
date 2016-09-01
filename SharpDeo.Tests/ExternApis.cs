using System;
using System.Net.Http;
using System.Threading.Tasks;
//using Xunit;

namespace SharpDeo.Tests {
    public class ExternApis {

        private static readonly Client Client = new Client();

        //[Theory]
        //[InlineData ("", "poney")]
        //[InlineData ("", "oignon")]
        public async Task TestDictionaryAccess(string expected, string word) {
            var actual = await Client.GetDictionaryDefintionAsync (word).ConfigureAwait (false);
            //Assert.Equal (expected, actual);
        }

        //[Theory]
        //[InlineData (4, "2+2")]
        //[InlineData (10, "10-5+2+3+21*2-42")]
        public async Task TestWolframAccess(int expected, string expression) {
            var x = await Client.ResolveAsync (expression).ConfigureAwait (false);
            var actual = Convert.ToInt32(x);
            //Assert.Equal (expected, actual);
        }
    }
}