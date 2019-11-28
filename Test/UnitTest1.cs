using API_ISBAN.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var xClients = new Collection<int>();

            for (var i = 0; i <= 20; i++)
            {
                xClients.Add(i);
            }

            var xSucess = true;

            //simulation 20 requests
            Parallel.ForEach(xClients, (iClient, iState) =>
            {
                var xData = HackNewsWrapper.GetData();

                if (xData == null)
                {
                    xSucess = false;
                    iState.Break();
                }
            });

            Assert.AreEqual(xSucess, true);
        }
    }
}
