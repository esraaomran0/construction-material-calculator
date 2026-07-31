using System.Collections.ObjectModel;

namespace FinalProject.Models
{
    public class AppData
    {
        public ObservableCollection<material> Materials { get; set; }
        public ObservableCollection<orders> Orders { get; set; }
    
    }
}
