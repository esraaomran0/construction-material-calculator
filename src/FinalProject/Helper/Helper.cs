using FinalProject.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Controls;

namespace FinalProject.Helper
{
   

    public class Helper
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "materialss.json");
        public static void savetojison(AppData data)
        {
            string dataConverted = JsonConvert.SerializeObject(data,Formatting.Indented); //take c# to string
            File.WriteAllText(filePath, dataConverted); //convert to jison
        }
        public static AppData LoadFromJson()
        {
            if (!File.Exists(filePath))
            return new AppData();
            string jsonData = File.ReadAllText(filePath); //read
            return JsonConvert.DeserializeObject<AppData>(jsonData) ?? new AppData(); //from string to c#
        }
        public static double checkvalue(TextBox textBox)
        {
            double.TryParse(textBox.Text, out double value);
            return value;
        }
        
    }
}
