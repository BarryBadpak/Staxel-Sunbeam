﻿using Plukit.Base;
using Staxel.Draw;

namespace Sunbeam.Staxel.Rendering.BitmapFont
{
	internal struct BmFontDrawCall
	{
		public readonly TextureVertexDrawable Drawable;
		public readonly Vector3D Offset;
		public readonly Vector3D Location;
		public readonly uint Rotation;

		public BmFontDrawCall(TextureVertexDrawable drawable, Vector3D location, Vector3D offset, uint rotation)
		{
			this.Drawable = drawable;
			this.Offset = offset;
			this.Location = location;
			this.Rotation = rotation;
		}
	}
}
