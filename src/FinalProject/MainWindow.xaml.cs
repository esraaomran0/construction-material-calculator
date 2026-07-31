using FinalProject.Models;
using FinalProject.Widows;
using FinalProject.Windows;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System;


namespace FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<material> listofmaterials { get; set; }
        public ObservableCollection<orders> Listoforders { get; set; }
        public DateTime LastSaved { get; set; }  
        private AppData data;
        public MainWindow()
        {
            InitializeComponent();
            data=Helper.Helper.LoadFromJson();
            listofmaterials=data.Materials;
            Listoforders =data.Orders;
            DataContext =this; //datacontext at all window so thebinding can see all things
            LastSaved = DateTime.Now;
        }
        //close main window
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save before closing?", "Exit", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result==MessageBoxResult.Yes)
            {
                Helper.Helper.savetojison(data);
            }
            else if (result==MessageBoxResult.Cancel)
            {
                e.Cancel=true;
            }
        }

        #region menue(file and help)
        //Helper then about
        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Construction Material Manager Project\n\n" +"Developer: Esraa Hussein", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //file then save
        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            Helper.Helper.savetojison(data);
            LastSaved=DateTime.Now;
            DataContext = null;
            DataContext=this;
            MessageBox.Show("Data saved successfully","Save",MessageBoxButton.OK,MessageBoxImage.Information);
        }
        // file then load
        private void MenuLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog(); //create window to open file 
            openDialog.Filter="JSON Files (*.json)|*.json"; //select type of file
            openDialog.Title="Load Data File";

            if (openDialog.ShowDialog()==true) //if i choosed file
            {
                string filepath = openDialog.FileName;
                string jsonData = System.IO.File.ReadAllText(filepath);
                AppData loadedData = Newtonsoft.Json.JsonConvert.DeserializeObject<AppData>(jsonData);//convert data from jison to object

                if (loadedData!=null)
                {
                    data=loadedData;
                    listofmaterials=data.Materials;
                    Listoforders=data.Orders;
                    DataContext=this;
                    MessageBox.Show("Data loaded successfully", "Load", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to load file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // file the exit
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save before closing?", "Exit", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result== MessageBoxResult.Yes)
            {
                Helper.Helper.savetojison(data);
                Application.Current.Shutdown();
            }
            else if (result == MessageBoxResult.No)
            {
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region buttons
        private void AddMaterial(object sender, RoutedEventArgs e)
        {
            AddMaterialwindow addnewmaterial = new AddMaterialwindow(data); //create new window
            addnewmaterial.ShowDialog(); //open window
            LastSaved=DateTime.Now;
            DataContext = null;
            DataContext=this;
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            if (materialsatdataGrid.SelectedItem is material selecmaterial)
            {
                MessageBoxResult result = MessageBox.Show(
              $"Are you sure you want to delete '{selecmaterial.Name}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    listofmaterials.Remove(selecmaterial);
                    Helper.Helper.savetojison(data);
                }
            }
            else
            {
                MessageBox.Show("Please select a material to delete.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            LastSaved=DateTime.Now;
            DataContext = null;
            DataContext=this;
        }
        private void Open_Calculator(object sender, RoutedEventArgs e)
        {
            CalculatorWindow addnewcalculator = new CalculatorWindow(data);
            addnewcalculator.ShowDialog();
            LastSaved=DateTime.Now;
            DataContext = null;
            DataContext=this;
        }

        private void Open_Orders(object sender, RoutedEventArgs e)
        {
            OrdersWindow addnewcalculator = new OrdersWindow(data);
            addnewcalculator.ShowDialog();
            LastSaved=DateTime.Now;
            DataContext = null;
            DataContext=this;
        }
        #endregion

        #region data about project
        //pending orders
        public int PendingCount => Listoforders?.Count(o=>o.Status=="Pending")??0;       
        //total spent at this month
        public decimal TotalSpentThisMonth=>Listoforders?.Where(o=>o.Date.Month==DateTime
        .Now.Month&&o.Date.Year==DateTime.Now.Year).Sum(o=>(decimal)o.Quantity*o.UnitPrice)??0;
        #endregion              
    }
}