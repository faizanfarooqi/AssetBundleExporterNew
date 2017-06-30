using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEditor;

public class RBAssetBundleExporter {

	//we donot need these function as this is editor script being called without playmode
	// Use this for initialization
	/*void Start () {
		Debug.Log("start method called");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	*/

	/*
	Function to build asset bundles and export them to the specified directory
	Created by Faizan-ur-Rehman (Original Author) 
	Last updated 6/22/2017
	Modified By: Faizan-ur-Rehman
	*/
	static void BuildAllAssetBundles ()
    {   
     	
		Debug.Log("Main script is started");
		string str_logFilePath = "C:\\Users\\Faizan\\Desktop\\assetBundleExporterLog.txt";
		string str_logText="In Editor script to export bundles";
		string str_unityProjectPath="C:\\Users\\Faizan\\Documents\\AssetBundleExporter\\";
		UnityEngine.Object[] unityObject_resources;
		bool b_isAssetBundleExported=false;
		int errorCode=0;
		if(System.IO.Directory.Exists(str_unityProjectPath+"Assets\\parrentResources")==false)
		{
			str_logText=str_logText+"\nPredefined path for resources in unity project ("+str_unityProjectPath+"Assets\\parrentResources) does not exist";
			errorCode=1;
		}
		else
		{
			string[] strArray_intermediateAddress=System.IO.Directory.GetDirectories(str_unityProjectPath+"Assets\\parrentResources");
			if(strArray_intermediateAddress.Length!=1)
			{
				if(strArray_intermediateAddress.Length<1)
				{
					str_logText=str_logText+"\nFolder in parrentResources which must contain resources folder is not found.";
				}
				if(strArray_intermediateAddress.Length>1)
				{
					str_logText=str_logText+"\nThere exists more then one directories in parrentResources Folder. expecting 1";
				}
				File.WriteAllText(str_logFilePath, str_logText);
				errorCode=2;		
			}
			else
			{
				string str_uniqueFolderName=getFoldernameFromPath(strArray_intermediateAddress[0]);
				if(System.IO.Directory.Exists(str_unityProjectPath+"Assets\\parrentResources\\"+str_uniqueFolderName+"\\Resources")==false)
				{
					str_logText=str_logText+"\nResources folder does not exist";
					errorCode=3;
				}
				else
				{
					//Do this for one folder whose name is taken from command line
					/*
					string[] strArray_resourceFolderNames=System.IO.Directory.GetDirectories(str_unityProjectPath+"Assets\\parrentResources\\"+str_uniqueFolderName+"\\Resources");
				
					//Path exists. No assign asset bundles to all files corresponding to folder names
					foreach (var str_folderPath in  strArray_resourceFolderNames)
					{
						var str_folder=getFoldernameFromPath(str_folderPath);
						Debug.Log("folder name is "+str_folder);
						str_logText=str_logText+"\nfolder name is "+str_folder;
						unityObject_resources = Resources.LoadAll(str_folder);

				        foreach (var resource in unityObject_resources)
				        {

				            Debug.Log("resource in folder "+str_folder+" is "+resource.name);
				            str_logText=str_logText+"\nresource in folder "+str_folder+" is "+resource.name;
				            Debug.Log(AssetDatabase.GetAssetPath(resource));
				            string str_assetPath = AssetDatabase.GetAssetPath(resource);
				         	AssetImporter.GetAtPath(str_assetPath).SetAssetBundleNameAndVariant(str_folder, "");

				        }
					}
					*/

					string[] arguments = Environment.GetCommandLineArgs();
					if(arguments.Length<1)
					{
						str_logText=str_logText+"\nAsset Folder name as Argument is not provided";
						errorCode=4;	
					}
					else
					{
						var strAssetFolderName = arguments[arguments.Length-1].ToString();
						Debug.Log("Folder name recieved through command line is " + strAssetFolderName);
						str_logText=str_logText+"\nfolder name is "+strAssetFolderName;
						if(System.IO.Directory.Exists(str_unityProjectPath+"Assets\\parrentResources\\"+str_uniqueFolderName+"\\Resources\\"+strAssetFolderName)==false)
						{
							str_logText=str_logText+"\nAsset Folder passed through comamnd line doesn't exist in resource folder";
							errorCode=5;
						}
						else
						{
							unityObject_resources = Resources.LoadAll(strAssetFolderName);
					        foreach (var resource in unityObject_resources)
					        {

					            Debug.Log("resource in folder "+strAssetFolderName+" is "+resource.name);
					            str_logText=str_logText+"\nresource in folder "+strAssetFolderName+" is "+resource.name;
					            Debug.Log(AssetDatabase.GetAssetPath(resource));
					            string str_assetPath = AssetDatabase.GetAssetPath(resource);
					         	AssetImporter.GetAtPath(str_assetPath).SetAssetBundleNameAndVariant(strAssetFolderName, "");

					        }

					        //Asset bundle names assigned. Now export bundles
					        BuildPipeline.BuildAssetBundles("newAssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
					        b_isAssetBundleExported=true;	
						}

					}
					

				}
					
			}
			
		}

		if(b_isAssetBundleExported==false)
		{
			str_logText=str_logText+"\nAsset Bundles are not exported correctly";
		}
		//writing Log in manual file
		File.WriteAllText(str_logFilePath, str_logText);
		if(errorCode!=0)
		{
			EditorApplication.Exit(errorCode);
		}
	

		/*
		Debug.Log("In the funciton and calling refresh funciton");
		AssetDatabase.Refresh();
		AssetDatabase.importPackageStarted();
		string str_logFilePath = "C:\\Users\\Faizan\\Desktop\\assetBundleExporterLog.txt";
		string str_logText="In Editor script to export bundles";
		str_logText=str_logText+"\nasset refresh function called";
		File.WriteAllText(str_logFilePath, str_logText);
		*/
	}
	
	/*
	Function that takes full absolute address and returns final folder name in the path
	Created by Faizan-ur-Rehman (original Author)
	Last Updated 6/22/2017
	Modified By: Faizan-ur-Rehman
	*/
	public  static string getFoldernameFromPath(string a_address)
	{
		int lastHashIndex=0;
		for(int i=0;i<a_address.Length;i++)
		{
			if(a_address[i]=='\\')
			{
				lastHashIndex=i;
			}
		}
		string Foldername="";
		for(int i=lastHashIndex+1;i<a_address.Length;i++)
		{
			Foldername=Foldername+a_address[i];
		}
		return Foldername;
	}

	//overriding onGUI function
	void onGUI()
	{

	}
}
