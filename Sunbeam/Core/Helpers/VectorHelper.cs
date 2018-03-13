using Plukit.Base;
using System;

namespace Sunbeam.Core.Helpers
{
	public class VectorHelper
	{
		/// <summary>
		/// Rotate a given Vector3D to a rotation direction
		/// </summary>
		/// <param name="rotation"></param>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Vector3D RotatePosition(uint rotation, Vector3D vec)
		{
			switch (rotation)
			{
				case 0u:
					return vec;
				case 1u:
					return new Vector3D(vec.Z, vec.Y, 0.0 - vec.X);
				case 2u:
					return new Vector3D(0.0 - vec.X, vec.Y, 0.0 - vec.Z);
				case 3u:
					return new Vector3D(0.0 - vec.Z, vec.Y, vec.X);
				default:
					throw new Exception();
			}
		}

		/// <summary>
		/// Get the rotation in radians for a rotation direction
		/// </summary>
		/// <param name="rotation"></param>
		/// <returns></returns>
		public static float GetRotationInRadians(uint rotation)
		{
			return (float)(1.5707963267948966 * (double)rotation);
		}
	}
}
