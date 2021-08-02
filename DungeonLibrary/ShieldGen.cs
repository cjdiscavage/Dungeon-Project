using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLibrary
{
    public class ShieldGen
    {
        private int _shields;
        public string Name { get; set; }
        public int MaxShields { get; set; }
        public int RegenRate { get; set; }
        public int Shields
        {
            get { return _shields; }
            set
            {
                _shields = value <= MaxShields ? value : MaxShields;
            }
        }

        public ShieldGen(string name, int shields, int maxShields, int regenRate)
        {
            Name = name;
            MaxShields = maxShields;
            Shields = shields;
            RegenRate = regenRate;
        }

        public override string ToString()
        {
            return string.Format($"{Name}\nShields: {Shields}/{MaxShields}\nRegen Rate: {RegenRate}");
        }
    }
}