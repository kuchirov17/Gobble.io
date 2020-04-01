using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Camera camera;
	private Vector3 defaultPos;
	private Vector3 step;

	public Transform target;
	private Vector3 startOffset;

	private void Awake()
	{
		camera = GetComponent<Camera>();
		GameManager.gm.cam = this;
	}

	private void Start()
	{
		defaultPos = transform.localPosition;
		step = defaultPos / GameManager.gm.pc.scale;

		startOffset = transform.position - target.position;
	}

	private void Update()
	{
	}

	public void ZoomOut ()
	{
		StartCoroutine(AnimateZoomOut());
	}

	private IEnumerator AnimateZoomOut()
	{
		Vector3 startPos = transform.localPosition;
		Vector3 targetPos = step * GameManager.gm.pc.scale;

		var t = 0f;
		while (t < .2f)
		{
			transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
			t += Time.deltaTime;
			yield return null;
		}
	}
}