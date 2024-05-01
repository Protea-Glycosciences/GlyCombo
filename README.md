# GlyCombo
![GitHub License](https://img.shields.io/github/license/Protea-Glycosciences/GlyCombo)

GlyCombo is an open source software for combinatorial glycan composition determination to identify glycans in MS acquisitions of glycan-containing samples in text or mzML formats.
This application enables rapid extraction of precursor *m/z* values from mzML files, a vendor-neutral format that ensures cross-platform compatibility.


<img src="/abstract.png" width="400">

Features
--------
- Open-source Windows application created in C#.
- Extensive monosaccharide and modification options including Hex, HexNAc, dHex, Neu5Ac, Neu5Gc, HexN, HexA, dHexNAc, Pent, KDN, Phosphate and Sulfate.
- Support for native and permethylated glycan samples.
- Support for free and reduced glycans.
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

*TBA*

Licence
-------
GlyCombo is released under the [Apache 2.0 License](LICENSE).