
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace FinalProject.Widows
{
    public partial class AddMaterialwindow : Window
    {
        public List<string> MaterialType { get; set; }
        public List<string> unittype { get; set; }
        private AppData appdata { get; set; }
        public AddMaterialwindow(AppData data)
        {
            InitializeComponent();
            appdata = data;
            saave.IsEnabled = false;
            MaterialType=appdata.Materials.Select(m=>m.Category).Distinct().ToList();
            DataContext=this;
        }

        #region buttons
        private void Button_Save(object sender, RoutedEventArgs e)
        {            
            material newMaterial=new material
            {
                Name= materialname.Text,
                Category= categoryname.SelectedItem.ToString(),
                Unit=((ComboBoxItem)unitindecation.SelectedItem).Content.ToString(),
                UnitPrice=decimal.Parse(unit_price.Text)
            };
            appdata.Materials.Add(newMaterial);
            Helper.Helper.savetojison(appdata);
            this.Close();
        }
        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region validation
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                bool isValid = true;
                string errorMessage = "";

                // check price textbox
                if (textBox.Name == "unit_price")
                {
                    isValid=double.TryParse(textBox.Text, out double val)&&val> 0;
                    errorMessage="Please enter a valid number greater than 0";
                }
                else
                {
                    isValid=!string.IsNullOrWhiteSpace(textBox.Text)&!double.TryParse(textBox.Text, out _)
                        &&!(appdata.Materials.Any(m=>m.Name.ToLower()==textBox.Text.Trim().ToLower()));
                    errorMessage="Cannot be empty or a number or duplicate name";
                }

                // apply validation
                if (!isValid&&!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.BorderBrush=Brushes.Red;
                    textBox.BorderThickness=new Thickness(1.5);
                    textBox.ToolTip=errorMessage;
                }
                else
                {
                    textBox.ClearValue(BorderBrushProperty);
                    textBox.ToolTip=null;
                }
            }

            TurnOnOffBtn();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TurnOnOffBtn();
        }
        private void TurnOnOffBtn()
        {
            saave.IsEnabled=true;
            bool isNameValid =!string.IsNullOrWhiteSpace(materialname.Text);
            bool isUnitValid=unitindecation.SelectedItem !=null;
            bool isCategoryValid =categoryname.SelectedItem !=null;
            bool isPriceValid =double.TryParse(unit_price.Text,out double price)&&price>0;
            if (saave!=null)
            {
                saave.IsEnabled=isNameValid&&isUnitValid&&isCategoryValid&&isPriceValid;
            }
        }
        #endregion
    }
}
