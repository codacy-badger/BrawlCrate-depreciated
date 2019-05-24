using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Globalization;

namespace System
{
    internal class UserDataConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is UserDataClass)
            {
                return ((UserDataClass)value).ToString();
            }

            return base.ConvertTo(context, culture, value, destType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;
                    string[] s2 = s.Split(':');
                    string[] s3 = s2[1].Split(',');

                    UserDataClass d = new UserDataClass
                    {
                        Name = s2[0]
                    };
                    foreach (string i in s3)
                    {
                        d._entries.Add(i);
                    }

                    return d;
                }
                catch { }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return (destType == typeof(UserDataClass)) ? true : base.CanConvertTo(context, destType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string)) ? true : base.CanConvertFrom(context, sourceType);
        }
    }

    internal class ExpandableObjectCustomConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            string s = (string)base.ConvertTo(context, culture, value, destType);
            return s.Substring(s.LastIndexOf('.') + 1);
        }
    }

    internal class UInt32HexTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(uint))
            {
                return $"0x{value:X8}";
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string input = (string)value;

                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(2);
                }

                return uint.Parse(input, System.Globalization.NumberStyles.HexNumber, culture);
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
    internal class Vector2fTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(Vector2))
            {
                Vector2 vec = (Vector2)value;
                return $"({vec._x},{vec._y})";
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string input = (string)value;
                if (input.StartsWith("(", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Trim(new char[] { '(', ')' });
                }

                string f1 = input.Substring(0, input.IndexOf(',')).Trim();
                string f2 = input.Substring(input.IndexOf(",") + 1, input.Length - (input.IndexOf(",") + 1));

                return new Vector2(float.Parse(f1), float.Parse(f2));
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
