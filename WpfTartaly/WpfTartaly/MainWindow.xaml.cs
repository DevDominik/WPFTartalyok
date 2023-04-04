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
using System.IO;
using Osztályok;

namespace WpfTartaly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Tartaly> tartalyok = new List<Tartaly>();
        public MainWindow()
        {
            InitializeComponent();
            rdoTeglatest.IsChecked = true;

        }

        private void rdoKocka_Checked(object sender, RoutedEventArgs e)
        {
            txtAel.IsEnabled = false;
            txtBel.IsEnabled = false;
            txtCel.IsEnabled = false;
            txtAel.Text = "10";
            txtBel.Text = "10";
            txtCel.Text = "10";
        }

        private void rdoTeglatest_Checked(object sender, RoutedEventArgs e)
        {
            txtAel.Text = "";
            txtBel.Text = "";
            txtCel.Text = "";
            txtAel.IsEnabled = true;
            txtBel.IsEnabled = true;
            txtCel.IsEnabled = true;
        }

        private void btnFelvesz_Click(object sender, RoutedEventArgs e)
        {
            if (txtNev.Text.Length >= 1 && txtAel.Text.Length >= 1 && txtBel.Text.Length >= 1 && txtCel.Text.Length >= 1)
            {
                try
                {
                    Tartaly ujtartaly = new Tartaly(txtNev.Text, int.Parse(txtAel.Text), int.Parse(txtBel.Text), int.Parse(txtCel.Text));
                    tartalyok.Add(ujtartaly);
                    lbTartalyok.Items.Add(ujtartaly.Info());
                }
                catch (FormatException)
                {

                    MessageBox.Show("Nem helyes adattípus (szöveg helyett számra van szükség az éleknél)");
                }
            }
            else
            {
                MessageBox.Show("Név és/vagy él mezőknél nincs adat megadva");
            }
        }

        private bool VanElemKivalasztva()
        {
            bool kivalasztva = false;
            if (lbTartalyok.SelectedIndex >= 0)
            {
                kivalasztva = true;
            }
            else
            {
                MessageBox.Show("Nincs kiválaszva elem a listában");
            }
            return kivalasztva;
        }

        private void ListaUjraEpit()
        {
            lbTartalyok.Items.Clear();
            foreach (Tartaly tartaly in tartalyok)
            {
                lbTartalyok.Items.Add(tartaly.Info());
            }
        }

        private void btnDuplaz_Click(object sender, RoutedEventArgs e)
        {
            if (VanElemKivalasztva())
            {
                tartalyok[lbTartalyok.SelectedIndex].DuplazMeretet();
                ListaUjraEpit();
            }
        }

        private void btnLeenged_Click(object sender, RoutedEventArgs e)
        {
            if (VanElemKivalasztva())
            {
                tartalyok[lbTartalyok.SelectedIndex].TeljesLeengedes();
                ListaUjraEpit();
            }
        }

        private void btntolt_Click(object sender, RoutedEventArgs e)
        {
            if (VanElemKivalasztva())
            {
                try
                {
                    try
                    {
                        tartalyok[lbTartalyok.SelectedIndex].Tolt(int.Parse(txtMennyitTolt.Text));
                        ListaUjraEpit();
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show($"Nem fér bele ennyi: {txtMennyitTolt.Text}; max. {tartalyok[lbTartalyok.SelectedIndex].Terfogat / 1000} liter, jelenleg {tartalyok[lbTartalyok.SelectedIndex].AktLiter} liter van benne");
                        txtMennyitTolt.Focus();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Nem helyes adattípus (szöveg helyett szám kell)");
                    txtMennyitTolt.Focus();
                }
            }
        }

        private void btnRogzit_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sr = new StreamWriter("adatok.csv", true);
            foreach (Tartaly tartaly in tartalyok)
            {
                sr.WriteLine($"{tartaly.Nev};{tartaly.aEl};{tartaly.bEl};{tartaly.cEl};{tartaly.AktLiter}");
            }
            sr.Close();
            MessageBox.Show("Sikeres mentés");
        }
    }
}