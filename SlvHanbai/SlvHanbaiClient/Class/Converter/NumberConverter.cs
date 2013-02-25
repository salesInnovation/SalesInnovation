using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SlvHanbaiClient.Class.Utility;

namespace SlvHanbaiClient.Class.Converter
{
    public class NumberConverter : IValueConverter
    {

        public NumberConverter()
        {
            // プロパティのデフォルトを設定
            this.PositiveSign = string.Empty;
            this.NegativeSign = "-";
            this.DecimalPlaces = 0;
            this.IsDisplayComma = true;
            this.IsMaxMinCheck = false;
            this.MaxNumber = 0;
            this.MinNumber = 0;
        }

        public string FormatString { get; set; }

        /// <summary>
        /// 入力された値が正の時に数字の前に表示する記号を取得・設定する。
        /// </summary>
        public string PositiveSign { get; set; }

        /// <summary>
        /// 入力された値が負の時に数字の前に表示する記号を取得・設定する。
        /// </summary>
        public string NegativeSign { get; set; }

        /// <summary>
        /// カンマ表示をするかどうかを取得・設定する。
        /// </summary>
        public bool IsDisplayComma { get; set; }

        /// <summary>
        /// 小数部の桁数を取得または設定する。
        /// </summary>
        public int DecimalPlaces { get; set; }

        /// <summary>
        /// 最大値・最小値チェックを設定する。
        /// </summary>
        public bool IsMaxMinCheck { get; set; }

        /// <summary>
        /// 小数部の桁数を取得または設定する。
        /// </summary>
        public double MaxNumber { get; set; }

        /// <summary>
        /// 小数部の桁数を取得または設定する。
        /// </summary>
        public double MinNumber { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // プロパティの値を検証する。
            this.ValidateMyProperties();

            // ターゲットになるプロパティの型がstringかを検証する。
            if (targetType != typeof(string))
            {
                return DependencyProperty.UnsetValue;
            }
            else if (value == null)
            {
                return null;
            }

            double dblValue = ExCast.zCDbl(value);

            // ソースの値を数値型に変換する。
            decimal number;
            if (decimal.TryParse(dblValue.ToString(), out number))
            {
                // 数字の前に表示する記号を取得する。
                string sign = this.GetSign(number);

                // フォーマットの書式を取得する
                string format = this.GetConvertFormat();

                // フォーマットした文字列を生成する
                string formatedValue = sign + string.Format(format, Math.Abs(number));

                if (!string.IsNullOrEmpty(this.FormatString))
                {
                    formatedValue = ExCast.zCDbl(formatedValue).ToString(this.FormatString);
                }

                if (this.IsMaxMinCheck == true)
                {
                    double _dbl = ExCast.zCDbl(formatedValue);
                    if (!(_dbl >= this.MinNumber && _dbl <= this.MaxNumber))
                    {
                        return "";
                    }
                    else 
                    {
                        return formatedValue;
                    }
                }
                else
                {
                    return formatedValue;
                }
            }
            else
            {
                if (this.IsMaxMinCheck == true)
                {
                    return 0;
                }

                // 数値に変換できない文字列の場合、そのまま返す。
                return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // プロパティの値を検証する。
            this.ValidateMyProperties();

            string text = value as string;
            if (!string.IsNullOrEmpty(text))
            {
                // ソースプロパティにセットする値を取得する。
                text = this.ConvertToPlaneNumber(text);

                decimal number;
                if (decimal.TryParse(text, out number))
                {
                    // 小数点以下の数字を整形する。
                    return number.ToString(this.GetFormat(false));
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return value;
            }
        }

        /// <summary>プロパティの値を検証する。</summary>
        private void ValidateMyProperties()
        {
            if (this.IsSufixNumber(this.PositiveSign))
            {
                throw new Exception("PositiveSignに末尾が数字で終わる文字列を設定できません。");
            }

            if (this.IsSufixNumber(this.NegativeSign))
            {
                throw new Exception("NegativeSignに末尾が数字で終わる文字列を設定できません。");
            }

            if (string.IsNullOrEmpty(this.NegativeSign))
            {
                throw new Exception("NegativeSignを空にできません。");
            }

            if (this.NegativeSign == this.PositiveSign)
            {
                throw new Exception("NegativeSignとPositiveSignを同じ値にできません。");
            }

            if (this.DecimalPlaces < 0)
            {
                throw new Exception("DecimalPlacesにマイナスの値を設定できません。");
            }
        }

        private bool IsSufixNumber(string str)
        {
            Regex r = new Regex(@"[0-9]$");
            Match m = r.Match(str);
            return m.Success;
        }

        /// <summary>数字の前に表示する記号を取得する。</summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string GetSign(decimal number)
        {
            string sign;
            if (number < 0)
            {
                sign = this.NegativeSign;
            }
            else
            {
                sign = this.PositiveSign;
            }

            return sign;
        }

        /// <summary>string.Formatメソッド用のフォーマット書式を取得する</summary>
        /// <returns></returns>
        private string GetConvertFormat()
        {
            string format = string.Format("{{0:{0}}}", this.GetFormat(this.IsDisplayComma));
            return format;
        }

        /// <summary>フォーマット書式を取得する</summary>
        /// <param name="isDisplayComma"></param>
        /// <returns></returns>
        private string GetFormat(bool isDisplayComma)
        {
            string format = "0";

            if (isDisplayComma)
            {
                format = "#,##" + format;
            }

            if (this.DecimalPlaces > 0)
            {
                format += ".";
                for (int i = 0; i < this.DecimalPlaces; i++)
                {
                    format += "0";
                }
            }

            return format;
        }

        /// <summary>ソースプロパティにセットする値を取得する。</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string ConvertToPlaneNumber(string text)
        {
            text = text.Replace(",", string.Empty);
            bool isNegative = this.IsNegativeNumber(text);
            if (isNegative)
            {
                text = text.Replace(this.NegativeSign, string.Empty);
                text += "-";
            }
            else if (!string.IsNullOrEmpty(this.PositiveSign))
            {
                text = text.Replace(this.PositiveSign, string.Empty);
            }

            return text;
        }

        private bool IsNegativeNumber(string text)
        {
            return text.StartsWith(this.NegativeSign);
        }

    }
}
