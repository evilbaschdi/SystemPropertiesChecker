# ![icon](./.repo/icon.png) SystemPropertiesChecker

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)
![Commit Activity](https://img.shields.io/github/commit-activity/m/evilbaschdi/SystemPropertiesChecker?style=for-the-badge&)
![Last Commit](https://img.shields.io/github/last-commit/evilbaschdi/SystemPropertiesChecker?style=for-the-badge&)
[![CodeFactor](https://www.codefactor.io/repository/github/evilbaschdi/SystemPropertiesChecker/badge/main?style=for-the-badge)](https://www.codefactor.io/repository/github/evilbaschdi/SystemPropertiesChecker/overview/main)


## About
some little tool I wrote to get quick information about installed service packs on windows. Since Windows 7 SP1 was the last one of those service packs, I decided to extend the functionality to display installed .Net frameworks and other software.

## `Basic` View
Gives information regarding device name, current IP, and the windows information used
![Screenshot_Basic](./.repo/basic.png)

## `History` View
List of former installed versions / builds of windows
![Screenshot_History](./.repo/history.png)

## `.net` View
Reads global.json to list installed .net (core) runtimes and SDKs
![Screenshot_Core](./.repo/core.png)

## `.net framework` View
Reads the windows registry to list installed / activated .net framework versions
![Screenshot_Framework](./.repo/framework.png)

## `other` View
Lists installed browsers and some other tools, such as Git for Windows or PowerShell
![Screenshot_Other](./.repo/other.png)
