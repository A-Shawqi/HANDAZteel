using HANDAZ.Entities;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HANDAZ.PEB.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_GenerateIfc_Click(object sender, RoutedEventArgs e)
        {
            HndzSite site = new HndzSite();
            HndzBuilding building = new HndzBuilding();
            HndzStorey storey = new HndzStorey( building);
            //HndzProduct product = new HndzProduct(ref storey);
            HndzNode node1 = new HndzNode();
            HndzNode node4 = new HndzNode();
            HndzNode node5 = new HndzNode();
            HndzNode node2 = new HndzNode(2,3,5);
            HndzNode ss = new HndzNode();
        }
    }
}
