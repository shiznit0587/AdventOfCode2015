using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day21
    {
        private Equipment[] _weapons = new Equipment[] {
                new Equipment("Dagger", 8, 4, 0),
                new Equipment("Shortsword", 10, 5, 0),
                new Equipment("Warhammer", 25, 6, 0),
                new Equipment("Longsword", 40, 7, 0),
                new Equipment("Greataxe", 74, 8, 0)
            };

        private Equipment[] _armor = new Equipment[] {
                null,
                new Equipment("Leather", 13, 0, 1),
                new Equipment("Chainmail", 31, 0, 2),
                new Equipment("Splintmail", 53, 0, 3),
                new Equipment("Bandedmail", 75, 0, 4),
                new Equipment("Platemail", 102, 0, 5)
            };
        private Equipment[] _rings = new Equipment[] {
                null,
                new Equipment("Damage +1", 25, 1, 0),
                new Equipment("Damage +2", 50, 2, 0),
                new Equipment("Damage +3", 100, 3, 0),
                new Equipment("Defense +1", 20, 0, 1),
                new Equipment("Defense +2", 40, 0, 2),
                new Equipment("Defense +3", 80, 0, 3)
            };

        public Day21()
        {
            Console.WriteLine("Running Day 21 - a");

            List<EquipmentSet> armorSets = new List<EquipmentSet>();

            foreach (Equipment weapon in _weapons)
            {
                foreach (Equipment armor in _armor)
                {
                    foreach (Equipment ring1 in _rings)
                    {
                        foreach (Equipment ring2 in _rings)
                        {
                            if (ring1 != ring2 || ring1 == null)
                            {
                                armorSets.Add(new EquipmentSet(weapon, armor, ring1, ring2));
                            }
                        }
                    }
                }
            }

            armorSets.Sort((a,b) => a.Cost - b.Cost);

            foreach (EquipmentSet armorSet in armorSets)
            {
                UserState enemy = new UserState(103, 9, 2);
                UserState user = new UserState(100, armorSet.TotalDamage, armorSet.TotalArmor);

                while (true)
                {
                    if (user.Attack(enemy))
                    {
                        break;
                    }
                    if (enemy.Attack(user))
                    {
                        break;
                    }
                }

                if (user.HP > 0)
                {
                    Console.WriteLine("Minimum Winning Armor Cost=" + armorSet.Cost);
                    break;
                }
            }

            Console.WriteLine("Running Day 21 - b");

            armorSets.Sort((a,b) => b.Cost - a.Cost);

            foreach (EquipmentSet armorSet in armorSets)
            {
                UserState enemy = new UserState(103, 9, 2);
                UserState user = new UserState(100, armorSet.TotalDamage, armorSet.TotalArmor);

                while (true)
                {
                    if (user.Attack(enemy))
                    {
                        break;
                    }
                    if (enemy.Attack(user))
                    {
                        break;
                    }
                }

                if (enemy.HP > 0)
                {
                    Console.WriteLine("Maximum Losing Armor Cost=" + armorSet.Cost);
                    break;
                }
            }
        }
    }

    class Equipment
    {
        public string Name {get;}
        public int Cost {get;}
        public int Damage {get;}
        public int Armor {get;}

        public Equipment(string name, int cost, int damage, int armor)
        {
            Name = name;
            Cost = cost;
            Damage = damage;
            Armor = armor;
        }
    }

    class EquipmentSet
    {
        public Equipment Weapon {get;}
        public Equipment Armor {get;}
        public Equipment Ring1 {get;}
        public Equipment Ring2 {get;}
        public int Cost {get; private set;}
        public int TotalDamage {get; private set;}
        public int TotalArmor {get; private set;}

        public EquipmentSet(Equipment weapon, Equipment armor, Equipment ring1, Equipment ring2)
        {
            Weapon = weapon;
            Armor = armor;
            Ring1 = ring1;
            Ring2 = ring2;

            UpdateFromEquipment(weapon);
            UpdateFromEquipment(armor);
            UpdateFromEquipment(ring1);
            UpdateFromEquipment(ring2);
        }

        private void UpdateFromEquipment(Equipment equipment)
        {
            if (equipment != null)
            {
                Cost += equipment.Cost;
                TotalDamage += equipment.Damage;
                TotalArmor += equipment.Armor;
            }
        }
    }

    class UserState
    {
        public int HP {get; private set;}
        public int Damage {get;}
        public int Armor {get;}

        public UserState(int hp, int damage, int armor)
        {
            HP = hp;
            Damage = damage;
            Armor = armor;
        }

        public bool Attack(UserState enemy)
        {
            return enemy.TakeHit(this);
        }
        
        public int CalculateDamage(UserState enemy)
        {
            return Math.Max(Damage - enemy.Armor, 1);
        }

        public bool TakeHit(UserState enemy)
        {
            HP -= enemy.CalculateDamage(this);
            return (HP <= 0);
        }
    }
}
