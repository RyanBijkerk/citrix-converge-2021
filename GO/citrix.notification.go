package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"net/http"
	"net/url"
	"os"
	"time"

	// go env -w GO111MODULE=off
	// go get github.com/google/uuid
	"github.com/google/uuid"
)

type Token struct {
	Type        string `json:"token_type"`
	AccessToken string `json:"access_token"`
	Expires     string `json:"expires_in"`
}

type Notification struct {
	DestinationAdmin string
	Component        string
	CreatedDate      time.Time
	EventId          string
	Severity         string
	Priority         string
	Content          []NotifcationContent
}

type NotifcationContent struct {
	LanguageTag string
	Title       string
	Description string
}

func main() {
	var token = getToken()

	var uri = fmt.Sprintf("https://notifications.citrixworkspacesapi.net/%s/notifications/items", os.Getenv("Citrix_Customer_Id"))

	var body = Notification{
		DestinationAdmin: "*",
		Component:        "Citrix Cloud",
		CreatedDate:      time.Now().UTC(),
		EventId:          fmt.Sprintln(uuid.New()),
		Severity:         "Information",
		Priority:         "Normal",
		Content: []NotifcationContent{
			{
				LanguageTag: "en-US",
				Title:       "Citrix Converge 2021 Notification",
				Description: "This notification is send using GO",
			},
		},
	}
	var jsonBody, jsonErr = json.Marshal(body)
	if jsonErr != nil {
		fmt.Println(jsonErr)
		return
	}

	var req, err = http.NewRequest(http.MethodPost, uri, bytes.NewBuffer(jsonBody))

	req.Header.Add("Authorization", fmt.Sprintf("CwsAuth Bearer=%s", token.AccessToken))
	req.Header.Add("Content-Type", "application/json")

	var client = &http.Client{}
	resp, err := client.Do(req)

	if err != nil {
		fmt.Println(err)
	}

	defer resp.Body.Close()

	fmt.Println("Status code: " + resp.Status)

}

func getToken() *Token {
	var uri = fmt.Sprintf("https://api-us.cloud.com/cctrustoauth2/%s/tokens/clients", os.Getenv("Citrix_Customer_Id"))

	var body = url.Values{
		"grant_type":    {"client_credentials"},
		"client_id":     {os.Getenv("Citrix_Client_Id")},
		"client_secret": {os.Getenv("Citrix_Client_Secret")},
	}

	resp, err := http.PostForm(uri, body)
	if err != nil {
		fmt.Println(err)
	}
	defer resp.Body.Close()

	var results = new(Token)
	bytes, err := ioutil.ReadAll(resp.Body)

	if err := json.Unmarshal(bytes, &results); err != nil {
		fmt.Println(err)
	}

	return results
}
