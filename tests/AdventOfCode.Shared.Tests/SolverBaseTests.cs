namespace AdventOfCode.Shared.Tests;

using System.Diagnostics.CodeAnalysis;

public abstract class SolverBaseTests<TSolver> where TSolver : ISolver, new()
{
    protected TSolver? Solver { get; private set; }

    [TestInitialize]
    [MemberNotNull(nameof(this.Solver))]
    public virtual void SetUp()
    {
        this.Solver = new TSolver();
    }

    [TestMethod]
    public void CanCreateChallenge()
    {
        Assert.IsNotNull(this.Solver);
    }
}
