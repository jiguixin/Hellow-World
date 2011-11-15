using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Infrastructure.Crosscutting.Utility.CommomHelper;

namespace Infrastructure.Crosscutting.Declaration
{
    /// <summary>
    /// 封装一些对象的扩展方法
    /// </summary>
    public static class ObjectExtendMethod
    {
        #region 对象扩展方法
        /// <summary>
        /// 将一个对象转化为Int型数据，为空时时返回0
        /// </summary>
        /// <param name="obj">要转化为Int型数据的对象</param>
        /// <returns>Int型数据，若转化失败返回0</returns>
        public static int ToInt32(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return default(int);
            }
        }


        /// <summary>
        /// 将一个对象转化为日期型数据
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回时间型数据,若转化失败,则返回DateTime的默认值</returns>
        public static DateTime ToDateTime(this object obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return "1900-1-1".ToDateTime();
                //return default(DateTime);
            }
        }


        /// <summary>
        /// 将一个对象转化为逻辑性数据
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回布尔值,若转化失败,返回布尔型的默认值</returns>
        public static bool ToBoolean(this object obj)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return default(bool);
            }
        }

        /// <summary>
        /// 将一个对象转化为实数类型
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回实数类型,若转化失败,返回实数的默认值</returns>
        public static decimal ToDecimal(this object obj)
        {
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return default(decimal);
            }
        }

        /// <summary>
        /// 转化为实数类型，发生异常时返回默认，而不报错
        /// </summary>
        /// <param name="obj">要进行转化的对象</param>
        /// <returns>返回实数类型,若转化失败,返回实数的默认值</returns>
        public static double ToDouble(this object obj)
        {
            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return default(double);
            }
        }


        /// <summary>
        /// 反序列化
        ///  先将数据库中取出的对象反序强制转化为byte数组，再反序列化为对象
        /// </summary>
        /// <param name="obj">要进行反序列化的对象</param>
        /// <returns>反序列化后生成的对象</returns>
        public static object Deserialize(this object obj)
        {
            try
            {
                return SerializeHelper.Deserialize((byte[])obj);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 序列话，将一个对象序列化为byte数组
        /// </summary>
        /// <param name="obj">要进行序列化的对象</param>
        /// <returns>返回二进制数据</returns>
        public static byte[] Serialize(this object obj)
        {
            return SerializeHelper.Serialize(obj);
        }

        #endregion

        #region 字符串扩展方法
        /// <summary>
        /// 判断字符串是否为空
        /// 为空时返回true、否则返回false
        /// </summary>
        /// <param name="s"></param>
        /// <returns>为空时返回true、否则返回false</returns>
        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// 判断字符串是否为int
        /// 为int 时返回true、否则返回false
        /// </summary>
        /// <param name="s"></param>
        /// <returns>为int 时返回true、否则返回false</returns>
        public static bool IsInt(this string s)
        {
            int i;
            bool b = int.TryParse(s, out i);
            return b;
        }

        /// <summary>
        /// 扩展方法用来判断字符串是不是Email形式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmail(this string s)
        {
            //Regex r = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");     
            Regex r = new Regex(@"^[\w-]+(\.[\w-]+)*\.*@[\w-]+(\.[\w-]+)+$");
            return r.IsMatch(s);
        }

        public static bool IsEmptyString(this string str)
        {
            return string.IsNullOrEmpty(str) || str.Trim().Length == 0;
        }

        public static bool IsNumberString(this string str)
        {
            int result = 0;
            return int.TryParse(str, out result);
        }

        public static bool IsValidateIP(this string str)
        {
            string pattern = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";

            if (IsEmptyString(str))
                return true;

            str = str.Trim();

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            return rgx.IsMatch(str);
        }

        public static bool IsValidateEmail(this string str)
        {
            string pattern = @"\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b";

            if (IsEmptyString(str))
                return true;

            str = str.Trim();

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            return rgx.IsMatch(str);
        }

        public static bool IsValidateUrl(this string str)
        {
            string pattern = @"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)";

            if (IsEmptyString(str))
                return true;

            str = str.Trim();

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            return rgx.IsMatch(str);
        }

        public static bool IsValidatePhone(this string str)
        {
            //string pattern = @"\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b";

            if (IsEmptyString(str))
                return true;

            //str = str.Trim();

            //Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            //return rgx.IsMatch(str);
            return true;
        }
         
        /// <summary>
        /// 日期格式字符串判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string str)
        {
            bool result;
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    DateTime.Parse(str);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns>验证成功为True，否则为False</returns>
        public static bool CheckIDCard(this string Id)
        {
            if (Id.Length == 18)
            {
                return ObjectExtendMethod.CheckIDCard18(Id);
            }
            return Id.Length == 15 && ObjectExtendMethod.CheckIDCard15(Id);
        }

        /// <summary>
        /// 转换成 HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string HtmlEncode(this string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        /// 解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string HtmlDecode(this string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        /// <summary>
        /// SQL注入字符清理
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        public static string SqlTextClear(this string sqlText)
        {
            if (sqlText == null)
            {
                return null;
            }
            if (sqlText == "")
            {
                return "";
            }
            sqlText = sqlText.Replace(",", "");
            sqlText = sqlText.Replace("<", "");
            sqlText = sqlText.Replace(">", "");
            sqlText = sqlText.Replace("--", "");
            sqlText = sqlText.Replace("'", "");
            sqlText = sqlText.Replace("\"", "");
            sqlText = sqlText.Replace("=", "");
            sqlText = sqlText.Replace("%", "");
            sqlText = sqlText.Replace(" ", "");
            return sqlText;
        }

        /// <summary>
        /// 在字符串中提取数值
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static decimal GetNumber(this string str)
        {
            decimal result = 0;
            if (!string.IsNullOrEmpty(str))
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                str = Regex.Replace(str, @"[^\d.\d]", " ");
                // 如果是数字，则转换为decimal类型 
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = decimal.Parse(str);
                }
            }
            return result; 
        }

        /// <summary>
        /// 提取字符串中的数值，如果不为数值者替换为 空字符 得到后在分割得到想到的数据
        /// 用str.Split(' ')分割，去掉不想要的空字符
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static string GetNumberStr(this string str)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                result = Regex.Replace(str, @"[^\d.\d]", " ");                  
            }
            return result;
        }

        /// <summary>
        /// 在字符串中提取整型
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static int GetNumberInt(this string str)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(str))
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                str = Regex.Replace(str, @"[^\d\d]", " ");
                // 如果是数字，则转换为decimal类型 
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = int.Parse(str);
                }
            }
            return result; 
        }

        #endregion

        #region IEnumerable扩展方法

        /// <summary>
        /// 将指定类型列表转换为T-Sql的In语句的类型
        /// </summary>        
        /// <returns></returns>
        public static string ToSqlInContent<TSource>(this IEnumerable<TSource> source)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var content in source)
                {
                    sb.Append("'");
                    sb.Append(content.ToString());
                    sb.Append("'");
                    sb.Append(",");
                }

                if (sb.Length > 2)
                    sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 将指定类型的列表转换为以逗号分开的字符串，如（"1,2,3"）
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static string ToColumnString<TSource>(this IEnumerable<TSource> sources)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                foreach (var content in sources)
                {
                    if (sb.Length > 0)
                        sb.Append(CommomConst.COMMA);

                    sb.Append(content);
                }

                if (sb.Length > 0)
                    return sb.ToString();
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 转换为一个DataTable
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TResult>(this IEnumerable<TResult> value) where TResult : class
        {
            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口
            Type type = typeof(TResult);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach<PropertyInfo>(type.GetProperties(), p =>
            {
                pList.Add(p);
                dt.Columns.Add(p.Name);
            });
            foreach (var item in value)
            {
                //创建一个DataRow实例
                DataRow row = dt.NewRow();
                //给row 赋值
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }


        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }

        #endregion

        #region DateTime 扩展方法

        /// <summary>
        /// 返回指定日期的是星期几，会根据区域信息来返回，如：中文环境为 “星期一”
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToCultureDayOfWeek(this DateTime dateTime)
        {
            try
            {
                return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dateTime.DayOfWeek);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 返回指定日期的农历日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToChineseDateTime(this DateTime dateTime)
        {
            try
            {
                return ChineseLunisolar.GetChineseDateTime(dateTime);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region Guid 扩展方法

        public static bool HasValue(this Guid source)
        {
            return source != Guid.Empty;
        }

        #endregion

        #region Helper

        #region String Extension Helper

        /// <summary>
        /// 验证15位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard18(string Id)
        {
            long num = 0L;
            if (!long.TryParse(Id.Remove(17), out num) || (double)num < Math.Pow(10.0, 16.0) || !long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out num))
            {
                return false;
            }
            string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (text.IndexOf(Id.Remove(2)) == -1)
            {
                return false;
            }
            string s = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime dateTime = default(DateTime);
            if (!DateTime.TryParse(s, out dateTime))
            {
                return false;
            }
            string[] array = "1,0,x,9,8,7,6,5,4,3,2".Split(new char[]
			{
				','
			});
            string[] array2 = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(new char[]
			{
				','
			});
            char[] array3 = Id.Remove(17).ToCharArray();
            int num2 = 0;
            for (int i = 0; i < 17; i++)
            {
                num2 += int.Parse(array2[i]) * int.Parse(array3[i].ToString());
            }
            int num3 = -1;
            Math.DivRem(num2, 11, out num3);
            return !(array[num3] != Id.Substring(17, 1).ToLower());
        }
        /// <summary>
        /// 验证18位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard15(string Id)
        {
            long num = 0L;
            if (!long.TryParse(Id, out num) || (double)num < Math.Pow(10.0, 14.0))
            {
                return false;
            }
            string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (text.IndexOf(Id.Remove(2)) == -1)
            {
                return false;
            }
            string s = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime dateTime = default(DateTime);
            return DateTime.TryParse(s, out dateTime);
        }

        #endregion

        #endregion

        //public static class IEnumerableExtensions
        //{
        //    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        //    {
        //        foreach (T item in items)
        //        {
        //            action(item);
        //        }
        //    }
        //}

        //public static class MiscExtensions
        //{
        //    #region Business Error Mapping

        //    public static List<ErrorItem> ToErrorItemList(this IEnumerable<ValidationErrorItem> validationErrorItems)
        //    {
        //        List<ErrorItem> items = new List<ErrorItem>();
        //        validationErrorItems.ForEach(validationErrorItem => items.Add(new ErrorItem { Key = validationErrorItem.ErrorKey, Parameters = validationErrorItem.Parameters }));
        //        return items;
        //    }

        //    #endregion
        //}
    }


    public static class BasicTypeExtensions
    {
       

        //public static bool HasValue(this UniqueId source)
        //{
        //    return source.Value != Guid.Empty;
        //}
    }
}
