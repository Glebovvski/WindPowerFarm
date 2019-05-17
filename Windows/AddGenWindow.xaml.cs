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

namespace WindEnergy
{
    /// <summary>
    /// Interaction logic for AddGenWindow.xaml
    /// </summary>
    public partial class AddGenWindow : Window
    {
        WindGenContext context;

        bool correct = false;

        string name;
        double ratedPower;
        double ratedWindSpeed;
        double maxWindSpeed;
        double minWindSpeed;
        double rotorRadius;
        double sweptArea;
        int height;
        decimal price = 0;

        MainWindow main;
        public AddGenWindow()
        {
            InitializeComponent();
            main = (MainWindow)Application.Current.MainWindow;
            context = new WindGenContext();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CheckValues();
            if (correct)
            {
                WindGenerator generator = new WindGenerator()
                {
                    IsWorking = true,
                    MaxWindSpeed = maxWindSpeed,
                    MinWindSpeed = minWindSpeed,
                    Name = name,
                    Radius = rotorRadius,
                    RatedPower = ratedPower,
                    RatedWindSpeed = ratedWindSpeed,
                    SweptArea = sweptArea,
                    Height = height == 0 ? 10 : height,
                    Price = price
                };
                main.vm.AddWindGenerator(generator);
                main.RefreshWindList();
                this.Close();
            }
        }

        public void CheckValues()
        {
            bool success;
            if (context.WindGenerators.Any(x => x.Name == nameTB.Text))
            {
                MessageBox.Show("Wind Generator with this name already exists in the list");
                return;
            }
            else name = nameTB.Text;

            success = double.TryParse(ratedPowerTB.Text, out ratedPower);
            if (!success)
            {
                MessageBox.Show("Please enter valid Rated Power value");
                return;
            }
            else if (ratedPower < 0)
            {
                MessageBox.Show("Rated Power value can not be negative number");
                return;
            }

            success = double.TryParse(ratedWindSpeedTB.Text, out ratedWindSpeed);
            if (!success)
            {
                MessageBox.Show("Please enter valid Rated Wind Speed value");
                return;
            }
            else if (ratedWindSpeed < 0)
            {
                MessageBox.Show("Rated Wind Speed value can not be negative number");
                return;
            }

            success = double.TryParse(maxWindSpeedTB.Text, out maxWindSpeed);
            if (!success)
            {
                MessageBox.Show("Please enter valid Max. Working Wind Speed value");
                return;
            }
            else if (maxWindSpeed < 0)
            {
                MessageBox.Show("Max Wind Speed value can not be negative number");
                return;
            }

            success = double.TryParse(minWindSpeedTB.Text, out minWindSpeed);
            if (!success)
            {
                MessageBox.Show("Please enter valid Starting Wind Speed Value");
                return;
            }
            else if (minWindSpeed < 0)
            {
                MessageBox.Show("Min Wind Speed value can not be negative number");
                return;
            }

            if (rotorRadiusTB.Text == string.Empty && sweptAreaTB.Text == string.Empty)
            {
                MessageBox.Show("Either Rotor Radius value or Swept Area value must be entered");
                return;
            }

            if (rotorRadiusTB.Text != string.Empty)
            {
                success = double.TryParse(rotorRadiusTB.Text, out rotorRadius);
                if (!success)
                {
                    MessageBox.Show("Please enter valid Rotor Radius value");
                    return;
                }
                else if (rotorRadius < 0)
                {
                    MessageBox.Show("Rotor radius value can not be negative number");
                    return;
                }
            }

            if (sweptAreaTB.Text != string.Empty)
            {
                success = double.TryParse(sweptAreaTB.Text, out sweptArea);
                if (!success)
                {
                    MessageBox.Show("Please enter valid Swept Area value");
                    return;
                }
                else if (sweptArea < 0)
                {
                    MessageBox.Show("Swept Area value can not be negative number");
                    return;
                }
            }

            success = int.TryParse(heightTB.Text, out height);
            if (!success)
            {
                MessageBox.Show("Please enter valid Height value");
            }
            else if (height < 0)
            {
                MessageBox.Show("Height value can not be negative number");
                return;
            }

            if (priceTB.Text != string.Empty)
            {
                success = decimal.TryParse(priceTB.Text, out price);
                if (!success)
                {
                    MessageBox.Show("Please enter valid Price value");
                    return;
                }
                else if(price<0)
                {
                    MessageBox.Show("Price value can not be negative number");
                    return;
                }
            }

            correct = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var item in grid.Children)
            {
                if (item is TextBox)
                    ((TextBox)item).Text = "";
            }
        }
    }
}
