#!/usr/bin/env zx

import { fs, chalk, question, path } from "zx";

const validYears = [
    "2015",
    "2016",
    "2017",
    "2018",
    "2019",
    "2020",
    "2021",
    "2022",
];
const validDays = [...Array(25).keys()].map((d) => d.toString());

const sourceDir = path.resolve(__dirname, "..", "source");
const testsDir = path.resolve(__dirname, "..", "tests");
const projectPrefix = "AdventOfCode.Year";

const projects = await fs.readdir(sourceDir);
const validProjeccts = projects.filter((project) => {
    if (project.startsWith(projectPrefix)) {
        var year = project.split(projectPrefix)[1];
        return validYears.includes(year);
    }
    return false;
});

const year = await question(
    "What year do you want to create a challenge for? ",
    { choices: validYears }
);

if (!validProjeccts.includes(`${projectPrefix}${year}`)) {
    console.log(chalk.red(`Invalid year: ${year}`));
    if (validYears.includes(year)) {
        console.log(
            chalk.red(`No existing solution, you will need to create one.`)
        );
    }
    process.exit(1);
}

const day = await question("What day do you want to create a challenge for? ", {
    choices: validDays,
});
if (!validDays.includes(day)) {
    console.log(chalk.red(`Invalid day: ${day}`));
    process.exit(1);
}

const dayDir = path.resolve(sourceDir, `${projectPrefix}${year}`, `Day${day}`);
if (await fs.pathExists(dayDir)) {
    console.log(chalk.red(`Day ${day} already exists.`));
    process.exit(1);
}

const fileHeader = (
    filename
) => `// ------------------------------------------------------------------------------
// <copyright file="${filename}" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------`;

await fs.mkdir(dayDir);
await fs.writeFile(path.resolve(dayDir, `Day${day}Input.txt`), ``);
await fs.writeFile(
    path.resolve(dayDir, `Day${day}Runner.cs`),
    `${fileHeader(`Day${day}Runner.cs`)}

namespace AdventOfCode.Year2015
{
    using AdventOfCode.Infrastructure;

    public class Day${day}Runner : Runner<Day${day}Challenge>
    {
        public override int Year => ${year};

        public override int Day => ${day};

        public override string Input => Properties.Resources.Day${day}Input;
    }
}
`
);
await fs.writeFile(
    path.resolve(dayDir, `Day${day}Challenge.cs`),
    `${fileHeader(`Day${day}Challenge.cs`)}

namespace AdventOfCode.Year2015
{
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure;

    public class Day${day}Challenge : IChallenge
    {
        public Task<string> SolvePart1(string input)
        {
            return this.Answer(false);
        }

        public Task<string> SolvePart2(string input)
        {
            return this.Answer(false);
        }
    }
}
`
);

console.log(chalk.green(`Created day ${day} in ${year}`));
console.log(
    chalk.yellow(
        "Note that currently you still have to manually add the input file into the project resources."
    )
);

const tests = await question("Do you also want to create tests? (y/N) ", {
    choices: ["y", "n"],
});
if (tests.toLowerCase() === "y") {
    console.log("tests")
    const testSolution = path.resolve(
        testsDir,
        `${projectPrefix}${year}.Tests`
    );
    if (!(await fs.pathExists(testSolution))) {
        console.log(chalk.red(`No existing test solution for this year`));
        process.exit(1);
    }
    const testDay = path.resolve(testSolution, `Day${day}Tests.cs`);
    if (await fs.pathExists(testDay)) {
        console.log(chalk.red(`A test already exists for day ${day}`));
        process.exit(1);
    }

    await fs.writeFile(testDay,
        `${fileHeader(`Day${day}Tests.cs`)}

namespace AdventOfCode.Year2015.Tests
{
    using System.Threading.Tasks;
    using AdventOfCode.Infrastructure.Tests;
    using AdventOfCode.Year2015;

    [TestClass]
    public class Day${day}Tests : ChallengeTests<Day${day}Challenge>
    {
        [TestMethod]
        [DataRow("input", "false")]
        public async Task Day${day}SolvePart1(string input, string answer)
        {
            if (this.Challenge == null)
            {
                Assert.Fail();
            }

            var solvedAnswer = await this.Challenge.SolvePart1(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }

        [TestMethod]
        [DataRow("input", "false")]
        public async Task Day${day}SolvePart2(string input, string answer)
        {
            if (this.Challenge == null)
            {
                Assert.Fail();
            }

            var solvedAnswer = await this.Challenge.SolvePart2(input).ConfigureAwait(false);
            Assert.AreEqual(answer, solvedAnswer);
        }
    }
}

`
    );

    console.log(chalk.green(`Created test file for day ${day}`));
}
