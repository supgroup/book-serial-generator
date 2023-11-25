using SerialGenerator.Classes;
 
using SerialGenerator.View.settings.commissions;
using SerialGenerator.View.settings.printerSetting;
using SerialGenerator.View.settings.users;
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

namespace SerialGenerator.View.settings
{
    /// <summary>
    /// Interaction logic for uc_settings.xaml
    /// </summary>
    public partial class uc_settings : UserControl
    {
        private static uc_settings _instance;
        public static uc_settings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new uc_settings();
                return _instance;
            }
        }
        public uc_settings()
        {
            InitializeComponent();
        }
        public static List<string> menuList;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_mainGrid);

                menuList = new List<string> { "commissions", "users", "printerSetting", };

                #region translate
                //if (MainWindow.lang.Equals("en"))
                //{
                //    MainWindow.resourcemanager = new ResourceManager("SerialGenerator.en_file", Assembly.GetExecutingAssembly());
                //    grid_main.FlowDirection = FlowDirection.LeftToRight;
                //}
                //else
                //{
                //    MainWindow.resourcemanager = new ResourceManager("SerialGenerator.ar_file", Assembly.GetExecutingAssembly());
                //    grid_main.FlowDirection = FlowDirection.RightToLeft;
                //}
                translate();
                #endregion
                if (HelpClass.isSupportPermision)
                {
                    //support
                    Btn_users_Click(btn_users, null);
                }
                else
                {
                    Btn_commissions_Click(btn_commissions, null);
                }
                HelpClass.EndAwait(grid_mainGrid);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_mainGrid);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void translate()
        {
            //financeSetting
            btn_commissions.Content = MainWindow.resourcemanager.GetString("generalSettings");
            btn_users.Content = MainWindow.resourcemanager.GetString("trUsers");
            btn_printerSetting.Content = MainWindow.resourcemanager.GetString("printSetting");

            //btn_salesSoto.Content = MainWindow.resourcemanager.GetString("soto");
            //btn_office.Content = MainWindow.resourcemanager.GetString("trOffices");
            //btn_passengers.Content = MainWindow.resourcemanager.GetString("passengers");
            //btn_flights.Content = MainWindow.resourcemanager.GetString("flights");
            //btn_operations.Content = MainWindow.resourcemanager.GetString("operations");
            //btn_customers.Content = MainWindow.resourcemanager.GetString("trCustomers");
        }

        void colorButtonRefreash(string str)
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
                                button.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                        }
                    }
                }
            }
        }




        private void Btn_commissions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Button button = sender as Button;
                colorButtonRefreash(button.Tag.ToString());
                MainWindow.mainWindow.second = button.Tag.ToString();
                MainWindow.mainWindow.setMainPath();
                grid_main.Children.Clear();
                uc_commissions ucsy = new uc_commissions();
                grid_main.Children.Add(ucsy);
                
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
       
        
        private void Btn_users_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Button button = sender as Button;
                colorButtonRefreash(button.Tag.ToString());
                MainWindow.mainWindow.second = button.Tag.ToString();
                MainWindow.mainWindow.setMainPath();
                grid_main.Children.Clear();
                uc_users ucsy = new uc_users();
                grid_main.Children.Add(ucsy);
                
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_printerSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Button button = sender as Button;
                colorButtonRefreash(button.Tag.ToString());
                MainWindow.mainWindow.second = button.Tag.ToString();
                MainWindow.mainWindow.setMainPath();
                grid_main.Children.Clear();
                uc_printerSetting ucsy = new uc_printerSetting();
                grid_main.Children.Add(ucsy);
                
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // Collect all generations of memory.
            GC.Collect();
        }


    }
}
