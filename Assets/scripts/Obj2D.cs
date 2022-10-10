using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Obj2D : MonoBehaviour
{
	public bool canTurn = true;

	public float MAX_V = 5f;
	protected float TURN_RATE = 700f;
	public float ACCEL = 20f;

	private static float dt;
	protected Rigidbody2D rb;

	/*
	 * Utility functions
	 */

	private Vector2 scaled(float n, Vector2 v) {
		return n * v.normalized;
	}
	
	protected static float angleDiff(float a1, float a2) {
		float diff = a2 - a1;
		// (diff + 180 mod 360) - 180
		while (diff > 180) {
			diff -= 360;
		}
		while (diff < -180) {
			diff += 360;
		}
		return diff;
	}
	
	private bool sameDir(Vector2 v1, Vector2 v2) {
		return Vector2.Dot(v1, v2) >= 0f;
	}

	public void moveToward(Vector2 point) {
		Vector2 diff = point - (Vector2) transform.position;
		accelTo(scaled(MAX_V, diff));
	}
	
	public static void turnToward(Transform tform, float angle2, float turn_rate) {
		float angle1 = tform.localEulerAngles.z;
		float diff = angleDiff(angle1, angle2);
		float rot = turn_rate * Time.fixedDeltaTime;
		if (diff > rot) {
			tform.localEulerAngles = new Vector3(0, 0, angle1 + rot);
		} else if (diff < -rot) {
			tform.localEulerAngles = new Vector3(0, 0, angle1 - rot);
		} else {
			tform.localEulerAngles = new Vector3(0, 0, angle2);
		}
	}

	public void accelTo(Vector2 desiredV) {
		Vector2 diffV = desiredV - (Vector2) rb.velocity;
		if (diffV.sqrMagnitude > ACCEL * ACCEL * dt * dt) {
			accel(scaled(ACCEL, diffV));
		} else {
			accel(diffV);
		}
	}

	public void accel(Vector2 force) {
		rb.AddForce(rb.mass * force);
	}

	protected void faceVelocity() {
		float vv = rb.velocity.sqrMagnitude;
		if (canTurn && vv > 0.3) {
			float angle = Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.y, rb.velocity.x);
			Obj2D.turnToward(transform, angle, TURN_RATE);
		}
	}
	

	/*
	 * Unity Events
	 */

	public virtual void Start() {
		rb = GetComponent<Rigidbody2D>();
		dt = Time.fixedDeltaTime;
	}
	
	public virtual void Update() {
	}

	public virtual void FixedUpdate () {
		// this should happen after actions are performed
		float vv = rb.velocity.sqrMagnitude;
		if (vv > MAX_V * MAX_V) 	{
			rb.velocity = MAX_V * rb.velocity.normalized;
		}
		//if (canTurn && vv > 0.5) {
		//	float angle = Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.y, rb.velocity.x);
		//	Obj2D.turnToward(transform, angle, TURN_RATE);
		//}
	}
}