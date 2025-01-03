using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace osucatch_editor_realtimeviewer
{
    public class Texture2D : IDisposable
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        private int textureId = 0;
        public Texture2D(Stream stream)
        {
            using (var bitmap = new Bitmap(stream))
            {
                this.Width = bitmap.Width;
                this.Height = bitmap.Height;
                this.textureId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, this.textureId);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, this.Width, this.Height), ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bitmap.UnlockBits(data);
            }
            stream.Dispose();
        }

        public Texture2D(string text, float fontScale)
        {
            // 创建一个Bitmap对象，大小为文本的尺寸
            int bitmapWidth = (int)((text.Contains(".")) ? text.Length * 24 / fontScale : text.Length * 28 / fontScale);
            Bitmap bitmap = new Bitmap(bitmapWidth, 40);

            // 使用指定的背景颜色填充Bitmap
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // 设置文字的渲染质量
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                Font font = new Font("Arial", (float)(32.0 * fontScale));
                SolidBrush solidBrush = new SolidBrush(Color.White);
                // 使用指定的Font和颜色绘制文本
                g.DrawString(text, font, solidBrush, new PointF(0, 0));
                font.Dispose();
                solidBrush.Dispose();
            }
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
            this.textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, this.textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, this.Width, this.Height), ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
        }

        public void Draw(Vector2 pos, Vector2 origin, Color4 color)
        {
            pos -= origin;
            GL.Color4(color);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.BindTexture(TextureTarget.Texture2D, this.textureId);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(pos.X, pos.Y);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(pos.X + this.Width, pos.Y);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(pos.X + this.Width, pos.Y + this.Height);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(pos.X, pos.Y + this.Height);
            GL.End();
        }
        public void Draw(Vector2 pos, float w, float h, Vector2 origin, Color4 color)
        {
            pos -= origin;
            GL.Color4(color);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.BindTexture(TextureTarget.Texture2D, this.textureId);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(pos.X, pos.Y);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(pos.X + w, pos.Y);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(pos.X + w, pos.Y + h);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(pos.X, pos.Y + h);
            GL.End();
        }
        public void Draw(Vector2 pos, Vector2 origin, Color4 color, float rotation, float scale)
        {
            pos -= origin;
            GL.Color4(color);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            if (rotation != 0 || scale != 0)
            {
                Vector3 diff = new Vector3(-pos.X - origin.X, -pos.Y - origin.Y, 0.0f);
                GL.Translate(-diff);
                GL.Rotate(MathHelper.RadiansToDegrees(rotation), 0.0f, 0.0f, 1.0f);
                GL.Scale(scale, scale, 1.0f);
                GL.Translate(diff);
            }
            GL.BindTexture(TextureTarget.Texture2D, this.textureId);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(pos.X, pos.Y);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(pos.X + this.Width, pos.Y);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(pos.X + this.Width, pos.Y + this.Height);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(pos.X, pos.Y + this.Height);
            GL.End();
        }
        public void Draw(Vector2 pos, Vector2 origin, Color4 color, Rectangle source, float rotation, float scale)
        {
            pos -= origin;
            Vector2 texCoordMin = new Vector2(source.X / (float)this.Width, source.Y / (float)this.Height);
            Vector2 texCoordMax = new Vector2((source.X + source.Width) / (float)this.Width, (source.Y + source.Height) / (float)this.Height);
            GL.Color4(color);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            if (rotation != 0 || scale != 0)
            {
                Vector3 diff = new Vector3(-pos.X - origin.X, -pos.Y - origin.Y, 0.0f);
                GL.Translate(-diff);
                GL.Rotate(MathHelper.RadiansToDegrees(rotation), 0.0f, 0.0f, 1.0f);
                GL.Scale(scale, scale, 1.0f);
                GL.Translate(diff);
            }
            GL.BindTexture(TextureTarget.Texture2D, this.textureId);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(texCoordMin.X, texCoordMin.Y);
            GL.Vertex2(pos.X, pos.Y);
            GL.TexCoord2(texCoordMax.X, texCoordMin.Y);
            GL.Vertex2(pos.X + source.Width, pos.Y);
            GL.TexCoord2(texCoordMax.X, texCoordMax.Y);
            GL.Vertex2(pos.X + source.Width, pos.Y + source.Height);
            GL.TexCoord2(texCoordMin.X, texCoordMax.Y);
            GL.Vertex2(pos.X, pos.Y + source.Height);
            GL.End();
        }
        public void Dispose()
        {
            GL.DeleteTexture(this.textureId);
        }
    }
}
