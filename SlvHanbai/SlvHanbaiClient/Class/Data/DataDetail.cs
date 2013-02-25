using System;
using System.Net;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;  // ObservableCollectionを利用するために必要   
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.svcSales;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.svcEstimate;
using SlvHanbaiClient.svcPurchaseOrder;
using SlvHanbaiClient.svcPurchase;
using SlvHanbaiClient.svcInOutDelivery;

namespace SlvHanbaiClient.Class.Data
{
    public class DataDetail
    {

        public static bool IsCalcPrice = true;

        public enum eKbn { Sales = 0, Purchase, InOutDelivery }

        #region SetCommodityToDetail : 商品から明細へ

        // For Sales
        public static void SetCommodityToDetail(int i, EntitySalesH entityH, ObservableCollection<EntitySalesD> entityD, svcMstData.EntityMstData mst)
        {
            if (i == -1) return;

            svcMstData.EntityMstData _mst = mst;

            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;

            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _SetCommodityToDetail(i, ref entityBaseH, ref entityBaseD, _mst, eKbn.Sales);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetailNumber(i, entityH, entityD);
            CalcDetail(i, entityH, entityD);
        }

        // For Order
        public static void SetCommodityToDetail(int i, EntityOrderH entityH, ObservableCollection<EntityOrderD> entityD, svcMstData.EntityMstData mst)
        {
            if (i == -1) return;

            svcMstData.EntityMstData _mst = mst;

            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;

            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _SetCommodityToDetail(i, ref entityBaseH, ref entityBaseD, _mst, eKbn.Sales);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetailNumber(i, entityH, entityD);
            CalcDetail(i, entityH, entityD);
        }

        // For Estimate
        public static void SetCommodityToDetail(int i, EntityEstimateH entityH, ObservableCollection<EntityEstimateD> entityD, svcMstData.EntityMstData mst)
        {
            if (i == -1) return;

            svcMstData.EntityMstData _mst = mst;

            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;

            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _SetCommodityToDetail(i, ref entityBaseH, ref entityBaseD, _mst, eKbn.Sales);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetailNumber(i, entityH, entityD);
            CalcDetail(i, entityH, entityD);
        }

        // For Purchase Order
        public static void SetCommodityToDetail(int i, EntityPurchaseOrderH entityH, ObservableCollection<EntityPurchaseOrderD> entityD, svcMstData.EntityMstData mst)
        {
            if (i == -1) return;

            svcMstData.EntityMstData _mst = mst;

            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;

            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _SetCommodityToDetail(i, ref entityBaseH, ref entityBaseD, _mst, eKbn.Purchase);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetailNumber(i, entityH, entityD);
            CalcDetail(i, entityH, entityD);
        }

        // For Purchase
        public static void SetCommodityToDetail(int i, EntityPurchaseH entityH, ObservableCollection<EntityPurchaseD> entityD, svcMstData.EntityMstData mst)
        {
            if (i == -1) return;

            svcMstData.EntityMstData _mst = mst;

            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;

            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _SetCommodityToDetail(i, ref entityBaseH, ref entityBaseD, _mst, eKbn.Purchase);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetailNumber(i, entityH, entityD);
            CalcDetail(i, entityH, entityD);
        }

        // For InOutDelivery
        public static void SetCommodityToDetail(int i, EntityInOutDeliveryH entityH, ObservableCollection<EntityInOutDeliveryD> entityD, svcMstData.EntityMstData mst)
        {
            if (i == -1) return;

            svcMstData.EntityMstData _mst = mst;

            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;

            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _SetCommodityToDetail(i, ref entityBaseH, ref entityBaseD, _mst, eKbn.InOutDelivery);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetailNumber(i, entityH, entityD);
            CalcDetail(i, entityH, entityD);
        }

        private static void _SetCommodityToDetail(int i, ref EntityBaseH entityBaseH, ref ObservableCollection<EntityBaseD> entityBaseD, svcMstData.EntityMstData _mst, eKbn kbn)
        {
            // 初期化
            entityBaseD[i].commodity_name = "";       // 商品名
            entityBaseD[i].unit_id = 0;               // 単位ID
            entityBaseD[i].enter_number = 0;          // 入数
            entityBaseD[i].case_number = 1;           // ケース数
            entityBaseD[i].number = 0;                // 数量
            entityBaseD[i].tax_division_id = 0;       // 税区分
            entityBaseD[i].tax_division_nm = "";      // 税区分名
            entityBaseD[i].tax_percent = 0;           // 税率
            entityBaseD[i].unit_price = 0;            // 売上単価
            entityBaseD[i].sales_cost = 0;            // 売上原価
            entityBaseD[i].price = 0;                 // 金額
            entityBaseD[i].tax = 0;                   // 消費税
            entityBaseD[i].no_tax_price = 0;          // 税抜金額
            entityBaseD[i].profits = 0;               // 粗利
            entityBaseD[i].profits_percent = 0;       // 粗利率
            entityBaseD[i].inventory_number = 0;      // 現在庫

            if (_mst == null)
            {
                return;
            }
            else
            {
                if (_mst.id == "")
                {
                    SetInitCombo(ref _mst);
                    entityBaseD[i].case_number = 0;        // ケース数
                }
            }

            entityBaseD[i].commodity_name = _mst.name;                        // 商品名
            entityBaseD[i].enter_number = ExCast.zCInt(_mst.attribute2);      // 入数

            // 数量(単価)小数桁
            entityBaseD[i].number_decimal_digit = ExCast.zCInt(_mst.attribute3);
            entityBaseD[i].unit_decimal_digit = ExCast.zCInt(_mst.attribute4);

            // 単位ID
            entityBaseD[i].unit_id = ExCast.zCInt(_mst.attribute1);
            entityBaseD[i].unit_nm = MeiNameList.GetName(MeiNameList.geNameKbn.UNIT_ID, entityBaseD[i].unit_id);

            // 税区分
            entityBaseD[i].tax_division_id = ExCast.zCInt(_mst.attribute5);
            entityBaseD[i].tax_division_nm = MeiNameList.GetName(MeiNameList.geNameKbn.TAX_DIVISION_ID, entityBaseD[i].tax_division_id);

            // 税区分が課税で内訳が消費税の場合
            if (entityBaseD[i].tax_division_id == 1 && entityBaseD[i].breakdown_id == 5)
            {
                entityBaseD[i].tax_division_nm = "非課税";
                entityBaseD[i].tax_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.TAX_DIVISION_ID, ExCast.zCStr(entityBaseD[i].tax_division_nm));
            }

            entityBaseD[i].tax_percent = 5;

            int _credit_rate = entityBaseH.credit_rate;
            if (_credit_rate == 0) _credit_rate = 100;

            // 上代税込
            entityBaseD[i].retail_price_before_tax = ExMath.zFloor(ExCast.zCDbl(_mst.attribute10) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
            // 売上単価税込
            entityBaseD[i].sales_unit_price_before_tax = ExMath.zFloor(ExCast.zCDbl(_mst.attribute12) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
            // 売上原価税込
            entityBaseD[i].sales_cost_price_before_tax = ExMath.zFloor(ExCast.zCDbl(_mst.attribute14) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
            // 仕入単価税込
            entityBaseD[i].purchase_unit_price_before_tax = ExMath.zFloor(ExCast.zCDbl(_mst.attribute16) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
            // 上代税抜
            entityBaseD[i].retail_price_skip_tax = ExMath.zFloor(ExCast.zCDbl(_mst.attribute9) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
            // 売上単価税抜
            entityBaseD[i].sales_unit_price_skip_tax = ExMath.zFloor(ExCast.zCDbl(_mst.attribute11) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
            // 売上原価税抜
            entityBaseD[i].sales_cost_price_skip_tax = ExMath.zFloor(ExCast.zCDbl(_mst.attribute13) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
            // 仕入単価税抜
            entityBaseD[i].purchase_unit_price_skip_tax = ExMath.zFloor(ExCast.zCDbl(_mst.attribute15) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);

            // 在庫
            entityBaseD[i].inventory_management_division_id = ExCast.zCInt(_mst.attribute6);
            entityBaseD[i].inventory_number = ExCast.zCDbl(_mst.attribute8);

            // 単価設定
            // 税転換が内税で明細課税有りの場合
            if ((entityBaseH.tax_change_id == 4 || entityBaseH.tax_change_id == 5 || entityBaseH.tax_change_id == 6) && entityBaseD[i].tax_division_id == 1)
            {
                switch (entityBaseH.unit_kind_id)
                {
                    case 1:     // 上代税込
                        entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].retail_price_before_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                        break;
                    case 2:     // 売上単価税込
                        switch (kbn)
                        {
                            case eKbn.Purchase:
                                entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].purchase_unit_price_before_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                                break;
                            default:
                                entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].sales_unit_price_before_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                                break;
                        }
                        break;
                    case 3:     // 売上原価税込
                        entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].sales_cost_price_before_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                        break;
                    default:
                        switch (kbn)
                        {
                            case eKbn.Purchase:
                                entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].purchase_unit_price_before_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                                break;
                            default:
                                entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].sales_unit_price_before_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                                break;
                        }
                        break;
                }
                // 売上原価税込
                entityBaseD[i].sales_cost = entityBaseD[i].sales_cost_price_before_tax;
            }
            else
            {
                switch (entityBaseH.unit_kind_id)
                {
                    case 1:     // 上代税抜
                        entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].retail_price_skip_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                        break;
                    case 2:     // 売上単価税抜
                        switch (kbn)
                        {
                            case eKbn.Purchase:
                                entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].purchase_unit_price_skip_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                                break;
                            default:
                                entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].sales_unit_price_skip_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                                break;
                        }
                        break;
                    case 3:     // 売上原価税抜
                        entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].sales_cost_price_skip_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                        break;
                    default:
                        switch (kbn)
                        {
                            case eKbn.Purchase:
                                entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].purchase_unit_price_skip_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                                break;
                            default:
                                entityBaseD[i].unit_price = ExMath.zFloor(ExCast.zCDbl(entityBaseD[i].sales_unit_price_skip_tax) * _credit_rate / 100, entityBaseD[i].unit_decimal_digit);
                                break;
                        }
                        break;
                }
                // 売上原価税抜
                entityBaseD[i].sales_cost = entityBaseD[i].sales_cost_price_skip_tax;
            }

            // 返品時
            if (entityBaseD[i].breakdown_id == 6)
            {
                CalRedcDetail(entityBaseD[i]);
            }
        }

        #endregion

        #region CalcDetailNumber : 明細数量計算

        private static void _CalcDetailNumber(int i, ref ObservableCollection<EntityBaseD> entityBaseD)
        {
            // 数量
            if (entityBaseD[i].enter_number != 0 && entityBaseD[i].case_number != 0)
            {
                entityBaseD[i].number = entityBaseD[i].enter_number * entityBaseD[i].case_number;
            }
            else if (entityBaseD[i].enter_number == 0 && entityBaseD[i].case_number != 0)
            {
                entityBaseD[i].number = entityBaseD[i].case_number;
            }
            else if (entityBaseD[i].case_number == 0 && entityBaseD[i].enter_number != 0)
            {
                entityBaseD[i].number = entityBaseD[i].enter_number;
            }

            // 受注残
            entityBaseD[i].order_stay_number = entityBaseD[i].order_number - entityBaseD[i].number;
        }

        // For Sales
        public static void CalcDetailNumber(int i, EntitySalesH entityH, ObservableCollection<EntitySalesD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetailNumber(i, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetail(i, entityH, entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Order
        public static void CalcDetailNumber(int i, EntityOrderH entityH, ObservableCollection<EntityOrderD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetailNumber(i, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetail(i, entityH, entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Estimate
        public static void CalcDetailNumber(int i, EntityEstimateH entityH, ObservableCollection<EntityEstimateD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetailNumber(i, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetail(i, entityH, entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Purchase Order
        public static void CalcDetailNumber(int i, EntityPurchaseOrderH entityH, ObservableCollection<EntityPurchaseOrderD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetailNumber(i, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetail(i, entityH, entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Purchase
        public static void CalcDetailNumber(int i, EntityPurchaseH entityH, ObservableCollection<EntityPurchaseD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetailNumber(i, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetail(i, entityH, entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For InOutDelivery
        public static void CalcDetailNumber(int i, EntityInOutDeliveryH entityH, ObservableCollection<EntityInOutDeliveryD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetailNumber(i, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細計算
            CalcDetail(i, entityH, entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        #endregion

        #region CalcDetail : 明細計算

        private static void _CalcDetail(int i, ref EntityBaseH entityBaseH, ref ObservableCollection<EntityBaseD> entityBaseD)
        {
            if (DataDetail.IsCalcPrice == true)
            {
                // 金額
                entityBaseD[i].price = ExMath.zCalcPrice((entityBaseD[i].number * entityBaseD[i].unit_price), entityBaseH.price_fraction_proc_id, 0);
            }

            // 消費税計算
            if (entityBaseD[i].tax_division_id == 1)
            {
                // 課税
                switch (entityBaseH.tax_change_id)
                {
                    // 外税/明細単位, 内税/明細単位
                    case 2:
                    case 5:
                        entityBaseD[i].tax = ExMath.zCalcTax(entityBaseD[i].price, entityBaseH.tax_change_id, 0, entityBaseH.tax_fraction_proc_id);
                        break;
                    default:
                        entityBaseD[i].tax = 0;
                        break;
                }
            }
            else
            {
                // 非課税
                entityBaseD[i].tax = 0;
            }

            // 税抜金額
            if (entityBaseH.tax_change_id == 5)
            {
                // 内税/明細単位
                entityBaseD[i].no_tax_price = entityBaseD[i].price - entityBaseD[i].tax;
            }
            else
            {
                // 内税/明細単位以外
                entityBaseD[i].no_tax_price = entityBaseD[i].price;
            }

            // 粗利
            double _profits = entityBaseD[i].no_tax_price - (entityBaseD[i].number * entityBaseD[i].sales_cost);
            entityBaseD[i].profits = ExMath.zCalcPrice(_profits, entityBaseH.price_fraction_proc_id, 0);

            // 粗利率
            entityBaseD[i].profits_percent = ExMath.zFloor((entityBaseD[i].profits / entityBaseD[i].no_tax_price) * 100, 0);

            IsCalcPrice = true;
        }

        // For Sales
        public static void CalcDetail(int i, EntitySalesH entityH, ObservableCollection<EntitySalesD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetail(i, ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Order
        public static void CalcDetail(int i, EntityOrderH entityH, ObservableCollection<EntityOrderD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetail(i, ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Estimate
        public static void CalcDetail(int i, EntityEstimateH entityH, ObservableCollection<EntityEstimateD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetail(i, ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Purchase Order
        public static void CalcDetail(int i, EntityPurchaseOrderH entityH, ObservableCollection<EntityPurchaseOrderD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetail(i, ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Purchase
        public static void CalcDetail(int i, EntityPurchaseH entityH, ObservableCollection<EntityPurchaseD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetail(i, ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For InOutDelivery
        public static void CalcDetail(int i, EntityInOutDeliveryH entityH, ObservableCollection<EntityInOutDeliveryD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcDetail(i, ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        #endregion

        #region CalcSumDetail : 明細合計計算

        private static void _CalcSumDetail(ref EntityBaseH entityBaseH, ref ObservableCollection<EntityBaseD> entityBaseD)
        { 
            double price = 0;

            // 初期化
            entityBaseH.sum_enter_number = 0;
            entityBaseH.sum_case_number = 0;
            entityBaseH.sum_number = 0;
            entityBaseH.sum_unit_price = 0;
            entityBaseH.sum_sales_cost = 0;
            entityBaseH.sum_price = 0;
            entityBaseH.sum_tax = 0;
            entityBaseH.sum_no_tax_price = 0;
            entityBaseH.sum_profits = 0;
            entityBaseH.profits_percent = 0;

            for (int i = 0; i <= entityBaseD.Count - 1; i++)
            {
                // 納品区分が取消以外
                if (entityBaseD[i].deliver_division_id != 4)
                {
                    entityBaseH.sum_enter_number += entityBaseD[i].enter_number;
                    entityBaseH.sum_case_number += entityBaseD[i].case_number;
                    entityBaseH.sum_number += entityBaseD[i].number;
                    entityBaseH.sum_unit_price += entityBaseD[i].unit_price;
                    entityBaseH.sum_sales_cost += entityBaseD[i].sales_cost;
                    entityBaseH.sum_price += entityBaseD[i].price;
                    entityBaseH.sum_tax += entityBaseD[i].tax;
                    entityBaseH.sum_no_tax_price += entityBaseD[i].no_tax_price;
                    entityBaseH.sum_profits += entityBaseD[i].profits;

                    // 課税区分(外税/伝票単位, 内税/伝票単位)
                    if (entityBaseH.tax_change_id == 1 || entityBaseH.tax_change_id == 4)
                    {
                        // 課税対象
                        if (entityBaseD[i].tax_division_id == 1)
                        {
                            price += entityBaseD[i].price;
                        }
                    }
                }
            }

            // 課税区分(外税/伝票単位, 内税/伝票単位)
            if (entityBaseH.tax_change_id == 1 || entityBaseH.tax_change_id == 4)
            {
                entityBaseH.sum_tax = ExMath.zCalcTax(price, entityBaseH.tax_change_id, 0, entityBaseH.tax_fraction_proc_id);

                if (entityBaseH.tax_change_id == 1)
                {
                    // 外税
                    entityBaseH.sum_no_tax_price = entityBaseH.sum_price;
                }
                else
                {
                    // 内税
                    entityBaseH.sum_no_tax_price = entityBaseH.sum_price - entityBaseH.sum_tax;
                }
            }

            if (entityBaseH.sum_no_tax_price != 0 && entityBaseH.sum_profits != 0)
            {
                entityBaseH.profits_percent = ExMath.zFloor((entityBaseH.sum_profits / entityBaseH.sum_no_tax_price) * 100, 0);
            }
        }

        // For Sales
        public static void CalcSumDetail(EntitySalesH entityH, ObservableCollection<EntitySalesD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcSumDetail(ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);
        }

        // For Order
        public static void CalcSumDetail(EntityOrderH entityH, ObservableCollection<EntityOrderD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcSumDetail(ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);
        }

        // For Estimate
        public static void CalcSumDetail(EntityEstimateH entityH, ObservableCollection<EntityEstimateD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcSumDetail(ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);
        }

        // For Purchase Order
        public static void CalcSumDetail(EntityPurchaseOrderH entityH, ObservableCollection<EntityPurchaseOrderD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcSumDetail(ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);
        }

        // For Purchase
        public static void CalcSumDetail(EntityPurchaseH entityH, ObservableCollection<EntityPurchaseD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcSumDetail(ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);
        }

        // For InOutDelivery
        public static void CalcSumDetail(EntityInOutDeliveryH entityH, ObservableCollection<EntityInOutDeliveryD> entityD)
        {
            EntityBaseH entityBaseH = null;
            ObservableCollection<EntityBaseD> entityBaseD = null;
            ConvertFrom(entityH, ref entityBaseH, entityD, ref entityBaseD);

            _CalcSumDetail(ref entityBaseH, ref entityBaseD);

            ConvertTo(entityBaseH, ref entityH, entityBaseD, ref entityD);
        }

        #endregion@:

        #region ReCalcDetail : 明細再計算

        // For Sales
        public static void ReCalcDetail(EntitySalesH entityH, ObservableCollection<EntitySalesD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                // 明細計算
                CalcDetail(i, entityH, entityD);
            }

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Order
        public static void ReCalcDetail(EntityOrderH entityH, ObservableCollection<EntityOrderD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                // 明細計算
                CalcDetail(i, entityH, entityD);
            }

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Estimate
        public static void ReCalcDetail(EntityEstimateH entityH, ObservableCollection<EntityEstimateD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                // 明細計算
                CalcDetail(i, entityH, entityD);
            }

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Purchase Order
        public static void ReCalcDetail(EntityPurchaseOrderH entityH, ObservableCollection<EntityPurchaseOrderD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                // 明細計算
                CalcDetail(i, entityH, entityD);
            }

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For Purchase
        public static void ReCalcDetail(EntityPurchaseH entityH, ObservableCollection<EntityPurchaseD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                // 明細計算
                CalcDetail(i, entityH, entityD);
            }

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        // For InOutDelivery
        public static void ReCalcDetail(EntityInOutDeliveryH entityH, ObservableCollection<EntityInOutDeliveryD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                // 明細計算
                CalcDetail(i, entityH, entityD);
            }

            // 明細合計計算
            CalcSumDetail(entityH, entityD);
        }

        #endregion

        #region 明細赤処理

        public static void CalRedcDetail(EntityBaseD _entityD)
        {
            _entityD.case_number = _entityD.case_number * -1;
            _entityD.number = _entityD.number * -1;
            _entityD.tax = _entityD.tax * -1;
            _entityD.no_tax_price = _entityD.no_tax_price * -1;
            _entityD.price = _entityD.price * -1;
            _entityD.profits = _entityD.profits * -1;
        }

        public static void CalRedcDetail(EntitySalesD _entityD)
        {
            _entityD._case_number = _entityD._case_number * -1;
            _entityD._number = _entityD._number * -1;
            _entityD._tax = _entityD._tax * -1;
            _entityD._no_tax_price = _entityD._no_tax_price * -1;
            _entityD._price = _entityD._price * -1;
            _entityD._profits = _entityD._profits * -1;
        }

        public static void CalRedcDetail(EntityPurchaseD _entityD)
        {
            _entityD._case_number = _entityD._case_number * -1;
            _entityD._number = _entityD._number * -1;
            _entityD._tax = _entityD._tax * -1;
            _entityD._no_tax_price = _entityD._no_tax_price * -1;
            _entityD._price = _entityD._price * -1;
            //_entityD._profits = _entityD._profits * -1;
        }

        #endregion

        private static void SetInitCombo(ref svcMstData.EntityMstData entityD)
        {
            // コンボボックス初期選択
            List<string> lst;
            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.UNIT_ID);
            entityD.attribute1 = ExCast.zCStr(MeiNameList.GetID(MeiNameList.geNameKbn.UNIT_ID, lst[0]));

            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.TAX_DIVISION_ID);
            entityD.attribute5 = ExCast.zCStr(MeiNameList.GetID(MeiNameList.geNameKbn.TAX_DIVISION_ID, lst[0]));
        }

        #region ConvertFrom 

        // For Sales
        public static void ConvertFrom(EntitySalesH entityH, ref EntityBaseH entityBaseH, ObservableCollection<EntitySalesD> entityD, ref ObservableCollection<EntityBaseD> entityBaseD)
        {
            entityBaseD = new ObservableCollection<EntityBaseD>();

            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                EntityBaseD _entityBaseD = new EntityBaseD();

                #region Set Entity Detail

                _entityBaseD.id = entityD[i]._id;
                _entityBaseD.rec_no = entityD[i]._rec_no;
                _entityBaseD.breakdown_id = entityD[i]._breakdown_id;
                _entityBaseD.breakdown_nm = entityD[i]._breakdown_nm;
                _entityBaseD.deliver_division_id = entityD[i]._deliver_division_id;
                _entityBaseD.deliver_division_nm = entityD[i]._deliver_division_nm;
                _entityBaseD.commodity_id = entityD[i]._commodity_id;
                _entityBaseD.commodity_name = entityD[i]._commodity_name;
                _entityBaseD.unit_id = entityD[i]._unit_id;
                _entityBaseD.unit_nm = entityD[i]._unit_nm;
                _entityBaseD.enter_number = entityD[i]._enter_number;
                _entityBaseD.case_number = entityD[i]._case_number;
                _entityBaseD.number = entityD[i]._number;
                _entityBaseD.order_number = entityD[i]._order_number;
                _entityBaseD.order_stay_number = entityD[i]._order_stay_number;
                _entityBaseD.unit_price = entityD[i]._unit_price;
                _entityBaseD.sales_cost = entityD[i]._sales_cost;
                _entityBaseD.tax = entityD[i]._tax;
                _entityBaseD.no_tax_price = entityD[i]._no_tax_price;
                _entityBaseD.price = entityD[i]._price;
                _entityBaseD.profits = entityD[i]._profits;
                _entityBaseD.profits_percent = entityD[i]._profits_percent;
                _entityBaseD.tax_division_id = entityD[i]._tax_division_id;
                _entityBaseD.tax_division_nm = entityD[i]._tax_division_nm;
                _entityBaseD.tax_percent = entityD[i]._tax_percent;
                _entityBaseD.inventory_management_division_id = entityD[i]._inventory_management_division_id;
                _entityBaseD.inventory_number = entityD[i]._inventory_number;
                _entityBaseD.retail_price_skip_tax = entityD[i]._retail_price_skip_tax;
                _entityBaseD.retail_price_before_tax = entityD[i]._retail_price_before_tax;
                _entityBaseD.sales_unit_price_skip_tax = entityD[i]._sales_unit_price_skip_tax;
                _entityBaseD.sales_unit_price_before_tax = entityD[i]._sales_unit_price_before_tax;
                _entityBaseD.sales_cost_price_skip_tax = entityD[i]._sales_cost_price_skip_tax;
                _entityBaseD.sales_cost_price_before_tax = entityD[i]._sales_cost_price_before_tax;
                _entityBaseD.number_decimal_digit = entityD[i]._number_decimal_digit;
                _entityBaseD.unit_decimal_digit = entityD[i]._unit_decimal_digit;

                #endregion

                entityBaseD.Add(_entityBaseD);
            }

            entityBaseH = new EntityBaseH();

            #region Set Entity Head

            entityBaseH.tax_change_id = entityH._tax_change_id;
            entityBaseH.business_division_id = entityH._business_division_id;
            entityBaseH.price_fraction_proc_id = entityH._price_fraction_proc_id;
            entityBaseH.tax_fraction_proc_id = entityH._tax_fraction_proc_id;
            entityBaseH.unit_kind_id = entityH._unit_kind_id;
            entityBaseH.sum_enter_number = entityH._sum_enter_number;
            entityBaseH.sum_case_number = entityH._sum_case_number;
            entityBaseH.sum_number = entityH._sum_number;
            entityBaseH.sum_unit_price = entityH._sum_unit_price;
            entityBaseH.sum_sales_cost = entityH._sum_sales_cost;
            entityBaseH.sum_tax = entityH._sum_tax;
            entityBaseH.sum_no_tax_price = entityH._sum_no_tax_price;
            entityBaseH.sum_price = entityH._sum_price;
            entityBaseH.sum_profits = entityH._sum_profits;
            entityBaseH.profits_percent = entityH._profits_percent;
            entityBaseH.credit_rate = entityH._credit_rate;

            #endregion
        }

        // For Order
        public static void ConvertFrom(EntityOrderH entityH, ref EntityBaseH entityBaseH, ObservableCollection<EntityOrderD> entityD, ref ObservableCollection<EntityBaseD> entityBaseD)
        {
            entityBaseD = new ObservableCollection<EntityBaseD>();

            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                EntityBaseD _entityBaseD = new EntityBaseD();

                #region Set Entity Detail

                _entityBaseD.id = entityD[i]._id;
                _entityBaseD.rec_no = entityD[i]._rec_no;
                _entityBaseD.breakdown_id = entityD[i]._breakdown_id;
                _entityBaseD.breakdown_nm = entityD[i]._breakdown_nm;
                _entityBaseD.deliver_division_id = entityD[i]._deliver_division_id;
                _entityBaseD.deliver_division_nm = entityD[i]._deliver_division_nm;
                _entityBaseD.commodity_id = entityD[i]._commodity_id;
                _entityBaseD.commodity_name = entityD[i]._commodity_name;
                _entityBaseD.unit_id = entityD[i]._unit_id;
                _entityBaseD.unit_nm = entityD[i]._unit_nm;
                _entityBaseD.enter_number = entityD[i]._enter_number;
                _entityBaseD.case_number = entityD[i]._case_number;
                _entityBaseD.number = entityD[i]._number;
                _entityBaseD.unit_price = entityD[i]._unit_price;
                _entityBaseD.sales_cost = entityD[i]._sales_cost;
                _entityBaseD.tax = entityD[i]._tax;
                _entityBaseD.no_tax_price = entityD[i]._no_tax_price;
                _entityBaseD.price = entityD[i]._price;
                _entityBaseD.profits = entityD[i]._profits;
                _entityBaseD.profits_percent = entityD[i]._profits_percent;
                _entityBaseD.tax_division_id = entityD[i]._tax_division_id;
                _entityBaseD.tax_division_nm = entityD[i]._tax_division_nm;
                _entityBaseD.tax_percent = entityD[i]._tax_percent;
                _entityBaseD.inventory_management_division_id = entityD[i]._inventory_management_division_id;
                _entityBaseD.inventory_number = entityD[i]._inventory_number;
                _entityBaseD.retail_price_skip_tax = entityD[i]._retail_price_skip_tax;
                _entityBaseD.retail_price_before_tax = entityD[i]._retail_price_before_tax;
                _entityBaseD.sales_unit_price_skip_tax = entityD[i]._sales_unit_price_skip_tax;
                _entityBaseD.sales_unit_price_before_tax = entityD[i]._sales_unit_price_before_tax;
                _entityBaseD.sales_cost_price_skip_tax = entityD[i]._sales_cost_price_skip_tax;
                _entityBaseD.sales_cost_price_before_tax = entityD[i]._sales_cost_price_before_tax;
                _entityBaseD.number_decimal_digit = entityD[i]._number_decimal_digit;
                _entityBaseD.unit_decimal_digit = entityD[i]._unit_decimal_digit;

                #endregion

                entityBaseD.Add(_entityBaseD);
            }

            entityBaseH = new EntityBaseH();

            #region Set Entity Head

            entityBaseH.tax_change_id = entityH._tax_change_id;
            entityBaseH.business_division_id = entityH._business_division_id;
            entityBaseH.price_fraction_proc_id = entityH._price_fraction_proc_id;
            entityBaseH.tax_fraction_proc_id = entityH._tax_fraction_proc_id;
            entityBaseH.unit_kind_id = entityH._unit_kind_id;
            entityBaseH.sum_enter_number = entityH._sum_enter_number;
            entityBaseH.sum_case_number = entityH._sum_case_number;
            entityBaseH.sum_number = entityH._sum_number;
            entityBaseH.sum_unit_price = entityH._sum_unit_price;
            entityBaseH.sum_sales_cost = entityH._sum_sales_cost;
            entityBaseH.sum_tax = entityH._sum_tax;
            entityBaseH.sum_no_tax_price = entityH._sum_no_tax_price;
            entityBaseH.sum_price = entityH._sum_price;
            entityBaseH.sum_profits = entityH._sum_profits;
            entityBaseH.profits_percent = entityH._profits_percent;
            entityBaseH.credit_rate = entityH._credit_rate;

            #endregion
        }

        // For Estimate
        public static void ConvertFrom(EntityEstimateH entityH, ref EntityBaseH entityBaseH, ObservableCollection<EntityEstimateD> entityD, ref ObservableCollection<EntityBaseD> entityBaseD)
        {
            entityBaseD = new ObservableCollection<EntityBaseD>();

            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                EntityBaseD _entityBaseD = new EntityBaseD();

                #region Set Entity Detail

                _entityBaseD.id = entityD[i]._id;
                _entityBaseD.rec_no = entityD[i]._rec_no;
                _entityBaseD.breakdown_id = entityD[i]._breakdown_id;
                _entityBaseD.breakdown_nm = entityD[i]._breakdown_nm;
                _entityBaseD.deliver_division_id = entityD[i]._deliver_division_id;
                _entityBaseD.deliver_division_nm = entityD[i]._deliver_division_nm;
                _entityBaseD.commodity_id = entityD[i]._commodity_id;
                _entityBaseD.commodity_name = entityD[i]._commodity_name;
                _entityBaseD.unit_id = entityD[i]._unit_id;
                _entityBaseD.unit_nm = entityD[i]._unit_nm;
                _entityBaseD.enter_number = entityD[i]._enter_number;
                _entityBaseD.case_number = entityD[i]._case_number;
                _entityBaseD.number = entityD[i]._number;
                _entityBaseD.unit_price = entityD[i]._unit_price;
                _entityBaseD.sales_cost = entityD[i]._sales_cost;
                _entityBaseD.tax = entityD[i]._tax;
                _entityBaseD.no_tax_price = entityD[i]._no_tax_price;
                _entityBaseD.price = entityD[i]._price;
                _entityBaseD.profits = entityD[i]._profits;
                _entityBaseD.profits_percent = entityD[i]._profits_percent;
                _entityBaseD.tax_division_id = entityD[i]._tax_division_id;
                _entityBaseD.tax_division_nm = entityD[i]._tax_division_nm;
                _entityBaseD.tax_percent = entityD[i]._tax_percent;
                _entityBaseD.inventory_management_division_id = entityD[i]._inventory_management_division_id;
                _entityBaseD.inventory_number = entityD[i]._inventory_number;
                _entityBaseD.retail_price_skip_tax = entityD[i]._retail_price_skip_tax;
                _entityBaseD.retail_price_before_tax = entityD[i]._retail_price_before_tax;
                _entityBaseD.sales_unit_price_skip_tax = entityD[i]._sales_unit_price_skip_tax;
                _entityBaseD.sales_unit_price_before_tax = entityD[i]._sales_unit_price_before_tax;
                _entityBaseD.sales_cost_price_skip_tax = entityD[i]._sales_cost_price_skip_tax;
                _entityBaseD.sales_cost_price_before_tax = entityD[i]._sales_cost_price_before_tax;
                _entityBaseD.number_decimal_digit = entityD[i]._number_decimal_digit;
                _entityBaseD.unit_decimal_digit = entityD[i]._unit_decimal_digit;

                #endregion

                entityBaseD.Add(_entityBaseD);
            }

            entityBaseH = new EntityBaseH();

            #region Set Entity Head

            entityBaseH.tax_change_id = entityH._tax_change_id;
            entityBaseH.business_division_id = entityH._business_division_id;
            entityBaseH.price_fraction_proc_id = entityH._price_fraction_proc_id;
            entityBaseH.tax_fraction_proc_id = entityH._tax_fraction_proc_id;
            entityBaseH.unit_kind_id = entityH._unit_kind_id;
            entityBaseH.sum_enter_number = entityH._sum_enter_number;
            entityBaseH.sum_case_number = entityH._sum_case_number;
            entityBaseH.sum_number = entityH._sum_number;
            entityBaseH.sum_unit_price = entityH._sum_unit_price;
            entityBaseH.sum_sales_cost = entityH._sum_sales_cost;
            entityBaseH.sum_tax = entityH._sum_tax;
            entityBaseH.sum_no_tax_price = entityH._sum_no_tax_price;
            entityBaseH.sum_price = entityH._sum_price;
            entityBaseH.sum_profits = entityH._sum_profits;
            entityBaseH.profits_percent = entityH._profits_percent;
            entityBaseH.credit_rate = entityH._credit_rate;

            #endregion
        }

        // For Purchase Order
        public static void ConvertFrom(EntityPurchaseOrderH entityH, ref EntityBaseH entityBaseH, ObservableCollection<EntityPurchaseOrderD> entityD, ref ObservableCollection<EntityBaseD> entityBaseD)
        {
            entityBaseD = new ObservableCollection<EntityBaseD>();

            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                EntityBaseD _entityBaseD = new EntityBaseD();

                #region Set Entity Detail

                _entityBaseD.id = entityD[i]._id;
                _entityBaseD.rec_no = entityD[i]._rec_no;
                _entityBaseD.breakdown_id = entityD[i]._breakdown_id;
                _entityBaseD.breakdown_nm = entityD[i]._breakdown_nm;
                _entityBaseD.deliver_division_id = entityD[i]._deliver_division_id;
                _entityBaseD.deliver_division_nm = entityD[i]._deliver_division_nm;
                _entityBaseD.commodity_id = entityD[i]._commodity_id;
                _entityBaseD.commodity_name = entityD[i]._commodity_name;
                _entityBaseD.unit_id = entityD[i]._unit_id;
                _entityBaseD.unit_nm = entityD[i]._unit_nm;
                _entityBaseD.enter_number = entityD[i]._enter_number;
                _entityBaseD.case_number = entityD[i]._case_number;
                _entityBaseD.number = entityD[i]._number;
                _entityBaseD.unit_price = entityD[i]._unit_price;
                _entityBaseD.tax = entityD[i]._tax;
                _entityBaseD.no_tax_price = entityD[i]._no_tax_price;
                _entityBaseD.price = entityD[i]._price;
                _entityBaseD.tax_division_id = entityD[i]._tax_division_id;
                _entityBaseD.tax_division_nm = entityD[i]._tax_division_nm;
                _entityBaseD.tax_percent = entityD[i]._tax_percent;
                _entityBaseD.inventory_management_division_id = entityD[i]._inventory_management_division_id;
                _entityBaseD.inventory_number = entityD[i]._inventory_number;
                _entityBaseD.retail_price_skip_tax = entityD[i]._retail_price_skip_tax;
                _entityBaseD.retail_price_before_tax = entityD[i]._retail_price_before_tax;
                _entityBaseD.sales_unit_price_skip_tax = entityD[i]._sales_unit_price_skip_tax;
                _entityBaseD.sales_unit_price_before_tax = entityD[i]._sales_unit_price_before_tax;
                _entityBaseD.sales_cost_price_skip_tax = entityD[i]._sales_cost_price_skip_tax;
                _entityBaseD.sales_cost_price_before_tax = entityD[i]._sales_cost_price_before_tax;
                _entityBaseD.purchase_unit_price_skip_tax = entityD[i]._purchase_unit_price_skip_tax;
                _entityBaseD.purchase_unit_price_before_tax = entityD[i]._purchase_unit_price_before_tax;
                _entityBaseD.number_decimal_digit = entityD[i]._number_decimal_digit;
                _entityBaseD.unit_decimal_digit = entityD[i]._unit_decimal_digit;

                #endregion

                entityBaseD.Add(_entityBaseD);
            }

            entityBaseH = new EntityBaseH();

            #region Set Entity Head

            entityBaseH.tax_change_id = entityH._tax_change_id;
            entityBaseH.business_division_id = entityH._business_division_id;
            entityBaseH.price_fraction_proc_id = entityH._price_fraction_proc_id;
            entityBaseH.tax_fraction_proc_id = entityH._tax_fraction_proc_id;
            entityBaseH.unit_kind_id = entityH._unit_kind_id;
            entityBaseH.sum_enter_number = entityH._sum_enter_number;
            entityBaseH.sum_case_number = entityH._sum_case_number;
            entityBaseH.sum_number = entityH._sum_number;
            entityBaseH.sum_unit_price = entityH._sum_unit_price;
            entityBaseH.sum_tax = entityH._sum_tax;
            entityBaseH.sum_no_tax_price = entityH._sum_no_tax_price;
            entityBaseH.sum_price = entityH._sum_price;
            entityBaseH.credit_rate = entityH._credit_rate;

            #endregion
        }

        // For Purchase
        public static void ConvertFrom(EntityPurchaseH entityH, ref EntityBaseH entityBaseH, ObservableCollection<EntityPurchaseD> entityD, ref ObservableCollection<EntityBaseD> entityBaseD)
        {
            entityBaseD = new ObservableCollection<EntityBaseD>();

            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                EntityBaseD _entityBaseD = new EntityBaseD();

                #region Set Entity Detail

                _entityBaseD.id = entityD[i]._id;
                _entityBaseD.rec_no = entityD[i]._rec_no;
                _entityBaseD.breakdown_id = entityD[i]._breakdown_id;
                _entityBaseD.breakdown_nm = entityD[i]._breakdown_nm;
                _entityBaseD.deliver_division_id = entityD[i]._deliver_division_id;
                _entityBaseD.deliver_division_nm = entityD[i]._deliver_division_nm;
                _entityBaseD.commodity_id = entityD[i]._commodity_id;
                _entityBaseD.commodity_name = entityD[i]._commodity_name;
                _entityBaseD.unit_id = entityD[i]._unit_id;
                _entityBaseD.unit_nm = entityD[i]._unit_nm;
                _entityBaseD.enter_number = entityD[i]._enter_number;
                _entityBaseD.case_number = entityD[i]._case_number;
                _entityBaseD.number = entityD[i]._number;
                _entityBaseD.order_number = entityD[i]._purchase_order_number;
                _entityBaseD.order_stay_number = entityD[i]._purchase_order_stay_number;
                _entityBaseD.unit_price = entityD[i]._unit_price;
                //_entityBaseD.sales_cost = entityD[i]._sales_cost;
                _entityBaseD.tax = entityD[i]._tax;
                _entityBaseD.no_tax_price = entityD[i]._no_tax_price;
                _entityBaseD.price = entityD[i]._price;
                //_entityBaseD.profits = entityD[i]._profits;
                //_entityBaseD.profits_percent = entityD[i]._profits_percent;
                _entityBaseD.tax_division_id = entityD[i]._tax_division_id;
                _entityBaseD.tax_division_nm = entityD[i]._tax_division_nm;
                _entityBaseD.tax_percent = entityD[i]._tax_percent;
                _entityBaseD.inventory_management_division_id = entityD[i]._inventory_management_division_id;
                _entityBaseD.inventory_number = entityD[i]._inventory_number;
                _entityBaseD.retail_price_skip_tax = entityD[i]._retail_price_skip_tax;
                _entityBaseD.retail_price_before_tax = entityD[i]._retail_price_before_tax;
                _entityBaseD.sales_unit_price_skip_tax = entityD[i]._sales_unit_price_skip_tax;
                _entityBaseD.sales_unit_price_before_tax = entityD[i]._sales_unit_price_before_tax;
                _entityBaseD.sales_cost_price_skip_tax = entityD[i]._sales_cost_price_skip_tax;
                _entityBaseD.sales_cost_price_before_tax = entityD[i]._sales_cost_price_before_tax;
                _entityBaseD.purchase_unit_price_skip_tax = entityD[i]._purchase_unit_price_skip_tax;
                _entityBaseD.purchase_unit_price_before_tax = entityD[i]._purchase_unit_price_before_tax;                
                _entityBaseD.number_decimal_digit = entityD[i]._number_decimal_digit;
                _entityBaseD.unit_decimal_digit = entityD[i]._unit_decimal_digit;

                #endregion

                entityBaseD.Add(_entityBaseD);
            }

            entityBaseH = new EntityBaseH();

            #region Set Entity Head

            entityBaseH.tax_change_id = entityH._tax_change_id;
            entityBaseH.business_division_id = entityH._business_division_id;
            entityBaseH.price_fraction_proc_id = entityH._price_fraction_proc_id;
            entityBaseH.tax_fraction_proc_id = entityH._tax_fraction_proc_id;
            entityBaseH.unit_kind_id = entityH._unit_kind_id;
            entityBaseH.sum_enter_number = entityH._sum_enter_number;
            entityBaseH.sum_case_number = entityH._sum_case_number;
            entityBaseH.sum_number = entityH._sum_number;
            entityBaseH.sum_unit_price = entityH._sum_unit_price;
            //entityBaseH.sum_sales_cost = entityH._sum_sales_cost;
            entityBaseH.sum_tax = entityH._sum_tax;
            entityBaseH.sum_no_tax_price = entityH._sum_no_tax_price;
            entityBaseH.sum_price = entityH._sum_price;
            //entityBaseH.sum_profits = entityH._sum_profits;
            //entityBaseH.profits_percent = entityH._profits_percent;
            entityBaseH.credit_rate = entityH._credit_rate;

            #endregion

        }

        // For InOutDelivery
        public static void ConvertFrom(EntityInOutDeliveryH entityH, ref EntityBaseH entityBaseH, ObservableCollection<EntityInOutDeliveryD> entityD, ref ObservableCollection<EntityBaseD> entityBaseD)
        {
            entityBaseD = new ObservableCollection<EntityBaseD>();

            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                EntityBaseD _entityBaseD = new EntityBaseD();

                #region Set Entity Detail

                _entityBaseD.id = entityD[i]._id;
                _entityBaseD.rec_no = ExCast.zCInt(entityD[i]._rec_no);
                //_entityBaseD.breakdown_id = entityD[i]._breakdown_id;
                //_entityBaseD.breakdown_nm = entityD[i]._breakdown_nm;
                //_entityBaseD.deliver_division_id = entityD[i]._deliver_division_id;
                //_entityBaseD.deliver_division_nm = entityD[i]._deliver_division_nm;
                _entityBaseD.commodity_id = entityD[i]._commodity_id;
                _entityBaseD.commodity_name = entityD[i]._commodity_name;
                _entityBaseD.unit_id = entityD[i]._unit_id;
                _entityBaseD.unit_nm = entityD[i]._unit_nm;
                _entityBaseD.enter_number = entityD[i]._enter_number;
                _entityBaseD.case_number = entityD[i]._case_number;
                _entityBaseD.number = entityD[i]._number;
                //_entityBaseD.order_number = entityD[i]._purchase_order_number;
                //_entityBaseD.order_stay_number = entityD[i]._purchase_order_stay_number;
                //_entityBaseD.unit_price = entityD[i]._unit_price;
                //_entityBaseD.sales_cost = entityD[i]._sales_cost;
                //_entityBaseD.tax = entityD[i]._tax;
                //_entityBaseD.no_tax_price = entityD[i]._no_tax_price;
                //_entityBaseD.price = entityD[i]._price;
                //_entityBaseD.profits = entityD[i]._profits;
                //_entityBaseD.profits_percent = entityD[i]._profits_percent;
                //_entityBaseD.tax_division_id = entityD[i]._tax_division_id;
                //_entityBaseD.tax_division_nm = entityD[i]._tax_division_nm;
                //_entityBaseD.tax_percent = entityD[i]._tax_percent;
                _entityBaseD.inventory_management_division_id = entityD[i]._inventory_management_division_id;
                _entityBaseD.inventory_number = entityD[i]._inventory_number;
                //_entityBaseD.retail_price_skip_tax = entityD[i]._retail_price_skip_tax;
                //_entityBaseD.retail_price_before_tax = entityD[i]._retail_price_before_tax;
                //_entityBaseD.sales_unit_price_skip_tax = entityD[i]._sales_unit_price_skip_tax;
                //_entityBaseD.sales_unit_price_before_tax = entityD[i]._sales_unit_price_before_tax;
                //_entityBaseD.sales_cost_price_skip_tax = entityD[i]._sales_cost_price_skip_tax;
                //_entityBaseD.sales_cost_price_before_tax = entityD[i]._sales_cost_price_before_tax;
                //_entityBaseD.purchase_unit_price_skip_tax = entityD[i]._purchase_unit_price_skip_tax;
                //_entityBaseD.purchase_unit_price_before_tax = entityD[i]._purchase_unit_price_before_tax;
                _entityBaseD.number_decimal_digit = entityD[i]._number_decimal_digit;
                //_entityBaseD.unit_decimal_digit = entityD[i]._unit_decimal_digit;

                #endregion

                entityBaseD.Add(_entityBaseD);
            }

            entityBaseH = new EntityBaseH();

            #region Set Entity Head

            //entityBaseH.tax_change_id = entityH._tax_change_id;
            //entityBaseH.business_division_id = entityH._business_division_id;
            //entityBaseH.price_fraction_proc_id = entityH._price_fraction_proc_id;
            //entityBaseH.tax_fraction_proc_id = entityH._tax_fraction_proc_id;
            //entityBaseH.unit_kind_id = entityH._unit_kind_id;
            entityBaseH.sum_enter_number = entityH._sum_enter_number;
            entityBaseH.sum_case_number = entityH._sum_case_number;
            entityBaseH.sum_number = entityH._sum_number;
            //entityBaseH.sum_unit_price = entityH._sum_unit_price;
            //entityBaseH.sum_sales_cost = entityH._sum_sales_cost;
            //entityBaseH.sum_tax = entityH._sum_tax;
            //entityBaseH.sum_no_tax_price = entityH._sum_no_tax_price;
            //entityBaseH.sum_price = entityH._sum_price;
            //entityBaseH.sum_profits = entityH._sum_profits;
            //entityBaseH.profits_percent = entityH._profits_percent;
            //entityBaseH.credit_rate = entityH._credit_rate;

            #endregion

        }

        #endregion

        #region Convert To

        // For Sales
        public static void ConvertTo(EntityBaseH entityBaseH, ref EntitySalesH entityH, ObservableCollection<EntityBaseD> entityBaseD, ref ObservableCollection<EntitySalesD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                #region Set Entity

                entityD[i]._id = entityBaseD[i].id;
                entityD[i]._rec_no = entityBaseD[i].rec_no;
                entityD[i]._breakdown_id = entityBaseD[i].breakdown_id;
                entityD[i]._breakdown_nm = entityBaseD[i].breakdown_nm;
                entityD[i]._deliver_division_id = entityBaseD[i].deliver_division_id;
                entityD[i]._deliver_division_nm = entityBaseD[i].deliver_division_nm;
                entityD[i]._commodity_id = entityBaseD[i].commodity_id;
                entityD[i]._commodity_name = entityBaseD[i].commodity_name;
                entityD[i]._unit_id = entityBaseD[i].unit_id;
                entityD[i]._unit_nm = entityBaseD[i].unit_nm;
                entityD[i]._enter_number = entityBaseD[i].enter_number;
                entityD[i]._case_number = entityBaseD[i].case_number;
                entityD[i]._number = entityBaseD[i].number;
                entityD[i]._order_number = entityBaseD[i].order_number;
                entityD[i]._order_stay_number = entityBaseD[i].order_stay_number;
                entityD[i]._unit_price = entityBaseD[i].unit_price;
                entityD[i]._sales_cost = entityBaseD[i].sales_cost;
                entityD[i]._tax = entityBaseD[i].tax;
                entityD[i]._no_tax_price = entityBaseD[i].no_tax_price;
                entityD[i]._price = entityBaseD[i].price;
                entityD[i]._profits = entityBaseD[i].profits;
                entityD[i]._profits_percent = entityBaseD[i].profits_percent;
                entityD[i]._tax_division_id = entityBaseD[i].tax_division_id;
                entityD[i]._tax_division_nm = entityBaseD[i].tax_division_nm;
                entityD[i]._tax_percent = entityBaseD[i].tax_percent;
                entityD[i]._inventory_management_division_id = entityBaseD[i].inventory_management_division_id;
                entityD[i]._inventory_number = entityBaseD[i].inventory_number;
                entityD[i]._retail_price_skip_tax = entityBaseD[i].retail_price_skip_tax;
                entityD[i]._retail_price_before_tax = entityBaseD[i].retail_price_before_tax;
                entityD[i]._sales_unit_price_skip_tax = entityBaseD[i].sales_unit_price_skip_tax;
                entityD[i]._sales_unit_price_before_tax = entityBaseD[i].sales_unit_price_before_tax;
                entityD[i]._sales_cost_price_skip_tax = entityBaseD[i].sales_cost_price_skip_tax;
                entityD[i]._sales_cost_price_before_tax = entityBaseD[i].sales_cost_price_before_tax;
                //entityD[i]._purchase_unit_price_skip_tax = entityBaseD[i].sales_cost_price_skip_tax;
                //entityD[i]._purchase_unit_price_before_tax = entityBaseD[i].sales_cost_price_before_tax;
                entityD[i]._number_decimal_digit = entityBaseD[i].number_decimal_digit;
                entityD[i]._unit_decimal_digit = entityBaseD[i].unit_decimal_digit;

                #endregion
            }

            #region Set Entity Head

            entityH._tax_change_id = entityBaseH.tax_change_id;
            entityH._business_division_id = entityBaseH.business_division_id;
            entityH._price_fraction_proc_id = entityBaseH.price_fraction_proc_id;
            entityH._tax_fraction_proc_id = entityBaseH.tax_fraction_proc_id;
            entityH._unit_kind_id = entityBaseH.unit_kind_id;
            entityH._sum_enter_number = entityBaseH.sum_enter_number;
            entityH._sum_case_number = entityBaseH.sum_case_number;
            entityH._sum_number = entityBaseH.sum_number;
            entityH._sum_unit_price = entityBaseH.sum_unit_price;
            entityH._sum_sales_cost = entityBaseH.sum_sales_cost;
            entityH._sum_tax = entityBaseH.sum_tax;
            entityH._sum_no_tax_price = entityBaseH.sum_no_tax_price;
            entityH._sum_price = entityBaseH.sum_price;
            entityH._sum_profits = entityBaseH.sum_profits;
            entityH._profits_percent = entityBaseH.profits_percent;
            entityH._credit_rate = entityBaseH.credit_rate;

            #endregion

        }

        // For Order
        public static void ConvertTo(EntityBaseH entityBaseH, ref EntityOrderH entityH, ObservableCollection<EntityBaseD> entityBaseD, ref ObservableCollection<EntityOrderD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                #region Set Entity

                entityD[i]._id = entityBaseD[i].id;
                entityD[i]._rec_no = entityBaseD[i].rec_no;
                entityD[i]._breakdown_id = entityBaseD[i].breakdown_id;
                entityD[i]._breakdown_nm = entityBaseD[i].breakdown_nm;
                entityD[i]._deliver_division_id = entityBaseD[i].deliver_division_id;
                entityD[i]._deliver_division_nm = entityBaseD[i].deliver_division_nm;
                entityD[i]._commodity_id = entityBaseD[i].commodity_id;
                entityD[i]._commodity_name = entityBaseD[i].commodity_name;
                entityD[i]._unit_id = entityBaseD[i].unit_id;
                entityD[i]._unit_nm = entityBaseD[i].unit_nm;
                entityD[i]._enter_number = entityBaseD[i].enter_number;
                entityD[i]._case_number = entityBaseD[i].case_number;
                entityD[i]._number = entityBaseD[i].number;
                entityD[i]._unit_price = entityBaseD[i].unit_price;
                entityD[i]._sales_cost = entityBaseD[i].sales_cost;
                entityD[i]._tax = entityBaseD[i].tax;
                entityD[i]._no_tax_price = entityBaseD[i].no_tax_price;
                entityD[i]._price = entityBaseD[i].price;
                entityD[i]._profits = entityBaseD[i].profits;
                entityD[i]._profits_percent = entityBaseD[i].profits_percent;
                entityD[i]._tax_division_id = entityBaseD[i].tax_division_id;
                entityD[i]._tax_division_nm = entityBaseD[i].tax_division_nm;
                entityD[i]._tax_percent = entityBaseD[i].tax_percent;
                entityD[i]._inventory_management_division_id = entityBaseD[i].inventory_management_division_id;
                entityD[i]._inventory_number = entityBaseD[i].inventory_number;
                entityD[i]._retail_price_skip_tax = entityBaseD[i].retail_price_skip_tax;
                entityD[i]._retail_price_before_tax = entityBaseD[i].retail_price_before_tax;
                entityD[i]._sales_unit_price_skip_tax = entityBaseD[i].sales_unit_price_skip_tax;
                entityD[i]._sales_unit_price_before_tax = entityBaseD[i].sales_unit_price_before_tax;
                entityD[i]._sales_cost_price_skip_tax = entityBaseD[i].sales_cost_price_skip_tax;
                entityD[i]._sales_cost_price_before_tax = entityBaseD[i].sales_cost_price_before_tax;
                //entityD[i]._purchase_unit_price_skip_tax = entityBaseD[i].sales_cost_price_skip_tax;
                //entityD[i]._purchase_unit_price_before_tax = entityBaseD[i].sales_cost_price_before_tax;
                entityD[i]._number_decimal_digit = entityBaseD[i].number_decimal_digit;
                entityD[i]._unit_decimal_digit = entityBaseD[i].unit_decimal_digit;

                #endregion
            }

            #region Set Entity Head

            entityH._tax_change_id = entityBaseH.tax_change_id;
            entityH._business_division_id = entityBaseH.business_division_id;
            entityH._price_fraction_proc_id = entityBaseH.price_fraction_proc_id;
            entityH._tax_fraction_proc_id = entityBaseH.tax_fraction_proc_id;
            entityH._unit_kind_id = entityBaseH.unit_kind_id;
            entityH._sum_enter_number = entityBaseH.sum_enter_number;
            entityH._sum_case_number = entityBaseH.sum_case_number;
            entityH._sum_number = entityBaseH.sum_number;
            entityH._sum_unit_price = entityBaseH.sum_unit_price;
            entityH._sum_sales_cost = entityBaseH.sum_sales_cost;
            entityH._sum_tax = entityBaseH.sum_tax;
            entityH._sum_no_tax_price = entityBaseH.sum_no_tax_price;
            entityH._sum_price = entityBaseH.sum_price;
            entityH._sum_profits = entityBaseH.sum_profits;
            entityH._profits_percent = entityBaseH.profits_percent;
            entityH._credit_rate = entityBaseH.credit_rate;

            #endregion

        }

        // For Estimate
        public static void ConvertTo(EntityBaseH entityBaseH, ref EntityEstimateH entityH, ObservableCollection<EntityBaseD> entityBaseD, ref ObservableCollection<EntityEstimateD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                #region Set Entity

                entityD[i]._id = entityBaseD[i].id;
                entityD[i]._rec_no = entityBaseD[i].rec_no;
                entityD[i]._breakdown_id = entityBaseD[i].breakdown_id;
                entityD[i]._breakdown_nm = entityBaseD[i].breakdown_nm;
                entityD[i]._deliver_division_id = entityBaseD[i].deliver_division_id;
                entityD[i]._deliver_division_nm = entityBaseD[i].deliver_division_nm;
                entityD[i]._commodity_id = entityBaseD[i].commodity_id;
                entityD[i]._commodity_name = entityBaseD[i].commodity_name;
                entityD[i]._unit_id = entityBaseD[i].unit_id;
                entityD[i]._unit_nm = entityBaseD[i].unit_nm;
                entityD[i]._enter_number = entityBaseD[i].enter_number;
                entityD[i]._case_number = entityBaseD[i].case_number;
                entityD[i]._number = entityBaseD[i].number;
                entityD[i]._unit_price = entityBaseD[i].unit_price;
                entityD[i]._sales_cost = entityBaseD[i].sales_cost;
                entityD[i]._tax = entityBaseD[i].tax;
                entityD[i]._no_tax_price = entityBaseD[i].no_tax_price;
                entityD[i]._price = entityBaseD[i].price;
                entityD[i]._profits = entityBaseD[i].profits;
                entityD[i]._profits_percent = entityBaseD[i].profits_percent;
                entityD[i]._tax_division_id = entityBaseD[i].tax_division_id;
                entityD[i]._tax_division_nm = entityBaseD[i].tax_division_nm;
                entityD[i]._tax_percent = entityBaseD[i].tax_percent;
                entityD[i]._inventory_management_division_id = entityBaseD[i].inventory_management_division_id;
                entityD[i]._inventory_number = entityBaseD[i].inventory_number;
                entityD[i]._retail_price_skip_tax = entityBaseD[i].retail_price_skip_tax;
                entityD[i]._retail_price_before_tax = entityBaseD[i].retail_price_before_tax;
                entityD[i]._sales_unit_price_skip_tax = entityBaseD[i].sales_unit_price_skip_tax;
                entityD[i]._sales_unit_price_before_tax = entityBaseD[i].sales_unit_price_before_tax;
                entityD[i]._sales_cost_price_skip_tax = entityBaseD[i].sales_cost_price_skip_tax;
                entityD[i]._sales_cost_price_before_tax = entityBaseD[i].sales_cost_price_before_tax;
                //entityD[i]._purchase_unit_price_skip_tax = entityBaseD[i].sales_cost_price_skip_tax;
                //entityD[i]._purchase_unit_price_before_tax = entityBaseD[i].sales_cost_price_before_tax;
                entityD[i]._number_decimal_digit = entityBaseD[i].number_decimal_digit;
                entityD[i]._unit_decimal_digit = entityBaseD[i].unit_decimal_digit;

                #endregion
            }

            #region Set Entity Head

            entityH._tax_change_id = entityBaseH.tax_change_id;
            entityH._business_division_id = entityBaseH.business_division_id;
            entityH._price_fraction_proc_id = entityBaseH.price_fraction_proc_id;
            entityH._tax_fraction_proc_id = entityBaseH.tax_fraction_proc_id;
            entityH._unit_kind_id = entityBaseH.unit_kind_id;
            entityH._sum_enter_number = entityBaseH.sum_enter_number;
            entityH._sum_case_number = entityBaseH.sum_case_number;
            entityH._sum_number = entityBaseH.sum_number;
            entityH._sum_unit_price = entityBaseH.sum_unit_price;
            entityH._sum_sales_cost = entityBaseH.sum_sales_cost;
            entityH._sum_tax = entityBaseH.sum_tax;
            entityH._sum_no_tax_price = entityBaseH.sum_no_tax_price;
            entityH._sum_price = entityBaseH.sum_price;
            entityH._sum_profits = entityBaseH.sum_profits;
            entityH._profits_percent = entityBaseH.profits_percent;
            entityH._credit_rate = entityBaseH.credit_rate;

            #endregion

        }

        // For Purchase Order
        public static void ConvertTo(EntityBaseH entityBaseH, ref EntityPurchaseOrderH entityH, ObservableCollection<EntityBaseD> entityBaseD, ref ObservableCollection<EntityPurchaseOrderD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                #region Set Entity

                entityD[i]._id = entityBaseD[i].id;
                entityD[i]._rec_no = entityBaseD[i].rec_no;
                entityD[i]._breakdown_id = entityBaseD[i].breakdown_id;
                entityD[i]._breakdown_nm = entityBaseD[i].breakdown_nm;
                entityD[i]._deliver_division_id = entityBaseD[i].deliver_division_id;
                entityD[i]._deliver_division_nm = entityBaseD[i].deliver_division_nm;
                entityD[i]._commodity_id = entityBaseD[i].commodity_id;
                entityD[i]._commodity_name = entityBaseD[i].commodity_name;
                entityD[i]._unit_id = entityBaseD[i].unit_id;
                entityD[i]._unit_nm = entityBaseD[i].unit_nm;
                entityD[i]._enter_number = entityBaseD[i].enter_number;
                entityD[i]._case_number = entityBaseD[i].case_number;
                entityD[i]._number = entityBaseD[i].number;
                //entityD[i]._order_number = entityBaseD[i].order_number;
                //entityD[i]._order_stay_number = entityBaseD[i].order_stay_number;
                entityD[i]._unit_price = entityBaseD[i].unit_price;
                //entityD[i]._sales_cost = entityBaseD[i].sales_cost;
                entityD[i]._tax = entityBaseD[i].tax;
                entityD[i]._no_tax_price = entityBaseD[i].no_tax_price;
                entityD[i]._price = entityBaseD[i].price;
                //entityD[i]._profits = entityBaseD[i].profits;
                //entityD[i]._profits_percent = entityBaseD[i].profits_percent;
                entityD[i]._tax_division_id = entityBaseD[i].tax_division_id;
                entityD[i]._tax_division_nm = entityBaseD[i].tax_division_nm;
                entityD[i]._tax_percent = entityBaseD[i].tax_percent;
                entityD[i]._inventory_management_division_id = entityBaseD[i].inventory_management_division_id;
                entityD[i]._inventory_number = entityBaseD[i].inventory_number;
                entityD[i]._retail_price_skip_tax = entityBaseD[i].retail_price_skip_tax;
                entityD[i]._retail_price_before_tax = entityBaseD[i].retail_price_before_tax;
                entityD[i]._sales_unit_price_skip_tax = entityBaseD[i].sales_unit_price_skip_tax;
                entityD[i]._sales_unit_price_before_tax = entityBaseD[i].sales_unit_price_before_tax;
                entityD[i]._sales_cost_price_skip_tax = entityBaseD[i].sales_cost_price_skip_tax;
                entityD[i]._sales_cost_price_before_tax = entityBaseD[i].sales_cost_price_before_tax;
                entityD[i]._purchase_unit_price_skip_tax = entityBaseD[i].purchase_unit_price_skip_tax;
                entityD[i]._purchase_unit_price_before_tax = entityBaseD[i].purchase_unit_price_before_tax;
                entityD[i]._number_decimal_digit = entityBaseD[i].number_decimal_digit;
                entityD[i]._unit_decimal_digit = entityBaseD[i].unit_decimal_digit;

                #endregion
            }

            #region Set Entity Head

            entityH._tax_change_id = entityBaseH.tax_change_id;
            entityH._business_division_id = entityBaseH.business_division_id;
            entityH._price_fraction_proc_id = entityBaseH.price_fraction_proc_id;
            entityH._tax_fraction_proc_id = entityBaseH.tax_fraction_proc_id;
            entityH._unit_kind_id = entityBaseH.unit_kind_id;
            entityH._sum_enter_number = entityBaseH.sum_enter_number;
            entityH._sum_case_number = entityBaseH.sum_case_number;
            entityH._sum_number = entityBaseH.sum_number;
            entityH._sum_unit_price = entityBaseH.sum_unit_price;
            //entityH._sum_sales_cost = entityBaseH.sum_sales_cost;
            entityH._sum_tax = entityBaseH.sum_tax;
            entityH._sum_no_tax_price = entityBaseH.sum_no_tax_price;
            entityH._sum_price = entityBaseH.sum_price;
            //entityH._sum_profits = entityBaseH.sum_profits;
            //entityH._profits_percent = entityBaseH.profits_percent;
            entityH._credit_rate = entityBaseH.credit_rate;
            entityH._payment_credit_price = entityBaseH.payment_credit_price;

            #endregion

        }

        // For Purchase
        public static void ConvertTo(EntityBaseH entityBaseH, ref EntityPurchaseH entityH, ObservableCollection<EntityBaseD> entityBaseD, ref ObservableCollection<EntityPurchaseD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                #region Set Entity

                entityD[i]._id = entityBaseD[i].id;
                entityD[i]._rec_no = entityBaseD[i].rec_no;
                entityD[i]._breakdown_id = entityBaseD[i].breakdown_id;
                entityD[i]._breakdown_nm = entityBaseD[i].breakdown_nm;
                entityD[i]._deliver_division_id = entityBaseD[i].deliver_division_id;
                entityD[i]._deliver_division_nm = entityBaseD[i].deliver_division_nm;
                entityD[i]._commodity_id = entityBaseD[i].commodity_id;
                entityD[i]._commodity_name = entityBaseD[i].commodity_name;
                entityD[i]._unit_id = entityBaseD[i].unit_id;
                entityD[i]._unit_nm = entityBaseD[i].unit_nm;
                entityD[i]._enter_number = entityBaseD[i].enter_number;
                entityD[i]._case_number = entityBaseD[i].case_number;
                entityD[i]._number = entityBaseD[i].number;
                entityD[i]._purchase_order_number = entityBaseD[i].order_number;
                entityD[i]._purchase_order_stay_number = entityBaseD[i].order_stay_number;
                entityD[i]._unit_price = entityBaseD[i].unit_price;
                //entityD[i]._sales_cost = entityBaseD[i].sales_cost;
                entityD[i]._tax = entityBaseD[i].tax;
                entityD[i]._no_tax_price = entityBaseD[i].no_tax_price;
                entityD[i]._price = entityBaseD[i].price;
                //entityD[i]._profits = entityBaseD[i].profits;
                //entityD[i]._profits_percent = entityBaseD[i].profits_percent;
                entityD[i]._tax_division_id = entityBaseD[i].tax_division_id;
                entityD[i]._tax_division_nm = entityBaseD[i].tax_division_nm;
                entityD[i]._tax_percent = entityBaseD[i].tax_percent;
                entityD[i]._inventory_management_division_id = entityBaseD[i].inventory_management_division_id;
                entityD[i]._inventory_number = entityBaseD[i].inventory_number;
                entityD[i]._retail_price_skip_tax = entityBaseD[i].retail_price_skip_tax;
                entityD[i]._retail_price_before_tax = entityBaseD[i].retail_price_before_tax;
                entityD[i]._sales_unit_price_skip_tax = entityBaseD[i].sales_unit_price_skip_tax;
                entityD[i]._sales_unit_price_before_tax = entityBaseD[i].sales_unit_price_before_tax;
                entityD[i]._sales_cost_price_skip_tax = entityBaseD[i].sales_cost_price_skip_tax;
                entityD[i]._sales_cost_price_before_tax = entityBaseD[i].sales_cost_price_before_tax;
                entityD[i]._purchase_unit_price_skip_tax = entityBaseD[i].sales_cost_price_skip_tax;
                entityD[i]._purchase_unit_price_before_tax = entityBaseD[i].sales_cost_price_before_tax;
                entityD[i]._number_decimal_digit = entityBaseD[i].number_decimal_digit;
                entityD[i]._unit_decimal_digit = entityBaseD[i].unit_decimal_digit;

                #endregion
            }

            #region Set Entity Head

            entityH._tax_change_id = entityBaseH.tax_change_id;
            entityH._business_division_id = entityBaseH.business_division_id;
            entityH._price_fraction_proc_id = entityBaseH.price_fraction_proc_id;
            entityH._tax_fraction_proc_id = entityBaseH.tax_fraction_proc_id;
            entityH._unit_kind_id = entityBaseH.unit_kind_id;
            entityH._sum_enter_number = entityBaseH.sum_enter_number;
            entityH._sum_case_number = entityBaseH.sum_case_number;
            entityH._sum_number = entityBaseH.sum_number;
            entityH._sum_unit_price = entityBaseH.sum_unit_price;
            //entityH._sum_sales_cost = entityBaseH.sum_sales_cost;
            entityH._sum_tax = entityBaseH.sum_tax;
            entityH._sum_no_tax_price = entityBaseH.sum_no_tax_price;
            entityH._sum_price = entityBaseH.sum_price;
            //entityH._sum_profits = entityBaseH.sum_profits;
            //entityH._profits_percent = entityBaseH.profits_percent;
            entityH._credit_rate = entityBaseH.credit_rate;

            #endregion

        }

        // For InOutDelivery
        public static void ConvertTo(EntityBaseH entityBaseH, ref EntityInOutDeliveryH entityH, ObservableCollection<EntityBaseD> entityBaseD, ref ObservableCollection<EntityInOutDeliveryD> entityD)
        {
            for (int i = 0; i <= entityD.Count - 1; i++)
            {
                #region Set Entity

                entityD[i]._id = entityBaseD[i].id;
                entityD[i]._rec_no = entityBaseD[i].rec_no;
                //entityD[i]._breakdown_id = entityBaseD[i].breakdown_id;
                //entityD[i]._breakdown_nm = entityBaseD[i].breakdown_nm;
                //entityD[i]._deliver_division_id = entityBaseD[i].deliver_division_id;
                //entityD[i]._deliver_division_nm = entityBaseD[i].deliver_division_nm;
                entityD[i]._commodity_id = entityBaseD[i].commodity_id;
                entityD[i]._commodity_name = entityBaseD[i].commodity_name;
                entityD[i]._unit_id = entityBaseD[i].unit_id;
                entityD[i]._unit_nm = entityBaseD[i].unit_nm;
                entityD[i]._enter_number = entityBaseD[i].enter_number;
                entityD[i]._case_number = entityBaseD[i].case_number;
                entityD[i]._number = entityBaseD[i].number;
                //entityD[i]._order_number = entityBaseD[i].order_number;
                //entityD[i]._order_stay_number = entityBaseD[i].order_stay_number;
                //entityD[i]._unit_price = entityBaseD[i].unit_price;
                //entityD[i]._sales_cost = entityBaseD[i].sales_cost;
                //entityD[i]._tax = entityBaseD[i].tax;
                //entityD[i]._no_tax_price = entityBaseD[i].no_tax_price;
                //entityD[i]._price = entityBaseD[i].price;
                //entityD[i]._profits = entityBaseD[i].profits;
                //entityD[i]._profits_percent = entityBaseD[i].profits_percent;
                //entityD[i]._tax_division_id = entityBaseD[i].tax_division_id;
                //entityD[i]._tax_division_nm = entityBaseD[i].tax_division_nm;
                //entityD[i]._tax_percent = entityBaseD[i].tax_percent;
                entityD[i]._inventory_management_division_id = entityBaseD[i].inventory_management_division_id;
                entityD[i]._inventory_number = entityBaseD[i].inventory_number;
                //entityD[i]._retail_price_skip_tax = entityBaseD[i].retail_price_skip_tax;
                //entityD[i]._retail_price_before_tax = entityBaseD[i].retail_price_before_tax;
                //entityD[i]._sales_unit_price_skip_tax = entityBaseD[i].sales_unit_price_skip_tax;
                //entityD[i]._sales_unit_price_before_tax = entityBaseD[i].sales_unit_price_before_tax;
                //entityD[i]._sales_cost_price_skip_tax = entityBaseD[i].sales_cost_price_skip_tax;
                //entityD[i]._sales_cost_price_before_tax = entityBaseD[i].sales_cost_price_before_tax;
                entityD[i]._number_decimal_digit = entityBaseD[i].number_decimal_digit;
                //entityD[i]._unit_decimal_digit = entityBaseD[i].unit_decimal_digit;

                #endregion
            }

            #region Set Entity Head

            //entityH._tax_change_id = entityBaseH.tax_change_id;
            //entityH._business_division_id = entityBaseH.business_division_id;
            //entityH._price_fraction_proc_id = entityBaseH.price_fraction_proc_id;
            //entityH._tax_fraction_proc_id = entityBaseH.tax_fraction_proc_id;
            //entityH._unit_kind_id = entityBaseH.unit_kind_id;
            entityH._sum_enter_number = entityBaseH.sum_enter_number;
            entityH._sum_case_number = entityBaseH.sum_case_number;
            entityH._sum_number = entityBaseH.sum_number;
            //entityH._sum_unit_price = entityBaseH.sum_unit_price;
            //entityH._sum_sales_cost = entityBaseH.sum_sales_cost;
            //entityH._sum_tax = entityBaseH.sum_tax;
            //entityH._sum_no_tax_price = entityBaseH.sum_no_tax_price;
            //entityH._sum_price = entityBaseH.sum_price;
            //entityH._sum_profits = entityBaseH.sum_profits;
            //entityH._profits_percent = entityBaseH.profits_percent;
            //entityH._credit_rate = entityBaseH.credit_rate;

            #endregion

        }

        #endregion

    }

    public class EntityBaseH
    {
        private int _tax_change_id;
        public int tax_change_id
        {
            set
            {
                this._tax_change_id = value;
            }
            get { return this._tax_change_id; }
        }

        private int _business_division_id;
        public int business_division_id
        {
            set
            {
                this._business_division_id = value;
            }
            get { return this._business_division_id; }
        }

        // 入数計
        private double _sum_enter_number = 0;
        public double sum_enter_number
        {
            set
            {
                this._sum_enter_number = value;
            }
            get { return this._sum_enter_number; }
        }

        // ケース数計
        private double _sum_case_number = 0;
        public double sum_case_number
        {
            set
            {
                this._sum_case_number = value;
            }
            get { return this._sum_case_number; }
        }

        // 数量計
        private double _sum_number = 0;
        public double sum_number
        {
            set
            {
                this._sum_number = value;
            }
            get { return this._sum_number; }
        }

        // 単価計
        private double _sum_unit_price = 0;
        public double sum_unit_price
        {
            set
            {
                this._sum_unit_price = value;
            }
            get { return this._sum_unit_price; }
        }

        // 売上原価計
        private double _sum_sales_cost = 0;
        public double sum_sales_cost
        {
            set
            {
                this._sum_sales_cost = value;
            }
            get { return this._sum_sales_cost; }
        }

        // 消費税額計
        private double _sum_tax = 0;
        public double sum_tax
        {
            set
            {
                this._sum_tax = value;
            }
            get { return this._sum_tax; }
        }

        // 税抜金額計
        private double _sum_no_tax_price = 0;
        public double sum_no_tax_price
        {
            set
            {
                this._sum_no_tax_price = value;
            }
            get { return this._sum_no_tax_price; }
        }

        // 金額計
        private double _sum_price = 0;
        public double sum_price
        {
            set
            {
                this._sum_price = value;
            }
            get { return this._sum_price; }
        }

        // 粗利計(売上金額計-売上原価計)
        private double _sum_profits = 0;
        public double sum_profits
        {
            set
            {
                this._sum_profits = value;
            }
            get { return this._sum_profits; }
        }

        // 粗利率(売上原価計÷売上金額計×100)
        private double _profits_percent = 0;
        public double profits_percent
        {
            set
            {
                this._profits_percent = value;
            }
            get { return this._profits_percent; }
        }

        // 金額端数処理ID
        private int _price_fraction_proc_id;
        public int price_fraction_proc_id
        {
            set
            {
                this._price_fraction_proc_id = value;
            }
            get { return this._price_fraction_proc_id; }
        }

        // 税端数処理ID
        private int _tax_fraction_proc_id = 0;
        public int tax_fraction_proc_id
        {
            set
            {
                this._tax_fraction_proc_id = value;
            }
            get { return this._tax_fraction_proc_id; }
        }

        // 単価種類ID
        private int _unit_kind_id = 0;
        public int unit_kind_id
        {
            set
            {
                this._unit_kind_id = value;
            }
            get { return this._unit_kind_id; }
        }

        // 与信金額
        private double _credit_limit_price = 0;
        public double credit_limit_price
        {
            set
            {
                this._credit_limit_price = value;
            }
            get { return this._credit_limit_price; }
        }

        // 売掛金額
        private double _sales_credit_price = 0;
        public double sales_credit_price
        {
            set
            {
                this._sales_credit_price = value;
            }
            get { return this._sales_credit_price; }
        }

        // 買掛金額
        private double _payment_credit_price = 0;
        public double payment_credit_price
        {
            set
            {
                this._payment_credit_price = value;
            }
            get { return this._payment_credit_price; }
        }

        // 掛率
        private int _credit_rate = 0;
        public int credit_rate
        {
            set
            {
                this._credit_rate = value;
            }
            get
            {
                return this._credit_rate;
            }
        }
    }

    public class EntityBaseD
    {
        private long _id;
        public long id
        {
            set
            {
                this._id = value;
            }
            get { return this._id; }
        }

        private int _rec_no;
        public int rec_no
        {
            set
            {
                this._rec_no = value;
            }
            get { return this._rec_no; }
        }

        private int _breakdown_id;
        public int breakdown_id
        {
            set
            {
                this._breakdown_id = value;
            }
            get { return this._breakdown_id; }
        }

        private string _breakdown_nm;
        public string breakdown_nm
        {
            set
            {
                this._breakdown_nm = value;
            }
            get { return this._breakdown_nm; }
        }

        private int _deliver_division_id;
        public int deliver_division_id
        {
            set
            {
                this._deliver_division_id = value;
            }
            get { return this._deliver_division_id; }
        }

        private string _deliver_division_nm;
        public string deliver_division_nm
        {
            set
            {
                this._deliver_division_nm = value;
            }
            get { return this._deliver_division_nm; }
        }

        private string _commodity_id;
        public string commodity_id
        {
            set
            {
                this._commodity_id = value;
            }
            get { return this._commodity_id; }
        }

        private string _commodity_name;
        public string commodity_name
        {
            set
            {
                this._commodity_name = value;
            }
            get { return this._commodity_name; }
        }

        private int _unit_id;
        public int unit_id
        {
            set
            {
                this._unit_id = value;
            }
            get { return this._unit_id; }
        }

        private string _unit_nm;
        public string unit_nm
        {
            set
            {
                this._unit_nm = value;
            }
            get { return this._unit_nm; }
        }

        private double _enter_number;
        public double enter_number
        {
            set
            {
                this._enter_number = value;
            }
            get { return this._enter_number; }
        }

        private double _case_number;
        public double case_number
        {
            set
            {
                this._case_number = value;
            }
            get { return this._case_number; }
        }

        private double _number;
        public double number
        {
            set
            {
                this._number = value;
            }
            get { return this._number; }
        }

        private double _unit_price;
        public double unit_price
        {
            set
            {
                this._unit_price = value;
            }
            get { return this._unit_price; }
        }

        private double _sales_cost;
        public double sales_cost
        {
            set
            {
                this._sales_cost = value;
            }
            get { return this._sales_cost; }
        }

        private double _tax;
        public double tax
        {
            set
            {
                this._tax = value;
            }
            get { return this._tax; }
        }

        private double _no_tax_price;
        public double no_tax_price
        {
            set
            {
                this._no_tax_price = value;
            }
            get { return this._no_tax_price; }
        }

        private double _price;
        public double price
        {
            set
            {
                this._price = value;
            }
            get { return this._price; }
        }

        private double _profits;
        public double profits
        {
            set
            {
                this._profits = value;
            }
            get { return this._profits; }
        }

        private double _profits_percent = 0;
        public double profits_percent
        {
            set
            {
                this._profits_percent = value;
            }
            get { return this._profits_percent; }
        }

        private int _tax_division_id;
        public int tax_division_id
        {
            set
            {
                this._tax_division_id = value;
            }
            get { return this._tax_division_id; }
        }

        private string _tax_division_nm;
        public string tax_division_nm
        {
            set
            {
                this._tax_division_nm = value;
            }
            get { return this._tax_division_nm; }
        }

        private int _tax_percent;
        public int tax_percent
        {
            set
            {
                this._tax_percent = value;
            }
            get { return this._tax_percent; }
        }

        // 在庫管理区分
        private int _inventory_management_division_id;
        public int inventory_management_division_id
        {
            set
            {
                this._inventory_management_division_id = value;
            }
            get { return this._inventory_management_division_id; }
        }

        // 現在庫
        private double _inventory_number;
        public double inventory_number
        {
            set
            {
                this._inventory_number = value;
            }
            get { return this._inventory_number; }
        }

        // 上代税抜
        private double _retail_price_skip_tax;
        public double retail_price_skip_tax
        {
            set
            {
                this._retail_price_skip_tax = value;
            }
            get { return this._retail_price_skip_tax; }
        }

        // 上代税込
        private double _retail_price_before_tax;
        public double retail_price_before_tax
        {
            set
            {
                this._retail_price_before_tax = value;
            }
            get { return this._retail_price_before_tax; }
        }

        // 売上単価税抜
        private double _sales_unit_price_skip_tax;
        public double sales_unit_price_skip_tax
        {
            set
            {
                this._sales_unit_price_skip_tax = value;
            }
            get { return this._sales_unit_price_skip_tax; }
        }

        // 売上単価税込
        private double _sales_unit_price_before_tax;
        public double sales_unit_price_before_tax
        {
            set
            {
                this._sales_unit_price_before_tax = value;
            }
            get { return this._sales_unit_price_before_tax; }
        }

        // 売上原価税抜
        private double _sales_cost_price_skip_tax;
        public double sales_cost_price_skip_tax
        {
            set
            {
                this._sales_cost_price_skip_tax = value;
            }
            get { return this._sales_cost_price_skip_tax; }
        }

        // 売上原価税込
        private double _sales_cost_price_before_tax;
        public double sales_cost_price_before_tax
        {
            set
            {
                this._sales_cost_price_before_tax = value;
            }
            get { return this._sales_cost_price_before_tax; }
        }

        // 仕入単価税抜
        private double _purchase_unit_price_skip_tax;
        public double purchase_unit_price_skip_tax
        {
            set
            {
                this._purchase_unit_price_skip_tax = value;
            }
            get { return this._purchase_unit_price_skip_tax; }
        }

        // 仕入単価税込
        private double _purchase_unit_price_before_tax;
        public double purchase_unit_price_before_tax
        {
            set
            {
                this._purchase_unit_price_before_tax = value;
            }
            get { return this._purchase_unit_price_before_tax; }
        }

        // 数量小数桁
        private int _number_decimal_digit;
        public int number_decimal_digit
        {
            set
            {
                this._number_decimal_digit = value;
            }
            get { return this._number_decimal_digit; }
        }

        // 単価小数桁
        private int _unit_decimal_digit;
        public int unit_decimal_digit
        {
            set
            {
                this._unit_decimal_digit = value;
            }
            get { return this._unit_decimal_digit; }
        }

        // 受注数
        private double _order_number;
        public double order_number
        {
            set
            {
                this._order_number = value;
            }
            get { return this._order_number; }
        }


        // 受注残数
        private double _order_stay_number;
        public double order_stay_number
        {
            set
            {
                this._order_stay_number = value;
            }
            get { return this._order_stay_number; }
        }

        private string _memo;
        public string memo
        {
            set
            {
                this._memo = value;
            }
            get { return this._memo; }
        }

        private int _lock_flg = 0;
        public int lock_flg
        {
            set
            {
                this._lock_flg = value;
            }
            get
            {
                return this._lock_flg;
            }
        }

        private string _message;
        public string message
        {
            set
            {
                this._message = value;
            }
            get { return this._message; }
        }

        public EntityBaseD()
        {
        }
    }
}
