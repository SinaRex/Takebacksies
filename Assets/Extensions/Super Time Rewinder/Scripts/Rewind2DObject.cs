//
//  Rewind2DObject.cs
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
	/// Manage rewind system for 2D objects, record or play orientation and position each FixedUpdate
	/// </summary>
	public class Rewind2DObject : RewindObject {

		#region Manager

		SpriteRenderer m_sprite_renderer;


		// initialisation of properties
		void Start () {

			// if apply childs is checked, we copy at runtime the "RewindObject" component in all childs 
			if (m_applyChilds) {
				Transform[] obs = this.GetComponentsInChildren<Transform> ();
				for (int i = 0; i < obs.Length; i++) {
					if (obs [i].gameObject.GetComponent<Rewind2DObject> () == null)
						obs [i].gameObject.AddComponent<Rewind2DObject> ();

					Rewind2DObject rewinder = obs [i].GetComponent<Rewind2DObject> ();
					rewinder.m_recordTime = this.m_recordTime;
				}
			}

			m_pointsInTime = new List<TimeKeyPoint>();
			m_sprite_renderer = GetComponent<SpriteRenderer>();

			m_animator = this.GetComponent<Animator> ();
			if (m_animator != null) 
				m_animator_activated = m_animator.enabled;

			m_currentKeyPoint = new TimeKeyPoint2D(transform.localPosition, transform.localRotation, transform.localScale);
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

			//2D sprite animation
			if (m_sprite_renderer != null)
				m_sprite_renderer.sprite = ((TimeKeyPoint2D)m_currentKeyPoint).sprite;
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

			// insert new point in the list
			if (m_sprite_renderer == null)
				m_pointsInTime.Insert(0, new TimeKeyPoint2D(m_currentKeyPoint.position, m_currentKeyPoint.rotation, m_currentKeyPoint.scale));
			else
				m_pointsInTime.Insert(0, new TimeKeyPoint2D(m_currentKeyPoint.position, m_currentKeyPoint.rotation, m_currentKeyPoint.scale, m_sprite_renderer.sprite));
		}


		#endregion //Manager	
	}
}
