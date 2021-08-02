using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLibrary
{
    public abstract class Character
    {
        private int _life;
        public string Name { get; set; }
        public int MaxLife { get; set; }
        public int HitChance { get; set; }
        public int Block { get; set; }
        public int Life
        {
            get { return _life; }
            set
            {
                _life = value <= MaxLife ? value : MaxLife;
            }
        }

        public Character(string name, int life, int maxLife, int hitChance, int block)
        {
            Name = name;
            MaxLife = maxLife;
            Life = life;
            HitChance = hitChance;
            Block = block;
        }

        public abstract int CalcDamage();

        public abstract void TakeDamage(string damageType, int damage);
    }
}