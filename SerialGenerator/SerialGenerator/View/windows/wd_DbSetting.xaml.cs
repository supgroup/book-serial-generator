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
    public partial class wd_DbSetting : Window
    {

        public wd_DbSetting()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }


        BrushConverter bc = new BrushConverter();
 
        private void refreshWindow()
        {
            dbmodel = HelpClass.getConnectionString();
            tb_server.Text = dbmodel.servername;
            tb_db.Text = dbmodel.dbname;
        }
        DbClass dbmodel = new DbClass();

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private   void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);

                #region translate

                grid_main.FlowDirection = FlowDirection.LeftToRight;

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
            txt_title.Text= "اعدادات قاعدة البيانات"  ;  
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_server, MainWindow.resourcemanager.GetString("trReportPrinterName")+"...");//
            // //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_saleInvPaperSize, MainWindow.resourcemanager.GetString("trInvoicesPaperSize") + "...");
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_db, MainWindow.resourcemanager.GetString("trDocPaperSize") + "...");

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
                string msg ="";
                //msg = await Saverepname();
                //msg = await Savedocpapersize();
                //  refreshWindow();
                if(!string.IsNullOrEmpty(tb_server.Text)&& !string.IsNullOrEmpty(tb_db.Text))
                {

                    msg= HelpClass.AddNewConnectionString(tb_server.Text.Trim(), tb_db.Text.Trim());
                }
             
                
                // await MainWindow.getPrintersNames();
                if (msg == "1")
                {
                    refreshWindow();
                    //await FillCombo.loading_getDefaultSystemInfo();
                    Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    await Task.Delay(500);
                  
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
