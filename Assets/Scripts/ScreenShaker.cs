using UnityEngine;
using System.Collections;

public class ScreenShaker : MonoBehaviour {

    public static ScreenShaker instance;
    public bool shaking = false;
    public float intensity = 0;
    public float damping = 0;
    public float duration = 0;

    void Awake()
    {
        instance = this;
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShakeIt();

        }
        if (shaking)
        {
            transform.Rotate(transform.forward, Random.Range(-intensity, intensity) * Time.deltaTime, Space.Self);
            intensity = Mathf.Max(intensity - damping, 0);

        }


	}
    public void ShakeIt()
    {
        StartCoroutine(Shake(20f, 1f, 0.1f));



    }

    public void ShakeIt(float intensity, float duration,float damping)
    {
        StartCoroutine(Shake(intensity, duration, damping));



    }

    public IEnumerator Shake(float intensity, float duration, float damping)
    {
        shaking = true;
        this.intensity = intensity;
        this.damping = damping;
        yield return new WaitForSeconds(duration);
        shaking = false;


    }
}
