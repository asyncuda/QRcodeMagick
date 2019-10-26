using System;
using System.Text;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace QRCodeTranslator
{
	//QRコードからデータベースにアクセスし、呪文と属性を取得するクラス
	public class NewTowelExtendedMagicSkill
	{
		//挑戦するコースの難易度の選択肢
		internal enum Level { Easy = 1, Normal, Hard};

		//QRコード内の文字列のハッシュと、生成する呪文と属性と攻撃力
		private readonly string Hash;
		/// <value>呪文１</value>
		public string Spell1 { get; private set; }
        ///<value>呪文２</value> 
		public string Spell2 { get; private set; }
		///<value>呪文の属性</value> 
		public string Attribute { get; private set; }
		///<value>攻撃力</value> 
		public int Power { get; private set; }

		///<summary>
		/// 中級、上級コース時に参照される、QRコードの文字列から2つの呪文とその属性・攻撃力を生成するクラスのコンストラクタ。
		/// </summary>
		/// <param name="qrString">QRコードの文字列</param>
		/// <param name="level">選択されたコース [Easy, Normal, Hard]</param>
		/// <param name="enemyAttr">敵の属性</param>
		/// <param name="databasePath">呪文と属性が保存されているデータベースのパス</param>
		public NewTowelExtendedMagicSkill(string qrString, int level, string enemyAttr, string databasePath)
        {
			//level4(2人プレイ)はlevel1(初級コース)に合わせる。
			if (level == 4) level = 1;
            
			//ハッシュ化
            Hash = EncryptQRString(qrString);

            //Hashから呪文の組み合わせと属性を決定

            HashToSpellAndAttribute(Hash, level, databasePath);

			//攻撃力の決定
			var rp = new RegulatePower(qrString, Hash, level, enemyAttr, Attribute);
			Power = rp.Power;
		}

		public string EncryptQRString(string qr)
        {
            //引数をバイト配列に変換
            byte[] input = Encoding.ASCII.GetBytes(qr);
            SHA256 sha = new SHA256CryptoServiceProvider();
            //ハッシュ化
            byte[] hash_sha256 = sha.ComputeHash(input);
            //ハッシュ値はバイト配列型なので空の文字列につなげる形でstring型にキャスト
            string result = "";
            for (int i = 0; i < hash_sha256.Length; i++)
            {
                result += string.Format("{0:X2}", hash_sha256[i]);
            }
            return result;
        }

		//（仮：例）ハッシュを数桁ずつ3つに分け、データベース内の、それらをインデックスとする2つの呪文と属性を取得
		public void HashToSpellAndAttribute(string hash, int level, string path)
		{
			var con = new SQLiteConnectionStringBuilder($"Data Source={path}");
            using (var cn = new SQLiteConnection(con.ToString()))
            {
				cn.Open();

                using (var cmd = new SQLiteCommand(cn))
                {
                    //呪文の個数(データベースの行数)を取得
                    cmd.CommandText = "select count(id) from spells";
                    int columns = Convert.ToInt32(cmd.ExecuteScalar());

                    //ハッシュ値を16進数が（int型で桁溢れしない限界）7文字ずつ4つに分け、それぞれをもとに呪文と攻撃力のデータベースに指定する1以上のインデックスと攻撃力を決定
                    int spell1Index = HashToIndex(0, 7);
                    int spell2Index = HashToIndex(7, 7);
                    
                    //属性は1番目の呪文に合わせる（データベースの同じインデックス（行）から取得）
                    int attributeIndex = spell1Index;

                    // 呪文を、ハッシュ値から得られたランダムなデータベースの行数から取得
                    Spell1 = SelectSpell(spell1Index, "spell1");
                    Spell2 = SelectSpell(spell2Index, "spell2");
					
					//初級コース時でないときのみ属性も取得する。初級コース時はAttributeはNULLにする。
					Attribute = level == (int)Level.Easy ? null : SelectSpell(attributeIndex, "attribute");

					//取り出す位置、長さを指定したハッシュの部分文字列を数値に変換し、1以上呪文の個数以下の整数に変換
					int HashToIndex(int start, int end)
                    {
                        //ハッシュ文字列を分割
                        string substr = hash.Substring(start, end);
                        //分割したハッシュ文字列から16進数に変換
                        int valueBySubstr = Convert.ToInt32(substr, 16);
                        //ハッシュ文字列から得られた数値から1以上データ数以下の数値に変換、取得するデータのインデックスとする
                        int idx = valueBySubstr % columns + 1;
                        return idx;
                    }

                    string SelectSpell(int index, string clm)
                    {
                        cmd.CommandText = "select " + clm + " from spells where id = " + index;
                        return (string)cmd.ExecuteScalar();

                    }                 
                }
				cn.Close();
            }
        }      
    }

	//攻撃力の調整を行うクラス
	internal class RegulatePower
	{
		
		internal int Power;
		
		public RegulatePower(string qrString, string hash, int level, string enemyAttr, string playerAttr)
		{
			BasicPowerDefinition(hash, level);
			SNSBonusEffect(qrString);
			if(enemyAttr != null && playerAttr != null) AttributeMatchBonusEffect(enemyAttr, playerAttr);
		}

		
        //ハッシュ文字列による攻撃力決定：上級コースか否かで基本攻撃力を分岐
        public void BasicPowerDefinition(string hash, int level)
        {
            //ハッシュ文字列を分割
            string subHash = hash.Substring(21, 7);

            //最大・最小攻撃力を保持するタプル
            (int min, int max) powers;

            //上級コースだった場合の最小・最大攻撃力
            (int min, int max) specialPowerForHigherCourse = (min: 10 ^ 8, max: 10 ^ 9);
            //通常の最小・最大攻撃力
            (int min, int max) normalPower = (min: 100, max: 10 ^ 4);

            //上級コースかどうかに合わせて最小・最大攻撃力を設定
            powers = (level == 3
				) ? specialPowerForHigherCourse : normalPower;

			//分割したハッシュ文字列をpowers.min以上powers.max以下に変換
			int power = Convert.ToInt32(subHash, 16) % (powers.max + 1 - powers.min) + powers.min;

            Power = power;
        }

        //QRコードがSNSアカウントなどであれば攻撃力加点
        public void SNSBonusEffect(string acc)
        {
            //どれだけ加点するか
            int bonus = 100;

            //QRコードにtwitterやlime.meの文字列があれば
            if (Regex.IsMatch(acc, @"twitter|line\.me"))
            {
				//Powerをbonus分加点
				Power += bonus;
            }
        }

		//[中・上級コース選択時] 敵と自分の属性が一致していれば攻撃力加点
		public void AttributeMatchBonusEffect(string enemAttr, string playerAttr)
        {
            int bonus = 100;

            if (enemAttr == playerAttr)
            {
                Power += bonus;
            }
        }
    }
}