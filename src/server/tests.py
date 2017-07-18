#/usr/bin/env python3

import sys
import json

import requests

serverConfig = json.loads(open("Speercs.Server/speercs.json", 'r').read());

testResults = {};
useToken = "";

print("Testing registration");
r = requests.post("http://localhost:5000/a/auth/register", data = {
    "username": "UnitTest",
    "password": "validpassword"
});

testResults["ValidRegistration"] = r.status_code == 200;

print("Testing existing username");
r = requests.post("http://localhost:5000/a/auth/register", data = {
    "username": "UnitTest",
    "password": "validpassword"
});

testResults["RepeatRegistration"] = r.status_code == 401;

print("Testing invalid username");
r = requests.post("http://localhost:5000/a/auth/register", data = {
    "username": "un* tTest",
    "password": "validpassword"
});

testResults["InvalidNameRegistration"] = r.status_code == 422;

print("Testing valid login");
r = requests.post("http://localhost:5000/a/auth/login", data = {
    "username": "UnitTest",
    "password": "validpassword"
});

testResults["ValidLogin"] = (r.status_code == 200 and r.json()["username"] == "UnitTest");
if r.status_code == 200:
    useToken = r.json()["apikey"];
else:
    print("Unable to continue without token :/");
    sys.exit(1);

print(useToken);

print("Testing invalid login");
r = requests.post("http://localhost:5000/a/auth/login", data = {
    "username": "unitTest",
    "password": "invalidpassword"
});

testResults["InvalidLogin"] = r.status_code == 401;

print("Testing room creation");
r = requests.post("http://localhost:5000/a/admin/map", data = {
    "x": 0,
    "y": 0,
    "density": 2
}, headers={
    "Authorization": serverConfig["adminKeys"][0]
});

testResults["RoomCreate"] = r.status_code == 200;

print("Testing valid room");
r = requests.get("http://localhost:5000/a/game/map/room", params = {
    "x": 0,
    "y": 0
}, headers={
    "Authorization": useToken
});

print(r.text);

testResults["ValidRoom"] = r.status_code == 200;

print("Testing invalid room");
r = requests.get("http://localhost:5000/a/game/map/room", params = {
    "x": 999,
    "y": 999
}, headers={
    "Authorization": useToken
});

testResults["InvalidRoom"] = r.status_code == 404;

print(testResults);
sys.exit(0);