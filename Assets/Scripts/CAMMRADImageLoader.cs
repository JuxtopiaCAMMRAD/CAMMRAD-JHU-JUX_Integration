using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CAMMRADImageLoader : MonoBehaviour {
		
	public const string PM = "PM", TRACKING= "Tracking";

	public Texture2D LoadImage(string type, string task_dir_name, string filename){

	
	Debug.Log (Application.persistentDataPath);
	Texture2D tex = null;
		byte[] fileData;
		Debug.Log ((Application.streamingAssetsPath + "/JuxResources/Tasks/" + task_dir_name + "/PM/Images/" + filename));

 
		switch (type) {
		case PM: //Looking for an image related to the PM


			tex = new Texture2D (2, 2);

			string filePath = Application.streamingAssetsPath + "/JuxResources/Tasks/" + task_dir_name + "/PM/Images/" + filename;

			// Code to deal with android compatibility
			if( filePath.Contains( "://" ) ) { // When in android
				WWW www = new WWW( filePath );
				//yield return www;
				while( !www.isDone ) { }

				tex.LoadImage( www.bytes );
			}
			else {                       // When not in android
				tex.LoadImage( File.ReadAllBytes( filePath ) );
			}


			break;
		case TRACKING://Looking for an image related to Tracking
			if (File.Exists ((Application.streamingAssetsPath + "/JuxResources/Tasks/" + task_dir_name + "/Tracking/Images/" + filename))) {
				fileData = File.ReadAllBytes ((Application.streamingAssetsPath + "/JuxResources/Tasks/" + task_dir_name + "/Tracking/Images/" + filename));
				tex = new Texture2D (2, 2);
				tex.LoadImage (fileData); 
			} else {

				Debug.Log ("camjux file does not exists");

			}

			break;

		default: 
			break;

		}

		return tex;

		}

	}

