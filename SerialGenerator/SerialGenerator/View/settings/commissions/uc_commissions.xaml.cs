using SerialGenerator.Classes;
using SerialGenerator.View.windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using netoaster;
namespace SerialGenerator.View.settings.commissions
{
    /// <summary>
    /// Interaction logic for uc_commissions.xaml
    /// </summary>
    public partial class uc_commissions : UserControl
    {
        public uc_commissions()
        {
            InitializeComponent();
        }
        private static uc_commissions _instance;
        public static uc_commissions Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new uc_commissions();
                return _instance;
            }
        }
         SetValues accuracy = new SetValues();
        SettingCls set = new SettingCls();
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // Collect all generations of memory.
            GC.Collect();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);
                //MainWindow.mainWindow.initializationMainTrack(this.Tag.ToString(), 1);

                #region translate
                //if (MainWindow.lang.Equals("en"))
                //{
                //    //MainWindow.resourcemanager = new ResourceManager("POS.en_file", Assembly.GetExecutingAssembly());
                //    grid_main.FlowDirection = FlowDirection.LeftToRight;
                //}
                //else
                //{
                    //MainWindow.resourcemanager = new ResourceManager("POS.ar_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                //}

                translate();
                FillCombo.fillAccuracy(cb_accuracy);
                cb_accuracy.SelectedValue = MainWindow.accuracy;
                getaccuracySetting();
                #endregion

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
            //trTax exchangePrice
           //  txt_taxHint.Text = MainWindow.resourcemanager.GetString("syrSoto");          
             txt_priceExchangeInfo.Text = MainWindow.resourcemanager.GetString("exchangePrice");
            txt_companyInfo.Text = MainWindow.resourcemanager.GetString("trComInfo");
            txt_companyHint.Text = MainWindow.resourcemanager.GetString("trSettingHint");
            txt_acc.Text= MainWindow.resourcemanager.GetString("trAccuracy");
            
            //txt_priceExchangeHint.Text = MainWindow.resourcemanager.GetString("syrSoto");
        }


        //private void Btn_tax_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Window.GetWindow(this).Opacity = 0.2;
        //        wd_rateSyrSoto w = new wd_rateSyrSoto();
        //        w.ShowDialog();
        //        Window.GetWindow(this).Opacity = 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        private void Btn_priceExchange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window.GetWindow(this).Opacity = 0.2;
                //wd_rateSyrSoto w = new wd_rateSyrSoto();
                //w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_officeCommissions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window.GetWindow(this).Opacity = 0.2;
                //wd_rateSyrSoto w = new wd_rateSyrSoto();
                //w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_company_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                //wd_companyInfo w = new wd_companyInfo();
                //w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

      

        private async void Btn_saveAccuracy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);

                #region validate
                if (!cb_accuracy.Text.Equals(""))
                {
                    if (cb_accuracy.SelectedValue.ToString() != MainWindow.accuracy)
                    {
                        accuracy.value = cb_accuracy.SelectedValue.ToString();
                        //accuracy.isSystem = 1;
                        //accuracy.isDefault = 1;
                        //  accuracy.settingId = nameId;
                        // string sName = await valueModel.Save(setVName);
                        decimal res = await accuracy.Save(accuracy);
                        if (res > 0)
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                            await Task.Delay(500);


                            await FillCombo.loading_getDefaultSystemInfo();
                        }
                        else
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                        }
                    }
                    else
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);

                    }

                }





                    //  save logo
                    // image
                    //  string sLogo = "";



                    #endregion
                    
                    

               
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
        private void getaccuracySetting()
        {
            #region  get accuracy

            //get company fax
            set = FillCombo.settingsCls.Where(s => s.name == "accuracy").FirstOrDefault<SettingCls>();
            var accuId = set.settingId;
            accuracy = FillCombo.settingsValues.Where(i => i.settingId == accuId).FirstOrDefault();
            //if (accuracy != null)
            //{

            //    MainWindow.accuracy = accuracy.value;
            //}

            #endregion
        }
    }
   
}
