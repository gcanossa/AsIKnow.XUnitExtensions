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

        [Trait("Category", "Order")]
        [Fact(DisplayName = nameof(Total))]
        [TestPriority(1000)]
        public void Total()
        {
            Assert.Equal(new string[] { nameof(Test2), nameof(Test1), nameof(Test3) }, Order);
        }

        [Trait("Category", "Order")]
        [Fact(DisplayName = nameof(Test1))]
        [TestPriority(2)]
        public void Test1()
        {
            Order.Add(nameof(Test1));
        }

        [Trait("Category", "Order")]
        [Fact(DisplayName = nameof(Test2))]
        [TestPriority(1)]
        public void Test2()
        {
            Order.Add(nameof(Test2));
        }

        [Trait("Category", "Order")]
        [Fact(DisplayName = nameof(Test3))]
        [TestPriority(2)]
        public void Test3()
        {
            Order.Add(nameof(Test3));
        }
    }
}
