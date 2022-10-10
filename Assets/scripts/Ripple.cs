using UnityEngine;
using System.Collections;

public class Ripple : MonoBehaviour {
	public const int NUM_POINTS = 32;
	public bool expired = false;

	private Vector3 point;
	private float delay;
	private float duration;
	float timePassed;

	private LineRenderer lineRenderer;
	private float initRad;

	public void init(Vector2 pt, float initR, float dly) {
		point = pt;
		expired = false;

		// for ripples
		lineRenderer = GetComponent<LineRenderer>();
		//lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetWidth(0.03F, 0.03F);
		lineRenderer.SetVertexCount(Ripple.NUM_POINTS + 1); // loop

		//Material/ whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
		//Material mat = new Material(Shader.Find("Particles/Additive"));
		Material mat = new Material(Shader.Find("Sprites/Default"));
		lineRenderer.material = mat;
		lineRenderer.enabled = false;

		initRad = initR;
		timePassed = 0f;
		duration = 1f;
		delay = dly;
	}

	void Update() {
		if (delay > 0f) {
			delay -= Time.deltaTime;
		} else {
			lineRenderer.enabled = true;
			timePassed += Time.deltaTime;
			duration = Mathf.Max (0f, duration - Time.deltaTime);
			if (duration == 0f) {
				expired = true;
				lineRenderer.enabled = false;
			} else {
				render ();
			}
		}
	}

	public void render() {
		//	Drawing.DrawLine(new Vector2(0,0), new Vector2(10,10), Color.black, 1f);
		Color col = new Color(1f, 1f, 1f, 0.1f + duration/4f);
		//Debug.Log (duration);
		lineRenderer.SetColors(col, col);

		float lastcos = 1f;
		float lastsin = 0f;
		float segmentcos = Mathf.Cos (2 * Mathf.PI / NUM_POINTS);
		float segmentsin = Mathf.Sin (2 * Mathf.PI / NUM_POINTS);
		float radius = initRad + timePassed * initRad * 2;//1.6f + 1.5f * Mathf.Sin(timePassed);
		for (int i=0; i <= NUM_POINTS; i++) {
			float newcos = lastcos * segmentcos - lastsin * segmentsin;
			float newsin = lastcos * segmentsin + lastsin * segmentcos;
			//float angle = (2f * Mathf.PI * i) / NUM_POINTS;
			//lineRenderer.SetPosition(i,
			///                         point + new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 2));
			lineRenderer.SetPosition(i, point + new Vector3(radius * newcos, radius * newsin, 2));
			lastcos = newcos;
			lastsin = newsin;
		}
	}
}
