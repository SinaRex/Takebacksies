//
//  TimeKeyPoint2D.cs
//
//  Author:
//       Lunar Cats Studio <lunar.cats.studio@gmail.com>
//
//  Copyright (c) 2018 Lunar Cats Studio


using UnityEngine;


namespace LunarCatsStudio.SuperRewinder {

	/// <summary>
	/// structur for store 2D object's information at in point in time (like: transform and current sprite for animations)
	/// </summary>
	public class TimeKeyPoint2D: TimeKeyPoint {

		// sprite renderer
		private Sprite m_sprite;


		/// <summary>
		/// Gets or sets the the current sprite used in animation.
		/// </summary>
		public Sprite sprite
		{
			get{ return m_sprite; }
			set{ m_sprite = value; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="LunarCatsStudio.SuperRewinder.TimeKeyPoint"/> class.
		/// </summary>
		/// <param name="_position">position.</param>
		/// <param name="_rotation">rotation.</param>
		/// <param name="_scale">scale.</param>
		public TimeKeyPoint2D (Vector3 _position, Quaternion _rotation, Vector3 _scale): 
		base (_position, _rotation, _scale)
		{
			m_sprite = null;
		}

	
		/// <summary>
		/// Initializes a new instance of the <see cref="LunarCatsStudio.SuperRewinder.TimeKeyPoint"/> class.
		/// </summary>
		/// <param name="_position">position.</param>
		/// <param name="_rotation">rotation.</param>
		/// <param name="_scale">scale.</param>
		/// <param name="_sprite">sprite.</param>
		public TimeKeyPoint2D (Vector3 _position, Quaternion _rotation, Vector3 _scale, Sprite _sprite):
		base (_position, _rotation, _scale)
		{
			m_sprite = _sprite;
		}
			
	}
}
