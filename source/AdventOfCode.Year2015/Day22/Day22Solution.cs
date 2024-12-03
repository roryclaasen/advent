namespace AdventOfCode.Year2015;

using AdventOfCode.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

[Problem(2015, 22, "Wizard Simulator 20XX")]
public partial class Day22Solution : IProblemSolver
{
    public object? PartOne(string input)
    {
        var boss = ParseInput(input);
        var currentFights = new List<Fight>
        {
            new() {
                BossDamage = boss.Damage,
                BossHitPoints = boss.HitPoints,
            }
        };
        int? minManaSpent = null;

        bool IsGameRunning(Fight fight) => !fight.IsGameOver && fight.PlayerManaSpent < (minManaSpent ?? int.MaxValue);

        while (currentFights.Any(IsGameRunning))
        {
            // Player turn
            foreach(var fight in currentFights.Where(IsGameRunning))
            {
                fight.ApplyEffects();
                if (fight.IsBossDead)
                {
                    minManaSpent = Math.Min(minManaSpent ?? int.MaxValue, fight.PlayerManaSpent);
                    continue;
                }
            }

            currentFights = GetNextFights(currentFights.Where(IsGameRunning)).ToList();

            foreach (var fight in currentFights.Where(IsGameRunning))
            {
                if (fight.IsBossDead)
                {
                    minManaSpent = Math.Min(minManaSpent ?? int.MaxValue, fight.PlayerManaSpent);
                    continue;
                }


                // Boss Turn
                fight.ApplyEffects();
                if (fight.IsBossDead)
                {
                    minManaSpent = Math.Min(minManaSpent ?? int.MaxValue, fight.PlayerManaSpent);
                    continue;
                }

                fight.BossAttack();
            }
        }

        return minManaSpent;
    }

    public object? PartTwo(string input)
    {
        var boss = ParseInput(input);
        var currentFights = new List<Fight>
        {
            new() {
                BossDamage = boss.Damage,
                BossHitPoints = boss.HitPoints,
            }
        };
        int? minManaSpent = null;

        bool IsGameRunning(Fight fight) => !fight.IsGameOver && fight.PlayerManaSpent < (minManaSpent ?? int.MaxValue);

        while (currentFights.Any(IsGameRunning))
        {
            // Player turn
            foreach (var fight in currentFights.Where(IsGameRunning))
            {
                fight.PlayerHitPoints--;
                if (fight.IsPlayerDead)
                {
                    continue;
                }

                fight.ApplyEffects();
                if (fight.IsBossDead)
                {
                    minManaSpent = Math.Min(minManaSpent ?? int.MaxValue, fight.PlayerManaSpent);
                    continue;
                }
            }

            currentFights = GetNextFights(currentFights.Where(IsGameRunning)).ToList();

            foreach (var fight in currentFights.Where(IsGameRunning))
            {
                if (fight.IsBossDead)
                {
                    minManaSpent = Math.Min(minManaSpent ?? int.MaxValue, fight.PlayerManaSpent);
                    continue;
                }


                // Boss Turn
                fight.ApplyEffects();
                if (fight.IsBossDead)
                {
                    minManaSpent = Math.Min(minManaSpent ?? int.MaxValue, fight.PlayerManaSpent);
                    continue;
                }

                fight.BossAttack();
            }
        }

        return minManaSpent;
    }

    private static (int HitPoints, int Damage) ParseInput(string input)
    {
        var lines = input.Lines();
        var hitPoints = int.Parse(lines[0]["Hit Points: ".Length..]);
        var damage = int.Parse(lines[1]["Damage: ".Length..]);
        return new(hitPoints, damage);
    }

    private static IEnumerable<Fight> GetNextFights(IEnumerable<Fight> fights)
    {
        foreach(var fight in fights)
        {
            var spells = Enum.GetValues<Spell>().Where(s => !fight.ActiveEffects.ContainsKey(s));
            foreach (var spell in spells)
            {
                var newFight = fight.Clone();
                newFight.CastSpell(spell);
                yield return newFight;
            }
        }
    }

    private class Fight
    {
        public int BossDamage { get; set; }

        public int BossHitPoints { get; set; }

        private int PlayerMana { get; set; } = 500;

        public int PlayerHitPoints { get; set; } = 50;

        public int PlayerManaSpent { get; private set; }

        private int PlayerArmor => this.ActiveEffects.ContainsKey(Spell.Shield) ? 7 : 0;

        public bool IsPlayerDead => this.PlayerHitPoints <= 0 || this.PlayerMana < 53;

        public bool IsBossDead => this.BossHitPoints <= 0;

        public bool IsGameOver => this.IsPlayerDead || this.IsBossDead;

        public Dictionary<Spell, int> ActiveEffects { get; private set; } = [];

        public void BossAttack()
        {
            this.PlayerHitPoints -= Math.Max(1, this.BossDamage - this.PlayerArmor);
        }

        public void ApplyEffects()
        {
            foreach (var effect in this.ActiveEffects.Keys)
            {
                switch (effect)
                {
                    case Spell.Shield:
                        break;
                    case Spell.Poison:
                        this.BossHitPoints -= 3;
                        break;
                    case Spell.Recharge:
                        this.PlayerMana += 101;
                        break;
                    default:
                        throw new Exception("Unknown Effect");
                }

                this.ActiveEffects[effect]--;
            }

            this.ActiveEffects.Where(e => e.Value <= 0).ToList().ForEach(e => this.ActiveEffects.Remove(e.Key));
        }

        public void CastSpell(Spell spell)
        {
            if (this.ActiveEffects.ContainsKey(spell))
            {
                throw new Exception("Spell already active");
            }

            switch (spell)
            {
                case Spell.MagicMissile:
                    this.BossHitPoints -= 4;
                    this.PlayerMana -= 53;
                    this.PlayerManaSpent += 53;
                    break;
                case Spell.Drain:
                    this.BossHitPoints -= 2;
                    this.PlayerHitPoints += 2;
                    this.PlayerMana -= 73;
                    this.PlayerManaSpent += 73;
                    break;
                case Spell.Shield:
                    this.ActiveEffects[Spell.Shield] = 6;
                    this.PlayerMana -= 113;
                    this.PlayerManaSpent += 113;
                    break;
                case Spell.Poison:
                    this.ActiveEffects[Spell.Poison] = 6;
                    this.PlayerMana -= 173;
                    this.PlayerManaSpent += 173;
                    break;
                case Spell.Recharge:
                    this.ActiveEffects[Spell.Recharge] = 5;
                    this.PlayerMana -= 229;
                    this.PlayerManaSpent += 229;
                    break;
                default:
                    throw new Exception("Unknown spell");
            }
        }

        public Fight Clone()
        {
            return new Fight
            {
                BossDamage = this.BossDamage,
                BossHitPoints = this.BossHitPoints,
                PlayerMana = this.PlayerMana,
                PlayerHitPoints = this.PlayerHitPoints,
                ActiveEffects = this.ActiveEffects.ToDictionary(),
                PlayerManaSpent = this.PlayerManaSpent
            };
        }
    }

    private enum Spell
    {
        MagicMissile,
        Drain,
        Shield,
        Poison,
        Recharge
    }
}
