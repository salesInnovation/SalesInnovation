#region using

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
using System.Windows.Navigation;
using System.Windows.Data;
using System.Windows.Browser;
using System.Collections.ObjectModel;  // ObservableCollectionを利用するために必要   
using System.ServiceModel.DomainServices.Client;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.View.UserControl.Custom;


#endregion

namespace SlvHanbaiClient.View.UserControl
{
    public partial class Utl_Install : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpOrder";
        private EntityOrderH objOrderH = new EntityOrderH();
        private ObservableCollection<EntityOrderD> objOrderListD;
        private Control activeControl;
        
        #endregion

        #region Constructor

        public Utl_Install()
        {
            InitializeComponent();
            Application.Current.InstallStateChanged += new EventHandler(App_InstallStateChanged);
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            if (Application.Current.InstallState == InstallState.Installed)
            {
                scrollViewer.Visibility = System.Windows.Visibility.Collapsed;
                txtDemo.Visibility = System.Windows.Visibility.Collapsed;
                tbChk.Visibility = System.Windows.Visibility.Collapsed;
                chkYes.Visibility = System.Windows.Visibility.Collapsed;

                tbMessage.Text = "SalesInnovation(販売管理アプリケーション)はインストール済です。";
                btnInstall.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                scrollViewer.Visibility = System.Windows.Visibility.Visible;
                txtDemo.Visibility = System.Windows.Visibility.Visible;
                tbChk.Visibility = System.Windows.Visibility.Visible;
                chkYes.Visibility = System.Windows.Visibility.Visible;
                SetText();

                tbMessage.Text = "SalesInnovation(販売管理アプリケーション)をインストールします。";
                btnInstall.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        private void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            if (chkYes.IsChecked == false)
            {
                ExMessageBox.Show("ソフトウェア使用許諾に同意頂けていない場合、デモアプリケーションのインストールを行う事はできません。");
                return;
            }

            Application.Current.Install();
        }

        #region Method

        private void App_InstallStateChanged(object sender, EventArgs e)
        {
            switch (Application.Current.InstallState)
            {
                case InstallState.Installed:
                    scrollViewer.Visibility = System.Windows.Visibility.Collapsed;
                    txtDemo.Visibility = System.Windows.Visibility.Collapsed;
                    tbChk.Visibility = System.Windows.Visibility.Collapsed;
                    chkYes.Visibility = System.Windows.Visibility.Collapsed;

                    //ExMessageBox.Show("SalesInnovation(販売管理アプリケーション)がインストールされました。");
                    tbMessage.Text = "SalesInnovation(販売管理アプリケーション)がインストールされました。";
                    tbMessage.Foreground = new SolidColorBrush(Colors.White);
                    btnInstall.Visibility = System.Windows.Visibility.Collapsed;
                    tbTitle.Visibility = System.Windows.Visibility.Collapsed;
                    //HtmlPage.Window.Eval("(window.open('','_top').opener=top).close();");
                    break;
                case InstallState.InstallFailed:
                    tbMessage.Text = "SalesInnovation(販売管理アプリケーション)のインストールに失敗しました。";
                    tbMessage.Foreground = new SolidColorBrush(Colors.White);
                    ExMessageBox.Show("SalesInnovation(販売管理アプリケーション)のインストールに失敗しました。");
                    break;
                case InstallState.Installing:
                case InstallState.NotInstalled:
                    break;
                default:
                    break;
            }
        }

        private void SetText()
        {
            string _text = "";

            _text += "【ご利用に際しての注意事項】" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += " SalesInnovationデモアプリケーションのIDおよびパスワードは全利用者共通のため、お客様が登録された情報は、第三者によって閲覧されることになります。" + Environment.NewLine;
            _text += " また、本アプリケーションの運営上、お客様が登録されたデータの初期化・削除を行うことがあります。" + Environment.NewLine;
            _text += " そのため、漏洩・破損・消滅した場合に支障をきたすおそれのある情報の開示はお控え下さい。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "";
            _text += "（使用目的）" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "お客様は、SalesInnovationの導入を検討する目的においてのみ、本利用規約に記載の条件の下、無償で本アプリケーションにおいてSalesInnovationを試用することができます。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "お客様は、本アプリケーション上に登録されたデータの試用・編集、新規データの作成・登録等を行うことができるものとします。" + Environment.NewLine;
            _text += " なお、お客様が本アプリケーション上に登録されたデータにつきましては、SalesInnovation運営者の判断により削除することがあるものとします。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "（評価アプリケーション用ＩＤおよびパスワード）" + Environment.NewLine;
            _text += " " + Environment.NewLine;
            _text += "お客様は、本アプリケーションにおいて、SalesInnovationを使用するためにSalesInnovation運営者が発行する全利用者共通のＩＤおよびパスワード（以下、「評価アプリケーション用ＩＤ等」という。）を、前項の目的の範囲内でSalesInnovationを稼動させるためにのみ試用することができます。よって、自己の業務使用目的等、前項の目的以外の使用方法には一切供してはならないものとします。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "（禁止事項）" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "お客様は、今回許諾された権利について、譲渡、貸与、転売、担保設定等は一切行なうことはできません。なお、今回許諾される権利は評価対象商品の試用権のみであり、この利用規約に記載のない事項についてはすべてSalesInnovation運営者に留保されることをここに確認するものとします。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "お客様は、本件ソフトウェアを第三者に配布、レンタル、リース、貸与及び譲渡することはできません。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "お客様は、本件ソフトウェアに含まれるプログラムに対して、修正を加えること、翻訳、翻案を行うこと、及び逆コンパイル、逆アセンブル等のリバースエンジニアリングを行うことはできません。 " + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "（著作権の帰属）" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "本件ソフトウェア(SalesInnovation)の著作権は、全てSalesInnovation運営者に帰属します。 。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "（保証の制限）" + Environment.NewLine;
            _text += " " + Environment.NewLine;
            _text += "お客様は、漏洩・破損・消滅した場合に支障をきたすおそれのある情報は、本アプリケーション上には登録しないものとします。お客様が本アプリケーションの利用の際に登録したデータの全部および一部が漏洩・破損した場合でも、SalesInnovation運営者はそれらの保証は行わないものとします。また、データが破損したことにより、お客様および第三者がいかなる損害を被った場合でも、SalesInnovation運営者は一切の損害賠償責任を負いません。" + Environment.NewLine;
            _text += " " + Environment.NewLine;
            _text += "本アプリケーションにアクセスした場合においてお客様および第三者が、ウィルス感染、ハッキング等の被害を被った場合でも、SalesInnovation運営者は一切の損害賠償責任を負いません。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "SalesInnovation運営者は、お客様、その他の第三者が本件ソフトウェアに関連して直接間接に蒙ったいかなる損害に対しても、賠償等の一切の責任を負わず、かつ、お客様はこれに対してSalesInnovation運営者を免責するものとします。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "SalesInnovation運営者は、はお客様に対し、本件ソフトウェアの動作保証、使用目的への適合性の保証、商業性の保証、使用結果についての的確性や信頼性の保証、第三者の権利侵害及び瑕疵担保義務も含め、いかなる責任も一切負いません。SIIがこれらの可能性について事前に知らされていた場合も同様です。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "SalesInnovation運営者は、本アプリケーションにおけるSalesInnovationに含まれた機能がお客様の要求を満足させるものであること、正常に作動すること、瑕疵（いわゆるバグ、構造上の問題等を含む）が存在していないことあるいは存在していた場合に、これが修正されること、のいずれも保証いたしません。また、本アプリケーションにおけるSalesInnovationの機能と製品版に搭載される機能とが一致することを保証するものではありません。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "（サービスの停止、中止）" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "以下の各号のいずれかに該当する場合は、本アプリケーションにおけるサービスの停止および緊急停止することがあるものとします。なお、SalesInnovation運営者はお客様および第三者からの緊急停止要請に関しては原則としてこれを受け付けないものとします。また、SalesInnovation運営者は、本アプリケーションにおけるサービスを停止すること、ならびに停止できなかったことによって、お客様および第三者が損害を被った場合でも一切の賠償責任を負いません。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "サービスを提供するために必要なサービスシステムのメンテナンス、電気通信設備の保守上または工事上やむを得ないとき、またはこれらにやむを得ない障害が発生したとき" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "サービスシステムに著しい不可や障害が与えられることによって正常なサービスを提供することが困難であると判断したとき" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "サービスを提供することにより、お客様あるいは第三者が著しい損害を受ける可能性を認知したとき" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "電気通信事業者または国外の電気通信事業体が電気通信サービスの提供を中止および停止することにより、サービスの提供を行なうことが困難になったとき" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "天災地変､戦争､内乱､法令の制定改廃その他の非常事態が発生した場合､またはそのおそれがあるとき" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "その他、SalesInnovation運営者がサービスの提供を停止、緊急停止する必要があると判断したとき" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "SalesInnovation運営者はお客様の認識如何に関わらず、自己の都合により本アプリケーションにおけるサービス内容等を変更および一部廃止することができるものとします。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "SalesInnovation運営者は自己の都合により、本アプリケーションにおけるサービスの提供の一部または全部を廃止することができるものとします。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "（情報の監視・削除）" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "SalesInnovation運営者は本アプリケーションの安全な運営のため、お客様の登録されたデータを監視することができます。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "SalesInnovation運営者は本アプリケーションの運営上、お客様の登録されたデータを定期的に初期化し、削除することができます。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "前項に関わらず、SalesInnovation運営者は、SalesInnovation運営者が当該情報を削除する必要があると判断した場合､お客様への事前の通知なしにお客様がSalesInnovation運営者のサーバー上に保存するデータの一部または全部を削除することができます｡" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "SalesInnovation運営者は､データの削除を行なったことおよび、行なわなかったことによりお客様が損害を被った場合について､その損害については一切責任を負いません｡" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "（使用の中止）" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "SalesInnovation運営者が適切な使用でないと判断した場合、お客様はただちに本アプリケーションにおけるSalesInnovationの使用を取りやめなければならないものとします。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "（責任の制限）" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "本アプリケーションにおける評価対象商品の試用に関するすべての責任および危険は、お客様が負担するものとし、SalesInnovation運営者は一切その責任を負わないものとします。" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "" + Environment.NewLine;
            _text += "以　上" + Environment.NewLine;

            txtDemo.Text = _text;
        }

        #endregion



    }

}
