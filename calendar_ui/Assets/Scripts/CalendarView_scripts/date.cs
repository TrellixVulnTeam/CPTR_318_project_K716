﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class date : MonoBehaviour {

	//display date
	public int day;
	public int month;
	public int year;

	public DateObj.DateObject today;
	
	//the first day of the week currently being displayed
	public DateObj.DateObject display_date;
	
	//used for last week button
	public void last_week()
	{
		display_date -= new DateObj.DateObject(0, 0, 7);
	}
	
	//used for next week button
	public void next_week()
	{
		display_date += new DateObj.DateObject(0, 0, 7);
	}

	//used for this week button
	public void this_week()
	{
		display_date = today.week_start();
	}
	
	//used by the Create event button
	public void create_event()
	{
		state.Title = "Title";
		state.Date = "Date (m/d/yyyy)";
		state.Start_time = "Start time (11:30a)";
		state.End_time = "End time (7:45p)";
		
		//link to edit event scene
		SceneManager.LoadScene("EditEvent");
	}

	//used by the Logout button
	public void logout()
	{
		//link to login scene
		SceneManager.LoadScene("Login");
	}
	
	
	// Use this for initialization
	void Start ()
	{
		DateTime ct = DateTime.Now;
		
		//allow today to be set by the user for simulation/testing purposes
		if (year != 0 && month != 0 && day != 0)
			today = new DateObj.DateObject(year, month, day);
		else
			today = new DateObj.DateObject(ct.Year, ct.Month, ct.Day);
		
		display_date = today.week_start();

		GetComponent<Text>().text = today.ToString("d:M:Y");

		this_week();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//check to see if it's a new day yet
		DateTime ct = DateTime.Now;
		
		if (ct.Year != today.year || ct.Month != today.month || ct.Day != today.day)
		{
			if (year != 0 && month != 0 && day != 0)
				today = new DateObj.DateObject(year, month, day);
			else
				today = new DateObj.DateObject(ct.Year, ct.Month, ct.Day);
			
			GetComponent<Text>().text = today.ToString("d:M:Y");
		}
	}
}