using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {

	public AudioSource source;
	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {		
		if (!source.isPlaying) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}
			
	}
}
