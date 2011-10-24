using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Data;
using System.Xml;

namespace Infrastructure.Crosscutting.Utility.CommomHelper
{
    /// <summary>
    /// 文件读写实用类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 读取文件,返回一个byte的数组
        /// </summary>
        /// <param name="fileName">要读取的文件路径</param>
        /// <returns>返回一个byte的数组</returns>
        public static byte[] ReadFile(string fileName)
        {
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                return pReadByte;
            }
            catch
            {
                return pReadByte;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }

        public static List<string> ReadFileLine(string fileName)
        {
            List<string> lst = new List<string>();
            if (string.IsNullOrEmpty(fileName))
                return lst; 

            StreamReader sr = new StreamReader(fileName,Encoding.Default);
           
            string content = string.Empty;

            while (content != null)
            {
                content = sr.ReadLine();
                
                if (content != null)
                {
                    lst.Add(content);    
                }                
            }

            sr.Close();

            return lst;
        }

        public static string ReadFileContent(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(fileName))
                return "";
            StreamReader sr = new StreamReader(fileName,Encoding.Default);

            string content = sr.ReadToEnd();
            sr.Close();

            return content;
        }

        public static DataSet ReadFileByExcel(string excelFilePath)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(excelFilePath))
                return ds;
            string connStr = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + excelFilePath +
                                ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
      
            List<string> lstTables = OleDbHelper.GetExcelTableNameList(connStr);

            if (lstTables == null || lstTables.Count == 0)
                return ds;

            string baseSql = "select * from [{0}]";


            foreach (string table in lstTables)
            {
                string sql = string.Format(baseSql, table);

               DataTable dt =  OleDbHelper.ExecuteDataTable(connStr, CommandType.Text, sql);

               ds.Tables.Add(dt);
            }
            return ds;
        }

        //
        /// <summary>
        /// 写byte[]到fileName
        /// </summary>
        /// <param name="pReadByte">要写入的数据</param>
        /// <param name="fileName">要写到的文件路径和文件名</param>
        /// <returns>若写入成功返回true,否则返回false</returns>
        public static bool WriteFile(byte[] pReadByte, string fileName)
        {

            FileStream pFileStream = null;
            try
            {
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);
            }

            catch
            {
                return false;
            }

            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();

            }

            return true;

        }

        /// <summary>
        /// 创建文件目录
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <returns></returns>
        public static bool CreateDirectory(string strPath)
        {
            try
            {
                if (strPath.Length == 0)
                { return false; }

                if (Directory.Exists(strPath))
                { return true; }

                Directory.CreateDirectory(strPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreateFile(string strPath)
        {
            try
            {
                if (string.IsNullOrEmpty(strPath))
                    return false;
                if (File.Exists(strPath))
                    return true;

                File.CreateText(strPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 创建文件目录
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <returns></returns>
        public static bool CreateFileDirectory(string strPath)
        {
            try
            {
                if (strPath.Length == 0)
                { return false; }

                if (Directory.Exists(strPath))
                { return true; } 

                Directory.CreateDirectory(Path.GetDirectoryName(strPath));
                return true;
            }
            catch
            {
                return false;
            }
        }
 
        /// <summary>
        /// 写文本日志
        /// </summary>
        /// <param name="strFile">文件名及路径</param>
        /// <param name="strLog">记录日志内容</param>
        public static void WriteFileLog(string strFile, string strLog)
        {
            try
            { 
                string fullPath = Path.GetDirectoryName(strFile);

                if (string.IsNullOrEmpty(fullPath))
                {
                    strFile = AppDomain.CurrentDomain.BaseDirectory + "\\" + strFile;
                }
                else
                {
                    CreateFileDirectory(strFile); 
                }

                StreamWriter sw = new StreamWriter(strFile, true);
                sw.WriteLine("[" + DateTime.Now.ToString() + "]：" + strLog);
                sw.Flush();
                sw.Close(); 
            }
            catch
            { }
        }

        public static Dictionary<string, string> ReadConfig()
        {
            Dictionary<string, string> setting = new Dictionary<string, string>();
            XmlDocument xdoc = new XmlDocument();
            XmlElement xroot = null;

            try
            {
                xdoc.Load("Connection.xml");
                xroot = xdoc.DocumentElement;

                foreach (XmlElement xChild in xroot.ChildNodes.OfType<XmlElement>())
                    setting.Add(xChild.Attributes["name"].InnerText, xChild.FirstChild.InnerText);
            }
            catch
            {
            }

            return setting;

            //public static void ReadConfig()
            //{
            //    RegistryKey root = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\iTi");
            //    Type settingType = typeof(ApplicationBaseSetting);
            //    FieldInfo[] fis = settingType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            //    string[] valNames = root.GetValueNames();
            //    string val = null;

            //    foreach (FieldInfo fi in fis)
            //    {
            //        if (valNames.Contains(fi.Name))
            //        {
            //            val = root.GetValue(fi.Name) as string;
            //            if (!TextHelper.IsEmptyString(val))
            //            {
            //                if (fi.FieldType.IsEnum)
            //                    fi.SetValue(settingType, Enum.ToObject(fi.FieldType, byte.Parse(val)));
            //                else
            //                    fi.SetValue(settingType, Convert.ChangeType(val, fi.FieldType));
            //            }
            //            else
            //            {
            //                if (Util.IsNullable(fi.FieldType))
            //                    fi.SetValue(settingType, null);
            //            }
            //        }
            //    }

            //    // connection string

            //    root.Close();
            //}
        }

    }
}
