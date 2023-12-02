// From: https://github.com/ewersp/CameraShake

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Attach CameraShakeManager to your primary Camera GameObject.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraShakeManager : Singleton<CameraShakeManager>
{

	[SerializeField]
	CameraShake[] shakes;
    /// <summary>
    /// Internal list of active camera shake components.
    /// </summary>
    private List<CameraShake> m_activeShakes = new List<CameraShake>();

	/// <summary>
	/// Convenience getter for the camera.
	/// </summary>
	private Camera Camera {
		get { return GetComponent<Camera>(); }
	}

	/// <summary>
	/// Unity recommends most camera logic run in late update, to ensure the camera is most up to date this frame.
	/// </summary>
	void LateUpdate() {
		Matrix4x4 shakeMatrix = Matrix4x4.identity;

		// For each active shake
		for (int i = m_activeShakes.Count - 1; i >= 0; i--) {

			// Concatenate its shake matrix
			shakeMatrix *= m_activeShakes[i].ComputeMatrix();

			// If done, remove
			if (m_activeShakes[i].IsDone()) {
				Destroy(m_activeShakes[i].gameObject);
				m_activeShakes.RemoveAt(i);
				Camera.ResetWorldToCameraMatrix();
			}
		}

		// Camera always looks down the negative z-axis
		shakeMatrix *= Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1));

		// Update camera matrix
		if (m_activeShakes.Count > 0) {
			Camera.worldToCameraMatrix = shakeMatrix * transform.worldToLocalMatrix;
		}
	}

	/// <summary>
	/// Start a camera shake.
	/// </summary>
	/// <returns>A reference to the camera shake object.</returns>
	public CameraShake Play(int index) {
		CameraShake cs = Instantiate(shakes[index], transform) as CameraShake;
		if (cs != null) {
			m_activeShakes.Add(cs);
		}
		return cs;
	}

	/// <summary>
	/// Stop a camera shake.
	/// </summary>
	/// <param name="shake">The camera shake to stop.</param>
	/// <param name="immediate">True to stop immediately this frame, false to ramp down.</param>
	public void Stop(CameraShake shake, bool immediate = false) {
		if (shake == null) return;

		shake.Finish(immediate);
	}

	/// <summary>
	/// Stop all active camera shakes.
	/// </summary>
	/// <param name="immediate">True to stop immediately this frame, false to ramp down.</param>
	public void StopAll(bool immediate = false) {
		foreach (var shake in m_activeShakes) {
			Stop(shake, immediate);
		}
        Camera.ResetWorldToCameraMatrix();
    }
}
