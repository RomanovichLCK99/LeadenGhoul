using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "My 2D Light", menuName = "Custom Lights/Custom 2D Light")]
public class LightSettings : ScriptableObject
{
	public Vector3 originalScale;
	public float speed;


	[Space(10)]
	[Header("Light customization")]
	[Space(10)]

	[Tooltip("The color of your light.")]
	public Color[] colorsToUse;

	[Tooltip("The min intensity of the light.")]
	[Range(0.0001f, 1.0f)]
	public float minTransparency = 0.01f;

	[Tooltip("The max intensity of the light.")]
	[Range(0.0f, 1.0f)]
	public float maxTransparency = 1;

	[Range(0f, 5.0f)]
	[Tooltip("The minimal range of the light (radius).")]
	public float minScaleMultiplier = 0.0001f;

	[Range(0f, 5.0f)]
	[Tooltip("The maximum range of the light (radius).")]
	public float maxScaleMultiplier = 1.0f;

	private FlickeringLight flickeringLight;

	public void Initialize(GameObject obj)
	{
		flickeringLight = obj.GetComponent<FlickeringLight>();

		flickeringLight.maxScaleMultiplier = maxScaleMultiplier;
		flickeringLight.minScaleMultiplier = minScaleMultiplier;

		flickeringLight.colorsToUse = colorsToUse;

		flickeringLight.maxTransparency = maxTransparency;
		flickeringLight.minTransparency = minTransparency;

		flickeringLight.speed = speed;
		flickeringLight.originalScale = originalScale;
	}
}
