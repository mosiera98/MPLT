using System.Data;
using System.Windows;

namespace SciChartExport
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataTable wpf_dt_asciinfo = new DataTable();
        
        public MainWindow()
        {
            InitializeComponent();
            
        }
    }
}
