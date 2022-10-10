using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Game : MonoBehaviour {
	public GameObject unitObject;
	public GameObject enemyObject;
	public GameObject rippleObject;
	public GameObject spawnObject;

	private Player player;
	private List<Enemy> enemies;

	private List<Ripple> ripples;
	private Queue<Ripple> ripplePool;
    
	private JSONNode levelData;
	private JSONArray waves;
	private float waveDelay;
	
	private int waveNum = 0;
	private int waveIteration = 0;
	private float delayMult = 1f;

	// Use this for initialization
	void Start () {
		levelData = JSON.Parse(LevelData.data);
		JSONNode playerData = levelData["player"];
		waves = levelData["waves"].AsArray;
		waveNum = levelData["testWave"].AsInt;

		enemies = new List<Enemy>();
		GameObject obj = Instantiate(unitObject);
		player = obj.GetComponent<Player>();
		player.transform.position = new Vector3(playerData["x"].AsInt, playerData["y"].AsInt,-1);
		player.scene = this;

		waveDelay = 0.5f;

		ripples = new List<Ripple>();
		ripplePool = new Queue<Ripple>();
	}

	public void spawnWave() {
		JSONNode wave = waves[waveNum % waves.Count];
		JSONArray waveEnemies = wave["enemies"].AsArray;

		// N wave iterations
		JSONNode waveIter = wave["wave_iter"];
		float w_iter_a = 0f, w_iter_r = 0f, w_iter_x=0f, w_iter_y=0f;
		int w_iter_n = 1;
		if (waveIter != null) {
			w_iter_a = waveIter["a"].AsFloat;
			w_iter_r = waveIter["r"].AsFloat;
			w_iter_x = waveIter["x"].AsFloat;
			w_iter_y = waveIter["y"].AsFloat;
			w_iter_n = waveIter["n"].AsInt;
		}

		// enemy iterations
		JSONNode enemyIter = wave["enemy_iter"];
		float iter_a = 0f, iter_r = 0f, iter_x=0f, iter_y=0f;
		int iter_n = 1;
		if (enemyIter != null) {
			iter_a = enemyIter["a"].AsFloat;
			iter_r = enemyIter["r"].AsFloat;
			iter_x = enemyIter["x"].AsFloat;
			iter_y = enemyIter["y"].AsFloat;
			iter_n = enemyIter["n"].AsInt;
		}
		// spawn units
		for (int i=0; i<iter_n; ++i) {
			foreach (JSONNode enemy in waveEnemies) {
				int type = enemy["type"].AsInt;
				float range = 5f;
				Enemy newUnit = Instantiate(enemyObject).GetComponent<Enemy>();
				Vector3 pos;
				if (enemy["polar"].AsBool) {
					float r = enemy["r"].AsFloat + iter_r * i + w_iter_r * waveIteration;
					float a = enemy["a"].AsFloat + iter_a * i + w_iter_a * waveIteration;
					//normalize
					r = r/5f;
					a = Mathf.Deg2Rad * (90 - a);
					pos = new Vector3(r * Mathf.Cos (a), r * Mathf.Sin (a), 0);
				} else {
					float x = enemy["x"].AsFloat + iter_x * i + w_iter_x * waveIteration;
					float y = enemy["y"].AsFloat + iter_y * i + w_iter_y * waveIteration;
					//normalize
					x = x/5f;
					y = y/5f;
					pos = new Vector3(x, y, 0);
				}
				newUnit.transform.position = pos;
				enemies.Add(newUnit);
				newUnit.scene = this;
				newUnit.player = player;
				newUnit.init ((EType) type);

				Vector3 offset = player.transform.position - newUnit.transform.position;
				float angle2 = Mathf.Rad2Deg * Mathf.Atan2(offset.y, offset.x);
				newUnit.transform.localEulerAngles = new Vector3(0, 0, angle2);
				GameObject spawner = Instantiate(spawnObject);
				spawner.transform.position = newUnit.transform.position;
			}
		}
		waveIteration++;
		if (waveIteration >= w_iter_n) {
			waveIteration = 0;
			waveNum++;
			waveDelay = wave["duration"].AsFloat * delayMult;
		} else {
			waveDelay = waveIter["delay"].AsFloat * delayMult;
		}
		if (waveNum > waves.Count) {
			waveNum = 0;
			delayMult *= 0.8f;
		}
	}

	public void spawnRipple(Vector2 pos, float size) {
		for (int n =0; n< 4; n++) {
			Ripple rip;
			if (ripplePool.Count > 0) {
				rip = ripplePool.Dequeue();
			} else {
				GameObject obj = Instantiate(rippleObject);
				rip = obj.GetComponent<Ripple>();
			}
			rip.init(pos, size, 0.3f * n);
			ripples.Add(rip);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// dead units
		for (int i = enemies.Count - 1; i >= 0; --i) {
			Enemy e = enemies[i];
			if (e.dead) {
				enemies.RemoveAt(i);
			}
		}
		// dead ripples
		for (int i = ripples.Count - 1; i >= 0; --i) {
			Ripple e = ripples[i];
			if (e.expired) {
				ripplePool.Enqueue(e);
				ripples.RemoveAt(i);
			}
		}
		// raindrop
		float range = 5f;
		Vector2 rainPos = new Vector3(Random.Range(-range, range),
		                              Random.Range(-range, range), 0);
		spawnRipple(rainPos, 0.1f);
		waveDelay -= Time.deltaTime;
		if (waveDelay <= 0f || enemies.Count == 0) {
			spawnWave();
		}
	}
    
    void LateUpdate() {
    }
}
