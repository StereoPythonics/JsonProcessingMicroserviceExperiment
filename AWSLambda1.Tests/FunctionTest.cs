using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using AWSLambda1;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AWSLambda1.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestFunctionParentingIsCorrect()
        {
            var function = new Function();
            var context = new TestLambdaContext();
            IEnumerable<dynamic> testOutput = function.FunctionHandler("0", context) as IEnumerable<dynamic>;

            Assert.False(testOutput.Any(a => (a.Photos as IEnumerable<dynamic>).Any(p => p.albumId != a.id)),"Some photos are returned inside the incorrect album");
        }

        [Fact]
        public void TestFunctionUserFilteringIsCorrect()
        {
            var function = new Function();
            var context = new TestLambdaContext();
            IEnumerable<dynamic> testOutput = function.FunctionHandler("10", context) as IEnumerable<dynamic>;

            Assert.True(testOutput.All(a => a.userId == 10), "Function is not filtering by user correctly");
        }
    }
}
