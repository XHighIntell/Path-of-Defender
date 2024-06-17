using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualBasic;

namespace Path_of_Defender {
    //public class ModConverter : ExpandableObjectConverter {
    //    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
    //        if (context != null && context.ToString().IndexOf("System.Windows.Forms.PropertyGridInternal.SingleSelectRootGridEntry") == -1) { return true; } 
    //        else { return false; }
    //    }
    //    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
    //        Mod mod = new Mod();
    //        if (propertyValues != null) {
    //            mod.Type = (ModType)propertyValues["Mod_Type"];
    //            mod.Value = (float)propertyValues["Mod_Value"];
    //        }
    //        return mod;
    //    }
    //}
    public class RangeValuesConverter : ExpandableObjectConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            //return base.CanConvertFrom(context, sourceType);
            //bool s = base.CanConvertFrom(context, sourceType); 
            if (sourceType == typeof(string)) { return true; } 
            else { return base.CanConvertFrom(context, sourceType); }
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
            try { 
                if (value is string) {
                    float value1, value2;
                    string[] s = Strings.Split((string)Strings.Replace((string)value, " ", ""), "-");
                    value1 = float.Parse(s[0]);
                    value2 = float.Parse(s[1]);
                    RangeValues Return = new RangeValues(value1, value2);

                    return Return;
                } else {
                    return base.ConvertFrom(context, culture, value);
                }
            }
            catch {
                return new RangeValues();
                //return context.; 
            
            }
        }
    }

    public class ItemCostConverter : ExpandableObjectConverter {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
            if (context != null && context.ToString().IndexOf("System.Windows.Forms.PropertyGridInternal.SingleSelectRootGridEntry") == -1) { return true; } 
            else { return false; }
        }
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
            ItemCost cost = new ItemCost();
            if (propertyValues != null) {
                cost.Item_Base_Name = (string)propertyValues["item"];
                cost.Quantity = (int)propertyValues["quantity"];
            }
            return cost;
        }
    }
    public class ObjectConverter : ExpandableObjectConverter {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
            if (context != null && context.ToString().IndexOf("System.Windows.Forms.PropertyGridInternal.SingleSelectRootGridEntry") == -1) { return true; } 
            else { return false; }
        }
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
            object obj = Activator.CreateInstance(context.PropertyDescriptor.PropertyType);

            foreach (System.Collections.DictionaryEntry entry in propertyValues) {
                System.Reflection.PropertyInfo pi = context.PropertyDescriptor.PropertyType.GetProperty(entry.Key.ToString());
                if ((pi != null) && (pi.CanWrite)) {
                    pi.SetValue(obj, Convert.ChangeType(entry.Value, pi.PropertyType), null);
                }
            }
            return obj;
        }
    }

    public class ItemIamgeEdior : UITypeEditor {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
            
        }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context) {
            return base.GetPaintValueSupported(context);
        }
        public override bool IsDropDownResizable {
            get { return true; }
        }
        ListBox List = new ListBox() { BorderStyle = BorderStyle.None };
        IWindowsFormsEditorService Form_Service;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            if (Form_Service == null) {
                Form_Service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                List.SelectedValueChanged += new EventHandler(List_SelectedValueChanged);
                for (int i = 0; i < Images.Items.Names.Length; i++) {
                    List.Items.Add(Images.Items.Names[i]);
                    if (Images.Items.Names[i] == (string)value) { List.SelectedIndex = i; }
                }
            }
            Form_Service.DropDownControl(List);
            if (List.SelectedItem == null) { return value; }
            return List.SelectedItem;
        }
        private void List_SelectedValueChanged(object sender, EventArgs e) {
            Form_Service.CloseDropDown();
            
        }
    }


    public struct SystemItemStructurexx {
        public ItemStructure[] White_Items;
        public ItemStructure[] Unique_Items;
        public Affix[] Prefixes, Suffixes;
        public string[] Rare_Prefixes_Name, Rare_Suffixes_Name;
    }

    //public class 
}
