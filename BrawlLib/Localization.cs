using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.ComponentModel
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter)]
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        static string Localize(string key)
        {
            return BrawlLib.Properties.Resources.ResourceManager.GetString(key);
        }
        public LocalizedDescriptionAttribute(string key) : base(Localize(key)) { }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        static string Localize(string key)
        {
            return BrawlLib.Properties.Resources.ResourceManager.GetString(key);
        }
        public LocalizedDisplayNameAttribute(string key) : base(Localize(key)) { }
    }
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter)]
    public class LocalizedCategoryAttribute : CategoryAttribute
    {
        public LocalizedCategoryAttribute(string key) : base(key) { }
        protected override string GetLocalizedString(string value)
        {
            return BrawlLib.Properties.Resources.ResourceManager.GetString(value);
        }
    }
}
