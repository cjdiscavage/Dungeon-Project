using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLibrary
{
    public class Enemy : Character
    {
        private int _shields;
        private int _mindamage;
        public int MaxShields { get; set; }
        public int MaxDamage { get; set; }
        public string DamageType { get; set; }
        public int Shields
        {
            get { return _shields; }
            set
            {
                _shields = value <= MaxShields ? value : MaxShields;
            }
        }
        public int MinDamage
        {
            get { return _mindamage; }
            set
            {
                _mindamage = value <= MaxDamage ? value : MaxDamage;
            }
        }

        public Enemy(string name, int life, int maxLife, int minDamage, int maxDamage, int hitChance, int block, int shields, int maxShields, string damageType) : base(name, life, maxLife, hitChance, block)
        {
            MaxShields = maxShields;
            Shields = shields;
            MaxDamage = maxDamage;
            MinDamage = minDamage;
            DamageType = damageType;
        }

        public override int CalcDamage()
        {
            return new Random().Next(MinDamage, MaxDamage + 1);
        }

        public override void TakeDamage(string damageType, int damage)
        {
            if (damageType == "Ballistic")
            {
                if (Shields >= damage / 2)
                {
                    damage /= 2;
                    Shields -= damage;
                    Life -= damage * 2 / 3;
                }
                else
                {
                    damage -= Shields;
                    Shields = 0;
                    Life -= damage * 2 / 3;
                }
            }
            else if (damageType == "Ion")
            {
                if (Shields >= damage * 2)
                {
                    damage *= 2;
                    Shields -= damage;
                }
                else
                {
                    damage *= 2;
                    damage -= Shields;
                    Shields = 0;
                    damage /= 4;
                    Life -= damage;
                }
            }
            else
            {
                if (Shields >= damage)
                {
                    Shields -= damage;
                }
                else
                {
                    damage -= Shields;
                    Shields = 0;
                    Life -= damage;
                }
            }
        }
    }
}