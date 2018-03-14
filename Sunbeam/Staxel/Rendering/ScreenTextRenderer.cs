using Plukit.Base;
using Staxel.Core;
using Staxel.Draw;
using System.Collections.Generic;

namespace Sunbeam.Staxel.Rendering
{
	internal struct ScreenTextDrawCall
	{
		public readonly TextureVertexDrawable Drawable;
		public readonly Vector2F Position;
		public readonly float Scale;

		public ScreenTextDrawCall(TextureVertexDrawable drawable, Vector2F position, float scale = 1f)
		{
			this.Drawable = drawable;
			this.Position = position;
			this.Scale = scale;
		}
	}

	public sealed class ScreenTextRenderer: TextRenderer
	{
		protected override float Units => 0.05f;
		private List<ScreenTextDrawCall> _drawCalls = new List<ScreenTextDrawCall>();

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

				foreach (ScreenTextDrawCall drawCall in this._drawCalls)
				{
					TextureVertexDrawable drawable = drawCall.Drawable;
					Vector2F position = drawCall.Position;
					float scale = drawCall.Scale;

					Vector2I viewPortSize = graphics.GetViewPortSize();
					scale *= Constants.UIZoomFactor / ((float)viewPortSize.Y / Constants.ViewPortScaleThreshold.Y);
					position *= Constants.UIZoomFactor;

					Vector2F projectedPosition = graphics.ScreenPosToProjectionPos(position);
					Matrix4F movedMatrix2 = Matrix4F.CreateTranslation(new Vector3F(projectedPosition.X, projectedPosition.Y, 0f));
					Matrix4F overlayMatrix = graphics.GetOverlayMatrix();
					movedMatrix2 = Matrix4F.Multiply(overlayMatrix, movedMatrix2);
					graphics.SetProjectionMatrix(movedMatrix2);

					drawable.Render(graphics, Matrix4F.Identity
						.Multiply(matrix));
				}

				graphics.PopShader();
				graphics.PopRenderState();
			}
		}
	}
}
