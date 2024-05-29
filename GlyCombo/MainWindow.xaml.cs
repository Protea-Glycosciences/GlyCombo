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
        private bool reduced;
        private decimal observedMass;
        private decimal theoreticalMass;
        private decimal error;
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
        List<decimal> scans = [];
        List<int> charges = [];
        List<decimal> retentionTimes = [];
        List<decimal> TICs = [];
        List<string> files = [];
        List<int> targetIndex = [];
        // Parameter report variables
        private bool monoHex = false;
        private bool monoHexA = false;
        private bool monodHex = false;
        private bool monoHexNAc = false;
        private bool monoHexN = false;
        private bool monodHexNAc = false;
        private bool monoPent = false;
        private bool monoKDN = false;
        private bool monoNeu5Ac = false;
        private bool monoNeu5Gc = false;
        private bool monoPhos = false;
        private bool monoSulf = false;
        private string errorType;
        private string derivatisation;
        private string param_monoHex;
        private string param_monoHexA;
        private string param_monodHex;
        private string param_monoHexNAc;
        private string param_monoHexN;
        private string param_monodHexNAc;
        private string param_monoPent;
        private string param_monoKDN;
        private string param_monoNeu5Ac;
        private string param_monoNeu5Gc;
        private string param_monoSulf;
        private string param_monoPhos;
        private string filePath;
        private string inputParameters;
        private float ElapsedMSec;

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+.");
            e.Handled = regex.IsMatch(e.Text);
        }

        public MainWindow() => InitializeComponent();
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {

            polarity = "";
            List<decimal> precursors = [];

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

            foreach (String file in openFileDialog.FileNames)
            {
                // Going to process each file one at a time using this section of the code.
                // Read each line from the given file
                StreamReader sr = new(file);

                // Parse each line of the mzml to extract important information from MS2 scans of the mzML (polarity, precursor m/z, charge state, scan # for given MS2)
                while ((line = sr.ReadLine()) != null)
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
                new Thread(() => { MessageBox.Show(fileNameOutput + " loaded with " + scans.Count + " MS2 scans."); }).Start();
            }


            // Let the user now start the processing. Without this step, the user may crash the program by starting the processing before the mzml info is extracted
            submitbutton.IsEnabled = IsEnabled;
        }


        // Execution of the combinatorial analysis
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            solutionMultiples = "";
            resetbutton.IsEnabled = IsEnabled;
            var watch = Stopwatch.StartNew();
            // Define the components in the combinatorial analysis, native and permethylated
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
            }
            else
            {
                derivatisation = "Permethylated";
                // Permethylated
                dhex = 174.089210m; // permethylated mass =  chemical formula = C8H14O4
                hex = 204.099775m; // permethylated mass =  chemical formula = C9H16O5
                hexnac = 245.126324m; // permethylated mass =  chemical formula = C11H19NO5
                hexn = 217.131409m; // permethylated mass =  chemical formula = C10H19NO4
                hexa = 218.079040m; // permethylated mass =  chemical formula = C9H14O6
                dhexnac = 215.115759m; // permethylated mass =  chemical formula = C10H17N1O4
                pent = 160.073560m; // permethylated mass =  chemical formula = C7H12O4
                kdn = 320.147120m; // permethylated mass =  chemical formula = C14H24O8
                neuac = 361.173669m; // permethylated mass =  chemical formula = C16H27NO8
                neugc = 391.184234m; // permethylated mass =  chemical formula = C17H29NO9
                phos = 93.981983m; // permethylated mass =  chemical formula = CH3O3P
            }
            decimal sulf = 79.956815m; // Nothing changes with permethylation

            // Add the components to combinatorial analysis based on which monosaccharides the user chooses to include
            List<decimal> numbers = [];
            if (HextoggleSwitch.IsOn == true)
            {
                numbers.Add(hex);
                monoHex = true;
            }

            if (HexAtoggleSwitch.IsOn == true)
            {
                numbers.Add(hexa);
                monoHexA = true;
            }
            if (dHextoggleSwitch.IsOn == true)
            {
                numbers.Add(dhex);
                monodHex = true;
            }
            if (HexNActoggleSwitch.IsOn == true)
            {
                numbers.Add(hexnac);
                monoHexNAc = true;
            }
            if (HexNtoggleSwitch.IsOn == true)
            {
                numbers.Add(hexn);
                monoHexN = true;
            }
            if (dHexNActoggleSwitch.IsOn == true)
            {
                numbers.Add(dhexnac);
                monodHexNAc = true;
            }
            if (PenttoggleSwitch.IsOn == true)
            {
                numbers.Add(pent);
                monoPent = true;
            }
            if (KDNtoggleSwitch.IsOn == true)
            {
                numbers.Add(kdn);
                monoKDN = true;
            }
            if (Neu5ActoggleSwitch.IsOn == true)
            {
                numbers.Add(neuac);
                monoNeu5Ac = true;
            }
            if (Neu5GctoggleSwitch.IsOn == true)
            {
                numbers.Add(neugc);
                monoNeu5Gc = true;
            }
            if (PhostoggleSwitch.IsOn == true)
            {
                numbers.Add(phos);
                monoPhos = true;
            }
            if (SulftoggleSwitch.IsOn == true)
            {
                numbers.Add(sulf);
                monoSulf = true;
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
            List<string> targetStrings = new(
                targetString.Split(new string[] { "\n" },
                StringSplitOptions.RemoveEmptyEntries));
            List<decimal> targets = targetStrings.ConvertAll(decimal.Parse);
            ProgressBarGlyCombo.Maximum = targets.Count;


            // For enabling off-by-one errors. Thermo is pretty good at correcting the selected ion m/z when it picks an isotopic distribution, but might be useful for others
            if (OffByOne.IsChecked == true)
            {
                // For each target in the list, remove one hydrogen to account for the C13 isotope being picked instead of monoisotopic (negative mode only)
                targetsToAdd = targets.Count;
                for (int o = 0; o < targetsToAdd; o++)
                {
                    targets.Add(targets[o] - (decimal)1.00727);
                }
            }

            // Early processing of target list, breaking it down so that the reducing ends are removed
            if (Native.IsChecked == true)
            {
                //native
                if (Free.IsChecked == true)
                {
                    reduced = false;
                    targets = targets.Select(z => z - 18.010555m).ToList();
                }
                else
                {
                    reduced = true;
                    targets = targets.Select(z => z - 20.026195m).ToList();
                }
            }
            else
            {
                // permethylated
                if (Free.IsChecked == true)
                {
                    reduced = false;
                    targets = targets.Select(z => z - 46.041855m).ToList();
                }
                else
                {
                    reduced = true;
                    targets = targets.Select(z => z - 62.073145m).ToList();
                }
            }
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
                if (Da.IsChecked == true)
                {
                    errorType = "Da";
                    errorTol = Convert.ToDecimal(DaError.Text);
                    targetLow = targets[i] - errorTol;
                    targetHigh = targets[i] + errorTol;
                }
                else
                {
                    errorType = "ppm";
                    errorTol = Convert.ToDecimal(ppmError.Text);
                    targetLow = targets[i] - (targets[i] * (errorTol / 1000000));
                    targetHigh = targets[i] + (targets[i] * (errorTol / 1000000));
                }
                decimal target = targets[i];
                ProgressBarGlyCombo.Value = i;
                Sum_up_recursive(numbers, target, [], targetFound, i);
            };

            // Write processed data to csv file
            string solutionHeader = "";
            string skylineSolutionHeader = "";
            string skylineSolutionMultiplesPreTrim = "";
            string skylineSolutionMultiples = "";
            if (TextRadioButton.IsChecked == false)
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
            if (monoHex == true)
            {
                param_monoHex = Environment.NewLine + "Hex (" + HexMin_int.ToString() + "-" + HexMax_int.ToString() + ")";
            }
            if (monoHexA == true)
            {
                param_monoHexA = Environment.NewLine + "HexA (" + HexAMin_int.ToString() + "-" + HexAMax_int.ToString() + ")";
            }
            if (monodHex == true)
            {
                param_monodHex = Environment.NewLine + "dHex (" + dHexMin_int.ToString() + "-" + dHexMax_int.ToString() + ")";
            }
            if (monoHexNAc == true)
            {
                param_monoHexNAc = Environment.NewLine + "HexNAc (" + HexNAcMin_int.ToString() + "-" + HexNAcMax_int.ToString() + ")";
            }
            if (monoHexN == true)
            {
                param_monoHexN = Environment.NewLine + "HexN (" + HexNMin_int.ToString() + "-" + HexNMax_int.ToString() + ")";
            }
            if (monodHexNAc == true)
            {
                param_monodHexNAc = Environment.NewLine + "dHexNAc (" + dHexNAcMin_int.ToString() + "-" + dHexNAcMax_int.ToString() + ")";
            }
            if (monoPent == true)
            {
                param_monoPent = Environment.NewLine + "Pent (" + PentMin_int.ToString() + "-" + PentMax_int.ToString() + ")";
            }
            if (monoKDN == true)
            {
                param_monoKDN = Environment.NewLine + "KDN (" + KDNMin_int.ToString() + "-" + KDNMax_int.ToString() + ")";
            }
            if (monoNeu5Ac == true)
            {
                param_monoNeu5Ac = Environment.NewLine + "Neu5Ac (" + Neu5AcMin_int.ToString() + "-" + Neu5AcMax_int.ToString() + ")";
            }
            if (monoNeu5Gc == true)
            {
                param_monoNeu5Gc = Environment.NewLine + "Neu5Gc (" + Neu5GcMin_int.ToString() + "-" + Neu5GcMax_int.ToString() + ")";
            }
            if (monoPhos == true)
            {
                param_monoPhos = Environment.NewLine + "Phos (" + PhosMin_int.ToString() + "-" + PhosMax_int.ToString() + ")";
            }
            if (monoSulf == true)
            {
                param_monoSulf = Environment.NewLine + "Sulf (" + SulfMin_int.ToString() + "-" + SulfMax_int.ToString() + ")";
            }
            // Converting precursor list to series of strings for subsequent confirmation
            string combinedTargets = string.Join(Environment.NewLine, targets.ToArray());
            File.WriteAllText(string.Concat(saveFileDialog1.FileName.AsSpan(0, saveFileDialog1.FileName.Length - 4), "_parameters.txt"),
                "GlyCombo Search Parameters"
                + Environment.NewLine
                + inputParameters
                + Environment.NewLine
                + "Error tolerance: "
                + errorTol + " " + errorType
                + Environment.NewLine
                + "Derivatisation: "
                + derivatisation.ToString()
                + Environment.NewLine
                + "Reduced: "
                + reduced.ToString()
                + Environment.NewLine
                + Environment.NewLine
                + "Monosaccharide ranges"
                + Environment.NewLine
                + param_monoHex
                + param_monoHexA
                + param_monodHex
                + param_monoHexNAc
                + param_monoHexN
                + param_monodHexNAc
                + param_monoPent
                + param_monoKDN
                + param_monoNeu5Ac
                + param_monoNeu5Gc
                + param_monoPhos
                + param_monoSulf
                + Environment.NewLine
                + Environment.NewLine
                + "Precursor targets"
                + Environment.NewLine
                + neutralPrecursorListmzml);
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
                if (Native.IsChecked == true)
                {
                    // Native
                    solutions = solutions.Replace("146.057908", "dHex ").Replace("162.052823", "Hex ").Replace("291.095416", "Neu5Ac ").Replace("307.090331", "Neu5Gc ").Replace("203.079372", "HexNAc ").Replace("79.966331", "Phos ").Replace("79.956815", "Sulf ").Replace(",", "").Replace("161.068808", "HexN ").Replace("176.032088", "HexA ").Replace("187.084458", "dHexNAc ").Replace("132.042258", "Pent ").Replace("250.068867", "KDN ");
                }
                else
                {
                    // Permethylated
                    kdn = 320.147120m; // permethylated mass =  chemical formula = C14H24O8
                    solutions = solutions.Replace("174.089210", "dHex ").Replace("204.099775", "Hex ").Replace("361.173669", "Neu5Ac ").Replace("391.184234", "Neu5Gc ").Replace("245.126324", "HexNAc ").Replace("93.981983", "Phos ").Replace("79.956815", "Sulf ").Replace(",", "").Replace("217.131409", "HexN ").Replace("218.079040", "HexA ").Replace("215.115759", "dHexNAc ").Replace("160.073560", "Pent ").Replace("320.147120", "KDN ");
                }

                // This replaces repeated monosaccharide names with 1 monosaccharide name and the number of the occurences
                string solutionsUpdate = "";
                int chemicalFormulaeC = 0;
                int chemicalFormulaeH = 0;
                int chemicalFormulaeO = 0;
                int chemicalFormulaeN = 0;
                int chemicalFormulaeP = 0;
                int chemicalFormulaeS = 0;
                int dHexCount;
                int HexACount;
                int HexNCount;
                int PentCount;
                int KDNCount;
                int hexCount;
                int neuAcCount;
                int neuGcCount;
                int hexNAcCount;
                int phosCount;
                int dhexnacCount;

                // Native processing
                if (Native.IsChecked == true)
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
                    neuAcCount = Regex.Matches(solutions, "Neu5Ac").Count;
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
                    if (reduced == false)
                    {
                        chemicalFormulaeH += 2;
                        chemicalFormulaeO += 1;
                    }
                    else
                    {
                        chemicalFormulaeH += 4;
                        chemicalFormulaeO += 1;
                    }
                }
                else
                // Permethylated processing
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
                        chemicalFormulaeC += (HexNCount * 10);
                        chemicalFormulaeH += (HexNCount * 19);
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
                    if (reduced == false)
                    {
                        chemicalFormulaeC += 2;
                        chemicalFormulaeH += 6;
                        chemicalFormulaeO += 1;
                    }
                    else
                    {
                        chemicalFormulaeC += 3;
                        chemicalFormulaeH += 10;
                        chemicalFormulaeO += 1;
                    }
                }

                // Sulfate is the same chemical formulae independent of derivatization status
                int sulfCount = Regex.Matches(solutions, "Sulf ").Count;
                if (sulfCount > 0)
                {
                    chemicalFormulaeO += (sulfCount * 3);
                    chemicalFormulaeS += (sulfCount);
                    solutionsUpdate = solutionsUpdate + "(Sulf)" + Convert.ToString(sulfCount) + " ";
                }

                // Preparation to export a chemical formulae in a format compatible with Skyline
                string chemicalFormula = "C" + chemicalFormulaeC + "H" + chemicalFormulaeH + "N" + chemicalFormulaeN + "O" + chemicalFormulaeO + "P" + chemicalFormulaeP + "S" + chemicalFormulaeS;
                chemicalFormula = chemicalFormula.Replace("N0", "").Replace("P0", "").Replace("S0", "");

                // Reducing end status, native or permethylated
                if (Native.IsChecked == true)
                {
                    // Native
                    if (reduced == false)
                    {
                        observedMass = s + 18.010565m;
                        theoreticalMass = target + 18.010565m;
                    }
                    else
                    {
                        observedMass = s + 20.026215m;
                        theoreticalMass = target + 20.026215m;
                    }
                }
                else
                {
                    // Permethylated
                    if (reduced == false)
                    {
                        observedMass = s + 18.010565m + 28.031300m;
                        theoreticalMass = target + 18.010565m + 28.031300m;
                    }
                    else
                    {
                        observedMass = s + 20.026195m + 42.046950m;
                        theoreticalMass = target + 20.026195m + 42.046950m;
                    }
                }

                // Calculation for error
                error = observedMass - theoreticalMass;

                // Calculation of scan number and charge state to be represented later
                targetIndex.Add(i);
                if (TextRadioButton.IsChecked == false)
                {
                    string scanNumberForOutput = "";
                    string chargeForOutput = "";
                    string retentionTimeForOutput = "";
                    string TICForOutput = "";
                    string FileForOutput = "";

                    // mzml input therefore output needs to be include scan #, charge, RT and TIC values.
                    // OffByOne error essentially doubles the target list, need to ensure that we can assign metadata to the +1 targets (otherwise it tries to call metadata from a limited list)
                    if (OffByOne.IsChecked == true)
                    {
                        if (i < targetsToAdd)
                        {
                            scanNumberForOutput = Convert.ToString(scans.ElementAt(i));
                            chargeForOutput = Convert.ToString(charges.ElementAt(i));
                            retentionTimeForOutput = Convert.ToString(retentionTimes.ElementAt(i));
                            TICForOutput = Convert.ToString(TICs.ElementAt(i));
                            FileForOutput = Convert.ToString(files.ElementAt(i));
                        }
                        else
                        {
                            scanNumberForOutput = Convert.ToString(scans.ElementAt(i-targetsToAdd));
                            chargeForOutput = Convert.ToString(charges.ElementAt(i-targetsToAdd));
                            retentionTimeForOutput = Convert.ToString(retentionTimes.ElementAt(i-targetsToAdd));
                            TICForOutput = Convert.ToString(TICs.ElementAt(i-targetsToAdd));
                            FileForOutput = Convert.ToString(files.ElementAt(i - targetsToAdd));
                        }
                    }
                    else
                    {
                        scanNumberForOutput = Convert.ToString(scans.ElementAt(i));
                        chargeForOutput = Convert.ToString(charges.ElementAt(i));
                        retentionTimeForOutput = Convert.ToString(retentionTimes.ElementAt(i));
                        TICForOutput = Convert.ToString(TICs.ElementAt(i));
                        FileForOutput = Convert.ToString(files.ElementAt(i));
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
            for (int k = 0; k < numbers.Count; k++)
            {
                List<decimal> remaining = [];
                decimal n = numbers[k];
                for (int j = k; j < numbers.Count; j++) remaining.Add(numbers[j]);
                List<decimal> partial_rec = new(partial)
                {
                    n
                };
                Sum_up_recursive(remaining, target, partial_rec, targetFound, i);
            }
        }

        private void Advanced_Button(object sender, RoutedEventArgs e)
        {
            if (advanced.Visibility == Visibility.Collapsed)
            {
                advanced.Visibility = Visibility.Visible;
                advanced_text.Text = "Hide Advanced Monosaccharides";
            }
            else
            {
                advanced.Visibility = Visibility.Collapsed;
                advanced_text.Text = "Show Advanced Monosaccharides";
            }
        }

        private void ToggleSwitch1_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container1.Visibility = Visibility.Visible;
                }
                else
                {
                    container1.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch2_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container2.Visibility = Visibility.Visible;
                }
                else
                {
                    container2.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch3_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container3.Visibility = Visibility.Visible;
                }
                else
                {
                    container3.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch4_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container4.Visibility = Visibility.Visible;
                }
                else
                {
                    container4.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch5_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container5.Visibility = Visibility.Visible;
                }
                else
                {
                    container5.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch6_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container6.Visibility = Visibility.Visible;
                }
                else
                {
                    container6.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch7_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container7.Visibility = Visibility.Visible;
                }
                else
                {
                    container7.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch8_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container8.Visibility = Visibility.Visible;
                }
                else
                {
                    container8.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch9_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container9.Visibility = Visibility.Visible;
                }
                else
                {
                    container9.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch10_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container10.Visibility = Visibility.Visible;
                }
                else
                {
                    container10.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch11_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container11.Visibility = Visibility.Visible;
                }
                else
                {
                    container11.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ToggleSwitch12_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch? toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    container12.Visibility = Visibility.Visible;
                }
                else
                {
                    container12.Visibility = Visibility.Collapsed;
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
            ProgressBarGlyCombo.Value = 0;
        }

        private void MzmlRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            submitbutton.IsEnabled = false;
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
                    advanced.Visibility = Visibility.Collapsed;
                    advanced_text.Text = "Show Advanced Monosaccharides";
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
                    advanced.Visibility = Visibility.Collapsed;
                    advanced_text.Text = "Show Advanced Monosaccharides";
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
                    advanced.Visibility = Visibility.Visible;
                    advanced_text.Text = "Hide Advanced Monosaccharides";
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
                    advanced.Visibility = Visibility.Collapsed;
                    advanced_text.Text = "Show Advanced Monosaccharides";
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
                    advanced.Visibility = Visibility.Visible;
                    advanced_text.Text = "Hide Advanced Monosaccharides";
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
                    advanced.Visibility = Visibility.Visible;
                    advanced_text.Text = "Hide Advanced Monosaccharides";
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
                    advanced.Visibility = Visibility.Visible;
                    advanced_text.Text = "Hide Advanced Monosaccharides";
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
                    advanced.Visibility = Visibility.Visible;
                    advanced_text.Text = "Hide Advanced Monosaccharides";
                    break;

                default:
                    //what you want when nothing is selected
                    break;
            }
        }
    }
}