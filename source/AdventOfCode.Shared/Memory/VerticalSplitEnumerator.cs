// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared.Memory;

using System;
using System.Collections;
using System.Collections.Generic;
using CommunityToolkit.HighPerformance;
using LinkDotNet.StringBuilder;

public ref struct VerticalSplitEnumerator : IEnumerator<ReadOnlySpan<char>>
{
    private readonly ReadOnlySpan2D<char> buffer;
    private readonly char separator;

    private int offset = 0;
    private ReadOnlySpan<char> current;

    internal VerticalSplitEnumerator(ReadOnlySpan2D<char> buffer, char separator)
    {
        this.buffer = buffer;
        this.separator = separator;
    }

    public readonly ReadOnlySpan<char> Current => this.current;

    readonly object IEnumerator.Current => throw new NotSupportedException();

    public readonly VerticalSplitEnumerator GetEnumerator() => this;

    public bool MoveNext()
    {
        var start = this.offset;
        var end = this.buffer.Width;
        if (start >= end)
        {
            this.current = default;
            return false;
        }

        for (var col = start; col < end; col++)
        {
            var isSeparator = true;
            for (var row = 0; row < this.buffer.Height; row++)
            {
                if (this.buffer[row, col] != this.separator)
                {
                    isSeparator = false;
                    break;
                }
            }

            if (isSeparator)
            {
                end = col;
                break;
            }
        }

        var sb = new ValueStringBuilder();
        for (var row = 0; row < this.buffer.Height; row++)
        {
            if (row > 0)
            {
                sb.AppendLine();
            }

            for (var col = start; col < end; col++)
            {
                sb.Append(this.buffer[row, col]);
            }
        }

        this.current = sb.AsSpan();
        this.offset = end + 1;
        return true;
    }

    public void Reset()
    {
        this.offset = 0;
        this.current = default;
    }

    readonly void IDisposable.Dispose() { }
}
