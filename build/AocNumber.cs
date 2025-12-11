// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.
// using System;

using System;
using System.ComponentModel;
using System.Globalization;

[TypeConverter(typeof(AocNumberTypeConverter))]
internal class AocNumber(int value)
{
    public int Value { get; set; } = value;

    public static implicit operator int(AocNumber aocYear) => aocYear.Value;

    public override string ToString() => this.Value.ToString();

    public class AocNumberTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string)
                || sourceType == typeof(int)
                || base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is int intValue)
            {
                return new AocNumber(intValue);
            }

            if (value is string stringValue && int.TryParse(stringValue, culture, out intValue))
            {
                return new AocNumber(intValue);
            }

            if (value is null)
            {
                return null;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
