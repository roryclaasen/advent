// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.
// using System;

using System;
using System.ComponentModel;
using System.Globalization;

[TypeConverter(typeof(AocNumberTypeConverter))]
internal class AocNumber
{
    public AocNumber(int value)
    {
        this.Value = value;
    }

    public AocNumber(string value)
    {
        if (int.TryParse(value, out var intValue))
        {
            this.Value = intValue;
        }
    }

    public int? Value { get; set; }

    public static implicit operator int?(AocNumber aocYear)
        => aocYear.Value;

    public static implicit operator string(AocNumber aocYear)
        => aocYear.Value?.ToString() ?? string.Empty;

    public class AocNumberTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string stringValue)
            {
                return new AocNumber(stringValue);
            }

            if (value is int intValue)
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
