#region using

using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class.Entity;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.View.UserControl.Master;

#endregion

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_InpMaster : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_InpMaster";
        public EntityOrderH objOrderH;
        public ObservableCollection<EntityOrderD> objOrderListD;
        public ObservableCollection<EntityDataFormOrderD> objDataFormOrderD = new ObservableCollection<EntityDataFormOrderD>();
        public object objBeforeOrderListD;
        public bool IsSearchFlg = false;        // マスタ参照一覧からの起動

        #endregion

        #region Constructor

        public Dlg_InpMaster()
        {
            InitializeComponent();
            //this.SetWindowsResource();
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Common.gWinGroupType != Common.geWinGroupType.InpMaster && Common.gWinGroupType != Common.geWinGroupType.InpMasterDetail)
            {
                this.DialogResult = false;
                //this.Close();
            }

            this.GridMaster.Children.Clear();

            // 証跡保存
            DataPgEvidence.SaveLoadOrUnLoadEvidence(Common.gWinGroupType, Common.gWinMsterType, DataPgEvidence.geOperationType.Start);

            ExUserControl utl = null;
            switch (Common.gWinMsterType)
            {
                case Common.geWinMsterType.Company:
                    utl = new Utl_MstCompany();
                    this.GridMaster.Children.Add(utl);
                    this.Width = utl.Width + 20;
                    break;
                case Common.geWinMsterType.CompanyGroup:
                    utl = new Utl_MstCompanyGroup();
                    this.GridMaster.Children.Add(utl);
                    this.Width = utl.Width + 20;
                    break;
                case Common.geWinMsterType.User:
                    utl = new Utl_MstUser();
                    this.GridMaster.Children.Add(utl);
                    break;
                case Common.geWinMsterType.Person:
                    utl = new Utl_MstPerson();
                    this.GridMaster.Children.Add(utl);
                    break;
                case Common.geWinMsterType.Customer:
                    utl = new Utl_MstCustomer();
                    this.GridMaster.Children.Add(utl);
                    this.Width = utl.Width + 20;
                    break;
                case Common.geWinMsterType.Commodity:
                    utl = new Utl_MstCommodity();
                    this.GridMaster.Children.Add(utl);
                    this.Width = utl.Width + 10;
                    break;
                case Common.geWinMsterType.Condition:
                    utl = new Utl_MstCondition();
                    this.GridMaster.Children.Add(utl);
                    this.Width = utl.Width + 10;
                    break;
                case Common.geWinMsterType.Class:
                    utl = new Utl_MstClass();
                    this.GridMaster.Children.Add(utl);
                    this.Width = utl.Width + 10;
                    break;
                case Common.geWinMsterType.Supplier:
                    utl = new Utl_MstSupplier();
                    this.GridMaster.Children.Add(utl);
                    this.Width = utl.Width + 20;
                    break;
                case Common.geWinMsterType.Authority:
                    utl = new Utl_MstAuthority();
                    this.GridMaster.Children.Add(utl);
                    this.Width = utl.Width + 20;
                    break;
                case Common.geWinMsterType.Purchase:
                    utl = new Utl_MstPurchase();
                    this.GridMaster.Children.Add(utl);
                    this.Width = utl.Width + 20;
                    break;
                default:
                    break;
            }

            if (utl != null) this.Width = utl.Width + 20;

            this.listDisplayTabIndex = ExVisualTreeHelper.GetDisplayTabIndex(this.GridMaster); // Tab Index 保持

        }

        private void ExChildWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            // 証跡保存
            DataPgEvidence.SaveLoadOrUnLoadEvidence(Common.gWinGroupType, Common.gWinMsterType, DataPgEvidence.geOperationType.End);
        }

        private void ExChildWindow_KeyUp(object sender, KeyEventArgs e)
        {
        }

        #endregion

    }



}

