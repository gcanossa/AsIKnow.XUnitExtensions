using AsIKnow.XUnitExtensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTest
{
    [TestCaseOrderer(Constants.PriorityOrdererTypeName, Constants.PriorityOrdererTypeAssemblyName)]
    public class UnitTest1
    {
        private static List<string> Order = new List<string>();

        [Fact]
        [TestPriority(1000)]
        public void Total()
        {
            Assert.Equal(new string[] { nameof(Test2), nameof(Test1), nameof(Test3) }, Order);
        }

        [Fact]
        [TestPriority(2)]
        public void Test1()
        {
            Order.Add(nameof(Test1));
        }
        [Fact]
        [TestPriority(1)]
        public void Test2()
        {
            Order.Add(nameof(Test2));
        }
        [Fact]
        [TestPriority(2)]
        public void Test3()
        {
            Order.Add(nameof(Test3));
        }
    }
}
