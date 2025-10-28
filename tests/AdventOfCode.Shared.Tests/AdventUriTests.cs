// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared.Tests;

using System;
using NSubstitute;

[TestClass]
public class AdventUriTests
{
    private IDateTimeProvider dateTimeProvider;
    private AdventUri adventUri;

    [TestInitialize]
    public void Initialize()
    {
        this.dateTimeProvider = Substitute.For<IDateTimeProvider>();
        this.dateTimeProvider.Now.Returns(new DateTime(2020, 12, 1, 0, 0, 0, DateTimeKind.Local));

        this.adventUri = new AdventUri(this.dateTimeProvider);
    }

    [TestMethod]
    public void CanBuildUriForYear()
    {
        var uri = this.adventUri.Build(2020);

        Assert.AreEqual("https://adventofcode.com/2020", uri.ToString());
    }

    [TestMethod]
    [DataRow(2014)]
    [DataRow(2021)]
    public void BuildUriForYearThrowsWhenYearsOutOfRange(int year)
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => this.adventUri.Build(year));
    }

    [TestMethod]
    public void CanBuildUriForDay()
    {
        var uri = this.adventUri.Build(2020, 1);

        Assert.AreEqual("https://adventofcode.com/2020/day/1", uri.ToString());
    }

    [TestMethod]
    [DataRow(2014)]
    [DataRow(2021)]
    public void BuildUriUriForDayThrowsWhenYearIsOutOfRange(int year)
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => this.adventUri.Build(year, 1));
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(26)]
    public void BuildUriUriForDayThrowsWhenDayIsOutOfRange(int day)
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => this.adventUri.Build(2020, day));
    }
}
