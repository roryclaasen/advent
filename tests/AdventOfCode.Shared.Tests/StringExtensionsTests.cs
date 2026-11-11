// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared.Tests;

using System;

[TestClass]
public sealed class StringExtensionsTests
{
    [TestMethod]
    [DataRow("\n first \n\n last \n")]
    [DataRow("\r\n first \r\n\r\n last \r\n")]
    [DataRow("\r first \r\r last \r")]
    [DataRow("\r\n first \n\r last \r\n")]
    public void LinesRecognizesLineEndingsAndPreservesSplitOptions(string input)
    {
        string[] expectedLines = ["", " first ", "", " last ", ""];
        string[] expectedTrimmedLines = ["first", "last"];
        Assert.AreSequenceEqual(expectedLines, input.Lines());
        Assert.AreSequenceEqual(expectedTrimmedLines, input.Lines(StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    [TestMethod]
    [DataRow("first\nline\n\nlast")]
    [DataRow("first\r\nline\r\n\r\nlast")]
    [DataRow("first\rline\r\rlast")]
    [DataRow("first\rline\r\n\nlast")]
    public void LinesSplitsBlocksAndNormalizesContainedLineEndings(string input)
    {
        Assert.AreSequenceEqual([$"first{Environment.NewLine}line", "last"], input.Lines(2));
    }
}
