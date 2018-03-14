using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plukit.Base;
using Staxel;
using Staxel.Draw;
using Sunbeam.Staxel.Rendering.BitmapFont;
using System.Collections.Generic;

namespace Sunbeam.Staxel.Rendering
{
	public abstract class TextRenderer
	{
		protected abstract float Units { get; }
		protected Texture2D FontTexture;
		protected BmFontFile FontFile;
		protected Dictionary<char, BmFontChar> CharacterMap = new Dictionary<char, BmFontChar>();

		public abstract void Draw(DeviceContext graphics, Vector3D renderOrigin, Matrix4F matrix);
		public abstract void Clear();

		/// <summary>
		/// Build a TextureVertexDrawable from the text
		/// </summary>
		/// <param name="text"></param>
		/// <param name="color"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		protected TextureVertexDrawable BuildDrawable(string text, Color color, float scale = 1f)
		{
			float UnitScale = this.Units * scale;
			int verticesCount = text.Length * 4;
			TextureVertex[] vertices = new TextureVertex[verticesCount];

			float x = 0f;
			int i = 0;
			foreach (char l in text)
			{
				if (this.CharacterMap.TryGetValue(l, out BmFontChar fontChar))
				{
					Vector4F UVPos = this.CharToUV(fontChar);

					float xOffset = fontChar.XOffset * UnitScale;
					float yOffset = fontChar.YOffset * UnitScale;
					float xAdvance = fontChar.XAdvance * UnitScale;
					float width = fontChar.Width * UnitScale;
					float height = fontChar.Height * UnitScale;

					vertices[i].Color = color;
					vertices[i + 1].Color = color;
					vertices[i + 2].Color = color;
					vertices[i + 3].Color = color;

					vertices[i].Position = new Vector3F(x + xOffset, 0f, 0f);
					vertices[i + 1].Position = new Vector3F(x + xOffset, height, 0f);
					vertices[i + 2].Position = new Vector3F(x + xOffset + width, height, 0f);
					vertices[i + 3].Position = new Vector3F(x + xOffset + width, 0f, 0f);

					vertices[i].Texture = new Vector2F(UVPos.X, UVPos.W);
					vertices[i + 1].Texture = new Vector2F(UVPos.X, UVPos.Y);
					vertices[i + 2].Texture = new Vector2F(UVPos.Z, UVPos.Y);
					vertices[i + 3].Texture = new Vector2F(UVPos.Z, UVPos.W);

					x += xAdvance;
					i += 4;
				}
			}

			return new TextureVertexDrawable(vertices, verticesCount);
		}

		/// <summary>
		/// Build a TextureVertexDrawable from the text where it is contained within a region
		/// </summary>
		/// <param name="text"></param>
		/// <param name="textBox"></param>
		/// <param name="alignment"></param>
		/// <param name="color"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		protected TextureVertexDrawable BuildDrawable(string text, Vector2F textBox, BmFontAlign alignment, Color color, float scale)
		{
			float UnitScale = this.Units * scale;
			int verticesCount = text.Length * 4;
			TextureVertex[] vertices = new TextureVertex[verticesCount];
			List<BmFontCharInfo> charInfo = new List<BmFontCharInfo>();
			List<float> lineWidths = new List<float>();

			// Textbox edge locations, make its origin the center
			float textBoxLeft = -textBox.X * 0.5f;
			float textBoxRight = textBox.X * 0.5f;
			float textBoxTop = textBox.Y * 0.5f;
			float textBoxBottom = -textBox.Y * 0.5f;

			// Keep track of offset positions
			int vIdx = 0; // Vertex index
			float x = textBoxLeft;
			float y = textBoxTop;
			float lineHeight = this.FontFile.Info.Size * UnitScale;
			float lineWidth = 0f;
			int lineNumber = 1;
			int wordNumber = 1;

			// First we build everything, afterward we reposition vertices according to the BmFontCharinfo and alignment
			// Should be slightly more efficient as to realign all previous characters on the line as soon as we add a new one
			foreach (char c in text)
			{
				// Supplied new line
				if (c == '\r' || c == '\n')
				{
					x = textBoxLeft;
					y -= lineHeight;
					lineWidths.Add(lineWidth);
					lineWidth = 0f;
					wordNumber = 1;
					lineNumber++;

					continue;
				}

				BmFontChar fontChar = this.CharacterMap.GetOrDefault(c);
				if (fontChar == null)
				{
					continue;
				}

				float xOffset = fontChar.XOffset * UnitScale;
				float yOffset = fontChar.YOffset * UnitScale;
				float xAdvance = fontChar.XAdvance * UnitScale;
				float width = fontChar.Width * UnitScale;
				float height = fontChar.Height * UnitScale;

				// Newline supplied or needed
				if (lineWidth + xAdvance >= textBox.X)
				{
					x = textBoxLeft;
					y -= lineHeight;

					// Outside of bottom boundary; just stop
					if (y - yOffset - lineHeight < textBoxBottom)
					{
						break;
					}

					// If we exceed the textbox and are not on the first word
					// we move the last word down a line
					if (wordNumber != 1)
					{
						float previousLineWidth = lineWidth;
						lineWidth = 0f;

						for (int i = 0; i < charInfo.Count; i++)
						{
							// If the previous line ended with a space remove it's width
							if (charInfo[i].LineNumber == lineNumber && charInfo[i].WordNumber == (wordNumber - 1) && charInfo[i].Char == ' ')
							{
								previousLineWidth -= charInfo[i].XAdvance;
							}

							if (charInfo[i].LineNumber == lineNumber && charInfo[i].WordNumber == wordNumber)
							{
								charInfo[i].LineNumber++;
								charInfo[i].WordNumber = 1;

								vertices[charInfo[i].FirstVertexIndex].Position.X = x + charInfo[i].XOffset;
								vertices[charInfo[i].FirstVertexIndex].Position.Y = y - charInfo[i].Height;
								vertices[charInfo[i].FirstVertexIndex + 1].Position.X = x + charInfo[i].XOffset;
								vertices[charInfo[i].FirstVertexIndex + 1].Position.Y = y;
								vertices[charInfo[i].FirstVertexIndex + 2].Position.X = x + charInfo[i].XOffset + charInfo[i].Width;
								vertices[charInfo[i].FirstVertexIndex + 2].Position.Y = y;
								vertices[charInfo[i].FirstVertexIndex + 3].Position.X = x + charInfo[i].XOffset + charInfo[i].Width;
								vertices[charInfo[i].FirstVertexIndex + 3].Position.Y = y - charInfo[i].Height;

								x += charInfo[i].XAdvance;
								lineWidth += charInfo[i].XAdvance;
							}
						}

						lineWidths.Add(previousLineWidth - lineWidth);
					}
					else
					{
						lineWidths.Add(lineWidth);
						lineWidth = 0f;
					}

					wordNumber = 1;
					lineNumber++;
				}

				// Ignore spaces on a new line
				if (lineWidth == 0f && c == ' ')
				{
					continue;
				}

				Vector4F UVPos = this.CharToUV(fontChar);

				vertices[vIdx].Color = color;
				vertices[vIdx + 1].Color = color;
				vertices[vIdx + 2].Color = color;
				vertices[vIdx + 3].Color = color;

				vertices[vIdx].Position = new Vector3F(x + xOffset, y - height, 0f);
				vertices[vIdx + 1].Position = new Vector3F(x + xOffset, y, 0f);
				vertices[vIdx + 2].Position = new Vector3F(x + xOffset + width, y, 0f);
				vertices[vIdx + 3].Position = new Vector3F(x + xOffset + width, y - height, 0f);

				vertices[vIdx].Texture = new Vector2F(UVPos.X, UVPos.W);
				vertices[vIdx + 1].Texture = new Vector2F(UVPos.X, UVPos.Y);
				vertices[vIdx + 2].Texture = new Vector2F(UVPos.Z, UVPos.Y);
				vertices[vIdx + 3].Texture = new Vector2F(UVPos.Z, UVPos.W);

				if (c == ' ')
				{
					wordNumber++;
				}

				charInfo.Add(new BmFontCharInfo(c, vIdx, xOffset, yOffset, width, height, xAdvance, lineNumber, wordNumber));

				if (c == ' ')
				{
					wordNumber++;
				}

				x += xAdvance;
				lineWidth += xAdvance;
				vIdx += 4;
			}

			// Bail out early if we have top left alignment
			if (!alignment.HasFlag(BmFontAlign.Center) && !alignment.HasFlag(BmFontAlign.Right)
				&& !alignment.HasFlag(BmFontAlign.Middle) && !alignment.HasFlag(BmFontAlign.Bottom))
			{
				return new TextureVertexDrawable(vertices, verticesCount);
			}

			// Add last line, this never creates a new line so we need to manually add it
			lineWidths.Add(lineWidth);

			// Lets justify the text!
			float offsetX = 0;
			float offsetY = 0;
			float textHeight = lineWidths.Count * lineHeight;

			if (alignment.HasFlag(BmFontAlign.Middle))
				offsetY -= (textBox.Y - textHeight) / 2;
			if (alignment.HasFlag(BmFontAlign.Bottom))
				offsetY -= textBox.Y - textHeight;

			for (int line = 0; line < lineWidths.Count; line++)
			{
				lineWidth = lineWidths[line];

				if (alignment.HasFlag(BmFontAlign.Center))
					offsetX = (textBox.X - lineWidth) / 2;
				if (alignment.HasFlag(BmFontAlign.Right))
					offsetX = textBox.X - lineWidth;

				for (int j = 0; j < charInfo.Count; j++)
				{
					if (charInfo[j].LineNumber == (line + 1))
					{
						vertices[charInfo[j].FirstVertexIndex].Position.X += offsetX;
						vertices[charInfo[j].FirstVertexIndex].Position.Y += offsetY;
						vertices[charInfo[j].FirstVertexIndex + 1].Position.X += offsetX;
						vertices[charInfo[j].FirstVertexIndex + 1].Position.Y += offsetY;
						vertices[charInfo[j].FirstVertexIndex + 2].Position.X += offsetX;
						vertices[charInfo[j].FirstVertexIndex + 2].Position.Y += offsetY;
						vertices[charInfo[j].FirstVertexIndex + 3].Position.X += offsetX;
						vertices[charInfo[j].FirstVertexIndex + 3].Position.Y += offsetY;
					}
				}
			}

			return new TextureVertexDrawable(vertices, verticesCount);
		}

		/// <summary>
		/// Retrieves the UV coordinates for agiven BmFontChar
		/// </summary>
		/// <param name="tChar"></param>
		/// <returns></returns>
		protected Vector4F CharToUV(BmFontChar tChar)
		{
			Vector4I charPosition = tChar.GetVector4I();

			return new Vector4F(
				(float)charPosition.X / (float)this.FontTexture.Width,
				((float)charPosition.Y / (float)this.FontTexture.Height),
				(float)(charPosition.X + charPosition.Z) / (float)this.FontTexture.Width,
				((float)(charPosition.Y + charPosition.W) / (float)this.FontTexture.Height)
			);
		}

		/// <summary>
		/// Initialize the TextRenderer
		/// </summary>
		/// <param name="graphics"></param>
		public void Init(DeviceContext graphics)
		{
			// Do nothing if both are initialized
			if (this.FontTexture != null && !this.FontTexture.IsDisposed)
			{
				return;
			}

			if (this.FontTexture != null && this.FontTexture.IsDisposed)
			{
				this.CharacterMap.Clear();
			}

			this.FontFile = BmFontLoader.Load(GameContext.ContentLoader.RootDirectory + "/mods/Sunbeam/Font/helvetipixel.fnt");
			this.FontTexture = graphics.GetTexture("mods/Sunbeam/Font/helvetipixel_0");

			foreach (BmFontChar fChar in this.FontFile.Chars)
			{
				char c = (char)fChar.ID;
				this.CharacterMap.Add(c, fChar);
			}
		}
	}
}
