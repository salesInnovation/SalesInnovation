using System;
using System.Text;
using System.Net;
using System;
using System.IO;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

namespace SlvHanbaiClient.Class
{
    public class ExWebClient
    {

        #region Field

        private SaveFileDialog saveDialog;

        private OpenFileDialog fileDialog;
        public System.IO.Stream data = null;
        public string uploadFileName = "";

        private Dlg_Progress dlgWin = null;
        public Utl_FunctionKey utlParentFKey = null;

        public ExChildWindow win = null;

        public int upLoadFileKbn = 0;

        #endregion

        #region FileDownLoad

        public void FileDownLoad(DataReport.geReportKbn rptKbn, string pgId, string[] prm)
        {
            try
            {
                this.saveDialog = new SaveFileDialog();
                switch (rptKbn)
                { 
                    case DataReport.geReportKbn.Download:
                        this.saveDialog.DefaultExt = ".pdf";
                        this.saveDialog.Filter = "All Files|*.*|Pdf Files|*.pdf";
                        break;
                    case DataReport.geReportKbn.Csv:
                        this.saveDialog.DefaultExt = ".csv";
                        this.saveDialog.Filter = "All Files|*.*|Csv Files|*.csv";
                        break;
                    case DataReport.geReportKbn.None:
                        this.saveDialog.Filter = "All Files|*.*";
                        break;
                    default:
                        break;
                }
                this.saveDialog.FilterIndex = 2;
            }
            catch (Exception ex)
            {
                ExMessageBox.Show("ファイル保存処理(ダイアログ表示)で予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message);
                if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
                return;
            }

            bool? dialogResult = this.saveDialog.ShowDialog();
            if (dialogResult == true)
            {
                try
                {
                    string downLoadFileName = "";
                    string downLoadFilePath = "";

                    downLoadFileName = System.Windows.Browser.HttpUtility.UrlEncode(prm[0]);
                    downLoadFilePath = prm[1].Replace(prm[0], System.Windows.Browser.HttpUtility.UrlEncode(prm[0])).Replace(@"\", "@AAB@").Replace("/", "@AAD@"); ;
                    var requestUri = string.Format("{0}?rptKbn={1}&pgId={2}&random={3}&downLoadFileName={4}&downLoadFilePath={5}", Common.gstrReportDownloadUrl, ((int)rptKbn).ToString(), pgId, Common.gstrSessionString, downLoadFileName, downLoadFilePath);
                    var client = new WebClient();
                    client.OpenReadCompleted += OnFileDownloadCompleted;
                    client.OpenReadAsync(new Uri(requestUri), null);

                    switch (rptKbn)
                    {
                        case DataReport.geReportKbn.OutPut:
                        case DataReport.geReportKbn.Download:
                            Common.gstrProgressDialogTitle = "PDFファイルダウンロード中";
                            break;
                        case DataReport.geReportKbn.Csv:
                            Common.gstrProgressDialogTitle = "CSVファイルダウンロード中";
                            break;
                    }
                    if (dlgWin == null) { dlgWin = new Dlg_Progress(); }
                    dlgWin.Show();
                    
                }
                catch (Exception ex)
                {
                    ExMessageBox.Show("ファイルダウンロード処理で予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message);
                    if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
                    return;
                }
            }
            else
            {
                this.saveDialog = null;
                if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
                GC.Collect();
            }
        }

        private void OnFileDownloadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ExMessageBox.Show("ダウンロードファイル保存処理で通信エラーが発生しました。" + Environment.NewLine + e.Error);
                }
                else
                {
                    try
                    {
                        byte[] data = ReadBinaryData(e.Result);

                        string _msg = "";
                        try
                        {
                            ExUTF8Encoding _encode = new ExUTF8Encoding();
                            _msg = _encode.OnGetString(data, 0 , 200);
                            if (_msg.Length > 25)
                            {
                                if (_msg.IndexOf("error message start ==>") != -1)
                                {
                                    MessageBox.Show("ダウンロードファイル保存処理で予期せぬエラーが発生しました。" + Environment.NewLine + _msg.Replace("error message start ==>", ""));
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        using (System.IO.Stream fs = (System.IO.Stream)this.saveDialog.OpenFile())
                        {
                            fs.Write(data, 0, data.Length);
                            fs.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExMessageBox.Show("ダウンロードファイル保存処理で予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message);
                        return;
                    }
                }
            }
            finally
            {
                if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;

                this.saveDialog = null;
                if (dlgWin != null)
                {
                    dlgWin.Close();
                    dlgWin = null;
                }
                GC.Collect();
            }
        }

        // ストリームからデータを読み込み、バイト配列に格納
        private byte[] ReadBinaryData(System.IO.Stream st)
        {

            byte[] buf = new byte[32768]; // 一時バッファ

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {

                while (true)
                {
                    // ストリームから一時バッファに読み込む
                    int read = st.Read(buf, 0, buf.Length);

                    if (read > 0)
                    {
                        // 一時バッファの内容をメモリ・ストリームに書き込む
                        ms.Write(buf, 0, read);
                    }
                    else
                    {
                        break;
                    }
                }
                // メモリ・ストリームの内容をバイト配列に格納
                return ms.ToArray();
            }
        }

        #endregion

        #region FileDelete

        public void FileDelete(DataReport.geReportKbn rptKbn, string pgId, string _deleteFileName, string _deleteFilePath)
        {
            try
            {
                string deleteFileName = "";
                string deleteFilePath = "";

                deleteFileName = System.Windows.Browser.HttpUtility.UrlEncode(_deleteFileName);
                deleteFilePath = _deleteFilePath.Replace(_deleteFileName, System.Windows.Browser.HttpUtility.UrlEncode(_deleteFileName)).Replace(@"\", "@AAB@").Replace("/", "@AAD@"); ;

                var requestUri = string.Format("{0}?rptKbn={1}&pgId={2}&random={3}&deleteFileName={4}&deleteFilePath={5}", 
                                               Common.gstrReportDeleteUrl, 
                                               ((int)rptKbn).ToString(), pgId, 
                                               Common.gstrSessionString, 
                                               deleteFileName,
                                               deleteFilePath);
                var client = new WebClient();
                client.UploadStringCompleted += OnFileDeleteCompleted;
                client.UploadStringAsync(new Uri(requestUri), "delete");
            }
            catch (Exception ex)
            {
                //ExMessageBox.Show("ファイルダウンロード処理で予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message);
                return;
            }
        }

        private void OnFileDeleteCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ExMessageBox.Show("出力ファイル削除処理で通信エラーが発生しました。" + Environment.NewLine + e.Error);
                }
                else
                {
                    try
                    {
                        string _ret = e.Result;
                        //ExMessageBox.Show(_ret);
                    }
                    catch (Exception ex)
                    {
                        ExMessageBox.Show("出力ファイル削除処理で予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message);
                        return;
                    }
                }
            }
            finally
            {
            }
        }

        #endregion

        #region FileUpload

        public void FileUpLoadFileSet()
        {
            this.data = null;
            this.uploadFileName = "";

            try
            {
                this.fileDialog = new OpenFileDialog();
                //this.dialog.DefaultExt = ".pdf";
                this.fileDialog.Filter = "All Files|*.*";
                this.fileDialog.FilterIndex = 2;
            }
            catch (Exception ex)
            {
                ExMessageBox.Show("ファイル参照で予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message);
                return;
            }

            bool? dialogResult = this.fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                try
                {
                    if (this.fileDialog.File.Length > (1 * 1000 * 1000 * 10))
                    {
                        ExMessageBox.Show("10MB以上のファイルはアップロードできません。(ファイルサイズ：" + this.fileDialog.File.Length.ToString() + ")");
                        return;
                    }
                    this.data = this.fileDialog.File.OpenRead();
                    this.uploadFileName = this.fileDialog.File.Name;
                    FileUpLoad();
                }
                catch (Exception ex)
                {
                    ExMessageBox.Show("ファイルアップロード処理で予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message);
                    return;
                }
            }
        }

        public void FileUpLoad()
        {
            try
            {
                if (this.uploadFileName == "") return;
                if (this.data == null) return;
                if (this.win == null) return;

                var requestUri = string.Format("{0}?random={1}&upLoadFileName={2}&upLoadFileKbn={3}", Common.gstrFileUploadUrl, Common.gstrSessionString, this.uploadFileName, this.upLoadFileKbn);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
                webRequest.Method = "POST";
                webRequest.BeginGetRequestStream(new AsyncCallback(WriteToStreamCallback), webRequest);
            }
            catch (Exception ex)
            {
                this.win.OnFileUploadCompleted("");
                ExMessageBox.Show("ファイルアップロード処理で予期せぬエラーが発生しました。(FileUpLoad)" + Environment.NewLine + ex.Message);
            }
        }

        private void WriteToStreamCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest webRequest = null;
                Stream requestStream = null;

                webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                requestStream = webRequest.EndGetRequestStream(asynchronousResult);
                byte[] buffer = new Byte[4096];
                int bytesRead = 0;
                int tempTotal = 0;

                while ((bytesRead = this.data.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                    requestStream.Flush();
                }

                this.data.Close();
                this.data.Dispose();

                requestStream.Close();

                webRequest.BeginGetResponse(new AsyncCallback(ReadHttpResponseCallback), webRequest);
            }
            catch (Exception ex)
            {
                this.win.OnFileUploadCompleted("");
            }
        }

        private void ReadHttpResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());

                string responsestring = reader.ReadToEnd();
                reader.Close();

                if (responsestring.IndexOf("error message start ==>") != -1)
                {
                    this.win.OnFileUploadCompleted("");
                }
                else
                {
                    this.win.OnFileUploadCompleted(responsestring.Replace("success file upload ==>", ""));
                }
            }
            catch (Exception ex)
            {
                this.win.OnFileUploadCompleted("");
            }
        }

        #endregion

    }
}
