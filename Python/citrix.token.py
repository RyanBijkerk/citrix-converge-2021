# Please note, requests needs to be installed using: pip install requests
import requests
import os

uri = f"https://api-us.cloud.com/cctrustoauth2/{os.environ['Citrix_Customer_Id']}/tokens/clients"

body = {
    "grant_type" : "client_credentials",
    "client_id" : os.environ['Citrix_Client_Id'],
    "client_secret" : os.environ['Citrix_Client_Secret']
}

try:
    request = requests.post(uri, data=body)
except requests.exceptions as e:
    print(f"Error: {e.RequestException}")

token = request.json()
print(f"Token: {token['access_token']}")