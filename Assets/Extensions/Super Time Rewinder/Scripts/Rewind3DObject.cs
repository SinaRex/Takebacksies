//
//  Rewind3DObject.cs
//
//  Author:
//       Lunar Cats Studio <lunar.cats.studio@gmail.com>
//
//  Copyright (c) 2018 Lunar Cats Studio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LunarCatsStudio.SuperRewinder {

	/// <summary>
	/// Manage rewind system for 3D objects, record or play orientation and position each FixedUpdate
	/// </summary>
	public class Rewind3DObject : RewindObject {

		#region Settings
		[Tooltip ("Keep Velocity after end of rewind")]
		public bool m_keepVelocity = true;
		#endregion //Settings

		#region APIs
		/// <summary>
		/// Starts the rewind.
		/// </summary>
		public override void StartRewind ()
		{
			base.StartRewind ();

			// Apply kinematic for smoothest rewind
			if(m_rigidBody != null)
				m_rigidBody.isKinematic = true;
		}


		/// <summary>
		/// Stops the rewind.
		/// </summary>
		public override void StopRewind ()
		{
			base.StopRewind ();

			if (m_rigidBody != null)
			{
				m_rigidBody.isKinematic = false;

				// Apply velocity of the current key point
				if(m_keepVelocity == true)
					m_rigidBody.velocity = ((TimeKeyPoint3D)m_currentKeyPoint).velocity;
			}
		}
			
		#endregion //APIs


		#region Manager
		Rigidbody m_rigidBody;


		// initialisation of properties
		void Start () {

			// if apply childs is checked, we copy at runtime the "Rewind3DObject" component in all childs 
			if (m_applyChilds) {
				Transform[] obs = this.GetComponentsInChildren<Transform> ();
				for (int i = 0; i < obs.Length; i++) {
					if (obs [i].gameObject.GetComponent<Rewind3DObject> () == null)
						obs [i].gameObject.AddComponent<Rewind3DObject> ();

					Rewind3DObject rewinder = obs [i].GetComponent<Rewind3DObject> ();
					rewinder.m_keepVelocity = this.m_keepVelocity;
					rewinder.m_recordTime = this.m_recordTime;
				}
			}

			m_pointsInTime = new List<TimeKeyPoint>();
			m_rigidBody = GetComponent<Rigidbody>();

			m_animator = this.GetComponent<Animator> ();
			if (m_animator != null) 
				m_animator_activated = m_animator.enabled;

			m_currentKeyPoint = new TimeKeyPoint3D(transform.localPosition, transform.localRotation, transform.localScale);
		}


		/// <summary>
		/// Load and apply next key point.
		/// </summary>
		protected override void LoadKeyPoint ()
		{
			//transform
			transform.localPosition = m_currentKeyPoint.position;
			transform.localRotation = m_currentKeyPoint.rotation;
			transform.localScale = m_currentKeyPoint.scale;

			//rigidbody
			if (m_rigidBody != null)
				m_rigidBody.velocity = ((TimeKeyPoint3D)m_currentKeyPoint).velocity;
		}

		/// <summary>
		/// Save and apply next key point.
		/// </summary>
		protected override void SaveKeyPoint ()
		{
			//transform
			m_currentKeyPoint.position = transform.localPosition;
			m_currentKeyPoint.rotation = transform.localRotation;
			m_currentKeyPoint.scale = transform.localScale;

			// rigidbody
			if (m_rigidBody == null)
				((TimeKeyPoint3D)m_currentKeyPoint).velocity = new Vector3 (0, 0, 0);
			else
				((TimeKeyPoint3D)m_currentKeyPoint).velocity = m_rigidBody.velocity;

			m_pointsInTime.Insert(0, new TimeKeyPoint3D(m_currentKeyPoint.position, m_currentKeyPoint.rotation, m_currentKeyPoint.scale, ((TimeKeyPoint3D)m_currentKeyPoint).velocity));
		}

		#endregion //Manager	
	}
}
