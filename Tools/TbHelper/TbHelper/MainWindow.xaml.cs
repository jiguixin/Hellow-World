using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Infrastructure.Crosscutting.Configuration;
using Infrastructure.Crosscutting.Tasks;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using System.Data;
using TbHelper.Model;
using Infrastructure.Crosscutting.Utility;
using System.Threading;

namespace TbHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    { 
        private BackgroundWorker bgwRun = new BackgroundWorker();

        private List<ClientName> lstClientName;
        private List<DataList> lstDataList;
        private List<Url> lstUrl;

        public MainWindow()
        {
            InitializeComponent();
            lstClientName = new List<ClientName>();
            lstDataList = new List<DataList>();
            lstUrl = new List<Url>();
            
            bgwRun.WorkerReportsProgress = true;
            bgwRun.DoWork += new DoWorkEventHandler(bgwRun_DoWork);
            bgwRun.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwRun_RunWorkerCompleted);
            bgwRun.ProgressChanged += new ProgressChangedEventHandler(bgwRun_ProgressChanged);

        }

        void bgwRun_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.labState.Content = e.UserState.ToString();
        }

        void bgwRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!bgwRun.IsBusy)
            {
                bgwRun.RunWorkerAsync();
            }
        }

        void bgwRun_DoWork(object sender, DoWorkEventArgs e)
        {
          
        }
        /**/
        /// <summary>
        /// 将Html标签转化为空格
        /// </summary>
        /// <param name="strHtml">待转化的字符串</param>
        /// <returns>经过转化的字符串</returns>
        private string stripHtml(string strHtml)
        {
            Regex objRegExp = new Regex("<(.|\n)+?>");
            string strOutput = objRegExp.Replace(strHtml, "");
            strOutput = strOutput.Replace("<", "&lt;");
            strOutput = strOutput.Replace(">", "&gt;");
            return strOutput;
        }
         
        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            if (!bgwRun.IsBusy)
            {
                bgwRun.RunWorkerAsync();
            }

            int i = 0;
            while (i < 5)
            {
                foreach (var dataList in lstDataList)
                {
                    foreach (var url in lstUrl)
                    {
                        foreach (var clientName in lstClientName)
                        {
                            string searchUrl = string.Format(url.WebSite, dataList.SearchKey, DateTime.Now.ToString("yyyyMMdd"));
                            webBrow.Navigate(searchUrl);
                            try
                            {
                                //bgwRun.ReportProgress(100, string.Format("正在请求{0}", searchUrl));
                                string webContent = HttpHelper.GETDataToUrl(searchUrl, Encoding.Default);

                                string tmpStr = "<li class=\"list-item\"[^>]*?>([\\s\\S]*?)</p>([\\s\\S]*?)</li>";
                                string regUrl = "href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))";
                                MatchCollection nameMatch = Regex.Matches(webContent, tmpStr, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                                foreach (Match nextmatch in nameMatch)
                                {
                                    if (nextmatch.Success)
                                    {
                                        var name = nextmatch.Groups[0].Value;
                                        if (name.Contains(clientName.Name))
                                        {
                                            Match urlMatch = Regex.Match(name, regUrl);

                                            if (urlMatch.Success)
                                            {
                                                string itemUrl = urlMatch.Value.Replace("href=\"", "").Replace("\"", "");

                                                //bgwRun.ReportProgress(100, string.Format("找到目标地址，正在请求：{0}", itemUrl));
                                                //HttpHelper.GETDataToUrl(itemUrl, Encoding.Default);
                                                webBrow.Navigate(itemUrl);
                                                Thread.Sleep(500);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }

                i++;
            }

            

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region GetData

            DataSet ds = ExcelHelper.GetExcelDataSet("Data.xls");

            if (ds == null)
            {
                MessageBox.Show("Data.xls文件不存在");
                return;
            }


            var dtDataList = ds.Tables["DataList"];
            var dtUrl = ds.Tables["Url"];
            var dtClientName = ds.Tables["ClientName"];

            if (dtDataList == null || dtUrl == null || dtClientName == null)
            {
                MessageBox.Show("Data.xls 中没有相应的表");
                return;
            }

            foreach (DataRow dr in dtDataList.Rows)
            {
                lstDataList.Add(new DataList(dr));
            }

            foreach (DataRow dr in dtUrl.Rows)
            {
                lstUrl.Add(new Url(dr));
            }

            foreach (DataRow dr in dtClientName.Rows)
            {
                lstClientName.Add(new ClientName(dr));
            }

            #endregion
        }
    }
}
