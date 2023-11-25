using SerialGenerator.Classes;
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

namespace SerialGenerator.View.sectionData
{
    /// <summary>
    /// Interaction logic for ud_HelpClass.xaml
    /// </summary>
    public partial class uc_sectionData : UserControl
    {
        private static uc_sectionData _instance;
        public static uc_sectionData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new uc_sectionData();
                return _instance;
            }
        }
        public uc_sectionData()
        {
            InitializeComponent();
        }
        public static List<string> menuList;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_mainGrid);

                menuList = new List<string> { "passengers", "office", "flights", "operations", };

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

                Btn_passengers_Click(btn_passengers, null);

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
            //btn_users.Content = MainWindow.resourcemanager.GetString("trUsers");
            btn_office.Content = MainWindow.resourcemanager.GetString("trOffices");
            btn_passengers.Content = MainWindow.resourcemanager.GetString("passengers");
            btn_flights.Content = MainWindow.resourcemanager.GetString("flights");
            btn_operations.Content = MainWindow.resourcemanager.GetString("bookSystems");//operations
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
       
        //private void Btn_users_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Button button = sender as Button;
        //        colorButtonRefreash(button.Tag.ToString());
        //        grid_main.Children.Clear();
        //        grid_main.Children.Add(uc_users.Instance);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}

        private void Btn_agents_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Button button = sender as Button;
            //    colorButtonRefreash(button.Tag.ToString());
            //    grid_main.Children.Clear();
            //    grid_main.Children.Add(uc_office.Instance);
            //}
            //catch (Exception ex)
            //{
            //    HelpClass.ExceptionMessage(ex, this);
            //}
        }

        private void Btn_customers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Button button = sender as Button;
                //colorButtonRefreash(button.Tag.ToString());
                //grid_main.Children.Clear();
                //grid_main.Children.Add(uc_customers.Instance);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_passengers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                colorButtonRefreash(button.Tag.ToString());
                MainWindow.mainWindow.second = button.Tag.ToString();
                MainWindow.mainWindow.setMainPath();
                grid_main.Children.Clear();
                //grid_main.Children.Add(uc_passengers.Instance);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_office_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                colorButtonRefreash(button.Tag.ToString());
                MainWindow.mainWindow.second = button.Tag.ToString();
                MainWindow.mainWindow.setMainPath();
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_office.Instance);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_flights_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                colorButtonRefreash(button.Tag.ToString());
                MainWindow.mainWindow.second = button.Tag.ToString();
                MainWindow.mainWindow.setMainPath();
                grid_main.Children.Clear();
                //uc_flights ucf = new uc_flights();
                //grid_main.Children.Add(ucf);
                //grid_main.Children.Add(uc_flights.Instance);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_operations_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                colorButtonRefreash(button.Tag.ToString());
                MainWindow.mainWindow.second = button.Tag.ToString();
                MainWindow.mainWindow.setMainPath();
                grid_main.Children.Clear();
                //uc_operations ucop = new uc_operations();
                //grid_main.Children.Add(uc_operations.Instance);
                //grid_main.Children.Add(ucop);
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