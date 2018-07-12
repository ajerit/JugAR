using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackList : MonoBehaviour {

	IEnumerable<TrackableBehaviour> activeTracks;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		StateManager sm = TrackerManager.Instance.GetStateManager();
		IEnumerable<TrackableBehaviour> activeTracks = sm.GetActiveTrackableBehaviours();
		//Debug.Log("List of tracking:");
		foreach (TrackableBehaviour tb in activeTracks) {
			//Debug.Log("Tracking: " + tb.TrackableName);
			if (tb.TrackableName.Equals("jon-wall")) {
				//tempPos = tb.transform.position; tb.transform.position.y
				GameObject tablero = GameObject.Find("TableroTarget");
				tb.transform.position = new Vector3(tb.transform.position.x, tablero.transform.position.y,tb.transform.position.z);
				//tb.transform.rotation = tablero.transform.rotation;
				//tempRot = tb.transform.rotation;
				//tb.transform.rotation = new Vector3(tempRot.x, 0f, tempRot.z);
			}
		}
		
	}
}
