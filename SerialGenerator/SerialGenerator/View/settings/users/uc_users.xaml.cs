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

namespace SerialGenerator.View.settings.users
{
    /// <summary>
    /// Interaction logic for uc_users.xaml
    /// </summary>
     public partial class uc_users : UserControl
    {
        public uc_users()
        {
            InitializeComponent();
        }
        private static uc_users _instance;
        public static uc_users Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new uc_users();
                return _instance;
            }
        }

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
                if (MainWindow.lang.Equals("en"))
                {
                    //MainWindow.resourcemanager = new ResourceManager("POS.en_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    //MainWindow.resourcemanager = new ResourceManager("POS.ar_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }

                translate();
                #endregion
                if (HelpClass.isSupportPermision)
                {
                    //support
                    brdr_db.Visibility = Visibility.Visible;
                }
                else
                {
                    brdr_db.Visibility = Visibility.Collapsed;
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
        private void translate()
        {
            //addNewuser
//            addNewuserHint
//            Changepass
//ChangepassHint
//usersPermissions
//usersPermissionsHint
             txt_addUserInfo.Text = MainWindow.resourcemanager.GetString("addNewuser");
            txt_addUserHint.Text = MainWindow.resourcemanager.GetString("addNewuserHint");
            txt_passwordChangeInfo.Text = MainWindow.resourcemanager.GetString("Changepass");
            txt_passwordChangeHint.Text = MainWindow.resourcemanager.GetString("ChangepassHint");
            //txt_companyInfo.Text = MainWindow.resourcemanager.GetString("trComInfo");
            //txt_companyHint.Text = MainWindow.resourcemanager.GetString("trSettingHint");
            txt_dataBaseInfo.Text = MainWindow.resourcemanager.GetString("dbSetting");
            txt_dataBaseHint.Text = MainWindow.resourcemanager.GetString("dbSettingHint");

            //txt_permissionInfo.Text = MainWindow.resourcemanager.GetString("usersPermissions");
            //txt_permissionHint.Text = MainWindow.resourcemanager.GetString("usersPermissionsHint");
            //txt_comHint.Text = MainWindow.resourcemanager.GetString("trSettingHint");
        }


        private void Btn_addUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_user w = new wd_user();
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_passwordChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                
                Window.GetWindow(this).Opacity = 0.2;
                wd_changePassword w = new wd_changePassword();
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                 
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
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
        private void Btn_dataBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_DbSetting w = new wd_DbSetting();
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
                
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

    }
}
