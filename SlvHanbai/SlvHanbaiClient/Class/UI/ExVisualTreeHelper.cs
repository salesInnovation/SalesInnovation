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
using System.Collections.Generic;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.UserControl.DataForm;
using SlvHanbaiClient.Class.Utility;

#endregion

namespace SlvHanbaiClient.Class.UI
{
    public class ExVisualTreeHelper
    {
        #region Constructor

        public ExVisualTreeHelper()

            : base()
        {

        }

        #endregion

        #region Method

        public static void FoucsNextControlNoFocus(Control ctl)
        {
            ExPage page = (ExPage)ExVisualTreeHelper.FindPerentPage(ctl);
            if (page != null)
            {
                page.OnNextControl();
            }
            else
            {
                ExChildWindow _win = (ExChildWindow)ExVisualTreeHelper.FindPerentChildWindow(ctl);
                if (_win != null) _win.OnNextControl();
            }
        }

        public static void FoucsNextControl(Control ctl)
        {
            ctl.Focus();
            ExPage page = (ExPage)ExVisualTreeHelper.FindPerentPage(ctl);
            if (page != null)
            {
                page.OnNextControl();
            }
            else
            {
                ExChildWindow _win = (ExChildWindow)ExVisualTreeHelper.FindPerentChildWindow(ctl);
                if (_win != null) _win.OnNextControl();
            }
        }

        public static Control GetBeforeDisplayTabIndexCtl(List<ListDisplayTabIndex> list, string searchCtlName)
        {
            Control ctl = null;

            foreach (ListDisplayTabIndex lst in list)
            {
                try
                {
                    if (lst.controlName == searchCtlName)
                    {
                        return ctl;
                    }
                    else
                    {
                        ctl = lst.ctl;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public static Control GetNextDisplayTabIndexCtl(List<ListDisplayTabIndex> list, string searchCtlName)
        {
            int flg = 0;

            foreach (ListDisplayTabIndex lst in list)
            {
                try
                {
                    if (flg == 1)
                    {
                        return lst.ctl;
                    }
                    if (lst.controlName == searchCtlName)
                    {
                        flg = 1;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public static string GetTagDisplayTabIndex(List<ListDisplayTabIndex> list, string searchCtlName)
        {
            foreach (ListDisplayTabIndex lst in list)
            {
                try
                {
                    if (lst.controlName == searchCtlName)
                    {
                        return lst.tag;
                    }
                }
                catch
                {
                }
            }
            return "";
        }

        public static List<ListDisplayTabIndex> GetDisplayTabIndex(DependencyObject RootObj)
        {
            List<ListDisplayTabIndex> _list = new List<ListDisplayTabIndex>();
            int i = 0;

            foreach (DependencyObject ob in FindVisualChildrenNoType(RootObj))
            {
                Control ctl = null;
                try
                {
                    if (ob is ExTextBox)
                    {
                        ExTextBox txt = (ExTextBox)ob;
                        if (txt.Tag.ToString().IndexOf("TabSearchOn") != -1 && txt.IsTabStop == true)
                        {
                            ctl = (Control)ob;
                            _list.Add(new ListDisplayTabIndex(i, ctl.Name, ExCast.zCStr(ctl.Tag), ctl));
                        }                        
                    }
                    if (ob is ExUserControl && (ob is Utl_Zip) == false)
                    {
                        ExUserControl utl = (ExUserControl)ob;
                        if (utl.Tag.ToString().IndexOf("TabSearchOn") != -1 && utl.IsTabStop == true)
                        {
                            ctl = (Control)ob;
                            _list.Add(new ListDisplayTabIndex(i, ctl.Name, ExCast.zCStr(ctl.Tag), ctl));
                        }
                    }
                    if (ob is Utl_Zip)
                    {
                        Utl_Zip zip = (Utl_Zip)ob;
                        i += 1;
                        _list.Add(new ListDisplayTabIndex(i, zip.txtZipNo1.Name, ExCast.zCStr(zip.Tag), zip.txtZipNo1));
                        i += 1;
                        _list.Add(new ListDisplayTabIndex(i, zip.txtZipNo2.Name, ExCast.zCStr(zip.Tag), zip.txtZipNo2));
                        i += 1;
                        _list.Add(new ListDisplayTabIndex(i, zip.btnZip.Name, ExCast.zCStr(zip.Tag), zip.btnZip));
                        i += 1;
                        _list.Add(new ListDisplayTabIndex(i, zip.txtAdress1.Name, ExCast.zCStr(zip.Tag), zip.txtAdress1));
                        i += 1;
                        _list.Add(new ListDisplayTabIndex(i, zip.txtAdress2.Name, ExCast.zCStr(zip.Tag), zip.txtAdress2));
                    }
                    if (ob is ComboBox || ob is ExDatePicker || ob is PasswordBox || ob is ExCheckBox || ob is ExDataGrid || ob is RadioButton)
                    {
                        ctl = (Control)ob;
                        if (ctl.IsTabStop == true)
                        {
                            _list.Add(new ListDisplayTabIndex(i, ctl.Name, ExCast.zCStr(ctl.Tag), ctl));
                        }
                    }
                    if (ob is Button)
                    {
                        ctl = (Control)ob;
                        if (ctl.Name != "btnZip")
                        {
                            _list.Add(new ListDisplayTabIndex(i, ctl.Name, ExCast.zCStr(ctl.Tag), ctl));
                        }
                    }
                }
                catch 
                { 
                }
                i += 1;
            }
            return _list;
        }

        public static System.Collections.IEnumerable FindVisualChildrenNoType(DependencyObject depObj)
        {
            Control ctl = null;
            int flg = 0;

            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null)
                    {
                        yield return child;
                    }
                    foreach (DependencyObject childOfChild in FindVisualChildrenNoType(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static List<ListControl> FindControlList(DependencyObject RootObj, string[] name)
        {
            List<ListControl> lst = new List<ListControl>();
            ListControl lstCtl = null;

            foreach (ExTextBox txt in FindVisualChildren<ExTextBox>(RootObj))
            {
                for (int i = 0; i < name.Length; i++)
                {
                    if (txt.Name == name[i])
                    {
                        lstCtl = new ListControl(txt, txt.Name);
                        lst.Add(lstCtl);
                    }
                }
            }

            foreach (ExDatePicker dap in FindVisualChildren<ExDatePicker>(RootObj))
            {
                for (int i = 0; i < name.Length; i++)
                {
                    if (dap.Name == name[i])
                    {
                        lstCtl = new ListControl(dap, dap.Name);
                        lst.Add(lstCtl);
                    }
                }
            }

            foreach (ComboBox cmb in FindVisualChildren<ComboBox>(RootObj))
            {
                for (int i = 0; i < name.Length; i++)
                {

                    if (cmb.Name == name[i])
                    {
                        lstCtl = new ListControl(cmb, cmb.Name);
                        lst.Add(lstCtl);
                    }
                }
            }

            return lst;
        }

        public static ExTextBox FindTextBox(DependencyObject RootObj, string name)
        {
            foreach (ExTextBox tb in FindVisualChildren<ExTextBox>(RootObj))
            {
                if (tb.Name == name)
                {
                    return tb;
                }
            }
            return null;
        }

        public static Grid FindGrid(DependencyObject RootObj, string name)
        {
            foreach (Grid dg in FindVisualChildren<Grid>(RootObj))
            {
                if (dg.Name == name)
                {
                    return dg;
                }
            }
            return null;
        }

        public static ExDatePicker FindDatePicker(DependencyObject RootObj, string name)
        {
            foreach (ExDatePicker tb in FindVisualChildren<ExDatePicker>(RootObj))
            {
                if (tb.Name == name)
                {
                    return tb;
                }
            }
            return null;
        }

        public static ComboBox FindComboBox(DependencyObject RootObj, string name)
        {
            foreach (ComboBox tb in FindVisualChildren<ComboBox>(RootObj))
            {
                if (tb.Name == name)
                {
                    return tb;
                }
            }
            return null;
        }

        public static ExDataGrid FindDataGrid(DependencyObject RootObj)
        {
            foreach (ExDataGrid dg in FindVisualChildren<ExDataGrid>(RootObj))
            {
                return dg;
            }
            return null;
        }

        public static ExUserControl FindUserControl(DependencyObject RootObj, string name)
        {
            foreach (ExUserControl utl in FindVisualChildren<ExUserControl>(RootObj))
            {
                if (utl.Name == name)
                {
                    return utl;
                }
            }
            return null;
        }

        public static ChildWindow FindChildWindowForTheme(DependencyObject RootObj, string tag_name)
        {
            foreach (ChildWindow win in FindVisualChildren<ChildWindow>(RootObj))
            {
                if (win.Tag.ToString() == tag_name)
                {
                    return win;
                }
            }
            return null;
        }

        public static void initDisplay(DependencyObject RootObj)
        {
            foreach (TextBox tb in FindVisualChildren<TextBox>(RootObj))
            {
                if (tb.IsReadOnly == true)
                {
                    tb.Background = new SolidColorBrush(Color.FromArgb(255, 206, 206, 206));
                    //try
                    //{
                    //    ExTextBox txt = (ExTextBox)tb;
                    //    txt.SetIme();
                    //}
                    //catch
                    //{ 
                    //}
                }
                tb.Text = "";
                try
                {
                    if (tb.IsTabStop == true && tb.IsEnabled == true)
                    {
                        ExTextBox txt = (ExTextBox)tb;
                        txt.SetIme();
                    }
                }
                catch
                {
                }
            }
        }

        public static void initDisplayNoReadOnly(DependencyObject RootObj)
        {
            foreach (TextBox tb in FindVisualChildren<TextBox>(RootObj))
            {
                if (tb.IsReadOnly == false)
                {
                    tb.Text = "";
                }
                try
                {
                    if (tb.IsTabStop == true && tb.IsEnabled == true)
                    {
                        ExTextBox txt = (ExTextBox)tb;
                        txt.SetIme();
                    }
                }
                catch
                {
                }
            }
        }

        public static void initDisplayIme(DependencyObject RootObj)
        {
            foreach (TextBox tb in FindVisualChildren<TextBox>(RootObj))
            {
                try
                {
                    if (tb.IsTabStop == true && tb.IsEnabled == true)
                    {
                        ExTextBox txt = (ExTextBox)tb;
                        txt.SetIme();
                    }
                }
                catch
                {
                }
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject 
        { 
            if (depObj != null) 
            { 
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) 
                { 
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i); 
                    if (child != null && child is T) 
                    { 
                        yield return (T)child; 
                    } 
                    foreach (T childOfChild in FindVisualChildren<T>(child)) 
                    { 
                        yield return childOfChild; 
                    } 
                } 
            } 
        }

        public static IEnumerable<T> FindVisualChildren2<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    //for entry in findChildren(depObj, DataGridColumnHeader):

                    //System.Diagnostics.Debug.WriteLine(child.ToString());
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in FindVisualChildren2<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static Control SearchControl(DependencyObject RootObj, string targetName, int cnt)
        {
            if (cnt == 0) return null;

            if (cnt == 1)
            {
                foreach (Control ctl1 in GetChildren<Control>(RootObj, true))
                {
                    if (ctl1.Name == targetName)
                    {
                        return ctl1;
                    }
                }      
            }
            else if (cnt == 2)
            {
                foreach (Control ctl1 in GetChildren<Control>(RootObj, true))
                {
                    if (ctl1.Name == targetName)
                    {
                        return ctl1;
                    }

                    foreach (Control ctl2 in GetChildren<Control>(ctl1, true))
                    {
                        if (ctl2.Name == targetName)
                        {
                            return ctl1;
                        }
                    }

                }
            }
            else if (cnt == 3)
            {
                foreach (Control ctl1 in GetChildren<Control>(RootObj, true))
                {
                    if (ctl1.Name == targetName)
                    {
                        return ctl1;
                    }

                    foreach (Control ctl2 in GetChildren<Control>(ctl1, true))
                    {
                        if (ctl2.Name == targetName)
                        {
                            return ctl2;
                        }

                        foreach (Control ctl3 in GetChildren<Control>(ctl2, true))
                        {
                            if (ctl3.Name == targetName)
                            {
                                return ctl3;
                            }
                        }
                    }

                }
            }
                
            
            return null;
        }

        public static Control SearchControlForTag(DependencyObject RootObj, string tag)
        {
            foreach (Control ctl in GetChildren<Control>(RootObj, true))
            {
                if (ExCast.zCStr(ctl.Tag).IndexOf(tag) != -1)
                {
                    return ctl;
                }
            }

            return null;
        }

        public static IEnumerable<T> GetChildren<T>(DependencyObject target, bool isFindAllChildren) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(target);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    DependencyObject o = VisualTreeHelper.GetChild(target, i);
                    if (o != null)
                    {
                        bool targetIsT = o is T;

                        if (targetIsT)
                        {
                            yield return o as T;
                        }

                        if (!targetIsT || isFindAllChildren)
                        {
                            foreach (T child in GetChildren<T>(o, isFindAllChildren))
                            {
                                yield return child;
                            }
                        }
                    }
                }
            }
        }

        public static void SetResource(DependencyObject RootObj)
        {
            foreach (ExUserControl utl in FindVisualChildren<ExUserControl>(RootObj))
            {
                if (utl.Tag != null)
                {
                    if (utl.Tag.ToString() == "Main" || utl.Tag.ToString() == "FunctionKeys")
                    {
                        utl.SetRecource();
                    }
                }
            }
        }

        public static DependencyObject FindPerentPage(DependencyObject depObj)
        {
            DependencyObject searchObj = depObj;
            DependencyObject Obj = null;
            if (depObj != null)
            {
                bool loop = true;
                while (loop)
                {
                    Obj = VisualTreeHelper.GetParent(searchObj);
                    if (Obj == null)
                    {
                        loop = false;
                    }
                    try
                    {
                        Obj = (ExPage)Obj;
                        loop = false;
                    }
                    catch
                    {
                        searchObj = Obj;
                    }
                }
            }
            return Obj;
        }

        public static DependencyObject FindPerentUserControl(DependencyObject depObj)
        {
            DependencyObject searchObj = depObj;
            DependencyObject Obj = null;
            if (depObj != null)
            {
                bool loop = true;
                while (loop)
                {
                    Obj = VisualTreeHelper.GetParent(searchObj);
                    if (Obj == null)
                    {
                        loop = false;
                    }
                    try
                    {
                        Obj = (ExUserControl)Obj;
                        loop = false;
                    }
                    catch
                    {
                        searchObj = Obj;
                    }
                }
            }
            return Obj;
        }

        public static DependencyObject FindPerentChildWindow(DependencyObject depObj)
        {
            DependencyObject searchObj = depObj;
            DependencyObject Obj = null;
            if (depObj != null)
            {
                bool loop = true;
                while (loop)
                {
                    Obj = VisualTreeHelper.GetParent(searchObj);
                    if (Obj == null)
                    {
                        loop = false;
                    }
                    try
                    {
                        Obj = (ExChildWindow)Obj;
                        loop = false;
                    }
                    catch
                    {
                        searchObj = Obj;
                    }
                }
            }
            return Obj;
        }

        public static DependencyObject FindPerentDataGrid(DependencyObject depObj)
        {
            DependencyObject searchObj = depObj;
            DependencyObject Obj = null;
            if (depObj != null)
            {
                bool loop = true;
                while (loop)
                {
                    Obj = VisualTreeHelper.GetParent(searchObj);
                    if (Obj == null)
                    {
                        loop = false;
                    }
                    try
                    {
                        Obj = (ExDataGrid)Obj;
                        loop = false;
                    }
                    catch
                    {
                        searchObj = Obj;
                    }
                }
            }
            return Obj;
        }

        public static DependencyObject FindPerentDataForm(DependencyObject depObj)
        {
            DependencyObject searchObj = depObj;
            DependencyObject Obj = null;
            if (depObj != null)
            {
                bool loop = true;
                while (loop)
                {
                    Obj = VisualTreeHelper.GetParent(searchObj);
                    if (Obj == null)
                    {
                        loop = false;
                    }
                    try
                    {
                        Obj = (ExDataForm)Obj;
                        loop = false;
                    }
                    catch
                    {
                        searchObj = Obj;
                    }
                }
            }
            return Obj;
        }

        public static bool CheckPerentIsPage(DependencyObject depObj)
        {
            DependencyObject obj = FindPerentPage(depObj);
            if (obj == null)
                return false;
            else
                return true;
        }

        public static void SetFunctionKeyEnabled(string strFunctionKeyName, bool enable, DependencyObject depObj)
        {
            DependencyObject obj = ExVisualTreeHelper.FindPerentChildWindow(depObj);
            if (obj == null)
            {
                obj = ExVisualTreeHelper.FindPerentPage(depObj);
                if (obj == null)
                {
                    return;
                }
            }

            Utl_FunctionKey utlKey = null;
            foreach (Utl_FunctionKey key in FindVisualChildren<Utl_FunctionKey>(obj))
            {
                utlKey = key;
                break;
            }
            if (utlKey == null)
            {
                return;
            }

            switch (strFunctionKeyName)
            {
                case "F1": utlKey.btnF1.IsEnabled = enable; break;
                case "F2": utlKey.btnF2.IsEnabled = enable; break;
                case "F3": utlKey.btnF3.IsEnabled = enable; break;
                case "F4": utlKey.btnF4.IsEnabled = enable; break;
                case "F5": utlKey.btnF5.IsEnabled = enable; break;
                case "F6": utlKey.btnF6.IsEnabled = enable; break;
                case "F7": utlKey.btnF7.IsEnabled = enable; break;
                case "F8": utlKey.btnF8.IsEnabled = enable; break;
                case "F9": utlKey.btnF9.IsEnabled = enable; break;
                case "F11": utlKey.btnF11.IsEnabled = enable; break;
                case "F12": utlKey.btnF12.IsEnabled = enable; break;
                default: break;
            }

            return;
        }

        public static void SetMode(Utl_FunctionKey.geFunctionKeyEnable mode, DependencyObject depObj)
        {
            DependencyObject obj = ExVisualTreeHelper.FindPerentChildWindow(depObj);
            if (obj == null)
            {
                obj = ExVisualTreeHelper.FindPerentPage(depObj);
                if (obj == null)
                {
                    return;
                }
                else
                {
                    Grid dg = ExVisualTreeHelper.FindGrid(obj, "GirdMenu");
                    if (dg != null)
                    {
                        if (dg.Visibility == Visibility.Visible)
                        {
                            return;
                        }
                    }
                }
            }

            Utl_Mode utlKey = null;
            foreach (Utl_Mode utlMode in FindVisualChildren<Utl_Mode>(obj))
            {
                utlMode.Mode = mode;
                break;
            }

            if (mode == Utl_FunctionKey.geFunctionKeyEnable.Sel)
            {
                SetEnabled(obj, false);
            }
            else
            {
                SetEnabled(obj, true);
            }

            return;
        }

        public static void SetEnabled(DependencyObject RootObj, bool flg)
        {
            foreach (DependencyObject ob in FindVisualChildrenNoType(RootObj))
            {
                Control ctl = null;
                try
                {
                    if (ob is ExTextBox || ob is ComboBox || ob is ExDatePicker || ob is PasswordBox || ob is ExCheckBox)
                    {
                        ctl = (Control)ob;
                        if (ctl.IsEnabled != flg) ctl.IsEnabled = flg;
                    }

                    if (ob is Button)
                    {
                        ctl = (Control)ob;

                        // ファンクションキーは非有効化しない
                        if (ctl.Name.IndexOf("btnF") == -1)
                        {
                            if (ctl.IsEnabled != flg) ctl.IsEnabled = flg;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static Utl_FunctionKey GetUtlFunctionKey(DependencyObject depObj)
        {
            bool search = false;

            int cnt = 1;
            while (cnt <= 10)
            {
                System.Threading.Thread.Sleep(cnt * 100);
                cnt += 1;

                search = false;

                DependencyObject obj = ExVisualTreeHelper.FindPerentChildWindow(depObj);
                if (obj == null)
                {
                    obj = ExVisualTreeHelper.FindPerentPage(depObj);
                    if (obj != null)
                    {
                        search = true;
                    }
                }
                else
                {
                    search = true;
                }

                if (search == true)
                {
                    Utl_FunctionKey utlKey = null;
                    foreach (Utl_FunctionKey key in FindVisualChildren<Utl_FunctionKey>(obj))
                    {
                        return key;
                    }
                }
            }

            return null;

        }

        public static ExUserControl GetMainUserControlForWindow(DependencyObject depObj)
        {
            DependencyObject obj = ExVisualTreeHelper.FindPerentChildWindow(depObj);
            if (obj == null)
            {
                return null;
            }

            foreach (ExUserControl utl in FindVisualChildren<ExUserControl>(obj))
            {
                if (utl.Tag.ToString() == "Main") return utl;
            }

            return null;
        }

        public static ExUserControl GetMainUserControlForPage(DependencyObject depObj)
        {
            DependencyObject obj = ExVisualTreeHelper.FindPerentPage(depObj);
            if (obj == null)
            {
                return null;
            }

            foreach (ExUserControl utl in FindVisualChildren<ExUserControl>(obj))
            {
                if (utl.Tag != null)
                {
                    if (utl.Tag.ToString() == "Main") return utl;
                }
                
            }

            return null;
        }

        #endregion

    }

    public class ListDisplayTabIndex
    {
        private int _index;
        public int index { set { this._index = value; } get { return this._index; } }
        private string _controlName;
        public string controlName { set { this._controlName = value; } get { return this._controlName; } }
        private string _tag;
        public string tag { set { this._tag = value; } get { return this._tag; } }
        private Control _ctl;
        public Control ctl { set { this._ctl = value; } get { return this._ctl; } }

        public ListDisplayTabIndex(int index, string controlName, string tag, Control ctl)
        {
            this.index = index;
            this.controlName = controlName;
            this.tag = tag;
            this.ctl = ctl;
        }
    }

    public class ListControl
    {
        private string _ctlName;
        public string ctlName { set { this._ctlName = value; } get { return this._ctlName; } }
        private Control _ctl;
        public Control ctl { set { this._ctl = value; } get { return this._ctl; } }

        public ListControl(Control ctl, string ctlNmae)
        {
            this.ctl = ctl;
            this.ctlName = ctlNmae;
        }
    }

}
