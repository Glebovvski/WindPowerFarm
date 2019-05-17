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
using System.Windows.Shapes;

namespace WindEnergy.Windows
{
    /// <summary>
    /// Interaction logic for CompareGeneratorsStats.xaml
    /// </summary>
    public partial class CompareGeneratorsStats : Window
    {
        private WindGenerator gen1;
        private WindGenerator gen2;
        MainWindow main;
        public CompareGeneratorsStats()
        {
            InitializeComponent();
            main = (MainWindow)Application.Current.MainWindow;
            Compare();
            cb1.ItemsSource = main.vm.WindGenerators.Select(x => x.Name + " (" + x.RatedPower + "W)");
            cb1.SelectedIndex = 0;
            gen1 = main.vm.WindGenerators.Where(x => x.Id == main.vm.WindGenerators.ToList()[cb1.SelectedIndex].Id).FirstOrDefault();

            cb2.ItemsSource = main.vm.WindGenerators.Select(x => x.Name + " (" + x.RatedPower + "W)");
            cb2.SelectedIndex = 0;
            gen2 = main.vm.WindGenerators.Where(x => x.Id == main.vm.WindGenerators.ToList()[cb2.SelectedIndex].Id).FirstOrDefault();
        }

        private void Compare()
        {
            foreach (var item in grid.Children)
            {
                if (item is TextBlock)
                    ((TextBlock)item).Foreground = new SolidColorBrush(Colors.Black);
            }
            Brush better = new SolidColorBrush(Colors.Green);
            if (gen1 != null && gen2 != null)
            {
                name1.Text = gen1.Name;
                name2.Text = gen2.Name;

                ratedPower1.Text = gen1.RatedPower.ToString();
                ratedPower2.Text = gen2.RatedPower.ToString();
                if (gen1.RatedPower >= gen2.RatedPower)
                    ratedPower1.Foreground = better;
                else
                    ratedPower2.Foreground = better;

                ratedWind1.Text = gen1.RatedWindSpeed.ToString();
                ratedWind2.Text = gen2.RatedWindSpeed.ToString();

                windSpeedRange1.Text = gen1.MinWindSpeed + " - " + gen1.MaxWindSpeed;
                windSpeedRange2.Text = gen2.MinWindSpeed + " - " + gen2.MaxWindSpeed;

                radius1.Text = gen1.Radius == null ? "N/A" : ((double)gen1.Radius).ToString("0.00");
                radius2.Text = gen2.Radius == null ? "N/A" : ((double)gen2.Radius).ToString("0.00");
                if (gen1.Radius >= gen2.Radius && gen1.Radius != null && gen2.Radius != null)
                    radius1.Foreground = better;
                else
                    radius2.Foreground = better;

                sweptArea1.Text = gen1.SweptArea.ToString();
                sweptArea2.Text = gen2.SweptArea.ToString();
                if (gen1.SweptArea >= gen2.SweptArea)
                    sweptArea1.Foreground = better;
                else
                    sweptArea2.Foreground = better;

                height1.Text = gen1.Height.ToString();
                height2.Text = gen2.Height.ToString();
                if (gen1.Height >= gen2.Height)
                    height1.Foreground = better;
                else
                    height2.Foreground = better;

                price1.Text = gen1.Price.ToString();
                price2.Text = gen2.Price.ToString();
                if (gen1.Price <= gen2.Price && gen2.Price > 0 && gen1.Price > 0)
                    price1.Foreground = better;
                else if(gen2.Price < gen1.Price && gen2.Price > 0 && gen1.Price > 0)
                    price2.Foreground = better;
            }
        }

        private void Cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gen1 = main.vm.WindGenerators.Where(x => x.Id == main.vm.WindGenerators.ToList()[cb1.SelectedIndex].Id).FirstOrDefault();
            Compare();
        }

        private void Cb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gen2 = main.vm.WindGenerators.Where(x => x.Id == main.vm.WindGenerators.ToList()[cb2.SelectedIndex].Id).FirstOrDefault();
            Compare();
        }
    }
}
