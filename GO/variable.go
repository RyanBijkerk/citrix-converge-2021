package main

import (
	"fmt"
	"reflect"
	"strings"
)

func main() {
	var var1 = 5
	fmt.Println(var1)

	var1 += 1
	fmt.Println(var1)

	var var2 = "This is a string"
	fmt.Println(var2)

	fmt.Println(strings.ToUpper(var2))

	var var3 = true
	fmt.Println(reflect.TypeOf(var3))

	if var3 == true {
		fmt.Println(true)
	} else {
		fmt.Println(false)
	}
}
