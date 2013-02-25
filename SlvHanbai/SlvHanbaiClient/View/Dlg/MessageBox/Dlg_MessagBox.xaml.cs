using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Utility;

namespace SlvHanbaiClient.View.Dlg.MessageBox
{
    public partial class Dlg_MessagBox : ExChildWindow
    {
        public delegate void MessageBoxClosedDelegate(MessageBoxResult result);
        public event MessageBoxClosedDelegate OnMessageBoxClosed;
        public MessageBoxResult Result { get; set; }

        public Dlg_MessagBox()
        {
            InitializeComponent();
            this.HasCloseButton = false;
            this.Closed += new EventHandler(MessageBoxChildWindow_Closed);
        }

        public Dlg_MessagBox(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            InitializeComponent();
            this.HasCloseButton = false;
            this.Closed += new EventHandler(MessageBoxChildWindow_Closed);

            this.Title = title;
            this.txtMsg.Text = message;
            if (this.txtMsg.ActualHeight <= 60)
            {
                this.Height = 150;
            }
            else
            {
                this.Height = 150 + (this.txtMsg.ActualHeight - 60);
            }
            if (this.Height > 650)
            {
                this.Height = 650;
            }
            DisplayButtons(buttons);
            DisplayIcon(icon);
        }

        private void DisplayButtons(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.Ok:
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Content = "Ok";
                    ExBackgroundWorker.DoWork_Focus(btnYes, 100);
                    break;

                case MessageBoxButtons.OkCancel:
                    btnCancel.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Content = "Ok";
                    ExBackgroundWorker.DoWork_FocusForLoad(btnCancel);
                    break;

                case MessageBoxButtons.YesNo:
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Visible;
                    btnYes.Visibility = Visibility.Visible;
                    ExBackgroundWorker.DoWork_Focus(btnNo, 100);
                    break;

                case MessageBoxButtons.YesNoCancel:
                    btnCancel.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    btnYes.Visibility = Visibility.Visible;
                    ExBackgroundWorker.DoWork_Focus(btnCancel, 100);
                    break;
            }
        }

        private void DisplayIcon(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    imgIcon.Source = new BitmapImage(new Uri("img/error.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.Information:
                    imgIcon.Source = new BitmapImage(new Uri("img/info.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.Question:
                    imgIcon.Source = new BitmapImage(new Uri("img/question.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.Warning:
                    imgIcon.Source = new BitmapImage(new Uri("img/warning.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.None:
                    imgIcon.Source = new BitmapImage(new Uri("img/info.png", UriKind.Relative));
                    break;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (btnYes.Content.ToString().ToLower().Equals("yes") == true)
            {
                //yes button
                this.Result = MessageBoxResult.Yes;
                this.DialogResult = true;
            }
            else
            {
                //ok button
                this.Result = MessageBoxResult.OK;
                this.DialogResult = false;
            }

            //this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.Cancel;
            this.DialogResult = false;
            //this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.No;
            this.DialogResult = false;
            //this.Close();
        }

        private void MessageBoxChildWindow_Closed(object sender, EventArgs e)
        {
            if (OnMessageBoxClosed != null)
                OnMessageBoxClosed(this.Result);
        }

        public void ShowDialog<TDialogViewModel>(IModalWindow view, TDialogViewModel viewModel, Action<TDialogViewModel> onDialogClose)
        {
            view.DataContext = viewModel;
            if (onDialogClose != null)
            {
                view.Closed += (sender, e) => onDialogClose(viewModel);
            }
            view.Show();
        }

        private void ExChildWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Result = MessageBoxResult.Cancel;
                this.DialogResult = false;
            }
        }

        private void txtMsg_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

    }

    public enum MessageBoxButtons
    {
        Ok, YesNo, YesNoCancel, OkCancel
    }

    public enum MessageBoxIcon
    {
        Question, Information, Error, None, Warning
    }
}

