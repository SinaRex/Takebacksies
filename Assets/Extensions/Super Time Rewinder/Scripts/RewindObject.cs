//
//  RewindObject.cs
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
	/// Manage rewind system, record or play orientation and position each FixedUpdate
	/// </summary>
	abstract public class RewindObject : MonoBehaviour {

	#region Settings
		[Tooltip ("Apply rewind effect on childs")]
		public bool m_applyChilds = false;

		[Header ("Record Settings")]
		[Tooltip ("Buffer time for the record in seconds")]
		public float m_recordTime  = 5f;
	#endregion //Settings

	#region APIs
		/// <summary>
		/// Starts the rewind.
		/// </summary>
		public virtual void StartRewind ()
		{
			m_isRewinding = true;

			if (m_animator != null) {
				m_animator_activated = m_animator.enabled;
				m_animator.enabled = false;
			}
		}


		/// <summary>
		/// Stops the rewind.
		/// </summary>
		public virtual void StopRewind ()
		{
			if (m_animator != null) {
				m_animator.enabled = m_animator_activated;
			}

			m_isRewinding = false;
		}


		/// <summary>
		/// Clears al recorded key points.
		/// </summary>
		public void ClearRewindDatas ()
		{
			m_pointsInTime.Clear ();
		}
	#endregion //APIs


	#region Manager

		//private properties
		protected bool m_isRewinding = false;
		protected List<TimeKeyPoint> m_pointsInTime;
		protected TimeKeyPoint m_currentKeyPoint;

		protected Animator m_animator;
		protected bool m_animator_activated;

				 
		/// <summary>
		/// Check and apply current action.
		/// </summary>
		void FixedUpdate ()
		{
			if (m_isRewinding)
				Rewind();
			else
				Record();
		}



		/// <summary>
		/// Rewind process.
		/// </summary>
		void Rewind ()
		{
			// we have points in the list for rewind
			if (m_pointsInTime.Count > 0)
			{
				m_currentKeyPoint = m_pointsInTime[0]; // extract next point

				LoadKeyPoint ();

				m_pointsInTime.RemoveAt(0); // delete used point
			} 
		}
			

		/// <summary>
		/// Record process.
		/// </summary>
		void Record ()
		{
			// check overflow of points list
			if (m_pointsInTime.Count > Mathf.Round(m_recordTime / Time.fixedDeltaTime))
				m_pointsInTime.RemoveAt(m_pointsInTime.Count - 1);

			SaveKeyPoint();
		}


		/// <summary>
		/// Load and apply next key point.
		/// </summary>
		protected virtual void LoadKeyPoint ()
		{
			//transform
			transform.localPosition = m_currentKeyPoint.position;
			transform.localRotation = m_currentKeyPoint.rotation;
			transform.localScale = m_currentKeyPoint.scale;
		}

		/// <summary>
		/// Save and apply next key point.
		/// </summary>
		protected virtual void SaveKeyPoint ()
		{
			//transform
			m_currentKeyPoint.position = transform.localPosition;
			m_currentKeyPoint.rotation = transform.localRotation;
			m_currentKeyPoint.scale = transform.localScale;

			// insert new point in the list
			m_pointsInTime.Insert(0, new TimeKeyPoint(m_currentKeyPoint.position, m_currentKeyPoint.rotation, m_currentKeyPoint.scale));
		}

	#endregion //Manager	
	}
}
