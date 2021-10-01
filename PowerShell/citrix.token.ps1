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

Write-Host "Token: $($request.access_token)"