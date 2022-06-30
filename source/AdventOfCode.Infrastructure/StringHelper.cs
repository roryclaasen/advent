// ------------------------------------------------------------------------------
// <copyright file="StringHelper.cs" company="Rory Claasen">
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace AdventOfCode.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class StringHelper
    {
        public static Point ToPoint(string point)
        {
            var cords = point.Split(',').Select(int.Parse).ToArray();
            return new Point(cords[0], cords[1]);
        }
    }
}
