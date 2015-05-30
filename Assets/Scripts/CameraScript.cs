using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	Deion player;
	// Use this for initialization
	void Start () {
		GameObject obj = GameObject.Find("Deion");
		player = (Deion)obj.GetComponent ("Deion");
	}

	// Update is called once per frame
	void Update () {
		Vector3 newpos = player.transform.position;
		transform.position = new Vector3(newpos.x, newpos.y, -10);
	}
}
