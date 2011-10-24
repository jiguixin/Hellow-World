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

        public static bool IsValidZip(string str, string gRCode)
        {
            // TODO: zip validation
            //Debug.Fail("IsValidZip() not implemented");

            //CultureInfo ci = Thread.CurrentThread.CurrentUICulture;
            // should implement validation based on cultureinfo: USA, canada, China

            return true;
        }

        public static bool IsValidatePassword(string str)
        {
            // TODO: zip validation
            //Debug.Fail("IsValidatePassword() not implemented");

            return true;
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
