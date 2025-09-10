using NUnit.Framework;
using Game.Common;

public class ExpMathTests
{
    [TestCase(1, 1.0f)]
    [TestCase(9, 1.0f)]
    [TestCase(10, 1.5f)]
    [TestCase(15, 1.5f)]
    [TestCase(19, 1.5f)]
    [TestCase(20, 2.25f)]
    [TestCase(30, 3.375f)]
    [TestCase(0, 1.0f)]
    [TestCase(-5, 1.0f)]
    public void ComputeExpMultiplier_ReturnsExpected(int wave, float expected)
    {
        float result = ExpMath.ComputeExpMultiplier(wave);
        Assert.That(result, Is.EqualTo(expected).Within(1e-5f));
    }
}
