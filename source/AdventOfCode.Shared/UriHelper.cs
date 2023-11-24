namespace AdventOfCode.Shared;

using System;

public class UriHelper(IDateTimeProvider dateTimeProvider)
{
    public Uri Get(int year)
    {
        if (year < 2015 || year > dateTimeProvider.Now.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(year), year, "Year must be between 2015 and the current year");
        }

        return new($"https://adventofcode.com/{year}");
    }

    public Uri Get(int year, int day)
    {
        if (year < 2015 || year > dateTimeProvider.Now.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(year), year, "Year must be between 2015 and the current year");
        }

        if (day < 1 || day > 25)
        {
            throw new ArgumentOutOfRangeException(nameof(day), day, "Day must be between 1 and 25");
        }

        return new($"https://adventofcode.com/{year}/day/{day}");
    }
}
