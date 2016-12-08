using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanmissionManager;
using DanmissionManager.Models;
using DanmissionManager.TestViewModels;
using NUnit.Framework;

namespace TestDanmissionManager.ExampleTests
{
    [TestFixture]
    public class TestTemplateViewModel
    {
        [Test]
        public void TestOne()
        {
            int i = 5;
            int j = 6;
            Assert.AreEqual(i, j);
        }

        [Test]
        public void TestTwo()
        {
            int i = 5;
            int j = 6;
            Assert.AreNotEqual(i, j);
        }
    }
}
