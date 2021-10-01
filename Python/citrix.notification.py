# Please note, requests needs to be installed using: pip install requests
from datetime import datetime
import requests
import uuid
import os
import json

from requests.api import head

def getToken():
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
    return token

token = getToken()

uri = f"https://notifications.citrixworkspacesapi.net/{os.environ['Citrix_Customer_Id']}/notifications/items"

body = {
    "destinationAdmin": "*",
    "component" : "Citrix Cloud",
    "createdDate" : datetime.utcnow().strftime('%Y-%m-%dT%H:%M:%SZ'),
    "eventId" : str(uuid.uuid4()),
    "severity" :"Information",
    "priority": "Normal",
    "content": [
        {
            "languageTag": "en-US",
            "title": "Citrix Converge 2021 Notification",
            "description" : "This notification is send using Python"
        }
    ]
}

header = {
    "Content-Type": "application/json",
    "Authorization": f"CwsAuth Bearer={token['access_token']}"
}

try:
    request = requests.post(uri, data=json.dumps(body), headers=header)
except requests.exceptions as e:
    print(f"Error: {e.RequestException}")

print(f"Status code: {request.status_code}")