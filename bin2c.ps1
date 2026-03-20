param(
    [string]$DllPath  = "$PSScriptRoot\tos2\bin\Release\netstandard2.1\tos2.dll",
    [string]$OutPath  = "$PSScriptRoot\loader\payload.h"
)

if (-not (Test-Path $DllPath)) {
    Write-Error "DLL not found at path: $DllPath"
    exit 1
}

try {
    $bytes = [System.IO.File]::ReadAllBytes($DllPath)
    $count = $bytes.Length

    $chunkSize = 16
    $hexBytes = $bytes | ForEach-Object { "0x{0:X2}" -f $_ }
    $chunks = for ($i = 0; $i -lt $hexBytes.Count; $i += $chunkSize) {
        ($hexBytes[$i..([Math]::Min($i + $chunkSize - 1, $hexBytes.Count - 1))]) -join ", "
    }
    $hex = ($chunks -join ",`n`t`t")

    $header = @"
#pragma once

namespace mrk::payload {

    // Auto-generated from $DllPath
    inline constexpr unsigned int size = $count;
    inline constexpr unsigned char data[] = {
        $hex
    };

} // namespace mrk
"@

    [System.IO.File]::WriteAllText($OutPath, $header, [System.Text.Encoding]::UTF8)
    Write-Host "Generated $OutPath ($count bytes)"
} catch {
    Write-Error "An error occurred: $_"
    exit 1
}