using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Keyrox.Shared.Objects;
using System.Windows.Forms;

namespace Keyrox.Shared.Converters {
    public class HexConverter : TypeConverter {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }
            else { return base.CanConvertFrom(context, sourceType); }
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
            if (destinationType == typeof(string)) {
                return "0x" + value.ToUInt32().ToString("x").ToUpper();
            }
            else { return base.ConvertTo(context, culture, value, destinationType); }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(string)) {
                return true;
            }
            else { return base.CanConvertTo(context, destinationType); }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
            try {
                if (value.GetType() == typeof(string)) {
                    return Hex.GetUintFromHexString(value.ToString());
                }
                else { return base.ConvertFrom(context, culture, value); }
            }
            catch (Exception) { MessageBox.Show("Invalid Hexadecimal value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return "0".ToUInt32(); }
        }
        
        public override bool GetPropertiesSupported(ITypeDescriptorContext context) {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {
            return TypeDescriptor.GetProperties(value);
        }
    }
}
