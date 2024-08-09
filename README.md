# GlyCombo
![GitHub License](https://img.shields.io/github/license/Protea-Glycosciences/GlyCombo)
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/Protea-Glycosciences/GlyCombo/total)
![GitHub Release](https://img.shields.io/github/v/release/Protea-Glycosciences/GlyCombo)

[![Static Badge](https://img.shields.io/badge/Download_GlyCombo_Installer-Automatic_Updating-058743)](https://github.com/Protea-Glycosciences/GlyCombo/releases/latest/download/GlyCombo_setup.exe)

If your IT security rules prevent the installation of GlyCombo with the above link, please download the portable executable below. This installation does not check for automatic updates so is not recommended.

[![Static Badge](https://img.shields.io/badge/Download_GlyCombo_Installer-Portable-red)](https://github.com/Protea-Glycosciences/GlyCombo/releases/latest/download/GlyCombo_Portable.exe)


GlyCombo is an open source software for combinatorial glycan composition determination to identify glycans in MS acquisitions of glycan-containing samples in text or mzML formats.
This application enables rapid extraction of precursor *m/z* values from mzML files, a vendor-neutral format that ensures cross-platform compatibility.


<img src="/abstract.png" width="400">

Features
--------
- Open-source Windows application created in C#.
- Unlimited monosaccharide and modification options including Hex, HexNAc, dHex, Neu5Ac, Neu5Gc, HexN, HexA, dHexNAc, Pent, KDN, Phosphate, Sulfate, and Custom monosaccharides.
- Support for native, peracetylated, and permethylated glycan samples.
- Support for all commonly observed adducts by MS, including custom adducts and the ability to search for multiple adducts in the same search.
- Support for free, reduced, and tagged glycans.
- Automatic polarity extraction for mzML input files.

Input and Output Options
--------
| Output Options					 | mzML File Input    | Text List of Masses  |
|------------------------------------|:------------------:|:-------------------: |
| CSV file of glycan combinations    | :heavy_check_mark: | :heavy_check_mark: 	 | 
| Parameters File                    | :heavy_check_mark: |	:heavy_check_mark:   |
| Skyline Import File			     | :heavy_check_mark: |				         |


Citation
--------
Please cite:

*Kelly MI, Ashwood C. GlyCombo enables rapid, complete glycan composition identification across diverse glycomic sample types. ChemRxiv. 2024; doi:10.26434/chemrxiv-2024-3j008  This content is a preprint and has not been peer-reviewed.*

Licence
-------
GlyCombo is released under the [Apache 2.0 License](LICENSE).
