#region using

using System;
using System.Collections;
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
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.Class.UI
{
    public class ExDataGrid : DataGrid
    {
        
        #region Filed

        private geEnterKeyDown _enterKeyDown = geEnterKeyDown.On;
        public geEnterKeyDown enterKeyDown { set { this._enterKeyDown = value; } get { return this._enterKeyDown; } }
        public enum geEnterKeyDown { Off = 0, On };

        private geKeyEnterDoubleClick _enterKeyDoubleClick = geKeyEnterDoubleClick.On;
        public geKeyEnterDoubleClick enterKeyDoubleClick { set { this._enterKeyDoubleClick = value; } get { return this._enterKeyDoubleClick; } }
        public enum geKeyEnterDoubleClick { Off = 0, On };

        private const int DoubleClickDureationMilliSeconds = 250;
        private LastClickInfo lastClickInfo = LastClickInfo.Empty;

        private string _ComboBoxColumnIndex = "";
        public string ComboBoxColumnIndex { set { this._ComboBoxColumnIndex = value; } get { return this._ComboBoxColumnIndex; } }

        private bool _IsMinusIndex = false;
        public bool IsMinusIndex { set { this._IsMinusIndex = value; } get { return this._IsMinusIndex; } }

        /// <summary>
        /// ダブルクリックされたときに発行する。
        /// </summary>
        public event EventHandler DoubleClick;

        private bool IsKeyDown = false;
        private bool IsKeyUp = false;

        private bool IsKeyEnterDoubleClick = false;

        private bool _IsBeginEdit = false;
        public bool IsBeginEdit { set { this._IsBeginEdit = value; } get { return this._IsBeginEdit; } }

        #endregion

        #region Constructor

        public ExDataGrid()

            : base()
        {

        }

        #endregion

        #region Events

        protected override void OnRowEditEnded(DataGridRowEditEndedEventArgs e)
        {
            base.OnRowEditEnded(e);
        }

        /// <summary>
        /// System.Windows.Controls.DataGrid.LoadingRow イベントを発生させる。
        /// </summary>
        /// <param name="e">イベントのデータ。</param>
        protected override void OnLoadingRow(DataGridRowEventArgs e)
        {
            base.OnLoadingRow(e);

            DataGridRow row = e.Row;

            // DataGridRowにダブルクリック検知用イベントハンドラを追加する。
            row.MouseLeftButtonUp -= new MouseButtonEventHandler(this.Row_MouseLeftButtonUp);
            row.MouseLeftButtonUp += new MouseButtonEventHandler(this.Row_MouseLeftButtonUp);

        }

        /// <summary>
        /// System.Windows.Controls.DataGrid.UnloadingRow イベントを発生させる。
        /// </summary>
        /// <param name="e">イベントのデータ。</param>
        protected override void OnUnloadingRow(DataGridRowEventArgs e)
        {
            base.OnUnloadingRow(e);

            e.Row.MouseLeftButtonUp -= new MouseButtonEventHandler(this.Row_MouseLeftButtonUp);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            int _cnt = 0;
            int _index = 0;

            try
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        if (enterKeyDown == geEnterKeyDown.On)
                        {
                            if (this.IsKeyDown == true)
                            {
                                this.IsKeyDown = false;
                                this.IsKeyUp = true;
                            }
                            else
                            {
                                if (this.IsKeyUp == true)
                                {
                                    this.IsKeyUp = false;
                                    if (Keyboard.Modifiers == ModifierKeys.Shift)
                                    {
                                        this.MoveBeforeCell();
                                    }
                                    else
                                    {
                                        this.MoveNextCell();
                                    }
                                }
                            }
                        }
                        e.Handled = true;
                        break;
                    case Key.Down:
                    //case Key.PageDown:
                        IEnumerable lst = this.ItemsSource;

                        if (this.SelectedIndex == -1) break;
                        if (lst == null) break;
                        if (this.CurrentColumn == null) break;

                        IEnumerator _lst = lst.GetEnumerator();

                        while (_lst.MoveNext())
                        {
                            _cnt += 1;
                        }

                        if (_cnt <= this.SelectedIndex + 1) break;

                        _index = this.CurrentColumn.DisplayIndex;

                        if (IsComboBoxColumnIndex(_index)) break;

                        this.SelectedCell(this.SelectedIndex + 2, _index);
                        this.ScrollIntoView(this.CurrentItem, this.CurrentColumn);

                        e.Handled = true;
                        break;
                    case Key.Up:
                    //case Key.PageUp:
                        if (this.ItemsSource == null) break;
                        if (this.SelectedIndex == -1) break;
                        if (this.SelectedIndex == 0)
                        {
                            this.OnEndControlFocus();
                            break;
                        }
                        if (this.CurrentColumn == null) break;

                        _index = this.CurrentColumn.DisplayIndex;

                        if (IsComboBoxColumnIndex(_index)) break;

                        this.SelectedCell(this.SelectedIndex, _index);
                        this.ScrollIntoView(this.CurrentItem, this.CurrentColumn);

                        e.Handled = true;
                        break;
                }
            }
            catch
            { 
            }
            base.OnKeyUp(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            int _cnt = 0;
            int _index = 0;

            switch (e.Key)
            {
                case Key.Down:
                case Key.PageDown:
                case Key.Up:
                case Key.PageUp:
                    // 無効に(KeyUpで制御)
                    e.Handled = true;
                    break;
                case Key.Tab:

                case Key.Enter:
                    if (enterKeyDoubleClick == geKeyEnterDoubleClick.On)
                    {
                        e.Handled = true;
                        if (e.Key != Key.Tab && this.DoubleClick != null)
                        {
                            //// そのまま実行すると固まる為の処置
                            Action foo = null;
                            foo = () =>
                            {
                                this.DoubleClick(null, EventArgs.Empty);
                            };
                            Dispatcher.BeginInvoke(foo);

                        }
                    }
                    if (enterKeyDown == geEnterKeyDown.On)
                    {
                        this.IsKeyDown = true;
                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            this.MoveBeforeCell();
                        }
                        else
                        {
                            this.MoveNextCell();
                        }
                        e.Handled = true;
                    }
                    break;
            }

            base.OnKeyDown(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (this.CurrentColumn != null)
            {
                this.BeginEdit();
            }
            base.OnGotFocus(e);
            this.IsKeyUp = false;
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            //this.CommitEdit();
            base.OnLostFocus(e);
        }

        #endregion

        #region Method

        public void MoveNextCell()
        {

            DataGridColumn currentcol = this.CurrentColumn;

            // 現在のカラムが最大かどうか
            bool isLastCol = (currentcol.DisplayIndex == this.Columns.Count - 2);

            if (!isLastCol)
            {
                // 編集を終了して次へ
                this.CommitEdit();
                if (IsMinusIndex == true)
                {
                    int before_selectedIndex = this.SelectedIndex;
                    if (before_selectedIndex >= 1)
                    {
                        this.SelectedIndex = before_selectedIndex - 1;
                    }
                }
                this.CurrentColumn = this.Columns[currentcol.DisplayIndex + 1];
                this.BeginEdit();
            }
            else
            {
                int nextIndex = this.SelectedIndex + 1;

                if (this.ItemsSource is ICollection)
                {
                    ICollection lists = (ICollection)this.ItemsSource;
                    if (nextIndex <= (lists.Count - 1))
                    {
                        this.CurrentColumn = this.Columns[0];
                        this.SelectedIndex = nextIndex;
                    }
                    else
                    {
                        ExUserControl utl = (ExUserControl)ExVisualTreeHelper.FindPerentUserControl(this);
                        utl.btnF7_Click(null, null);
                        return;
                    }
                }
            }
            this.ScrollIntoView(this.CurrentItem, this.CurrentColumn);
        }

        public void MoveBeforeCell()
        {
            DataGridColumn currentcol = this.CurrentColumn;

            if (this.SelectedIndex == -1)
            {
                this.CurrentColumn = this.Columns[0];
                this.SelectedIndex = 0;
                return;
            }

            if (currentcol.DisplayIndex == 0 && this.SelectedIndex == 0)
            {
                this.OnEndControlFocus();
                return;
            }

            // 現在のカラムが最初かどうか
            bool isFirstCol = (currentcol.DisplayIndex == 0);

            if (!isFirstCol)
            {
                // 編集を終了して次へ
                this.CommitEdit();
                if (IsMinusIndex == true)
                {
                    int before_selectedIndex = this.SelectedIndex;
                    if (before_selectedIndex >= 1)
                    {
                        this.SelectedIndex = before_selectedIndex - 1;
                    }
                }
                this.CurrentColumn = this.Columns[currentcol.DisplayIndex - 1];
                this.BeginEdit();
            }
            else
            {
                int nextIndex = this.SelectedIndex - 1;

                this.CurrentColumn = this.Columns[this.Columns.Count - 2];
                this.SelectedIndex = nextIndex;
            }
            this.ScrollIntoView(this.CurrentItem, this.CurrentColumn);
        }

        public void SelectedFirst()
        {
            try
            {
                this.CommitEdit();
                this.CurrentColumn = this.Columns[0];
                this.BeginEdit();

                this.SelectedIndex = 0;
                this.ScrollIntoView(this.CurrentItem, this.CurrentColumn);
            }
            catch
            { 
            }
        }

        public void SelectedCell(int _RowIndex, int _ColumnIndex)
        {
            try
            {
                if (_RowIndex == 0) return;
                this.SelectedIndex = _RowIndex - 1;
                this.CurrentColumn = this.Columns[_ColumnIndex];

                if (this.IsBeginEdit)
                {
                    this.CommitEdit();
                    this.SelectedIndex = _RowIndex - 1;
                    this.CurrentColumn = this.Columns[_ColumnIndex];
                    this.BeginEdit();
                }
            }
            catch
            {
            }
        }

        private void Row_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime lastClickTime = this.lastClickInfo.Time;
            DataGridRow lastClickRow = this.lastClickInfo.Row;

            // 直前にクリックされた行が今回クリックされた行と一致し、
            // かつ、クリックされた間隔が一定時間以内に収まっているか?
            if (lastClickRow == sender
                && (now - lastClickTime).TotalMilliseconds < DoubleClickDureationMilliSeconds)
            {
                // 直前にクリックされた情報を初期化。
                this.lastClickInfo = LastClickInfo.Empty;

                // ダブルクリックイベントを発行
                this.OnDoubleClick(sender as DataGridRow);
            }
            else
            {
                // クリックした行情報を保持しておく
                this.lastClickInfo = new LastClickInfo(now, sender as DataGridRow);
            }
        }

        private void OnDoubleClick(DataGridRow row)
        {
            if (this.DoubleClick != null)
            {
                //try
                //{
                //    MessageBox.Show("a");
                //    this.DoubleClick(row, EventArgs.Empty);
                //}
                //catch
                //{
                //}

                // そのまま実行すると固まる為の処置
                Action foo = null;
                foo = () =>
                {
                    this.DoubleClick(row, EventArgs.Empty);
                };
                Dispatcher.BeginInvoke(foo);
            }
        }

        public bool ConfirmEditEnd()
        {
            DataGridColumn dgCol = null;
            for (int i = 1; i <= 10; i++)
            {
                try
                {
                    this.CommitEdit();
                    dgCol = this.CurrentColumn;
                }
                catch
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
            if (dgCol == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // コンボボックスではKeyDown,Upは規定の動作
        private bool IsComboBoxColumnIndex(int _index)
        {
            if (string.IsNullOrEmpty(this._ComboBoxColumnIndex)) return false;

            string[] colIndex = this._ComboBoxColumnIndex.Split(',');
            if (colIndex.Length == 0) return false;

            // 数値項目のみ制御を行う(ひらがなや半角英数項目はなぜかいく)
            for (int i = 0; i <= colIndex.Length - 1; i++)
            {
                if (ExCast.IsNumeric(colIndex[i]))
                {
                    if (_index == ExCast.zCInt(colIndex[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void OnEndControlFocus()
        {
            ExUserControl utl = (ExUserControl)ExVisualTreeHelper.FindPerentUserControl(this);
                            if (utl == null) return;
                            Control ctl = ExVisualTreeHelper.SearchControlForTag(utl, "End");
                            if (ctl == null) return;
                            this.CommitEdit();
                            ExBackgroundWorker.DoWork_Focus(ctl, 10);
                            ExBackgroundWorker.DoWork_Focus(ctl, 100);
        }

        #endregion

        #region Class

        /// <summary> クリックした行情報を保持する。 </summary>
        private class LastClickInfo
        {
            public static readonly LastClickInfo Empty = new LastClickInfo(DateTime.MinValue, null);

            public LastClickInfo(DateTime time, DataGridRow row)
            {
                this.Time = time;
                this.Row = row;
            }

            /// <summary> クリックされた時間を取得する。 </summary>
            public DateTime Time { get; private set; }

            /// <summary> クリックされた行を取得する。 </summary>
            public DataGridRow Row { get; private set; }
        }

        #endregion

    }

    public class ExDataGridUtilty
    {
        public static void zCommitEdit(ExDataGrid dg)
        {
            try
            {
            }
            catch
            { 
            }
        }
    }
}
