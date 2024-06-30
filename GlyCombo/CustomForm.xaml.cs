using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using glycombo;

namespace GlyCombo
{
    /// <summary>
    /// Interaction logic for CustomForm.xaml
    /// </summary>
    public partial class CustomForm : Window
    {
        int customMono = 0;
        private string customContent = "Overview:" + Environment.NewLine +
            "Monosaccharides are described as their mass/chemical formula as if linked to other sugars (e.g. glucose is 162.0528 and C6H10O5) and the provided monoisotopic mass should reflect this.";

        private MainViewModel ViewModel;

        public CustomForm(MainViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            customText.Text = customContent;
            customMonoNameBox1.Visibility = Visibility.Collapsed;
            customMonoMassBox1.Visibility = Visibility.Collapsed;
            customMonoCBox1.Visibility = Visibility.Collapsed;
            customMonoHBox1.Visibility = Visibility.Collapsed;
            customMonoNBox1.Visibility = Visibility.Collapsed;
            customMonoOBox1.Visibility = Visibility.Collapsed;
            customMonoMinBox1.Visibility = Visibility.Collapsed;
            customMonoMaxBox1.Visibility = Visibility.Collapsed;
            customMonoNameBox2.Visibility = Visibility.Collapsed;
            customMonoMassBox2.Visibility = Visibility.Collapsed;
            customMonoCBox2.Visibility = Visibility.Collapsed;
            customMonoHBox2.Visibility = Visibility.Collapsed;
            customMonoNBox2.Visibility = Visibility.Collapsed;
            customMonoOBox2.Visibility = Visibility.Collapsed;
            customMonoMinBox2.Visibility = Visibility.Collapsed;
            customMonoMaxBox2.Visibility = Visibility.Collapsed;
            customMonoNameBox3.Visibility = Visibility.Collapsed;
            customMonoMassBox3.Visibility = Visibility.Collapsed;
            customMonoCBox3.Visibility = Visibility.Collapsed;
            customMonoHBox3.Visibility = Visibility.Collapsed;
            customMonoNBox3.Visibility = Visibility.Collapsed;
            customMonoOBox3.Visibility = Visibility.Collapsed;
            customMonoMinBox3.Visibility = Visibility.Collapsed;
            customMonoMaxBox3.Visibility = Visibility.Collapsed;
            customMonoNameBox4.Visibility = Visibility.Collapsed;
            customMonoMassBox4.Visibility = Visibility.Collapsed;
            customMonoCBox4.Visibility = Visibility.Collapsed;
            customMonoHBox4.Visibility = Visibility.Collapsed;
            customMonoNBox4.Visibility = Visibility.Collapsed;
            customMonoOBox4.Visibility = Visibility.Collapsed;
            customMonoMinBox4.Visibility = Visibility.Collapsed;
            customMonoMaxBox4.Visibility = Visibility.Collapsed;
            customMonoNameBox5.Visibility = Visibility.Collapsed;
            customMonoMassBox5.Visibility = Visibility.Collapsed;
            customMonoCBox5.Visibility = Visibility.Collapsed;
            customMonoHBox5.Visibility = Visibility.Collapsed;
            customMonoNBox5.Visibility = Visibility.Collapsed;
            customMonoOBox5.Visibility = Visibility.Collapsed;
            customMonoMinBox5.Visibility = Visibility.Collapsed;
            customMonoMaxBox5.Visibility = Visibility.Collapsed;
        }

        public void customMonoCheck1_Checked(object sender, RoutedEventArgs e)
        {
            customMonoNameBox1.Visibility = Visibility.Visible;
            customMonoMassBox1.Visibility = Visibility.Visible;
            customMonoCBox1.Visibility = Visibility.Visible;
            customMonoHBox1.Visibility = Visibility.Visible;
            customMonoNBox1.Visibility = Visibility.Visible;
            customMonoOBox1.Visibility = Visibility.Visible;
            customMonoMinBox1.Visibility = Visibility.Visible;
            customMonoMaxBox1.Visibility = Visibility.Visible;
            customMono += 1;
            if (customMono > 0) { ViewModel.ButtonText = "Custom (Enabled)"; }
            else { ViewModel.ButtonText = "Custom"; }
        }

        public void customMonoCheck1_Unchecked(object sender, RoutedEventArgs e)
        {
            customMonoNameBox1.Visibility = Visibility.Collapsed;
            customMonoMassBox1.Visibility = Visibility.Collapsed;
            customMonoCBox1.Visibility = Visibility.Collapsed;
            customMonoHBox1.Visibility = Visibility.Collapsed;
            customMonoNBox1.Visibility = Visibility.Collapsed;
            customMonoOBox1.Visibility = Visibility.Collapsed;
            customMonoMinBox1.Visibility = Visibility.Collapsed;
            customMonoMaxBox1.Visibility = Visibility.Collapsed;
            customMono += -1;
            if (customMono > 0) { ViewModel.ButtonText = "Custom (Enabled)"; }
            else { ViewModel.ButtonText = "Custom"; }
        }

        private void customSettingsSave_Click(object sender, RoutedEventArgs e)
        {

            string SaveOutput = "## GlyCombo Custom Monosaccharide Settings" + Environment.NewLine + "## <CustomMono#>, Custom Name, Custom Mass, # of Carbons, # of Hydrogens, # of Nitrogens, # of Oxygens, Minimum count, Maximum count" + Environment.NewLine;
            if (customMonoCheck1.IsChecked == true)
            {
                SaveOutput += "<CustomMono1>" + "," + customMonoNameBox1.Text + "," + customMonoMassBox1.Text + "," + customMonoCBox1.Text + "," + customMonoHBox1.Text + "," + customMonoNBox1.Text+ "," + customMonoOBox1.Text+ "," + customMonoMinBox1.Text+ "," + customMonoMaxBox1.Text+ Environment.NewLine;
            }
            if (customMonoCheck2.IsChecked == true)
            {
                SaveOutput += "<CustomMono2>" + "," + customMonoNameBox2.Text + "," + customMonoMassBox2.Text + "," + customMonoCBox2.Text + "," + customMonoHBox2.Text+ "," + customMonoNBox2.Text+ "," + customMonoOBox2.Text+ "," + customMonoMinBox2.Text+ "," + customMonoMaxBox2.Text+ Environment.NewLine;
            }
            if (customMonoCheck3.IsChecked == true)
            {
                SaveOutput += "<CustomMono3>" + "," + customMonoNameBox3.Text + "," + customMonoMassBox3.Text + "," + customMonoCBox3.Text + "," + customMonoHBox3.Text+ "," + customMonoNBox3.Text+ "," + customMonoOBox3.Text+ "," + customMonoMinBox3.Text+ "," + customMonoMaxBox3.Text+ Environment.NewLine;
            }
            if (customMonoCheck4.IsChecked == true)
            {
                SaveOutput += "<CustomMono4>" + "," + customMonoNameBox4.Text + "," + customMonoMassBox4.Text + "," + customMonoCBox4.Text + "," + customMonoHBox4.Text+ "," + customMonoNBox4.Text+ "," + customMonoOBox4.Text+ "," + customMonoMinBox4.Text+ "," + customMonoMaxBox4.Text+ Environment.NewLine;
            }
            if (customMonoCheck5.IsChecked == true)
            {
                SaveOutput += "<CustomMono5>" + "," + customMonoNameBox5.Text + "," + customMonoMassBox5.Text + "," + customMonoCBox5.Text + "," + customMonoHBox5.Text+ "," + customMonoNBox5.Text+ "," + customMonoOBox5.Text+ "," + customMonoMinBox5.Text+ "," + customMonoMaxBox5.Text+ Environment.NewLine;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                DefaultExt = "txt",
                Title = "Save Text File"
            };
            // Show dialog and check if the result is OK
            if (saveFileDialog.ShowDialog() == true)
            {
                // Write the content to the selected file
                File.WriteAllText(saveFileDialog.FileName, SaveOutput);
            }
        }

        private void customSettingsLoad_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user which text file they want to return settings from
            OpenFileDialog openFileDialogCustom = new()
            {
                Filter = "Text files (txt)|*.txt",
            };

            if (openFileDialogCustom.ShowDialog() == true)
            {
                //Open streamreader to read each line
                StreamReader reader = new(openFileDialogCustom.FileName);
                string line;
                for (int i = 0; i < 8; i++)
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        if (line.Contains("<CustomMono1>"))
                        {
                            string[] MonoSplitLine1 = line.Split(',');
                            customMonoNameBox1.Text = MonoSplitLine1[1];
                            customMonoMassBox1.Text = MonoSplitLine1[2];
                            customMonoCBox1.Text = MonoSplitLine1[3];
                            customMonoHBox1.Text = MonoSplitLine1[4];
                            customMonoNBox1.Text = MonoSplitLine1[5];
                            customMonoOBox1.Text = MonoSplitLine1[6];
                            customMonoMinBox1.Text = MonoSplitLine1[7];
                            customMonoMaxBox1.Text = MonoSplitLine1[8];
                            customMonoCheck1.IsChecked = true;
                        }
                        if (line.Contains("<CustomMono2>"))
                        {
                            string[] MonoSplitLine2 = line.Split(',');
                            customMonoNameBox2.Text = MonoSplitLine2[1];
                            customMonoMassBox2.Text = MonoSplitLine2[2];
                            customMonoCBox2.Text = MonoSplitLine2[3];
                            customMonoHBox2.Text = MonoSplitLine2[4];
                            customMonoNBox2.Text = MonoSplitLine2[5];
                            customMonoOBox2.Text = MonoSplitLine2[6];
                            customMonoMinBox2.Text = MonoSplitLine2[7];
                            customMonoMaxBox2.Text = MonoSplitLine2[8];
                            customMonoCheck2.IsChecked = true;
                        }
                        if (line.Contains("<CustomMono3>"))
                        {
                            string[] MonoSplitLine3 = line.Split(',');
                            customMonoNameBox3.Text = MonoSplitLine3[1];
                            customMonoMassBox3.Text = MonoSplitLine3[2];
                            customMonoCBox3.Text = MonoSplitLine3[3];
                            customMonoHBox3.Text = MonoSplitLine3[4];
                            customMonoNBox3.Text = MonoSplitLine3[5];
                            customMonoOBox3.Text = MonoSplitLine3[6];
                            customMonoMinBox3.Text = MonoSplitLine3[7];
                            customMonoMaxBox3.Text = MonoSplitLine3[8];
                            customMonoCheck3.IsChecked = true;
                        }
                        if (line.Contains("<CustomMono4>"))
                        {
                            string[] MonoSplitLine4 = line.Split(',');
                            customMonoNameBox4.Text = MonoSplitLine4[1];
                            customMonoMassBox4.Text = MonoSplitLine4[2];
                            customMonoCBox4.Text = MonoSplitLine4[3];
                            customMonoHBox4.Text = MonoSplitLine4[4];
                            customMonoNBox4.Text = MonoSplitLine4[5];
                            customMonoOBox4.Text = MonoSplitLine4[6];
                            customMonoMinBox4.Text = MonoSplitLine4[7];
                            customMonoMaxBox4.Text = MonoSplitLine4[8];
                            customMonoCheck4.IsChecked = true;
                        }
                        if (line.Contains("<CustomMono5>"))
                        {
                            string[] MonoSplitLine5 = line.Split(',');
                            customMonoNameBox5.Text = MonoSplitLine5[1];
                            customMonoMassBox5.Text = MonoSplitLine5[2];
                            customMonoCBox5.Text = MonoSplitLine5[3];
                            customMonoHBox5.Text = MonoSplitLine5[4];
                            customMonoNBox5.Text = MonoSplitLine5[5];
                            customMonoOBox5.Text = MonoSplitLine5[6];
                            customMonoMinBox5.Text = MonoSplitLine5[7];
                            customMonoMaxBox5.Text = MonoSplitLine5[8];
                            customMonoCheck5.IsChecked = true;
                        }
                    }
                    else { return; }
                }
            }
            else
            {
                return;
            }

        }

        public void customMonoCheck3_Unchecked(object sender, RoutedEventArgs e)
        {
            customMonoNameBox3.Visibility = Visibility.Collapsed;
            customMonoMassBox3.Visibility = Visibility.Collapsed;
            customMonoCBox3.Visibility = Visibility.Collapsed;
            customMonoHBox3.Visibility = Visibility.Collapsed;
            customMonoNBox3.Visibility = Visibility.Collapsed;
            customMonoOBox3.Visibility = Visibility.Collapsed;
            customMonoMinBox3.Visibility = Visibility.Collapsed;
            customMonoMaxBox3.Visibility = Visibility.Collapsed;
            customMono += -1;
            if (customMono > 0) { ViewModel.ButtonText = "Custom (Enabled)"; }
            else { ViewModel.ButtonText = "Custom"; }
        }

        public void customMonoCheck2_Unchecked(object sender, RoutedEventArgs e)
        {
            customMonoNameBox2.Visibility = Visibility.Collapsed;
            customMonoMassBox2.Visibility = Visibility.Collapsed;
            customMonoCBox2.Visibility = Visibility.Collapsed;
            customMonoHBox2.Visibility = Visibility.Collapsed;
            customMonoNBox2.Visibility = Visibility.Collapsed;
            customMonoOBox2.Visibility = Visibility.Collapsed;
            customMonoMinBox2.Visibility = Visibility.Collapsed;
            customMonoMaxBox2.Visibility = Visibility.Collapsed;
            customMono += -1;
            if (customMono > 0) { ViewModel.ButtonText = "Custom (Enabled)"; }
            else { ViewModel.ButtonText = "Custom"; }
        }

        public void customMonoCheck4_Unchecked(object sender, RoutedEventArgs e)
        {
            customMonoNameBox4.Visibility = Visibility.Collapsed;
            customMonoMassBox4.Visibility = Visibility.Collapsed;
            customMonoCBox4.Visibility = Visibility.Collapsed;
            customMonoHBox4.Visibility = Visibility.Collapsed;
            customMonoNBox4.Visibility = Visibility.Collapsed;
            customMonoOBox4.Visibility = Visibility.Collapsed;
            customMonoMinBox4.Visibility = Visibility.Collapsed;
            customMonoMaxBox4.Visibility = Visibility.Collapsed;
            customMono += -1;
            if (customMono > 0) { ViewModel.ButtonText = "Custom (Enabled)"; }
            else { ViewModel.ButtonText = "Custom"; }
        }

        public void customMonoCheck5_Unchecked(object sender, RoutedEventArgs e)
        {
            customMonoNameBox5.Visibility = Visibility.Collapsed;
            customMonoMassBox5.Visibility = Visibility.Collapsed;
            customMonoCBox5.Visibility = Visibility.Collapsed;
            customMonoHBox5.Visibility = Visibility.Collapsed;
            customMonoNBox5.Visibility = Visibility.Collapsed;
            customMonoOBox5.Visibility = Visibility.Collapsed;
            customMonoMinBox5.Visibility = Visibility.Collapsed;
            customMonoMaxBox5.Visibility = Visibility.Collapsed;
            customMono += -1;
            if (customMono > 0) { ViewModel.ButtonText = "Custom (Enabled)"; }
            else { ViewModel.ButtonText = "Custom"; }
        }

        public void customMonoCheck2_Checked(object sender, RoutedEventArgs e)
        {
            customMonoNameBox2.Visibility = Visibility.Visible;
            customMonoMassBox2.Visibility = Visibility.Visible;
            customMonoCBox2.Visibility = Visibility.Visible;
            customMonoHBox2.Visibility = Visibility.Visible;
            customMonoNBox2.Visibility = Visibility.Visible;
            customMonoOBox2.Visibility = Visibility.Visible;
            customMonoMinBox2.Visibility = Visibility.Visible;
            customMonoMaxBox2.Visibility = Visibility.Visible;
            customMono += 1;
            if (customMono > 0) { ViewModel.ButtonText = "Custom (Enabled)"; }
            else { ViewModel.ButtonText = "Custom"; }
        }

        public void customMonoCheck3_Checked(object sender, RoutedEventArgs e)
        {
            customMonoNameBox3.Visibility = Visibility.Visible;
            customMonoMassBox3.Visibility = Visibility.Visible;
            customMonoCBox3.Visibility = Visibility.Visible;
            customMonoHBox3.Visibility = Visibility.Visible;
            customMonoNBox3.Visibility = Visibility.Visible;
            customMonoOBox3.Visibility = Visibility.Visible;
            customMonoMinBox3.Visibility = Visibility.Visible;
            customMonoMaxBox3.Visibility = Visibility.Visible;
            customMono += 1;
            if (customMono > 0) { ViewModel.ButtonText = "Custom (Enabled)"; }
            else { ViewModel.ButtonText = "Custom"; }
        }

        public void customMonoCheck4_Checked(object sender, RoutedEventArgs e)
        {
            customMonoNameBox4.Visibility = Visibility.Visible;
            customMonoMassBox4.Visibility = Visibility.Visible;
            customMonoCBox4.Visibility = Visibility.Visible;
            customMonoHBox4.Visibility = Visibility.Visible;
            customMonoNBox4.Visibility = Visibility.Visible;
            customMonoOBox4.Visibility = Visibility.Visible;
            customMonoMinBox4.Visibility = Visibility.Visible;
            customMonoMaxBox4.Visibility = Visibility.Visible;
            customMono += 1;
            if (customMono > 0) { ViewModel.ButtonText = "Custom (Enabled)"; }
            else { ViewModel.ButtonText = "Custom"; }
        }

        public void customMonoCheck5_Checked(object sender, RoutedEventArgs e)
        {
            customMonoNameBox5.Visibility = Visibility.Visible;
            customMonoMassBox5.Visibility = Visibility.Visible;
            customMonoCBox5.Visibility = Visibility.Visible;
            customMonoHBox5.Visibility = Visibility.Visible;
            customMonoNBox5.Visibility = Visibility.Visible;
            customMonoOBox5.Visibility = Visibility.Visible;
            customMonoMinBox5.Visibility = Visibility.Visible;
            customMonoMaxBox5.Visibility = Visibility.Visible;
            customMono += 1;
            if (customMono > 0) { ViewModel.ButtonText = "Custom (Enabled)"; }
            else { ViewModel.ButtonText = "Custom"; }
        }
    }
}
