using UnityEngine;
using System.Collections;

public class ZoomCamera : MonoBehaviour {
	
	private float startTouchMagnitude;
	private float startTouchZoom;
	private float targetZoom;
	public float minZoom = 50f;
	public float maxZoom = 100f;
	public float zoomSpeed = 0f;
	
	void Update() {
		if(Input.touchCount == 2) {
			if(Input.touches[1].phase == TouchPhase.Began) {
				startTouchMagnitude = (Input.touches[0].position-Input.touches[1].position).magnitude;
				startTouchZoom = Camera.main.orthographicSize;
			}
			float relativeMagnitudeChange = startTouchMagnitude / (Input.touches[0].position-Input.touches[1].position).magnitude;
			targetZoom = startTouchZoom * relativeMagnitudeChange;
		}
		targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
		Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, zoomSpeed);
	}
}
