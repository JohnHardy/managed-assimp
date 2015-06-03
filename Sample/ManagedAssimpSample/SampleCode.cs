/*
 * Copyright (C) 2011 by John Hardy
 * 
 * This file is part of The Managed Assimp Wrapper.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to dea
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * If you would like to use The Managed Assimp Wrapper under another license, 
 * contact John Hardy at john at highwire-dtc dot com.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 * Many thanks to the people at Assimp (assimp.sourceforge.net) 
 * and SlimDX (slimdx.org) for their fantastic work without which, this would not have been
 * possible.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.SampleFramework;

using Assimp.ManagedAssimp;


namespace ManagedAssimpSample
{
    /// <summary>
    /// A simple window with a mesh rendering.
    /// This is based heavily on the SlimDX samples.
    /// </summary>
    public class SampleWindow : Sample
    {
        /// <summary>
        /// A reference to the vertex declaration that describes how to render our special vertex format.
        /// </summary>
    	private VertexDeclaration pVertexDeclaration;

        /// <summary>
        /// A reference to the model we want to render.
        /// </summary>
        private SimpleModel pModel;

        /// <summary>
        /// Construct a new sample window to render a mesh.
        /// </summary>
        /// <param name="sMeshPath">The mesh to render.</param>
        public SampleWindow(String sMeshPath)
        {
            // Load the mesh.
            pModel = new SimpleModel(sMeshPath);
        }

        /// <summary>
        /// Called to create the render device and window.
        /// </summary>
        protected override void OnInitialize()
        {
            // Build the device settings.
            var settings = new DeviceSettings9
            {
                AdapterOrdinal = 0,
                CreationFlags = CreateFlags.HardwareVertexProcessing,
                Width = WindowWidth,
                Height = WindowHeight
            };

            // Start up the graphics device.
            InitializeDevice(settings);
        }

        /// <summary>
        /// This is called when we need to reload resources (for instance, when the device resets).
        /// </summary>
        protected override void OnResourceLoad()
        {
            // If we have no context, ignore.
            if (Context9 == null)
                return;

            // Reload the texture.
            pModel.reloadTextures(Context9.Device);

            // Disable lighting.
            Context9.Device.SetRenderState( RenderState.Lighting, false );

            // Setup the declaration which tells us how to render it.
        	pVertexDeclaration = new VertexDeclaration(Context9.Device, new[] {
				new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0), 
                new VertexElement(0, 12, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
				VertexElement.VertexDeclarationEnd
        	});
        }

        /// <summary>
        /// This is called to free any resources when the device is lost.
        /// </summary>
        protected override void OnResourceUnload()
        {
            // Delete the textures on the model.
            if (pModel != null)
                pModel.freeTextures();
  
            // Delete the vertex declaration.
            if (pVertexDeclaration != null)
			    pVertexDeclaration.Dispose();
        }

        /// <summary>
        /// This is called to reset the render target before the frame is drawn.
        /// </summary>
        protected override void OnRenderBegin()
        {
            Context9.Device.Clear( ClearFlags.Target | ClearFlags.ZBuffer, 0x05803, 1.0f, 0 );
            Context9.Device.BeginScene();
        }

        /// <summary>
        /// This is called to actually render the model.
        /// </summary>
        protected override void OnRender()
        {
            // Swizz the camera round a bit.
            long time = Environment.TickCount % 5000;
            float angle = (float)(time * (2.0f * Math.PI) / 5000.0f);
            Matrix world = Matrix.RotationY(angle);
            Context9.Device.SetTransform(TransformState.World, world);

            // Set up our view matrix.
            Vector3 Eye = new Vector3(0.0f, 3.0f, -5.0f);
            Eye.Normalize();
            Eye *= (pModel.Radius * 2.5f);
            Vector3 LookAt = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f);
            Matrix view = Matrix.LookAtLH(Eye, LookAt, Up);
            Context9.Device.SetTransform(TransformState.View, view);

            // For the projection matrix, we set up a perspective transform (which
            // transforms geometry from 3D view space to 2D viewport space.
            float fov = (float)Math.PI / 4;
            float apsectRatio = (float)this.WindowWidth / this.WindowHeight;
            float nearPlane = pModel.Radius;
            float farPlane = pModel.Radius * 1000;
            Matrix projection = Matrix.PerspectiveFovLH(fov, apsectRatio, nearPlane, farPlane);
            Context9.Device.SetTransform(TransformState.Projection, projection);
            
            // Set the vertex declaration.
        	Context9.Device.VertexDeclaration = pVertexDeclaration;

            // Draw the model.
            pModel.drawMesh(Context9.Device);
        }

        /// <summary>
        /// This is called to present the frame to the screen once it has been drawn.
        /// </summary>
        protected override void OnRenderEnd()
        {
            Context9.Device.EndScene();
            Context9.Device.Present();
        }
    }
}
