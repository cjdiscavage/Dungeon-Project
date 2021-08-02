using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLibrary
{
    public class Weapon
    {
        private int _minDamage;
        public string DamageType { get; set; }
        public int MaxDamage { get; set; }
        public int BonusHitChance { get; set; }
        public string Name { get; set; }
        public int MinDamage
        {
            get { return _minDamage; }
            set
            {
                _minDamage = value <= MaxDamage ? value : MaxDamage;
            }
        }

        public Weapon(string name, string damageType, int minDamage, int maxDamage, int bonusHitChance)
        {
            Name = name;
            DamageType = damageType;
            MaxDamage = maxDamage;
            MinDamage = minDamage;
            BonusHitChance = bonusHitChance;
        }

        public override string ToString()
        {
            return string.Format($"{Name}\nDamage: {MinDamage}-{MaxDamage}\nHit chance bonus {BonusHitChance}%\nDamage Type: {DamageType}");
        }
    }
}