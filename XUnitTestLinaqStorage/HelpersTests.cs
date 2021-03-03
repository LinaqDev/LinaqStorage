using LinaqStorage.Helpers;
using System.IO;
using Xunit;

namespace XUnitTestLinaqStorage
{


    public class HelpersTests
    {
        [Fact]
        public void CreateGuidTest()
        {
            Assert.NotNull(Helpers.CreateNewGuid);
        }

        [Fact]
        public void CreateGuidWithoutDashTest()
        {
            Assert.NotNull(Helpers.CreateNewGuidWithoutDash());
        }

        [Fact]
        public void CreateMD5Test()
        {
            string path = @"Example.txt";
            File.AppendAllLines(path, new[] { "Example Content" });
            Assert.NotNull(Helpers.CreateMd5Key(path));
            Assert.Equal(Helpers.CreateMd5Key(path), Helpers.CreateMd5Key(path));
        }
    }
}
