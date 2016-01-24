using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {
    public float moveSpeed = 3;
    public float rotationSpeed = 10;
    public float distFromGround = 0.5f;
    public float amplitudeBreathing = 0.1f;
    public float breathSpeed = 1;
    public LayerMask layer;
    public float lerpSpeed = 4;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");





        transform.Rotate(transform.up, hor * rotationSpeed * Time.deltaTime, Space.World);
        RaycastHit hit;

            
            Debug.DrawLine(transform.position, transform.position + (-transform.up + transform.forward*2) * 4);
            if (Physics.Raycast(transform.position, -transform.up + transform.forward*2, out hit, 4, layer.value))
            {

                // find forward direction with new myNormal:
                Vector3 myForward = Vector3.Cross(transform.right, hit.normal);
                // align character to the new normal while keeping the forward direction:
                Quaternion targetRot = Quaternion.LookRotation(myForward, hit.normal);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,lerpSpeed* Time.deltaTime);
              
            }
            else if(Physics.Raycast(transform.position+transform.forward, -transform.up - transform.forward * 0.25f, out hit, 4, layer.value))
            {
                //shoot ray backwards for going around edges

                // find forward direction with new myNormal:
                Vector3 myForward = Vector3.Cross(transform.right, hit.normal);
                // align character to the new normal while keeping the forward direction:
                Quaternion targetRot = Quaternion.LookRotation(myForward, hit.normal);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
            }
            if (Physics.Raycast(transform.position, -transform.up , out hit, 5, layer.value))
            { 
                transform.position = Vector3.Lerp(transform.position, hit.point + hit.normal * (distFromGround + amplitudeBreathing * Mathf.Sin(breathSpeed * Time.time)), Time.deltaTime);

            }
            
     
        transform.position += (transform.forward * vert ).normalized * moveSpeed * Time.deltaTime;

        
        //transform.position = new Vector3(transform.position.x, distFromGround , transform.position.z);
        

    }
}
