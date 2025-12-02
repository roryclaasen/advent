// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared.Numerics;

using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public ref struct RangeEnumerator<TNumber> : IEnumerator<TNumber>
    where TNumber : struct, INumber<TNumber>
{
    private readonly TNumber start;
    private readonly TNumber end;

    private TNumber current;

    public RangeEnumerator(TNumber start, TNumber end)
    {
        this.start = start;
        this.end = end;

        this.current = this.start - TNumber.One;
    }

    public readonly TNumber Current => this.current;

    readonly object IEnumerator.Current => this.current;

    public bool MoveNext()
    {
        if (this.current < this.end)
        {
            this.current++;
            return true;
        }

        return false;
    }

    public void Reset() => this.current = this.start;

    public readonly void Dispose()
    {
    }
}
