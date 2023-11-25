﻿using SerialGenerator.Classes;
 
//using POS.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFTabTip;
using SerialGenerator.ApiClasses;
using System.Threading;
using SerialGenerator.View.sectionData;
//using SerialGenerator.View.sales;
using SerialGenerator.View.settings;
using System.Windows.Resources;
using SerialGenerator.View.windows;
 

namespace SerialGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ResourceManager resourcemanager;
        public static ResourceManager resourcemanagerreport;
        static public MainWindow mainWindow;
        public static string lang = "ar";
        public static string Reportlang = "ar";
        internal static Users userLogin = new Users();
        public static int userID=1;
        //public static CountryCode Region;
        internal static int? userLogInID;
        public static Boolean go_out = false;

        internal static string dateFormat;
        internal static string accuracy;
       // static public GroupObject groupObject = new GroupObject();
        //static public List<GroupObject> groupObjects = new List<GroupObject>();
        Users userModel = new Users();
        ImageBrush myBrush = new ImageBrush();
        public static string Currency;
        public static int CurrencyId;
        //public static string companyName;
        //public static string Email;
        //public static string Fax;
        //public static string Mobile;
        //public static string Address;
        //public static string Phone;
        //public static string logoImage;
       

        static SetValues valueModel = new SetValues();

        public static string print_on_save_sale;
        public static string email_on_save_sale;
   

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                mainWindow = this;
                windowFlowDirection();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        void windowFlowDirection()
        {
            lang = "ar";
            #region
            if (lang.Equals("en"))
            {
                resourcemanager = new ResourceManager("SerialGenerator.en_file", Assembly.GetExecutingAssembly());
                grid_mainGrid.FlowDirection = FlowDirection.LeftToRight;
            }
            else
            {
                resourcemanager = new ResourceManager("SerialGenerator.ar_file", Assembly.GetExecutingAssembly());
                grid_mainGrid.FlowDirection = FlowDirection.RightToLeft;
            }
            #endregion
        }

        private void translate()
        {
            txt_bookSales.Text = resourcemanager.GetString("activationcopy");
            txt_accounting.Text = resourcemanager.GetString("trUsers");
            //txt_reports.Text = resourcemanager.GetString("trReports");
            //txt_sectionData.Text = resourcemanager.GetString("trSectionData");
            //txt_settings.Text = resourcemanager.GetString("Settings");

            mi_changePassword.Header = resourcemanager.GetString("trChangePassword");
            BTN_logOut.Header = resourcemanager.GetString("trLogOut");

            //txt_notifications.Text = resourcemanager.GetString("trNotifications");
            //txt_noNoti.Text = resourcemanager.GetString("trNoNotifications");
            //btn_showAll.Content = resourcemanager.GetString("trShowAll");
        }

        public static List<string> menuList;

        #region loading
        List<keyValueBool> loadingList;
        private async Task getImg(string imagename, Ellipse img_userLogin)
        {
            try
            {
                if (string.IsNullOrEmpty(imagename))
                {
                   clearImg();
                }
                else
                {
                    //byte[] imageBuffer = await setVLogo.downloadImage(setVLogo.value); // read this as BLOB from your DB

                    var bitmapImage = new BitmapImage();
                    //   BitmapImage img= Bitma;

                    string dir =System.IO.Directory.GetCurrentDirectory();
                    string tmpPath = System.IO.Path.Combine(dir, Global.TMPUsersFolder);
                    tmpPath = System.IO.Path.Combine(tmpPath,imagename);
                    byte[] imageBuffer = System.IO.File.ReadAllBytes(tmpPath);
                    if (imageBuffer != null)
                    {
                        using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                        {
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = memoryStream;
                            bitmapImage.EndInit();
                        }
                    }

                  
                    img_userLogin.Fill = new ImageBrush(bitmapImage);
                 
                }
            }
            catch (Exception ex) { }

        }
        public string first, second;
        public void setMainPath()
        {
            #region first
            if (!string.IsNullOrWhiteSpace(first))
            {
                switch (first)
                {
                    case "bookSales":
                        txt_firstPath.Text = resourcemanager.GetString("activationcopy");
                        break;
                    case "accounting":
                        txt_firstPath.Text = resourcemanager.GetString("accounting");
                        break;
                    case "reports":
                        txt_firstPath.Text = resourcemanager.GetString("trReports");
                        break;
                    case "sectionData":
                        txt_firstPath.Text = resourcemanager.GetString("trSectionData");
                        break;
                    case "settings":
                        txt_firstPath.Text = resourcemanager.GetString("Settings");
                        break;


                    default:
                        txt_firstPath.Text = "";
                        break;
                }
            }
            #endregion

            #region second
            if (!string.IsNullOrWhiteSpace(second))
            {
                switch (second)
                {
                    //passengers office flights operations
                    case "passengers":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("passengers");
                        break;
                    case "office":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("trOffices");
                        break;
                    case "flights":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("flights");
                        break;
                    case "bookSystems":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("bookSystems");
                        break;
                    case "salesSyria":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("syr");
                        break;
                    case "salesSoto":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("soto");
                        break;
                    //report
                    case "paymentsSts":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("financeChange");
                        break;
                    case "bookSts":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("bookProfits");
                        break;
                    case "systemTransactions":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("paySysTrans");
                        break;
                    //setting
                    case "commissions":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("generalSettings");
                        break;
                    case "users":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("trUsers");
                        break;
                    case "printerSetting":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("printSetting");
                        break;
                    //Account
                    case "payment":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("trPayments");
                        break;
                    case "received":
                        txt_secondPath.Text = "> " + resourcemanager.GetString("trReceived");
                        break;
                    

                    default:
                        txt_secondPath.Text = "";
                        break;
                }
            }
            else
            {
                txt_secondPath.Text = "";
            }
            #endregion
        }
        async Task loading_getUserPersonalInfo()
        {
            #region user personal info
            txt_userName.Text = userLogin.name;
            string job = "";
            switch(userLogin.type)
            {
                case "us":    job = "Employee"; break;
                case "ad":    job = "Admin";    break;
              //  case "ag":    job = "Agent";    break;
            }
            txt_userJob.Text = job;
            try
            {
               //await getImg(userLogin.image, img_userLogin);
                //if (!string.IsNullOrEmpty(userLogin.image))
                //{
                //    byte[] imageBuffer = await userModel.downloadImage(userLogin.image); // read this as BLOB from your DB

                //    var bitmapImage = new BitmapImage();

                //    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                //    {
                //        bitmapImage.BeginInit();
                //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                //        bitmapImage.StreamSource = memoryStream;
                //        bitmapImage.EndInit();
                //    }

                //    img_userLogin.Fill = new ImageBrush(bitmapImage);
                //}
                //else
                //{
                 //   clearImg();
                //}
            }
            catch
            {
                clearImg();
            }
            //foreach (var item in loadingList)
            //{
            //    if (item.key.Equals("loading_getUserPersonalInfo"))
            //    {
            //        item.value = true;
            //        break;
            //    }
            //}
            #endregion
        }
       
        #endregion

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
              
                if (sender != null)
                    HelpClass.StartAwait(grid_mainGrid);
                //windowFlowDirection();
                menuList = new List<string> { "bookSales", "accounting", "reports",
                   "sectionData","settings"};
                //menuList = new List<string> { "applications", "sales", "reports",
                //   "sectionData","settings"};
                
                translate();
                if (HelpClass.isSupportPermision)
                {
                    //support
                    Btn_settings_Click(btn_settings, null);
                }
                else
                {
                    #region loading
                    //loadingList = new List<keyValueBool>();
                    //bool isDone = true;
                    //loadingList.Add(new keyValueBool { key = "loading_getGroupObjects", value = false });
                    //loadingList.Add(new keyValueBool { key = "loading_getUserPersonalInfo", value = false });


                    await FillCombo.loading_getDefaultSystemInfo();
                    await FillCombo.Getprintparameter();
                    //loading_getGroupObjects();
                    // await FillCombo.RefreshCountry();
                    //  FillCombo.fillRegion();
                    await loading_getUserPersonalInfo();


                    #endregion

                    //if (MainWindow.userLogin.type == "ag")
                    //{
                    //    btn_applications.Visibility = Visibility.Collapsed;
                    //    btn_sectionData.Visibility = Visibility.Collapsed;
                    //    btn_settings.Visibility = Visibility.Collapsed;

                    //    Btn_sales_Click(btn_sales , null);
                    //}
                    //else
                    //{
                    Btn_bookSales_Click(btn_bookSales, null);
                   // Btn_sectionData_Click(btn_sectionData, null);
                    //}

                }
                if (sender != null)
                    HelpClass.EndAwait(grid_mainGrid);
            }
            catch (Exception ex)
            {
                if (sender != null)
                    HelpClass.EndAwait(grid_mainGrid);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        /*
        void ColorButtonRefresh(string str)
        {
            foreach (Button button in FindControls.FindVisualChildren<Button>(this))
            {
                if (button.Tag != null)
                {
                    foreach (var item in menuList)
                    {
                        if (item == button.Tag.ToString())
                        {
                            if (item == str)
                                button.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                            else
                                button.Background = null;

                        }
                    }
                }
            }
        }
        */
        void ColorBorderRefresh(string str)
        {
            foreach (Border border in FindControls.FindVisualChildren<Border>(this))
            {
                if (border.Tag != null)
                {
                    foreach (var item in menuList)
                    {
                        if (item == border.Tag.ToString())
                        {
                            if (item == str)
                                border.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                            else
                                border.Background = null;

                        }
                    }
                }
            }
        }
        void colorTextRefresh(string str)
        {
            foreach (TextBlock textBlock in FindControls.FindVisualChildren<TextBlock>(this))
            {
                if (textBlock.Tag != null)
                {
                    foreach (var item in menuList)
                    {
                        if (item == textBlock.Tag.ToString())
                        {
                            if (item == str)
                            textBlock.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                                else
                            textBlock.Foreground = Application.Current.Resources["LightGrey"] as SolidColorBrush;

                        }
                    }
                }
            }
        }
        void ColorIconRefresh(string str)
        {
            foreach (Path path in FindControls.FindVisualChildren<Path>(this))
            {
                if (path.Tag != null)
                {
                    foreach (var item in menuList)
                    {
                        if (item == path.Tag.ToString())
                        {
                            if (item == str)
                                path.Fill = Application.Current.Resources["White"] as SolidColorBrush;
                            else
                                path.Fill = Application.Current.Resources["LightGrey"] as SolidColorBrush;

                        }
                    }
                }
            }
        }
        /*
        void openVisible(string str)
        {
            foreach (Rectangle rectangle in FindControls.FindVisualChildren<Rectangle>(this))
            {
                if (rectangle.Tag != null)
                {
                    foreach (var item in menuList)
                    {
                        if (item == rectangle.Tag.ToString())
                        {
                            if (item == str)
                                rectangle.Visibility = Visibility.Visible;
                            else
                                rectangle.Visibility = Visibility.Collapsed;

                        }
                    }
                }
            }
        }
       */
        private void BTN_Close_Click(object sender, RoutedEventArgs e)
        {//close
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_mainGrid);
                Application.Current.Shutdown();

                if (sender != null)
                    HelpClass.EndAwait(grid_mainGrid);
            }
            catch (Exception ex)
            {
                if (sender != null)
                    HelpClass.EndAwait(grid_mainGrid);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void BTN_Minimize_Click(object sender, RoutedEventArgs e)
        {//minimize
            try
            {
                this.WindowState = System.Windows.WindowState.Minimized;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        //private void Btn_applications_Click(object sender, RoutedEventArgs e)
        //{//application
        //    try
        //    {
        //        Button button = sender as Button;
        //        colorTextRefreash(button.Tag.ToString());
        //        ColorIconRefreash(button.Tag.ToString());
        //        openVisible(button.Tag.ToString());
        //        grid_main.Children.Clear();
        //        grid_main.Children.Add(uc_applications.Instance);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        private void Btn_bookSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                colorTextRefresh(button.Tag.ToString());
                ColorIconRefresh(button.Tag.ToString());
                //ColorButtonRefresh(button.Tag.ToString());
                ColorBorderRefresh(button.Tag.ToString());
                first = button.Tag.ToString();
                second = "";
                setMainPath();
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_office.Instance);
                //grid_main.Children.Add(uc_bookSales.Instance);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        
        private void Btn_accounting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                colorTextRefresh(button.Tag.ToString());
                ColorIconRefresh(button.Tag.ToString());
                //ColorButtonRefresh(button.Tag.ToString());
                ColorBorderRefresh(button.Tag.ToString());
                first = button.Tag.ToString();
                second = "";
                setMainPath();
                grid_main.Children.Clear();
                //grid_main.Children.Add(uc_accounting.Instance);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_sectionData_Click(object sender, RoutedEventArgs e)
        {//sectiondata
            try
            {
                Button button = sender as Button;
                colorTextRefresh(button.Tag.ToString());
                ColorIconRefresh(button.Tag.ToString());
                //ColorButtonRefresh(button.Tag.ToString());
                ColorBorderRefresh(button.Tag.ToString());
                first = button.Tag.ToString();
                second = "";
                setMainPath();
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_sectionData.Instance);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_reports_Click(object sender, RoutedEventArgs e)
        {//reports
            try
            {
                Button button = sender as Button;
                colorTextRefresh(button.Tag.ToString());
                ColorIconRefresh(button.Tag.ToString());
                //ColorButtonRefresh(button.Tag.ToString());
                ColorBorderRefresh(button.Tag.ToString());
                first = button.Tag.ToString();
                second = "";
                setMainPath();
                grid_main.Children.Clear();
                //grid_main.Children.Add(uc_reports.Instance);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                colorTextRefresh(button.Tag.ToString());
                ColorIconRefresh(button.Tag.ToString());
                //ColorButtonRefresh(button.Tag.ToString());
                ColorBorderRefresh(button.Tag.ToString());
                first = button.Tag.ToString();
                second = "";
                setMainPath();
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_settings.Instance);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void clearImg()
        {
            Uri resourceUri = new Uri("pic/no-image-icon-90x90.png", UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            myBrush.ImageSource = temp;
            img_userLogin.Fill = myBrush;

        }

        private async void BTN_logOut_Click(object sender, RoutedEventArgs e)
        {//log out
            try
            {
                await close();

                Application.Current.Shutdown();
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        async Task close()
        {
            //log out
            userLogin.isOnline = 0;
            await userModel.Save(userLogin);
            //close
            wd_logIn logIn = new wd_logIn();
            logIn.Show();
            this.Close();

        }

        private async void Mi_changePassword_Click(object sender, RoutedEventArgs e)
        {//change password
            try
            {
                Window.GetWindow(this).Opacity = 0.2;
                wd_changePassword w = new wd_changePassword();
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;

                userLogin = await userModel.GetByID(userLogin.userId);

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

     
    }
}
