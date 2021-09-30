

import http.client, urllib.request, urllib.parse, urllib.error, base64


# Set these values to the correct values for your Video Indexer account
accountId = 'accountId'
location = 'location'
accessToken = 'accessToken'

# Set these values to the details of the video and face
videoId = 'videoId'
faceId = 'faceId'
newName = 'newName'

headers = {
    # Request headers
    'x-ms-client-request-id': '',
}

params = urllib.parse.urlencode({
    # Request parameters
    'newName': newName,
    #'personId': '{string}',
    #'createNewPerson': 'False',
    'accessToken': accessToken,
})

try:
    conn = http.client.HTTPSConnection('api.videoindexer.ai')
    conn.request("PUT", "/" + location + "/Accounts/" + accountId + "/Videos/" + videoId + "/Index/Faces/" + faceId + "?%s" % params, "", headers)
    response = conn.getresponse()
    data = response.read()
    print(data)
    conn.close()
except Exception as e:
    print("[Errno {0}] {1}".format(e.errno, e.strerror))

