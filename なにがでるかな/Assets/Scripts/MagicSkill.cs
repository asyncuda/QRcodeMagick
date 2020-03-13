using System;
using System.Text;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
namespace Magick
{
    //魔法の種類
    public enum MagicType
    {
        Attack,
        Heal
    }

    public class MagicSkill : MonoBehaviour
    {
        /// <summary>
        /// 攻撃力
        /// </summary>
        public int Power { get; private set; }

        /// <summary>
        /// 魔法の優先度
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// 呪文
        /// </summary>
        public string Spell { get; private set; }

        /// <summary>
        /// 魔法の種類：攻撃・回復（・補助魔法？）
        /// </summary>
        public MagicType MType { get; private set; }

        /// <summary>
        /// 呪文の属性
        /// </summary>
        public string Attribute { get; private set; }

        /// <summary>
        /// QRコードがSNSのURLだった場合にどのSNSなのかを保持する変数
        /// </summary>
        public string SpecialCode { get; private set; }

        //QRコード内の文字列のハッシュと、生成する呪文と属性と攻撃力
        private readonly string Hash;

        //呪文を登録してあるデータベースへのパス
        private readonly string databasePath = Application.streamingAssetsPath + "/spells.db";

        /// <summary>
        /// QRコードから2つの呪文とその属性・攻撃力を決定するクラス
        /// </summary>
        /// <param name="qrString">QRコードの文字列</param>
        public MagicSkill(string qrString)
        {
            //ハッシュ化
            Hash = EncryptQRString(qrString);

            //Hashから呪文・優先度・属性・呪文の種類を決定
            HashToSPAM(Hash, databasePath);

            //攻撃力を決定
            var rp = new RegulatePower(qrString, Hash);
            Power = (int)rp.Power;
            SpecialCode = rp.SpecialCode;
        }

        private string EncryptQRString(string qr)
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

        //（仮：例）ハッシュから呪文と優先度と属性を取得
        private void HashToSPAM(string hash, string path)
        {
            var con = new SQLiteConnectionStringBuilder("Data Source=" + path);
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
                    int magickTypeSeed = HashToIndex(14, 7);
                    int random = new System.Random(Convert.ToInt32(Hash, 16)).Next(0, int.MaxValue);
                    Priority = random;
                    //QRコードごとに異なる乱数をもとにランダムに呪文の種類を決定
                    foreach (MagicType item in Enum.GetValues(typeof(MagicType)))
                    {
                        if ((int)item == random % Enum.GetValues(typeof(MagicType)).Length)
                        {
                            MType = item;
                        }
                    }

                    //属性は1番目の呪文に合わせる（データベースの同じインデックス（行）から取得）
                    int attributeIndex = spell1Index;

                    // 呪文を、ハッシュ値から得られたランダムなデータベースの行数から取得
                    string Spell1 = ExtractFromDatabase(spell1Index, "spell1");
                    string Spell2 = ExtractFromDatabase(spell2Index, "spell2");
                    Spell = Spell1 + Spell2;
                    Attribute = ExtractFromDatabase(attributeIndex, "attribute");

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

                    string ExtractFromDatabase(int index, string clm)
                    {
                        cmd.CommandText = "select " + clm + " from spells where id = " + index;
                        return (string)cmd.ExecuteScalar();

                    }
                }
            }
        }
    }

    //攻撃力の調整を行うクラス
    internal class RegulatePower
    {
        internal double Power { get; private set; }
        internal string SpecialCode { get; private set; }

        public RegulatePower(string qrString, string hash)
        {
            BasicPowerDefinition(hash);
            SNSBonusEffect(qrString);
        }

        //ハッシュ文字列による攻撃力決定：上級コースか否かで基本攻撃力を分岐
        public void BasicPowerDefinition(string hash)
        {
            //ハッシュ文字列を分割
            string subHash = hash.Substring(21, 7);

            //通常の最小・最大攻撃力
            (int min, int max) powers = (min: 1000, max: 10 ^ 4);

            //分割したハッシュ文字列をpowers.min以上powers.max以下に変換
            int power = (Convert.ToInt32(subHash, 16) % (powers.max + 1 - powers.min)) + powers.min;

            Power = power;
        }

        //QRコードがSNSアカウントなどであれば攻撃力加点
        public void SNSBonusEffect(string acc)
        {
            //どれだけ加点するか
            double bonus = 1.2;

            //QRコード内検索
            var m = Regex.Match(acc, @"twitter|line\.me");

            //QRコードにtwitterやlime.meの文字列があれば
            if (m.Success)
            {
                //Powerをbonus分加点, 
                Power *= bonus;

                //検索結果に見つかった文字列に応じてラベル付け
                switch (m.Value)
                {
                    case "twitter":
                        SpecialCode = "Twitter";
                        break;
                    case "line.me":
                        SpecialCode = "LINE";
                        break;
                }
            }
        }
    }


}