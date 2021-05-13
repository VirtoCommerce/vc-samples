using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Platform.Core.Settings;

namespace ProductVideoModule.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public const string Access = "productvideo:access";
                public const string Read = "productvideo:read";
                public const string Create = "productvideo:create";
                public const string Update = "productvideo:update";
                public const string Delete = "productvideo:delete";

                public static string[] AllPermissions = { Access, Read, Create, Update, Delete };
            }
        }
        public static class Settings
        {
            public static class General
            {
                //example SettingDescriptor
                public static readonly SettingDescriptor SomeConst = new SettingDescriptor
                {
                    Name = "ProductVideo.SomeConst",
                    GroupName = "ProductVideo|General",
                    ValueType = SettingValueType.Integer,
                    DefaultValue = 765,
                };
                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return SomeConst;
                        //yield return ProductVideo.Integer;
                        //yield return ProductVideo.DateTime;
                    }
                }
            }
            public static IEnumerable<SettingDescriptor> AllSettings
            {
                get
                {
                    return General.AllSettings;
                }
            }
        }
    }
}

