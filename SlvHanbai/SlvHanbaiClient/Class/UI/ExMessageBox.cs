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
using System.Threading;
using System.Windows.Threading;
using SlvHanbaiClient.View.Dlg.MessageBox;

#endregion

namespace SlvHanbaiClient.Class.UI
{
    public class ExMessageBox
    {
        public static ExUserControl utl = null;
        public static ExPage page = null;
        public static ExChildWindow win = null;
        public static MessageBoxResult _MessageBoxResult;
        public static Control errCtl;

        #region Method

        public static void Show(ExPage _page, Control _errCtl, string strMsg, MessageBoxIcon Icon)
        {
            if (string.IsNullOrEmpty(strMsg)) return;

            Dlg_MessagBox msg = null;
            if (Icon == MessageBoxIcon.Error)
            {
                msg = new Dlg_MessagBox("エラー確認", strMsg, MessageBoxButtons.Ok, Icon);
            }
            else
            {
                msg = new Dlg_MessagBox("確認", strMsg, MessageBoxButtons.Ok, Icon);
            }
            msg.Closed -= ExMessageBox._dlg_Closed;
            msg.Closed += ExMessageBox._dlg_Closed;
            msg.Show();
            ExMessageBox.page = _page;
            ExMessageBox.errCtl = _errCtl;

            //MessageBox.Show(strMsg, "確認", MessageBoxButton.OK);
        }

        public static void Show(ExUserControl _utl, Control _errCtl, string strMsg, MessageBoxIcon Icon)
        {
            if (string.IsNullOrEmpty(strMsg)) return;

            Dlg_MessagBox msg = null;
            if (Icon == MessageBoxIcon.Error)
            {
                msg = new Dlg_MessagBox("エラー確認", strMsg, MessageBoxButtons.Ok, Icon);
            }
            else
            {
                msg = new Dlg_MessagBox("確認", strMsg, MessageBoxButtons.Ok, Icon);
            }
            msg.Closed -= ExMessageBox._dlg_Closed;
            msg.Closed += ExMessageBox._dlg_Closed;
            msg.Show();
            ExMessageBox.utl = _utl;
            ExMessageBox.errCtl = _errCtl;

            //MessageBox.Show(strMsg, "確認", MessageBoxButton.OK);
        }

        public static void Show(ExPage _page, Control _errCtl, string strMsg)
        {
            if (string.IsNullOrEmpty(strMsg)) return;

            Dlg_MessagBox msg = new Dlg_MessagBox("確認", strMsg, MessageBoxButtons.Ok, MessageBoxIcon.Information);
            msg.Closed -= ExMessageBox._dlg_Closed;
            msg.Closed += ExMessageBox._dlg_Closed;
            msg.Show();
            ExMessageBox.page = _page;
            ExMessageBox.errCtl = _errCtl;

            //MessageBox.Show(strMsg, "確認", MessageBoxButton.OK);
        }

        public static void Show(ExUserControl _utl, Control _errCtl, string strMsg)
        {
            if (string.IsNullOrEmpty(strMsg)) return;

            Dlg_MessagBox msg = new Dlg_MessagBox("確認", strMsg, MessageBoxButtons.Ok, MessageBoxIcon.Information);
            msg.Closed -= ExMessageBox._dlg_Closed;
            msg.Closed += ExMessageBox._dlg_Closed;
            msg.Show();
            ExMessageBox.utl = _utl;
            ExMessageBox.errCtl = _errCtl;

            //MessageBox.Show(strMsg, "確認", MessageBoxButton.OK);
        }

        private static void _dlg_Closed(object sender, EventArgs e)
        {
            if (ExMessageBox.utl != null)
            {
                utl.ResultMessageBox(ExMessageBox.errCtl);
            }
            else if (ExMessageBox.page != null)
            {
                page.ResultMessageBox(ExMessageBox.errCtl);
            }

            ExMessageBox.utl = null;
            ExMessageBox.page = null;

        }

        public static void Show(string strMsg)
        {
            if (string.IsNullOrEmpty(strMsg)) return;
            if (!string.IsNullOrEmpty(Common.gstrMsgSessionError)) return;

            Dlg_MessagBox msg = new Dlg_MessagBox("確認", strMsg, MessageBoxButtons.Ok, MessageBoxIcon.Information);
            msg.Show();

            if (strMsg.IndexOf("セッションタイムアウトが発生しました。") != -1)
            {
                Common.gstrMsgSessionError = strMsg;
            }

            //MessageBox.Show(strMsg, "確認", MessageBoxButton.OK);
        }

        public static void Show(string strMsg, MessageBoxIcon Icon)
        {
            if (string.IsNullOrEmpty(strMsg)) return;

            Dlg_MessagBox msg = null;
            if (Icon == MessageBoxIcon.Error)
            {
                msg = new Dlg_MessagBox("エラー確認", strMsg, MessageBoxButtons.Ok, Icon);
            }
            else
            {
                msg = new Dlg_MessagBox("確認", strMsg, MessageBoxButtons.Ok, Icon);
            }
            msg.Show();

            //MessageBox.Show(strMsg, "確認", MessageBoxButton.OK);
        }

        public static void Show(string strMsg, string strTitle)
        {
            if (string.IsNullOrEmpty(strMsg)) return;

            Dlg_MessagBox msg = new Dlg_MessagBox(strTitle, strMsg, MessageBoxButtons.Ok, MessageBoxIcon.Information);
            msg.Show();

            //MessageBox.Show(strMsg, strTitle, MessageBoxButton.OK);
        }

        public static void ResultShow(ExUserControl _utl, Control _errCtl, string strMsg)
        {
            if (string.IsNullOrEmpty(strMsg)) return;

            ExMessageBox.utl = _utl;
            ExMessageBox.errCtl = _errCtl;
            Dlg_MessagBox msg = new Dlg_MessagBox("確認", strMsg, MessageBoxButtons.OkCancel, MessageBoxIcon.Question);
            msg.Closed -= ExMessageBox.dlg_Closed;
            msg.Closed += ExMessageBox.dlg_Closed;
            msg.Show();

            //return MessageBox.Show(strMsg, "確認", MessageBoxButton.OKCancel);
        }

        public static void ResultShow(ExPage _page, Control _errCtl, string strMsg)
        {
            if (string.IsNullOrEmpty(strMsg)) return;

            ExMessageBox.page = _page;
            ExMessageBox.errCtl = _errCtl;
            Dlg_MessagBox msg = new Dlg_MessagBox("確認", strMsg, MessageBoxButtons.OkCancel, MessageBoxIcon.Question);
            msg.Closed -= ExMessageBox.dlg_Closed;
            msg.Closed += ExMessageBox.dlg_Closed;
            msg.Show();

            //return MessageBox.Show(strMsg, "確認", MessageBoxButton.OKCancel);
        }

        public static void ResultShow(ExChildWindow _win, Control _errCtl, string strMsg)
        {
            if (string.IsNullOrEmpty(strMsg)) return;

            ExMessageBox.win = _win;
            ExMessageBox.errCtl = _errCtl;
            Dlg_MessagBox msg = new Dlg_MessagBox("確認", strMsg, MessageBoxButtons.OkCancel, MessageBoxIcon.Question);
            msg.Closed -= ExMessageBox.dlg_Closed;
            msg.Closed += ExMessageBox.dlg_Closed;
            msg.Show();

            //return MessageBox.Show(strMsg, "確認", MessageBoxButton.OKCancel);
        }

        public static void ResultShowClr(ExUserControl _utl, Control _errCtl)
        {
            ExMessageBox.utl = _utl;
            ExMessageBox.errCtl = _errCtl;
            Dlg_MessagBox msg = new Dlg_MessagBox("確認", "クリアします。" + "よろしいですか？", MessageBoxButtons.OkCancel, MessageBoxIcon.Question);
            msg.Closed -= ExMessageBox.dlg_ClosedClr;
            msg.Closed += ExMessageBox.dlg_ClosedClr;
            msg.Show();
        }

        public static void ResultShowClr(ExPage _page, Control _errCtl)
        {
            ExMessageBox.page = _page;
            ExMessageBox.errCtl = _errCtl;
            Dlg_MessagBox msg = new Dlg_MessagBox("確認", "クリアします。" + "よろしいですか？", MessageBoxButtons.OkCancel, MessageBoxIcon.Question);
            msg.Closed -= ExMessageBox.dlg_ClosedClr;
            msg.Closed += ExMessageBox.dlg_ClosedClr;
            msg.Show();
        }

        public static void ResultShowClr(ExChildWindow _win, Control _errCtl)
        {
            ExMessageBox.win = _win;
            ExMessageBox.errCtl = _errCtl;
            Dlg_MessagBox msg = new Dlg_MessagBox("確認", "クリアします。" + "よろしいですか？", MessageBoxButtons.OkCancel, MessageBoxIcon.Question);
            msg.Closed -= ExMessageBox.dlg_ClosedClr;
            msg.Closed += ExMessageBox.dlg_ClosedClr;
            msg.Show();
        }


        //public static void ResultShow(string strMsg, string strTitle)
        //{
        //    //if (ExMessageBox.utl != null || ExMessageBox.page != null)
        //    //{
        //    //    Dlg_MessagBox msg = new Dlg_MessagBox("確認", strMsg, MessageBoxButtons.OkCancel, MessageBoxIcon.Question);
        //    //    msg.Closed -= ExMessageBox.dlg_Closed;
        //    //    msg.Closed += ExMessageBox.dlg_Closed;
        //    //    msg.Show();
        //    //}

        //    //return MessageBox.Show(strMsg, strTitle, MessageBoxButton.OKCancel);
        //}

        private static void dlg_Closed(object sender, EventArgs e)
        {
            Dlg_MessagBox msg = (Dlg_MessagBox)sender;

            if (ExMessageBox.utl != null)
            {
                if (msg.Result == MessageBoxResult.OK)
                {
                    utl.ResultMessageBox(msg.Result, null);
                }
                else
                {
                    utl.ResultMessageBox(msg.Result, ExMessageBox.errCtl);
                }
            }
            else if (ExMessageBox.page != null)
            {
                if (msg.Result == MessageBoxResult.OK)
                {
                    page.ResultMessageBox(msg.Result, null);
                }
                else
                {
                    page.ResultMessageBox(msg.Result, ExMessageBox.errCtl);
                }
            }
            else if (ExMessageBox.win != null)
            {
                if (msg.Result == MessageBoxResult.OK)
                {
                    win.ResultMessageBox(msg.Result, null);
                }
                else
                {
                    win.ResultMessageBox(msg.Result, ExMessageBox.errCtl);
                }
            }

            ExMessageBox.utl = null;
            ExMessageBox.page = null;
            ExMessageBox.win = null;
        }

        private static void dlg_ClosedClr(object sender, EventArgs e)
        {
            Dlg_MessagBox msg = (Dlg_MessagBox)sender;
            msg.Closed -= ExMessageBox.dlg_ClosedClr;

            if (ExMessageBox.utl != null)
            {
                if (msg.Result == MessageBoxResult.OK)
                {
                    utl.ResultMessageBoxClr(msg.Result, null);
                }
                else
                {
                    utl.ResultMessageBoxClr(msg.Result, ExMessageBox.errCtl);
                }
            }
            else if (ExMessageBox.page != null)
            {
                if (msg.Result == MessageBoxResult.OK)
                {
                    page.ResultMessageBoxClr(msg.Result, null);
                }
                else
                {
                    page.ResultMessageBoxClr(msg.Result, ExMessageBox.errCtl);
                }
            }
            else if (ExMessageBox.win != null)
            {
                if (msg.Result == MessageBoxResult.OK)
                {
                    win.ResultMessageBoxClr(msg.Result, null);
                }
                else
                {
                    win.ResultMessageBoxClr(msg.Result, ExMessageBox.errCtl);
                }
            }

            ExMessageBox.utl = null;
            ExMessageBox.page = null;
            ExMessageBox.win = null;
        }

        #endregion

    }
}
