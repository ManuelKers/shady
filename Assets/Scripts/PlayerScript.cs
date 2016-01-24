using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public float moveSpeed;
    public static PlayerScript instance;

    void Awake() {
        instance = this;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
	}


    public void Movement()
    {
        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        //transform.Rotate(transform.up, hor * rotationSpeed * Time.deltaTime, Space.World);
        transform.position += (transform.forward * vert + transform.right* hor).normalized * moveSpeed * Time.deltaTime;
    }
}
