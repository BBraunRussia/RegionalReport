using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;

namespace RegionR.Reports
{
    public partial class FormPersSalesReport : Form
    {
        public FormPersSalesReport()
        {
            InitializeComponent();
        }

        private void FormPersSalesReport_Load(object sender, EventArgs e)
        {
            // Set the processing mode for the ReportViewer to Local
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            LocalReport localReport = reportViewer1.LocalReport;

            localReport.ReportPath = "..\\..\\Reports\\Report1.rdlc";

            PersSalesReportList persSalesReportList = PersSalesReportList.GetUniqueInstance(DateTime.Today.Year);

            List<PersSalesReport> list = persSalesReportList.GetList();
                        
            // Create a report data source for the sales order data
            ReportDataSource dsSalesOrder = new ReportDataSource("PersSalesList", list);

            localReport.DataSources.Add(dsSalesOrder);
            /*
            // Create a report data source for the sales order detail 
            // data
            ReportDataSource dsSalesOrderDetail =
                new ReportDataSource();
            dsSalesOrderDetail.Name = "SalesOrderDetail";
            dsSalesOrderDetail.Value =
                dataset.Tables["SalesOrderDetail"];

            localReport.DataSources.Add(dsSalesOrderDetail);
            
            // Create a report parameter for the sales order number 
            ReportParameter rpSalesOrderNumber = new ReportParameter();
            rpSalesOrderNumber.Name = "SalesOrderNumber";
            rpSalesOrderNumber.Values.Add("SO43661");
            
            // Set the report parameters for the report
            localReport.SetParameters(
                new ReportParameter[] { rpSalesOrderNumber });
            */
            // Refresh the report
            
            reportViewer1.RefreshReport();
        }
    }
}
