# cert2src

[![Release](https://github.com/bytewizer/cert2src/actions/workflows/release.yml/badge.svg)](https://github.com/bytewizer/cert2src/actions/workflows/release.yml)

This is a simple command line utility for download and exporting the chain root certificates required for [TinyCLR OS](https://www.ghielectronics.com/) to access secure sites.  Certificate can be exported in Security Certificate File Format (crt) commonly embedded as a binary project resource or a csharp source code array easly added to a project as source code.

```
Usage: cert2src url [options]

Download and export root certificates required for TinyCLR OS to access secure sites.

options:
 --help        Displays general help information about other commands.
 --path        Output chain root certificate as base-64 encoded PEM format to file.
 --code        Set output format as csharp source code array.
 --width       Width of the source code array output (default 18).
```
Note: you can included the --path flag without a path and it will default to the executable path location. 

## Installation

Download the latest [release-win64.zip](https://github.com/bytewizer/cert2src/releases)  file and unzip to a local working directory.

## Examples

```
C:\project>cert2src.exe https://www.google.com --path

-----BEGIN CERTIFICATE-----
MIIDdTCCAl2gAwIBAgILBAAAAAABFUtaw5QwDQYJKoZIhvcNAQEFBQAwVzELMAkGA1UEBhMCQkUx
GTAXBgNVBAoTEEdsb2JhbFNpZ24gbnYtc2ExEDAOBgNVBAsTB1Jvb3QgQ0ExGzAZBgNVBAMTEkds

[Intentionally omitted]

hm4qxFYxldBniYUr+WymXUadDKqC5JlR3XC321Y9YeRq4VzW9v493kHMB65jUr9TU/Qr6cf9tveC
X4XSQRjbgbMEHMUfpIBvFSDJ3gyICh3WZlXi/EjJKSZp4A==
-----END CERTIFICATE-----

Root certificate successfully exported to 'C:\project\certificate.crt'
Root certificate downloaded from 'https://www.google.com/'
```

```
C:\project>cert2src.exe https://www.google.com --path -code -width 10

private static readonly byte[] Certificate =
{
     0x2d, 0x2d, 0x2d, 0x2d, 0x2d, 0x42, 0x45, 0x47, 0x49, 0x4e,
     0x20, 0x43, 0x45, 0x52, 0x54, 0x49, 0x46, 0x49, 0x43, 0x41,

     [Intentionally omitted]

     0x2d, 0x2d, 0x2d, 0x2d, 0x45, 0x4e, 0x44, 0x20, 0x43, 0x45,
     0x2d, 0x2d, 0x2d, 0x2d, 0x0d, 0x0a
};

Root certificate successfully exported to 'C:\project\certificate.cs'
Root certificate downloaded from 'https://www.google.com/'
```

## Contributions

Contributions to this project are always welcome. Please consider forking this project on GitHub and sending a pull request to get your improvements added to the original project.

## Disclaimer

All source, documentation, instructions and products of this project are provided as-is without warranty. No liability is accepted for any damages, data loss or costs incurred by its use.