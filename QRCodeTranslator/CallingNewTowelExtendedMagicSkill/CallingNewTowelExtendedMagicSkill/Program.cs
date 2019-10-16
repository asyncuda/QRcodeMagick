using System;
using QRCodeTranslator;

namespace CallingNewTowelExtendedMagicSkill
{
    class Program
    {
		static void Main(string[] args)
        {
			var nems = new NewTowelExtendedMagicSkill(@"line.me\fuga", true, "FIRE", @"spell.db");
            Console.WriteLine("Spell1 :{0}, Spell2 :{1}, Power:{2}, Attribute :{3} ", nems.Spell1, nems.Spell2, nems.Power, nems.Attribute);
        }
    }
}