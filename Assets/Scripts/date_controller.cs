﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class date_controller : MonoBehaviour {

	public Text date;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		string time = System.DateTime.Now.Hour >= 13 ? System.DateTime.Now.Hour - 12 + ":" +  (System.DateTime.Now.Minute < 10 ?  "0" : "") + System.DateTime.Now.Minute + " PM": System.DateTime.Now.Hour + ":" +  (System.DateTime.Now.Minute < 10 ?  "0" : "") + System.DateTime.Now.Minute + " AM";
		setDate (time);
	}


	public void setDate(string newDate){

		date.text = newDate;
	}

}
