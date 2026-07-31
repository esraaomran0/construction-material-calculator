using FinalProject.Enums;
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FinalProject.Widows
{
    public partial class CalculatorWindow : Window
    {
        public List<ElementTypes> elementtypes { get; set; }
        public List<ElementTypes> elementtypes2 { get; set; }
        public List<material> MaterialName { get; set; }
        public List<material> MaterialName2 { get; set; }
        public List<material> PaintMaterials { get; set; }
        public List<BarsTypes> barsdiameter { get; set; }
        public List<SurfaceTypeForpaint> Surfacearea { get; set; }
        public List<material> TileMaterials { get; set; }
        public List<TileSize> tilesize { get; set; }
        private AppData appdata { get; set; }
        public CalculatorWindow(AppData data)
        {
            InitializeComponent();
            appdata = data;
            DataContext = this;

            #region Concrete Init
            elementtypes =Enum.GetValues(typeof(ElementTypes)).Cast<ElementTypes>().ToList();
            MaterialName =data.Materials.Where(mat=>mat.Category=="Concrete").ToList();
            Calculate_btn.IsEnabled = false;
            #endregion

            #region Steel Init
            MaterialName2 =data.Materials.Where(mat => mat.Category =="Steel").ToList();
            barsdiameter =Enum.GetValues(typeof(BarsTypes)).Cast<BarsTypes>().ToList();
            calc_btn_steel.IsEnabled = false;
            SteelLengthBox.Text="12";
            #endregion

            #region Paint Init
            Surfacearea =Enum.GetValues(typeof(SurfaceTypeForpaint)).Cast<SurfaceTypeForpaint>().ToList();
            PaintMaterials =data.Materials.Where(m=>m.Category=="Paint").ToList();
            CalculatePaint.IsEnabled = false;
            PaintCoverageBox.Text="12";
            #endregion

            #region Tiles Init
            TileMaterials =data.Materials.Where(m=>m.Category=="Tiles").ToList();
            tilesize =Enum.GetValues(typeof(TileSize)).Cast<TileSize>().ToList();
            CalculateTiles.IsEnabled = false;
            WastePercentagebox.Text = "10";
            #endregion
        }

        //buttons

        #region Concrete
        private void Calculate_btn_Click(object sender, RoutedEventArgs e)
        {
            double length=Helper.Helper.checkvalue(ConcreteLengthBox);
            double width=Helper.Helper.checkvalue(ConcreteWidthBox);
            double depth=Helper.Helper.checkvalue(ConcreteDepthBox);
            double quantity=Helper.Helper.checkvalue(ConcreteQuantityBox);
            double volume = length*width*depth*quantity;
            ConcreteResultText.Text=(volume+(.1*volume)).ToString();
        }
        private void save_btn_Click(object sender, RoutedEventArgs e)
        {            
            var selectedmaterial = ConcreteName.SelectedItem as material;

            var neworder = new orders
            {
                OrderNumber = appdata.Orders.Count+1,
                MaterialName = selectedmaterial.Name,
                Category = selectedmaterial.Category,
                Quantity = Helper.Helper.checkvalue(ConcreteQuantityBox),
                Unit = selectedmaterial.Unit,
                elementtype = (ElementTypes)ElementTypeBox.SelectedItem,
                UnitPrice = selectedmaterial.UnitPrice,
                Status = "Pending",
                Date = DateTime.Now,
            };
            appdata.Orders.Add(neworder);
            Helper.Helper.savetojison(appdata);
            MessageBox.Show("Order saved", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Steel
        private void calc_btn_steel_Click(object sender, RoutedEventArgs e)
        {
            BarsTypes selectedBar = (BarsTypes)SteelDiameterBox.SelectedItem;
            string diameterStr = selectedBar.ToString().Substring(1);
            double diameter = double.Parse(diameterStr);
            double length = Helper.Helper.checkvalue(SteelLengthBox);
            double quantity = Helper.Helper.checkvalue(SteelBarsBox);
            double totalweight = ((diameter*diameter)/162)*length*quantity;
            SteelResultText.Text=(totalweight+(.05*totalweight)).ToString();
        }
        private void save_btn_steel_Click(object sender, RoutedEventArgs e)
        {
            var selectedmaterial = steelName.SelectedItem as material;
            var neworder = new orders
            {
                OrderNumber = appdata.Orders.Count+1,
                MaterialName = selectedmaterial.Name,
                Category = selectedmaterial.Category,
                Quantity = Helper.Helper.checkvalue(SteelBarsBox),
                Unit = selectedmaterial.Unit,
                elementtype = (ElementTypes)ElementTypee.SelectedItem,
                UnitPrice = selectedmaterial.UnitPrice,
                Status = "Pending",
                Date = DateTime.Now,
            };
            appdata.Orders.Add(neworder);
            Helper.Helper.savetojison(appdata);
            MessageBox.Show("Order saved", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Paint
        private void CalculatePaint_Click(object sender, RoutedEventArgs e)
        {         
            double area = Helper.Helper.checkvalue(PaintAreaBox);          
            int coats = int.Parse(((ComboBoxItem)(PaintCoatsBox.SelectedItem)).Content.ToString());
            double coverage = Helper.Helper.checkvalue(PaintCoverageBox);
            double result=(area*coats)/coverage;
            PaintResultText.Text=result.ToString();
        }
        private void SavePaint_Click(object sender, RoutedEventArgs e)
        {            
            var selectedmaterial=PaintMaterialBox.SelectedItem as material;
            var neworder = new orders
            {
                OrderNumber = appdata.Orders.Count+1,
                MaterialName = selectedmaterial.Name,
                Category = selectedmaterial.Category,
                Quantity = Helper.Helper.checkvalue(PaintAreaBox),
                Unit = selectedmaterial.Unit,
                elementtype = (ElementTypes)PaintAreaType.SelectedItem,
                UnitPrice = selectedmaterial.UnitPrice,
                Status = "Pending",
                Date = DateTime.Now,
            };
            appdata.Orders.Add(neworder);
            Helper.Helper.savetojison(appdata);
            MessageBox.Show("Order saved", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Tiles
        private void CalculateTiles_Click(object sender, RoutedEventArgs e)
        {          
            double roomLength=Helper.Helper.checkvalue(RoomLengthBox);
            double roomWidth=Helper.Helper.checkvalue(RoomWidthBox);
            double area=roomLength*roomWidth;
            string value = ((TileSize)TileSizeBox.SelectedItem).ToString().Substring(1);
            string[] parts = value.Split('x');
            double tileWidth = double.Parse(parts[0])/100;
            double tilelength = double.Parse(parts[1])/100;
            double tilesarea = tileWidth * tilelength;
            double wastePercentage = Helper.Helper.checkvalue(WastePercentagebox);
            double result = (area/tilesarea)*(1 + wastePercentage/100);
            TilesResultText.Text = $"{result:F0} Tiles";
        } 
        private void SaveTiles_Click(object sender, RoutedEventArgs e)
        {           
            var selectedmaterial = TileMaterialBox.SelectedItem as material;
            var neworder = new orders
            {
                OrderNumber = appdata.Orders.Count + 1,
                MaterialName = selectedmaterial.Name,
                Category = selectedmaterial.Category,
                Quantity = 1,  //assume its one room
                Unit = selectedmaterial.Unit,
                elementtype = ElementTypes.Slab,
                UnitPrice = selectedmaterial.UnitPrice,
                Status = "Pending",
                Date = DateTime.Now,
            };
            appdata.Orders.Add(neworder);
            Helper.Helper.savetojison(appdata);
            MessageBox.Show("Order saved", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Validation
        //needed func. checks
        private bool IsValidPositive(TextBox textBox)
        {
            return double.TryParse(textBox.Text,out double value)&&value> 0;
        }
        private void TurnOnOffButtons()
        {
            bool concreteValid =
                IsValidPositive(ConcreteLengthBox) &&
                IsValidPositive(ConcreteWidthBox) &&
                IsValidPositive(ConcreteDepthBox) &&
                IsValidPositive(ConcreteQuantityBox)&&ConcreteName.SelectedItem!=null&&ElementTypeBox.SelectedItem!=null;
            Calculate_btn.IsEnabled = concreteValid;

            bool steelValid =
                IsValidPositive(SteelLengthBox) &&
                IsValidPositive(SteelBarsBox)&&steelName.SelectedItem!=null&&SteelDiameterBox.SelectedItem!=null;
            calc_btn_steel.IsEnabled = steelValid;

            bool paintValid =
                IsValidPositive(PaintAreaBox) &&
                IsValidPositive(PaintCoverageBox)&&PaintMaterialBox.SelectedItem!=null&&PaintAreaType.SelectedItem!=null
                &&PaintCoatsBox.SelectedItem!=null ;
            CalculatePaint.IsEnabled = paintValid;

            double waste = Helper.Helper.checkvalue(WastePercentagebox);
            bool tilesValid =
                IsValidPositive(RoomLengthBox) &&
                IsValidPositive(RoomWidthBox) &&
                waste >= 5 && waste <= 20&&TileMaterialBox.SelectedItem!=null&&TileSizeBox.SelectedItem!=null; ;
            CalculateTiles.IsEnabled = tilesValid;
        }
        //for combo box
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TurnOnOffButtons();
        }
        //on all dimensions to be positive numbers
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Name == "WastePercentagebox") return;

                string input = textBox.Text;
                bool isValid =double.TryParse(input,out double value) && value > 0;

                if (!isValid&&!string.IsNullOrEmpty(input))
                {
                    textBox.BorderBrush = Brushes.Red;
                    textBox.BorderThickness = new Thickness(1.5);
                    textBox.ToolTip = "Please enter a positive number greater than 0";
                }
                else
                {
                    textBox.ClearValue(BorderBrushProperty);
                    textBox.ToolTip = null;
                }
            }
            TurnOnOffButtons();
        }
        //on tiles
        private void WastePercentagebox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double WastePercentage = Helper.Helper.checkvalue(WastePercentagebox);
            if (!string.IsNullOrEmpty(WastePercentagebox.Text) &&
                WastePercentage >= 5 && WastePercentage <= 20)
            {
                WastePercentagebox.ClearValue(BackgroundProperty);
                WastePercentagebox.ToolTip = null;
            }
            else
            {
                WastePercentagebox.Background = Brushes.LightCoral;
                WastePercentagebox.ToolTip = "Waste Percentage must be between 5% and 20%";
            }
            TurnOnOffButtons();
        }
        #endregion

    }
}