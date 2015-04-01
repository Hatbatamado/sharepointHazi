using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
   public static class EnumHelper
    {
        public static string ToDisplayString(this Enum e)
        {
            return EnumDisplayStringAttribute.ToDisplayString(e);
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDisplayStringAttribute : Attribute
    {

        public EnumDisplayStringAttribute(string stringValue)
        {
            StringValue = stringValue;
        }

        public string StringValue { get; private set; }

        public static string ToDisplayString(Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            System.Reflection.FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the attributes
            EnumDisplayStringAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(EnumDisplayStringAttribute), false) as EnumDisplayStringAttribute[];

            string strValue = value.ToString();

            // Return the first if there was a match.
            if (attribs != null && attribs.Length > 0)
            {
                strValue = attribs[0].StringValue;
            }
            return strValue;
        }
    }
}
