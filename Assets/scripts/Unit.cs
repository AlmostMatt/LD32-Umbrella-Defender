using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : Obj2D {
	protected int ATTACK = 0;

	protected int maxHealth = 10;
	protected int health;

	private float damagedRecently = 0f;

	protected StatusMap statusMap;
	protected ActionMap actionMap;

	public bool dead = false;
	public bool justDied = false;
	public Game scene;

	private int frame = 0;

	/*
	 * Unity Events
	 */

	void Awake() {
		statusMap = new StatusMap(this);
		actionMap = new ActionMap(this);
	}

	// Use this for initialization
	override public void Start () {
		base.Start();
		health = maxHealth;
	}
	
	// Update is called once per frame
	override public void Update () {
		base.Update();
	}

	override public void FixedUpdate () {
		if (statusMap.has(State.ANIMATION)) {
		}
		statusMap.update(Time.fixedDeltaTime);
		actionMap.update(Time.fixedDeltaTime);
		base.FixedUpdate();

		++frame;
		if (frame % 4 == 0) {
			scene.spawnRipple(transform.position, 0.25f);
		}
	}

	// called after (fixed)update => after objects move
	void LateUpdate() {
		damagedRecently = Mathf.Max(0f, damagedRecently - 2 * Time.deltaTime);
		//SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		Renderer r = GetComponent<Renderer>();
		r.material.color = new Color(1f, 1f-damagedRecently, 1f-damagedRecently);
		justDied = false;
	}

	/*
	 * Unit actions
	 */

	public void cast(int action, object target) {
		actionMap.use(action, target);
		statusMap.add(new Status(State.ANIMATION), actionMap.getCastTime(action));
	}

	public virtual void damage(int amount) {
		if (!statusMap.has (State.INVULNERABLE)) {
			statusMap.add(new Status(State.INVULNERABLE), 0.2f);
			damagedRecently = 1f;
			if (health > 0 && health <= amount) {
				dead = true;
				health = 0;
				justDied = true;
			} else {
				health -= amount;
			}
		}
	}
	
	/*
	 * State logic
	 */
	
	protected bool canMove() {
		return !dead && !statusMap.has(State.ANIMATION)
			&& !statusMap.has(State.STUNNED);
	}
	
	protected bool canAttack() {
		return !dead && actionMap.ready(ATTACK)
			&& !statusMap.has(State.ANIMATION)
				&& !statusMap.has(State.STUNNED);
	}
}
