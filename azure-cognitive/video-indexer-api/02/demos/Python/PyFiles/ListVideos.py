

import http.client, urllib.request, urllib.parse, urllib.error, base64

# Set these values to the correct values for your Video Indexer account
accountId = 'accountId'
location = 'location'
accessToken = 'accessToken'


headers = {
  'accessToken': accessToken
}

params = urllib.parse.urlencode({
    # Request parameters
    'pageSize': '25',
    'skip': '0',
})

try:
    conn = http.client.HTTPSConnection('api.videoindexer.ai')
    conn.request("GET", "/" + location + "/Accounts/" + accountId + "/Videos?accessToken={accessToken}&%s" % params, "{body}", headers)
    response = conn.getresponse()
    data = response.read()
    print(data)
    conn.close()
except Exception as e:
    print("[Errno {0}] {1}".format(e.errno, e.strerror))

