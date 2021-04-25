using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	public Transform camTransform;

	public bool shaking;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if (shaking)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
		}
		else
		{
			camTransform.localPosition = originalPos;
		}
	}

	public void ToggleShake()
	{
		shaking = !shaking;
	}
}