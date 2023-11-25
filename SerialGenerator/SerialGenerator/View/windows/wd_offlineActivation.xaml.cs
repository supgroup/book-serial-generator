using BookAccountApp.ApiClasses;
using BookAccountApp.Classes;
using Microsoft.Win32;
using netoaster;
using Newtonsoft.Json;
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

namespace BookAccountApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_offlineActivation.xaml
    /// </summary>
    public partial class wd_offlineActivation : Window
    {
        public wd_offlineActivation()
        {
            InitializeComponent();
        }

        public static List<string> requiredControlList;
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        public PackageUser packageUser = new PackageUser();
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {//mouse down
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
            }
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
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {
                HelpClass.StartAwait(grid_main);

                requiredControlList = new List<string> { "type" };

                #region translate
                if (MainWindow.lang.Equals("en"))
                {
                    MainWindow.resourcemanager = new ResourceManager("BookAccountApp.en_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    MainWindow.resourcemanager = new ResourceManager("BookAccountApp.ar_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translat();
                #endregion

                PackageUser pu = packageUser;

                FillCombo.fillOfflineActivation(cb_type);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void translat()
        {
            txt_title.Text = MainWindow.resourcemanager.GetString("trOfflineActivation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_type, MainWindow.resourcemanager.GetString("trOfflineActivation")+"...");
            btn_download.Content = MainWindow.resourcemanager.GetString("trDownload");
            btn_upload.Content = MainWindow.resourcemanager.GetString("trUpload");
        }

        private void _validate()
        {
            try
            {
                HelpClass.validateWindow(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
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

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {//unload
            GC.Collect();
        }

        private void Cb_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_type.SelectedValue.ToString() == "rn")
                {
                    //try
                    //{
                    //grid_btns.ColumnDefinitions.RemoveAt(1);
                    //}
                    //catch
                    //{ }
                    //btn_upload.Visibility = Visibility.Collapsed;
                    col_upload.Width = new GridLength(0);

                }
                else
                {
                    //try
                    //{
                    //    ColumnDefinition cd = new ColumnDefinition();
                    //    cd.Width = new GridLength(1, GridUnitType.Star);
                    //    grid_btns.ColumnDefinitions.Add(cd);
                    //}
                    //catch
                    //{ }
                    //btn_upload.Visibility = Visibility.Visible;
                    col_upload.Width = col_download.Width; ;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
           
        }

        private void Tb_validateEmptyLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                _validate();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

      
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {//close
            this.Close();
        }

        private async void Btn_download_Click(object sender, RoutedEventArgs e)
        {//download
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.validateWindow(requiredControlList, this);
                if (cb_type.SelectedIndex != -1)
                {
                    //string activeState = "rn";//rn OR up  from buton
                    string activeState = cb_type.SelectedValue.ToString();

                    PackageUser pumodel = new PackageUser();
                    ReportCls rc = new ReportCls();
                    SendDetail sd = new SendDetail();
                    sd = await pumodel.ActivateServerOffline(packageUser.packageUserId, activeState);
                    packagesSend packtemp = new packagesSend();
                    if (sd.packageSend.result > 0)
                    {
                        //encode
                        string myContent = JsonConvert.SerializeObject(sd);

                        saveFileDialog.Filter = "File|*.ac;";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string DestPath = saveFileDialog.FileName;

                            bool res = false;

                            res = rc.encodestring(myContent, DestPath);
                            // rc.DelFile(pdfpath);
                            //  rc.decodefile(DestPath,@"D:\stringlist.txt");
                            if (res)
                            {
                                //done
                                Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                            }
                            else
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            }
                        }
                    }
                    else
                    {
                        //error
                        Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }

                    HelpClass.EndAwait(grid_main);
                }
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
            

        }

        OpenFileDialog openFileDialog = new OpenFileDialog();
        string activeState = "";
        private async void Btn_upload_Click(object sender, RoutedEventArgs e)
        {//upload
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.validateWindow(requiredControlList, this);
                if (cb_type.SelectedIndex != -1)
                {
                    PackageUser pumodel = new PackageUser();
                    //activeState = "up";
                    activeState = cb_type.SelectedValue.ToString();

                    if (activeState == "up")
                    {
                        string filepath = "";
                        openFileDialog.Filter = "INC|*.ac; ";

                        if (openFileDialog.ShowDialog() == true)
                        {
                            filepath = openFileDialog.FileName;

                            // bool resr = ReportCls.decodefile(filepath, @"D:\stringlist.txt");//comment
                            SendDetail dc = new SendDetail();
                            string objectstr = "";

                            objectstr = ReportCls.decodetoString(filepath);

                            dc = JsonConvert.DeserializeObject<SendDetail>(objectstr, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                            if (dc.packageSend.packageUserId== packageUser.packageUserId)
                            {
                                int res = await pumodel.updatecustomerdata(dc, activeState);

                             //   MessageBox.Show(res.ToString());
                                if (res > 0)
                                {
                                    if (res == 1)
                                    {
                                        // update done
                                        Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trRestoreDoneSuccessfuly"), animation: ToasterAnimation.FadeIn);

                                    }
                                    //else if (res == 2)
                                    //{
                                    //    //no update
                                    //    Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trRestoreNotComplete"), animation: ToasterAnimation.FadeIn);
                                    //}
                                }
                                else
                                {
                                    // error
                                    Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trRestoreNotComplete"), animation: ToasterAnimation.FadeIn);

                                }
                            }
                            else{

                                // The File dosn't belong to this Package 
                                Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trRestoreNotComplete")+ "The File dosn't belong to this Package", animation: ToasterAnimation.FadeIn);

                            }

                        }
                        else
                        {
                            // error
                            Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trRestoreNotComplete"), animation: ToasterAnimation.FadeIn);

                        }
                    }
                   



                }
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
