using AsIKnow.XUnitExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTest
{
    [TestCaseOrderer(AsIKnow.XUnitExtensions.Constants.PriorityOrdererTypeName, AsIKnow.XUnitExtensions.Constants.PriorityOrdererTypeAssemblyName)]
    [Collection("Database collection")]
    public class UnitTest3
    {
        protected DatabaseFixture Fixture { get; set; }

        public UnitTest3(DatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        [Trait("Category", "Database")]
        [Fact(DisplayName = nameof(ResolveDependencies))]
        [TestPriority(0)]
        public void ResolveDependencies()
        {
            Assert.True(true);
        }
    }
}
