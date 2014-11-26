using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public string[] audioName;
	public AudioClip[] audioClip;
	public bool clipFound; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play(string clipName){
		for (int i=0; i< audioName.Length; i++) {
			if(clipName == audioName[i]){
				gameObject.audio.clip = audioClip[i];
				gameObject.audio.Play();
				clipFound=true;
				break;
			}
			else{
				clipFound=false;
			}
		}
		if (!clipFound) {
			Debug.Log("Audio Clip Not Found!");
		}
	}
}
