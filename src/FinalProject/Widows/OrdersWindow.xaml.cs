using FinalProject.Helper;
using FinalProject.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FinalProject.Windows
{
    public partial class OrdersWindow: Window
    {
        public ObservableCollection<orders> Orders { get; set; }
        public ICollectionView OrdersCollection { get; set; }
        public List<string> CategoryOptions { get; set; }
        public List<string> MaterialForSearch { get; set; }
        private AppData appata { get; set; }
        public OrdersWindow(AppData data)
        {
            InitializeComponent();
            appata = data;
            if (appata.Orders == null)
            {
                appata.Orders = new ObservableCollection<orders>();
            }
            Orders = appata.Orders;
            OrdersCollection = CollectionViewSource.GetDefaultView(Orders);
            CategoryOptions=appata.Orders.Select(o=>o.Category).Distinct().ToList();
            DataContext = this;
            UpdateMaterialList();
            UpdateSummary();
        }

        #region filters event
        private void ApplyFilter()
        {
            if (OrdersCollection == null) return;

            OrdersCollection.Filter = (item) =>
            {
                var order = item as orders;
                if (order == null) return false;
                // 1. Status filter
                bool statusMatch = true;
                if (StatusFilter.SelectedItem is ComboBoxItem statusItem
                    && statusItem.Content.ToString() != "All")
                {
                    statusMatch = string.Equals(order.Status,statusItem.Content.ToString(),System.StringComparison.OrdinalIgnoreCase);
                }
                // 2. Category filter
                bool categoryMatch = true;
                if (CategoryFilter.SelectedItem is string selectedCategory)
                {
                    categoryMatch = order.Category == selectedCategory;
                }

                // 3. Material filter
                bool materialMatch = true;
                if (MaterialFilter.SelectedItem is string selectedMaterial)
                {
                    materialMatch = order.MaterialName == selectedMaterial;
                }
                //Search filter
                bool searchMatch = true;
                if (SearchBox != null && !string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    searchMatch = order.MaterialName.ToLower().Contains(SearchBox.Text.ToLower().Trim())
                                      ||order.Category.ToLower().Contains(SearchBox.Text.ToLower().Trim())
                                      ||order.OrderNumber.Equals(SearchBox.Text.Trim());
                }

                return statusMatch&&categoryMatch&&materialMatch&&searchMatch; ;
            };

            UpdateSummary();
        }
        private void Filter(object sender, System.EventArgs e)
        {
            ApplyFilter();
            if (sender==CategoryFilter)
                UpdateMaterialList();
        }
        private void UpdateMaterialList()
        {
            var currentMaterial = MaterialFilter.SelectedItem?.ToString();
            MaterialForSearch = OrdersCollection.Cast<orders>().Select(o => o.MaterialName).Distinct().ToList();
            MaterialFilter.ItemsSource = MaterialForSearch;
            if (currentMaterial != null && MaterialForSearch.Contains(currentMaterial))
                MaterialFilter.SelectedItem = currentMaterial;
            else
                MaterialFilter.SelectedItem = null;
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }
        #endregion

        #region updates
        private void UpdateSummary()
        {
            if (TotalOrdersText == null || GrandTotalText == null)
                return;
            var filteredOrders = OrdersCollection.Cast<orders>().ToList();
            TotalOrdersText.Text = filteredOrders.Count.ToString();

            double grandTotal = filteredOrders.Sum(o => o.Total);
            GrandTotalText.Text = $"{grandTotal:N0} EGP";

            var pendingOrders = filteredOrders.Where(o => o.Status == "Pending").ToList();
            PendingOrdersText.Text = pendingOrders.Count.ToString();
        }
        #endregion

        #region buttons
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            StatusFilter.SelectedIndex = 0;
            CategoryFilter.SelectedItem = null;
            MaterialFilter.SelectedItem = null;
            SearchBox.Text = string.Empty;  
            OrdersCollection.Filter = null;
            UpdateMaterialList();
            UpdateSummary();
        }
        private void MarkDelivered_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersDataGrid.SelectedItem as orders;
            if (selectedOrder==null)
            {
                MessageBox.Show("Please select an order first.", "noSelection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (selectedOrder.Status=="Delivered")
            {
                MessageBox.Show("This order is already delivered.", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                selectedOrder.Status ="Delivered";
                OrdersCollection.Refresh();
                Helper.Helper.savetojison(appata);
                UpdateSummary();
            }
        }
        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersDataGrid.SelectedItem as orders;
            if (selectedOrder ==null)
            {
                MessageBox.Show("Please select an order first.","noSelection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show($"Are you sure you want to delete Order #{selectedOrder.OrderNumber}?",
            "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result==MessageBoxResult.Yes)
            {
                Orders.Remove(selectedOrder);
                Helper.Helper.savetojison(appata);
                UpdateSummary();
            }
        }
        private void Export_Summary(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = "Orders.csv"
            };

            if (dialog.ShowDialog() == true) //open window to save file..save(true)
            {
                string filePath = dialog.FileName;              
                while (System.IO.Path.GetExtension(filePath) != ".csv")
                {
                    MessageBox.Show("Please select a CSV file format only.","Invalid File Type", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var records = OrdersCollection.Cast<orders>().ToList(); 
                using (var writer = new System.IO.StreamWriter(filePath)) //to write at file
                {
                    writer.WriteLine("OrderNumber,MaterialName,Category,ElementType,Quantity,Unit,UnitPrice,Total,Date,Status");
                    // Rows
                    foreach (var order in records)
                    {
                    writer.WriteLine($"{order.OrderNumber},{order.MaterialName},{order.Category},{order.elementtype},{order.Quantity},{order.Unit},{order.UnitPrice},{order.Total},{order.Date:dd/MM/yyyy},{order.Status}");
                    }
                }
                MessageBox.Show("Export completed successfully!","Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

       
    }
}