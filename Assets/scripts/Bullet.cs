using UnityEngine;
using System.Collections;

public class Bullet : Obj2D {
	private Unit target;
	public Player player;

	// Use this for initialization
	override public void Start () {
		base.Start();
	}

	// launch at max speed and also home in
	public void setTarget(Unit u) {
		setTarget(u.transform.position);
		target = u;
	}
	
	// intiially launch at max speed
	public void setTarget(Vector2 pt) {
		Vector2 offset = (pt - (Vector2) transform.position);
		float angle = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x);
		transform.localEulerAngles = new Vector3(0,0,angle);
	}

	override public void FixedUpdate() {
		if (target != null) {
			moveToward(target.transform.position);
		}

		base.FixedUpdate();
		
		float vv = rb.velocity.sqrMagnitude;
		if (canTurn && vv > 0.5) {
			float angle = Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.y, rb.velocity.x);
			Obj2D.turnToward(transform, angle, TURN_RATE);
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		GameObject obj = col.gameObject;
		if (obj.name == "char(Clone)") {
			obj.GetComponent<Player>().damage(1);
		} else if (obj.name == "umbrella") {
			player.hitUmbrella(col.transform.position);
		}
		player.scene.spawnRipple(transform.position, 0.1f);
		Destroy(gameObject);
	}
}
