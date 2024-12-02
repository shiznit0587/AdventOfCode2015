using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    class Logger
    {
        static bool logging = false;
        public static void Log(string log) {
            if (logging) Console.WriteLine(log);
        }
    }

    public class Day22
    {
        private GameState _bestSoFar;
    
        public Day22()
        {
            Console.WriteLine("Running Day 22 - a");

            GameState state = new GameState() { UserHP = 50, UserMana = 500, BossHP = 51, BossDamage = 9 };

            Recurse(state);

            if (_bestSoFar != null)
                Console.WriteLine("Lowest Mana Cost = " + _bestSoFar.ManaSpent);
            else
                Console.WriteLine("Lowest Mana Cost = null");
            
            Console.WriteLine("Running Day 22 - b");
            
            _bestSoFar = null;

            state = new GameState() { UserHP = 50, UserMana = 500, BossHP = 51, BossDamage = 9, HardMode = true };

            Recurse(state);

            if (_bestSoFar != null)
                Console.WriteLine("Lowest Mana Cost = " + _bestSoFar.ManaSpent);
            else
                Console.WriteLine("Lowest Mana Cost = null");
        }

        private void Recurse(GameState state)
        {
            Logger.Log("Recurse");
            //Console.ReadLine();
            // Check if we've already spent more mana than the current best solution.
            if (_bestSoFar != null && state.ManaSpent > _bestSoFar.ManaSpent)
            {
                // Bail this branch.
                Logger.Log("Branch already cost more (" + state.ManaSpent + ") than best so far (" + _bestSoFar.ManaSpent + ")");
                return;
            }

            foreach (Spell spell in GetSpells())
            {
                Logger.Log("Casting Spell: " + spell.GetType().Name);
                
                // Copy state for each spell.
                GameState copy = state.Clone();

                if (!RunUserTurn(copy, spell))
                {
                    Logger.Log("Game Over on User Turn (or spell could not be cast)");
                    SaveIfBest(copy);
                    continue;
                }

                if (!RunBossTurn(copy))
                {
                    Logger.Log("Game Over on Boss Turn");
                    SaveIfBest(copy);
                    continue;
                }

                Recurse(copy);
            }

            Logger.Log("Walking back up");
            Logger.Log("");
        }

        private void SaveIfBest(GameState state)
        {
            if (state.UserHP > 0 && state.BossHP <= 0 && (_bestSoFar == null || _bestSoFar.ManaSpent > state.ManaSpent))
            {
                _bestSoFar = state;
                Logger.Log("Best So Far = " + _bestSoFar.ManaSpent);
            }
        }

        private void RunTest1()
        {
            GameState state = new GameState() { UserHP = 10, UserMana = 250, BossHP = 13, BossDamage = 8 };
            List<Spell> choices = new List<Spell>() { new Poison(), new MagicMissile() };
            RunTest(state, choices);
        }

        private void RunTest2()
        {
            GameState state = new GameState() { UserHP = 10, UserMana = 250, BossHP = 14, BossDamage = 8 };
            List<Spell> choices = new List<Spell>() { new Recharge(), new Shield(), new Drain(), new Poison(), new MagicMissile() };
            RunTest(state, choices);
        }

        private void RunTest(GameState state, List<Spell> choices)
        {
            foreach (Spell choice in choices)
            {
                if (!RunUserTurn(state, choice))
                {
                    break;
                }

                if (!RunBossTurn(state))
                {
                    break;
                }
            }

            Logger.Log("");
            state.LogState();
        }

        private bool RunUserTurn(GameState state, Spell spell)
        {
            AdvanceTurn(state, true);

            if (state.GameOver())
                return false;

            if (!CanCastSpell(state, spell))
                return false;

            if (spell.Cast(state))
                state.ActiveEffects.Add(spell);

            return !state.GameOver();
        }

        private bool RunBossTurn(GameState state)
        {
            AdvanceTurn(state, false);

            if (state.GameOver())
                return false;

            BossHit(state);

            return !state.GameOver();
        }

        private void AdvanceTurn(GameState state, bool userTurn)
        {
            Logger.Log("");
            Logger.Log(userTurn ? "-- Player turn --" : "-- Boss turn --");
            state.LogState();

            if (userTurn && state.HardMode)
            {
                state.UserHP--;
                Logger.Log("Hard Mode Applied :: player has " + state.UserHP + " hit points");
                if (state.GameOver())
                {
                    Logger.Log("Hard Mode Caused Game Over");
                    return;
                }
            }

            state.ApplyEffects();
        }

        private void BossHit(GameState state)
        {
            int damage = Math.Max(state.BossDamage - state.UserArmor, 1);

            if (state.UserArmor > 0)
            {
                Logger.Log("Boss attacks for " + state.BossDamage + " - " + state.UserArmor + " = " + damage + " damage!");
            }
            else
            {
                Logger.Log("Boss attacks for " + damage + " damage!");
            }
            state.UserHP -= damage;
        }

        private List<Spell> GetSpells()
        {
            return new List<Spell>() { new MagicMissile(), new Drain(), new Shield(), new Poison(), new Recharge() };
        }

        private bool CanCastSpell(GameState state, Spell spell)
        {
            return spell.Cost <= state.UserMana && !state.ActiveEffects.Exists(s => s.GetType() == spell.GetType());
        }
    }

    abstract class Spell
    {
        /**
         * Get the Mana cost of the Spell.
         * @return Mana cost of the Spell.
         */
        abstract public int Cost {get;}

        /**
         * Casts the Spell against the given GameState.
         * @param state GameState to cast the Spell against.
         * @return Whether the Spell has a lasting Effect.
        */
        abstract public bool Cast(GameState state);
        
        /**
         * Applies the Spell's lasting Effect against the given GameState.
         * @param state GameState to apply the Spell's lasting Effect against.
         * @return Whether the Spell still has a lasting Effect.
        */
        virtual public bool Effect(GameState state)
        {
            return false;
        }

        abstract public Spell Clone();
    }

    class MagicMissile : Spell
    {
        override public int Cost {get { return 53; }}
        override public bool Cast(GameState state)
        {
            Logger.Log("Player casts Magic Missile, dealing 4 damage.");
            state.ManaSpent += Cost;
            state.UserMana -= Cost;
            state.BossHP -= 4;
            return false;
        }
        override public Spell Clone()
        {
            return new MagicMissile();
        }
    }

    class Drain : Spell
    {
        override public int Cost {get { return 73; }}
        override public bool Cast(GameState state)
        {
            Logger.Log("Player casts Drain, dealing 2 damage, and healing 2 hit points.");
            state.ManaSpent += Cost;
            state.UserMana -= Cost;
            state.BossHP -= 2;
            state.UserHP += 2;
            return false;
        }
        override public Spell Clone()
        {
            return new Drain();
        }
    }

    class Shield : Spell
    {
        private int turns = 6;
        override public int Cost {get { return 113; }}
        override public bool Cast(GameState state)
        {
            Logger.Log("Player casts Shield, increasing armor by 7.");
            state.ManaSpent += Cost;
            state.UserMana -= Cost;
            state.UserArmor += 7;
            return true;
        }
        override public bool Effect(GameState state)
        {
            Logger.Log("Shield's timer is now " + (turns-1) + ".");
            if (turns == 1)
            {
                Logger.Log("Shield wears off, decreasing armor by 7.");
                state.UserArmor -= 7;
            }
            return (--turns > 0);
        }
        override public Spell Clone()
        {
            return new Shield(){turns = this.turns};
        }
    }

    class Poison : Spell
    {
        private int turns = 6;
        override public int Cost {get { return 173; }}
        override public bool Cast(GameState state)
        {
            Logger.Log("Player casts Poison.");
            state.ManaSpent += Cost;
            state.UserMana -= Cost;
            return true;
        }
        override public bool Effect(GameState state)
        {
            Logger.Log("Poison deals 3 damage; its timer is now " + (turns-1) + ".");
            state.BossHP -= 3;
            if (turns == 1)
            {
                Logger.Log("Poison wears off.");
            }
            return (--turns > 0);
        }
        override public Spell Clone()
        {
            return new Poison(){turns = this.turns};
        }
    }

    class Recharge : Spell
    {
        private int turns = 5;
        override public int Cost {get { return 229; }}
        override public bool Cast(GameState state)
        {
            Logger.Log("Player casts Recharge.");
            state.ManaSpent += Cost;
            state.UserMana -= Cost;
            return true;
        }
        override public bool Effect(GameState state)
        {
            Logger.Log("Recharge provides 101 mana; its timer is now " + (turns-1) + ".");
            state.UserMana += 101;
            if (turns == 1)
            {
                Logger.Log("Recharge wears off.");
            }
            return (--turns > 0);
        }
        override public Spell Clone()
        {
            return new Recharge(){turns = this.turns};
        }
    }

    class GameState
    {
        public bool HardMode;
        public int UserHP;
        public int UserMana;
        public int UserArmor;
        public int BossHP;
        public int BossDamage;
        public int ManaSpent;
        public List<Spell> ActiveEffects = new List<Spell>();
        
        public GameState Clone()
        {
            return new GameState()
            {
                HardMode = this.HardMode,
                UserHP = this.UserHP,
                UserMana = this.UserMana,
                UserArmor = this.UserArmor,
                BossHP = this.BossHP,
                BossDamage = this.BossDamage,
                ManaSpent = this.ManaSpent,
                ActiveEffects = this.ActiveEffects.Select(s => s.Clone()).ToList()
            };
        }

        public void ApplyEffects()
        {
            List<Spell> expiredSpells = ActiveEffects.Where(s => !s.Effect(this)).ToList();
            expiredSpells.ForEach(s => ActiveEffects.Remove(s));
        }

        public bool GameOver()
        {
            return (BossHP <= 0) || (UserHP <= 0);
        }

        public void LogState()
        {
            Logger.Log("- Player has " + UserHP + " hit points, " + UserArmor + " armor, " + UserMana + " mana");
            Logger.Log("- Boss has " + BossHP + " hit points");
        }
    }
}
