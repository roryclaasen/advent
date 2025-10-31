// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;

internal interface ITest : ICompile, IHasArtifacts
{
    [Parameter("Output folder for test results")]
    public AbsolutePath TestResultDirectory
        => this.TryGetValue(() => this.TestResultDirectory)
        ?? (this.ArtifactsDirectory / "test-results");

    public Target Test => _ => _
        .Requires(() => this.Solution)
        .Requires(() => this.Configuration)
        .DependsOn(this.Compile)
        .Produces(this.TestResultDirectory / "*.trx")
        .Executes(this.RunTest);

    private void RunTest()
    {
        try
        {
            this.TestResultDirectory.DeleteDirectory();

            var settings = new DotNetTestSettings()
                .EnableNoRestore()
                .EnableNoBuild()
                .SetConfiguration(this.Configuration)
                .AddProcessAdditionalArguments("--solution", this.Solution)
                .AddProcessAdditionalArguments("--report-trx")
                .AddProcessAdditionalArguments("--results-directory", this.TestResultDirectory);

            DotNetTasks.DotNetTest(settings);
        }
        finally
        {
            this.ReportTestCount();
        }
    }

    private void ReportTestCount()
    {
        static IEnumerable<string> GetOutcomes(AbsolutePath file) => XmlTasks.XmlPeek(
            file,
            "/xn:TestRun/xn:Results/xn:UnitTestResult/@outcome",
            ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"));

        var resultFiles = this.TestResultDirectory.GlobFiles("*.trx");
        List<string> outcomes = [.. resultFiles.SelectMany(GetOutcomes)];

        var passedTests = outcomes.Count(x => x == "Passed");
        var failedTests = outcomes.Count(x => x == "Failed");
        var skippedTests = outcomes.Count(x => x == "NotExecuted");

        this.ReportSummary(_ => _
            .When(failedTests > 0, _ => _
                .AddPair("Failed", failedTests.ToString(CultureInfo.InvariantCulture)))
            .AddPair("Passed", passedTests.ToString(CultureInfo.InvariantCulture))
            .When(skippedTests > 0, _ => _
                .AddPair("Skipped", skippedTests.ToString(CultureInfo.InvariantCulture))));
    }
}
