import http.client
import urllib.request
import urllib.parse
import urllib.error
import base64
import json

# Set these values to the correct values for your Video Indexer account
accountId = 'accountId'
location = 'location'
accessToken = 'accessToken'


allowEdit = 'True'
accessToken = ''

# Set these values to the correct values for your video
name = 'name'
description = 'description'
videoUrl = 'videoUrl'

accessTokenHeaders = {
    # Request headers
    'Ocp-Apim-Subscription-Key': subscriptionKey,
}

accessTokenParams = urllib.parse.urlencode({
    # Request parameters
    'allowEdit': allowEdit,
})

try:
    print("Getting Access Token...")
    conn = http.client.HTTPSConnection('api.videoindexer.ai')

     # Call the Get Account Access Token method
    conn.request("GET", "/auth/" + location + "/Accounts/" + accountId + "/AccessToken?%s" % accessTokenParams, "{body}", accessTokenHeaders)
    response = conn.getresponse()
    data = response.read()

    # Convert the byte array to a string
    accessToken = data[1:-1].decode("utf-8") 
    
    conn.close()
except Exception as e:
    print("[Errno {0}] {1}".format(e.errno, e.strerror))






headers = {
}

params = urllib.parse.urlencode({
    # Request parameters
    'privacy': 'Private',
    'description': description,
    'videoUrl': videoUrl,

})

try:
    conn = http.client.HTTPSConnection('api.videoindexer.ai')
    print("Adding Media Asset")
    conn.request("POST", "/" + location + "/Accounts/" + accountId + "/Videos?name=" + name + "&accessToken=" + accessToken + "&%s" % params, "", headers)
    response = conn.getresponse()
    data = response.read()
    print(data)
    conn.close()
except Exception as e:
    print("[Errno {0}] {1}".format(e.errno, e.strerror))

