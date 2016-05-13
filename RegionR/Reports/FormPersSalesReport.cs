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
using System.Security;
using System.Security.Permissions;

namespace RegionR.Reports
{
    public partial class FormPersSalesReport : Form
    {
        private const string REPORT_PATH = "Report1.rdlc";
        //private const string REPORT_PATH = "..\\..\\Reports\\Report1.rdlc";

        private int _currentYear;

        public FormPersSalesReport()
        {
            InitializeComponent();

            int year = DateTime.Today.Year;

            cbYear.Items.Add(year);
            cbYear.Items.Add(year - 1);
            cbYear.Items.Add(year - 2);
            cbYear.SelectedIndex = 0;
        }

        private void FormPersSalesReport_Load(object sender, EventArgs e)
        {
            CreateReport();
        }

        private void CreateReport()
        {
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            LocalReport localReport = reportViewer1.LocalReport;
            
            //localReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
            
            localReport.ReportPath = REPORT_PATH;
            
            _currentYear = GetYear();
            
            PersSalesReportList persSalesReportList = PersSalesReportList.GetUniqueInstance(_currentYear);
            
            List<PersSalesReport> list = persSalesReportList.GetList();
            
            ReportDataSource dsSalesOrder = new ReportDataSource("PersSalesList", list);
            
            if (localReport.DataSources.Count > 0)
                localReport.DataSources.Clear();
            
            localReport.DataSources.Add(dsSalesOrder);

            reportViewer1.RefreshReport();
        }

        private void reportViewer1_ReportRefresh(object sender, CancelEventArgs e)
        {
            if (_currentYear != GetYear())
                CreateReport();
        }

        private int GetYear()
        {
            int year = Convert.ToInt32(cbYear.SelectedItem);
            return year;
        }
    }
}
