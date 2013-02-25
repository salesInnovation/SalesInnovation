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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SlvHanbaiClient.svcSysName;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Utility;

namespace SlvHanbaiClient.Class.Data
{
    public class MeiNameList
    {
        public static List<ListData> glstTaxChange = new List<ListData>();                  //  1:税転換ID
        public static List<ListData> glstBusinessDivison = new List<ListData>();            //  2:取引区分ID(売上)
        public static List<ListData> glstBreakdown = new List<ListData>();                  //  3:内訳ID
        public static List<ListData> glstDeliverDivision = new List<ListData>();            //  4:納品区分ID
        public static List<ListData> glstUnit = new List<ListData>();                       //  5:単位ID
        public static List<ListData> glstTaxDivision = new List<ListData>();                //  6:課税区分ID
        public static List<ListData> glstInventoryDivison = new List<ListData>();           //  7:在庫管理区分ID
        public static List<ListData> glstUnitPriceDivision = new List<ListData>();          //  8:単価区分ID
        public static List<ListData> glstDisplayDivision = new List<ListData>();            //  9:表示区分ID
        public static List<ListData> glstTitle = new List<ListData>();                      // 10:敬称ID
        public static List<ListData> glstFractionProc = new List<ListData>();               // 11:端数処理ID
        public static List<ListData> glstCollectCycle = new List<ListData>();               // 12:回収サイクルID
        public static List<ListData> glstClass = new List<ListData>();                      // 13:分類区分ID
        public static List<ListData> glstDividePermission = new List<ListData>();           // 14:分納許可ID
        public static List<ListData> glstInquiryDivision = new List<ListData>();            // 15:問い合わせ区分ID
        public static List<ListData> glstLevel = new List<ListData>();                      // 16:レベルID
        public static List<ListData> glstInquiryState = new List<ListData>();               // 17:問い合わせ状態ID
        public static List<ListData> glstApprovalState = new List<ListData>();              // 18:承認状態ID
        public static List<ListData> glstAccountKbn = new List<ListData>();                 // 19:預金種別
        public static List<ListData> glstOpenCloseState = new List<ListData>();             // 20:状態ID
        public static List<ListData> glstBusinessDivisonPu = new List<ListData>();          // 21:取引区分ID(仕入)
        public static List<ListData> glstSendkbn = new List<ListData>();                    // 22:発送区分ID
        public static List<ListData> glstTaxChangePu = new List<ListData>();                // 23:税転換ID(仕入)
        public static List<ListData> glstUnitPriceDivisionPu = new List<ListData>();        // 24:単価区分ID(仕入)
        public static List<ListData> glstInOutDeliveryKbn = new List<ListData>();           // 25:入出庫区分
        public static List<ListData> glstInOutDeliveryProcKbn = new List<ListData>();       // 26:入出庫処理区分
        public static List<ListData> glstInOutDeliveryToKbn = new List<ListData>();         // 27:入出庫先区分
        public static ObservableCollection<EntityName> objNameList = new ObservableCollection<EntityName>();　　       // 名称マスタ用リスト
        
        public enum geNameKbn
        {
            NONE = 0,
            TAX_CHANGE_ID = 1,              //  1:税転換ID
            BUSINESS_DIVISION_ID,           //  2:取引区分ID(売上)
            BREAKDOWN_ID,                   //  3:内訳ID
            DELIVER_DIVISION_ID,            //  4:納品区分ID
            UNIT_ID,				        //  5:単位ID
            TAX_DIVISION_ID,                //  6:課税区分ID
            INVENTORY_DIVISION_ID,          //  7:在庫管理区分ID
            UNIT_PRICE_DIVISION_ID,         //  8:単価区分ID
            DISPLAY_DIVISION_ID,            //  9:表示区分ID
            TITLE_ID,                       // 10:敬称ID
            FRACTION_PROC_ID,               // 11:端数処理ID
            COLLECT_CYCLE_ID,               // 12:回収サイクルID
            CLASS,                          // 13:分類区分ID
            DIVIDE_PERMISSION_ID,           // 14:分納許可ID
            INQUIRY_DIVISION_ID,            // 15:問い合わせ区分ID
            LEVEL_ID,                       // 16:レベルID
            INQUIRY_STATE_ID,               // 17:問い合わせ状態ID
            APPROVAL_STATE_ID,              // 18:承認状態ID
            ACCOUNT_KBN,                    // 19:預金種別
            OPEN_CLOSE_STATE_ID,            // 20:状態ID
            BUSINESS_DIVISION_PU_ID,        // 21:取引区分ID(仕入)
            SEND_KBN,                       // 22:発送区分
            TAX_CHANGE_PU_ID,               // 23:税転換ID(仕入)
            UNIT_PRICE_DIVISION_PU_ID,      // 24:単価区分ID(仕入)
            IN_OUT_DELIVERY_KBN,            // 25:入出庫区分
            IN_OUT_DELIVERY_PROC_KBN,       // 26:入出庫処理区分
            IN_OUT_DELIVERY_TO_KBN          // 27:入出庫先区分
        };
        public static geNameKbn gNameKbn;

        public static bool gIsPaymentCycle;

        public MeiNameList(object objList)
        {
            // 初期化
            Init();

            // 名称マスタリストから個別名称リストへセット
            SetIndividualList(objList);
        }

        // 初期化
        private void Init()
        {
            objNameList.Clear();
            glstTaxChange.Clear();
            glstBusinessDivison.Clear();
            glstBreakdown.Clear();
            glstDeliverDivision.Clear();
            glstTaxChange.Clear();
            glstUnit.Clear();
            glstTaxDivision.Clear();
            glstInventoryDivison.Clear();
            glstUnitPriceDivision.Clear();
            glstDisplayDivision.Clear();
            glstTitle.Clear();
            glstFractionProc.Clear();
            glstCollectCycle.Clear();
            glstClass.Clear();
            glstDividePermission.Clear();
            glstInquiryDivision.Clear();
            glstLevel.Clear();
            glstInquiryState.Clear();
            glstApprovalState.Clear();
            glstAccountKbn.Clear();
            glstOpenCloseState.Clear();
            glstBusinessDivisonPu.Clear();
            glstSendkbn.Clear();
            glstTaxChangePu.Clear();
            glstUnitPriceDivisionPu.Clear();
            glstInOutDeliveryKbn.Clear();
            glstInOutDeliveryProcKbn.Clear();
            glstInOutDeliveryToKbn.Clear();
        }

        // 名称マスタリストから個別名称リストへセット
        private void SetIndividualList(object objList)
        {
            objNameList = (ObservableCollection<EntityName>)objList;
            for (int i = 0; i <= objNameList.Count - 1; i++)
            {
                switch ((geNameKbn)objNameList[i].division_id)
                {
                    case geNameKbn.TAX_CHANGE_ID:               // 税転換ID
                        glstTaxChange.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.BUSINESS_DIVISION_ID:        // 取引区分ID
                        glstBusinessDivison.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.BREAKDOWN_ID:                // 内訳ID	
                        glstBreakdown.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.DELIVER_DIVISION_ID:         // 納品区分ID	
                        glstDeliverDivision.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.UNIT_ID:                     // 単位ID	
                        glstUnit.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.TAX_DIVISION_ID:             // 課税区分ID
                        glstTaxDivision.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.INVENTORY_DIVISION_ID:       // 在庫管理区分ID
                        glstInventoryDivison.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.UNIT_PRICE_DIVISION_ID:      // 単価区分ID
                        glstUnitPriceDivision.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.DISPLAY_DIVISION_ID:         // 表示区分ID
                        glstDisplayDivision.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.TITLE_ID:                    // 敬称ID
                        glstTitle.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.FRACTION_PROC_ID:            // 端数処理ID
                        glstFractionProc.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.COLLECT_CYCLE_ID:            // 回収サイクルID
                        glstCollectCycle.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.CLASS:                       // 分類区分ID
                        int _id = 0;
                        try
                        {
                            _id = ExCast.zCInt(ExMath.zCeiling(ExCast.zCDbl(ExCast.zCDbl(objNameList[i].id) / 3), 0));
                        }
                        catch
                        { 
                        }
                        glstClass.Add(new ListData(_id, objNameList[i].description));
                        break;
                    case geNameKbn.DIVIDE_PERMISSION_ID:        // 分納許可ID
                        glstDividePermission.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.INQUIRY_DIVISION_ID:         // 問い合わせ区分ID
                        glstInquiryDivision.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.LEVEL_ID:            // 問い合わせ緊急度ID
                        glstLevel.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.INQUIRY_STATE_ID:            // 問い合わせ状態ID
                        glstInquiryState.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.APPROVAL_STATE_ID:           // 承認状態ID
                        glstApprovalState.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.ACCOUNT_KBN:                 // 預金種別
                        glstAccountKbn.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.OPEN_CLOSE_STATE_ID:         // 状態ID
                        glstOpenCloseState.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.BUSINESS_DIVISION_PU_ID:     // 取引区分ID(仕入)
                        glstBusinessDivisonPu.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.SEND_KBN:                    // 発送区分
                        glstSendkbn.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.TAX_CHANGE_PU_ID:            // 税転換ID(仕入)
                        glstTaxChangePu.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.UNIT_PRICE_DIVISION_PU_ID:   // 単価区分ID(仕入)
                        glstUnitPriceDivisionPu.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.IN_OUT_DELIVERY_KBN:         // 入出庫区分
                        glstInOutDeliveryKbn.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.IN_OUT_DELIVERY_PROC_KBN:    // 入出庫処理区分
                        glstInOutDeliveryProcKbn.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    case geNameKbn.IN_OUT_DELIVERY_TO_KBN:      // 入出庫先区分
                        glstInOutDeliveryToKbn.Add(new ListData(objNameList[i].id, objNameList[i].description));
                        break;
                    default:
                        break;
                }
            }
        }

        // 名称マスタ名称取得
        public static string GetName(geNameKbn nameKbn, int id)
        {
            switch (nameKbn)
            {
                case geNameKbn.TAX_CHANGE_ID:               // 税転換ID
                    if (glstTaxChange.Count < id) return "";
                    return glstTaxChange[id - 1].DESCRIPTION;
                case geNameKbn.BUSINESS_DIVISION_ID:        // 取引区分ID
                    if (glstBusinessDivison.Count < id) return "";
                    return glstBusinessDivison[id - 1].DESCRIPTION;
                case geNameKbn.BREAKDOWN_ID:                // 内訳ID	
                    if (glstBreakdown.Count < id) return "";
                    return glstBreakdown[id - 1].DESCRIPTION;
                case geNameKbn.DELIVER_DIVISION_ID:         // 納品区分ID	
                    if (glstDeliverDivision.Count < id) return "";
                    return glstDeliverDivision[id - 1].DESCRIPTION;
                case geNameKbn.UNIT_ID:                     // 単位ID	
                    if (glstUnit.Count < id) return "";
                    return glstUnit[id - 1].DESCRIPTION;
                case geNameKbn.TAX_DIVISION_ID:             // 課税区分ID
                    if (glstTaxDivision.Count < id) return "";
                    return glstTaxDivision[id - 1].DESCRIPTION;
                case geNameKbn.INVENTORY_DIVISION_ID:       // 在庫管理区分ID
                    if (glstInventoryDivison.Count < id) return "";
                    return glstInventoryDivison[id - 1].DESCRIPTION;
                case geNameKbn.UNIT_PRICE_DIVISION_ID:       // 単価区分ID
                    if (glstUnitPriceDivision.Count < id) return "";
                    return glstUnitPriceDivision[id - 1].DESCRIPTION;
                case geNameKbn.DISPLAY_DIVISION_ID:         // 表示区分ID
                    if (glstDisplayDivision.Count - 1 < id) return "";
                    return glstDisplayDivision[id].DESCRIPTION;
                case geNameKbn.TITLE_ID:                    // 敬称ID	
                    if (glstTitle.Count < id) return "";
                    return glstTitle[id - 1].DESCRIPTION;
                case geNameKbn.FRACTION_PROC_ID:            // 端数処理ID	
                    if (glstFractionProc.Count < id) return "";
                    return glstFractionProc[id - 1].DESCRIPTION;
                case geNameKbn.COLLECT_CYCLE_ID:            // 回収サイクルID	
                    if (glstCollectCycle.Count < id) return "";
                    return glstCollectCycle[id - 1].DESCRIPTION;
                case geNameKbn.CLASS:                       // 分類区分ID	
                    if (glstClass.Count < id) return "";
                    return glstClass[id - 1].DESCRIPTION;
                case geNameKbn.DIVIDE_PERMISSION_ID:        // 分納許可ID
                    if (glstDividePermission.Count < id) return "";
                    return glstDividePermission[id - 1].DESCRIPTION;
                case geNameKbn.INQUIRY_DIVISION_ID:         // 問い合わせ区分ID
                    if (glstInquiryDivision.Count < id) return "";
                    return glstInquiryDivision[id - 1].DESCRIPTION;
                case geNameKbn.LEVEL_ID:            // 問い合わせ緊急度ID
                    if (glstLevel.Count < id) return "";
                    return glstLevel[id - 1].DESCRIPTION;
                case geNameKbn.INQUIRY_STATE_ID:            // 問い合わせ状態ID
                    if (glstInquiryState.Count < id) return "";
                    return glstInquiryState[id - 1].DESCRIPTION;
                case geNameKbn.APPROVAL_STATE_ID:           // 承認状態ID
                    if (glstApprovalState.Count < id) return "";
                    return glstApprovalState[id - 1].DESCRIPTION;
                case geNameKbn.ACCOUNT_KBN:                 // 預金種別
                    if (glstAccountKbn.Count < id) return "";
                    return glstAccountKbn[id - 1].DESCRIPTION;
                case geNameKbn.OPEN_CLOSE_STATE_ID:         // 状態ID
                    if (glstOpenCloseState.Count < id) return "";
                    return glstOpenCloseState[id - 1].DESCRIPTION;
                case geNameKbn.BUSINESS_DIVISION_PU_ID:     // 取引区分ID(仕入)
                    if (glstBusinessDivisonPu.Count < id) return "";
                    return glstBusinessDivisonPu[id - 1].DESCRIPTION;
                case geNameKbn.SEND_KBN:                    // 発送区分
                    if (glstSendkbn.Count < id) return "";
                    return glstSendkbn[id - 1].DESCRIPTION;
                case geNameKbn.TAX_CHANGE_PU_ID:            // 税転換ID(仕入)
                    if (glstTaxChangePu.Count < id) return "";
                    return glstTaxChangePu[id - 1].DESCRIPTION;
                case geNameKbn.UNIT_PRICE_DIVISION_PU_ID:   // 単価区分ID(仕入)
                    if (glstUnitPriceDivisionPu.Count < id) return "";
                    return glstUnitPriceDivisionPu[id - 1].DESCRIPTION;
                case geNameKbn.IN_OUT_DELIVERY_KBN:         // 入出庫区分
                    if (glstInOutDeliveryKbn.Count < id) return "";
                    return glstInOutDeliveryKbn[id - 1].DESCRIPTION;
                case geNameKbn.IN_OUT_DELIVERY_PROC_KBN:    // 入出庫処理区分
                    if (glstInOutDeliveryProcKbn.Count < id) return "";
                    return glstInOutDeliveryProcKbn[id - 1].DESCRIPTION;
                case geNameKbn.IN_OUT_DELIVERY_TO_KBN:      // 入出庫先区分
                    if (glstInOutDeliveryToKbn.Count < id) return "";
                    return glstInOutDeliveryToKbn[id - 1].DESCRIPTION;
                default:
                    return "";
            }
        }

        // 名称マスタID取得
        public static int GetID(geNameKbn nameKbn, string description)
        {
            switch (nameKbn)
            {
                case geNameKbn.TAX_CHANGE_ID:               // 税転換ID
                    for (int i = 0; i <= glstTaxChange.Count - 1; i++)
                    {
                        if (glstTaxChange[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.BUSINESS_DIVISION_ID:        // 取引区分ID
                    for (int i = 0; i <= glstBusinessDivison.Count - 1; i++)
                    {
                        if (glstBusinessDivison[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.BREAKDOWN_ID:                // 内訳ID	
                    for (int i = 0; i <= glstBreakdown.Count - 1; i++)
                    {
                        if (glstBreakdown[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.DELIVER_DIVISION_ID:         // 納品区分ID	
                    for (int i = 0; i <= glstDeliverDivision.Count - 1; i++)
                    {
                        if (glstDeliverDivision[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.UNIT_ID:                     // 単位ID	
                    for (int i = 0; i <= glstUnit.Count - 1; i++)
                    {
                        if (glstUnit[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.TAX_DIVISION_ID:             // 課税区分ID
                    for (int i = 0; i <= glstTaxDivision.Count - 1; i++)
                    {
                        if (glstTaxDivision[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.INVENTORY_DIVISION_ID:       // 在庫管理区分ID
                    for (int i = 0; i <= glstInventoryDivison.Count - 1; i++)
                    {
                        if (glstInventoryDivison[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.UNIT_PRICE_DIVISION_ID:      // 単価区分ID
                    for (int i = 0; i <= glstUnitPriceDivision.Count - 1; i++)
                    {
                        if (glstUnitPriceDivision[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.DISPLAY_DIVISION_ID:         // 表示区分ID
                    for (int i = 0; i <= glstDisplayDivision.Count - 1; i++)
                    {
                        if (glstDisplayDivision[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.TITLE_ID:                    // 敬称ID
                    for (int i = 0; i <= glstTitle.Count - 1; i++)
                    {
                        if (glstTitle[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.FRACTION_PROC_ID:            // 端数処理ID
                    for (int i = 0; i <= glstFractionProc.Count - 1; i++)
                    {
                        if (glstFractionProc[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.COLLECT_CYCLE_ID:            // 回収サイクルID
                    for (int i = 0; i <= glstCollectCycle.Count - 1; i++)
                    {
                        if (glstCollectCycle[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.CLASS:                       // 分類区分ID
                    for (int i = 0; i <= glstClass.Count - 1; i++)
                    {
                        if (glstClass[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.DIVIDE_PERMISSION_ID:        // 分納許可ID
                    for (int i = 0; i <= glstDividePermission.Count - 1; i++)
                    {
                        if (glstDividePermission[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.INQUIRY_DIVISION_ID:         // 問い合わせ区分ID
                    for (int i = 0; i <= glstInquiryDivision.Count - 1; i++)
                    {
                        if (glstInquiryDivision[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.LEVEL_ID:            // 問い合わせ緊急度ID
                    for (int i = 0; i <= glstLevel.Count - 1; i++)
                    {
                        if (glstLevel[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.INQUIRY_STATE_ID:            // 問い合わせ状態ID
                    for (int i = 0; i <= glstInquiryState.Count - 1; i++)
                    {
                        if (glstInquiryState[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.APPROVAL_STATE_ID:           // 承認状態ID
                    for (int i = 0; i <= glstApprovalState.Count - 1; i++)
                    {
                        if (glstApprovalState[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.ACCOUNT_KBN:                 // 預金種別
                    for (int i = 0; i <= glstAccountKbn.Count - 1; i++)
                    {
                        if (glstAccountKbn[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.OPEN_CLOSE_STATE_ID:         // 状態ID
                    for (int i = 0; i <= glstOpenCloseState.Count - 1; i++)
                    {
                        if (glstOpenCloseState[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.BUSINESS_DIVISION_PU_ID:     // 取引区分ID(仕入)
                    for (int i = 0; i <= glstBusinessDivisonPu.Count - 1; i++)
                    {
                        if (glstBusinessDivisonPu[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.SEND_KBN:                    // 発送区分
                    for (int i = 0; i <= glstSendkbn.Count - 1; i++)
                    {
                        if (glstSendkbn[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.TAX_CHANGE_PU_ID:            // 税転換ID(仕入)
                    for (int i = 0; i <= glstTaxChangePu.Count - 1; i++)
                    {
                        if (glstTaxChangePu[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.UNIT_PRICE_DIVISION_PU_ID:   // 単価区分ID(仕入)
                    for (int i = 0; i <= glstUnitPriceDivisionPu.Count - 1; i++)
                    {
                        if (glstUnitPriceDivisionPu[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.IN_OUT_DELIVERY_KBN:         // 入出庫区分
                    for (int i = 0; i <= glstInOutDeliveryKbn.Count - 1; i++)
                    {
                        if (glstInOutDeliveryKbn[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.IN_OUT_DELIVERY_PROC_KBN:    // 入出庫処理区分
                    for (int i = 0; i <= glstInOutDeliveryProcKbn.Count - 1; i++)
                    {
                        if (glstInOutDeliveryProcKbn[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                case geNameKbn.IN_OUT_DELIVERY_TO_KBN:      // 入出庫先区分
                    for (int i = 0; i <= glstInOutDeliveryToKbn.Count - 1; i++)
                    {
                        if (glstInOutDeliveryToKbn[i].DESCRIPTION == description) return i + 1;
                    }
                    return 0;
                default:
                    return 0;
            }
        }

        public static List<string> GetListMei(geNameKbn nameKbn)
        {
            List<string> lst = new List<string>();
            List<ListData> lstData = new List<ListData>();

            switch (nameKbn)
            {
                case geNameKbn.TAX_CHANGE_ID:               // 税転換ID
                    lstData = glstTaxChange;
                    break;
                case geNameKbn.BUSINESS_DIVISION_ID:        // 取引区分ID
                    lstData = glstBusinessDivison;
                    break;
                case geNameKbn.BREAKDOWN_ID:                // 内訳ID	
                    lstData = glstBreakdown;
                    break;
                case geNameKbn.DELIVER_DIVISION_ID:         // 納品区分ID	
                    lstData = glstDeliverDivision;
                    break;
                case geNameKbn.UNIT_ID:                     // 単位ID	
                    lstData = glstUnit;
                    break;
                case geNameKbn.TAX_DIVISION_ID:             // 課税区分ID
                    lstData = glstTaxDivision;
                    break;
                case geNameKbn.INVENTORY_DIVISION_ID:       // 在庫管理区分ID
                    lstData = glstInventoryDivison;
                    break;
                case geNameKbn.UNIT_PRICE_DIVISION_ID:      // 単価区分ID
                    lstData = glstUnitPriceDivision;
                    break;
                case geNameKbn.DISPLAY_DIVISION_ID:         // 表示区分ID
                    lstData = glstDisplayDivision;
                    break;
                case geNameKbn.TITLE_ID:                    // 敬称ID
                    lstData = glstTitle;
                    break;
                case geNameKbn.FRACTION_PROC_ID:            // 端数処理ID
                    lstData = glstFractionProc;
                    break;
                case geNameKbn.COLLECT_CYCLE_ID:            // 回収サイクルID
                    lstData = glstCollectCycle;
                    break;
                case geNameKbn.CLASS:                       // 分類区分ID
                    lstData = glstClass;
                    break;
                case geNameKbn.DIVIDE_PERMISSION_ID:        // 分納許可ID
                    lstData = glstDividePermission;
                    break;
                case geNameKbn.INQUIRY_DIVISION_ID:         // 問い合わせ区分ID
                    lstData = glstInquiryDivision;
                    break;
                case geNameKbn.LEVEL_ID:                    // 問い合わせ緊急度ID
                    lstData = glstLevel;
                    break;
                case geNameKbn.INQUIRY_STATE_ID:            // 問い合わせ状態ID
                    lstData = glstInquiryState;
                    break;
                case geNameKbn.APPROVAL_STATE_ID:           // 承認状態ID
                    lstData = glstApprovalState;
                    break;
                case geNameKbn.ACCOUNT_KBN:                 // 預金種別
                    lstData = glstAccountKbn;
                    break;
                case geNameKbn.OPEN_CLOSE_STATE_ID:         // 状態ID
                    lstData = glstOpenCloseState;
                    break;
                case geNameKbn.BUSINESS_DIVISION_PU_ID:     // 取引区分ID(仕入)
                    lstData = glstBusinessDivisonPu;
                    break;
                case geNameKbn.SEND_KBN:                    // 発送区分
                    lstData = glstSendkbn;
                    break;
                case geNameKbn.TAX_CHANGE_PU_ID:            // 税転換ID(仕入)
                    lstData = glstTaxChangePu;
                    break;
                case geNameKbn.UNIT_PRICE_DIVISION_PU_ID:   // 単価区分ID(仕入)
                    lstData = glstInOutDeliveryKbn;
                    break;
                case geNameKbn.IN_OUT_DELIVERY_KBN:         // 入出庫区分
                    lstData = glstUnitPriceDivisionPu;
                    break;
                case geNameKbn.IN_OUT_DELIVERY_PROC_KBN:    // 入出庫処理区分
                    lstData = glstInOutDeliveryProcKbn;
                    break;
                case geNameKbn.IN_OUT_DELIVERY_TO_KBN:      // 入出庫先区分
                    lstData = glstInOutDeliveryToKbn;
                    break;
            }

            for (int i = 0; i <= lstData.Count - 1; i++)
            {
                lst.Add(lstData[i].DESCRIPTION);
            }

            return lst;
        }

    }

    public class ListData
    {
        private int id;
        public int ID { set { this.id = value; } get { return this.id; } }
        private string description;
        public string DESCRIPTION { set { this.description = value; } get { return this.description; } }

        public ListData(int id, string description)
        {
            this.id = id;
            this.description = description;
        }
    }
}
