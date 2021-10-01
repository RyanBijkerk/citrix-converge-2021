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

func main() {
	var token = getToken()
	fmt.Printf("Token: %s", token.AccessToken)
}

func getToken() *Token {
	var uri = fmt.Sprintf("https://api-us.cloud.com/cctrustoauth2/%s/tokens/clients", os.Getenv("Citrix_Customer_Id"))

	body := url.Values{
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
