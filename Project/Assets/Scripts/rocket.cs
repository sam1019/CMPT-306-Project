using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rocket : MonoBehaviour {


	public Vector3 moveDestination;
	public float moveSpeed = 10.0f;
	public List<GameObject> explorePrefabList = new List<GameObject> ();

	private GameObject explore;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Moving the player to destination
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f) {

			//SendMessage("Play","jetMove");
			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			
			//Used to check if the player has reached it's destination, if so next turn
			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				transform.position = moveDestination;
				explore = Instantiate(explorePrefabList[UnityEngine.Random.Range(0,explorePrefabList.Count-1)], transform.position + Vector3.back *2, transform.rotation) as GameObject;
				Destroy(gameObject);
				Destroy(explore.gameObject,1);
			}
		}
	}
}
