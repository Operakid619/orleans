using System;
using System.Collections.Generic;
using Orleans.Runtime;
using Xunit;

namespace NonSilo.Tests;

[TestCategory("BVT")]
[TestSuite("BVT")]
[TestProvider("None")]
[TestArea("Runtime")]
public sealed class UtilsEnumeratorDisposalTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    public void EnumerableToStringDisposesEnumerator(int count)
    {
        var disposed = false;
        IEnumerable<int> Values()
        {
            try
            {
                for (var i = 0; i < count; i++) yield return i;
            }
            finally { disposed = true; }
        }

        Utils.EnumerableToString(Values());
        Assert.True(disposed);
    }

    [Fact]
    public void EnumerableToStringDisposesEnumeratorWhenFormatterThrows()
    {
        var disposed = false;
        IEnumerable<int> Values()
        {
            try { yield return 1; yield return 2; }
            finally { disposed = true; }
        }

        Assert.Throws<InvalidOperationException>(() =>
            Utils.EnumerableToString(Values(), _ => throw new InvalidOperationException()));
        Assert.True(disposed);
    }
}
