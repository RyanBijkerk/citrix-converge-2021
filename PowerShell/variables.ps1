$var1 = 5
Write-Host $var1

$var1 += 1
Write-Host $var1

$var2 = "This is a string"
Write-Host $var2

Write-Host $var2.ToUpper()

$var3 = $true
Write-Host $var3.GetType()

if ($var3 -eq $true) {
    Write-Host $true
} else {
    Write-Host $false
}
