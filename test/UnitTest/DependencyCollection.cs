﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTest
{
    [CollectionDefinition("Dependency collection")]
    public class DependencyCollection : ICollectionFixture<DependencyFixture>
    {
    }
}
