using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace DummyModule.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public const string Read = "dummy:read";
                public const string Create = "dummy:create";
                public const string Access = "dummy:access";
                public const string Update = "dummy:update";
                public const string Delete = "dummy:delete";

                public static string[] AllPermissions = { Read, Create, Access, Update, Delete };
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static SettingDescriptor DummyShortText = new SettingDescriptor
                {
                    Name = "Dummy.ShortText",
                    GroupName = "Dummy|General",
                    ValueType = SettingValueType.ShortText,
                    IsDictionary = true,
                    DefaultValue = "foo",
                    AllowedValues = new object[] { "bar", "foo" }
                };

                public static SettingDescriptor DummyInteger = new SettingDescriptor
                {
                    Name = "Dummy.Integer",
                    GroupName = "Dummy|General",
                    ValueType = SettingValueType.Integer,
                    DefaultValue = 50
                };

                public static SettingDescriptor DummyDateTime = new SettingDescriptor
                {
                    Name = "Dummy.DateTime",
                    GroupName = "Dummy|General",
                    ValueType = SettingValueType.DateTime,
                    DefaultValue = default(DateTime)
                };

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return DummyShortText;
                        yield return DummyInteger;
                        yield return DummyDateTime;
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
