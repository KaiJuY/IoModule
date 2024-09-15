using IOControlModule;
using IOControlModule.MitControlModule;
namespace LibTester
{
    public class UnitTest1
    {
        IMitControlModule mitControlModule;
        [Fact]
        public void Test1()
        {
            mitControlModule = new McControlModule("127.0.0.1", 5003);
            Assert.True(mitControlModule!=null);
        }
        [Fact]
        public void Test2()
        {
            mitControlModule = new MxControlModule("SIM", "127.0.0.1", 5555); 
            Assert.True(mitControlModule!=null);
        }
    }
}