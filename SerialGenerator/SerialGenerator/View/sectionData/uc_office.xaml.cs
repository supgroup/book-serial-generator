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
using Newtonsoft.Json;

namespace SerialGenerator.View.sectionData
{
    /// <summary>
    /// Interaction logic for uc_office.xaml
    /// </summary>
    public partial class uc_office : UserControl
    {
        public uc_office()
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
        private static uc_office _instance;
        public static uc_office Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new uc_office();
                return _instance;
            }
        }

        CustomerSerials customerSerialsModel = new CustomerSerials();
        IEnumerable<CustomerSerials> customerSerialQuery;
        IEnumerable<CustomerSerials> customerSerialList;
        int tgl_customerSerialstate = 1;
        bool first = true;
        string searchText = "";
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        public static List<string> requiredControlList;
        ReportCls repcls = new ReportCls();
        ActivateModel activeModel = new ActivateModel();
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                requiredControlList = new List<string> { "officename", "startDate", "years" };

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

                Keyboard.Focus(tb_officename);

                await RefreshOfficesList();
                await Search();
                Clear();
                FillCombo.fillyears(cb_years);
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
         
            txt_active.Text = MainWindow.resourcemanager.GetString("trActive");
            txt_title.Text = MainWindow.resourcemanager.GetString("activeData");

            //  activeData   officeNameHint trStartDateHint durationyearHint trEndTime  agentIdCodeHint  generateActiveCode  activationCodeHint
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_officename, MainWindow.resourcemanager.GetString("officeNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_startDate, MainWindow.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_years, MainWindow.resourcemanager.GetString("durationyearHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_customerCode, MainWindow.resourcemanager.GetString("agentIdCodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_activeCode, MainWindow.resourcemanager.GetString("activationCodeHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_PasswordSoto, MainWindow.resourcemanager.GetString("passwordSotoHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, MainWindow.resourcemanager.GetString("trNoteHint"));



            dg_office.Columns[0].Header = MainWindow.resourcemanager.GetString("trNo.");
            dg_office.Columns[1].Header = MainWindow.resourcemanager.GetString("officeName");
            dg_office.Columns[2].Header = MainWindow.resourcemanager.GetString("trStartDate");
            dg_office.Columns[3].Header = MainWindow.resourcemanager.GetString("durationyear");
            dg_office.Columns[4].Header = MainWindow.resourcemanager.GetString("trEndTime");
            dg_office.Columns[5].Header = MainWindow.resourcemanager.GetString("agentIdCode");

            tt_clear.Content = MainWindow.resourcemanager.GetString("trClear");
            tt_report.Content = MainWindow.resourcemanager.GetString("trPdf");
            tt_excel.Content = MainWindow.resourcemanager.GetString("trExcel");
            tt_preview.Content = MainWindow.resourcemanager.GetString("trPreview");
            tt_print.Content = MainWindow.resourcemanager.GetString("trPrint");
           

            btn_add.Content = MainWindow.resourcemanager.GetString("trAdd");
            btn_update.Content = MainWindow.resourcemanager.GetString("trUpdate");
            btn_delete.Content = MainWindow.resourcemanager.GetString("trDelete");
            btn_generatecode.Content = MainWindow.resourcemanager.GetString("generateActiveCode");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
         

                HelpClass.StartAwait(grid_main);
             
                customerSerialsModel = new CustomerSerials();
                if (HelpClass.validate(requiredControlList, this))
                {
                    //tb_custCode.Text = await customerSerialsModel.generateCodeNumber("cu");

                    customerSerialsModel.officeName = tb_officename.Text;
                    customerSerialsModel.startDate = dp_startDate.SelectedDate;
                     customerSerialsModel.yearCount = Convert.ToInt32(cb_years.SelectedValue); ;
                    customerSerialsModel.customerHardCode = string.IsNullOrEmpty(tb_customerCode.Text)?"": tb_customerCode.Text.Trim();
                    //customerSerialsModel.activateCode = tb_activeCode.Text;
                    customerSerialsModel.isActive = 1;

                    customerSerialsModel.createUserId = MainWindow.userLogin.userId;
                    customerSerialsModel.updateUserId = MainWindow.userLogin.userId;
                    // calculate
                    customerSerialsModel.expireDate = customerSerialsModel.startDate.Value.AddYears(customerSerialsModel.yearCount);
                    if (string.IsNullOrEmpty(customerSerialsModel.customerHardCode))
                    {
                        customerSerialsModel.confirmStat = "waithard";
                    }else if (string.IsNullOrEmpty(customerSerialsModel.activateCode))
                    {
                        customerSerialsModel.confirmStat = "notgen";
                    }
                    else
                    {
                        customerSerialsModel.confirmStat = "gen";
                    }
                   
                
                    decimal s = await customerSerialsModel.Save(customerSerialsModel);
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
              
                if (customerSerialsModel.customerSerialId > 0)
                {
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        refreshModel();
                         
                       customerSerialsModel.activateCode = generatecode();

                        decimal s = await customerSerialsModel.Save(customerSerialsModel);
                        if (s <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            
                            Toaster.ShowSuccess(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
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
                if (customerSerialsModel.customerSerialId != 0)
                {
                    if ((!customerSerialsModel.canDelete) && (customerSerialsModel.isActive != 1))
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
                        if (customerSerialsModel.canDelete)
                            w.contentText = MainWindow.resourcemanager.GetString("trMessageBoxDelete");
                        if (!customerSerialsModel.canDelete)
                            w.contentText = MainWindow.resourcemanager.GetString("trMessageBoxDeactivate");
                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;
                        #endregion

                        if (w.isOk)
                        {
                            string popupContent = "";
                            if (customerSerialsModel.canDelete) popupContent = MainWindow.resourcemanager.GetString("trPopDelete");
                            if ((!customerSerialsModel.canDelete) && (customerSerialsModel.isActive == 1)) popupContent = MainWindow.resourcemanager.GetString("trPopInActive");

                            var s = await customerSerialsModel.Delete(customerSerialsModel.customerSerialId, MainWindow.userLogin.userId, customerSerialsModel.canDelete);
                            if (s < 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                customerSerialsModel.customerSerialId = 0;
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
            customerSerialsModel.isActive = 1;
            var s = await customerSerialsModel.Save(customerSerialsModel);
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

                if (customerSerialList is null)
                    await RefreshOfficesList();
                tgl_customerSerialstate = 1;
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
                if (customerSerialList is null)
                    await RefreshOfficesList();
                tgl_customerSerialstate = 0;
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
                    customerSerialsModel = dg_office.SelectedItem as CustomerSerials;
                    this.DataContext = customerSerialsModel;
                    if (customerSerialsModel != null)
                    {
                        tb_activeCode.Text = customerSerialsModel.activateCode;
                      //  cb_years.SelectedValue = customerSerialsModel.yearCount;
                        #region delete
                        if (customerSerialsModel.canDelete)
                            btn_delete.Content = MainWindow.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (customerSerialsModel.isActive == 0)
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
                tb_search.Text = "";
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
            if (customerSerialList is null)
                await RefreshOfficesList();
          
            searchText = tb_search.Text.ToLower();
            customerSerialQuery = customerSerialList.Where(s =>
            (s.customerSerialId.ToString().Contains(searchText) ||
            s.Number.ToLower().Contains(searchText) ||
            s.officeName.ToLower().Contains(searchText) ||
            s.startDate.ToString().Contains(searchText)
            || s.customerHardCode.ToString().Contains(searchText)
            ||
            s.expireDate.ToString().Contains(searchText)
            || s.yearCount.ToString().Contains(searchText)
            
            
            ) && s.isActive == tgl_customerSerialstate);
            //&& s.isActive == tgl_customerSerialstate
            //);
            
            RefreshOfficesView();
        }
        async Task<IEnumerable<CustomerSerials>> RefreshOfficesList()
        {
            customerSerialList = await customerSerialsModel.GetAll();
            return customerSerialList;
        }
        void RefreshOfficesView()
        {
            dg_office.ItemsSource = customerSerialQuery;
          // txt_count.Text = customerSerialQuery.Count().ToString();
        }
        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 

        void Clear()
        {
            this.DataContext = new CustomerSerials();
            customerSerialsModel = new CustomerSerials();
            btn_generatecode.IsEnabled = false;
            tb_activeCode.Text = "";
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

            clsReports.CustomerSerialsReport(customerSerialQuery, rep, reppath, paramarr);
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
                //win_lvc win = new win_lvc(customerSerialQuery, 2, false);
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

            //    if (customerSerialsModel.customerSerialId > 0)
            //{
            //    FileClass fcls = new FileClass();
            //    string foldername = customerSerialsModel.customerSerialId.ToString();
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
            //                    fcls.tableRowId = customerSerialsModel.customerSerialId;
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
            //    if (customerSerialsModel.customerSerialId > 0)
            //    {             
            //    Window.GetWindow(this).Opacity = 0.2;
            //    wd_files w = new wd_files();
            //    w.type = "customerSerialsModel";
            //    w.itemId = customerSerialsModel.customerSerialId;
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

        private async void Btn_generatecode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);
                if (customerSerialsModel.customerSerialId > 0 && !string.IsNullOrEmpty(customerSerialsModel.customerHardCode))
                {
                    //string encodekey = "";
                    //activeModel = new ActivateModel();
                    //activeModel.confirmStat = customerSerialsModel.confirmStat;
                    //activeModel.customerHardCode = customerSerialsModel.customerHardCode;
                    //activeModel.expireDate = customerSerialsModel.expireDate;
                    //activeModel.officeName = customerSerialsModel.officeName;
                    //activeModel.startDate = customerSerialsModel.startDate;
                    //activeModel.yearCount = customerSerialsModel.yearCount;
                    //////

                    //string orginalkey = JsonConvert.SerializeObject(activeModel);
                    //string orginalkeyencripted = ReportCls.FinalEncode(orginalkey);


                    refreshModel();
                   
                    customerSerialsModel.activateCode = generatecode();
                    decimal s = await customerSerialsModel.Save(customerSerialsModel);

                    await RefreshOfficesList();
                    await Search();
                    //decode

                    string orginalkeydec = ReportCls.FinalDecode(tb_activeCode.Text);
                    ActivateModel activemodret = JsonConvert.DeserializeObject<ActivateModel>(orginalkeydec, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                 //   ActivateModel activemodret = JsonConvert.DeserializeObject<ActivateModel>(orginalkeydec);

                }
                else
                {

                    Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

                }

                if (sender != null)
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        public   string generatecode( )
        {
            try
            {
                if (customerSerialsModel.customerSerialId > 0 && !string.IsNullOrEmpty(customerSerialsModel.customerHardCode))
                {
                   
                    activeModel = new ActivateModel();
                    activeModel.confirmStat = customerSerialsModel.confirmStat;
                    activeModel.customerHardCode = customerSerialsModel.customerHardCode;
                    activeModel.expireDate = customerSerialsModel.expireDate;
                    activeModel.officeName = customerSerialsModel.officeName;
                    activeModel.startDate = customerSerialsModel.startDate;
                    activeModel.yearCount = customerSerialsModel.yearCount;
                    //

                    string orginalkey = JsonConvert.SerializeObject(activeModel);
                    string orginalkeyencripted = ReportCls.FinalEncode(orginalkey);// orginalkey
                    tb_activeCode.Text = orginalkeyencripted;
                    return orginalkeyencripted;
                }
                else
                {
                    tb_activeCode.Text = "";
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public void refreshModel()
        {
            
                if (customerSerialsModel.customerSerialId > 0)
                {
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        customerSerialsModel.officeName = tb_officename.Text;
                        customerSerialsModel.startDate = dp_startDate.SelectedDate;
                        customerSerialsModel.yearCount = Convert.ToInt32(cb_years.SelectedValue);
                        customerSerialsModel.customerHardCode = string.IsNullOrEmpty(tb_customerCode.Text) ? "" : tb_customerCode.Text.Trim();

                        customerSerialsModel.isActive = 1;

                        customerSerialsModel.updateUserId = MainWindow.userLogin.userId;
                        // calculate
                        customerSerialsModel.expireDate = customerSerialsModel.startDate.Value.AddYears(customerSerialsModel.yearCount);
                        if (string.IsNullOrEmpty(customerSerialsModel.customerHardCode))
                        {
                            customerSerialsModel.confirmStat = "waithard";
                        }
                        else if (string.IsNullOrEmpty(customerSerialsModel.activateCode))
                        {
                            customerSerialsModel.confirmStat = "notgen";
                        }
                        else
                        {
                            customerSerialsModel.confirmStat = "gen";
                        }
                    }
                }
                    
        }

        private void Tb_customerCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
                if (!(string.IsNullOrEmpty(tb_customerCode.Text)|| customerSerialsModel.customerSerialId<=0))
                {
                    btn_generatecode.IsEnabled = true;
                }
                else
                {
                    btn_generatecode.IsEnabled = false;
                }
               
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
