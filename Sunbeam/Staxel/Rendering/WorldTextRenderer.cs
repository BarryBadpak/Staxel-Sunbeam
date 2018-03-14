using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plukit.Base;
using Staxel.Draw;
using Sunbeam.Core.Helpers;
using Sunbeam.Staxel.Rendering.BitmapFont;
using System.Collections.Generic;

namespace Sunbeam.Staxel.Rendering
{
	internal struct WorldTextDrawCall
	{
		public readonly TextureVertexDrawable Drawable;
		public readonly Vector3D Offset;
		public readonly Vector3D Location;
		public readonly uint Rotation;

		public WorldTextDrawCall(TextureVertexDrawable drawable, Vector3D location, Vector3D offset, uint rotation)
		{
			this.Drawable = drawable;
			this.Offset = offset;
			this.Location = location;
			this.Rotation = rotation;
		}
	}

	public sealed class WorldTextRenderer: TextRenderer
	{
		protected override float Units => 0.06f / 36f;
		private List<WorldTextDrawCall> _drawCalls = new List<WorldTextDrawCall>();

		/// <summary>
		/// Clear out any pending draw calls
		/// </summary>
		public override void Clear()
		{
			this._drawCalls.Clear();
		}

		/// <summary>
		/// Draw all of the drawcalls to the DeviceContext
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="renderOrigin"></param>
		/// <param name="matrix"></param>
		public override void Draw(DeviceContext graphics, Vector3D renderOrigin, Matrix4F matrix)
		{
			if (this._drawCalls.Count > 0)
			{
				if (this.FontTexture == null)
				{
					this.Init(graphics);
				}

				graphics.PushShader();
				graphics.PushRenderState();
				graphics.DepthBuffer(true, false);
				graphics.SetShader(graphics.GetShader("NameTag"));
				graphics.SetTexture(this.FontTexture);
				graphics.SetRasterizerState(CullMode.None);

				foreach (WorldTextDrawCall drawCall in this._drawCalls)
				{
					TextureVertexDrawable drawable = drawCall.Drawable;
					Vector3F delta = (drawCall.Location - renderOrigin).ToVector3F();
					float rotation = VectorHelper.GetRotationInRadians(drawCall.Rotation);

					drawable.Render(graphics, Matrix4F.Identity
						.Translate(drawCall.Offset.ToVector3F())
						.Rotate(rotation, Vector3F.UnitY)
						.Translate(delta)
						.Multiply(matrix));
				}

				graphics.SetRasterizerState(CullMode.CullCounterClockwiseFace);
				graphics.PopShader();
				graphics.PopRenderState();
			}
		}

		/// <summary>
		/// Draw text
		/// </summary>
		/// <param name="v"></param>
		/// <param name="location"></param>
		/// <param name="rotation"></param>
		/// <param name="color"></param>
		public void DrawString(string text, Vector3D location, Vector3D offset, float scale, uint rotation, Color color)
		{
			this._drawCalls.Add(new WorldTextDrawCall(this.BuildDrawable(text, color, scale), location, offset, rotation));
		}

		/// <summary>
		/// Draw text contained within a region
		/// </summary>
		/// <param name="text"></param>
		/// <param name="location"></param>
		/// <param name="rotation"></param>
		/// <param name="color"></param>
		public void DrawString(string text, Vector2F textBox, BmFontAlign alignment, Vector3D offset, Vector3D location, float scale, uint rotation, Color color)
		{
			this._drawCalls.Add(new WorldTextDrawCall(this.BuildDrawable(text, textBox, alignment, color, scale), location, offset, rotation));
		}
	}
}
