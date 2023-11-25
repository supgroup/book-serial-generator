using SerialGenerator.ApiClasses;
using SerialGenerator.Classes;
using SerialGenerator.View.windows;
using Microsoft.Win32;
using netoaster;
using POS.View.windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Reporting.WinForms;


namespace SerialGenerator.View.sectionData
{
    /// <summary>
    /// Interaction logic for uc_office.xaml
    /// </summary>
    public partial class uc_user : UserControl
    {
        public uc_user()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private static uc_user _instance;
        public static uc_user Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new uc_user();
                return _instance;
            }
        }

        Users office = new Users();
        IEnumerable<Users> officesQuery;
        IEnumerable<Users> offices;
        bool tgl_officestate = true;
        bool first = true;
        string searchText = "";
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        public static List<string> requiredControlList;
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                requiredControlList = new List<string> { "name", "userName" };

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

                //await FillCombo.fillCountries(cb_areaMobile);
                //await FillCombo.fillCountries(cb_areaPhone);
                //await FillCombo.fillCountries(cb_areaFax);
                //await FillCombo.fillCountriesNames(cb_country);
                //FillCombo.fillAgentLevel(cb_custlevel);

                Keyboard.Focus(tb_name);

                await RefreshOfficesList();
                await Search();
                Clear();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void translate()
        {

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, MainWindow.resourcemanager.GetString("trSearchHint"));
            //txt_baseInformation.Text = MainWindow.resourcemanager.GetString("trBaseInformation");
            txt_active.Text = MainWindow.resourcemanager.GetString("trActive");
            txt_title.Text = MainWindow.resourcemanager.GetString("officeInfo");
            txt_exportDocsButton.Text = MainWindow.resourcemanager.GetString("docExport");
            txt_uploadDocsButton.Text = MainWindow.resourcemanager.GetString("docUpload");
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_custCode, MainWindow.resourcemanager.GetString("trCodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, MainWindow.resourcemanager.GetString("officeNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_joinDate, MainWindow.resourcemanager.GetString("joinDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_mobile, MainWindow.resourcemanager.GetString("mobileNumHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_userName, MainWindow.resourcemanager.GetString("trUserNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_passwordSY, MainWindow.resourcemanager.GetString("passwordSyrHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_PasswordSoto, MainWindow.resourcemanager.GetString("passwordSotoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, MainWindow.resourcemanager.GetString("trNoteHint"));
       
            //docUpload docExport
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_lastName, MainWindow.resourcemanager.GetString("trLastNameHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_father, MainWindow.resourcemanager.GetString("fatherHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_mother, MainWindow.resourcemanager.GetString("motherHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, MainWindow.resourcemanager.GetString("trNoteHint"));

            //txt_contactInformation.Text = MainWindow.resourcemanager.GetString("trContactInformation");

            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_custlevel, MainWindow.resourcemanager.GetString("trLevelHint"));
            //txt_contactInformation.Text = MainWindow.resourcemanager.GetString("trContactInformation");
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, MainWindow.resourcemanager.GetString("trNoteHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_address, MainWindow.resourcemanager.GetString("trAdressHint"));
            /*
            father  
fatherHint  	
mother  
motherHint  
*/
            //   offices
            
            dg_office.Columns[0].Header = MainWindow.resourcemanager.GetString("trNo.");
            dg_office.Columns[1].Header = MainWindow.resourcemanager.GetString("officeName");
            dg_office.Columns[2].Header = MainWindow.resourcemanager.GetString("joinDate");
            dg_office.Columns[3].Header = MainWindow.resourcemanager.GetString("mobileNum");
            dg_office.Columns[4].Header = MainWindow.resourcemanager.GetString("trUserName");
            dg_office.Columns[5].Header = MainWindow.resourcemanager.GetString("passwordSyr");
            dg_office.Columns[6].Header = MainWindow.resourcemanager.GetString("passwordSoto");


            //dg_office.Columns[3].Header = MainWindow.resourcemanager.GetString("trMobile");

            tt_clear.Content = MainWindow.resourcemanager.GetString("trClear");
            tt_report.Content = MainWindow.resourcemanager.GetString("trPdf");
            tt_excel.Content = MainWindow.resourcemanager.GetString("trExcel");
            tt_preview.Content = MainWindow.resourcemanager.GetString("trPreview");
            tt_print.Content = MainWindow.resourcemanager.GetString("trPrint");
            //tt_count.Content = MainWindow.resourcemanager.GetString("trCount");

            btn_add.Content = MainWindow.resourcemanager.GetString("trAdd");
            btn_update.Content = MainWindow.resourcemanager.GetString("trUpdate");
            btn_delete.Content = MainWindow.resourcemanager.GetString("trDelete");

        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
         

                HelpClass.StartAwait(grid_main);
             
                office = new Users();
                if (HelpClass.validate(requiredControlList, this))
                {
                    //tb_custCode.Text = await office.generateCodeNumber("cu");

                    office.name = tb_name.Text;
                    //office.joinDate = dp_joinDate.SelectedDate;
                    //office.mobile =tb_mobile.Text.Trim();
                    //office.userName = tb_userName.Text;
                    //office.passwordSY = tb_passwordSY.Text;
                    //office.PasswordSoto = tb_PasswordSoto.Text;
                    //office.notes = tb_notes.Text;

                    office.createUserId = MainWindow.userLogin.userId;
                    office.updateUserId = MainWindow.userLogin.userId;


                    decimal s = await office.Save(office);
                    if (s <= 0)
                        Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    else
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);


                        Clear();
                        await RefreshOfficesList();
                        await Search();
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
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                HelpClass.StartAwait(grid_main);
              
                if (office.userId > 0)
                {
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        //office.custname = tb_custname.Text;
                        office.name = tb_name.Text;
                        //office.joinDate = dp_joinDate.SelectedDate;
                        //office.mobile = tb_mobile.Text.Trim();
                        //office.userName = tb_userName.Text;
                        //office.passwordSY = tb_passwordSY.Text;
                        //office.PasswordSoto = tb_PasswordSoto.Text;
                        office.notes = tb_notes.Text;
                        office.updateUserId = MainWindow.userLogin.userId;
                        //office.countryId = Convert.ToInt32(cb_country.SelectedValue);
                        //office.email = tb_email.Text;
                        //office.mobile = cb_areaMobile.Text + "-" + tb_mobile.Text; ;
                        //if (!tb_phone.Text.Equals(""))
                        //    office.phone = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                        //if (!tb_fax.Text.Equals(""))
                        //    office.fax = cb_areaFax.Text + "-" + cb_areaFaxLocal.Text + "-" + tb_fax.Text;
                        //if (cb_custlevel.SelectedValue != null)
                        //    office.custlevel = cb_custlevel.SelectedValue.ToString();
                        //office.company = tb_company.Text.Trim();
                        //office.address = tb_address.Text;

                        //office.createUserId = MainWindow.userLogin.userId;

                        //office.balance = 0;

                        decimal s = await office.Save(office);
                        if (s <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);

                            //if (isImgPressed)
                            //{
                            //    int officeId = (int)s;
                            //    string b = await office.uploadImage(imgFileName, Md5Encription.MD5Hash("Inc-m" + officeId.ToString()), officeId);
                            //    office.image = b;
                            //    isImgPressed = false;
                            //    if (!b.Equals(""))
                            //    {
                            //        await getImg();
                            //    }
                            //    else
                            //    {
                            //        HelpClass.clearImg(btn_image);
                            //    }
                            //}

                            await RefreshOfficesList();
                            await Search();
                        }
                    }
                }
                else
                    Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                HelpClass.StartAwait(grid_main);
                if (office.userId != 0)
                {
                    if ((!office.canDelete) && (office.isActive == false))
                    {
                        #region
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                        w.contentText = MainWindow.resourcemanager.GetString("trMessageBoxActivate");
                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;
                        #endregion

                        if (w.isOk)
                            await activate();
                    }
                    else
                    {
                        #region
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                        if (office.canDelete)
                            w.contentText = MainWindow.resourcemanager.GetString("trMessageBoxDelete");
                        if (!office.canDelete)
                            w.contentText = MainWindow.resourcemanager.GetString("trMessageBoxDeactivate");
                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;
                        #endregion

                        if (w.isOk)
                        {
                            string popupContent = "";
                            if (office.canDelete) popupContent = MainWindow.resourcemanager.GetString("trPopDelete");
                            if ((!office.canDelete) && (office.isActive == true)) popupContent = MainWindow.resourcemanager.GetString("trPopInActive");
                            var s = 0;
                            //var s = await office.Delete(office.officeId, MainWindow.userLogin.userId, office.canDelete);
                            if (s < 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                //office.officeId = 0;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await RefreshOfficesList();
                                await Search();
                                Clear();

                            }
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
        private async Task activate()
        {//activate
            office.isActive = true;
            var s = await office.Save(office);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshOfficesList();
                await Search();
            }
        }
        #endregion
        #region events
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Tgl_isActive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (offices is null)
                    await RefreshOfficesList();
                tgl_officestate = true;
                if (first)
                {
                    first = false;
                }
                else
                {
                    await Search();

                }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Tgl_isActive_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (offices is null)
                    await RefreshOfficesList();
                tgl_officestate = false;
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Dg_office_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                //selection
                if (dg_office.SelectedIndex != -1)
                {
                    office = dg_office.SelectedItem as Users;
                    this.DataContext = office;
                    if (office != null)
                    {
                        //tb_custCode.Text = office.custCode;
                        //cb_country.SelectedValue = office.countryId;
                        this.DataContext = office;
                        #region delete
                        if (office.canDelete)
                            btn_delete.Content = MainWindow.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (office.isActive == false)
                                btn_delete.Content = MainWindow.resourcemanager.GetString("trActive");
                            else
                                btn_delete.Content = MainWindow.resourcemanager.GetString("trInActive");
                        }
                        #endregion
                    }
                }
                HelpClass.clearValidate(requiredControlList, this);
                //p_error_email.Visibility = Visibility.Collapsed;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
                await RefreshOfficesList();
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

        #region Refresh & Search
        async Task Search()
        {
            //search
            //if (offices is null)
            //    await RefreshOfficesList();
          
            //searchText = tb_search.Text.ToLower();
            //officesQuery = offices.Where(s =>
            //(s.officeId.ToString().Contains(searchText) ||
            //s.name.ToLower().Contains(searchText) ||
            //s.userName.ToLower().Contains(searchText) ||
            //s.joinDate.ToString().Contains(searchText)
            //||
            //s.mobile.ToLower().Contains(searchText)
            //) && s.isActive == tgl_officestate);
            //&& s.isActive == tgl_officestate
            //);
            
            RefreshOfficesView();
        }
        async Task<IEnumerable<Users>> RefreshOfficesList()
        {
            offices = await office.GetAll();
            return offices;
        }
        void RefreshOfficesView()
        {
            dg_office.ItemsSource = officesQuery;
          // txt_count.Text = officesQuery.Count().ToString();
        }
        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 

        void Clear()
        {
            this.DataContext = new Users();


            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only  digits
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only english and digits
                Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
                if (!regex.IsMatch(e.Text))
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private void Spaces_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = e.Key == Key.Space;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void validateEmpty_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion



        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // Collect all generations of memory.
            GC.Collect();
        }

        #region reports

        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog2 = new SaveFileDialog();
       
        public void BuildReport()
        {

            //string firstTitle = "paymentsReport";
            ////string secondTitle = "";
            ////string subTitle = "";
            //string Title = "";
          
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            //if (isArabic)
            //{
            addpath = @"\Reports\SectionData\Ar\ArOffice.rdlc";

            //}
            //else
            //{
            //    addpath = @"\Reports\SectionData\En\EnOffices.rdlc";
            //}
            //D:\myproj\posproject3\SerialGenerator\SerialGenerator\Reports\statisticReports\En\EnBook.rdlc
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            //     subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            //  Title = MainWindow.resourcemanagerreport.GetString("trAccountantReport");

            //clsReports.OfficesReport(officesQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();
             
        }

        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                HelpClass.StartAwait(grid_main);

                #region
                BuildReport();

                saveFileDialog.Filter = "PDF|*.pdf;";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToPDF(rep, filepath);
                }
                #endregion

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {

            //preview
            try
            {
                HelpClass.StartAwait(grid_main);

                #region
                Window.GetWindow(this).Opacity = 0.2;

                string pdfpath = "";
                //
                pdfpath = @"\Thumb\report\temp.pdf";
                pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                BuildReport();

                LocalReportExtensions.ExportToPDF(rep, pdfpath);
                wd_previewPdf w = new wd_previewPdf();
                w.pdfPath = pdfpath;
                if (!string.IsNullOrEmpty(w.pdfPath))
                {
                    w.ShowDialog();
                    w.wb_pdfWebViewer.Dispose();


                }
                Window.GetWindow(this).Opacity = 1;
                #endregion

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {

            //print
            try
            {
                HelpClass.StartAwait(grid_main);

                #region
                BuildReport();
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, FillCombo.rep_printer_name, FillCombo.rep_print_count == null ? short.Parse("1") : short.Parse(FillCombo.rep_print_count));
                #endregion

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {

            //excel
            try
            {
                HelpClass.StartAwait(grid_main);

                #region
                //Thread t1 = new Thread(() =>
                //{
                BuildReport();
                this.Dispatcher.Invoke(() =>
                {
                    saveFileDialog.Filter = "EXCEL|*.xls;";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filepath = saveFileDialog.FileName;
                        LocalReportExtensions.ExportToExcel(rep, filepath);
                    }
                });


                //});
                //t1.Start();
                #endregion

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{
                #region
                Window.GetWindow(this).Opacity = 0.2;
                //win_lvc win = new win_lvc(officesQuery, 2, false);
                //win.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                #endregion
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }



        #endregion

    
    
        private async void Btn_uploadDocs_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    HelpClass.StartAwait(grid_main);

            //    if (office.officeId > 0)
            //{
            //    FileClass fcls = new FileClass();
            //    string foldername = office.officeId.ToString();
            //    openFileDialog.Multiselect = true;
            //    openFileDialog.Title = MainWindow.resourcemanager.GetString("docUpload"); 
            //        if (openFileDialog.ShowDialog() == true)
            //        {
            //            string dir = System.IO.Path.Combine(Global.rootofficeFolder, foldername);
            //            if (!Directory.Exists(dir))
            //            {
            //                Directory.CreateDirectory(dir);
            //            }
            //            // Read the files
            //            decimal s = 0;
            //            foreach (String file in openFileDialog.FileNames)
            //            {
            //                // Create a PictureBox.
                             

            //                    string fName = System.IO.Path.GetFileNameWithoutExtension(file);
            //                    string fileName = System.IO.Path.GetFileName(file);
            //                    string ext = System.IO.Path.GetExtension(file);
            //                    string destpath = System.IO.Path.Combine(dir, fileName);
            //                    string newname = fName;
            //                    while (File.Exists(destpath))
            //                    {
            //                        newname = fName + "-" + HelpClass.GenerateRandomNo();
            //                        destpath = System.IO.Path.Combine(dir, newname + ext);
            //                    }
            //                    File.Copy(file, destpath);
            //                    //save todb
            //                    fcls.fileName = newname;
            //                    fcls.extention = ext;
            //                    fcls.folderName = foldername;
            //                    fcls.tableRowId = office.officeId;
            //                   s = await fcls.SaveOffice(fcls);
                          


            //                }
            //            if (s <= 0)
            //                Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            //            else
            //            {
            //                Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopUpload"), animation: ToasterAnimation.FadeIn);
            //            }
            //        }
            //    }
            //    else
            //        Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

            //    HelpClass.EndAwait(grid_main);
            //}
            //catch (Exception ex)
            //{
            //    HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this);
            //}
        }
     
        private void Btn_exportDocs_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (sender != null)
            //        HelpClass.StartAwait(grid_main);
            //    if (office.officeId > 0)
            //    {             
            //    Window.GetWindow(this).Opacity = 0.2;
            //    wd_files w = new wd_files();
            //    w.type = "office";
            //    w.itemId = office.officeId;
            //    w.ShowDialog();
            //    //await FillCombo.fillStatementsTable(cb_opStatement);
            //    Window.GetWindow(this).Opacity = 1;
            //    }
            //    else
            //    {

            //        Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

            //    }

            //    if (sender != null)
            //        HelpClass.EndAwait(grid_main);
            //}
            //catch (Exception ex)
            //{
            //    Window.GetWindow(this).Opacity = 1;
            //    if (sender != null)
            //        HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this);
            //}
        }
    }
}
