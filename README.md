
<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![License: MIT][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/mvelasquez10/ck-backend">
    <img src="logo.png" alt="Logo" width="80" height="80">
  </a>
  <h3 align="center">Collective Knowledge - Backend</h3>
  <p align="center">
    This is the repository for the backend of my profile's project, for more information please visit 
    <a href="https://mvelasquez.net"><strong>Miguel Velasquez</strong></a>
  </p>
</p>

<!-- TABLE OF CONTENTS -->
## Table of Contents

* [About the Project](#about-the-project)
  * [Built With](#built-with)
* [Getting Started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [Installation](#installation)
* [Usage](#usage)
* [License](#license)
* [Contact](#contact)


<!-- ABOUT THE PROJECT -->
## About The Project
This is the backend project for the Collective Knowledge which is a profile’s demonstration application. The focus of this project is to show the implementation of this side of the project to anyone who want to dig deeper into the features implemented.
### Built With

* [Asp.net Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0)

<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites
You need to install dotnet 3.1 SDK to build the solution
* [.Net 3.1 Core](https://dotnet.microsoft.com/download/dotnet-core/3.1)

### Installation

1. Clone the repo
```sh
git clone https://github.com/mvelasquez10/ck-backend.git
```
2. Restore the packages
```sh
dotnet restore
```
3. Build the solution
```sh
dotnet build
```

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<!-- USAGE -->
## Usage
Build the complete solution

Start the following services
* CK.Rest.Languages
* CK.Rest.Users
* CK.Rest.Posts

Alternative you can also start after the other are up and running
* CK.Rest.Proxy

<!-- CONTACT -->
## Contact

Author: [Miguel Velasquez][linkedin-url]

Project Link: [Collective Knowledge - Backend](https://github.com/mvelasquez10/ck-backend)

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[license-shield]: https://img.shields.io/badge/License-MIT-green.svg?style=flat-square
[license-url]: https://github.com/mvelasquez10/ck-backend/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=flat-square&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/mvelasquez10
