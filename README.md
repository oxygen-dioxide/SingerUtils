# SingerUtils
**English** | [简体中文](README_zh.md)

SingerUtils is an open-source UTAU plugin that provides a set of utilities for voicebank management. This plugin is mainly oriented to OpenUTAU users.

SingerUtils is developed with the same technologies used by [OpenUTAU](https://github.com/stakira/OpenUtau): .net Core and Avalonia.

## Download
[Download from GitHub](https://github.com/oxygen-dioxide/SingerUtils/releases)

## Features
### Voicebank Cleanup
Many UTAU engines write intermediate files into your voicebanks, such as .frq and .llsm files. These files will occupy your disk space, and you probably won't use them again. Using "Voicebank Cleanup", you can easily delete these files.

You can choose which file formats to delete.

### Voicebank Pack
using "Voicebank Pack", you can pack your voicebank into an archive encoded in utf-8. This archive won't garble in various system locales.

Like "Voicebank Cleanup", you may want to exclude intermediate files from your archive. You can choose which file formats to exclude.

### Voicebank Merge (work in progress)
OpenUTAU natively supports multi-color voicebanks. Using "Voicebank Merge", you can merge another voicebank into your currently using voicebank as a voice color.