# ![icon](./.repo/icon.png) SystemPropertiesChecker

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)

## About
some little tool I wrote to get quick information about installed service packs on windows. Since Windows 7 SP1 was the last one of those service packs, I decided to extend the functionality to display installed .Net frameworks and other software.

### `Basic` View
Gives information regarding device name, current IP, and the windows information used
![Screenshot_Basic](./.repo/basic.png)

### `History` View
List of former installed versions / builds of windows
![Screenshot_History](./.repo/history.png)

### `.net` View
Reads global.json to list installed .net (core) runtimes and SDKs
![Screenshot_Core](./.repo/core.png)

### `.net framework` View
Reads the windows registry to list installed / activated .net framework versions
![Screenshot_Framework](./.repo/framework.png)

### `other` View
Lists installed browsers and some other tools, such as Git for Windows or PowerShell
![Screenshot_Other](./.repo/other.png)

## Package Feeds

Default by `NuGet.config` is myget.org

| Feed                           | Feed Url                                                         |
| :----------------------------- | :--------------------------------------------------------------- |
| ![myget.org][myGetBadge]       | <https://www.myget.org/F/evilbaschdi/api/v3/index.json>          |
| ![codeberg.org][codebergBadge] | <https://codeberg.org/api/packages/evilbaschdi/nuget/index.json> |

## Quality & Activity

| Branch                                | Status & Activity                                                                                                                                                        |
| :------------------------------------ | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ![Main Branch][mainBranchBadge]       | [![CodeFactor][codeFactorMainBadge]][codeFactorMainOverview] ![Commit Activity Main][commitActivityMainBadge] ![Last Commit Main][lastCommitMainBadge]                   |
| ![Develop Branch][developBranchBadge] | [![CodeFactor][codeFactorDevelopBadge]][codeFactorDevelopOverview] ![Commit Activity Develop][commitActivityDevelopBadge] ![Last Commit Develop][lastCommitDevelopBadge] |

[myGetBadge]: https://img.shields.io/badge/MyGet.org-gray?style=for-the-badge&logo=myget
[codebergBadge]: https://img.shields.io/badge/Codeberg-gray?style=for-the-badge&logo=codeberg

[mainBranchBadge]: https://img.shields.io/badge/branch-main-brightgreen?style=for-the-badge&logo=git&logoColor=white&color=c9ff00
[developBranchBadge]: https://img.shields.io/badge/branch-develop-blue?style=for-the-badge&logo=git&logoColor=white&color=0080ff

[codeFactorMainBadge]: https://www.codefactor.io/repository/github/evilbaschdi/SystemPropertiesChecker/badge/main?style=for-the-badge
[codeFactorMainOverview]: https://www.codefactor.io/repository/github/evilbaschdi/SystemPropertiesChecker/overview/main
[commitActivityMainBadge]: https://img.shields.io/github/commit-activity/m/evilbaschdi/SystemPropertiesChecker/main?style=for-the-badge
[lastCommitMainBadge]: https://img.shields.io/github/last-commit/evilbaschdi/SystemPropertiesChecker/main?style=for-the-badge

[codeFactorDevelopBadge]: https://www.codefactor.io/repository/github/evilbaschdi/SystemPropertiesChecker/badge/develop?style=for-the-badge
[codeFactorDevelopOverview]: https://www.codefactor.io/repository/github/evilbaschdi/SystemPropertiesChecker/overview/develop
[commitActivityDevelopBadge]: https://img.shields.io/github/commit-activity/m/evilbaschdi/SystemPropertiesChecker/develop?style=for-the-badge
[lastCommitDevelopBadge]: https://img.shields.io/github/last-commit/evilbaschdi/SystemPropertiesChecker/develop?style=for-the-badge