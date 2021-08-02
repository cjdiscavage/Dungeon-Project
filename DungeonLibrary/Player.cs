using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLibrary
{
    public class Player : Character
    {
        public Weapon EquippedWeapon { get; set; }
        public ShieldGen EquippedGen { get; set; }
        public Player(string name, int life, int maxLife, int hitChance, int block, Weapon equippedWeapon, ShieldGen equippedGen) : base(name, life, maxLife, hitChance, block)
        {
            EquippedWeapon = equippedWeapon;
            EquippedGen = equippedGen;
        }

        public override string ToString()
        {
            return string.Format($"{Name}\nHealth: {Life}/{MaxLife}\nShields: {EquippedGen.Shields}/{EquippedGen.MaxShields}\nShield Regen: {EquippedGen.RegenRate}\nDamage: {EquippedWeapon.MinDamage}-{EquippedWeapon.MaxDamage}");
        }

        public override int CalcDamage()
        {
            return new Random().Next(EquippedWeapon.MinDamage, EquippedWeapon.MaxDamage + 1);
        }

        public override void TakeDamage(string damageType, int damage)
        {
            if (damageType == "Ballistic")
            {
                if (EquippedGen.Shields >= damage / 2)
                {
                    damage /= 2;
                    EquippedGen.Shields -= damage;
                    Life -= damage * 2 / 3;
                }
                else
                {
                    damage -= EquippedGen.Shields;
                    EquippedGen.Shields = 0;
                    Life -= damage * 2 / 3;
                }
            }
            else if (damageType == "Ion")
            {
                if (EquippedGen.Shields >= damage * 2)
                {
                    damage *= 2;
                    EquippedGen.Shields -= damage;
                }
                else
                {
                    damage *= 2;
                    damage -= EquippedGen.Shields;
                    EquippedGen.Shields = 0;
                    damage /= 4;
                    Life -= damage;
                }
            }
            else
            {
                if (EquippedGen.Shields >= damage)
                {
                    EquippedGen.Shields -= damage;
                }
                else
                {
                    damage -= EquippedGen.Shields;
                    EquippedGen.Shields = 0;
                    Life -= damage;
                }
            }

        }
    }
}