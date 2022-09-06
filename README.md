# cert2src

[![Build](https://github.com/bytewizer/runtime/actions/workflows/actions.yml/badge.svg)](https://github.com/bytewizer/runtime/actions/workflows/actions.yml)

This is a simple utility for download and converting / export root certificates required for [GHI Electronics TinyCLR OS](https://www.ghielectronics.com/) to access secure websites.

```
Usage: cert2src url [options]

Download and export root certificates required for TinyCLR OS to access secure websites.

options:
 --help        Displays general help information about other commands.
 --path        Output chain root certificate as base-64 encoded PEM format to file.
 --code        Set output format as csharp source code array.
 --width       Width of the source code array output (default 18).
```
Note: if you do not provide a 'path' it will default to the current path.

## Examples
```
C:\project>cert2src https://www.google.com --path

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
C:\project>cert2src https://www.google.com --path -code -width 10

private static readonly byte[] Certificate =
{
     0x2d, 0x2d, 0x2d, 0x2d, 0x2d, 0x42, 0x45, 0x47, 0x49, 0x4e,
     0x20, 0x43, 0x45, 0x52, 0x54, 0x49, 0x46, 0x49, 0x43, 0x41,

     [Intentionally omitted]

     0x2d, 0x2d, 0x2d, 0x2d, 0x45, 0x4e, 0x44, 0x20, 0x43, 0x45,
     0x52, 0x54, 0x49, 0x46, 0x49, 0x43, 0x41, 0x54, 0x45, 0x2d,
     0x2d, 0x2d, 0x2d, 0x2d, 0x0d, 0x0a
};

Root certificate successfully exported to 'C:\project\certificate.cs'
Root certificate downloaded from 'https://www.google.com/'
```

## Requirements

Software: <a href="https://visualstudio.microsoft.com/downloads/">Visual Studio 2019/2022</a> and <a href="https://www.ghielectronics.com/">GHI Electronics TinyCLR OS</a> 

## Contributions

Contributions to this project are always welcome. Please consider forking this project on GitHub and sending a pull request to get your improvements added to the original project.

## Disclaimer

All source, documentation, instructions and products of this project are provided as-is without warranty. No liability is accepted for any damages, data loss or costs incurred by its use.