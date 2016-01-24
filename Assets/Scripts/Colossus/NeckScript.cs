using UnityEngine;
using System.Collections;

public class NeckScript : MonoBehaviour {

    public float normalSize;
    private float size;
    private float t = 0;
    public float breathSpeed = 2;
    public float amplitudeBreath = 2;
	// Use this for initialization
	void Start () {
        normalSize = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
        t+=Time.deltaTime;
	    size = normalSize + amplitudeBreath *Mathf.Sin(t * breathSpeed);
        transform.localScale = new Vector3(size, size, size);


	}
}
