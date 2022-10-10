using UnityEngine;
using System.Collections;

public class Player : Unit {
	public const string UMBRELLA_PATH = "shoulder/hand/umbrellaHead/umbrella";
	public const string TRAIL_PATH = "shoulder/hand/umbrellaHead/trail";
	private Animator animator;
	
	private AudioSource source;
	public AudioClip[] swingSounds;
	public AudioClip[] damagedSounds;
	public AudioClip[] harmlessSounds;
	public AudioClip[] deathSounds;

	private UnityEngine.UI.Slider healthBar;
	// Use this for initialization
	override public void Start () {
		maxHealth = 30;
		base.Start();
		actionMap.add(ATTACK, new Ability(0.15f));

		animator = GetComponent<Animator>();
		source = GetComponent<AudioSource>();

		MAX_V = 5;
		ACCEL = 30;
		healthBar = GameObject.Find("Canvas/healthBar").GetComponent<UnityEngine.UI.Slider>();
	}
	
	override public void FixedUpdate() {
		float dx = Input.GetAxis("Horizontal");
		float dy = Input.GetAxis("Vertical");
		Vector2 desiredV = new Vector2(dx, dy);
		if (desiredV.sqrMagnitude > 1) {
			desiredV.Normalize();
		}
		accelTo(MAX_V * desiredV);

		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Vector2 offset = ray.origin - transform.position;
		float angle = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x);
		//Transform shoulder = transform.FindChild("shoulder");
		//Obj2D.turnToward(shoulder, angle - transform.localEulerAngles.z, 700);
		Obj2D.turnToward(transform, angle, TURN_RATE);

		// exit time and triggers are dumb
		int attackState = Animator.StringToHash("swing");
		if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == attackState) {
			animator.ResetTrigger("swing");
		}

		if (Input.GetAxis("Fire1") >= 0.5f && canAttack()) {
			animator.SetBool("closed", true);
			cast (ATTACK, null);
			animator.SetTrigger("swing");
			statusMap.add(new Status(State.SWINGING), 0.15f);
			statusMap.add(new Status(State.SWUNG_RECENTLY), 0.4f);
		}

		base.FixedUpdate();
	}

	override public void Update() {
		base.Update();

		if (dead) {
			float s = transform.localScale.x;
			s = Mathf.Max(0.01f, s - Time.deltaTime * 0.05f);
			transform.localScale = new Vector3(s,s,s);
		}
	}
	
	private void playSound(AudioClip[] soundList) {
		if (soundList.Length > 0) {
			int clip_index = Random.Range(0, soundList.Length);
			//float vol = Random.Range(0.5f, 1f);
			AudioClip sound = soundList[clip_index];//[sound_index];
			if (dead) {
				AudioSource.PlayClipAtPoint(sound, transform.position, 1f);
			} else {
				source.PlayOneShot(sound, 1f);
			}
		}
	}
	
	override public void damage(int amount) {
		base.damage(amount);
		if (justDied) {
			playSound(deathSounds);
			GetComponent<Animator>().SetBool("dead", true);
			StartCoroutine(deathWait());
		} else {
			playSound(damagedSounds);
		}
		healthBar.value = ((float) health) / maxHealth;
	}

	public void hitUmbrella(Vector3 point) {
		playSound(harmlessSounds);
	}

	public bool isAttacking() {
		// used when umbrella hits an enemy
		return statusMap.has (State.SWINGING);
	}
	
	IEnumerator deathWait() {
		yield return new WaitForSeconds(5f);
		Application.LoadLevel("menu");
	}
}
