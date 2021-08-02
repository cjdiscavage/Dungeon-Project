using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonLibrary;

namespace Dungeon
{
    class Program
    {
        static void Main()
        {
            //Game variables needed
            string name;
            string damageType;
            int damage;
            int level = 1;
            int enemiesKilled = 0;
            bool isBoss = false;
            bool exit = false;
            bool newEnemy = false;
            Enemy enemy;

            //Intro to the game
            Console.WriteLine("You have been sent to one of the bases of the infamous pirate group \"Zero Laws\"");
            Console.WriteLine("The gear you were given isn't the best, so you'll have to find better gear at the base.");
            Console.WriteLine("What is your name?");
            name = Console.ReadLine();
            Console.WriteLine("Would you like laser, ballistic, or ion weapons?\n" +
                "Ion does bonus damage to shields, but less to life.\n" +
                "Ballistic partially ignores shields to hit both life and shields, but is slightly weaker against life.\n" +
                "Laser is normal damage.");
            damageType = Console.ReadLine().ToLower().Trim();
            switch (damageType)
            {
                case "ballistic":
                    damageType = "Ballistic";
                    break;
                case "ion":
                    damageType = "Ion";
                    break;
                default:
                    damageType = "Laser";
                    break;
            }
            Console.Clear();
            //Creates starting weapon, shield gen, and player
            Weapon starterGun = new Weapon("Rifle", damageType, 8, 15, 10);
            ShieldGen starterGen = new ShieldGen("Shield Generator", 100, 100, 15);
            Player player = new Player(name, 100, 100, 80, 5, starterGun, starterGen);


            //The first of two nested loops for the game
            do
            {
                //Increases the level if you kill enough enemies
                if (enemiesKilled == 2 + level)
                {
                    Console.WriteLine("You advance deeper into the base.");
                    level++;
                    enemiesKilled = 0;
                    player.EquippedGen.Shields += 30;
                    if (level == 4)
                    {
                        isBoss = true;
                    }
                }
                //Generates a list of enemies depending on the level
                enemy = GenerateEnemy(level);
                newEnemy = false;
                Console.WriteLine($"A {enemy.Name} attacks");
                Console.ReadKey(true);
                do
                {
                    //Main menu
                    Console.Clear();
                    Console.WriteLine("What do you do?\n" +
                        "A)ttac\n" +
                        "R)un\n" +
                        "Y)our stats\n" +
                        "E)nemy stats\n" +
                        "Esc) to quit");
                    ConsoleKey userKey = Console.ReadKey(true).Key;
                    switch (userKey)
                    {
                        //Attack
                        case ConsoleKey.A:
                            //If you damage the enemy and by how much
                            if (new Random().Next(1, 101) < player.HitChance + player.EquippedWeapon.BonusHitChance - enemy.Block)
                            {
                                damage = player.CalcDamage();
                                enemy.TakeDamage(player.EquippedWeapon.DamageType, damage);
                                Console.Write("You delt ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(damage);
                                Console.ResetColor();
                                Console.Write($" damage to the {enemy.Name}.\n");
                            }
                            else
                            {
                                Console.WriteLine("You missed.");
                            }
                            System.Threading.Thread.Sleep(20);
                            //If the enemy damages you and by how much
                            if (new Random().Next(1, 101) < enemy.HitChance - player.Block)
                            {
                                damage = enemy.CalcDamage();
                                player.TakeDamage(enemy.DamageType, damage);
                                Console.Write($"{enemy.Name} delt ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(damage);
                                Console.ResetColor();
                                Console.Write(" damage to you.\n");
                            }
                            else
                            {
                                player.EquippedGen.Shields += player.EquippedGen.RegenRate;
                                Console.WriteLine($"{enemy.Name} missed.");
                            }
                            //If you or the enemy dies
                            if (player.Life <= 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You died.\nGame over");
                                Console.ResetColor();
                                exit = true;
                            }
                            else if (enemy.Life <= 0)
                            {
                                Console.WriteLine($"You killed the {enemy.Name}.");
                                GenerateLoot(player);
                                if (isBoss)
                                {
                                    exit = true;
                                    Console.WriteLine("You managed to clear out the base.\nCongrats.");
                                }
                                enemiesKilled++;
                                newEnemy = true;
                            }
                            Console.ReadKey(true);
                            break;
                        //Run away
                        case ConsoleKey.R:
                            //Enemy free attack
                            if (new Random().Next(1, 101) < enemy.HitChance - player.Block)
                            {
                                damage = enemy.CalcDamage();
                                player.TakeDamage(enemy.DamageType, damage);
                                Console.Write($"{enemy.Name} delt ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(damage);
                                Console.ResetColor();
                                Console.Write(" damage to you.\n");
                            }
                            else
                            {
                                Console.WriteLine($"{enemy.Name} missed.");
                            }
                            //If you die
                            if (player.Life <= 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You died.\nGame over");
                                Console.ResetColor();
                                exit = true;
                            }
                            else
                            {
                                Console.WriteLine("You escaped.");
                                newEnemy = true;
                            }
                            Console.ReadKey(true);
                            break;
                        case ConsoleKey.Y:
                            //Check stats
                            Console.WriteLine(player);
                            Console.ReadKey(true);
                            break;
                        case ConsoleKey.E:
                            //Enemy stats
                            Console.WriteLine(enemy.Name);
                            if (enemy.Life >= enemy.MaxLife * 2 / 3)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else if (enemy.Life >= enemy.MaxLife / 3)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.WriteLine($"Max life: {enemy.MaxLife}");
                            Console.ResetColor();
                            if (enemy.Shields >= enemy.MaxShields * 2 / 3)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                            }
                            else if (enemy.Shields >= enemy.MaxShields / 3)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            Console.WriteLine($"Max shields: {enemy.MaxShields}");
                            Console.ResetColor();
                            Console.WriteLine($"Max damage: {enemy.MaxDamage}");
                            Console.ReadKey(true);
                            break;
                        //Exit the game
                        case ConsoleKey.Escape:
                            exit = true;
                            break;
                        //Invalid input
                        default:
                            Console.WriteLine("Invalid input, try again.");
                            Console.ReadKey(true);
                            break;
                    }
                } while (!exit && !newEnemy);
            } while (!exit);
        }

        //Generates a level 1 enemy
        public static Enemy GenerateEnemy(int level)
        {
            if (level == 1)
            {
                //Creates list of possible enemies to fight at the outpost entrance
                Enemy guard = new Enemy("Guard", 50, 50, 5, 10, 70, 5, 25, 25, "Laser");
                Enemy sniper = new Enemy("Sniper", 20, 20, 10, 20, 95, 0, 55, 55, "Ballistic");
                Enemy jeep = new Enemy("Jeep", 50, 50, 10, 25, 50, 15, 100, 100, "Ballistic");
                Enemy aaTurret = new Enemy("AA Turret", 150, 150, 100, 300, 10, -100, 100, 100, "Ion");
                Enemy sentryTurret = new Enemy("Sentry Turret", 30, 30, 15, 30, 100, 5, 100, 100, "Ballistic");

                //Puts all entrance enemies into an array
                Enemy[] enemies =
                {
                guard, guard, guard, guard, guard, sniper, sniper, sniper, jeep, jeep, aaTurret, sentryTurret
            };
                return enemies[new Random().Next(enemies.Length)];
            }
            else if (level == 2)
            {
                //Creates a list of possible enemies to fight on the interior of the base
                Enemy guard = new Enemy("Guard", 50, 50, 5, 10, 70, 5, 25, 25, "Laser");
                Enemy bombTrap = new Enemy("Bomb Trap", 1, 1, 50, 100, 80, -100, 0, 0, "Laser");
                Enemy heavyGuard = new Enemy("Heavy Guard", 80, 80, 10, 20, 70, 10, 40, 40, "Laser");
                Enemy ionGuard = new Enemy("Ion Guard", 50, 50, 5, 10, 70, 5, 25, 25, "Ion");
                Enemy sentryTurret = new Enemy("Sentry Turret", 30, 30, 15, 30, 100, 5, 100, 100, "Ballistic");

                //Puts all interior enemise into an array
                Enemy[] enemies =
                {
                guard, guard, guard, guard, guard, ionGuard, ionGuard, ionGuard, heavyGuard, heavyGuard, bombTrap, sentryTurret
            };
                return enemies[new Random().Next(enemies.Length)];
            }
            else if (level == 3)
            {
                //Creates a list of possible enemies to fight in the core of the base
                Enemy guard = new Enemy("Guard", 50, 50, 5, 10, 70, 5, 25, 25, "Laser");
                Enemy heavyGuard = new Enemy("Heavy Guard", 80, 80, 10, 20, 70, 10, 40, 40, "Laser");
                Enemy ionGuard = new Enemy("Ion Guard", 50, 50, 5, 10, 70, 5, 25, 25, "Ion");
                Enemy sentryTurret = new Enemy("Sentry Turret", 30, 30, 15, 30, 100, 5, 100, 100, "Ballistic");
                Enemy ionTurret = new Enemy("Ion Turret", 50, 50, 10, 30, 85, 0, 125, 125, "Ion");
                Enemy flameTurret = new Enemy("Flame Turret", 80, 80, 2, 5, 95, 5, 200, 200, "Laser");

                //Puts all core enemies into an array
                Enemy[] enemies =
                {
                guard, guard, guard, ionGuard, ionGuard, ionGuard, heavyGuard, heavyGuard, sentryTurret, ionTurret, flameTurret
            };
                return enemies[new Random().Next(enemies.Length)];
            }
            else
            {
                //Creates a list of bosses at the end of the base
                Enemy railGunner = new Enemy("Rail Gunner", 65, 65, 60, 125, 75, 10, 75, 75, "Ballistic");
                Enemy fireWall = new Enemy("Fire Wall", 300, 300, 10, 20, 80, 5, 250, 250, "Laser");
                Enemy distortionField = new Enemy("Distortion Field", 175, 175, 15, 25, 1000, 10, 80, 80, "Ion"); //1000% chance to hit is NOT a typo

                //Puts boss enemies into an array
                Enemy[] enemies =
                {
                railGunner, fireWall, distortionField
            };
                return enemies[new Random().Next(enemies.Length)];
            }
        }

        public static void GenerateLoot(Player player)
        {
            int lootType = new Random().Next(2);
            switch (lootType)
            {
                case 0:
                    Console.WriteLine("You found a weapon upgrade.");
                    player.EquippedWeapon.MaxDamage += new Random().Next(5, 16);
                    player.EquippedWeapon.MinDamage += new Random().Next(2, 8);
                    break;
                case 1:
                    Console.WriteLine("You found an armor upgrade.");
                    player.EquippedGen.MaxShields += new Random().Next(10, 26);
                    player.MaxLife += new Random().Next(10, 26);
                    player.Life += 40;
                    player.EquippedGen.Shields += 45;
                    break;
            }
        }
    }
}