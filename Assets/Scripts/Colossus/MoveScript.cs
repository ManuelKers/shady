using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public enum State { Roaming, Angry }

public class MoveScript : MonoBehaviour {
    public float moveSpeed = 3;
    private float curSpeed;
    public float rotationSpeed = 10;
    public float distFromGround = 0.5f;
    public float amplitudeBreathing = 0.1f;
    public float breathSpeed = 1;
    public LayerMask layer;
    public float lerpSpeed = 4;
    public Transform rayCaster;

    public LayerMask spotLayer;
    public GameObject eyes;
    public float spotRange = 50;
    public State state;

    private Vector3 lastSeenPosition;
    Vector3 targetPosition;
    private float cooldownTime = 0;
    // Use this for initialization
    void Start () {
        state = State.Roaming;
        GetNewPosition();
        curSpeed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 vecToPlayer = (PlayerScript.instance.transform.position - eyes.transform.position);
        if (Vector3.Dot(vecToPlayer.normalized, eyes.transform.forward) > 0f)
        {

            RaycastHit hit;
            if (Physics.Raycast(eyes.transform.position, vecToPlayer.normalized, out hit, spotRange, spotLayer.value))
            {

                if (hit.collider.tag == "Player")
                {
                    state = State.Angry;
                    targetPosition = new Vector3(PlayerScript.instance.transform.position.x, PlayerScript.instance.transform.position.y, PlayerScript.instance.transform.position.z);
                    cooldownTime = 0;

                }
            }


        }

        cooldownTime += Time.deltaTime;

        if (Vector3.Distance(transform.position, PlayerScript.instance.transform.position) < 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        switch (state)
        {


            case State.Roaming:
                if (Vector3.Distance(transform.position, targetPosition) < 5)
                {
                    GetNewPosition();
                }
                if (Vector3.Distance(transform.position, PlayerScript.instance.transform.position) > 20)
                {
                    curSpeed = moveSpeed * 2;
                }
                else
                {
                    curSpeed = moveSpeed;
                }
                    ; break;
            case State.Angry:
                curSpeed = moveSpeed;
                if (cooldownTime > 3)
                {
                    curSpeed = moveSpeed;
                    cooldownTime = 0;
                    state = State.Roaming;
                    GetNewPosition();
                }
                break;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetPosition - new Vector3(transform.position.x,transform.position.y,transform.position.z)),rotationSpeed*Time.deltaTime);
        //if(Vector3.Dot(transform.forward, (targetPosition - new Vector3(transform.position.x,0,transform.position.z)).normalized)<0.9){
        //    transform.Rotate(transform.up,rotationSpeed*Time.deltaTime);
        //}
        RayCastsForOrientation();

        transform.position += (transform.forward).normalized * curSpeed * Time.deltaTime;
        
       
     
        

        

    }

    public void GetNewPosition()
    {

        targetPosition = PlayerScript.instance.transform.position + new Vector3(Random.Range(-30, 30), transform.position.y, Random.Range(-30, 30));
        Debug.Log(targetPosition);

    }

    public void Movement()
    {
        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        transform.Rotate(transform.up, hor * rotationSpeed * Time.deltaTime, Space.World);
        transform.position += (transform.forward * vert).normalized * moveSpeed * Time.deltaTime;
    }

    public void RayCastsForOrientation()
    {
        RaycastHit hit;
        Debug.DrawLine(rayCaster.transform.position, rayCaster.transform.position + (rayCaster.transform.forward) * rayCaster.localScale.x);
        if (Physics.Raycast(rayCaster.transform.position, rayCaster.transform.forward, out hit, rayCaster.localScale.x, layer.value))
        {

            // find forward direction with new myNormal:
            Vector3 myForward = Vector3.Cross(transform.right, hit.normal);
            // align character to the new normal while keeping the forward direction:
            Quaternion targetRot = Quaternion.LookRotation(myForward, hit.normal);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);

        }
        else if (Physics.Raycast(rayCaster.transform.position + transform.forward, -transform.up - transform.forward * 0.25f, out hit, 20, layer.value))
        {
            //shoot ray backwards for going around edges

            // find forward direction with new myNormal:
            Vector3 myForward = Vector3.Cross(transform.right, hit.normal);
            // align character to the new normal while keeping the forward direction:
            Quaternion targetRot = Quaternion.LookRotation(myForward, hit.normal);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
        }
        if (Physics.Raycast(transform.position, -transform.up, out hit, 5, layer.value))
        {
            transform.position = Vector3.Lerp(transform.position, hit.point + hit.normal * (distFromGround + amplitudeBreathing * Mathf.Sin(breathSpeed * Time.time)), Time.deltaTime);

        }
    }
}
