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
using SlvHanbaiClient.View.UserControl.DataForm;
using SlvHanbaiClient.View.UserControl.DataForm.Sales;

namespace SlvHanbaiClient.Class.UI
{
    public class ExComboBox : ComboBox
    {
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:

                    #region DataForm

                    ExDataForm _df = (ExDataForm)ExVisualTreeHelper.FindPerentDataForm(this);
                    if (_df != null)
                    {
                        ExUserControl _utl = (ExUserControl)ExVisualTreeHelper.FindPerentUserControl(this);

                        #region DataForm EstimateDetail

                        Utl_DataFormEstimate utlEstimate = null;
                        try
                        {
                            utlEstimate = (Utl_DataFormEstimate)_utl;
                        }
                        catch
                        {
                        }
                        if (utlEstimate != null)
                        {
                            if (Keyboard.Modifiers == ModifierKeys.Shift)
                            {
                                utlEstimate.OnBeforeControl();
                                e.Handled = true;
                                return;
                            }
                            else
                            {
                                utlEstimate.OnNextControl();
                                e.Handled = true;
                                return;
                            }
                        }

                        #endregion

                        #region DataForm OrderDetail

                        Utl_DataFormOrder utlOrder = null;
                        try
                        {
                            utlOrder = (Utl_DataFormOrder)_utl; 
                        }
                        catch
                        { 
                        }
                        if (utlOrder != null)
                        {
                            if (Keyboard.Modifiers == ModifierKeys.Shift)
                            {
                                utlOrder.OnBeforeControl();
                                e.Handled = true;
                                return;
                            }
                            else
                            {
                                utlOrder.OnNextControl();
                                e.Handled = true;
                                return;
                            }
                        }

                        #endregion

                        #region DataForm SalesDetail

                        Utl_DataFormSales utlSales = null;
                        try
                        {
                            utlSales = (Utl_DataFormSales)_utl;
                        }
                        catch
                        {
                        }
                        if (utlSales != null)
                        {
                            if (Keyboard.Modifiers == ModifierKeys.Shift)
                            {
                                utlSales.OnBeforeControl();
                                e.Handled = true;
                                return;
                            }
                            else
                            {
                                utlSales.OnNextControl();
                                e.Handled = true;
                                return;
                            }
                        }

                        #endregion

                    }

                    #endregion

                    #region DataGrid

                    ExDataGrid _dg = (ExDataGrid)ExVisualTreeHelper.FindPerentDataGrid(this);
                    if (_dg != null)
                    { 
                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            //_dg.MoveBeforeCell();
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                            //_dg.MoveNextCell();
                            e.Handled = true;
                            return;
                        }
                    }

                    #endregion

                    #region Page

                    ExPage _page = (ExPage)ExVisualTreeHelper.FindPerentPage(this);
                    if (_page != null)
                    { 
                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            _page.OnBeforeControl();
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                            _page.OnNextControl();
                            e.Handled = true;
                            return;
                        }
                    }

                    #endregion

                    #region ChildWindow

                    ExChildWindow _win = (ExChildWindow)ExVisualTreeHelper.FindPerentChildWindow(this);
                    if (_win != null)
                    { 
                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            _win.OnBeforeControl();
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                            _win.OnNextControl();
                            e.Handled = true;
                            return;
                        }
                    }

                    #endregion

                    break;
            }

            base.OnKeyDown(e);
        }
    }
}
