#!/usr/bin/env python

# sample client app with authentication
import requests
from requests.auth import HTTPBasicAuth

URL = "http://localhost/public"
  
PARAMS = {'param1':'some value for param1'} 

if __name__ == "__main__": 
    print "Public method:"
    
    r = requests.get(url = URL, params = PARAMS) 

    print r  
    print r.json()

    print "Login using POST request:"

    URL = "http://localhost/login"

    r = requests.post(url = URL, auth=HTTPBasicAuth('admin', 'Admin123'))

    print r
    print r.json()

    print "Wrong username/password:"

    URL = "http://localhost/private"
    r = requests.get(url = URL, params = PARAMS, auth=HTTPBasicAuth('user', 'pass')) 

    print r  
    print r.json()

    print "Now it must be OK:"

    r = requests.get(url = URL, params = PARAMS, auth=HTTPBasicAuth('admin', 'Admin123')) 

    print r  
    print r.json()
