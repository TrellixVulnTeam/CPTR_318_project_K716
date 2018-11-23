﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class login : MonoBehaviour {
	
	//should be set to UsernameField 
	public InputField Username;
	//should be set to PasswordField
	public InputField Password;

	//has to be initialized to a full array to enable the check to see if all the values have been supplied before submitting them
	private string[] newEventData = {"", ""};
	
	private readonly Color badData = new Color(0.91f, 0.26f, 0.26f, 255);
	private readonly Color goodData = new Color(1, 1, 1, 255);

	private bool check_string(string val)
	{
		bool bad = true;
		string[] vals = val.Split(' ');
		
		foreach (string p in vals)
		{
			if (p != "")
			{
				bad = false;
				break;
			}
		}

		return bad;
	}

	//should be used by the UsernameField
	public void username_input()
	{
		string val = Username.text;
		
		//if unusable
		if (check_string(val))
			Username.GetComponent<Image>().color = badData;
		else
		{
			Username.GetComponent<Image>().color = goodData;
			newEventData[0] = val;
		}
	}
	
	//should be used by the PasswordField
	public void password_input()
	{
		string val = Password.text;
		
		//if unusable
		if (check_string(val))
			Password.GetComponent<Image>().color = badData;
		else
		{
			Password.GetComponent<Image>().color = goodData;
			newEventData[1] = val;
		}
	}
	
	// used by the Login button
	public void loggin()
	{
		//if all values have been supplied
		if (ArrayUtility.IndexOf(newEventData, "") == -1)
		{
			// send newEventData to to database with protocol






			bool response = true;
				
				
				
			// *** ***
			
			if(response)
				SceneManager.LoadScene("CalendarView");
		}
		else
		{
			if (newEventData[0] == "")
			{
				Username.GetComponent<Image>().color = badData;
			}

			if (newEventData[1] == "")
			{
				Password.GetComponent<Image>().color = badData;
			}
		}
	}

	// used by the CreateAccount button
	public void create_new()
	{
		SceneManager.LoadScene("CreateAccount");
	}
}