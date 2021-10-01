
$token = Get-Token

$uri = "https://registry.citrixworkspacesapi.net/$($env:Citrix_Customer_Id)/resourcelocations "

$header = @{
    Authorization = "CwsAuth Bearer=$($token.access_token)"
}

try {
    $locations =  Invoke-RestMethod -Uri $uri -Method GET -Headers $header
} catch {
    Write-Host "Error: $($_.Exception)"
}

foreach ($location in $locations.items) {
    Write-Host $location.Name
}

function Get-Token() {
    $uri = "https://api-us.cloud.com/cctrustoauth2/$($env:Citrix_Customer_Id)/tokens/clients"

    $body = @{
        grant_type = "client_credentials"
        client_id = $env:Citrix_Client_Id
        client_secret = $env:Citrix_Client_Secret
    }

    try {
        $request = Invoke-RestMethod -Uri $uri -Method POST -Body $body 
    } catch {
        Write-Host "Error: $($_.Exception)"
    }

    return $request
}