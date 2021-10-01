
$token = Get-Token

$uri = "https://notifications.citrixworkspacesapi.net/$($env:Citrix_Customer_Id)/notifications/items"

$body = @{
    destinationAdmin = "*"
    component = "Citrix Cloud"
    createdDate = (Get-Date).ToUniversalTime()
    eventId = (New-Guid).Guid
    severity = "Information"
    priority = "Normal"
    content = @(
        @{
            languageTag = "en-US"
            title = "Citrix Converge 2021 Notification"
            description = "This notification is send using PowerShell"
        }
    )
}

$header = @{
    Authorization = "CwsAuth Bearer=$($token.access_token)"
}

try {
    $reponse = Invoke-WebRequest -Uri $uri -Method Post -Headers $header -Body ($body | ConvertTo-Json) -ContentType "application/json" -UseBasicParsing
} catch {
    Write-Host "Error: $($_.Exception)"
}

Write-Host "Status code: $($reponse.StatusCode)"

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