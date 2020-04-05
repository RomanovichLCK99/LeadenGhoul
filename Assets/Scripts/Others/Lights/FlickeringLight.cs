using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class FlickeringLight : MonoBehaviour {

	public LightSettings lightSettings;

	[HideInInspector] public float speed;

	[HideInInspector] public Color[] colorsToUse;

	[HideInInspector] public float minTransparency = 0.01f;
	[HideInInspector] public float maxTransparency  = 1;

	[HideInInspector] public float minScaleMultiplier = 0.0001f;
	[HideInInspector] public float maxScaleMultiplier = 1.0f;

	[HideInInspector] public Vector3 originalScale;


	private Transform myTransform;
	private SpriteRenderer mySpriteRenderer;

	bool hasStartedColorChange = false;
	

	void Start() {
		lightSettings.Initialize(gameObject);

		myTransform = GetComponent<Transform>();
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}

	void FixedUpdate	() {
		if (hasStartedColorChange == false) {
			StartCoroutine(colorChange());
		}
	}

	IEnumerator colorChange() {
		hasStartedColorChange = true;

		float scaleMultiplier = Random.Range(minScaleMultiplier, maxScaleMultiplier);


		Color theColorToUse = colorsToUse[Random.Range(0,colorsToUse.Length)];

		mySpriteRenderer.color = new Color(theColorToUse.r, theColorToUse.g, theColorToUse.b, Random.Range(minTransparency,maxTransparency)  );
		myTransform.localScale = originalScale * scaleMultiplier;

		yield return new WaitForSeconds(speed * Time.deltaTime);
		hasStartedColorChange = false;
	}
	
}