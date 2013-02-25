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
using SlvHanbaiClient.Class.UI;

namespace SlvHanbaiClient.Class.Utility
{
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
            }

            System.Threading.Thread.Sleep(1000);
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
            _utl.InputCheckUpdate();
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //this.tbProgress.Text = (e.ProgressPercentage.ToString() + "%");
        }


    }
}
