namespace AdventOfCode.Shared.Tests;

using NSubstitute;
using System;

[TestClass]
public class UriHelperTests
{
    private IDateTimeProvider dateTimeProvider;
    private UriHelper uriHelper;

    [TestInitialize]
    public void Initialize()
    {
        this.dateTimeProvider = Substitute.For<IDateTimeProvider>();
        this.dateTimeProvider.Now.Returns(new DateTime(2020, 12, 1, 0, 0, 0, DateTimeKind.Local));

        this.uriHelper = new UriHelper(this.dateTimeProvider);
    }

    [TestMethod]
    public void CanGetUriForYear()
    {
        var uri = this.uriHelper.Get(2020);

        Assert.AreEqual("https://adventofcode.com/2020", uri.ToString());
    }

    [TestMethod]
    [DataRow(2014)]
    [DataRow(2021)]
    public void GetUriForYearThrowsWhenYearsOutOfRange(int year)
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => this.uriHelper.Get(year));
    }

    [TestMethod]
    public void CanGetUriForDay()
    {
        var uri = this.uriHelper.Get(2020, 1);

        Assert.AreEqual("https://adventofcode.com/2020/day/1", uri.ToString());
    }

    [TestMethod]
    [DataRow(2014)]
    [DataRow(2021)]
    public void GetUriUriForDayThrowsWhenYearIsOutOfRange(int year)
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => this.uriHelper.Get(year, 1));
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(26)]
    public void GetUriUriForDayThrowsWhenDayIsOutOfRange(int day)
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => this.uriHelper.Get(2020, day));
    }
}
