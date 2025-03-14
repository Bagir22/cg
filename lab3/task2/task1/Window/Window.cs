﻿using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace task2
{
    public class Window : GameWindow
    {
        private List<BaseSmesharik> smeshariki = new List<BaseSmesharik>();
        private BaseSmesharik _selectedSmesharik = null;
        private bool _isDragging = false;
        private float _offsetX, _offsetY;
        private Vector2 _offset;

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
                Vector2 mousePos = ConvertToWorldCoords(e.X, e.Y);

                foreach (var smesharik in smeshariki)
                {
                    float distance = (mousePos - new Vector2(smesharik.x, smesharik.y)).Length;

                    if (distance <= smesharik.size * 5)
                    {
                        _offset = mousePos - new Vector2(smesharik.x, smesharik.y);
                        _isDragging = true;
                        _selectedSmesharik = smesharik;
                        break;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (_isDragging && _selectedSmesharik != null)
            {
                Vector2 mousePos = ConvertToWorldCoords(e.X, e.Y);
                Vector2 newPos = mousePos - _offset;

                _selectedSmesharik.x = newPos.X;
                _selectedSmesharik.y = newPos.Y;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButton.Left)
            {
                _isDragging = false;
                _selectedSmesharik = null;
            }
        }

        private Vector2 ConvertToWorldCoords(int mouseX, int mouseY)
        {
            Vector4 vec = new Vector4(
                (float)mouseX / Width * 2 - 1,
                1 - (float)mouseY / Height * 2, 
                0, 1);

            Matrix4 view = GetMatrix(GetPName.ModelviewMatrix);
            Matrix4 proj = GetMatrix(GetPName.ProjectionMatrix);

            Matrix4 inv = Matrix4.Invert(view * proj);

            Vector4 result = Vector4.Transform(vec, inv);
            result /= result.W;

            return new Vector2(result.X, result.Y);
        }

        private Matrix4 GetMatrix(GetPName matrixMode)
        {
            float[] matrixData = new float[16];
            GL.GetFloat(matrixMode, matrixData);
            return new Matrix4(
                matrixData[0], matrixData[1], matrixData[2], matrixData[3],
                matrixData[4], matrixData[5], matrixData[6], matrixData[7],
                matrixData[8], matrixData[9], matrixData[10], matrixData[11],
                matrixData[12], matrixData[13], matrixData[14], matrixData[15]
            );
        }
    }
}
