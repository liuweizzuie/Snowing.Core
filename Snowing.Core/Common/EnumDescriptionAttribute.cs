using Snowing.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Snowing.Common
{
    /// <summary>
    /// EnumDescription 用于描述枚举的特性。	
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EnumDescriptionAttribute : Attribute
    {
        private static IDictionary<string, IList<EnumDescriptionAttribute>> EnumDescriptionCache = new Dictionary<string, IList<EnumDescriptionAttribute>>(); //EnumType.FullName - IList<EnumDescription>

        #region Ctor
        public EnumDescriptionAttribute(string _description) : this(_description, null)
        {
        }

        public EnumDescriptionAttribute(string _description, object _tag)
        {
            this.description = _description;
            this.tag = _tag;
        }
        #endregion

        #region property
        #region Description
        private string description = "";
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        #endregion

        #region EnumValue
        private object enumValue = null;
        public object EnumValue
        {
            get
            {
                return this.enumValue;
            }
        }
        #endregion

        #region Tag
        private object tag = null;
        public object Tag
        {
            get
            {
                return this.tag;
            }
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return this.description;
        }
        #endregion

        #endregion

        #region DoGetFieldTexts
        /// <summary>
        /// DoGetFieldTexts 得到枚举类型定义的所有枚举值的描述文本		
        /// </summary>	
        private static IList<EnumDescriptionAttribute> GetDescriptions(Type enumType)
        {
            if (!EnumDescriptionAttribute.EnumDescriptionCache.ContainsKey(enumType.FullName))
            {
                FieldInfo[] fields = enumType.GetFields();
                IList<EnumDescriptionAttribute> list = new List<EnumDescriptionAttribute>();
                foreach (FieldInfo fi in fields)
                {
                    object[] eds = fi.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                    if (eds.Length == 1)
                    {
                        EnumDescriptionAttribute enumDescription = (EnumDescriptionAttribute)eds[0];
                        enumDescription.enumValue = fi.GetValue(null);
                        list.Add(enumDescription);
                    }
                }

                EnumDescriptionAttribute.EnumDescriptionCache.Add(enumType.FullName, list);
            }

            return EnumDescriptionAttribute.EnumDescriptionCache[enumType.FullName];
        }
        #endregion

        #region GetEnumDescriptionText
        /// <summary>
        /// GetEnumDescriptionText 获取枚举类型的描述文本。
        /// </summary>	   
        public static string GetEnumDescriptionText(Type enumType)
        {
            EnumDescriptionAttribute[] enumDescriptionAry = (EnumDescriptionAttribute[])enumType.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (enumDescriptionAry.Length < 1)
            {
                return string.Empty;
            }

            return enumDescriptionAry[0].Description;
        }
        #endregion

        /// <summary>
        /// 获取枚举值的描述文本。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionByValue<T>(Object value) where T : struct
        {
            Type enumType = typeof(T);
            IList<EnumDescriptionAttribute> descriptions = GetDescriptions(enumType);
            EnumDescriptionAttribute description = descriptions.First(e => string.Equals(e.EnumValue.ToString(), value.ToString()));
            return description.Description;
        }

        #region GetEnumTag
        /// <summary>
        /// GetEnumTag 获取枚举类型携带的Tag。
        /// </summary>
        public static object GetEnumTag(Type enumType)
        {
            EnumDescriptionAttribute[] eds = (EnumDescriptionAttribute[])enumType.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (eds.Length != 1) return string.Empty;
            return eds[0].Tag;
        }
        #endregion

        #region GetFieldText
        /// <summary>
        /// GetFieldDescriptionText 获得指定枚举值的描述文本。
        /// </summary>		
        public static string GetFieldText(object enumValue)
        {
            IList<EnumDescriptionAttribute> list = EnumDescriptionAttribute.GetDescriptions(enumValue.GetType());
            if (list == null)
            {
                return null;
            }
            list.ToArray();
            return list.ConvertFirst<EnumDescriptionAttribute, string>(ed => ed.Description, ed => string.Equals(ed.EnumValue.ToString(), enumValue.ToString()));

        }
        #endregion

        #region GetFieldTag
        /// <summary>
        /// GetFieldTag 获得指定枚举值的Tag。
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static object GetFieldTag(object enumValue)
        {
            IList<EnumDescriptionAttribute> list = EnumDescriptionAttribute.GetDescriptions(enumValue.GetType());
            if (list == null)
            {
                return null;
            }

            return list.ConvertFirst<EnumDescriptionAttribute, object>(ed => ed.Tag, ed => string.Equals(ed.EnumValue.ToString(), enumValue.ToString()));

        }
        #endregion

        #region GetEnumValueByTag
        /// <summary>
        /// GetEnumValueByTag 根据描述Tag获取对应的枚举值
        /// </summary>     
        public static object GetEnumValueByTag(Type enumType, object tag)
        {
            IList<EnumDescriptionAttribute> list = EnumDescriptionAttribute.GetDescriptions(enumType);
            if (list == null)
            {
                return null;
            }
            return list.ConvertFirst<EnumDescriptionAttribute, object>(ed => ed.enumValue, ed => string.Equals(ed.Tag.ToString(), tag.ToString()));
        }

        #endregion

        public static T GetEnumValueByDescription<T>(string description)
        {
            IList<EnumDescriptionAttribute> list = EnumDescriptionAttribute.GetDescriptions(typeof(T));
            return (T)list.First(ed => string.Equals(ed.Description, description)).EnumValue;
        }
    }
}
