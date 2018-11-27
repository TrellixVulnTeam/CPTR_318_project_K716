﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

public class ClientComms : MonoBehaviour {

	private TcpClient socket;
	private Thread clientThread;
	private int timeout = 20;

	public bool Connected
	{
		get { return socket != null && socket.Connected; }
		
	}

	public void Connect()
	{
		try
		{
			clientThread = new Thread(Execute);
			clientThread.IsBackground = true;
			clientThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}

	private void Execute()
	{
		Debug.Log("need to reset this to the RPi's IP address and an open port");
		socket = new TcpClient(state.Server_ip, state.Server_port);
		
		Byte[] bytes = new Byte[state.MaxReplyLen];
		
		int events_rcvd = 0;
		int events_to_rcv = 0;

		while (true)
		{
			using (NetworkStream stream = socket.GetStream())
			{
				int length;

				while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
				{
					var incommingData = new byte[length];
					
					Array.Copy(bytes, 0, incommingData, 0, length);
					
					string serverMessage = Encoding.ASCII.GetString(incommingData); 
					string[] strArr = serverMessage.Split(',');

					switch (strArr[0])
					{
						case "51":
							state.RequestLoginAuth.data = strArr[1];

							state.RequestLoginAuth.written = true;
							break;
							
						case "52":
							//convert to list of dictionaries
							if (strArr[1] == "a")
							{
								events_to_rcv = int.Parse(strArr[2]);
								state.displayable_events.data.Clear();
							}
							else if (strArr[1] == "b")
							{
								Dictionary<string, object> evnt = new Dictionary<string, object>();
								evnt.Add("title", strArr[3]);
								evnt.Add("date", strArr[4]);
								evnt.Add("start time", strArr[5]);
								evnt.Add("end time", strArr[6]);
								
								state.displayable_events.data.Add(evnt);

								events_rcvd++;
							}

							if (events_rcvd == events_to_rcv)
							{
								events_rcvd = 0;
								events_to_rcv = 0;

								state.displayable_events.written = true;
							}
							break;
						
						case "53":
							state.SendNewEvent.data = strArr[1];

							state.SendNewEvent.written = true;
							break;
							
						case "54":
							state.SendEditedEvent.data = strArr[1];

							state.SendEditedEvent.written = true;
							break;
						
						case "55":
							state.SendNewUser.data = strArr[1];

							state.SendNewUser.written = true;
							break;
					}
					
				}
			}
		}
	}

	private void SendMessage(string message)
	{
		if (socket == null)
		{
			return;
		}
		
		try
		{
			NetworkStream stream = socket.GetStream();

			if (stream.CanWrite)
			{
				//get message up to length
				if (message.Length < state.MaxReplyLen)
				{
					message += ",";
					
					while (message.Length < state.MaxReplyLen)
					{
						message += " ";
					}
				}
				
				//turn message from string into bytes
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);
				
				//write byte array to socket stream
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}

	public void RequestLoginAuth(string user, string pswd)
	{
		SendMessage("01," + user + "," + pswd);
	}

	public void RequestEventRange(string start_date, string end_date)
	{
		SendMessage("02," + start_date + "," + end_date);
	}

	public void SendNewEvent(string title, string date, string start_time, string end_time)
	{
		SendMessage("03," + title + "," + date + "," + start_time + "," + end_time);
	}

	public void SendEditedEvent(string title, string date, string start_time, string end_time)
	{
		SendMessage("04," + title + "," + date + "," + start_time + "," + end_time);
	}
	
	public void SendNewUser(string user, string pswd)
	{
		SendMessage("05," + user + "," + pswd);
	}
}