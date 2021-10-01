package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"net/http"
	"net/url"
	"os"
)

type Token struct {
	Type        string `json:"token_type"`
	AccessToken string `json:"access_token"`
	Expires     string `json:"expires_in"`
}

type Items struct {
	Items []Location `json:"items"`
}

type Location struct {
	Id           string `json:"id"`
	Name         string `json:"name"`
	InternalOnly bool   `json:"internalOnly"`
	TimeZone     string `json:"timeZone"`
	ReadOnly     bool   `json:"readOnly"`
}

func main() {
	var token = getToken()

	var uri = fmt.Sprintf("https://registry.citrixworkspacesapi.net/%s/resourcelocations", os.Getenv("Citrix_Customer_Id"))

	var req, err = http.NewRequest(http.MethodGet, uri, nil)

	req.Header.Add("Authorization", fmt.Sprintf("CwsAuth Bearer=%s", token.AccessToken))

	var client = &http.Client{}
	resp, err := client.Do(req)

	if err != nil {
		fmt.Println(err)
	}

	var locations = new(Items)
	bytes, err := ioutil.ReadAll(resp.Body)

	if err := json.Unmarshal(bytes, &locations); err != nil {
		fmt.Println(err)
	}

	defer resp.Body.Close()

	for _, location := range locations.Items {
		fmt.Println(location.Name)
	}
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
