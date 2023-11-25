using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SerialGenerator.ApiClasses;
using Newtonsoft.Json;
//using POS.View.storage;
namespace SerialGenerator.Classes
{
    class clsReports
    {
        public static void setReportLanguage(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("lang", MainWindow.Reportlang));

        }

        public static void Header(List<ReportParameter> paramarr)
        {

            ReportCls rep = new ReportCls();
            paramarr.Add(new ReportParameter("companyName", FillCombo.companyName));
            paramarr.Add(new ReportParameter("Fax", FillCombo.Fax));
            paramarr.Add(new ReportParameter("Tel", FillCombo.Mobile));
            paramarr.Add(new ReportParameter("Address", FillCombo.Address));
            paramarr.Add(new ReportParameter("Email", FillCombo.Email));
            paramarr.Add(new ReportParameter("logoImage", "file:\\" + rep.GetLogoImagePath()));
            paramarr.Add(new ReportParameter("trcomAddress", MainWindow.resourcemanagerreport.GetString("trAddress")));
            paramarr.Add(new ReportParameter("trcomTel", MainWindow.resourcemanagerreport.GetString("trmobilenum")));//tel
            paramarr.Add(new ReportParameter("trcomFax", MainWindow.resourcemanagerreport.GetString("fax")));
            paramarr.Add(new ReportParameter("trcomEmail", MainWindow.resourcemanagerreport.GetString("email")));


        }
        public static void HeaderNoLogo(List<ReportParameter> paramarr)
        {

            ReportCls rep = new ReportCls();
            paramarr.Add(new ReportParameter("companyName", FillCombo.companyName));
            paramarr.Add(new ReportParameter("Fax", FillCombo.Fax));
            paramarr.Add(new ReportParameter("Tel", FillCombo.Mobile));
            paramarr.Add(new ReportParameter("Address", FillCombo.Address));
            paramarr.Add(new ReportParameter("Email", FillCombo.Email));


        }
        //public static void bankdg(List<ReportParameter> paramarr)
        //{

        //    ReportCls rep = new ReportCls();
        //    paramarr.Add(new ReportParameter("trTransferNumber", MainWindow.resourcemanagerreport.GetString("trTransferNumberTooltip")));


        //}
        //public static void serialsReport(IEnumerable<PosSerials> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();

        //  //  paramarr.Add(new ReportParameter("trVendor", MainWindow.resourcemanagerreport.GetString("trVendor")));
        //    rep.DataSources.Add(new ReportDataSource("DataSetSerials", Query));

      

        //}
        //public static void serialsMailReport(IEnumerable<PosSerials> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();

        //    //  paramarr.Add(new ReportParameter("trVendor", MainWindow.resourcemanagerreport.GetString("trVendor")));
        //    rep.DataSources.Add(new ReportDataSource("DataSetSerials", Query));



        //}
         public static void agentReport(IEnumerable<Users> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", MainWindow.resourcemanagerreport.GetString("trAgents")));
            //table columns
            paramarr.Add(new ReportParameter("trCode", MainWindow.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", MainWindow.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trMobile", MainWindow.resourcemanagerreport.GetString("trMobile")));
          

            DateFormConv(paramarr);
        }
        public static void usersReport(IEnumerable<Users> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", MainWindow.resourcemanagerreport.GetString("trAgents")));
            //table columns
            paramarr.Add(new ReportParameter("trCode", MainWindow.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", MainWindow.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trMobile", MainWindow.resourcemanagerreport.GetString("trMobile")));


            DateFormConv(paramarr);
        }
        
        public static void CustomerSerialsReport(IEnumerable<CustomerSerials> QueryList, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            List<CustomerSerials> Query = JsonConvert.DeserializeObject<List<CustomerSerials>>(JsonConvert.SerializeObject(QueryList));
            foreach (CustomerSerials row in Query)
            {
                row.strstartDate = dateFrameConverter(row.startDate);
                row.strexpireDate = dateFrameConverter(row.expireDate);
            }

            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", MainWindow.resourcemanagerreport.GetString("activationcopy")));
            //table columns
            paramarr.Add(new ReportParameter("trNo", MainWindow.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("officeName", MainWindow.resourcemanagerreport.GetString("officeName")));
            paramarr.Add(new ReportParameter("trStartDate", MainWindow.resourcemanagerreport.GetString("trStartDate")));
            paramarr.Add(new ReportParameter("durationyear", MainWindow.resourcemanagerreport.GetString("durationyear")));
            paramarr.Add(new ReportParameter("trEndTime", MainWindow.resourcemanagerreport.GetString("trEndTime")));
            paramarr.Add(new ReportParameter("agentIdCode", MainWindow.resourcemanagerreport.GetString("agentIdCode")));
         

            /*
             *  trNo
officeName
trStartDate
durationyear
trEndTime
agentIdCode

             * */
            //DateFormConv(paramarr);
        }


        public static string dateFrameConverter(DateTime? date)
        {
            DateTime dateval;
            if (date is DateTime)
                dateval = (DateTime)date;
            else return date.ToString();

            switch (MainWindow.dateFormat)
            {
                case "ShortDatePattern":
                    return dateval.ToString(@"dd/MM/yyyy");
                case "LongDatePattern":
                    return dateval.ToString(@"dddd, MMMM d, yyyy");
                case "MonthDayPattern":
                    return dateval.ToString(@"MMMM dd");
                case "YearMonthPattern":
                    return dateval.ToString(@"MMMM yyyy");
                default:
                    return dateval.ToString(@"dd/MM/yyyy");
            }

        }

      
   

        
        //public static void CustomersReport(IEnumerable<Customers> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();
        //    rep.DataSources.Add(new ReportDataSource("DataSet", Query));
        //    //title
        //    paramarr.Add(new ReportParameter("trTitle", MainWindow.resourcemanagerreport.GetString("trAgents")));
        //    //table columns
        //    paramarr.Add(new ReportParameter("trCode", MainWindow.resourcemanagerreport.GetString("trCode")));
        //    paramarr.Add(new ReportParameter("trName", MainWindow.resourcemanagerreport.GetString("trName")));
        //    paramarr.Add(new ReportParameter("trCompanyName", MainWindow.resourcemanagerreport.GetString("trName")));
        //    paramarr.Add(new ReportParameter("trMobile", MainWindow.resourcemanagerreport.GetString("trMobile")));


        //    DateFormConv(paramarr);
        //}

        //public static void packagesReport(IEnumerable<Packages> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();
        //    rep.DataSources.Add(new ReportDataSource("DataSet", Query));
        //    //title
        //    paramarr.Add(new ReportParameter("trTitle", MainWindow.resourcemanagerreport.GetString("trPackages")));
        //    //table columns
        //    paramarr.Add(new ReportParameter("trCode", MainWindow.resourcemanagerreport.GetString("trCode")));
        //    paramarr.Add(new ReportParameter("trPackage", MainWindow.resourcemanagerreport.GetString("trPackage")));
        //    paramarr.Add(new ReportParameter("trProgram", MainWindow.resourcemanagerreport.GetString("trProgram")));
        //    paramarr.Add(new ReportParameter("trVersion", MainWindow.resourcemanagerreport.GetString("trVersion")));


        //    DateFormConv(paramarr);
        //}
        //public static void  versionsReport(IEnumerable<Versions> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();
        //    rep.DataSources.Add(new ReportDataSource("DataSet", Query));
        //    //title
        //    paramarr.Add(new ReportParameter("trTitle", MainWindow.resourcemanagerreport.GetString("trVersions")));
        //    //table columns
        //    paramarr.Add(new ReportParameter("trCode", MainWindow.resourcemanagerreport.GetString("trCode")));
        //    paramarr.Add(new ReportParameter("trName", MainWindow.resourcemanagerreport.GetString("trName")));
        //    paramarr.Add(new ReportParameter("trProgram", MainWindow.resourcemanagerreport.GetString("trProgram")));

        //    DateFormConv(paramarr);
        //}
        //public static void programsReport(IEnumerable<Programs> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();
        //    rep.DataSources.Add(new ReportDataSource("DataSet", Query));
        //    //title
        //    paramarr.Add(new ReportParameter("trTitle", MainWindow.resourcemanagerreport.GetString("trPrograms")));
        //    //table columns
        //    paramarr.Add(new ReportParameter("trCode", MainWindow.resourcemanagerreport.GetString("trCode")));
        //    paramarr.Add(new ReportParameter("trName", MainWindow.resourcemanagerreport.GetString("trName")));
        //    paramarr.Add(new ReportParameter("trProgram", MainWindow.resourcemanagerreport.GetString("trProgram")));

        //    DateFormConv(paramarr);
        //}
            public static void PaymentsSaleParam(  List<ReportParameter> paramarr)
        {
             
          
            //table columns
            paramarr.Add(new ReportParameter("trProcessNumTooltip", MainWindow.resourcemanagerreport.GetString("trProcessNumTooltip")));
            paramarr.Add(new ReportParameter("trBookNum", MainWindow.resourcemanagerreport.GetString("trBookNum")));
            paramarr.Add(new ReportParameter("trPackage", MainWindow.resourcemanagerreport.GetString("trPackage")));
            paramarr.Add(new ReportParameter("trProcessDate", MainWindow.resourcemanagerreport.GetString("trProcessDate")));
            paramarr.Add(new ReportParameter("trExpirationDate", MainWindow.resourcemanagerreport.GetString("trExpirationDate")));
            paramarr.Add(new ReportParameter("trPaid", MainWindow.resourcemanagerreport.GetString("trPaid")));

         
        }
         //public static void BookSale(IEnumerable<PackageUser> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();
        //    rep.DataSources.Add(new ReportDataSource("DataSetBook", Query));
        //    //title
        //    paramarr.Add(new ReportParameter("trTitle", MainWindow.resourcemanagerreport.GetString("trServiceBook")));
        //    //table columns
        //    //paramarr.Add(new ReportParameter("trProcessNumTooltip", MainWindow.resourcemanagerreport.GetString("trProcessNumTooltip")));
        //    //paramarr.Add(new ReportParameter("trBookNum", MainWindow.resourcemanagerreport.GetString("trBookNum")));
        //    //paramarr.Add(new ReportParameter("trProcessDate", MainWindow.resourcemanagerreport.GetString("trProcessDate")));
        //    //paramarr.Add(new ReportParameter("trExpirationDate", MainWindow.resourcemanagerreport.GetString("trExpirationDate")));
        //    //paramarr.Add(new ReportParameter("trPaid", MainWindow.resourcemanagerreport.GetString("trPaid")));

        //    DateFormConv(paramarr);
        //}

        public static void DateFormConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("dateForm", MainWindow.dateFormat));
        }

        //public static void bondsDocReport(LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();



        //    DateFormConv(paramarr);


        //}

        

        //public static void orderReport(IEnumerable<PosSerials> invoiceQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();

        //    rep.DataSources.Add(new ReportDataSource("DataSetInvoice", invoiceQuery));
        //}

 
    }
}
