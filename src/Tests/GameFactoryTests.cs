using System;
using NUnit.Framework;
using Skyscraper;

namespace Tests
{
    [TestFixture]
    public class GameFactoryTests
    {
        [Test]
        public void ShouldReturnGameData()
        {
            var sut = new GameFactory();
            var result = sut.Create(8);

            Assert.NotNull(result);
        }
    }
}
