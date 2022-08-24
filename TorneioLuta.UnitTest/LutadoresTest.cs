using NUnit.Framework;
using TorneioLuta.Domain.Entities;
using TorneioLuta.Interfaces.Application;

namespace TorneioLuta.UnitTest
{
    [TestFixture]
    public class LutadoresTest
    {
        private ILutadoresApp _lutadoresApp;

        [SetUp]
        public void Setup(ILutadoresApp lutadoresApp)
        {
            _lutadoresApp = lutadoresApp;
        }

        [Test]
        public void GetLutadores()
        {
            var result = _lutadoresApp.GetAll();
            
            Assert.AreEqual(1, result.Count);
        }
    }
}