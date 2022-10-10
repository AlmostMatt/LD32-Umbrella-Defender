using System;
using UnityEngine;
public enum State {ANIMATION, STUNNED, SWINGING, SWUNG_RECENTLY, INVULNERABLE};

public class Status
{
	// enumerate types

	public State type;
	public float duration;

	public Status (State statusType)
	{
		type = statusType;
		duration = 0;
	}

	public virtual void begin(Unit owner) {
		switch (type) {
		case State.ANIMATION:
			owner.canTurn = false;
			break;
		case State.SWUNG_RECENTLY:
			owner.transform.FindChild(Player.UMBRELLA_PATH).gameObject.layer = 12; // UmbrellaClosed
			//Transform t = owner.transform.FindChild(Player.TRAIL_PATH);
			//t.GetComponent<TrailRenderer>().time = 0.5f;
			break;
		}
	}

	public virtual void expire(Unit owner) {
		switch (type) {
		case State.ANIMATION:
			owner.canTurn = true;
			break;
		case State.SWUNG_RECENTLY:
			owner.transform.FindChild(Player.UMBRELLA_PATH).gameObject.layer = 9; // UmbrellaClosed
			owner.GetComponent<Animator>().SetBool("closed", false);
			//Transform t = owner.transform.FindChild(Player.TRAIL_PATH);
			//t.GetComponent<TrailRenderer>().time = 0f;
			break;
		}
	}
}