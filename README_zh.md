# SingerUtils
[English](README.md) | **简体中文**

SingerUtils是一款开源的UTAU插件，提供了与UTAU音源管理相关的一些实用功能。该插件主要面向OpenUTAU用户。

SingerUtils使用和[OpenUTAU](https://github.com/stakira/OpenUtau)相同的底层技术开发：.net Core和Avalonia。

## 下载
[从GitHub下载](https://github.com/oxygen-dioxide/SingerUtils/releases)

## 功能
### 音源清理
许多UTAU引擎会在你的音源中保存中间文件（例如.frq和.llsm）。这些中间文件会占用硬盘空间，而且你可能不会再次用到它们。使用“音源清理”可以批量删除这些文件。

可以指定需要删除的文件格式。

### 音源打包
使用“音源打包”，可以将你的音源打包为一个UTF-8编码的压缩包，以分发给其他用户。压缩包在不同系统语言下打开均不会乱码。

与“音源清理”类似，你可能不希望将中间文件添加到压缩包中。可以指定打包中排除的文件格式。

### 音源合并（开发中）
OpenUTAU原生支持多音色音源。使用“音源合并”，可以将另一个音源作为一个音色并入到当前音源中。