using UnityEngine;
using System.Collections;

public enum EType {RUSH=0, RANGE=1, CLUSTER=2};

public class Enemy : Unit {
	public Player player;
	public GameObject bulletObject;

	public Sprite[] images;

	private AudioSource source;
	public AudioClip[] spawnSounds;
	public AudioClip[] attackSounds;
	public AudioClip[] hitSounds;
	public AudioClip[] deathSounds;

	private float range;
	private float tooClose = 0f;
	private EType type;
	private int int_type;

	// Use this for initialization
	override public void Start () {
		base.Start();
		health = maxHealth;
		source = GetComponent<AudioSource>();
		// is typed?
		playSound(spawnSounds);
	}

	public void init(EType t) {
		type = t;
		int_type = (int) t;
		switch (t) {
		case EType.RUSH:
			actionMap.add(ATTACK, new Ability(0.5f));
			statusMap.add(new Status(State.STUNNED), 0.8f);
			MAX_V = 3.5f;
			maxHealth = 2;
			range = 0.5f;
			break;
		case EType.RANGE:
			actionMap.add(ATTACK, new Ability(0.8f));
			statusMap.add(new Status(State.STUNNED), 0.8f);
			MAX_V = 2f;
			range = 6f;
			tooClose = 3f;
			maxHealth = 2;
			break;
		case EType.CLUSTER:
			actionMap.add(ATTACK, new Ability(1.5f));
			statusMap.add(new Status(State.STUNNED), 1.4f);
			MAX_V = 0.5f;
			range = 7f;
			tooClose = 5f;
			maxHealth = 1;
			break;
		}
		GetComponent<SpriteRenderer>().sprite = images[(int) t];

	}
	
	override public void FixedUpdate() {
		Vector2 offset = (Vector2) (player.transform.position - transform.position);
		if (canMove() && offset.sqrMagnitude > range * range) {
			accelTo(MAX_V * offset);
			faceVelocity();
		} else if (canAttack()) {
			float angle2 = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x);
			float angle1 = transform.localEulerAngles.z;
			float adiff = angleDiff(angle1, angle2);
			if (Mathf.Abs (adiff) > 5f) {
				Obj2D.turnToward(transform, angle2, TURN_RATE);
			} else {
				Bullet bullet;
				Transform gun;
				cast(ATTACK, player);
				playSound(attackSounds);
				switch (type) {
				case EType.RUSH:
					player.damage(1);
					break;
				case EType.RANGE:
					gun = transform.Find("1/gun");
					bullet = fireGun(gun, 7f, 20f);
					bullet.setTarget(player.transform.position);
					break;
				case EType.CLUSTER:
					for (int n=1; n<=4; n++) {
						gun = transform.Find("2/gun" + n);
						bullet = fireGun(gun, 6f, 4f);
						bullet.setTarget(player);
					}
					break;
				}
			}
		} else if (canMove() && offset.sqrMagnitude < tooClose * tooClose) {
			accelTo(- MAX_V * offset);
			faceVelocity();
		}

		//float angle = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x);
		//Obj2D.turnToward(transform, angle, );
		//transform.localEulerAngles = new Vector3(0, 0, angle);
		
		base.FixedUpdate();
	}

	private Bullet fireGun(Transform gun, float velocity, float accel) {
		GameObject obj = Instantiate(bulletObject);
		Bullet bullet = obj.GetComponent<Bullet>();
		bullet.MAX_V = velocity;
		bullet.ACCEL = accel;
		bullet.transform.position = gun.position;
		bullet.transform.eulerAngles = gun.eulerAngles;
		bullet.GetComponent<Rigidbody2D>().velocity = bullet.MAX_V * gun.right;
		bullet.player = player;
		return bullet;
	}

	override public void damage(int amount) {
		base.damage(amount);
		if (justDied) {
			playSound(deathSounds);
			GetComponent<Animator>().SetBool("dead", true);
			StartCoroutine(deathWait());
		} else {
			playSound(hitSounds);
			statusMap.add(new Status(State.STUNNED), 0.4f);
		}
	}

	IEnumerator deathWait() {
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}

	private void playSound(AudioClip[] soundList) {
		if (soundList.Length > int_type) {
			float vol = Random.Range(0.5f, 1f);
			AudioClip sound = soundList[int_type];//[sound_index];
			if (dead) {
				AudioSource.PlayClipAtPoint(sound, transform.position, vol);
			} else {
				source.PlayOneShot(sound, vol);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "umbrella") {
			if (player.isAttacking()) {
				damage(1);
			}
		}
	}
}
