using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRCodeTranslator;

namespace CallingNewTowelExtendedMagicSkill
{
    class Program
    {
        [TestMethod]
        public void TestMethod1()
        {
            Hoge();
        }
        void Hoge()
        {
            var instance = new NewTowelExtendedmagicSkill("hogefugapiyo");
            Console.WriteLine("Spell1 :{0}, Spell2 :{1}, Power:{2}, Attribute :{3} ", nems.Spell1, nems.Spell2, nems.Power, nems.Attribute);
        }
    }
}
