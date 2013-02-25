#region using

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
using SlvHanbaiClient.Class.Utility;

#endregion

namespace SlvHanbaiClient.Class.UI
{
    public class ExTextBox : TextBox
    {

        #region Filed Enum

        private const String CLASS_NM = "ExTextBox";

        // 入力モード
        public enum geInputMode
        {
            Number = 0,         // 数値
            ID,                 // ID(0フォーマット数値、半角英数)
            Date,               // 日付
            FullShapeNative,    // 全角ひらがな
            HalfKana,           // カナ
            FullKana,           // カナ
            Alphanumeric        // 半角英数
        }
        private geInputMode _InputMode = geInputMode.Number;
        public geInputMode InputMode 
        { 
            set 
            { 
                this._InputMode = value;
                SetIdFigure();
            } 
            get 
            { 
                return this._InputMode; 
            } 
        }

        public enum geIdFigureType
        {
            None,
            Customer,
            Purchase,
            Commodity
        }
        private geIdFigureType _IdFigureType = geIdFigureType.None;
        public geIdFigureType IdFigureType 
        { 
            set 
            { 
                this._IdFigureType = value;
                SetIdFigure();
            } 
            get 
            { 
                return this._IdFigureType; 
            } 
        }

        // 小数点桁数
        private int _DecimalNum;
        public int DecimalNum { get { return (int)GetValue(DecimalNumProperty); } set { SetValue(DecimalNumProperty, value); } }

        // 最小入力値
        private double _MinNumber = 0;
        public double MinNumber { set { this._MinNumber = value; } get { return this._MinNumber; } }

        // 最大入力値
        private double _MaxNumber = 999999999;
        public double MaxNumber { set { this._MaxNumber = value; } get { return this._MaxNumber; } }

        // 最大入力バイト数
        private int _MaxLengthB = 0;
        public int MaxLengthB 
        { 
            set 
            { 
                this._MaxLengthB = value;
                this.MaxLength = ExCast.zCInt(value);
            } 
            get 
            { 
                return this._MaxLengthB; 
            } 
        }

        // フォーマット指定文字
        private string _FormatString = "#,##0";
        public string FormatString { set { this._FormatString = value; } get { return this._FormatString; } }

        // 設定値
        private string _Value;
        public string Value { set { this._Value = value; } get { return this._Value; } }

        private string _BeforeValue;
        public string BeforeValue { set { this._BeforeValue = value; } get { return this._BeforeValue; } }

        private bool _UpdataFlg;
        public bool UpdataFlg { set { this._UpdataFlg = value; } get { return this._UpdataFlg; } }


        // 0 To Null
        private bool _ZeroToNull = false;
        public bool ZeroToNull { set { this._ZeroToNull = value; } get { return this._ZeroToNull; } }

        // Null To 0
        private bool _NullToZero = false;
        public bool NullToZero { set { this._NullToZero = value; } get { return this._NullToZero; } }

        // 前回MouseLeftButtonDown時間
        private int intBeforeHour = 0;
        private int intBeforeMinute = 0;
        private int intBeforeSecond = 0;
        private int intBeforeMilliSecond = 0;

        public event MouseButtonEventHandler MouseDoubleClick;

        private bool _IsSelectAll = false;
        public bool IsSelectAll { set { this._IsSelectAll = value; } get { return this._IsSelectAll; } }

        private bool _IsDoubleFocusFlg = false;

        private bool _IsDataGridSelectCell= false;
        public bool IsDataGridSelectCell { set { this._IsDataGridSelectCell = value; } get { return this._IsDataGridSelectCell; } }

        private int _DataGridSelectedColumnIndex = 0;
        public int DataGridSelectedColumnIndex { set { this._DataGridSelectedColumnIndex = value; } get { return this._DataGridSelectedColumnIndex; } }

        public string DataGridRecNo { get { return (string)GetValue(DataGridRecNoProperty); } set { SetValue(DataGridRecNoProperty, value); } }
        
        private bool rockFlg = false;

        #endregion

        #region DependencyProperty

        #region DecimalNumProperty

        public static readonly DependencyProperty DecimalNumProperty =
            DependencyProperty.Register("DecimalNum",
                                        typeof(int),
                                        typeof(ExTextBox),
                                        new PropertyMetadata(new PropertyChangedCallback(ExTextBox.OnDecimalNumPropertyChanged)));

        private static void OnDecimalNumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region DataGridRecNoProperty

        public static readonly DependencyProperty DataGridRecNoProperty =
            DependencyProperty.Register("DataGridRecNo",
                                        typeof(string),
                                        typeof(ExTextBox),
                                        new PropertyMetadata(new PropertyChangedCallback(ExTextBox.OnDataGridRecNoPropertyChanged)));

        private static void OnDataGridRecNoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #endregion

        #region Constructor

        public ExTextBox()

            : base()
        {
            this.TextChanged += OnTextChanged;
            this.IsTabStop = true;
        }

        //~ExTextBox()
        //{
        //    this.TextChanged -= OnTextChanged;
        //}

        #endregion

        #region Events

        protected override void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                if (rockFlg == true) return;

                // 入力モードによる制御
                switch (InputMode)
                {
                    case geInputMode.Number:
                        // 数値型
                        // PlatformKeyCode 189 : -(マイナスキー)
                        // PlatformKeyCode 190 : .(小数点キー)
                        if ((Key.D0 <= e.Key && e.Key <= Key.D9) || (Key.NumPad0 <= e.Key && e.Key <= Key.NumPad9) ||
                            (Key.F1 <= e.Key && e.Key <= Key.F12) ||
                            (Key.Shift == e.Key) || (Key.Ctrl == e.Key) ||
                            (Key.Right == e.Key) || (Key.Left == e.Key) ||
                            (Key.Delete == e.Key) || (Key.Back == e.Key) || (Key.Tab == e.Key) ||
                            (Key.Home == e.Key) || (Key.End == e.Key) || (Key.Enter == e.Key))
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            if (Key.Decimal == e.Key || e.PlatformKeyCode == 190)
                            {
                                if (this.DecimalNum != 0)
                                {
                                    break;
                                }
                            }

                            if (Key.Subtract == e.Key || e.PlatformKeyCode == 189)
                            {
                                if (this.MinNumber < 0)
                                {
                                    break;
                                }
                            }

                            // ここで止める
                            e.Handled = true;
                        }
                        break;
                    case geInputMode.Date:
                        // 日付型
                        if ((Key.D0 <= e.Key && e.Key <= Key.D9) || (Key.NumPad0 <= e.Key && e.Key <= Key.NumPad9) ||
                            (Key.F1 <= e.Key && e.Key <= Key.F12) ||
                            (Key.Shift == e.Key) || (Key.Ctrl == e.Key) ||
                            (Key.Right == e.Key) || (Key.Left == e.Key) ||
                            (Key.Delete == e.Key) || (Key.Back == e.Key) || (Key.Tab == e.Key) ||
                            (Key.Home == e.Key) || (Key.End == e.Key) || (Key.Enter == e.Key))
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            // ここで止める
                            e.Handled = true;
                        }
                        break;
                    default:
                        if (Keyboard.Modifiers == ModifierKeys.Control && Key.V == e.Key)
                        {
                            // 入力バイト数制限を掛けていた場合、コピペでは制御できない為、止める
                            if (ExCast.zCInt(this.MaxLengthB) != 0)
                            {
                                e.Handled = true;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            base.OnKeyDown(e);
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            try
            {
                bool flg = InputCheck2(e.Text);
                
                if (flg == false)
                {
                    e.Handled = true;
                }
                if (e.Handled == false)
                {
                    // 値セット
                    switch (InputMode)
                    {
                        case geInputMode.Number:
                            //if (this.Text.IndexOf(".") != -1)
                            //{
                            //    string format = "0.";
                            //    for (int i = 1; i <= this.DecimalNum; i++)
                            //    {
                            //        format += "0";
                            //    }
                            //    this.Text = ExMath.zFloor(ExCast.zCDbl(this.Text), this.DecimalNum).ToString(format);
                            //}
                            this.Value = ExCast.zCDbl(this.Text).ToString();
                            break;
                        case geInputMode.Alphanumeric:
                            this.Value = this.Text;
                            break;
                        case geInputMode.Date:
                            this.Value = this.Text.Replace("/", "");
                            break;
                        case geInputMode.FullShapeNative:
                            this.Value = this.Text;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            base.OnTextInput(e);
        }

        protected override void OnTextInputUpdate(TextCompositionEventArgs e)
        {
            try
            {
                bool flg = InputCheck(e.Text);

                if (flg == false)
                {
                    e.Handled = true;
                }

                // 値セット
                switch (InputMode)
                {
                    case geInputMode.Number:
                        this.Value = ExCast.zCDbl(this.Text).ToString();
                        break;
                    case geInputMode.Alphanumeric:
                        this.Value = this.Text;
                        break;
                    case geInputMode.Date:
                        this.Value = this.Text.Replace("/", "");
                        break;
                    case geInputMode.FullShapeNative:
                        this.Value = this.Text;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            base.OnTextInputUpdate(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.UpdataFlg = false;
            this.BeforeValue = this.Text;

            base.OnGotFocus(e);

            try
            {
                OnFormatString();

                // 値セット
                switch (InputMode)
                {
                    case geInputMode.ID:
                        this.SelectAll();
                        break;
                    case geInputMode.Number:
                        if (this.Text != "")
                        {
                            this.Text = ExCast.zCDbl(this.Text).ToString();
                        }
                        this.TextAlignment = System.Windows.TextAlignment.Right;
                        this.SelectAll();
                        break;
                    case geInputMode.Date:
                        if (this.Text != "")
                        {
                            this.Text = this.Text.Replace("/", "");
                        }
                        break;
                }
                if (this.IsSelectAll)
                {
                    this.SelectAll();
                }

                SetIme();
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            try
            {
                if (this.BeforeValue == this.Text) this.UpdataFlg = false;
                else this.UpdataFlg = true;

                // 初期化
                intBeforeHour = 0;
                intBeforeMinute = 0;
                intBeforeSecond = 0;
                intBeforeMilliSecond = 0;

                // 値セット
                switch (InputMode)
                {
                    case geInputMode.Number:
                        this.Value = ExCast.zCDbl(this.Text).ToString();
                        break;
                    case geInputMode.Alphanumeric:
                        this.Value = this.Text;
                        break;
                    case geInputMode.Date:
                        if (this.Text == "") { break; }
                        if (this.Text.Length != 8) { this.Text = ""; break; }
                        this.Value = this.Text;
                        break;
                    case geInputMode.FullShapeNative:
                        this.Value = this.Text;
                        break;
                }

                if (this.NullToZero == true)
                {
                    if (string.IsNullOrEmpty(this.Text))
                    {
                        this.Text = "0";
                    }
                }

                // フォーマット
                OnFormatString();

            }
            catch (Exception ex)
            {
            }
            this._IsDoubleFocusFlg = false;
            base.OnLostFocus(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            bool _flg = false;

            try
            {
                DateTime dtNow = DateTime.Now;

                // 前回ミリ秒設定有り時
                if (intBeforeHour != 0 || intBeforeMinute != 0 || intBeforeSecond != 0 || intBeforeMilliSecond != 0)
                {
                    // 今回ミリ秒取得
                    int intHour = dtNow.Hour;
                    int intMinute = dtNow.Minute;
                    int intSecond = dtNow.Second;
                    int intMilliSecond = dtNow.Millisecond;

                    // 時間間隔取得
                    TimeSpan ts = new TimeSpan(0, intHour, intMinute, intSecond, intMilliSecond);
                    TimeSpan tsBefore = new TimeSpan(0, intBeforeHour, intBeforeMinute, intBeforeSecond, intBeforeMilliSecond);
                    TimeSpan diff = tsBefore - ts;
                    double dbl = ExCast.zCDbl(diff.Duration().ToString().Replace(":", ""));

                    // 初期化
                    intBeforeHour = 0;
                    intBeforeMinute = 0;
                    intBeforeSecond = 0;
                    intBeforeMilliSecond = 0;

                    // マウスダウンの時間間隔が0.5秒以内
                    if (dbl <= 0.5)
                    {
                        OnMouseDoubleClick(this, e);
                        _flg = true;
                    }
                }
                else
                {
                    // 前回時刻設定
                    intBeforeHour = dtNow.Hour;
                    intBeforeMinute = dtNow.Minute;
                    intBeforeSecond = dtNow.Second;
                    intBeforeMilliSecond = dtNow.Millisecond;
                }

                if (this._IsDataGridSelectCell && _flg == false)
                {
                    ExDataGrid dg = (ExDataGrid)ExVisualTreeHelper.FindPerentDataGrid(this);
                    if (dg != null)
                    {
                        dg.SelectedCell(ExCast.zCInt(this.DataGridRecNo), this.DataGridSelectedColumnIndex);
                        //ExBackgroundWorker.DoWork_DataGridSelectCell(dg, ExCast.zCInt(this.DataGridRecNo) - 1, this.DataGridSelectedColumnIndex);
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.ZeroToNull == true)
            {
                if (this.Text != null)
                {
                    if (this.Text == "0")
                    {
                        this.Text = "";
                    }
                }
            }
        }

        #endregion

        #region Method

        public void OnFormatString()
        {
            switch (InputMode)
            {
                case geInputMode.Date:
                    if (this.Text == "") { break; }
                    if (this.Text.Length != 8) { break; }
                    this.Text = this.Text.Substring(0, 4) + "/" + this.Text.Substring(4, 2) + "/" + this.Text.Substring(6, 2);
                    break;
                case geInputMode.Number:
                    if (this.Text == "") { break; }
                    if (this.FormatString.IndexOf(".") == -1 && this.DecimalNum > 0)
                    {
                        this.FormatString = "#,##0.00";
                        //string format = "";
                        //if (this.FormatString == "")
                        //{
                        //    format = "0.";
                        //}
                        //else
                        //{
                        //    format = FormatString + ".";
                        //}
                        //for (int i = 1; i <= this.DecimalNum; i++)
                        //{
                        //    format += "0";
                        //}
                        this.Text = ExCast.zCDbl(this.Text).ToString(this.FormatString);
                        if (this.DecimalNum > 0)
                        {
                            int _length = this.Text.Length;
                            if (this.DecimalNum == 1)
                            {
                                this.Text = this.Text.Substring(0, this.Text.Length - 1) + "0";
                            }
                        }
                    }
                    else
                    {
                        this.Text = ExCast.zCDbl(this.Text).ToString(this.FormatString);
                        // 上記だけではなぜか設定されない
                        string str = ExCast.zCDbl(this.Text).ToString(this.FormatString);
                        this.Text = str;

                    }
                    break;
                case geInputMode.ID:
                    if (this.Text == "") { break; }
                    if (ExCast.zCInt(this.MaxLengthB) == 0) { break; }
                    if (ExCast.IsNumeric(this.Text) == false) { break; }

                    FormatToID();
                    break;
                default:
                    break;
            }
        }

        public void FormatToID()
        {
            if (InputMode != geInputMode.ID) return;
            if (this.Text == "") return;
            if (ExCast.zCInt(this.MaxLengthB) == 0) return;
            if (ExCast.IsNumeric(this.Text) == false) return;

            string str0 = "";
            for (int i = 1; i <= ExCast.zCInt(this.MaxLengthB); i++)
            {
                str0 += "0";
            }
            string str = ExCast.zCDbl(this.Text).ToString(str0);
            this.Text = str;
            this.Text = str;
        }

        public virtual void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MouseDoubleClick != null)
            {
                MouseDoubleClick(this, e);
            }
        }

        public void SetIme()
        {
            //string _text = "";
            //_text = this.Text;
            //this.Text = " ";
            //this.SelectAll();
            //this.Select(0, 0);
            //this.Text = _text;
            //ExBackgroundWorkerDoWork_SetIme(this, this.InputMode);
            InputMethod.SetIsInputMethodEnabled(this, true);

            switch (InputMode)
            {
                case geInputMode.ID:
                    this.TextAlignment = System.Windows.TextAlignment.Right;
                    InputMethod.SetPreferredImeState(this, InputMethodState.Off);
                    InputMethod.SetIsInputMethodEnabled(this, false);
                    break;
                case geInputMode.Number:
                    this.TextAlignment = System.Windows.TextAlignment.Right;
                    InputMethod.SetPreferredImeState(this, InputMethodState.Off);
                    InputMethod.SetIsInputMethodEnabled(this, false);
                    break;
                case geInputMode.Alphanumeric:
                    this.TextAlignment = System.Windows.TextAlignment.Left;
                    InputMethod.SetPreferredImeState(this, InputMethodState.Off);
                    InputMethod.SetIsInputMethodEnabled(this, false);
                    break;
                case geInputMode.Date:
                    this.TextAlignment = System.Windows.TextAlignment.Center;
                    InputMethod.SetPreferredImeState(this, InputMethodState.Off);
                    InputMethod.SetIsInputMethodEnabled(this, false);
                    break;
                case geInputMode.FullShapeNative:
                    this.TextAlignment = System.Windows.TextAlignment.Left;
                    InputMethod.SetIsInputMethodEnabled(this, true);
                    InputMethod.SetPreferredImeState(this, InputMethodState.On);
                    InputMethod.SetPreferredImeConversionMode(this, ImeConversionModeValues.FullShape | ImeConversionModeValues.Native);
                    break;
                case geInputMode.HalfKana:
                    this.TextAlignment = System.Windows.TextAlignment.Left;
                    InputMethod.SetIsInputMethodEnabled(this, true);
                    InputMethod.SetPreferredImeState(this, InputMethodState.On);
                    InputMethod.SetPreferredImeConversionMode(this, ImeConversionModeValues.Katakana | ImeConversionModeValues.Native);
                    break;
                case geInputMode.FullKana:
                    this.TextAlignment = System.Windows.TextAlignment.Left;
                    InputMethod.SetIsInputMethodEnabled(this, true);
                    InputMethod.SetPreferredImeState(this, InputMethodState.On);
                    InputMethod.SetPreferredImeConversionMode(this, ImeConversionModeValues.Katakana | ImeConversionModeValues.FullShape);
                    break;
            }
        }

        public void SetZeroToNullString()
        {
            if (ExCast.zCInt(this.Text.Trim()) == 0)
            {
                this.Text = "";
            }
        }

        private bool InputCheck(string _text)
        {
            string _strText = "";

            switch (InputMode)
            {
                case geInputMode.Number:
                    // 最大・最小入力値チェック
                    _strText = this.Text;
                    if (this.SelectedText != "")
                    {
                        _strText = this.Text.Replace(this.SelectedText, "");
                    }
                    if (ExCast.zCDbl(_strText + _text) < this.MinNumber || ExCast.zCDbl(_strText + _text) > this.MaxNumber)
                    {
                        return false;
                    }

                    double _dbl = ExCast.zCDbl(_strText + _text);
                    string strText;
                    string str;
                    if (this.DecimalNum > 0)
                    {
                        if (this.DecimalNum == 1)
                        {
                            strText = _dbl.ToString("#,##0.00");
                            str = strText.Substring(strText.Length - 1, 1);
                            if (str != "0") return false;
                        }
                        else if (this.DecimalNum == 2)
                        {
                            strText = _dbl.ToString("#,##0.000");
                            str = strText.Substring(strText.Length - 1, 1);
                            if (str != "0") return false;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    break;
                case geInputMode.ID:
                case geInputMode.Alphanumeric:
                case geInputMode.FullKana:
                case geInputMode.HalfKana:
                case geInputMode.FullShapeNative:
                    if (ExCast.zCInt(this.MaxLengthB) != 0)
                    {
                        if (ExString.IsFullString(_text) || ExString.LenB(_text) >= 2)
                        {
                            // 最大入力バイト数チェック
                            if (ExString.LenB(this.Text) - ExString.LenB(this.SelectedText) > ExCast.zCInt(this.MaxLengthB))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            // 最大入力バイト数チェック

                            if (ExString.LenB(this.Text + _text) - ExString.LenB(this.SelectedText) > ExCast.zCInt(this.MaxLengthB))
                            {
                                return false;
                            }
                        }
                    }
                    break;
            }

            // 全角チェック
            switch (InputMode)
            {
                case geInputMode.Number:
                case geInputMode.ID:
                case geInputMode.Alphanumeric:
                //case geInputMode.HalfKana:
                    string strText = "";
                    for (int i = 1; i <= this.Text.Length; i++)
                    {
                        string str = this.Text.Substring(i - 1, 1);
                        byte[] bytes = ExSjisEncoding.ucstojms(str);

                        // 全角は除く
                        if (bytes.Length == 1) strText += str;
                    }
                    if (strText != this.Text) 
                    {
                        return false;
                        //this.Text = strText;
                    }
                    break;
            }

            // 0入力チェック
            switch (InputMode)
            {
                case geInputMode.ID:
                    if (this.Text + _text == "0")
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        private bool InputCheck2(string _text)
        {
            string _strText = "";
            string _chktext = "";
            int _selectionStart = 0;

            switch (InputMode)
            {
                case geInputMode.Number:
                    // 最大・最小入力値チェック
                    _strText = this.Text;
                    if (this.SelectedText != "")
                    {
                        _strText = this.Text.Replace(this.SelectedText, "");
                    }
                    if (ExCast.zCDbl(_strText + _text) < this.MinNumber || ExCast.zCDbl(_strText + _text) > this.MaxNumber)
                    {
                        return false;
                    }

                    double _dbl = ExCast.zCDbl(_strText + _text);
                    string strText;
                    string str;
                    if (this.DecimalNum > 0)
                    {
                        if (this.DecimalNum == 1)
                        {
                            strText = _dbl.ToString("#,##0.00");
                            str = strText.Substring(strText.Length - 1, 1);
                            if (str != "0") return false;
                        }
                        else if (this.DecimalNum == 2)
                        {
                            strText = _dbl.ToString("#,##0.000");
                            str = strText.Substring(strText.Length - 1, 1);
                            if (str != "0") return false;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    break;
                case geInputMode.ID:
                case geInputMode.Alphanumeric:
                case geInputMode.FullKana:
                case geInputMode.HalfKana:
                case geInputMode.FullShapeNative:
                    if (ExCast.zCInt(this.MaxLengthB) != 0)
                    {
                        if (ExString.IsFullString(_text) || ExString.LenB(_text) >= 2)
                        {
                            // 最大入力バイト数チェック
                            if (ExString.LenB(this.Text) - ExString.LenB(this.SelectedText) > ExCast.zCInt(this.MaxLengthB))
                            {
                                _chktext = this.Text + this.SelectedText;
                                for (int i = 1; i <= _chktext.Length; i++)
                                {
                                    _chktext = _chktext.Substring(0, _chktext.Length - 1);
                                    if (ExString.LenB(_chktext) <= ExCast.zCInt(this.MaxLengthB))
                                    {
                                        _selectionStart = this.SelectionStart + _text.Length;
                                        this.Text = _chktext;
                                        this.SelectionStart = _selectionStart;
                                        break;
                                    }
                                }
                                return false;
                            }
                        }
                        else
                        {
                            // 最大入力バイト数チェック

                            if (ExString.LenB(this.Text + _text) - ExString.LenB(this.SelectedText) > ExCast.zCInt(this.MaxLengthB))
                            {
                                _chktext = this.Text + _text + this.SelectedText;
                                for (int i = 1; i <= _chktext.Length; i++)
                                {
                                    _chktext = _chktext.Substring(0, _chktext.Length - 1);
                                    if (ExString.LenB(_chktext) <= ExCast.zCInt(this.MaxLengthB))
                                    {
                                        this.Text = _chktext;
                                        break;
                                    }
                                }
                                return false;
                            }
                        }
                    }
                    break;
            }

            // 全角チェック
            switch (InputMode)
            {
                case geInputMode.Number:
                case geInputMode.ID:
                case geInputMode.Alphanumeric:
                    //case geInputMode.HalfKana:
                    string strText = "";
                    for (int i = 1; i <= this.Text.Length; i++)
                    {
                        string str = this.Text.Substring(i - 1, 1);
                        byte[] bytes = ExSjisEncoding.ucstojms(str);

                        // 全角は除く
                        if (bytes.Length == 1) strText += str;
                    }
                    if (strText != this.Text)
                    {
                        return false;
                        //this.Text = strText;
                    }
                    break;
            }

            // 0入力チェック
            switch (InputMode)
            {
                case geInputMode.ID:
                    if (this.Text + _text == "0")
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        private void SetIdFigure()
        {
            if (this.InputMode == geInputMode.ID && this._IdFigureType != geIdFigureType.None)
            {
                switch (this._IdFigureType)
                {
                    case geIdFigureType.Commodity:
                        this.MaxLengthB = Common.gintidFigureCommodity;
                        break;
                    case geIdFigureType.Customer:
                        this.MaxLengthB = Common.gintidFigureCustomer;
                        break;
                    case geIdFigureType.Purchase:
                        this.MaxLengthB = Common.gintidFigurePurchase;
                        break;
                }
            }
        }

        #endregion

    }
}
