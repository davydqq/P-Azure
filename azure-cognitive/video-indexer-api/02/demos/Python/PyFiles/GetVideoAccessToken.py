
########### Python 3.2 #############
import http.client, urllib.request, urllib.parse, urllib.error, base64



# Set these values to the correct values for your Video Indexer account
accountId = 'accountId'
subscriptionKey = 'subscriptionKey'
location = 'location'

allowEdit = 'False'

# Set these values to the correct values for your video
videoId = 'videoId'

headers = {
    # Request headers
    'Ocp-Apim-Subscription-Key': subscriptionKey,
}

params = urllib.parse.urlencode({
    # Request parameters
    'allowEdit': allowEdit,
})

try:
    conn = http.client.HTTPSConnection('api.videoindexer.ai')
    conn.request("GET", "/auth/" + location + "/Accounts/" + accountId + "/Videos/" + videoId + "/AccessToken?%s" % params, "{body}", headers)
    response = conn.getresponse()
    data = response.read()
    print(data)
    conn.close()
except Exception as e:
    print("[Errno {0}] {1}".format(e.errno, e.strerror))

####################################
