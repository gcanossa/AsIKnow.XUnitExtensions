using AsIKnow.XUnitExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTest
{
    [Collection("Dependency collection")]
    public class UnitTest2
    {
        protected DependencyFixture Fixture { get; set; }

        public UnitTest2(DependencyFixture fixture)
        {
            Fixture = fixture;
        }

        [Trait("Category", "DI")]
        [Fact(DisplayName = nameof(DI_ok))]
        [TestPriority(0)]
        public void DI_ok()
        {
            Assert.NotNull(Fixture.GetService<TestTypes.Type2>());
        }
    }
}
