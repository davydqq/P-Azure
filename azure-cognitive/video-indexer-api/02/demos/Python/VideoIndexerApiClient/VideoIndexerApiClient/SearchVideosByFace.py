
import http.client
import urllib.request
import urllib.parse
import urllib.error
import base64
import json

# Set these values to the correct values for your Video Indexer account
accountId = 'accountId'
subscriptionKey = 'subscriptionKey'
location = 'location'

allowEdit = 'False'
accessToken = ''

# Specify a search query
face = 'James'


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
    'face': face,
    'pageSize': '25',
    'skip': '0',
    'accessToken': accessToken,
})

try:
    conn = http.client.HTTPSConnection('api.videoindexer.ai')
    conn.request("GET", "/" + location + "/Accounts/" + accountId + "/Videos/Search?%s" % params, "{body}", headers)
    response = conn.getresponse()
    data = response.read()


    searchJsonString = data.decode("utf-8") 

    #print(searchJsonString)

    jsonDoc = json.loads(searchJsonString)

    for result in jsonDoc['results']:
        print (result['name'])

        for match in result['searchMatches']:
            print ('    ' + match['startTime'][:8] + ' - ' + match['type'] + ' - ' +  match['text'])

    conn.close()
except Exception as e:
    print("[Errno {0}] {1}".format(e.errno, e.strerror))

####################################



