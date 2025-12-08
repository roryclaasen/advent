// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace AdventOfCode.Shared;

public sealed class UnionFind
{
    private readonly int[] parent;
    private readonly int[] rank;

    public UnionFind(int size)
    {
        this.parent = new int[size];
        this.rank = new int[size];
        for (var i = 0; i < size; i++)
        {
            this.parent[i] = i;
            this.rank[i] = 0;
        }
    }

    public int Find(int x)
    {
        if (this.parent[x] != x)
        {
            this.parent[x] = this.Find(this.parent[x]);
        }

        return this.parent[x];
    }

    public void Union(int x, int y)
    {
        var rootX = this.Find(x);
        var rootY = this.Find(y);

        if (rootX == rootY)
        {
            return;
        }

        if (this.rank[rootX] < this.rank[rootY])
        {
            this.parent[rootX] = rootY;
        }
        else if (this.rank[rootX] > this.rank[rootY])
        {
            this.parent[rootY] = rootX;
        }
        else
        {
            this.parent[rootY] = rootX;
            this.rank[rootX]++;
        }
    }
}
