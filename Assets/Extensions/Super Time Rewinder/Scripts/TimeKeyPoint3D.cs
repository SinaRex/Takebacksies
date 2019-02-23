//
//  TimeKeyPoint3D.cs
//
//  Author:
//       Lunar Cats Studio <lunar.cats.studio@gmail.com>
//
//  Copyright (c) 2018 Lunar Cats Studio


using UnityEngine;


namespace LunarCatsStudio.SuperRewinder {

	/// <summary>
	/// structur for store 3D object's information at in point in time (like: position, rotation and velocity)
	/// </summary>
	public class TimeKeyPoint3D: TimeKeyPoint {

		// RigidBody
		private Vector3 m_velocity;

		/// <summary>
		/// Gets or sets the rigidbody velocity.
		/// </summary>
		public Vector3 velocity
		{
			get{ return m_velocity; }
			set{ m_velocity = value; }
		}

	
		/// <summary>
		/// Initializes a new instance of the <see cref="LunarCatsStudio.SuperRewinder.TimeKeyPoint"/> class.
		/// </summary>
		/// <param name="_position">position.</param>
		/// <param name="_rotation">rotation.</param>
		/// <param name="_scale">scale.</param>
		public TimeKeyPoint3D (Vector3 _position, Quaternion _rotation, Vector3 _scale):
		base (_position, _rotation, _scale)
		{
			m_velocity = new Vector3 (0, 0, 0);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LunarCatsStudio.SuperRewinder.TimeKeyPoint"/> class.
		/// </summary>
		/// <param name="_position">position.</param>
		/// <param name="_rotation">rotation.</param>
		/// <param name="_scale">scale.</param>
		/// <param name="_velocity">velocity.</param>
		public TimeKeyPoint3D (Vector3 _position, Quaternion _rotation, Vector3 _scale, Vector3 _velocity):
		base (_position, _rotation, _scale)
		{
			m_velocity = _velocity;
		}
			
	}
}
