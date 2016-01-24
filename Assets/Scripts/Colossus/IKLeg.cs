using UnityEngine;
using System.Collections;

public class IKLeg : MonoBehaviour
{
	public GameObject UpperLeg;
	public GameObject LowerLeg;
		
	public Vector3 TargetPosition;
	private float maxDist;

	public LayerMask layer;
	public GameObject target;
	public float elbowAngle = 0;
	public bool isLeftLeg = false;
	Quaternion oldRot;
	Quaternion oldRotLower;
	public float smoothSpeed = 2;
	public float maxStepAngle = 95;
	private float stepAngle;
	float t = 0;
	// Use this for initialization
	void Start ()
	{

        maxDist = UpperLeg.transform.lossyScale.z + LowerLeg.transform.lossyScale.z;
        Debug.Log("Arm size: " + maxDist);
		PickNewTargetPosition ();
        if (target != null)
        {
            target.transform.position = TargetPosition;
        }
				

	}
	
	// Update is called once per frame
	void Update ()
	{

		float distToTarget = Vector3.Distance (transform.position, TargetPosition); 
		float angle = Vector3.Angle (transform.forward, (new Vector3 (TargetPosition.x, transform.position.y, TargetPosition.z) - transform.position).normalized);

		stepAngle = maxStepAngle;
		if (distToTarget < maxDist && angle < stepAngle) {
			UpdateAngles ();
			LerpToNewPosition ();
		} else {
			PickNewTargetPosition ();
            
		}


	}

		
	void PickNewTargetPosition ()
	{
		Debug.Log ("Picking new Target!");

		Vector3 sideDir = isLeftLeg ? -(transform.right) : (transform.right);

		RaycastHit hit;


        
		if (Physics.Raycast (transform.position, (transform.forward * 2 + sideDir - transform.up) / 2f, out hit, maxDist, layer.value)) {
			TargetPosition = hit.point;
			target.transform.position = TargetPosition;
		}
		oldRot = UpperLeg.transform.rotation;
		oldRotLower = LowerLeg.transform.rotation;
		t = 0; 
	}

	void UpdateAngles ()
	{

		float d = Vector3.Distance (transform.position, TargetPosition);
		//We cannot reach the position if d exceeds the maximumDistance
		if (d > maxDist) {
            Debug.Log("dist is larger than targetPosition");
			return;
		}

		//this calculation is when two circles are at the same y-position
		float UpperLength = UpperLeg.transform.lossyScale.z;
        float LowerLength = LowerLeg.transform.lossyScale.z;
        
		float x = (Sqr (d) - Sqr (UpperLength) + Sqr (LowerLength)) / (2 * d);

		UpperLeg.transform.LookAt (TargetPosition);

		//calculate angle (intersection between two circles
		float angle = -Mathf.Acos (x / UpperLength) * Mathf.Rad2Deg;
		UpperLeg.transform.position = transform.position;
		UpperLeg.transform.Rotate(transform.right, angle);

		//we rotate the vector with the angle between the current position and the targetPosition (for custom elbowAngle)
		UpperLeg.transform.Rotate ((TargetPosition - transform.position).normalized, elbowAngle,Space.World);

		//We place the Lowerleg at the end of the Upperleg and pointing toward the target position
		LowerLeg.transform.position = UpperLeg.transform.position + UpperLeg.transform.forward * UpperLength; 
		LowerLeg.transform.LookAt (TargetPosition);


	}

	void LerpToNewPosition ()
	{
		t += Time.deltaTime * smoothSpeed;
		t = Mathf.Clamp01 (t);
		UpperLeg.transform.rotation = Quaternion.Slerp (oldRot, UpperLeg.transform.rotation, t);
		//We place the Lowerleg at the end of the Upperleg and pointing toward the target position
		LowerLeg.transform.position = UpperLeg.transform.position + UpperLeg.transform.forward * UpperLeg.transform.lossyScale.z;
		LowerLeg.transform.rotation = Quaternion.Slerp (oldRotLower, LowerLeg.transform.rotation, t);

	}

	public float Sqr (float a)
	{
		return a * a;
	}
		
}
