import json
import sys
import os
import getopt
from pprint import pprint

#script to export filname from json file(taken as argument) to output file(taken as second argument)
# created by Faizan-ur-Rehman Last Updated 04 July, 2017
def main(): 
	retVal = 0 #denotes success
	options, arguments = getopt.getopt(sys.argv[1:],"")
	print "Mumber of arguments passed is "+ str(len(arguments))
	if len(arguments)==2:
		print(sys.argv)
		jsonFileLocation = arguments[0]
		outputFileLocation = arguments[1]
		if os.path.exists(jsonFileLocation):
			target = open(outputFileLocation, 'w')
			with open(jsonFileLocation) as data_file:    
				data = json.load(data_file)

			pprint(data)
			if 'unityAsset' in data:
				if 'fileName' in data['unityAsset']:
					print("Filename from json File is " + data["unityAsset"]["fileName"])
					target.write(data["unityAsset"]["fileName"])
				else:
					retVal = -4
					print "filename is not found as key under unityAsset key in json file"
			else:
				retVal = -3
				print "unityAsset is not found as key in json file."
			
		else:
			retVal = -2
			print "Input json file does not exists."
	else:
		retVal = -1
		print "You must provide two arguments. First Json file path and second output file path to out value of keys exported from json file."
	return retVal
	
if __name__ == "__main__":
	sys.exit(main())
