using Microsoft.Win32;
using ModernWpf.Controls;
using System.ComponentModel;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System;
using System.Configuration;
using Windows.Media.Capture;
using System.Collections.Generic;
using GlyCombo;
using System.Reflection.Metadata;
using System.Collections;
using System.Security.Policy;

namespace glycombo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private decimal errorTol;
        private string solutionProcess;
        private string solutions;
        private string solutionMultiples = "";
        private int targetsToAdd;
        private int iterations;
        private decimal targetLow;
        private decimal targetHigh;
        private string reducedEnd;
        private decimal observedMass;
        private decimal theoreticalMass;
        private string massErrorType;
        private decimal error;
        // Monosaccharides
        private decimal dhex;
        private decimal hex;
        private decimal hexnac;
        private decimal hexn;
        private decimal hexa;
        private decimal dhexnac;
        private decimal pent;
        private decimal kdn;
        private decimal neuac;
        private decimal neugc;
        private decimal phos;
        private decimal lneuac;
        private decimal eeneuac;
        private decimal dneuac;
        private decimal amneuac;
        private decimal acetyl;
        private decimal lneugc;
        private decimal eeneugc;
        private decimal dneugc;
        private decimal amneugc;
        private decimal sulf;
        // Adducts
        private decimal adductCustom;
        private int searchRepeats;
        List<decimal> targetAdducts;
        List<decimal> targetAdductsProcessing;
        // Monoaccharide ranges
        private int HexMin_int;
        private int HexMax_int;
        private int HexNAcMin_int;
        private int HexNAcMax_int;
        private int dHexMin_int;
        private int dHexMax_int;
        private int HexAMin_int;
        private int HexAMax_int;
        private int HexNMin_int;
        private int HexNMax_int;
        private int PentMin_int;
        private int PentMax_int;
        private int KDNMin_int;
        private int KDNMax_int;
        private int Neu5AcMin_int;
        private int Neu5AcMax_int;
        private int Neu5GcMin_int;
        private int Neu5GcMax_int;
        private int PhosMin_int;
        private int PhosMax_int;
        private int SulfMin_int;
        private int SulfMax_int;
        private int dHexNAcMin_int;
        private int dHexNAcMax_int;
        private int lNeuAcMin_int;
        private int lNeuAcMax_int;
        private int eeNeuAcMin_int;
        private int eeNeuAcMax_int;
        private int dNeuAcMin_int;
        private int dNeuAcMax_int;
        private int amNeuAcMin_int;
        private int amNeuAcMax_int;
        private int acetylMin_int;
        private int acetylMax_int;
        private int lNeuGcMin_int;
        private int lNeuGcMax_int;
        private int eeNeuGcMin_int;
        private int eeNeuGcMax_int;
        private int dNeuGcMin_int;
        private int dNeuGcMax_int;
        private int amNeuGcMin_int;
        private int amNeuGcMax_int;
        private decimal precursor;
        private string line;
        private string[] precursorLine;
        private string polarity;
        private string[] chargeLine;
        private int charge;
        private string[] RTLine;
        private decimal retentionTime;
        private string neutralPrecursorListmzml;
        private string targetString;
        private string scanNumber;
        private string[] scanLine;
        private decimal TIC;
        private string[] TICLine;
        List<decimal> numbers = [];
        List<decimal> scans = [];
        List<int> charges = [];
        List<decimal> retentionTimes = [];
        List<decimal> TICs = [];
        List<string> files = [];
        List<int> targetIndex = [];
        List<decimal> targets = [];
        List<string> targetStrings = [];
        // Parameter report variables
        private bool monoCustom1 = false;
        private bool monoCustom2 = false;
        private bool monoCustom3 = false;
        private bool monoCustom4 = false;
        private bool monoCustom5 = false;
        private string derivatisation;
        private string param_monoCustom1;
        private string param_monoCustom2;
        private string param_monoCustom3;
        private string param_monoCustom4;
        private string param_monoCustom5;
        private string currentMonosaccharideSelection;
        private string currentAdductSelection;
        private string filePath;
        private string inputParameters;
        private float ElapsedMSec;
        // For multiple tasks, enabling progress bar
        private bool DaChecked;
        private string inputChecked = "Text";
        private bool offByOneChecked;
        int customMono = 0;
        private string customContent = "Overview:" + Environment.NewLine +
            "Monosaccharides are described as their mass/chemical formula as if linked to other sugars (e.g. glucose is 162.0528 and C6H10O5) and the provided monoisotopic mass should reflect this.";

        // Custom Monosaccharide enabled checker
        string customMono1Name = "null";
        int customMono1CCount = 0;
        int customMono1HCount = 0;
        int customMono1NCount = 0;
        int customMono1OCount = 0;
        decimal customMono1Mass = 0;
        int customMono1Min = 0;
        int customMono1Max = 0;
        string customMono2Name = "null";
        int customMono2CCount = 0;
        int customMono2HCount = 0;
        int customMono2NCount = 0;
        int customMono2OCount = 0;
        decimal customMono2Mass = 0;
        int customMono2Min = 0;
        int customMono2Max = 0;
        string customMono3Name = "null";
        int customMono3CCount = 0;
        int customMono3HCount = 0;
        int customMono3NCount = 0;
        int customMono3OCount = 0;
        decimal customMono3Mass = 0;
        int customMono3Min = 0;
        int customMono3Max = 0;
        string customMono4Name = "null";
        int customMono4CCount = 0;
        int customMono4HCount = 0;
        int customMono4NCount = 0;
        int customMono4OCount = 0;
        decimal customMono4Mass = 0;
        int customMono4Min = 0;
        int customMono4Max = 0;
        string customMono5Name = "null";
        int customMono5CCount = 0;
        int customMono5HCount = 0;
        int customMono5NCount = 0;
        int customMono5OCount = 0;
        decimal customMono5Mass = 0;
        int customMono5Min = 0;
        int customMono5Max = 0;
        int customReducingCCount = 0;
        int customReducingHCount = 0;
        int customReducingNCount = 0;
        int customReducingOCount = 0;
        decimal customReducingMass = 0;
        string customReducingName = "null";
        string customReducedMassOutput = "null";

        public MainViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            ViewModel.ButtonText = "Custom";
            DataContext = ViewModel;
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

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Taking user to the Github website for support
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        }

        public void customMonoCheck1_Checked(object sender, RoutedEventArgs e)
        {
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
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
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
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
            UpdateMonosaccharideTextBox();
            string saveOutput = "## GlyCombo v0.7 search settings" + Environment.NewLine;
            saveOutput += "<Input> " + inputChecked + Environment.NewLine;
            saveOutput += "<Error tolerance> " + DaError.Text + "," + massErrorType + Environment.NewLine;
            saveOutput += "<Reducing end> " + reducingEndBox.Text + Environment.NewLine;
            if (reducingEndBox.Text == "Custom" && customReducingCheck.IsChecked == true)
            {
                saveOutput += "## Custom reducing end: Name, Mass, #C, #H, #N, #O" + Environment.NewLine;
                saveOutput += "<Custom reducing end> " + customReducingNameBox.Text + "," + customReducingMassBox.Text + "," + customReducingCBox.Text + "," + customReducingHBox.Text + "," + customReducingOBox.Text + "," + customReducingNBox.Text + Environment.NewLine;
            }
            saveOutput += "<Derivatisation> " + derivatisation + Environment.NewLine;
            saveOutput += "<OffByOne enabled> " + offByOneChecked + Environment.NewLine;
            saveOutput += "## Monosaccharides: Monosaccharide1(Min-Max), Monosaccharide2(Min-Max)" + Environment.NewLine;
            saveOutput += currentMonosaccharideSelection + Environment.NewLine;
            if (customMonoCheck1.IsChecked == true || customMonoCheck2.IsChecked == true || customMonoCheck3.IsChecked == true || customMonoCheck4.IsChecked == true || customMonoCheck5.IsChecked == true )
            {
                saveOutput += "## CustomMono#: Name, Mass, #C, #H, #N, #O, Min., Max." + Environment.NewLine;
                if (customMonoCheck1.IsChecked == true)
                {
                    saveOutput += "<CustomMono1> " + customMonoNameBox1.Text + "," + customMonoMassBox1.Text + "," + customMonoCBox1.Text + "," + customMonoHBox1.Text + "," + customMonoNBox1.Text + "," + customMonoOBox1.Text + "," + customMonoMinBox1.Text + "," + customMonoMaxBox1.Text + Environment.NewLine;
                }
                if (customMonoCheck2.IsChecked == true)
                {
                    saveOutput += "<CustomMono2> " + customMonoNameBox2.Text + "," + customMonoMassBox2.Text + "," + customMonoCBox2.Text + "," + customMonoHBox2.Text + "," + customMonoNBox2.Text + "," + customMonoOBox2.Text + "," + customMonoMinBox2.Text + "," + customMonoMaxBox2.Text + Environment.NewLine;
                }
                if (customMonoCheck3.IsChecked == true)
                {
                    saveOutput += "<CustomMono3> " + customMonoNameBox3.Text + "," + customMonoMassBox3.Text + "," + customMonoCBox3.Text + "," + customMonoHBox3.Text + "," + customMonoNBox3.Text + "," + customMonoOBox3.Text + "," + customMonoMinBox3.Text + "," + customMonoMaxBox3.Text + Environment.NewLine;
                }
                if (customMonoCheck4.IsChecked == true)
                {
                    saveOutput += "<CustomMono4> " + customMonoNameBox4.Text + "," + customMonoMassBox4.Text + "," + customMonoCBox4.Text + "," + customMonoHBox4.Text + "," + customMonoNBox4.Text + "," + customMonoOBox4.Text + "," + customMonoMinBox4.Text + "," + customMonoMaxBox4.Text + Environment.NewLine;
                }
                if (customMonoCheck5.IsChecked == true)
                {
                    saveOutput += "<CustomMono5> " + customMonoNameBox5.Text + "," + customMonoMassBox5.Text + "," + customMonoCBox5.Text + "," + customMonoHBox5.Text + "," + customMonoNBox5.Text + "," + customMonoOBox5.Text + "," + customMonoMinBox5.Text + "," + customMonoMaxBox5.Text + Environment.NewLine;
                }
            }
            saveOutput += "## Adducts: Adduct1, Adduct2" + Environment.NewLine;
            saveOutput += currentAdductSelection + Environment.NewLine;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                DefaultExt = "parameters.txt",
                Title = "Save Text File"
            };
            // Show dialog and check if the result is OK
            if (saveFileDialog.ShowDialog() == true)
            {
                // Write the content to the selected file
                File.WriteAllText(saveFileDialog.FileName, saveOutput);
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
                        if (line.Contains("<Input>"))
                        {
                            string result = line.Replace("<Input> ", string.Empty);
                            if (result == "Text") { TextRadioButton.IsChecked = true; }
                            else { MzmlRadioButton.IsChecked = true; }
                        }
                        if (line.Contains ("<Error tolerance>"))
                        {
                            string result = line.Replace("<Error tolerance> ", string.Empty);
                            string[] errorToleranceLine = result.Split(',');
                            DaError.Text = errorToleranceLine[0];
                            if (errorToleranceLine[1] == "Da") { Da.IsChecked = true; }
                            else { ppm.IsChecked = true; }
                        }
                        if (line.Contains("<Reducing end>"))
                        {
                            string result = line.Replace("<Reducing end> ", string.Empty);
                            switch (result)
                            {
                                case "Free":
                                    // Handle Free case
                                    reducingEndBox.Text = "Free";
                                    break;
                                case "Reduced":
                                    // Handle Reduced case
                                    reducingEndBox.Text = "Reduced";
                                    break;
                                case "InstantPC":
                                    // Handle InstantPC case
                                    reducingEndBox.Text = "InstantPC";
                                    break;
                                case "Rapifluor-MS":
                                    // Handle Rapifluor-MS case
                                    reducingEndBox.Text = "Rapifluor-MS";
                                    break;
                                case "2-aminobenzoic acid":
                                    // Handle 2-aminobenzoic acid case
                                    reducingEndBox.Text = "2-aminobenzoic acid";
                                    break;
                                case "2-aminobenzamide":
                                    // Handle 2-aminobenzamide case
                                    reducingEndBox.Text = "2-aminobenzamide";
                                    break;
                                case "Procainamide":
                                    // Handle Procainamide case
                                    reducingEndBox.Text = "Procainamide";
                                    break;
                                case "Girard's reagent P":
                                    // Handle Girard's reagent P case
                                    reducingEndBox.Text = "Girard's reagent P";
                                    break;
                                case "Custom":
                                    // Handle Custom case
                                    reducingEndBox.Text = "Custom";
                                    break;
                                default:
                                    // Handle default case
                                    reducingEndBox.Text = "";
                                    break;
                            }
                        }
                        if (line.Contains("<Custom reducing end>"))
                        {
                            string result = line.Replace("<Custom reducing end> ", string.Empty);
                            string[] customReducingEndLine = result.Split(',');
                            customReducingNameBox.Text = customReducingEndLine[0];
                            customReducingMassBox.Text = customReducingEndLine[1];
                            customReducingCBox.Text = customReducingEndLine[2];
                            customReducingHBox.Text = customReducingEndLine[3];
                            customReducingNBox.Text = customReducingEndLine[4];
                            customReducingOBox.Text = customReducingEndLine[5];
                        }
                        if (line.Contains("<Derivatisation>"))
                        {
                            string result = line.Replace("<Derivatisation> ", string.Empty);
                            switch (result)
                            {
                                case "Native":
                                    Native.IsChecked = true;
                                    break;
                                case "Permethylated":
                                    Permeth.IsChecked = true;
                                    break;
                                case "Peracetylated":
                                    Peracetyl.IsChecked = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (line.Contains("<OffByOne enabled>"))
                        {
                            string result = line.Replace("<OffByOne enabled> ", string.Empty);
                            if (result == "True") { OffByOne.IsChecked = true; }
                            else { OffByOne.IsChecked = false; }
                        }
                        if (line.Contains("<Monosaccharides>"))
                        {
                            string result = line.Replace("<Monosaccharides> ", string.Empty);
                            string[] monosaccharideLine = result.Split(',');
                            // Look through each , string section for a monosaccharide.
                            // e.g. Hex(1-12),HexNAc(2-8)
                            // Extract out the monosaccharide (Hex), the minimum (1), and the maximum (12) and set them to the respective sections
                            // So if Hex seen, enabled the Hex toggle, then parse the min and the max
                            foreach (string item in monosaccharideLine)
                            {
                                string trimmedItem = item.Trim();
                                // Need to work on this regex to split each string into 3
                                var match = Regex.Match(trimmedItem, @"(\w+)\((\d+)-(\d+)\)");
                                    string z = match.Groups[1].Value;
                                    int x = int.Parse(match.Groups[2].Value);
                                    int y = int.Parse(match.Groups[3].Value);
                                    if (z == "Hex")
                                    {
                                        HextoggleSwitch.IsOn = true;
                                        HexMin.Text = Convert.ToString(x);
                                        HexMax.Text = Convert.ToString(y);
                                    }
                                MessageBox.Show(z);
                            }
                        }
                        if (line.Contains("<CustomMono1>"))
                        {
                            string result = line.Replace("<CustomMono1> ", string.Empty);
                            string[] MonoSplitLine1 = result.Split(',');
                            customMonoNameBox1.Text = MonoSplitLine1[0];
                            customMonoMassBox1.Text = MonoSplitLine1[1];
                            customMonoCBox1.Text = MonoSplitLine1[2];
                            customMonoHBox1.Text = MonoSplitLine1[3];
                            customMonoNBox1.Text = MonoSplitLine1[4];
                            customMonoOBox1.Text = MonoSplitLine1[5];
                            customMonoMinBox1.Text = MonoSplitLine1[6];
                            customMonoMaxBox1.Text = MonoSplitLine1[7];
                            customMonoCheck1.IsChecked = true;
                        }
                        if (line.Contains("<CustomMono2>"))
                        {
                            string result = line.Replace("<CustomMono2> ", string.Empty);
                            string[] MonoSplitLine2 = result.Split(',');
                            customMonoNameBox2.Text = MonoSplitLine2[0];
                            customMonoMassBox2.Text = MonoSplitLine2[2];
                            customMonoCBox2.Text = MonoSplitLine2[2];
                            customMonoHBox2.Text = MonoSplitLine2[3];
                            customMonoNBox2.Text = MonoSplitLine2[4];
                            customMonoOBox2.Text = MonoSplitLine2[5];
                            customMonoMinBox2.Text = MonoSplitLine2[6];
                            customMonoMaxBox2.Text = MonoSplitLine2[7];
                            customMonoCheck2.IsChecked = true;
                        }
                        if (line.Contains("<CustomMono3>"))
                        {
                            string result = line.Replace("<CustomMono3> ", string.Empty);
                            string[] MonoSplitLine3 = result.Split(',');
                            customMonoNameBox3.Text = MonoSplitLine3[0];
                            customMonoMassBox3.Text = MonoSplitLine3[1];
                            customMonoCBox3.Text = MonoSplitLine3[2];
                            customMonoHBox3.Text = MonoSplitLine3[3];
                            customMonoNBox3.Text = MonoSplitLine3[4];
                            customMonoOBox3.Text = MonoSplitLine3[5];
                            customMonoMinBox3.Text = MonoSplitLine3[6];
                            customMonoMaxBox3.Text = MonoSplitLine3[7];
                            customMonoCheck3.IsChecked = true;
                        }
                        if (line.Contains("<CustomMono4>"))
                        {
                            string result = line.Replace("<CustomMono4> ", string.Empty);
                            string[] MonoSplitLine4 = result.Split(',');
                            customMonoNameBox4.Text = MonoSplitLine4[0];
                            customMonoMassBox4.Text = MonoSplitLine4[1];
                            customMonoCBox4.Text = MonoSplitLine4[2];
                            customMonoHBox4.Text = MonoSplitLine4[3];
                            customMonoNBox4.Text = MonoSplitLine4[4];
                            customMonoOBox4.Text = MonoSplitLine4[5];
                            customMonoMinBox4.Text = MonoSplitLine4[6];
                            customMonoMaxBox4.Text = MonoSplitLine4[7];
                            customMonoCheck4.IsChecked = true;
                        }
                        if (line.Contains("<CustomMono5>"))
                        {
                            string result = line.Replace("<CustomMono5> ", string.Empty);
                            string[] MonoSplitLine5 = result.Split(',');
                            customMonoNameBox5.Text = MonoSplitLine5[0];
                            customMonoMassBox5.Text = MonoSplitLine5[1];
                            customMonoCBox5.Text = MonoSplitLine5[2];
                            customMonoHBox5.Text = MonoSplitLine5[3];
                            customMonoNBox5.Text = MonoSplitLine5[4];
                            customMonoOBox5.Text = MonoSplitLine5[5];
                            customMonoMinBox5.Text = MonoSplitLine5[6];
                            customMonoMaxBox5.Text = MonoSplitLine5[7];
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
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
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
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
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
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
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
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
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
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
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
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
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
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
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
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
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

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBarMZML.Visibility = Visibility.Visible;
                // Running the mzML formatting in a different thread so we can give a status update
                if (TextRadioButton.IsChecked != true)
                {
                    await Task.Run(() => mzMLProcess());
                }
                else
                {
                    // Ask the user which text or csv file they want to analyse. Also allow .csv input.
                    OpenFileDialog openFileDialog = new()
                    {
                        Filter = "Text files (*.txt)|*.txt|CSV files (*.csv)|*.csv",
                        Multiselect = true
                    };

                    if (openFileDialog.ShowDialog() == true)
                    {

                        // Get the selected file path
                        filePath = openFileDialog.FileName;
                    }
                    else
                    {
                        return;
                    }

                    foreach (String file in openFileDialog.FileNames)
                    {
                        // File type specific processing
                        string fileExtension = System.IO.Path.GetExtension(file).ToLower();
                        string fileContent = "";

                        if (fileExtension == ".txt")
                        {
                            // Process to make a string from the txt file
                            fileContent = ReadMassFileWithSeparator(file, Environment.NewLine);

                            // Replace text box with the individual mass values
                            InputMasses.Text = fileContent;
                        }
                        else if (fileExtension == ".csv")
                        {
                            // Process to make a string from the csv
                            fileContent = ReadMassFileWithSeparator(file, ",");

                            // Replace text box with the individual mass values
                            InputMasses.Text = fileContent;

                        }
                    }
                }
            }
            finally
            {
                ProgressBarMZML.Visibility = Visibility.Collapsed;
                // Let the user now start the processing. Without this step, the user may crash the program by starting the processing before the mzml info is extracted
                if (scans.Any())
                {
                    submitbutton.IsEnabled = IsEnabled;
                }
            }
        }

        private void mzMLProcess()
        {
            List<string> fileNames = [];
            List<int> fileScans = [];
            // Ask the user which mzml file they want to analyse
            OpenFileDialog openFileDialog = new()
            {
                Filter = "mzML files (mzML)|*.mzML",
                // Allows the user to select more than one mzML for conversion
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {

                // Get the selected file path
                filePath = openFileDialog.FileName;
            }
            else
            {
                return;
            }
            polarity = "";
            List<decimal> precursors = [];
                foreach (String file in openFileDialog.FileNames)
            {
                // Going to process each file one at a time using this section of the code.
                // Read each line from the given file
                StreamReader sr = new(file);

                // Parse each line of the mzml to extract important information from MS2 scans of the mzML (polarity, precursor m/z, charge state, scan # for given MS2)
                for (line = sr.ReadLine(); line != null; line = sr.ReadLine())
                {
                    // Problem: Bruker and Thermo mzmls have all lines in different positions
                    // Thermo order: Spectrum index (including scan#), then "ms level" value="2", then "negative", then "selected ion m/z", then "charge state"
                    // Bruker order: Spectrum index (including scan#), then "negative scan", then "ms level" value ="2", then "charge state", then "selected ion m/z"
                    // Sciex doesn't use scan #, so we've adapted cycle and experiment number to make (X.Y) representation instead
                    // Code modified to perform this per spectrum, rather than trying to hard code by line positions

                    // find lines containing positive or negative mode
                    if (line.Contains("MS:1000129"))
                    {
                        polarity = "negative";
                    }
                    if (line.Contains("MS:1000130"))
                    {
                        polarity = "positive";
                    }

                    // find lines containing retention time, to minute time scale
                    if (line.Contains("MS:1000016"))
                    {
                        // Bruker records retention time by the second
                        if (line.Contains("unitName=\"second\""))
                        {
                            // split the line containing this by "
                            RTLine = line.Split("\"");
                            // After the 7th ", that's where the charge can be found, so convert it from string array into int
                            retentionTime = decimal.Parse(RTLine[7]) / 60;
                        }
                        else
                        // whereas Thermo/Sciex records RT by the minute
                        {
                            // split the line containing this by "
                            RTLine = line.Split("\"");
                            // After the 7th ", that's where the charge can be found, so convert it from string array into int
                            retentionTime = decimal.Parse(RTLine[7]);
                        }
                    }

                    // find lines containing total ion chromatogram intensity for that scan, only supported by Thermo and Sciex
                    if (line.Contains("MS:1000285"))
                    {
                        // split the line containing this by "
                        TICLine = line.Split("\"");
                        // After the 7th ", that's where the charge can be found, so convert it from string array into int
                        TIC = decimal.Parse(TICLine[7], System.Globalization.NumberStyles.AllowExponent | System.Globalization.NumberStyles.AllowDecimalPoint);
                    }

                    // find lines containing the precursor
                    if (line.Contains("\"selected ion m/z\""))
                    {
                        // split the line containing this by "
                        precursorLine = line.Split("\"");
                        // After the 7th ", that's where the precursor m/z can be found, so convert it from string array into decimal for accuracy
                        precursor = Math.Round(decimal.Parse(precursorLine[7]), 6);
                    }
                    // find lines containing the charge
                    if (line.Contains("\"charge state\""))
                    {
                        // split the line containing this by "
                        chargeLine = line.Split("\"");
                        // After the 7th ", that's where the charge can be found, so convert it from string array into int
                        charge = int.Parse(chargeLine[7]);
                    }
                    // find lines containing the scan number
                    if (line.Contains("scan=")
                        // To ensure we don't pick up the Thermo spectrum title
                        && line.Contains("defaultArrayLength"))
                    {
                        // split the line containing this by "
                        scanLine = line.Split("\"");
                        // After the 3rd ", that's where the scan # can be found. This is the Thermo-specific extraction:
                        if (line.Contains("controller"))
                        {
                            scanNumber = scanLine[3].Replace("controllerType=0 controllerNumber=1 scan=", "");
                        }
                        // And this is for Bruker
                        else
                        {
                            scanNumber = scanLine[3].Replace("scan=", "");
                        }
                    }
                    // Sciex specific scan number interpretation
                    if (line.Contains(" cycle=")
                        && line.Contains(" experiment=")
                        && line.Contains("defaultArrayLength"))
                    {
                        //split the line containing cycle and experiment number by =
                        scanLine = line.Split("=");
                        // Cycle # is after the 5th "="
                        string cycleScan = scanLine[5].Replace(" experiment", ".");
                        string experimentScan = scanLine[6].Replace("\" defaultArrayLength", "");
                        scanNumber = cycleScan + experimentScan;
                    }
                    if (line.Contains("</spectrum>"))
                    {
                        if (precursor != 0 && charge != 0)
                        {
                            // Convert precursor m/z and z to obtain neutral precursor mass
                            switch (polarity)
                            {
                                case "negative":
                                    neutralPrecursorListmzml += Convert.ToString(charge * precursor + (charge * 1.007276m)) + Environment.NewLine;
                                    charge = -charge;
                                    break;
                                case "positive":
                                    neutralPrecursorListmzml += Convert.ToString(charge * precursor - (charge * 1.007276m)) + Environment.NewLine;
                                    break;
                                default:
                                    break;
                            }
                            // Put the scan value into a list of scan numbers that feature MS2.
                            scans.Add(decimal.Parse(scanNumber));
                            // Adds charge to a list for the end report
                            charges.Add(charge);
                            // Add RT to a list for the end report
                            retentionTimes.Add(retentionTime);
                            // Add TIC to a list for the end report
                            TICs.Add(TIC);
                            // Add stripped file name to the index
                            string fileName = file.Substring(file.LastIndexOf('\\') + 1);
                            files.Add(fileName);
                        }
                        precursor = 0;
                        charge = 0;
                        scanNumber = "";
                        polarity = "";
                        retentionTime = 0;
                        TIC = 0;
                    }
                }
                string fileNameOutput = file.Substring(file.LastIndexOf('\\') + 1);
            }
            if (!scans.Any())
            {
                MessageBox.Show("No MS2 found in the given mzML file. Please confirm the selected file has MS2 scans, or select a different file.");
            }
            else
            {
                // Provide list of all filenames provided in the openFileDialog, without the directory name
                string allFiles = string.Join(", ", openFileDialog.FileNames.Select(System.IO.Path.GetFileName));
                // Provide number of scans for each filename
                MessageBox.Show("Files " + allFiles + " have completed uploading with a total number of " + scans.Count + " MS2 scans identified.");
            }
        }

        static string ReadMassFileWithSeparator(string filePath, string separator)
        {
            // Processes the neutral mass input with separators
            string[] lines = File.ReadAllLines(filePath);
            if (separator == ",")
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace(",", Environment.NewLine);
                }
            }
            return string.Join(separator, lines);
        }

        // Execution of the combinatorial analysis
        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            // Determine if the customReducedCheckbox is checked
            if (customReducingCheck.IsChecked == true)
            {
                customReducingName = customReducingNameBox.Text;
                customReducedMassOutput = customReducingMassBox.Text;
            }
            // Determine if the customMonoCheckbox is checked for each checkbox
            if (customMonoCheck1.IsChecked == true)
            {
                // Extract out all the custom monosaccharide information for 1
                customMono1Name = customMonoNameBox1.Text;
                customMono1Mass = decimal.Parse(customMonoMassBox1.Text);
                customMono1CCount = int.Parse(customMonoCBox1.Text);
                customMono1HCount = int.Parse(customMonoHBox1.Text);
                customMono1NCount = int.Parse(customMonoNBox1.Text);
                customMono1OCount = int.Parse(customMonoOBox1.Text);
                customMono1Min = int.Parse(customMonoMinBox1.Text);
                customMono1Max = int.Parse(customMonoMaxBox1.Text);
            }
            if (customMonoCheck2.IsChecked == true)
            {
                // Extract out all the custom monosaccharide information for 2
                customMono2Name = customMonoNameBox2.Text;
                customMono2Mass = decimal.Parse(customMonoMassBox2.Text);
                customMono2CCount = int.Parse(customMonoCBox2.Text);
                customMono2HCount = int.Parse(customMonoHBox2.Text);
                customMono2NCount = int.Parse(customMonoNBox2.Text);
                customMono2OCount = int.Parse(customMonoOBox2.Text);
                customMono2Max = int.Parse(customMonoMaxBox2.Text);
                customMono2Min = int.Parse(customMonoMinBox2.Text);
            }
            if (customMonoCheck3.IsChecked == true)
            {
                // Extract out all the custom monosaccharide information for 3
                customMono3Name = customMonoNameBox3.Text;
                customMono3Mass = decimal.Parse(customMonoMassBox3.Text);
                customMono3CCount = int.Parse(customMonoCBox3.Text);
                customMono3HCount = int.Parse(customMonoHBox3.Text);
                customMono3NCount = int.Parse(customMonoNBox3.Text);
                customMono3OCount = int.Parse(customMonoOBox3.Text);
                customMono3Max = int.Parse(customMonoMaxBox3.Text);
                customMono3Min = int.Parse(customMonoMinBox3.Text);
            }
            if (customMonoCheck4.IsChecked == true)
            {
                // Extract out all the custom monosaccharide information for 4
                customMono4Name = customMonoNameBox4.Text;
                customMono4Mass = decimal.Parse(customMonoMassBox4.Text);
                customMono4CCount = int.Parse(customMonoCBox4.Text);
                customMono4HCount = int.Parse(customMonoHBox4.Text);
                customMono4NCount = int.Parse(customMonoNBox4.Text);
                customMono4OCount = int.Parse(customMonoOBox4.Text);
                customMono4Max = int.Parse(customMonoMaxBox4.Text);
                customMono4Min = int.Parse(customMonoMinBox4.Text);
            }
            if (customMonoCheck5.IsChecked == true)
            {
                // Extract out all the custom monosaccharide information for 5
                customMono5Name = customMonoNameBox5.Text;
                customMono5Mass = decimal.Parse(customMonoMassBox5.Text);
                customMono5CCount = int.Parse(customMonoCBox5.Text);
                customMono5HCount = int.Parse(customMonoHBox5.Text);
                customMono5NCount = int.Parse(customMonoNBox5.Text);
                customMono5OCount = int.Parse(customMonoOBox5.Text);
                customMono5Max = int.Parse(customMonoMaxBox5.Text);
                customMono5Min = int.Parse(customMonoMinBox5.Text);
            }
            try
            {
                solutionMultiples = "";
                resetbutton.IsEnabled = IsEnabled;
                ProgressBarSubmit.Visibility = Visibility.Visible;
                // Define the components in the combinatorial analysis: native, permethylated, peracetylated
                if (Native.IsChecked == true)
                {
                    derivatisation = "Native";
                    // Native
                    dhex = 146.057908m; // permethylated mass = 174.089210 chemical formula = C8H14O4
                    hex = 162.052823m; // permethylated mass = 204.099775 chemical formula = C9H16O5
                    hexnac = 203.079372m; // permethylated mass = 245.126324 chemical formula = C11H19NO5
                    hexn = 161.068808m; // permethylated mass = 217.131409 chemical formula = C10H19NO4
                    hexa = 176.032088m; // permethylated mass = 218.079040 chemical formula = C9H14O6
                    dhexnac = 187.084458m; // permethylated mass = 215.115759 chemical formula = C10H17N1O4
                    pent = 132.042258m; // permethylated mass = 160.073560 chemical formula = C7H12O4
                    kdn = 250.068867m; // permethylated mass = 320.147120 chemical formula = C14H24O8
                    neuac = 291.095416m; // permethylated mass = 361.173669 chemical formula = C16H27NO8
                    neugc = 307.090331m; // permethylated mass = 391.184234 chemical formula = C17H29NO9
                    phos = 79.966331m; // permethylated mass = 93.981983 chemical formula = CH3O3P
                    lneuac = 273.0848518m;
                    eeneuac = 319.1267166m;
                    dneuac = 318.1427011m;
                    amneuac = 290.1114009m;
                    acetyl = 42.010565m;
                    lneugc = 289.0797664m;
                    eeneugc = 335.1216313m;
                    dneugc = 306.1063155m;
                    amneugc = 334.1376157m;
                    sulf = 79.956815m; // SO3
                }
                if (Permeth.IsChecked == true)
                {
                    derivatisation = "Permethylated";
                    // Permethylated
                    dhex = 174.089210m; // chemical formula = C8H14O4
                    hex = 204.099775m; //  chemical formula = C9H16O5
                    hexnac = 245.126324m; //  chemical formula = C11H19NO5
                    hexn = 203.115758m; //  chemical formula = C9H17NO4
                    hexa = 218.079040m; //  chemical formula = C9H14O6
                    dhexnac = 215.115758m; //  chemical formula = C10H17N1O4
                    pent = 160.073560m; //  chemical formula = C7H12O4
                    kdn = 320.147120m; // chemical formula = C14H24O8
                    neuac = 361.173669m; // chemical formula = C16H27NO8
                    neugc = 391.184234m; // chemical formula = C17H29NO9
                    phos = 93.981980m; // chemical formula = PO3H3C1
                    sulf = 65.941165m; // chemical formula = SO3C-1H-2
                }
                if (Peracetyl.IsChecked == true)
                {
                    derivatisation = "Peracetylated";
                    // Peracetylated
                    dhex = 230.079038m; // chemical formula = C10H14O6
                    hex = 288.084517m; // chemical formula = C12H16O8
                    hexnac = 287.100501m; // chemical formula = C12H17NO7
                    hexn = 287.100501m; // chemical formula = C12H17NO7
                    hexa = 260.053217m; // chemical formula = C10H12O8
                    dhexnac = 247.105587m; // chemical formula = C10H17NO6
                    pent = 216.063388m; // chemical formula = C9H12O6
                    kdn = 376.100561m; // chemical formula = C15H20O11
                    neuac = 417.127110m; // chemical formula = C17H23NO11
                    neugc = 475.132593m; // chemical formula = C19H25NO13
                    phos = 37.955765m; // chemical formula = PO2C-2H-1
                    sulf = 37.946250m; // chemical formula = SO2C-2H-2
                }

                // Add the components to combinatorial analysis based on which monosaccharides the user chooses to include
                if (HextoggleSwitch.IsOn == true)
                {
                    numbers.Add(hex);
                }

                if (HexAtoggleSwitch.IsOn == true)
                {
                    numbers.Add(hexa);
                }
                if (dHextoggleSwitch.IsOn == true)
                {
                    numbers.Add(dhex);
                }
                if (HexNActoggleSwitch.IsOn == true)
                {
                    numbers.Add(hexnac);
                }
                if (HexNtoggleSwitch.IsOn == true)
                {
                    numbers.Add(hexn);
                }
                if (dHexNActoggleSwitch.IsOn == true)
                {
                    numbers.Add(dhexnac);
                }
                if (PenttoggleSwitch.IsOn == true)
                {
                    numbers.Add(pent);
                }
                if (KDNtoggleSwitch.IsOn == true)
                {
                    numbers.Add(kdn);
                }
                if (Neu5ActoggleSwitch.IsOn == true)
                {
                    numbers.Add(neuac);
                }
                if (Neu5GctoggleSwitch.IsOn == true)
                {
                    numbers.Add(neugc);
                }
                if (PhostoggleSwitch.IsOn == true)
                {
                    numbers.Add(phos);
                }
                if (SulftoggleSwitch.IsOn == true)
                {
                    numbers.Add(sulf);
                }
                if (lNeuActoggleSwitch.IsOn == true)
                {
                    numbers.Add(lneuac);
                }
                if (eNeuActoggleSwitch.IsOn == true)
                {
                    numbers.Add(eeneuac);
                }
                if (dNeuActoggleSwitch.IsOn == true)
                {
                    numbers.Add(dneuac);
                }
                if (amNeuActoggleSwitch.IsOn == true)
                {
                    numbers.Add(amneuac);
                }
                if (AcetyltoggleSwitch.IsOn == true)
                {
                    numbers.Add(acetyl);
                }
                if (lNeuGctoggleSwitch.IsOn == true)
                {
                    numbers.Add(lneugc);
                }
                if (eNeuGctoggleSwitch.IsOn == true)
                {
                    numbers.Add(eeneugc);
                }
                if (dNeuGctoggleSwitch.IsOn == true)
                {
                    numbers.Add(dneugc);
                }
                if (amNeuGctoggleSwitch.IsOn == true)
                {
                    numbers.Add(amneugc);
                }
                if (customMonoCheck1.IsChecked == true)
                {
                    numbers.Add(customMono1Mass);
                    monoCustom1 = true;
                }
                if (customMonoCheck2.IsChecked == true)
                {
                    numbers.Add(customMono2Mass);
                    monoCustom2 = true;
                }
                if (customMonoCheck3.IsChecked == true)
                {
                    numbers.Add(customMono3Mass);
                    monoCustom3 = true;
                }
                if (customMonoCheck4.IsChecked == true)
                {
                    numbers.Add(customMono4Mass);
                    monoCustom4 = true;
                }
                if (customMonoCheck5.IsChecked == true)
                {
                    numbers.Add(customMono5Mass);
                    monoCustom5 = true;
                }

                // Process for multiple targets conditionally based on text box or mzml input
                if (TextRadioButton.IsChecked == true)
                {
                    targetString = InputMasses.Text;
                    inputParameters = "Mass List: " + targetString.Replace("\r\n", ",");
                }
                else
                {
                    targetString = neutralPrecursorListmzml;
                    inputParameters = "mzML: " + filePath;
                }

                // Turn that input into a list of masses
                targetStrings = new(
                    targetString.Split(new string[] { "\n" },
                    StringSplitOptions.RemoveEmptyEntries));
                targets = targetStrings.ConvertAll(decimal.Parse);

                // Adduct calculation
                // This can result in huge combinatorial searches but it's there for the user as an option
                // if mzml input used, force M+H and M-H, then let the user add on other adducts (problem with this is that positive mode will have negative adducts etc)
               
                // Only trigger this if something other than M is selected
                if (negativeMHCheckBox.IsChecked == true ||
                    negativeMFACheckBox.IsChecked == true ||
                    negativeMAACheckBox.IsChecked == true ||
                    negativeMTFACheckBox.IsChecked == true ||
                    positiveMHCheckBox.IsChecked == true ||
                    positiveMNaCheckBox.IsChecked == true ||
                    positiveMKCheckBox.IsChecked == true ||
                    positiveMNH4CheckBox.IsChecked == true
                    )
                {
                    // mzML input has been processed as de / protonated to generate a neutral mass list, so adducts offset is +/- 1 Da for the respective negative/positive adducts
                    // We also don't bother with doing M, M+H, and M-H because they are all the same after mzML processing (M+H and M-H become M)
                    if (MzmlRadioButton.IsChecked == true)
                    {
                        // Making a separate list to then be used for target building
                        targetAdductsProcessing = targets;
                        targetAdducts = new List<decimal>();

                        // Subtracting H- from all targets and saving that as a new list
                        if (negativeMHCheckBox.IsChecked == true || neutralMCheckBox.IsChecked == true || positiveMHCheckBox.IsChecked == true)
                        {
                            searchRepeats += 1;
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o]);
                            }
                        }
                        // M+COOH adduct calculation
                        if (negativeMFACheckBox.IsChecked == true)
                        {
                            searchRepeats += 1;
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)44.998201 - (decimal)1.007276);
                            }
                        }
                        // M+acetic acid adduct calculation
                        if (negativeMAACheckBox.IsChecked == true)
                        {
                            searchRepeats += 1;
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)59.013851 - (decimal)1.007276);
                            }
                        }
                        // M+TFA adduct calculation
                        if (negativeMTFACheckBox.IsChecked == true)
                        {
                            searchRepeats += 1;
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)112.985586 - (decimal)1.007276);
                            }
                        }
                        // M+Na adduct calculation
                        if (positiveMNaCheckBox.IsChecked == true)
                        {
                            searchRepeats += 1;
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)22.989218 + (decimal)1.007276);
                            }
                        }
                        // M+K adduct calculation
                        if (positiveMKCheckBox.IsChecked == true)
                        {
                            searchRepeats += 1;
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)38.963158 + (decimal)1.007276);
                            }
                        }
                        // M+NH4 adduct calculation
                        if (positiveMNH4CheckBox.IsChecked == true)
                        {
                            searchRepeats += 1;
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)18.033823 + (decimal)1.007276);
                            }
                        }
                        // Custom adduct calculcation
                        if (customAdductCheckBox.IsChecked == true)
                        {
                            searchRepeats += 1;
                            targetsToAdd = targetAdductsProcessing.Count;
                            // Processing of customAdductMassText to account for mzML assuming a protonated/deprotonated precursor
                            if (customAdductPolarity.SelectedIndex == 0) // Protonated
                            {
                                adductCustom = Convert.ToDecimal(customAdductMassText.Text) - (decimal)1.007276;
                            }
                            else // Deprotonated
                            {
                                adductCustom = Convert.ToDecimal(customAdductMassText.Text) + (decimal)1.007276;
                            }
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - adductCustom);
                            }
                        }
                        targets = targetAdducts;
                    }
                    // Text input is singly charged m/z values that are observed via experiments like MALDI-MS of permethylated glycans so no modification of mass is needed.
                    if (TextRadioButton.IsChecked == true)
                    {
                        // Making a separate list to then be used for target building
                        targetAdductsProcessing = targets;
                        targetAdducts = new List<decimal>();

                        // Subtracting H- from all targets and saving that as a new list
                        if (negativeMHCheckBox.IsChecked == true)
                        {
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] + (decimal)1.007276);
                            }
                        }
                        // Appending the list with the original text if the user has M selected
                        if (neutralMCheckBox.IsChecked == true)
                        {
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o]);
                            }
                        }
                        // M+COOH adduct calculation
                        if (negativeMFACheckBox.IsChecked == true)
                        {
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)44.998201);
                            }
                        }
                        // M+acetic acid adduct calculation
                        if (negativeMAACheckBox.IsChecked == true)
                        {
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)59.013851);
                            }
                        }
                        // M+TFA adduct calculation
                        if (negativeMTFACheckBox.IsChecked == true)
                        {
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)112.985586);
                            }
                        }
                        // M+H adduct calculation
                        if (positiveMHCheckBox.IsChecked == true)
                        {
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)1.007276);
                            }
                        }
                        // M+Na adduct calculation
                        if (positiveMNaCheckBox.IsChecked == true)
                        {
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)22.989218);
                            }
                        }
                        // M+K adduct calculation
                        if (positiveMKCheckBox.IsChecked == true)
                        {
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)38.963158);
                            }
                        }
                        // M+NH4 adduct calculation
                        if (positiveMNH4CheckBox.IsChecked == true)
                        {
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - (decimal)18.033823);
                            }
                        }
                        // Custom adduct calculcation
                        if (customAdductCheckBox.IsChecked == true)
                        {
                            targetsToAdd = targetAdductsProcessing.Count;
                            for (int o = 0; o < targetsToAdd; o++)
                            {
                                targetAdducts.Add(targetAdductsProcessing[o] - Convert.ToDecimal(customAdductMassText.Text));
                            }
                        }
                        targets = targetAdducts;
                    }
                }
                
                // For enabling off-by-one errors. Thermo is pretty good at correcting the selected ion m/z when it picks an isotopic distribution, but might be useful for others
                if (OffByOne.IsChecked == true)
                {
                    searchRepeats += 1;
                    offByOneChecked = true;
                    // For each target in the list, remove one hydrogen to account for the C13 isotope being picked instead of monoisotopic (negative mode only)
                    targetsToAdd = targets.Count;
                    for (int o = 0; o < targetsToAdd; o++)
                    {
                        targets.Add(targets[o] - (decimal)1.007276);
                    }
                }

                // Early processing of target list, breaking it down so that the reducing ends are removed
                if (Native.IsChecked == true)
                {
                    switch (reducingEndBox.SelectedIndex)
                    {
                        case 0:
                            reducedEnd = "Free";
                            targets = targets.Select(z => z - 18.010555m).ToList();
                            break;
                        case 1:
                            reducedEnd = "Reduced";
                            targets = targets.Select(z => z - 20.026195m).ToList();
                            break;
                        case 2:
                            reducedEnd = "InstantPC";
                            targets = targets.Select(z => z - (18.010555m + 261.14773m)).ToList();
                            break;
                        case 3:
                            reducedEnd = "Rapifluor-MS";
                            targets = targets.Select(z => z - (18.010555m + 311.17461m)).ToList();
                            break;
                        case 4:
                            reducedEnd = "2AA";
                            targets = targets.Select(z => z - (18.010555m + 121.052774m)).ToList();
                            break;
                        case 5:
                            reducedEnd = "2AB";
                            targets = targets.Select(z => z - (18.010555m + 120.068758m)).ToList();
                            break;
                        case 6:
                            reducedEnd = "Procainamide";
                            targets = targets.Select(z => z - (18.010555m + 219.173557m)).ToList();
                            break;
                        case 7:
                            reducedEnd = "girP";
                            targets = targets.Select(z => z - (18.010555m + 134.07182m)).ToList();
                            break;
                        case 8:
                            reducedEnd = "Custom";
                            customReducingMass = Convert.ToDecimal(customReducingMassBox.Text);
                            targets = targets.Select(z => z - (18.010555m + Convert.ToDecimal(customReducingMassBox.Text))).ToList();
                            break;
                        default:
                            break;
                    }
                }
                if (Permeth.IsChecked == true)
                {
                    switch (reducingEndBox.SelectedIndex)
                    {
                        case 0:
                            reducedEnd = "Free";
                            targets = targets.Select(z => z - (18.010555m + 28.031300m)).ToList();
                            break;
                        case 1:
                            reducedEnd = "Reduced";
                            targets = targets.Select(z => z - (20.026195m + 42.046950m)).ToList();
                            break;
                        case 8:
                            customReducingMass = Convert.ToDecimal(customReducingMassBox.Text);
                            reducedEnd = "Custom";
                            targets = targets.Select(z => z - (18.010555m + Convert.ToDecimal(customReducingMassBox.Text))).ToList();
                            break;
                        default:
                            break;
                    }
                }                 
                if (Peracetyl.IsChecked == true)
                {
                    switch (reducingEndBox.SelectedIndex)
                    {
                        case 0:
                            reducedEnd = "Free";
                            targets = targets.Select(z => z - (18.010555m + 84.021129m)).ToList();
                            break;
                        case 1:
                            reducedEnd = "Reduced";
                            targets = targets.Select(z => z - (20.026195m + 126.031694m)).ToList();
                            break;
                        case 8:
                            customReducingMass = Convert.ToDecimal(customReducingMassBox.Text);
                            reducedEnd = "Custom";
                            targets = targets.Select(z => z - (18.010555m + Convert.ToDecimal(customReducingMassBox.Text))).ToList();
                            break;
                        default:
                            break;
                    }
                }

                // Define the upper and lower error tolerances for search
                if (Da.IsChecked == true)
                {
                    DaChecked = true;
                    errorTol = Convert.ToDecimal(DaError.Text);
                }
                else
                {
                    DaChecked = false;
                    errorTol = Convert.ToDecimal(ppmError.Text);
                }

                // Limiting combinatorial steps by user input
                HexMin_int = int.Parse(HexMin.Text);
                HexMax_int = int.Parse(HexMax.Text);
                HexNAcMin_int = int.Parse(HexNAcMin.Text);
                HexNAcMax_int = int.Parse(HexNAcMax.Text);
                dHexMin_int = int.Parse(dHexMin.Text);
                dHexMax_int = int.Parse(dHexMax.Text);
                HexAMin_int = int.Parse(HexAMin.Text);
                HexAMax_int = int.Parse(HexAMax.Text);
                HexNMin_int = int.Parse(HexNMin.Text);
                HexNMax_int = int.Parse(HexNMax.Text);
                PentMin_int = int.Parse(PentMin.Text);
                PentMax_int = int.Parse(PentMax.Text);
                KDNMin_int = int.Parse(KDNMin.Text);
                KDNMax_int = int.Parse(KDNMax.Text);
                Neu5AcMin_int = int.Parse(Neu5AcMin.Text);
                Neu5AcMax_int = int.Parse(Neu5AcMax.Text);
                Neu5GcMin_int = int.Parse(Neu5GcMin.Text);
                Neu5GcMax_int = int.Parse(Neu5GcMax.Text);
                PhosMin_int = int.Parse(PhosMin.Text);
                PhosMax_int = int.Parse(PhosMax.Text);
                SulfMin_int = int.Parse(SulfMin.Text);
                SulfMax_int = int.Parse(SulfMax.Text);
                dHexNAcMin_int = int.Parse(dHexNAcMin.Text);
                dHexNAcMax_int = int.Parse(dHexNAcMax.Text);
                lNeuAcMin_int = int.Parse(lNeuAcMin.Text);
                lNeuAcMax_int = int.Parse(lNeuAcMax.Text);
                eeNeuAcMin_int = int.Parse(eeNeuAcMin.Text);
                eeNeuAcMax_int = int.Parse(eeNeuAcMax.Text);
                dNeuAcMin_int = int.Parse(dNeuAcMin.Text);
                dNeuAcMax_int = int.Parse(dNeuAcMax.Text);
                amNeuAcMin_int = int.Parse(amNeuAcMin.Text);
                amNeuAcMax_int = int.Parse(amNeuAcMax.Text);
                acetylMin_int = int.Parse(AcetylMin.Text);
                acetylMax_int = int.Parse(AcetylMax.Text);
                lNeuGcMin_int = int.Parse(lNeuGcMin.Text);
                lNeuGcMax_int = int.Parse(lNeuGcMax.Text);
                eeNeuGcMin_int = int.Parse(eeNeuGcMin.Text);
                eeNeuGcMax_int = int.Parse(eeNeuGcMax.Text);
                dNeuGcMin_int = int.Parse(dNeuGcMin.Text);
                dNeuGcMax_int = int.Parse(dNeuGcMax.Text);
                amNeuGcMin_int = int.Parse(amNeuGcMin.Text);
                amNeuGcMax_int = int.Parse(amNeuGcMax.Text);
                if (customReducingCheck.IsChecked == true)
                    {
                    customReducingCCount = Convert.ToInt16(customReducingCBox.Text);
                    customReducingHCount = Convert.ToInt16(customReducingHBox.Text);
                    customReducingNCount = Convert.ToInt16(customReducingNBox.Text);
                    customReducingOCount = Convert.ToInt16(customReducingOBox.Text);
                }
                // Running the mzML formatting in a different thread so we can give a status update
                await Task.Run(() => glyComboProcess());
            }
            finally
            {
                ProgressBarSubmit.Visibility = Visibility.Collapsed;
            }
        }
        
        private void glyComboProcess()
        {
            var watch = Stopwatch.StartNew();
            iterations = 0;
            Sum_up(numbers, targets);
            solutions = "";
            // Pop-up to let the user know the search has finished
            watch.Stop();
            ElapsedMSec = watch.ElapsedMilliseconds;
            new Thread(() => { MessageBox.Show("GlyCombo has finished running." + Environment.NewLine + ((solutionMultiples.Length - solutionMultiples.Replace(Environment.NewLine, string.Empty).Length)/2) + " monosaccharide combinations identified over " + iterations + " iterations." + Environment.NewLine + "Total search time: " + ElapsedMSec/1000 + " seconds."); }).Start();
            solutionProcess = "";
        }

        // Process to match glycan compositions by sum_up_recursive
        private void Sum_up(List<decimal> numbers, List<decimal> targets)
        {
            // Save file dialog
            SaveFileDialog saveFileDialog1 = new()
            {
                Filter = "csv files (*.csv)|*.csv",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (saveFileDialog1.ShowDialog() != true)
            {
                MessageBox.Show("Please type in file name for export and click OK");
                saveFileDialog1.ShowDialog();
            }
            // Pop-up to let the user know the search is running
            new Thread(() => { MessageBox.Show("Search started." + Environment.NewLine + "Processing " + targets.Count + " precursors." + Environment.NewLine + "Please wait for the search to complete."); }).Start();

            // For each target in the list
            for (int i = 0; i < targets.Count; i++)
            {
                bool targetFound = false;
                // Define the upper and lower error tolerances for search
                if (DaChecked == true)
                {
                    targetLow = targets[i] - errorTol;
                    targetHigh = targets[i] + errorTol;
                }
                else
                {
                    targetLow = targets[i] - (targets[i] * (errorTol / 1000000));
                    targetHigh = targets[i] + (targets[i] * (errorTol / 1000000));
                }
                decimal target = targets[i];
                Sum_up_recursive(numbers, target, [], targetFound, i);
            };

            // Write processed data to csv file
            string solutionHeader = "";
            string skylineSolutionHeader = "";
            string skylineSolutionMultiplesPreTrim = "";
            string skylineSolutionMultiples = "";
            if (inputChecked == "Text")
            {
                solutionHeader = "Composition,Observed mass,Theoretical mass,Molecular Formula,Mass error,Scan number,Precursor Charge,Retention Time,TIC,File Name";
                skylineSolutionHeader = "Molecule List Name,Molecule Name,Observed mass,Theoretical mass,Molecular Formula,Mass error,Scan number,Precursor Charge,Retention Time,TIC,Note";
                // Process the SolutionMultiples string in a way that generates an output compatible with Skyline with no user intervention
                skylineSolutionMultiplesPreTrim = (solutionMultiples.Insert(0, Environment.NewLine)).Replace(Environment.NewLine, Environment.NewLine + "GlyCombo,");
                skylineSolutionMultiples = skylineSolutionMultiplesPreTrim.Substring(0, skylineSolutionMultiplesPreTrim.Length - 10);
                File.WriteAllText(string.Concat(saveFileDialog1.FileName.AsSpan(0, saveFileDialog1.FileName.Length - 4), "_SkylineImport.csv"), skylineSolutionHeader + skylineSolutionMultiples);
            }
            else
            {
                solutionHeader = "Composition,Observed mass,Theoretical mass,ChemicalFormula,Mass error";
                File.WriteAllText(saveFileDialog1.FileName, solutionHeader + Environment.NewLine + solutionMultiples);
            }
            File.WriteAllText(saveFileDialog1.FileName, solutionHeader + Environment.NewLine + solutionMultiples);

            // Prepare the parameter report for the search used
            if (monoCustom1 == true)
            {
                param_monoCustom1 = Environment.NewLine + customMono1Name + " (" + customMono1Min.ToString() + "-" + customMono1Max.ToString() + ")";
            }
            if (monoCustom2 == true)
            {
                param_monoCustom2 = Environment.NewLine + customMono2Name + " (" + customMono2Min.ToString() + "-" + customMono2Max.ToString() + ")";
            }
            if (monoCustom3 == true)
            {
                param_monoCustom3 = Environment.NewLine + customMono3Name + " (" + customMono3Min.ToString() + "-" + customMono3Max.ToString() + ")";
            }
            if (monoCustom4 == true)
            {
                param_monoCustom4 = Environment.NewLine + customMono4Name + " (" + customMono4Min.ToString() + "-" + customMono4Max.ToString() + ")";
            }
            if (monoCustom5 == true)
            {
                param_monoCustom5 = Environment.NewLine + customMono5Name + " (" + customMono5Min.ToString() + "-" + customMono5Max.ToString() + ")";
            }

            // Converting precursor list to series of strings for subsequent confirmation
            string combinedTargets = string.Join(Environment.NewLine, targets.ToArray());
            string submitOutput = "## GlyCombo v0.7 search output" + Environment.NewLine;
            submitOutput += "<Input> " + inputChecked + Environment.NewLine;
            submitOutput += "<Error tolerance> " + errorTol + "," + massErrorType + Environment.NewLine;
            submitOutput += "<Reducing end> " + reducedEnd.ToString() + Environment.NewLine;
            if (reducedEnd.ToString() == "Custom")
            {
                submitOutput += "## Custom reducing end: Name, Mass, #C, #H, #N, #O" + Environment.NewLine;
                submitOutput += "<Custom reducing end> " + customReducingName + "," + customReducedMassOutput + "," + customReducingCCount + "," + customReducingHCount + "," + customReducingOCount + "," + customReducingNCount + Environment.NewLine;
            }
            submitOutput += "<Derivatisation> " + derivatisation + Environment.NewLine;
            submitOutput += "<OffByOne enabled> " + offByOneChecked + Environment.NewLine;
            submitOutput += "## Monosaccharides: Monosaccharide1(Min-Max), Monosaccharide2(Min-Max)" + Environment.NewLine;
            submitOutput += currentMonosaccharideSelection + Environment.NewLine;
            if (monoCustom1 == true || monoCustom2 == true || monoCustom3 == true || monoCustom4 == true || monoCustom5 == true)
            {
                submitOutput += "## CustomMono#: Name, Mass, #C, #H, #N, #O, Min., Max." + Environment.NewLine;
                if (monoCustom1 == true)
                {
                    submitOutput += "<CustomMono1> " + customMono1Name + "," + customMono1Mass + "," + customMono1CCount + "," + customMono1HCount + "," + customMono1NCount + "," + customMono1OCount + "," + customMono1Min + "," + customMono1Max + Environment.NewLine;
                }
                if (monoCustom2 == true)
                {
                    submitOutput += "<CustomMono2> " + customMono2Name + "," + customMono2Mass + "," + customMono2CCount + "," + customMono2HCount + "," + customMono2NCount + "," + customMono2OCount + "," + customMono2Min + "," + customMono2Max + Environment.NewLine;
                }
                if (monoCustom3 == true)
                {
                    submitOutput += "<CustomMono3> " + customMono3Name + "," + customMono3Mass + "," + customMono3CCount + "," + customMono3HCount + "," + customMono3NCount + "," + customMono3OCount + "," + customMono3Min + "," + customMono3Max + Environment.NewLine;
                }
                if (monoCustom4 == true)
                {
                    submitOutput += "<CustomMono4> " + customMono4Name + "," + customMono4Mass + "," + customMono4CCount + "," + customMono4HCount + "," + customMono4NCount + "," + customMono4OCount + "," + customMono4Min + "," + customMono4Max + Environment.NewLine;
                }
                if (monoCustom5 == true)
                {
                    submitOutput += "<CustomMono5> " + customMono5Name + "," + customMono5Mass + "," + customMono5CCount + "," + customMono5HCount + "," + customMono5NCount + "," + customMono5OCount + "," + customMono5Min + "," + customMono5Max + Environment.NewLine;
                }
            }
            submitOutput += "## Adducts: Adduct1, Adduct2" + Environment.NewLine;
            submitOutput += currentAdductSelection + Environment.NewLine;
            File.WriteAllText(string.Concat(saveFileDialog1.FileName.AsSpan(0, saveFileDialog1.FileName.Length - 4), "_parameters.txt"),
                submitOutput
                + "<Precursor targets>"
                + Environment.NewLine
                + targetString);
        }

        public void Sum_up_recursive(List<decimal> numbers, decimal target, List<decimal> partial, bool targetFound, int i)
        {
            decimal s = 0;
            solutionProcess = "";
            solutions = "";

            // Count the number of times we have done this calculation and add more sugars to a given composition
            iterations += 1;
            foreach (decimal x in partial) s += x;

            // Once s is between the required mass range, write a line into solutions that contains all identified compositions
            if (s >= targetLow && s <= targetHigh)
            {
                // Combines each of the solutions for the given mass
                solutions = string.Join("", partial.ToArray());            
                
                // This replaces all the masses with their respective monosaccharide identities
                if (derivatisation == "Native")
                {
                    // Native
                    solutions = solutions.Replace("146.057908", "dHex ").Replace("162.052823", "Hex ").Replace("291.095416", "Neu5Ac ").Replace("307.090331", "Neu5Gc ").Replace("203.079372", "HexNAc ").Replace("79.966331", "Phos ").Replace("79.956815", "Sulf ").Replace(",", "").Replace("161.068808", "HexN ").Replace("176.032088", "HexA ").Replace("187.084458", "dHexNAc ").Replace("132.042258", "Pent ").Replace("250.068867", "KDN ").Replace("273.0848518", "lneuac ").Replace("319.1267166", "eeneuac ").Replace("318.1427011", "dneuac ").Replace("290.1114009", "amneuac ").Replace("42.010565", "acetyl ").Replace("289.0797664", "lneugc ").Replace("335.1216313", "eeneugc ").Replace("306.1063155", "dneugc ").Replace("334.1376157", "amneugc ").Replace(customMono1Mass.ToString(),customMono1Name + " ").Replace(customMono2Mass.ToString(), customMono2Name + " ").Replace(customMono3Mass.ToString(), customMono3Name + " ").Replace(customMono4Mass.ToString(), customMono4Name + " ").Replace(customMono5Mass.ToString(), customMono5Name + " ");
                }
                if (derivatisation == "Permethylated")
                {
                    // Permethylated
                    solutions = solutions.Replace("174.089210", "dHex ").Replace("204.099775", "Hex ").Replace("361.173669", "Neu5Ac ").Replace("391.184234", "Neu5Gc ").Replace("245.126324", "HexNAc ").Replace("93.981983", "Phos ").Replace("79.956815", "Sulf ").Replace(",", "").Replace("203.115758", "HexN ").Replace("218.079040", "HexA ").Replace("215.115759", "dHexNAc ").Replace("160.073560", "Pent ").Replace("320.147120", "KDN ").Replace(customMono1Mass.ToString(), customMono1Name + " ").Replace(customMono2Mass.ToString(), customMono2Name + " ").Replace(customMono3Mass.ToString(), customMono3Name + " ").Replace(customMono4Mass.ToString(), customMono4Name + " ").Replace(customMono5Mass.ToString(), customMono5Name + " ");
                }
                if (derivatisation == "Peracetylated")
                {
                    // Peracetylated
                    solutions = solutions.Replace("230.079038", "dHex ").Replace("288.084517", "Hex ").Replace("417.127110", "Neu5Ac ").Replace("475.132593", "Neu5Gc ").Replace("287.100501", "HexNAc ").Replace("93.981983", "Phos ").Replace("79.956815", "Sulf ").Replace(",", "").Replace("287.100501", "HexN ").Replace("260.053217", "HexA ").Replace("247.105587", "dHexNAc ").Replace("216.063388", "Pent ").Replace("376.100561", "KDN ").Replace(customMono1Mass.ToString(), customMono1Name + " ").Replace(customMono2Mass.ToString(), customMono2Name + " ").Replace(customMono3Mass.ToString(), customMono3Name + " ").Replace(customMono4Mass.ToString(), customMono4Name + " ").Replace(customMono5Mass.ToString(), customMono5Name + " ");
                }

                // This replaces repeated monosaccharide names with 1 monosaccharide name and the number of the occurences
                string solutionsUpdate = "";
                int chemicalFormulaeC = 0;
                int chemicalFormulaeH = 0;
                int chemicalFormulaeO = 0;
                int chemicalFormulaeN = 0;
                int chemicalFormulaeP = 0;
                int chemicalFormulaeS = 0;
                int dHexCount = 0;
                int HexACount = 0;
                int HexNCount = 0;
                int PentCount = 0;
                int KDNCount= 0;
                int hexCount = 0;
                int neuAcCount = 0;
                int neuGcCount = 0;
                int hexNAcCount = 0;
                int phosCount = 0;
                int sulfCount = 0;
                int dhexnacCount = 0;
                int lNeuAcCount = 0;
                int eeNeuAcCount = 0;
                int dNeuAcCount = 0;
                int amNeuAcCount = 0;
                int acetylCount = 0;
                int lNeuGcCount = 0;
                int eeNeuGcCount = 0;
                int dNeuGcCount = 0;
                int amNeuGcCount = 0;
                int customMono1Count = 0;
                int customMono2Count = 0;
                int customMono3Count = 0;
                int customMono4Count = 0;
                int customMono5Count = 0;

                // Native processing
                if (derivatisation == "Native")
                {
                    // Chemical formulae for native
                    dHexCount = Regex.Matches(solutions, "dHex ").Count;
                    if (dHexCount > 0)
                    {
                        chemicalFormulaeC += (dHexCount * 6);
                        chemicalFormulaeH += (dHexCount * 10);
                        chemicalFormulaeO += (dHexCount * 4);
                        solutionsUpdate = solutionsUpdate + "(dHex)" + Convert.ToString(dHexCount) + " ";
                    }
                    HexACount = Regex.Matches(solutions, "HexA ").Count;
                    if (HexACount > 0)
                    {
                        chemicalFormulaeC += (HexACount * 6);
                        chemicalFormulaeH += (HexACount * 8);
                        chemicalFormulaeO += (HexACount * 6);
                        solutionsUpdate = solutionsUpdate + "(HexA)" + Convert.ToString(HexACount) + " ";
                    }
                    HexNCount = Regex.Matches(solutions, "HexN ").Count;
                    if (HexNCount > 0)
                    {
                        chemicalFormulaeC += (HexNCount * 6);
                        chemicalFormulaeH += (HexNCount * 11);
                        chemicalFormulaeO += (HexNCount * 4);
                        chemicalFormulaeN += (HexNCount);
                        solutionsUpdate = solutionsUpdate + "(HexN)" + Convert.ToString(HexNCount) + " ";
                    }
                    PentCount = Regex.Matches(solutions, "Pent ").Count;
                    if (PentCount > 0)
                    {
                        chemicalFormulaeC += (PentCount * 5);
                        chemicalFormulaeH += (PentCount * 8);
                        chemicalFormulaeO += (PentCount * 4);
                        solutionsUpdate = solutionsUpdate + "(Pent)" + Convert.ToString(PentCount) + " ";
                    }
                    KDNCount = Regex.Matches(solutions, "KDN ").Count;
                    if (KDNCount > 0)
                    {
                        chemicalFormulaeC += (KDNCount * 9);
                        chemicalFormulaeH += (KDNCount * 14);
                        chemicalFormulaeO += (KDNCount * 8);
                        solutionsUpdate = solutionsUpdate + "(KDN)" + Convert.ToString(KDNCount) + " ";
                    }
                    hexCount = Regex.Matches(solutions, "Hex ").Count - Regex.Matches(solutions, "dHex ").Count;
                    if (hexCount > 0)
                    {
                        chemicalFormulaeC += (hexCount * 6);
                        chemicalFormulaeH += (hexCount * 10);
                        chemicalFormulaeO += (hexCount * 5);
                        solutionsUpdate = solutionsUpdate + "(Hex)" + Convert.ToString(hexCount) + " ";
                    }
                    neuAcCount = Regex.Matches(solutions, "Neu5Ac ").Count;
                    if (neuAcCount > 0)
                    {
                        chemicalFormulaeC += (neuAcCount * 11);
                        chemicalFormulaeH += (neuAcCount * 17);
                        chemicalFormulaeN += (neuAcCount);
                        chemicalFormulaeO += (neuAcCount * 8);
                        solutionsUpdate = solutionsUpdate + "(NeuAc)" + Convert.ToString(neuAcCount) + " ";
                    }
                    neuGcCount = Regex.Matches(solutions, "Neu5Gc ").Count;
                    if (neuGcCount > 0)
                    {
                        chemicalFormulaeC += (neuGcCount * 11);
                        chemicalFormulaeH += (neuGcCount * 17);
                        chemicalFormulaeN += (neuGcCount);
                        chemicalFormulaeO += (neuGcCount * 9);
                        solutionsUpdate = solutionsUpdate + "(NeuGc)" + Convert.ToString(neuGcCount) + " ";
                    }
                    hexNAcCount = Regex.Matches(solutions, "HexNAc ").Count - Regex.Matches(solutions, "dHexNAc ").Count;
                    if (hexNAcCount > 0)
                    {
                        chemicalFormulaeC += (hexNAcCount * 8);
                        chemicalFormulaeH += (hexNAcCount * 13);
                        chemicalFormulaeN += (hexNAcCount);
                        chemicalFormulaeO += (hexNAcCount * 5);
                        solutionsUpdate = solutionsUpdate + "(HexNAc)" + Convert.ToString(hexNAcCount) + " ";
                    }
                    phosCount = Regex.Matches(solutions, "Phos ").Count;
                    if (phosCount > 0)
                    {
                        chemicalFormulaeH += (phosCount);
                        chemicalFormulaeO += (phosCount * 3);
                        chemicalFormulaeP += (phosCount);
                        solutionsUpdate = solutionsUpdate + "(Phos)" + Convert.ToString(phosCount) + " ";
                    }
                    dhexnacCount = Regex.Matches(solutions, "dHexNAc ").Count;
                    if (dhexnacCount > 0)
                    {
                        chemicalFormulaeC += (dhexnacCount * 8);
                        chemicalFormulaeH += (dhexnacCount * 13);
                        chemicalFormulaeN += (dhexnacCount);
                        chemicalFormulaeO += (dhexnacCount * 4);
                        solutionsUpdate = solutionsUpdate + "(dHexNAc)" + Convert.ToString(dhexnacCount) + " ";
                    }
                    lNeuAcCount = Regex.Matches(solutions, "lneuac ").Count;
                    if (lNeuAcCount > 0)
                    {
                        chemicalFormulaeC += (lNeuAcCount * 11);
                        chemicalFormulaeH += (lNeuAcCount * 15);
                        chemicalFormulaeN += (lNeuAcCount);
                        chemicalFormulaeO += (lNeuAcCount * 7);
                        solutionsUpdate = solutionsUpdate + "(lNeuAc)" + Convert.ToString(lNeuAcCount) + " ";
                    }
                    eeNeuAcCount = Regex.Matches(solutions, "eeneuac ").Count;
                    if (eeNeuAcCount > 0)
                    {
                        chemicalFormulaeC += (eeNeuAcCount * 13);
                        chemicalFormulaeH += (eeNeuAcCount * 21);
                        chemicalFormulaeN += (eeNeuAcCount);
                        chemicalFormulaeO += (eeNeuAcCount * 8);
                        solutionsUpdate = solutionsUpdate + "(eNeuAc)" + Convert.ToString(eeNeuAcCount) + " ";
                    }
                    dNeuAcCount = Regex.Matches(solutions, "dneuac ").Count;
                    if (dNeuAcCount > 0)
                    {
                        chemicalFormulaeC += (dNeuAcCount * 13);
                        chemicalFormulaeH += (dNeuAcCount * 22);
                        chemicalFormulaeN += (dNeuAcCount * 2);
                        chemicalFormulaeO += (dNeuAcCount * 7);
                        solutionsUpdate = solutionsUpdate + "(dNeuAc)" + Convert.ToString(dNeuAcCount) + " ";
                    }
                    amNeuAcCount = Regex.Matches(solutions, "amneuac ").Count;
                    if (amNeuAcCount > 0)
                    {
                        chemicalFormulaeC += (amNeuAcCount * 11);
                        chemicalFormulaeH += (amNeuAcCount * 18);
                        chemicalFormulaeN += (amNeuAcCount * 2);
                        chemicalFormulaeO += (amNeuAcCount * 7);
                        solutionsUpdate = solutionsUpdate + "(amNeuAc)" + Convert.ToString(amNeuAcCount) + " ";
                    }
                    acetylCount = Regex.Matches(solutions, "acetyl ").Count;
                    if (acetylCount > 0)
                    {
                        chemicalFormulaeC += (dhexnacCount * 2);
                        chemicalFormulaeH += (dhexnacCount * 2);
                        chemicalFormulaeO += (dhexnacCount * 1);
                        solutionsUpdate = solutionsUpdate + "(acetyl)" + Convert.ToString(acetylCount) + " ";
                    }
                    lNeuGcCount = Regex.Matches(solutions, "lneugc ").Count;
                    if (lNeuGcCount > 0)
                    {
                        chemicalFormulaeC += (lNeuGcCount * 11);
                        chemicalFormulaeH += (lNeuGcCount * 15);
                        chemicalFormulaeN += (lNeuGcCount);
                        chemicalFormulaeO += (lNeuGcCount * 8);
                        solutionsUpdate = solutionsUpdate + "(lNeuGc)" + Convert.ToString(lNeuGcCount) + " ";
                    }
                    eeNeuGcCount = Regex.Matches(solutions, "eeneugc ").Count;
                    if (eeNeuGcCount > 0)
                    {
                        chemicalFormulaeC += (eeNeuGcCount * 13);
                        chemicalFormulaeH += (eeNeuGcCount * 21);
                        chemicalFormulaeN += (eeNeuGcCount);
                        chemicalFormulaeO += (eeNeuGcCount * 9);
                        solutionsUpdate = solutionsUpdate + "(eNeuGc)" + Convert.ToString(eeNeuGcCount) + " ";
                    }
                    dNeuGcCount = Regex.Matches(solutions, "dneugc ").Count;
                    if (dNeuGcCount > 0)
                    {
                        chemicalFormulaeC += (dNeuGcCount * 13);
                        chemicalFormulaeH += (dNeuGcCount * 22);
                        chemicalFormulaeN += (dNeuGcCount * 2);
                        chemicalFormulaeO += (dNeuGcCount * 8);
                        solutionsUpdate = solutionsUpdate + "(dNeuGc)" + Convert.ToString(dNeuGcCount) + " ";
                    }
                    amNeuGcCount = Regex.Matches(solutions, "amneugc ").Count;
                    if (amNeuGcCount > 0)
                    {
                        chemicalFormulaeC += (amNeuGcCount * 11);
                        chemicalFormulaeH += (amNeuGcCount * 18);
                        chemicalFormulaeN += (amNeuGcCount * 2);
                        chemicalFormulaeO += (amNeuGcCount * 8);
                        solutionsUpdate = solutionsUpdate + "(amNeuGc)" + Convert.ToString(amNeuGcCount) + " ";
                    }

                    switch (reducedEnd)
                    {
                        case "Free":
                            chemicalFormulaeH += 2;
                            chemicalFormulaeO += 1;
                            break;
                        case "Reduced":
                            chemicalFormulaeH += 4;
                            chemicalFormulaeO += 1;
                            break;
                        case "InstantPC":
                            chemicalFormulaeC += 14;
                            chemicalFormulaeH += 21;
                            chemicalFormulaeN += 3;
                            chemicalFormulaeO += 3;
                            break;
                        case "Rapifluor-MS":
                            chemicalFormulaeC += 17;
                            chemicalFormulaeH += 23;
                            chemicalFormulaeN += 5;
                            chemicalFormulaeO += 2;
                            break;
                        case "2AA":
                            chemicalFormulaeC += 7;
                            chemicalFormulaeH += 9;
                            chemicalFormulaeN += 1;
                            chemicalFormulaeO += 2;
                            break;
                        case "2AB":
                            chemicalFormulaeC += 7;
                            chemicalFormulaeH += 10;
                            chemicalFormulaeN += 2;
                            chemicalFormulaeO += 1;
                            break;
                        case "Procainamide":
                            chemicalFormulaeC += 13;
                            chemicalFormulaeH += 23;
                            chemicalFormulaeN += 3;
                            chemicalFormulaeO += 1;
                            break;
                        case "girP":
                            chemicalFormulaeC += 7;
                            chemicalFormulaeH += 10;
                            chemicalFormulaeN += 3;
                            chemicalFormulaeO += 1;
                            break;
                        case "Custom":
                            chemicalFormulaeC += customReducingCCount;
                            chemicalFormulaeH += customReducingHCount;
                            chemicalFormulaeN += customReducingNCount;
                            chemicalFormulaeO += customReducingOCount;
                            break;
                        default:
                            break;
                    }
                }
                // Permethylated processing
                if (derivatisation == "Permethylated")
                {
                    // Chemical formulae for permethylated
                    dHexCount = Regex.Matches(solutions, "dHex ").Count;
                    if (dHexCount > 0)
                    {
                        chemicalFormulaeC += (dHexCount * 8);
                        chemicalFormulaeH += (dHexCount * 14);
                        chemicalFormulaeO += (dHexCount * 4);
                        solutionsUpdate = solutionsUpdate + "(dHex)" + Convert.ToString(dHexCount) + " ";
                    }
                    HexACount = Regex.Matches(solutions, "HexA ").Count;
                    if (HexACount > 0)
                    {
                        chemicalFormulaeC += (HexACount * 9);
                        chemicalFormulaeH += (HexACount * 14);
                        chemicalFormulaeO += (HexACount * 6);
                        solutionsUpdate = solutionsUpdate + "(HexA)" + Convert.ToString(HexACount) + " ";
                    }
                    HexNCount = Regex.Matches(solutions, "HexN ").Count;
                    if (HexNCount > 0)
                    {
                        chemicalFormulaeC += (HexNCount * 9);
                        chemicalFormulaeH += (HexNCount * 17);
                        chemicalFormulaeO += (HexNCount * 4);
                        chemicalFormulaeN += (HexNCount);
                        solutionsUpdate = solutionsUpdate + "(HexN)" + Convert.ToString(HexNCount) + " ";
                    }
                    PentCount = Regex.Matches(solutions, "Pent ").Count;
                    if (PentCount > 0)
                    {
                        chemicalFormulaeC += (PentCount * 7);
                        chemicalFormulaeH += (PentCount * 12);
                        chemicalFormulaeO += (PentCount * 4);
                        solutionsUpdate = solutionsUpdate + "(Pent)" + Convert.ToString(PentCount) + " ";
                    }
                    KDNCount = Regex.Matches(solutions, "KDN ").Count;
                    if (KDNCount > 0)
                    {
                        chemicalFormulaeC += (KDNCount * 14);
                        chemicalFormulaeH += (KDNCount * 24);
                        chemicalFormulaeO += (KDNCount * 8);
                        solutionsUpdate = solutionsUpdate + "(KDN)" + Convert.ToString(KDNCount) + " ";
                    }
                    hexCount = Regex.Matches(solutions, "Hex ").Count - Regex.Matches(solutions, "dHex ").Count;
                    if (hexCount > 0)
                    {
                        chemicalFormulaeC += (hexCount * 9);
                        chemicalFormulaeH += (hexCount * 16);
                        chemicalFormulaeO += (hexCount * 5);
                        solutionsUpdate = solutionsUpdate + "(Hex)" + Convert.ToString(hexCount) + " ";
                    }
                    neuAcCount = Regex.Matches(solutions, "Neu5Ac ").Count;
                    if (neuAcCount > 0)
                    {
                        chemicalFormulaeC += (neuAcCount * 16);
                        chemicalFormulaeH += (neuAcCount * 27);
                        chemicalFormulaeN += (neuAcCount);
                        chemicalFormulaeO += (neuAcCount * 8);
                        solutionsUpdate = solutionsUpdate + "(NeuAc)" + Convert.ToString(neuAcCount) + " ";
                    }
                    neuGcCount = Regex.Matches(solutions, "Neu5Gc ").Count;
                    if (neuGcCount > 0)
                    {
                        chemicalFormulaeC += (neuGcCount * 17);
                        chemicalFormulaeH += (neuGcCount * 29);
                        chemicalFormulaeN += (neuGcCount);
                        chemicalFormulaeO += (neuGcCount * 9);
                        solutionsUpdate = solutionsUpdate + "(NeuGc)" + Convert.ToString(neuGcCount) + " ";
                    }
                    hexNAcCount = Regex.Matches(solutions, "HexNAc ").Count - Regex.Matches(solutions, "dHexNAc ").Count;
                    if (hexNAcCount > 0)
                    {
                        chemicalFormulaeC += (hexNAcCount * 11);
                        chemicalFormulaeH += (hexNAcCount * 19);
                        chemicalFormulaeN += (hexNAcCount);
                        chemicalFormulaeO += (hexNAcCount * 5);
                        solutionsUpdate = solutionsUpdate + "(HexNAc)" + Convert.ToString(hexNAcCount) + " ";
                    }
                    phosCount = Regex.Matches(solutions, "Phos ").Count;
                    if (phosCount > 0)
                    {
                        chemicalFormulaeC += (phosCount);
                        chemicalFormulaeH += (phosCount * 3);
                        chemicalFormulaeO += (phosCount * 3);
                        chemicalFormulaeP += (phosCount);
                        solutionsUpdate = solutionsUpdate + "(Phos)" + Convert.ToString(phosCount) + " ";
                    }
                    dhexnacCount = Regex.Matches(solutions, "dHexNAc ").Count;
                    if (dhexnacCount > 0)
                    {
                        chemicalFormulaeC += (dhexnacCount * 10);
                        chemicalFormulaeH += (dhexnacCount * 17);
                        chemicalFormulaeN += (dhexnacCount);
                        chemicalFormulaeO += (dhexnacCount * 4);
                        solutionsUpdate = solutionsUpdate + "(dHexNAc)" + Convert.ToString(dhexnacCount) + " ";
                    }
                    sulfCount = Regex.Matches(solutions, "Sulf ").Count;
                    if (sulfCount > 0)
                    {
                        chemicalFormulaeC += (sulfCount * -1);
                        chemicalFormulaeH += (sulfCount * -2);
                        chemicalFormulaeO += (sulfCount * 3);
                        chemicalFormulaeS += (sulfCount);
                        solutionsUpdate = solutionsUpdate + "(Sulf)" + Convert.ToString(sulfCount) + " ";
                    }
                    switch (reducedEnd)
                    {
                        case "Free":
                            chemicalFormulaeC += 2;
                            chemicalFormulaeH += 6;
                            chemicalFormulaeO += 1;
                            break;
                        case "Reduced":
                            chemicalFormulaeC += 3;
                            chemicalFormulaeH += 10;
                            chemicalFormulaeO += 1;
                            break;
                        case "Custom":
                            chemicalFormulaeC += customReducingCCount;
                            chemicalFormulaeH += customReducingHCount;
                            chemicalFormulaeN += customReducingNCount;
                            chemicalFormulaeO += customReducingOCount;
                            break;
                        default:
                            break;
                    }
                }
                // peracetylated processing
                if (derivatisation == "Peracetylated")
                {
                    // Chemical formulae for peracetylated
                    dHexCount = Regex.Matches(solutions, "dHex ").Count;
                    if (dHexCount > 0)
                    {
                        chemicalFormulaeC += (dHexCount * 10);
                        chemicalFormulaeH += (dHexCount * 14);
                        chemicalFormulaeO += (dHexCount * 6);
                        solutionsUpdate = solutionsUpdate + "(dHex)" + Convert.ToString(dHexCount) + " ";
                    }
                    HexACount = Regex.Matches(solutions, "HexA ").Count;
                    if (HexACount > 0)
                    {
                        chemicalFormulaeC += (HexACount * 10);
                        chemicalFormulaeH += (HexACount * 12);
                        chemicalFormulaeO += (HexACount * 8);
                        solutionsUpdate = solutionsUpdate + "(HexA)" + Convert.ToString(HexACount) + " ";
                    }
                    HexNCount = Regex.Matches(solutions, "HexN ").Count;
                    if (HexNCount > 0)
                    {
                        chemicalFormulaeC += (HexNCount * 12);
                        chemicalFormulaeH += (HexNCount * 17);
                        chemicalFormulaeO += (HexNCount * 7);
                        chemicalFormulaeN += (HexNCount);
                        solutionsUpdate = solutionsUpdate + "(HexN)" + Convert.ToString(HexNCount) + " ";
                    }
                    PentCount = Regex.Matches(solutions, "Pent ").Count;
                    if (PentCount > 0)
                    {
                        chemicalFormulaeC += (PentCount * 9);
                        chemicalFormulaeH += (PentCount * 12);
                        chemicalFormulaeO += (PentCount * 6);
                        solutionsUpdate = solutionsUpdate + "(Pent)" + Convert.ToString(PentCount) + " ";
                    }
                    KDNCount = Regex.Matches(solutions, "KDN ").Count;
                    if (KDNCount > 0)
                    {
                        chemicalFormulaeC += (KDNCount * 15);
                        chemicalFormulaeH += (KDNCount * 28);
                        chemicalFormulaeO += (KDNCount * 11);
                        solutionsUpdate = solutionsUpdate + "(KDN)" + Convert.ToString(KDNCount) + " ";
                    }
                    hexCount = Regex.Matches(solutions, "Hex ").Count - Regex.Matches(solutions, "dHex ").Count;
                    if (hexCount > 0)
                    {
                        chemicalFormulaeC += (hexCount * 12);
                        chemicalFormulaeH += (hexCount * 16);
                        chemicalFormulaeO += (hexCount * 8);
                        solutionsUpdate = solutionsUpdate + "(Hex)" + Convert.ToString(hexCount) + " ";
                    }
                    neuAcCount = Regex.Matches(solutions, "Neu5Ac ").Count;
                    if (neuAcCount > 0)
                    {
                        chemicalFormulaeC += (neuAcCount * 17);
                        chemicalFormulaeH += (neuAcCount * 23);
                        chemicalFormulaeN += (neuAcCount);
                        chemicalFormulaeO += (neuAcCount * 11);
                        solutionsUpdate = solutionsUpdate + "(NeuAc)" + Convert.ToString(neuAcCount) + " ";
                    }
                    neuGcCount = Regex.Matches(solutions, "Neu5Gc ").Count;
                    if (neuGcCount > 0)
                    {
                        chemicalFormulaeC += (neuGcCount * 19);
                        chemicalFormulaeH += (neuGcCount * 25);
                        chemicalFormulaeN += (neuGcCount);
                        chemicalFormulaeO += (neuGcCount * 13);
                        solutionsUpdate = solutionsUpdate + "(NeuGc)" + Convert.ToString(neuGcCount) + " ";
                    }
                    hexNAcCount = Regex.Matches(solutions, "HexNAc ").Count - Regex.Matches(solutions, "dHexNAc ").Count;
                    if (hexNAcCount > 0)
                    {
                        chemicalFormulaeC += (hexNAcCount * 12);
                        chemicalFormulaeH += (hexNAcCount * 17);
                        chemicalFormulaeN += (hexNAcCount);
                        chemicalFormulaeO += (hexNAcCount * 7);
                        solutionsUpdate = solutionsUpdate + "(HexNAc)" + Convert.ToString(hexNAcCount) + " ";
                    }
                    phosCount = Regex.Matches(solutions, "Phos ").Count;
                    if (phosCount > 0)
                    {
                        chemicalFormulaeC += (phosCount * -2);
                        chemicalFormulaeH += (phosCount * -1);
                        chemicalFormulaeO += (phosCount * 2);
                        chemicalFormulaeP += (phosCount);
                        solutionsUpdate = solutionsUpdate + "(Phos)" + Convert.ToString(phosCount) + " ";
                    }
                    dhexnacCount = Regex.Matches(solutions, "dHexNAc ").Count;
                    if (dhexnacCount > 0)
                    {
                        chemicalFormulaeC += (dhexnacCount * 10);
                        chemicalFormulaeH += (dhexnacCount * 17);
                        chemicalFormulaeN += (dhexnacCount);
                        chemicalFormulaeO += (dhexnacCount * 6);
                        solutionsUpdate = solutionsUpdate + "(dHexNAc)" + Convert.ToString(dhexnacCount) + " ";
                    }
                    sulfCount = Regex.Matches(solutions, "Sulf ").Count;
                    if (sulfCount > 0)
                    {
                        chemicalFormulaeC += (sulfCount * -2);
                        chemicalFormulaeH += (sulfCount * -2);
                        chemicalFormulaeO += (sulfCount * 2);
                        chemicalFormulaeS += (sulfCount);
                        solutionsUpdate = solutionsUpdate + "(Sulf)" + Convert.ToString(sulfCount) + " ";
                    }
                    switch (reducedEnd)
                    {
                        case "Free":
                            chemicalFormulaeC += 4;
                            chemicalFormulaeH += 6;
                            chemicalFormulaeO += 3;
                            break;
                        case "Reduced":
                            chemicalFormulaeC += 6;
                            chemicalFormulaeH += 10;
                            chemicalFormulaeO += 4;
                            break;
                        case "Custom":
                            chemicalFormulaeC += customReducingCCount;
                            chemicalFormulaeH += customReducingHCount;
                            chemicalFormulaeN += customReducingNCount;
                            chemicalFormulaeO += customReducingOCount;
                            break;
                        default:
                            break;
                    }
                }

                // Custom monosaccharides are independent of derivatisation status
                customMono1Count = Regex.Matches(solutions, customMono1Name + " ").Count;
                if (customMono1Count > 0)
                {
                    chemicalFormulaeC += (customMono1Count * customMono1CCount);
                    chemicalFormulaeH += (customMono1Count * customMono1HCount);
                    chemicalFormulaeN += (customMono1Count * customMono1NCount);
                    chemicalFormulaeO += (customMono1Count * customMono1OCount);
                    solutionsUpdate = solutionsUpdate + "(" + customMono1Name + ")" + Convert.ToString(customMono1Count) + " ";
                }
                customMono2Count = Regex.Matches(solutions, customMono2Name + " ").Count;
                if (customMono2Count > 0)
                {
                    chemicalFormulaeC += (customMono2Count * customMono2CCount);
                    chemicalFormulaeH += (customMono2Count * customMono2HCount);
                    chemicalFormulaeN += (customMono2Count * customMono2NCount);
                    chemicalFormulaeO += (customMono2Count * customMono2OCount);
                    solutionsUpdate = solutionsUpdate + "(" + customMono2Name + ")" + Convert.ToString(customMono2Count) + " ";
                }
                customMono3Count = Regex.Matches(solutions, customMono3Name + " ").Count;
                if (customMono3Count > 0)
                {
                    chemicalFormulaeC += (customMono3Count * customMono3CCount);
                    chemicalFormulaeH += (customMono3Count * customMono3HCount);
                    chemicalFormulaeN += (customMono3Count * customMono3NCount);
                    chemicalFormulaeO += (customMono3Count * customMono3OCount);
                    solutionsUpdate = solutionsUpdate + "(" + customMono3Name + ")" + Convert.ToString(customMono3Count) + " ";
                }
                customMono4Count = Regex.Matches(solutions, customMono4Name + " ").Count;
                if (customMono4Count > 0)
                {
                    chemicalFormulaeC += (customMono4Count * customMono4CCount);
                    chemicalFormulaeH += (customMono4Count * customMono4HCount);
                    chemicalFormulaeN += (customMono4Count * customMono4NCount);
                    chemicalFormulaeO += (customMono4Count * customMono4OCount);
                    solutionsUpdate = solutionsUpdate + "(" + customMono4Name + ")" + Convert.ToString(customMono4Count) + " ";
                }
                customMono5Count = Regex.Matches(solutions, customMono5Name + " ").Count;
                if (customMono5Count > 0)
                {
                    chemicalFormulaeC += (customMono5Count * customMono5CCount);
                    chemicalFormulaeH += (customMono5Count * customMono5HCount);
                    chemicalFormulaeN += (customMono5Count * customMono5NCount);
                    chemicalFormulaeO += (customMono5Count * customMono5OCount);
                    solutionsUpdate = solutionsUpdate + "(" + customMono5Name + ")" + Convert.ToString(customMono5Count) + " ";
                }

                // Preparation to export a chemical formulae in a format compatible with Skyline
                string chemicalFormula = "C" + chemicalFormulaeC + "H" + chemicalFormulaeH + "N" + chemicalFormulaeN + "O" + chemicalFormulaeO + "P" + chemicalFormulaeP + "S" + chemicalFormulaeS;
                chemicalFormula = chemicalFormula.Replace("N0", "").Replace("P0", "").Replace("S0", "");

                // Reducing end status: native, permethylated, or peracetylated
                if (derivatisation == "Native")
                {
                    switch (reducedEnd)
                    {
                        case "Free":
                            observedMass = s + 18.010565m;
                            theoreticalMass = target + 18.010565m;
                            break;
                        case "Reduced":
                            observedMass = s + 20.026195m;
                            theoreticalMass = target + 20.026195m;
                            break;
                        case "InstantPC":
                            observedMass = s + 18.010565m + 261.1477m;
                            theoreticalMass = target + 18.010565m + 261.1477m;
                            break;
                        case "Rapifluor-MS":
                            observedMass = s + 18.010565m + 311.17461m;
                            theoreticalMass = target + 18.010565m + 311.17461m;
                            break;
                        case "2AA":
                            observedMass = s + 18.010565m + 121.052774m;
                            theoreticalMass = target + 18.010565m + 121.052774m;
                            break;
                        case "2AB":
                            observedMass = s + 18.010565m + 120.068758m;
                            theoreticalMass = target + 18.010565m + 120.068758m;
                            break;
                        case "Procainamide":
                            observedMass = s + 18.010565m + 219.1735574m;
                            theoreticalMass = target + 18.010565m + 219.1735574m;
                            break;
                        case "girP":
                            observedMass = s + 18.010565m + 134.06405m;
                            theoreticalMass = target + 18.010565m + 134.06405m;
                            break;
                        case "Custom":
                            observedMass = s + 18.010565m + customReducingMass;
                            theoreticalMass = target + 18.010565m + customReducingMass;
                            break;
                        default:
                            break;
                    }
                }
                if (derivatisation == "Permethylated")
                {
                    // Permethylated
                    switch (reducedEnd)
                    {
                        case "Free":
                            observedMass = s + 18.010565m + 28.031300m;
                            theoreticalMass = target + 18.010565m + 28.031300m;
                            break;
                        case "Reduced":
                            observedMass = s + 20.026195m + 42.046950m;
                            theoreticalMass = target + 20.026195m + 42.046950m;
                            break;
                        case "Custom":
                            observedMass = s + 18.010565m + customReducingMass;
                            theoreticalMass = target + 18.010565m + customReducingMass;
                            break;
                        default:
                            break;
                    }
                }
                if (derivatisation == "Peracetylated")
                {
                    // Peracetylated
                    switch (reducedEnd)
                    {
                        case "Free":
                            observedMass = s + 18.010565m + 84.021129m;
                            theoreticalMass = target + 18.010565m + 84.021129m;
                            break;
                        case "Reduced":
                            observedMass = s + 20.026195m + 126.031694m;
                            theoreticalMass = target + 20.026195m + 126.031694m;
                            break;
                        case "Custom":
                            observedMass = s + 18.010565m + customReducingMass;
                            theoreticalMass = target + 18.010565m + customReducingMass;
                            break;
                        default:
                            break;
                    }
                }

                // Calculation for mass error
                error = observedMass - theoreticalMass;

                // Calculation of scan number and charge state to be represented later
                targetIndex.Add(i);
                if (inputChecked == "mzML")
                {
                    string scanNumberForOutput = "";
                    string chargeForOutput = "";
                    string retentionTimeForOutput = "";
                    string TICForOutput = "";
                    string FileForOutput = "";

                    // mzml input therefore output needs to be include scan #, charge, RT and TIC values.
                    // Adducts multiply the target list, this step ensures that we can assign metadata to all of the targets (otherwise it looks for targets that aren't there)
                    for (int z = 0; z < searchRepeats; z++)
                    {
                        int index = i % scans.Count;
                        scanNumberForOutput = Convert.ToString(scans.ElementAt(index));
                        chargeForOutput = Convert.ToString(charges.ElementAt(index));
                        retentionTimeForOutput = Convert.ToString(retentionTimes.ElementAt(index));
                        TICForOutput = Convert.ToString(TICs.ElementAt(index));
                        FileForOutput = Convert.ToString(files.ElementAt(index));

                        // OffByOne error essentially doubles the target list, need to ensure that we can assign metadata to the +1 targets (otherwise it tries to call metadata from a limited list)
                        if (offByOneChecked == true)
                        {
                            // Repeat the logic based on the condition
                            index = (i + 1) % scans.Count;
                            scanNumberForOutput = Convert.ToString(scans.ElementAt(index));
                            chargeForOutput = Convert.ToString(charges.ElementAt(index));
                            retentionTimeForOutput = Convert.ToString(retentionTimes.ElementAt(index));
                            TICForOutput = Convert.ToString(TICs.ElementAt(index));
                            FileForOutput = Convert.ToString(files.ElementAt(index));
                        }
                    }

                    // Adding of each string component to output
                    solutionProcess += solutionsUpdate + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + theoreticalMass + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + observedMass + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + chemicalFormula + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + error + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + scanNumberForOutput + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + chargeForOutput + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + retentionTimeForOutput + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + TICForOutput + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + FileForOutput + Environment.NewLine;
                }
                else
                {
                    // just text input, so no charge state, scan number, RT, or TIC info
                    solutionProcess += solutionsUpdate + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + theoreticalMass + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + observedMass + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + chemicalFormula + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + error + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + Environment.NewLine;
                }


                // Method to remove all compositions outside of user-set bounds
                int outOfBounds = 0;


                if (hexCount < HexMin_int
                    || hexCount > HexMax_int)
                {
                    outOfBounds += 1;
                }
                if (hexNAcCount < HexNAcMin_int
                    || hexNAcCount > HexNAcMax_int)
                {
                    outOfBounds += 1;
                }
                if (dHexCount < dHexMin_int
                    || dHexCount > dHexMax_int)
                {
                    outOfBounds += 1;
                }
                if (HexACount < HexAMin_int
                    || HexACount > HexAMax_int)
                {
                    outOfBounds += 1;
                }
                if (HexNCount < HexNMin_int
                    || HexNCount > HexNMax_int)
                {
                    outOfBounds += 1;
                }
                if (PentCount < PentMin_int
                    || PentCount > PentMax_int)
                {
                    outOfBounds += 1;
                }
                if (KDNCount < KDNMin_int
                    || KDNCount > KDNMax_int)
                {
                    outOfBounds += 1;
                }
                if (neuAcCount < Neu5AcMin_int
                    || neuAcCount > Neu5AcMax_int)
                {
                    outOfBounds += 1;
                }
                if (neuGcCount < Neu5GcMin_int
                    || neuGcCount > Neu5GcMax_int)
                {
                    outOfBounds += 1;
                }
                if (phosCount < PhosMin_int
                    || phosCount > PhosMax_int)
                {
                    outOfBounds += 1;
                }
                if (sulfCount < SulfMin_int
                    || sulfCount > SulfMax_int)
                {
                    outOfBounds += 1;
                }
                if (dhexnacCount < dHexNAcMin_int
                    || dhexnacCount > dHexNAcMax_int)
                {
                    outOfBounds += 1;
                }
                if (lNeuAcCount < lNeuAcMin_int
                    || lNeuAcCount > lNeuAcMax_int)
                {
                    outOfBounds += 1;
                }
                if (eeNeuAcCount < eeNeuAcMin_int
                    || eeNeuAcCount > eeNeuAcMax_int)
                {
                    outOfBounds += 1;
                }
                if (dNeuAcCount < dNeuAcMin_int
                    || dNeuAcCount > dNeuAcMax_int)
                {
                    outOfBounds += 1;
                }
                if (amNeuAcCount < amNeuAcMin_int
                    || amNeuAcCount > amNeuAcMax_int)
                {
                    outOfBounds += 1;
                }
                if (acetylCount < acetylMin_int
                    || acetylCount > acetylMax_int)
                {
                    outOfBounds += 1;
                }
                if (lNeuGcCount < lNeuGcMin_int
                    || lNeuGcCount > lNeuGcMax_int)
                {
                    outOfBounds += 1;
                }
                if (eeNeuGcCount < eeNeuGcMin_int
                    || eeNeuGcCount > eeNeuGcMax_int)
                {
                    outOfBounds += 1;
                }
                if (dNeuGcCount < dNeuGcMin_int
                    || dNeuGcCount > dNeuGcMax_int)
                {
                    outOfBounds += 1;
                }
                if (amNeuGcCount < amNeuGcMin_int
                    || amNeuGcCount > amNeuGcMax_int)
                {
                    outOfBounds += 1;
                }
                if (customMono1Count < customMono1Min
                    || customMono1Count > customMono1Max)
                {
                     outOfBounds += 1;
                }
                if (customMono2Count < customMono2Min
                    || customMono2Count > customMono2Max)
                {
                    outOfBounds += 1;
                }
                if (customMono3Count < customMono3Min
                    || customMono3Count > customMono3Max)
                {
                    outOfBounds += 1;
                }
                if (customMono4Count < customMono4Min
                    || customMono4Count > customMono4Max)
                {
                    outOfBounds += 1;
                }
                if (customMono5Count < customMono5Min
                    || customMono5Count > customMono5Max)
                {
                    outOfBounds += 1;
                }

                // The only solutions that get reported are those that do not fall outside of any specified monosaccharide ranges
                if (outOfBounds == 0)
                {
                    solutionMultiples += solutionProcess.ToString();
                }
            }

            // Give up if the mass is too high
            if (s >= targetHigh)
            {
                return;
            }

            // Keep adding monosaccharides until remainder resets
            // Starting from current index k, each subset of numbers is considered only once
            // By starting the loop at k, each combination is built by progressively adding monosaccharides, avoiding different combinations of the same numbers
            for (int k = 0; k < numbers.Count; k++)
            {
                List<decimal> remaining = [];
                decimal n = numbers[k];
                for (int j = k; j < numbers.Count; j++) remaining.Add(numbers[j]);
                // Combinations are built in a consistent order, avoiding permutations of the same set of monosaccharides
                List<decimal> partial_rec = new(partial)
                {
                    n
                };
                Sum_up_recursive(remaining, target, partial_rec, targetFound, i);
            }
        }

        private void HextoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    Hex_container.Visibility = Visibility.Visible;
                }
                else
                {
                    Hex_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void HexNActoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    HexNAc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    HexNAc_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void dHextoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    dHex_container.Visibility = Visibility.Visible;
                }
                else
                {
                    dHex_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Neu5ActoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    NeuAc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    NeuAc_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Neu5GctoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    NeuGc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    NeuGc_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void HexNtoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    HexN_container.Visibility = Visibility.Visible;
                }
                else
                {
                    HexN_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void HexAtoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    HexA_container.Visibility = Visibility.Visible;
                }
                else
                {
                    HexA_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void dHexNActoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    dHexNAc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    dHexNAc_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void PenttoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    Pent_container.Visibility = Visibility.Visible;
                }
                else
                {
                    Pent_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void KDNtoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    KDN_container.Visibility = Visibility.Visible;
                }
                else
                {
                    KDN_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void PhostoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    Phos_container.Visibility = Visibility.Visible;
                }
                else
                {
                    Phos_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SulftoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    Sulf_container.Visibility = Visibility.Visible;
                }
                else
                {
                    Sulf_container.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void lNeuActoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    lNeuAc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    lNeuAc_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void eNeuActoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    eNeuAc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    eNeuAc_container.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void dNeuActoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    dNeuAc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    dNeuAc_container.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void amNeuActoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    amNeuAc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    amNeuAc_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void AcetyltoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    Acetyl_container.Visibility = Visibility.Visible;
                }
                else
                {
                    Acetyl_container.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void lNeuGctoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    lNeuGc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    lNeuGc_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void eNeuGctoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    eNeuGc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    eNeuGc_container.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void dNeuGctoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    dNeuGc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    dNeuGc_container.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void amNeuGctoggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                UpdateMonosaccharideTextBox();
                currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
                if (toggleSwitch.IsOn == true)
                {
                    amNeuGc_container.Visibility = Visibility.Visible;
                }
                else
                {
                    amNeuGc_container.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            solutionProcess = "";
            solutions = "";
            solutionMultiples = "";
            iterations = 0;
            InputMasses.Text = "";
            filePath = "";
            offByOneChecked = false;
            derivatisation = "";
            DaChecked = false;
            inputChecked = "Text";
            submitbutton.IsEnabled = false;
        }

        private void MzmlRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (customAdductPolarity != null)
            {
                customAdductPolarity.IsEnabled = true;
            }
            browseButton = (Button)FindName("browseButton");
            if (browseButton != null)
            {
                browseButton.IsEnabled = true;
                browseButton.Content = "Browse mzML";
            }
            else
            {
                // Handle the null case
                Console.WriteLine("browseButton is null");
            }
            submitbutton.IsEnabled = false;
            inputChecked = "mzML";
            inputOrLabel.Visibility = Visibility.Collapsed;
            InputMasses.Visibility = Visibility.Collapsed;
            positiveMHCheckBox.IsEnabled = false;
            negativeMHCheckBox.IsEnabled = false;
            positiveMHCheckBox.IsChecked = true;
            negativeMHCheckBox.IsChecked = true;
        }

        private void PresetCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Fast presets
            switch (PresetCombo.SelectedIndex)
            {
                case 0: //N/A
                    HextoggleSwitch.IsOn = false;
                    HexMin.Text = "0";
                    HexMax.Text = "0";
                    HexNActoggleSwitch.IsOn = false;
                    HexNAcMin.Text = "0";
                    HexNAcMax.Text = "0";
                    dHextoggleSwitch.IsOn = false;
                    dHexMin.Text = "0";
                    dHexMax.Text = "0";
                    Neu5ActoggleSwitch.IsOn = false;
                    Neu5AcMin.Text = "0";
                    Neu5AcMax.Text = "0";
                    Neu5GctoggleSwitch.IsOn = false;
                    Neu5GcMin.Text = "0";
                    Neu5GcMax.Text = "0";
                    HexNtoggleSwitch.IsOn = false;
                    HexAtoggleSwitch.IsOn = false;
                    dHexNActoggleSwitch.IsOn = false;
                    PenttoggleSwitch.IsOn = false;
                    KDNtoggleSwitch.IsOn = false;
                    PhostoggleSwitch.IsOn = false;
                    SulftoggleSwitch.IsOn = false;
                    lNeuActoggleSwitch.IsOn = false;
                    eNeuActoggleSwitch.IsOn = false;
                    dNeuActoggleSwitch.IsOn = false;
                    amNeuActoggleSwitch.IsOn = false;
                    AcetyltoggleSwitch.IsOn = false;
                    lNeuGctoggleSwitch.IsOn = false;
                    eNeuGctoggleSwitch.IsOn = false;
                    dNeuGctoggleSwitch.IsOn = false;
                    amNeuGctoggleSwitch.IsOn = false;
                    break;

                case 1: // Mammal NG
                    HextoggleSwitch.IsOn = true;
                    HexMin.Text = "1";
                    HexMax.Text = "12";
                    HexNActoggleSwitch.IsOn = true;
                    HexNAcMin.Text = "2";
                    HexNAcMax.Text = "8";
                    dHextoggleSwitch.IsOn = true;
                    dHexMin.Text = "0";
                    dHexMax.Text = "3";
                    Neu5ActoggleSwitch.IsOn = true;
                    Neu5AcMin.Text = "0";
                    Neu5AcMax.Text = "5";
                    Neu5GctoggleSwitch.IsOn = true;
                    Neu5GcMin.Text = "0";
                    Neu5GcMax.Text = "2";
                    HexNtoggleSwitch.IsOn = false;
                    HexAtoggleSwitch.IsOn = false;
                    dHexNActoggleSwitch.IsOn = false;
                    PenttoggleSwitch.IsOn = false;
                    KDNtoggleSwitch.IsOn = false;
                    PhostoggleSwitch.IsOn = false;
                    SulftoggleSwitch.IsOn = false;
                    lNeuActoggleSwitch.IsOn = false;
                    eNeuActoggleSwitch.IsOn = false;
                    dNeuActoggleSwitch.IsOn = false;
                    amNeuActoggleSwitch.IsOn = false;
                    AcetyltoggleSwitch.IsOn = false;
                    lNeuGctoggleSwitch.IsOn = false;
                    eNeuGctoggleSwitch.IsOn = false;
                    dNeuGctoggleSwitch.IsOn = false;
                    amNeuGctoggleSwitch.IsOn = false;
                    break;

                case 2: // Mammal OG
                    HextoggleSwitch.IsOn = true;
                    HexMin.Text = "0";
                    HexMax.Text = "6";
                    HexNActoggleSwitch.IsOn = true;
                    HexNAcMin.Text = "1";
                    HexNAcMax.Text = "8";
                    dHextoggleSwitch.IsOn = true;
                    dHexMin.Text = "0";
                    dHexMax.Text = "3";
                    Neu5ActoggleSwitch.IsOn = true;
                    Neu5AcMin.Text = "0";
                    Neu5AcMax.Text = "3";
                    Neu5GctoggleSwitch.IsOn = true;
                    Neu5GcMin.Text = "0";
                    Neu5GcMax.Text = "1";
                    HexNtoggleSwitch.IsOn = false;
                    HexAtoggleSwitch.IsOn = false;
                    dHexNActoggleSwitch.IsOn = false;
                    PenttoggleSwitch.IsOn = false;
                    KDNtoggleSwitch.IsOn = false;
                    PhostoggleSwitch.IsOn = false;
                    SulftoggleSwitch.IsOn = true;
                    SulfMin.Text = "0";
                    SulfMax.Text = "2";
                    lNeuActoggleSwitch.IsOn = false;
                    eNeuActoggleSwitch.IsOn = false;
                    dNeuActoggleSwitch.IsOn = false;
                    amNeuActoggleSwitch.IsOn = false;
                    AcetyltoggleSwitch.IsOn = false;
                    lNeuGctoggleSwitch.IsOn = false;
                    eNeuGctoggleSwitch.IsOn = false;
                    dNeuGctoggleSwitch.IsOn = false;
                    amNeuGctoggleSwitch.IsOn = false;
                    break;

                case 3: // GSL
                    HextoggleSwitch.IsOn = true;
                    HexMin.Text = "2";
                    HexMax.Text = "6";
                    HexNActoggleSwitch.IsOn = true;
                    HexNAcMin.Text = "0";
                    HexNAcMax.Text = "4";
                    dHextoggleSwitch.IsOn = true;
                    dHexMin.Text = "0";
                    dHexMax.Text = "3";
                    Neu5ActoggleSwitch.IsOn = true;
                    Neu5AcMin.Text = "0";
                    Neu5AcMax.Text = "5";
                    Neu5GctoggleSwitch.IsOn = true;
                    Neu5GcMin.Text = "0";
                    Neu5GcMax.Text = "1";
                    HexNtoggleSwitch.IsOn = false;
                    HexAtoggleSwitch.IsOn = false;
                    dHexNActoggleSwitch.IsOn = false;
                    PenttoggleSwitch.IsOn = false;
                    KDNtoggleSwitch.IsOn = false;
                    PhostoggleSwitch.IsOn = false;
                    SulftoggleSwitch.IsOn = false;
                    lNeuActoggleSwitch.IsOn = false;
                    eNeuActoggleSwitch.IsOn = false;
                    dNeuActoggleSwitch.IsOn = false;
                    amNeuActoggleSwitch.IsOn = false;
                    AcetyltoggleSwitch.IsOn = false;
                    lNeuGctoggleSwitch.IsOn = false;
                    eNeuGctoggleSwitch.IsOn = false;
                    dNeuGctoggleSwitch.IsOn = false;
                    amNeuGctoggleSwitch.IsOn = false;
                    break;

                case 4: // Plant N-glycan
                    HextoggleSwitch.IsOn = true;
                    HexMin.Text = "0";
                    HexMax.Text = "12";
                    HexNActoggleSwitch.IsOn = true;
                    HexNAcMin.Text = "2";
                    HexNAcMax.Text = "4";
                    dHextoggleSwitch.IsOn = true;
                    dHexMin.Text = "0";
                    dHexMax.Text = "3";
                    Neu5ActoggleSwitch.IsOn = false;
                    Neu5AcMin.Text = "0";
                    Neu5AcMax.Text = "0";
                    Neu5GctoggleSwitch.IsOn = false;
                    Neu5GcMin.Text = "0";
                    Neu5GcMax.Text = "0";
                    HexNtoggleSwitch.IsOn = false;
                    HexAtoggleSwitch.IsOn = false;
                    dHexNActoggleSwitch.IsOn = false;
                    PenttoggleSwitch.IsOn = true;
                    PentMin.Text = "0";
                    PentMax.Text = "1";
                    KDNtoggleSwitch.IsOn = false;
                    PhostoggleSwitch.IsOn = false;
                    SulftoggleSwitch.IsOn = false;
                    lNeuActoggleSwitch.IsOn = false;
                    eNeuActoggleSwitch.IsOn = false;
                    dNeuActoggleSwitch.IsOn = false;
                    amNeuActoggleSwitch.IsOn = false;
                    AcetyltoggleSwitch.IsOn = false;
                    lNeuGctoggleSwitch.IsOn = false;
                    eNeuGctoggleSwitch.IsOn = false;
                    dNeuGctoggleSwitch.IsOn = false;
                    amNeuGctoggleSwitch.IsOn = false;
                    break;

                case 5: // Plant O-glycan
                    HextoggleSwitch.IsOn = true;
                    HexMin.Text = "0";
                    HexMax.Text = "10";
                    HexNActoggleSwitch.IsOn = false;
                    HexNAcMin.Text = "0";
                    HexNAcMax.Text = "0";
                    dHextoggleSwitch.IsOn = false;
                    dHexMin.Text = "0";
                    dHexMax.Text = "0";
                    Neu5ActoggleSwitch.IsOn = false;
                    Neu5AcMin.Text = "0";
                    Neu5AcMax.Text = "0";
                    Neu5GctoggleSwitch.IsOn = false;
                    Neu5GcMin.Text = "0";
                    Neu5GcMax.Text = "0";
                    HexNtoggleSwitch.IsOn = false;
                    HexAtoggleSwitch.IsOn = false;
                    HexAMin.Text = "0";
                    HexAMax.Text = "0";
                    dHexNActoggleSwitch.IsOn = false;
                    PenttoggleSwitch.IsOn = true;
                    PentMin.Text = "0";
                    PentMax.Text = "10";
                    KDNtoggleSwitch.IsOn = false;
                    PhostoggleSwitch.IsOn = false;
                    SulftoggleSwitch.IsOn = false;
                    lNeuActoggleSwitch.IsOn = false;
                    eNeuActoggleSwitch.IsOn = false;
                    dNeuActoggleSwitch.IsOn = false;
                    amNeuActoggleSwitch.IsOn = false;
                    AcetyltoggleSwitch.IsOn = false;
                    lNeuGctoggleSwitch.IsOn = false;
                    eNeuGctoggleSwitch.IsOn = false;
                    dNeuGctoggleSwitch.IsOn = false;
                    amNeuGctoggleSwitch.IsOn = false;
                    break;

                case 6: // Fungal N-glycan
                    HextoggleSwitch.IsOn = true;
                    HexMin.Text = "1";
                    HexMax.Text = "40";
                    HexNActoggleSwitch.IsOn = true;
                    HexNAcMin.Text = "2";
                    HexNAcMax.Text = "2";
                    dHextoggleSwitch.IsOn = false;
                    dHexMin.Text = "0";
                    dHexMax.Text = "0";
                    Neu5ActoggleSwitch.IsOn = false;
                    Neu5AcMin.Text = "0";
                    Neu5AcMax.Text = "0";
                    Neu5GctoggleSwitch.IsOn = false;
                    Neu5GcMin.Text = "0";
                    Neu5GcMax.Text = "0";
                    HexNtoggleSwitch.IsOn = false;
                    HexAtoggleSwitch.IsOn = false;
                    HexAMin.Text = "0";
                    HexAMax.Text = "0";
                    dHexNActoggleSwitch.IsOn = false;
                    PenttoggleSwitch.IsOn = false;
                    PentMin.Text = "0";
                    PentMax.Text = "0";
                    KDNtoggleSwitch.IsOn = false;
                    PhostoggleSwitch.IsOn = true;
                    PhosMin.Text = "0";
                    PhosMax.Text = "2";
                    SulftoggleSwitch.IsOn = true;
                    SulfMin.Text = "0";
                    SulfMax.Text = "2";
                    lNeuActoggleSwitch.IsOn = false;
                    eNeuActoggleSwitch.IsOn = false;
                    dNeuActoggleSwitch.IsOn = false;
                    amNeuActoggleSwitch.IsOn = false;
                    AcetyltoggleSwitch.IsOn = false;
                    lNeuGctoggleSwitch.IsOn = false;
                    eNeuGctoggleSwitch.IsOn = false;
                    dNeuGctoggleSwitch.IsOn = false;
                    amNeuGctoggleSwitch.IsOn = false;
                    break;

                case 7: // Fungal O-glycan
                    HextoggleSwitch.IsOn = true;
                    HexMin.Text = "1";
                    HexMax.Text = "10";
                    HexNActoggleSwitch.IsOn = false;
                    HexNAcMin.Text = "0";
                    HexNAcMax.Text = "0";
                    dHextoggleSwitch.IsOn = false;
                    dHexMin.Text = "0";
                    dHexMax.Text = "0";
                    Neu5ActoggleSwitch.IsOn = false;
                    Neu5AcMin.Text = "0";
                    Neu5AcMax.Text = "0";
                    Neu5GctoggleSwitch.IsOn = false;
                    Neu5GcMin.Text = "0";
                    Neu5GcMax.Text = "0";
                    HexNtoggleSwitch.IsOn = false;
                    HexAtoggleSwitch.IsOn = true;
                    HexAMin.Text = "0";
                    HexAMax.Text = "1";
                    dHexNActoggleSwitch.IsOn = false;
                    PenttoggleSwitch.IsOn = false;
                    PentMin.Text = "0";
                    PentMax.Text = "0";
                    KDNtoggleSwitch.IsOn = false;
                    PhostoggleSwitch.IsOn = false;
                    PhosMin.Text = "0";
                    PhosMax.Text = "0";
                    SulftoggleSwitch.IsOn = false;
                    SulfMin.Text = "0";
                    SulfMax.Text = "0";
                    lNeuActoggleSwitch.IsOn = false;
                    eNeuActoggleSwitch.IsOn = false;
                    dNeuActoggleSwitch.IsOn = false;
                    amNeuActoggleSwitch.IsOn = false;
                    AcetyltoggleSwitch.IsOn = false;
                    lNeuGctoggleSwitch.IsOn = false;
                    eNeuGctoggleSwitch.IsOn = false;
                    dNeuGctoggleSwitch.IsOn = false;
                    amNeuGctoggleSwitch.IsOn = false;
                    break;

                default:
                    //what you want when nothing is selected
                    break;
            }
        }

        private void TextRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            inputChecked = "Text";
            if (customAdductPolarity != null)
            {
                customAdductPolarity.IsEnabled = false;
            }
            browseButton = (Button)FindName("browseButton");
            if (browseButton != null)
            {
                browseButton.IsEnabled = true;
                browseButton.Content = "Browse txt";
            }
            submitbutton = (Button)FindName("submitbutton");
            if (submitbutton != null)
            {
                submitbutton.IsEnabled = true;
            }
            if (inputOrLabel != null && InputMasses != null)
            {
                inputOrLabel.Visibility = Visibility.Visible;
                InputMasses.Visibility = Visibility.Visible;
            }
            if (positiveMHCheckBox != null && negativeMHCheckBox != null) {
                positiveMHCheckBox.IsChecked = false;
                negativeMHCheckBox.IsChecked = false;
                positiveMHCheckBox.IsEnabled = true;
                negativeMHCheckBox.IsEnabled = true;
            }
        }

        private void UpdateMonosaccharideTextBox()
        {
            var toggledMonoSwitches = new StringBuilder();
            toggledMonoSwitches.Append("<Monosaccharides>");
            // Check the state of each ToggleSwitch and build the string accordingly
            if (HextoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" Hex(" + HexMin.Text + "-" + HexMax.Text + "),");
            if (HexNActoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" HexNAc(" + HexNAcMin.Text + "-" + HexNAcMax.Text + "),");
            if (dHextoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" dHex(" + dHexMin.Text + "-" + dHexMax.Text + "),");
            if (Neu5ActoggleSwitch.IsOn)
               toggledMonoSwitches.Append(" NeuAc(" + Neu5AcMin.Text + "-" + Neu5AcMax.Text + "),");
            if (Neu5GctoggleSwitch.IsOn)
               toggledMonoSwitches.Append(" NeuGc(" + Neu5GcMin.Text + "-" + Neu5GcMax.Text + "),");
            if (HexNtoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" HexN(" + HexNMin.Text + "-" + HexNMax.Text + "),");
            if (HexAtoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" HexA(" + HexAMin.Text + "-" + HexAMax.Text + "),");
            if (dHexNActoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" dHexNAc(" + dHexNAcMin.Text + "-" + dHexNAcMax.Text + "),");
            if (PenttoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" Pent(" + PentMin.Text + "-" + PentMax.Text + "),");
            if (KDNtoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" KDN(" + KDNMin.Text + "-" + KDNMax.Text + "),");
            if (PhostoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" Phos(" + PhosMin.Text + "-" + PhosMax.Text + "),");
            if (SulftoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" Sulf(" + SulfMin.Text + "-" + SulfMax.Text + "),");
            if (lNeuActoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" L(" + lNeuAcMin.Text + "-" + lNeuAcMax.Text + "),");
            if (eNeuActoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" E(" + eeNeuAcMin.Text + "-" + eeNeuAcMax.Text + "),");
            if(dNeuActoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" D(" + dNeuAcMin.Text + "-" + dNeuAcMax.Text + "),");
            if (amNeuActoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" Am(" + amNeuAcMin.Text + "-" + amNeuAcMax.Text + "),");
            if (AcetyltoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" Acetyl(" + AcetylMin.Text + "-" + AcetylMax.Text + "),");
            if (lNeuGctoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" LG(" + lNeuGcMin.Text + "-" + lNeuGcMax.Text + "),");
            if (eNeuGctoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" EG(" + eeNeuGcMin.Text + "-" + eeNeuGcMax.Text + "),");
            if (dNeuGctoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" DG(" + dNeuGcMin.Text + "-" + dNeuGcMax.Text + "),");
            if (amNeuGctoggleSwitch.IsOn)
                toggledMonoSwitches.Append(" AmG(" + amNeuGcMin.Text + "-" + amNeuGcMax.Text + "),");
            if (customMonoCheck1.IsChecked == true )
                toggledMonoSwitches.Append(" " + customMonoNameBox1.Text+ "(" + customMonoMinBox1.Text + "-" + customMonoMaxBox1.Text + "),");
            if (customMonoCheck2.IsChecked == true)
                toggledMonoSwitches.Append(" " + customMonoNameBox2.Text + "(" + customMonoMinBox2.Text + "-" + customMonoMaxBox2.Text + "),");
            if (customMonoCheck3.IsChecked == true)
                toggledMonoSwitches.Append(" " + customMonoNameBox3.Text + "(" + customMonoMinBox3.Text + "-" + customMonoMaxBox3.Text + "),");
            if (customMonoCheck4.IsChecked == true)
                toggledMonoSwitches.Append(" " + customMonoNameBox4.Text + "(" + customMonoMinBox4.Text + "-" + customMonoMaxBox4.Text + "),");
            if (customMonoCheck5.IsChecked == true)
                toggledMonoSwitches.Append(" " + customMonoNameBox5.Text + "(" + customMonoMinBox5.Text + "-" + customMonoMaxBox5.Text + "),");
            currentMonosaccharideSelection = toggledMonoSwitches.ToString().TrimEnd(',');
        }

        private void UpdateAdductTextBox()
        {
            var toggledAdducts = new StringBuilder();
            toggledAdducts.Append("<Adducts>");
            // Check the state of each Adduct Checkbox and build the string accordingly
            if (neutralMCheckBox.IsChecked == true)
                toggledAdducts.Append(" [M],");
            if (negativeMHCheckBox.IsChecked == true)
                toggledAdducts.Append(" [M-H⁻]⁻,");
            if (positiveMHCheckBox.IsChecked == true)
                toggledAdducts.Append(" [M+H]⁺,");
            if (negativeMFACheckBox.IsChecked == true)
                toggledAdducts.Append(" [M+COO]⁻,");
            if (positiveMNaCheckBox.IsChecked == true)
                toggledAdducts.Append(" [M+Na]⁺,");
            if (negativeMAACheckBox.IsChecked == true)
                toggledAdducts.Append(" [M+CH₃COO]⁻,");
            if (positiveMKCheckBox.IsChecked == true)
                toggledAdducts.Append(" [M+K]⁺,");
            if (negativeMTFACheckBox.IsChecked == true)
                toggledAdducts.Append(" [M+TFA-H]⁻,");
            if (positiveMNH4CheckBox.IsChecked == true)
                toggledAdducts.Append(" [M+NH₄]⁺,");
            if (customAdductCheckBox.IsChecked == true)
                toggledAdducts.Append(" Custom[M+" + customAdductMassText.Text + "]" + customAdductPolarity.Text + ",");
            currentAdductSelection = toggledAdducts.ToString().TrimEnd(',');
        }

        private void customAdductCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (customAdductCheckBox.IsChecked == true) {
                customAdductMassLabel.Visibility = Visibility.Visible;
                customAdductMassText.Visibility = Visibility.Visible;
                customAdductPolarity.Visibility = Visibility.Visible;
                customAdductPolarityLabel.Visibility = Visibility.Visible;
            }
        }

        private void customAdductCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            customAdductMassLabel.Visibility = Visibility.Collapsed;
            customAdductMassText.Visibility = Visibility.Collapsed;
            customAdductPolarity.Visibility = Visibility.Collapsed;
            customAdductPolarityLabel.Visibility = Visibility.Collapsed;
        }

        private void reducingEndBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reducingEndBox.SelectedIndex == 8)
            {
                customReducingCheck.IsChecked = true;
            }
        }

        private void customReducingCheck_Checked(object sender, RoutedEventArgs e)
        {
            reducingEndBox.SelectedIndex = 8;
            customReducingNameBox.Visibility = Visibility.Visible;
            customReducingMassBox.Visibility = Visibility.Visible;
            customReducingCBox.Visibility = Visibility.Visible;
            customReducingHBox.Visibility = Visibility.Visible;
            customReducingNBox.Visibility = Visibility.Visible;
            customReducingOBox.Visibility = Visibility.Visible;
        }

        private void customReducingCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            reducingEndBox.SelectedIndex = -1;
            customReducingNameBox.Visibility = Visibility.Collapsed;
            customReducingMassBox.Visibility = Visibility.Collapsed;
            customReducingCBox.Visibility = Visibility.Collapsed;
            customReducingHBox.Visibility = Visibility.Collapsed;
            customReducingNBox.Visibility = Visibility.Collapsed;
            customReducingOBox.Visibility = Visibility.Collapsed;
        }

        private void startTab_GotFocus(object sender, RoutedEventArgs e)
        {
            UpdateMonosaccharideTextBox();
            currentMonosaccharideSelectionInfo.Text = currentMonosaccharideSelection;
            UpdateAdductTextBox();
            currentAdductSelectionInfo.Text = currentAdductSelection;
            if (OffByOne.IsChecked == true) {offByOneChecked = true;}    
            else {offByOneChecked = false;}
        }

        private void Da_Checked(object sender, RoutedEventArgs e)
        {
            massErrorType = "Da";
        }

        private void ppm_Checked(object sender, RoutedEventArgs e)
        {
            massErrorType = "ppm";
        }

        private void Native_Checked(object sender, RoutedEventArgs e)
        {
            derivatisation = "Native";
        }

        private void Permeth_Checked(object sender, RoutedEventArgs e)
        {
            derivatisation = "Permethylated";
        }

        private void Peracetyl_Checked(object sender, RoutedEventArgs e)
        {
            derivatisation = "Peracetylated";
        }
    }
}