namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

[Problem(2015, 21, "RPG Simulator 20XX")]
public partial class Day21Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var boss = ParseInput(input);
        var minGold = int.MaxValue;
        foreach (var items in this.GetAllItemCombinations())
        {
            var goldSpent = items.Sum(i => i.Cost);
            if (goldSpent < minGold && Fight(boss, items))
            {
                minGold = Math.Min(minGold, goldSpent);
            }
        };

        return minGold;
    }

    public object? PartTwo(string input)
    {
        var boss = ParseInput(input);
        var maxGold = int.MinValue;
        foreach (var items in this.GetAllItemCombinations())
        {
            var goldSpent = items.Sum(i => i.Cost);
            if (goldSpent > maxGold && !Fight(boss, items))
            {
                maxGold = Math.Max(maxGold, goldSpent);
            }
        }

        return maxGold;
    }

    private IEnumerable<Item[]> GetAllItemCombinations()
    {
        var shop = GetShop();
        foreach (var weapon in shop.Where(i => i.Type == ItemType.Weapon))
        {
            yield return [weapon];

            foreach (var armor in shop.Where(i => i.Type == ItemType.Armor))
            {
                yield return [weapon, armor];

                foreach (var ring1 in shop.Where(i => i.Type == ItemType.Ring))
                {
                    yield return [weapon, armor, ring1];

                    foreach (var ring2 in shop.Where(i => i.Type == ItemType.Ring && i != ring1))
                    {
                        yield return [weapon, armor, ring1, ring2];
                    }
                }
            }

            foreach (var ring1 in shop.Where(i => i.Type == ItemType.Ring))
            {
                yield return [weapon, ring1];

                foreach (var ring2 in shop.Where(i => i.Type == ItemType.Ring && i != ring1))
                {
                    yield return [weapon, ring1, ring2];
                }
            }
        }
    }

    private static bool Fight(BossStats boss, IEnumerable<Item> playerItems)
    {
        var playerDamage = playerItems.Sum(i => i.Damage);
        var playerArmor = playerItems.Sum(i => i.Armor);

        var playerHitPoints = 100;
        var bossHitPoints = boss.HitPoints;
        while (playerHitPoints > 0 && bossHitPoints > 0)
        {
            playerHitPoints -= Math.Max(0, boss.Damage - playerArmor);
            bossHitPoints -= Math.Max(0, playerDamage - boss.Armor);
        }

        return playerHitPoints > 0;
    }

    private static List<Item> GetShop() => new()
    {
        // Weapons
        new Item("Dagger", 8, 4, 0, ItemType.Weapon),
        new Item("Shortsword", 10, 5, 0, ItemType.Weapon),
        new Item("Warhammer", 25, 6, 0, ItemType.Weapon),
        new Item("Longsword", 40, 7, 0, ItemType.Weapon),
        new Item("Greataxe", 74, 8, 0, ItemType.Weapon),

        // Armor
        new Item("Leather", 13, 0, 1, ItemType.Armor),
        new Item("Chainmail", 31, 0, 2, ItemType.Armor),
        new Item("Splintmail", 53, 0, 3, ItemType.Armor),
        new Item("Bandedmail", 75, 0, 4, ItemType.Armor),
        new Item("Platemail", 102, 0, 5, ItemType.Armor),

        // Rings
        new Item("Damage +1", 25, 1, 0, ItemType.Ring),
        new Item("Damage +2", 50, 2, 0, ItemType.Ring),
        new Item("Damage +3", 100, 3, 0, ItemType.Ring),
        new Item("Defense +1", 20, 0, 1, ItemType.Ring),
        new Item("Defense +2", 40, 0, 2, ItemType.Ring),
        new Item("Defense +3", 80, 0, 3, ItemType.Ring)
    };

    private record Item(string Name, int Cost, int Damage, int Armor, ItemType Type);

    private enum ItemType
    {
        Weapon,
        Armor,
        Ring
    }

    private static BossStats ParseInput(string input)
    {
        var hitPoints = 0;
        var damage = 0;
        var armor = 0;
        foreach (var line in input.Lines())
        {
            if (line.StartsWith("Hit Points: "))
            {
                hitPoints = int.Parse(line[12..]);
            }
            else if (line.StartsWith("Damage: "))
            {
                damage = int.Parse(line[8..]);
            }
            else if (line.StartsWith("Armor: "))
            {
                armor = int.Parse(line[7..]);
            }
        }

        return new BossStats(hitPoints, damage, armor);
    }

    private record BossStats(int HitPoints, int Damage, int Armor);
}
