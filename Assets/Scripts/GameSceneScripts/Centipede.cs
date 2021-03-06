﻿using UnityEngine;
using System.Collections;

public class Centipede : MonoBehaviour {

	public int speed = 1;

	public string origin = "top";
	public string direction = "right";

	// Use this for initialization
	void Start () {
		StartCoroutine (ReverseDirection ());
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Barrier") {
			StopAllCoroutines();
			StartCoroutine(ReverseDirection());
		}
		if (col.gameObject.tag == "Bullet") {
			Destroy (gameObject);
			Destroy (col.gameObject);
		}
	}

	IEnumerator Move(Vector3 from, Vector3 to){
		float startTime = Time.time;
		float dist = Vector3.Distance(from, to);
		while(gameObject.rigidbody.position != to){
			float timePassed = (Time.time - startTime)*speed;
			gameObject.rigidbody.position = Vector3.Lerp (from, to, timePassed/dist);
			yield return null;
		}
	}

	IEnumerator ReverseDirection(){
		Vector3 pos = gameObject.rigidbody.position;
		Vector3 newPos = gameObject.rigidbody.position;
		switch (origin) {
			case "top":
				newPos += Vector3.down * gameObject.transform.localScale.x;
				break;
			case "bottom":
				newPos += Vector3.up * gameObject.transform.localScale.x;
				break;
			case "left":
				newPos += Vector3.right * gameObject.transform.localScale.x;
				break;
			case "right":
				newPos += Vector3.left * gameObject.transform.localScale.x;
				break;
		}
		yield return StartCoroutine( Move(pos, newPos));
		switch (direction) {
		case "up":
			direction = "down";
			break;
		case "down":
			direction = "up";
			break;
		case "left":
			direction = "right";
			break;
		case "right":
			direction = "left";
			break;
		}
		Debug.Log ("move triggered");
		yield return StartCoroutine (MoveOnceDirection (direction));
		StartCoroutine (MoveDirection(direction));
	}

	IEnumerator MoveDirection(string direction){
		while (true) {
			yield return StartCoroutine(MoveOnceDirection(direction));
		}
	}

	IEnumerator MoveOnceDirection(string direction){
		Vector3 newPos = gameObject.rigidbody.position;
		switch (direction) {
		case "down":
			newPos += Vector3.down * gameObject.transform.localScale.x;
			break;
		case "up":
			newPos += Vector3.up * gameObject.transform.localScale.x;
			break;
		case "right":
			newPos += Vector3.right * gameObject.transform.localScale.x;
			break;
		case "left":
			newPos += Vector3.left * gameObject.transform.localScale.x;
			break;
		}
		yield return StartCoroutine(Move(gameObject.rigidbody.position, newPos));
	}
}
