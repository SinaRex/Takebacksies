//
//  TimeKeyPoint.cs
//
//  Author:
//       Lunar Cats Studio <lunar.cats.studio@gmail.com>
//
//  Copyright (c) 2018 Lunar Cats Studio


using UnityEngine;


namespace LunarCatsStudio.SuperRewinder {

	/// <summary>
	/// structur for store object's transform information at in point in time (like: position, rotation and scale)
	/// </summary>
	public class TimeKeyPoint {

		// Transform 
		private Vector3    m_position;
		private Quaternion m_rotation;
		private Vector3    m_scale;


		/// <summary>
		/// Gets or sets the transform scale.
		/// </summary>
		public Vector3 scale
		{
			get{ return m_scale; }
			set{ m_scale = value; }
		}

		/// <summary>
		/// Gets or sets the transform position.
		/// </summary>
		public Vector3 position
		{
			get{ return m_position; }
			set{ m_position = value; }
		}

		/// <summary>
		/// Gets or sets the transform rotation.
		/// </summary>
		public Quaternion rotation
		{
			get{ return m_rotation; }
			set{ m_rotation = value; }
		}
			


		/// <summary>
		/// Initializes a new instance of the <see cref="LunarCatsStudio.SuperRewinder.TimeKeyPoint"/> class.
		/// </summary>
		/// <param name="_position">position.</param>
		/// <param name="_rotation">rotation.</param>
		/// <param name="_scale">scale.</param>
		public TimeKeyPoint (Vector3 _position, Quaternion _rotation, Vector3 _scale)
		{
			m_position = _position;
			m_rotation = _rotation;
			m_scale = _scale;
		}
			
	}
}
