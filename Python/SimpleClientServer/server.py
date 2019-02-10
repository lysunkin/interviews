#!/usr/bin/env python
"""
Simple HTTP server in python.
Usage::
    ./server.py [<port>]
Send a GET request::
    curl http://localhost
Send a POST request::
    curl -d "foo=bar&bin=baz" http://localhost

basic authentication is supported (also other types of authentication can be implemented)

"""
from BaseHTTPServer import BaseHTTPRequestHandler, HTTPServer
import SocketServer
import base64

sample_username = 'admin'
sample_password = 'Admin123'
sample_token = '12345678900987654321'

def verify_credentials(data):
     try:
         base64_data = data.split(' ')[1]
         decoded = base64.decodestring(base64_data)
         print decoded
         return decoded == sample_username + ':' + sample_password
     except:
         pass

     return False

class S(BaseHTTPRequestHandler):
    def _set_OK_headers(self):
        self.send_response(200)
        self.send_header('Content-type', 'application/json')
        self.end_headers()

    def _set_Forbidden_headers(self):
        self.send_response(403)
        self.send_header('Content-type', 'application/json')
        self.end_headers()

    def _set_Unauthorized_headers(self):
        self.send_response(401)
        self.send_header('Content-type', 'application/json')
        self.end_headers()

    def do_GET(self):
        path = self.path.split('?')[0]
        if path == "/private":
            # check for the basic auth token
            if self.headers['Authorization'] != '':
                if verify_credentials(self.headers['Authorization']):
                    self._set_OK_headers()
                    self.wfile.write('{"message": "private authenticated data ' + self.path + '"}')
                else:
                    self._set_Unauthorized_headers()
                    self.wfile.write('{"message": "unauthorized ' + self.path + '"}')
            else:
                self._set_Forbidden_headers()
                self.wfile.write('{"message": "forbidded ' + self.path + '"}')
        else:
            self._set_OK_headers()
            self.wfile.write('{"message": "public data ' + self.path + '"}')

    # this method can be used for implementing basic username/password auth and then working with "Bearer TOKEN"
    def do_POST(self):
        # Doesn't do anything with posted data
        path = self.path.split('?')[0]
        if path == '/login':
            # check password
            if self.headers['Authorization'] != '':
                if verify_credentials(self.headers['Authorization']):
                    self._set_OK_headers()
                    self.wfile.write('{"token": "' + sample_token + '"}')
                else:
                    self._set_Unauthorized_headers()
                    self.wfile.write('{"message": "unauthorized ' + self.path + '"}')
            else:
                self._set_Forbidden_headers()
                self.wfile.write('{"message": "private page ' + self.path + '"}')
        else:
            self._set_OK_headers()
            self.wfile.write('{"message": "do nothing for ' + self.path + '"}')
        
def run(server_class=HTTPServer, handler_class=S, port=80):
    server_address = ('', port)
    httpd = server_class(server_address, handler_class)
    print 'Starting httpd...'
    httpd.serve_forever()

if __name__ == "__main__":
    from sys import argv

    if len(argv) == 2:
        run(port=int(argv[1]))
    else:
        run()
