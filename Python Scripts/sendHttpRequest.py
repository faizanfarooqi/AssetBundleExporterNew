import json   
import requests
import sys
import os
import getopt
from pprint import pprint

def main():
        retVal = 0 #denotes success
	options, arguments = getopt.getopt(sys.argv[1:],"")
	print "Mumber of arguments passed is "+ str(len(arguments))
	if len(arguments)==1:
                strAssetBundleZipName = arguments[0];

                #Creating payload for request
                objUnityAssetBundleInfo = {}
                objUnityAssetBundleInfo['bucketUrl'] = 'http://'
                objUnityAssetBundleInfo['fileName'] = strAssetBundleZipName+'.zip'
                
                objThumbnailInfo = {}
                objThumbnailInfo['bucketUrl'] = 'http://'
                objThumbnailInfo['fileName'] = '1114_LesArcsChairByCharlottePerriand01.jpg'
                
                objRequestPayload = {}
                objRequestPayload['variantId'] = '1114'
                objRequestPayload['unityAssetBundle'] = objUnityAssetBundleInfo
                objRequestPayload['thumbnail'] = objThumbnailInfo

                #print(json.dumps(objRequestPayload))
                strApiURL='http://34.210.53.110/roombuilder_backend/index.php/api/variant_asset/set_variant_asset/1';
                apiResponse = requests.post(strApiURL, data=json.dumps(objRequestPayload))
                if str(apiResponse.status_code)=='200':
                        print "Api returned success"
                else:
                        print "Api returned " + apiResponse.status_code
                        retVal = 2
        else:
                print "Parameters not passed correctly."
                retVal = 1
        return retVal
if __name__ == "__main__":
	sys.exit(main())
