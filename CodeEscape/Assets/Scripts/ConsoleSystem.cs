using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum CommandType
{
	None,
	Result,
	Log,
	Error,
	Special
};

public class ConsoleSystem : MonoBehaviour {
	 
	bool showConsole;
	string command = "";
	List<HistoryItem> consoleHistory;
	Vector2 scrollPosition = Vector2.zero;
	
	//main console window style
	GUIStyle consoleBackground = new GUIStyle();
	
	//text styles for items displayed in console
	GUIStyle consoleTextNone = new GUIStyle();
	GUIStyle consoleTextSpecial = new GUIStyle();
	GUIStyle consoleTextResult = new GUIStyle();
	GUIStyle consoleTextLog = new GUIStyle();
	GUIStyle consoleTextError = new GUIStyle();
	
		
	public int ItemHeight = 20;		//line height for each item
	public int MaxItems = 2000;  	//maximum history records
	public int MaxDisplayed = 10; 	//default console size - will show 10 historic items
	
	public OpenDoor open_door;
	public Move_1 move_1;
	public Move_2 move_2;
	public Move_3 move_3;
	public Move_4 move_4;
	private string bullets  = "Code Loaded:\nNone";
	
	

	
	
	
	internal class HistoryItem
	{
		internal string command;
		internal CommandType type;
	};
	
	static ConsoleSystem instance;
	public static void Log(string message)
	{
		if(instance != null)
			instance.AddHistory(message, CommandType.Log);
	}
	
	public static void Log(string message, CommandType type)
	{
		if(instance != null)
			instance.AddHistory(message, type);
	}

	void Start () {
		
		//setup static instance for static calls
		instance = this;
		consoleHistory = new List<HistoryItem>();
		
		//setup console styles
		consoleTextNone.normal.textColor = Color.white;
		consoleTextSpecial.normal.textColor = Color.blue;
		consoleTextResult.normal.textColor = Color.green;
		consoleTextLog.normal.textColor = Color.yellow;
		consoleTextError.normal.textColor = Color.red;
		
		//create a default texture to use for background of console
		Texture2D tex = new Texture2D(1, 1);
		tex.SetPixel(0,0, Color.gray);
		tex.Apply();
		consoleBackground.normal.background = tex;
		
		ConsoleSystem.Log("Console System Initialised");
		Screen.lockCursor = true;
		
		
	}

	void Update () {
		
		if(Input.GetKeyDown(KeyCode.BackQuote))
			showConsole = !showConsole;
		
		
	}
	
	void AddHistory(string command, CommandType type)
	{
		consoleHistory.Add(new HistoryItem() { command = command, type = type });
	
		if(consoleHistory.Count > MaxItems)
			consoleHistory.RemoveAt(0);
		
		scrollPosition.y = Mathf.Clamp((consoleHistory.Count * ItemHeight) - (MaxDisplayed * ItemHeight), 0 , MaxItems * ItemHeight);
	}
	
	GUIStyle GetStyle(CommandType type)
	{
		switch(type)
		{
			case CommandType.None:
				return consoleTextNone;
			case CommandType.Result:
				return consoleTextResult;
			case CommandType.Special:
				return consoleTextSpecial;
			case CommandType.Log:
				return consoleTextLog;
			case CommandType.Error:
				return consoleTextError;	
		}
		
		return consoleTextNone;
	}
	
	void OnGUI()
	{
		GUI.Box(new Rect(Screen.width - 150,Screen.height - 100,150,100), "Code Loaded:\n" + bullets);
		if(open_door.enabled == true){
			bullets = "open(door)";
		}
		else if(move_1.enabled == true){
			bullets = "move(platform, 1)";
		}
		else if(move_2.enabled == true){
			bullets = "move(platform, 2)";
		}
		else if(move_3.enabled == true){
			bullets = "move(platform, 3)";
		}
		else if(move_4.enabled == true){
			bullets = "move(platform, 4)";
		}
		else {
			bullets = "None";
		}
		if(showConsole)
		{
			//catch certain key presses
			Event e = Event.current;
        	if (e != null)
			{
				if(e.type == EventType.KeyDown)
				{
					if(e.keyCode == KeyCode.Return) 
						ExecuteCommand();
					if(e.keyCode == KeyCode.BackQuote)
					{
						showConsole = false;
						e.Use();
					}
				}
			}
			
			GUI.BeginGroup(new Rect(0,0, 500, (MaxDisplayed+1) * ItemHeight), consoleBackground);	
			
			scrollPosition = GUI.BeginScrollView(new Rect(0,0, 500, MaxDisplayed * ItemHeight), scrollPosition, new Rect(0, 0, 500-20, consoleHistory.Count*ItemHeight),false, true);
			
			int index = 0;
			foreach(HistoryItem history in consoleHistory)
			{
				GUI.Label(new Rect(0,index*ItemHeight,500,ItemHeight), history.command, GetStyle(history.type));
				index++;
			}
			 GUI.EndScrollView();
			
			command = command.Replace("`","");
			
			GUI.SetNextControlName("CommandField");
			command = GUI.TextField(new Rect(0,MaxDisplayed * ItemHeight,500, ItemHeight),command);
			GUI.EndGroup();
			
			GUI.FocusControl("CommandField");
			
			if(scrollPosition.y < 0)
				scrollPosition.y = 0;
		}
	}
void ExecuteCommand()
	{
		if(command.Trim().Length > 0)
		{
			string c = command.ToLower().Trim();
			if(c.StartsWith("clear()"))
				consoleHistory.Clear();
			else 
			{
				AddHistory(command, CommandType.None);				
                if (c.StartsWith("open(")){
					if(c.Contains ("door)")){
						AddHistory ("Open Function Initialized", CommandType.Log);
						open_door.enabled = true;
						}
					else if (c.Contains ("door)") == false){
							AddHistory("Error: That is not a valid parameter.", CommandType.Error);
						}
				}
				else if(c.StartsWith("move(")){
						if (c.EndsWith(")")){
							AddHistory ("Move Function Initialized", CommandType.Log);
							if(c.Contains("platform,")){
								AddHistory ("Object to Move {Platform}", CommandType.Log);
								if (c.Contains ("1)")){
									AddHistory ("Platform to Move {1}", CommandType.Log);
									move_1.enabled = true;
									
									}
								else if(c.Contains ("2)")){
									AddHistory ("Platform to Move {2}", CommandType.Log);
									move_2.enabled = true;
									}
								else if(c.Contains("3)")){
									AddHistory ("Platform to Move {3}", CommandType.Log);
									move_3.enabled = true;
								}
								else if(c.Contains("4)")){
									AddHistory ("Platform to Move {4}", CommandType.Log);
									move_4.enabled = true;
									}
								}
							}

					else if(c.Contains ("platform")== false){
							AddHistory("Error: That is not a valid parameter.", CommandType.Error);
						}
					else if(c.Contains ("1")== false){
							if(c.Contains ("2")==false)
								if(c.Contains ("3")== false)
									if(c.Contains("4")== false)
										AddHistory("Error: That is not a valid parameter.", CommandType.Error);
						}
				}
				if(c.StartsWith("open()")){
					AddHistory("Error: Open Function Requires One Parameter; open(<object>)", CommandType.Error);
				}
				else if(c.StartsWith("move()")){
					AddHistory("Error: Move Function Requires Two Parameters; move(<object>, <object_number>)", CommandType.Error);
				}
			}
		}
	command = "";
	}    
}
			
				