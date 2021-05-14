@POWERSHELL "&{Try { ./build.ps1 -target Test; exit $lastExitCode; } Catch { exit -1; }}" 

