using SerialGenerator.Classes;
using SerialGenerator.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Drawing.Printing;
using netoaster;

namespace SerialGenerator.View.windows
{
    /// <summary>
    /// Interaction logic for wd_reportPrinterSetting.xaml
    /// </summary>
    public partial class wd_reportPrinterSetting : Window
    {

        public wd_reportPrinterSetting()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }


        BrushConverter bc = new BrushConverter();

        // printer
        //List<Papersize> papersizeList = new List<Papersize>();
        //List<Printers> printersList = new List<Printers>();
        //Printers repprinterRow = new Printers();
        //Printers salprinterRow = new Printers();
        //PosSetting possettingModel = new PosSetting();

      

        //Printers printerModel = new Printers();
        //Papersize papersizeModel = new Papersize();







      
        /*
        public void fillcb_saleInvPaperSize()
        {

            cb_saleInvPaperSize.ItemsSource = papersizeList.Where(x => x.printfor.Contains("sal"));
            // var paperList = papersizeList.Where(x => x.printfor.Contains("sal"));
            cb_saleInvPaperSize.DisplayMemberPath = "paperSize1";
            cb_saleInvPaperSize.SelectedValuePath = "sizeId";
            if (salepapersizeId > 0)
            {
                cb_saleInvPaperSize.SelectedValue = salepapersizeId;
            }


        }
        */
        private void refreshWindow()
        {

            // printersList = getsystemPrinters();
            //await GetposSetting();
            //fillcb_salname();
            FillCombo.fillcb_repname(cb_repname);
            FillCombo.fillcb_docpapersize(cb_docpapersize);


        }
        private async Task<decimal> Saverepname()
        {

                SettingCls set = new SettingCls();
        SetValues setV = new SetValues();
        set = FillCombo.settingsCls.Where(s => s.name == "rep_printer_name").FirstOrDefault();
               int nameId = set.settingId;
        setV = FillCombo.settingsValues.Where(i => i.settingId == nameId).FirstOrDefault();
        setV.value = cb_repname.Text.ToString();
                decimal res = await setV.Save(setV);
            if (res>0)
            {
                FillCombo.rep_printer_name = setV.value;
            }
            return res;
    }
        private async Task<decimal> Savedocpapersize()
        {

            SettingCls set = new SettingCls();
            SetValues setV = new SetValues();
            set = FillCombo.settingsCls.Where(s => s.name == "docPapersize").FirstOrDefault();
            int nameId = set.settingId;
            setV = FillCombo.settingsValues.Where(i => i.settingId == nameId).FirstOrDefault();
            setV.value = cb_docpapersize.SelectedValue.ToString();
            decimal res = await setV.Save(setV);
            if (res > 0)
            {
                FillCombo.docPapersize = setV.value;
            }
            return res;
        }
        
    
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);

                #region translate

                grid_main.FlowDirection = FlowDirection.RightToLeft;

                translate();
                #endregion

                  refreshWindow();

                if (sender != null)
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void translate()
        {
            txt_title.Text = MainWindow.resourcemanager.GetString("thePrint");  
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_repname, MainWindow.resourcemanager.GetString("trReportPrinterName")+"...");//
             //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_saleInvPaperSize, MainWindow.resourcemanager.GetString("trInvoicesPaperSize") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_docpapersize, MainWindow.resourcemanager.GetString("trDocPaperSize") + "...");

            btn_save.Content = MainWindow.resourcemanager.GetString("trSave");
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    //Btn_save_Click(null, null);
                }
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }

        List<SettingCls> set = new List<SettingCls>();

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch //(Exception ex)
            {
                //HelpClass.ExceptionMessage(ex, this);
            }
        }



        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);
                decimal msg =0;
                msg = await Saverepname();
                msg = await Savedocpapersize();
                //  refreshWindow();

                // await MainWindow.getPrintersNames();
                if (msg > 0)
                {
                    await FillCombo.loading_getDefaultSystemInfo();
                    Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    await Task.Delay(500);
                    this.Close();
                }
                else
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                }

                if (sender != null)
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
    }
}
