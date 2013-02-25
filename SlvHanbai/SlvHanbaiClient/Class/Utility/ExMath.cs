using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SlvHanbaiClient.Class
{
    public class ExMath
    {
        // 切り捨て
        public static double zFloor(double dbl, int deciemal)
        {
            try
            {
                double Mult = 0;
                if (deciemal == 0)
                {
                    Mult = 1;
                    return Math.Floor(dbl * Mult);
                }
                else
                {
                    Mult =　Math.Pow(10, deciemal);
                    return Math.Floor(dbl * Mult) / Mult;
                }
            }
            catch
            {
                return dbl;
            }
        }

        // 切り上げ
        public static double zCeiling(double dbl, int deciemal)
        {
            try
            {
                double Mult = 0;
                if (deciemal == 0)
                {
                    Mult = 1;
                    return Math.Ceiling(dbl * Mult);
                }
                else
                {
                    Mult = Math.Pow(10, deciemal);
                    return Math.Ceiling(dbl * Mult) / Mult;
                }

            }
            catch
            {
                return dbl;
            }
        }

        // 四捨五入
        public static double zRound(double dbl, int deciemal)
        {
            try
            {
                return RoundOff(dbl, deciemal);
            }
            catch
            {
                return dbl;
            }
        }

        public static double RoundOff(double roundValue, Int32 digits)
        {
            int signValue = 0;					    // 符号退避用
            double absValue = 0;				    // 絶対値退避用
            double wk1 = 0, wk2 = 0, result = 0;	// 計算結果退避用

            // 符号を取得
            signValue = System.Math.Sign(roundValue);

            // 絶対値を取得
            absValue = System.Math.Abs(roundValue);

            // 小数点の位置をずらす
            wk1 = absValue * (double)(System.Math.Pow(10, digits));

            // 四捨五入
            wk2 = System.Math.Floor(wk1 + 0.5d);

            // 小数点の位置を戻す
            wk2 = wk2 / (double)(System.Math.Pow(10, digits));

            // 符号を戻す
            result = wk2 * signValue;

            return result;
        }


        public static double zCalcPrice(double price, int Fraction, int diciemal)
        {
            try
            {
                switch (Fraction)
                {
                    case 1:     // 切捨
                        return zFloor(price, diciemal);
                    case 2:     // 切上
                        return zCeiling(price, diciemal);
                    case 3:     // 四捨五入
                        return zRound(price, diciemal);
                    default:    // 切上
                        return zCeiling(price, diciemal);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 税計算
        /// </summary>
        /// <param name="price">税抜金額</param>
        /// <param name="taxKbn">税区分(1～3:外税 4～6:内税 7:非課税)</param>
        /// <param name="deciemal">小数点以下桁数</param>
        /// <param name="Fraction">端数処理区分(1:切捨 2:切上 3:四捨五入)</param>
        /// <returns></returns>
        public static double zCalcTax(double price, int taxKbn, int deciemal, int Fraction)
        {
            double tax = 0;
            try
            {
                switch (taxKbn)
                {
                    // 外税
                    case 1:
                    case 2:
                    case 3:
                        tax = price * 0.05;
                        break;
                    // 内税
                    case 4:
                    case 5:
                    case 6:
                        tax = price * 5 / 105;
                        break;
                    // 非課税
                    case 7:
                        return 0;
                    default:
                        return 0;
                }

                switch (Fraction)
                {
                    case 1:     // 切捨
                        return zFloor(tax, deciemal);
                    case 2:     // 切上
                        return zCeiling(tax, deciemal);
                    case 3:     // 四捨五入
                        return zRound(tax, deciemal);
                    default:
                        return zCeiling(tax, deciemal);
                }

            }
            catch
            {
                return 0;
            }
        }
    }
}
