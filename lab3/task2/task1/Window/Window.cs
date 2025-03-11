using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace task2
{
    public class Window : GameWindow
    {
        private List<BaseSmesharik> smeshariki = new List<BaseSmesharik>();
        private BaseSmesharik selectedSmesharik = null;
        private bool isDragging = false;
        private float offsetX, offsetY;

        public Window(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {
            smeshariki.Add(new Crosh(0, 0, 5, new Color4(0.5f, 0.8f, 1f, 1))); 
            smeshariki.Add(new Crosh(50, 10, 2, new Color4(1.0f, 0.5f, 0.8f, 1))); 
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            float diff = (float)Width / Height;
            GL.Ortho(-100 * diff, 100 * diff, -100, 100, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            foreach (var smesharik in smeshariki)
            {
                smesharik.Draw();
            }

            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButton.Left)
            {
                //Razobratcya cherez matrici
                float mouseX = (e.X - Width / 2f) / (Width / 2f) * 100 * ((float)Width / Height);
                float mouseY = -(e.Y - Height / 2f) / (Height / 2f) * 100;
                
                foreach (var smesharik in smeshariki)
                {
                    float distance = (float)Math.Sqrt(Math.Pow(mouseX - smesharik.x, 2) + Math.Pow(mouseY - smesharik.y, 2));
                    if (distance <= smesharik.size * 5)
                    {   
                        offsetX = mouseX - smesharik.x;
                        offsetY = mouseY - smesharik.y;
                        isDragging = true;
                        selectedSmesharik = smesharik;
                        break;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDragging && selectedSmesharik != null)
            {
                float mouseX = (e.X - Width / 2f) / (Width / 2f) * 100 * ((float)Width / Height);
                float mouseY = -(e.Y - Height / 2f) / (Height / 2f) * 100;

                selectedSmesharik.x = mouseX - offsetX;
                selectedSmesharik.y = mouseY - offsetY;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButton.Left)
            {
                isDragging = false;
                selectedSmesharik = null;
            }
        }
    }
}
