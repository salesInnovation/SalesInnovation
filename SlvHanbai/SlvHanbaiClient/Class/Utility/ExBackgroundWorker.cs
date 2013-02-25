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
using System.ComponentModel;
using System.Collections.ObjectModel;  // ObservableCollectionを利用するために必要   
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.svcCondition;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Report;

namespace SlvHanbaiClient.Class.Utility
{
    public class ExBackgroundWorkerExec
    {
        private Control ctl = null;
        private ExDataGrid dg = null;
        private DataGridColumn dataGridColumn;
        private object obj = null;
        private int waitTime = 0;
        private int selectIndex = 0;
        private ExTextBox.geInputMode InputMode = ExTextBox.geInputMode.Alphanumeric;

        public void DoFocus(Control _ctl, int _waitTime)
        {
            BackgroundWorker bw = new BackgroundWorker();
            ctl = _ctl;
            waitTime = _waitTime;
            bw.DoWork -= new DoWorkEventHandler(bw_DoFocus);
            bw.DoWork += new DoWorkEventHandler(bw_DoFocus);
            bw.RunWorkerAsync();
        }

        public void DoSelectedIndex(ExDataGrid _dg, int _waitTime, int _selectIndex)
        {
            BackgroundWorker bw = new BackgroundWorker();
            dg = _dg;
            waitTime = _waitTime;
            selectIndex = _selectIndex;
            bw.DoWork -= new DoWorkEventHandler(bw_DoSelectedIndex);
            bw.DoWork += new DoWorkEventHandler(bw_DoSelectedIndex);
            bw.RunWorkerAsync();
        }

        public void DoDataGridColumn(ExDataGrid _dg, int _waitTime, DataGridColumn _dataGridColumn)
        {
            BackgroundWorker bw = new BackgroundWorker();
            dg = _dg;
            waitTime = _waitTime;
            dataGridColumn = _dataGridColumn;
            bw.DoWork -= new DoWorkEventHandler(bw_DoDataGridColumn);
            bw.DoWork += new DoWorkEventHandler(bw_DoDataGridColumn);
            bw.RunWorkerAsync();
        }

        public void DoDataGridItemSource(ExDataGrid _dg, int _waitTime, object _obj)
        {
            BackgroundWorker bw = new BackgroundWorker();
            dg = _dg;
            waitTime = _waitTime;
            obj = _obj;
            bw.DoWork -= new DoWorkEventHandler(bw_DoDataGridItemSource);
            bw.DoWork += new DoWorkEventHandler(bw_DoDataGridItemSource);
            bw.RunWorkerAsync();
        }

        public void DoDataGridSelectedFirst(ExDataGrid _dg, int _waitTime)
        {
            BackgroundWorker bw = new BackgroundWorker();
            dg = _dg;
            waitTime = _waitTime;
            bw.DoWork -= new DoWorkEventHandler(bw_DoDataGridSelectedFirst);
            bw.DoWork += new DoWorkEventHandler(bw_DoDataGridSelectedFirst);
            bw.RunWorkerAsync();
        }

        public void DoStartUpInstanceSet()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork -= new DoWorkEventHandler(bw_DoStartUpInstanceSet);
            bw.DoWork += new DoWorkEventHandler(bw_DoStartUpInstanceSet);
            bw.RunWorkerAsync();
        }

        public void DoSetIme(Control _ctl, ExTextBox.geInputMode _InputMode)
        {
            BackgroundWorker bw = new BackgroundWorker();
            ctl = _ctl;
            InputMode = _InputMode;
            bw.DoWork -= new DoWorkEventHandler(bw_DoSetIme);
            bw.DoWork += new DoWorkEventHandler(bw_DoSetIme);
            bw.RunWorkerAsync();
        }

        public void DoClose(Control _ctl, int _waitTime)
        {
            BackgroundWorker bw = new BackgroundWorker();
            ctl = _ctl;
            waitTime = _waitTime;
            bw.DoWork -= new DoWorkEventHandler(bw_DoClose);
            bw.DoWork += new DoWorkEventHandler(bw_DoClose);
            bw.RunWorkerAsync();
        }

        private void bw_DoSetIme(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            ExTextBox txt = (ExTextBox)ctl;
            txt.Dispatcher.BeginInvoke(
                () =>
                {
                    InputMethod.SetIsInputMethodEnabled(txt, true);
                }
            );

            switch (this.InputMode)
            {
                case ExTextBox.geInputMode.Number:
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            InputMethod.SetPreferredImeState(txt, InputMethodState.Off);
                        }
                    );
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            txt.TextAlignment = System.Windows.TextAlignment.Right;
                        }
                    );
                    break;
                case ExTextBox.geInputMode.Alphanumeric:
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            InputMethod.SetPreferredImeState(txt, InputMethodState.Off);
                        }
                    );
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            txt.TextAlignment = System.Windows.TextAlignment.Left;
                        }
                    );
                    break;
                case ExTextBox.geInputMode.Date:
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            InputMethod.SetPreferredImeState(txt, InputMethodState.Off);
                        }
                    );
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            txt.TextAlignment = System.Windows.TextAlignment.Center;
                        }
                    );
                    break;
                case ExTextBox.geInputMode.FullShapeNative:
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            InputMethod.SetPreferredImeState(txt, InputMethodState.On);
                        }
                    );
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            InputMethod.SetPreferredImeConversionMode(txt, ImeConversionModeValues.FullShape | ImeConversionModeValues.Native | ImeConversionModeValues.Roman);
                        }
                    );
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            txt.TextAlignment = System.Windows.TextAlignment.Left;
                        }
                    );
                    break;
                case ExTextBox.geInputMode.HalfKana:
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            InputMethod.SetPreferredImeState(txt, InputMethodState.On);
                        }
                    );
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            InputMethod.SetPreferredImeConversionMode(txt, ImeConversionModeValues.FullShape | ImeConversionModeValues.Native | ImeConversionModeValues.Katakana);
                        }
                    );
                    txt.Dispatcher.BeginInvoke(
                        () =>
                        {
                            txt.TextAlignment = System.Windows.TextAlignment.Left;
                        }
                    );
                    break;
            }
        }

        private void bw_DoFocus(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (this.ctl == null) return;
            if (waitTime != 0) System.Threading.Thread.Sleep(waitTime);

            //bool _flg = true;
            //ctl.Dispatcher.BeginInvoke(() => _flg = ctl.IsTabStop);
            //if (_flg == false) return;

            ctl.Dispatcher.BeginInvoke(
                () =>
                {
                    ctl.Focus();
                }
            );
        }

        private void bw_DoClose(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            ExChildWindow win = null;

            if (ctl == null) return;

            try
            {
                win = (ExChildWindow)ctl;
            }
            catch
            {
                return;
            }

            if (waitTime != 0) System.Threading.Thread.Sleep(waitTime);

            win.Dispatcher.BeginInvoke(
                () =>
                {
                    win.Close();
                }
            );
        }

        private void bw_DoSelectedIndex(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (this.dg == null) return;
            if (waitTime != 0) System.Threading.Thread.Sleep(waitTime);

            dg.Dispatcher.BeginInvoke(
                () =>
                {
                    dg.SelectedIndex = this.selectIndex;
                }
            );
        }

        private void bw_DoDataGridColumn(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (this.dg == null) return;
            if (this.dataGridColumn == null) return;
            if (waitTime != 0) System.Threading.Thread.Sleep(waitTime);

            dg.Dispatcher.BeginInvoke(
                () =>
                {
                    dg.CurrentColumn = this.dataGridColumn;
                }
            );

            dg.Dispatcher.BeginInvoke(
                () =>
                {
                    dg.MoveNextCell();
                }
            );

            dg.Dispatcher.BeginInvoke(
                () =>
                {
                    dg.MoveBeforeCell();
                }
            );

        }

        private void bw_DoDataGridItemSource(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (this.dg == null) return;
            if (this.obj == null) return;
            if (waitTime != 0) System.Threading.Thread.Sleep(waitTime);

            dg.Dispatcher.BeginInvoke(
                () =>
                {
                    dg.ItemsSource = (ObservableCollection<EntityCondition>)this.obj;
                }
            );
        }

        private void bw_DoDataGridSelectedFirst(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (this.dg == null) return;
            if (waitTime != 0) System.Threading.Thread.Sleep(waitTime);

            dg.Dispatcher.BeginInvoke(
                () =>
                {
                    dg.Focus();
                    dg.SelectedFirst();
                }
            );
        }

        private static void bw_DoStartUpInstanceSet(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Dlg_InpSearch InpSearch = null;

            Common.reportView.Dispatcher.BeginInvoke(
                () =>
                {
                    Common.reportView = new View.Dlg.Report.Dlg_ReportView();
                }
            );

            InpSearch.Dispatcher.BeginInvoke(
                () =>
                {
                    InpSearch = Common.InpSearchEstimate;
                }
            );
        }

    }

    public class ExBackgroundWorker
    {

        public static void DoWork_Focus(Control _ctl, int _waitTime)
        {
            try
            {
                ExBackgroundWorkerExec bw = new ExBackgroundWorkerExec();
                bw.DoFocus(_ctl, _waitTime);
            }
            catch
            { 
            }
        }

        public static void DoWork_FocusForLoad(Control _ctl)
        {
            try
            {
                ExBackgroundWorker.DoWork_Focus(_ctl, 10);
                ExBackgroundWorker.DoWork_Focus(_ctl, 50);
                ExBackgroundWorker.DoWork_Focus(_ctl, 100);
                ExBackgroundWorker.DoWork_Focus(_ctl, 200);
                ExBackgroundWorker.DoWork_Focus(_ctl, 300);
                ExBackgroundWorker.DoWork_Focus(_ctl, 400);
            }
            catch
            {
            }
        }

        public static void DoWork_SetIme(Control _ctl, ExTextBox.geInputMode _InputMode)
        {
            try
            {
                ExBackgroundWorkerExec bw = new ExBackgroundWorkerExec();
                bw.DoSetIme(_ctl, _InputMode);
            }
            catch
            {
            }
        }

        public static void DoWork_Close(Control _ctl, int _waitTime)
        {
            try
            {
                ExBackgroundWorkerExec bw = new ExBackgroundWorkerExec();
                bw.DoClose(_ctl, _waitTime);
            }
            catch
            {
            }
        }

        public static void DoWork_SelectedIndex(ExDataGrid _dg, int _waitTime, int _selectIndex)
        {
            try
            {
                ExBackgroundWorkerExec bw = new ExBackgroundWorkerExec();
                bw.DoSelectedIndex(_dg, _waitTime, _selectIndex);
            }
            catch
            {
            }
        }

        public static void DoWork_DataGridColum(ExDataGrid _dg, int _waitTime, DataGridColumn _dataGridColumn)
        {
            try
            {
                ExBackgroundWorkerExec bw = new ExBackgroundWorkerExec();
                bw.DoDataGridColumn(_dg, _waitTime, _dataGridColumn);
            }
            catch
            {
            }
        }

        public static void DoWork_DataGridSelectCell(ExDataGrid _dg, int _selectIndex, int _dataGridColIndex)
        {
            try
            {
                // DataGridにItemsourceを設定して、設定が確定するのは非同期で若干のタイムラグがある為の処理
                ExBackgroundWorker.DoWork_Focus(_dg, 0);
                ExBackgroundWorker.DoWork_SelectedIndex(_dg, 10, _selectIndex);
                ExBackgroundWorker.DoWork_DataGridColum(_dg, 20, _dg.Columns[_dataGridColIndex]);
            }
            catch
            {
            }
        }

        public static void DoWork_SelectDgCell(ExDataGrid _dg, int _selectIndex, int _dataGridColIndex)
        {
            try
            {
                // DataGridにItemsourceを設定して、設定が確定するのは非同期で若干のタイムラグがある為の処理
                ExBackgroundWorker.DoWork_Focus(_dg, 10);
                ExBackgroundWorker.DoWork_SelectedIndex(_dg, 20, _selectIndex);
                ExBackgroundWorker.DoWork_DataGridColum(_dg, 100, _dg.Columns[_dataGridColIndex]);
                System.Threading.Thread.Sleep(300);
            }
            catch
            {
            }
        }

        public static void DoWork_DataGridItemSource(ExDataGrid _dg, int _waitTime, object _obj)
        {
            try
            {
                ExBackgroundWorkerExec bw = new ExBackgroundWorkerExec();
                bw.DoDataGridItemSource(_dg, _waitTime, _obj);
            }
            catch
            {
            }
        }

        public static void DoWork_DataGridSelectedFirst(ExDataGrid _dg, int _waitTime)
        {
            try
            {
                ExBackgroundWorkerExec bw = new ExBackgroundWorkerExec();
                bw.DoDataGridSelectedFirst(_dg, _waitTime);
            }
            catch
            {
            }
        }

        public static void DoWork_StartUpInstanceSet()
        {
            try
            {
                ExBackgroundWorkerExec bw = new ExBackgroundWorkerExec();
                bw.DoStartUpInstanceSet();
            }
            catch
            {
            }
        }

    }

    // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
    public class ExBackgroundInputCheckWk
    {
        private Control _focusCtl = null;
        public Control focusCtl
        {
            set
            {
                this._focusCtl = value;
            }
            get
            {
                return this._focusCtl;
            }
        }

        private ExUserControl _utl = null;
        public ExUserControl utl
        {
            set
            {
                this._utl = value;
            }
            get
            {
                return this._utl;
            }
        }

        private ExChildWindow _win = new ExChildWindow();
        public ExChildWindow win
        {
            set
            {
                this._win = value;
            }
            get
            {
                return this._win;
            }
        }

        private BackgroundWorker _bw = new BackgroundWorker();
        public BackgroundWorker bw
        {
            set
            {
                this._bw = value;
            }
            get
            {
                return this._bw;
            }
        }

        public int waitTime = 0;


        public ExBackgroundInputCheckWk()
        {
            _bw.WorkerReportsProgress = false;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this._focusCtl != null)
            {
                // 入力確定の為の処理
                this._focusCtl.Dispatcher.BeginInvoke(() =>
                {
                    _focusCtl.Focus();
                });

                if (waitTime != 0)
                {
                    System.Threading.Thread.Sleep(waitTime);
                }
                System.Threading.Thread.Sleep(100);
            }            
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                //this.tbProgress.Text = "Canceled!";
            }
            else if (!(e.Error == null))
            {
                //this.tbProgress.Text = ("Error: " + e.Error.Message);
            }
            else
            {
                //this.tbProgress.Text = "Done!";
            }

            if (_utl != null)
            {
                _utl.InputCheckUpdate();
            }
            else
            {
                _win.InputCheckUpdate();
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //this.tbProgress.Text = (e.ProgressPercentage.ToString() + "%");
        }

    }

    // 入力確定の為、BackgroundWorker経由で行挿入を呼出
    public class ExBackgroundRecordAddWk
    {
        private Control _focusCtl = null;
        public Control focusCtl
        {
            set
            {
                this._focusCtl = value;
            }
            get
            {
                return this._focusCtl;
            }
        }

        private ExUserControl _utl = new ExUserControl();
        public ExUserControl utl
        {
            set
            {
                this._utl = value;
            }
            get
            {
                return this._utl;
            }
        }

        private BackgroundWorker _bw = new BackgroundWorker();
        public BackgroundWorker bw
        {
            set
            {
                this._bw = value;
            }
            get
            {
                return this._bw;
            }
        }

        public bool waitFlg = false;

        public ExBackgroundRecordAddWk()
        {
            _bw.WorkerReportsProgress = false;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            int _cnt = 0;
            if (waitFlg == true)
            {
                while (Common.gblnDesynchronizeLock)
                {
                    System.Threading.Thread.Sleep(100);
                    _cnt += 1;
                    if (_cnt >= 10)
                    {
                        break;
                    }
                }
            }

            if (this._focusCtl != null)
            {
                // 入力確定の為の処理
                this._focusCtl.Dispatcher.BeginInvoke(() =>
                {
                    _focusCtl.Focus();
                });
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _utl.RecordAdd();
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

    }

    // 入力確定の為、BackgroundWorker経由で行挿入を呼出
    public class ExBackgroundReportViewWk
    {
        public Dlg_ReportView reportView;
        public Uri uri;
        private BackgroundWorker _bw = new BackgroundWorker();
        public BackgroundWorker bw
        {
            set
            {
                this._bw = value;
            }
            get
            {
                return this._bw;
            }
        }

        public ExBackgroundReportViewWk()
        {
            _bw.WorkerReportsProgress = false;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(bw_DoReportView);
            _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void bw_DoReportView(object sender, DoWorkEventArgs e)
        {
            try
            {
                // 入力確定の為の処理
                this.reportView.Dispatcher.BeginInvoke(() =>
                {
                    reportView.utlReortView.webBrowser.Source = this.uri;
                });
            }
            catch
            { 
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            reportView.stlProcess.Visibility = Visibility.Collapsed;
            reportView.utlReortView.Visibility = Visibility.Visible;
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

    }

}
