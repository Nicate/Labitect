using System;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour {
	public Text logText;


	public void log(string message) {
		if(logText.text == "") {
			logText.text = message;
		}
		else {
			logText.text = logText.text + Environment.NewLine + message;
		}
	}
}
