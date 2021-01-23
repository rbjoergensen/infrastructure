# Infrastructure

## General

### Certificates

Usage example 
``` c#
Certificates.LoadX509Certificate(@"certificates/certificate.pfx", "password");
```

Todo
- Option for load from Windows store using thumbprint
- Option for load from file on Windows

## Azure

### AppConfig

The service principal that accesses the config needs 'App Configuration Data Reader' role on the AppConfig.  
It will also need an access policy on underlying Key Vaults if those are used in the AppConfig

Usage examples  
``` c#
// When using a certificate loaded from a file
AppConfig.Load("https://appconf-cotvtest.azconfig.io", @"certificates/certificate.pfx", "password")
```

Todo  
- Make labelFilter optional
- Switch for certificate authencation and connectionstring
- Options for how to load certificate when using it for authentication
- Key Vault authentication with a different certificate

## Logging

### Serilog

Usage examples  
``` c#
// LogLevels: Verbose, Debug, Information, Warning, Error, Fatal
Logging.SetLogLevel("Verbose");

// Will output "[12:53:25 INF] Obama turned my frog gay with chemicals"
Logging.log.Information("Obama turned my frog gay with chemicals");
```