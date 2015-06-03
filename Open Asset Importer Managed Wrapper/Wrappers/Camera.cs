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
using System.Runtime.InteropServices;

using Assimp.ManagedAssimp.Unmanaged;


namespace Assimp.ManagedAssimp
{
    /// <summary>
    /// Describes a virtual camera in the scene.
    /// Cameras have a representation in the node graph and can be animated.
    /// </summary>
    /// <author>John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk</author>
    /// <date>20 July 2009</date>
    [Serializable]
    public class Camera
    {
        #region Properties
        /// <summary>
        /// The name of the camera.
        /// </summary>
        private String mName;

        /// <summary>
        /// Position of the camera relative to the parent.
        /// </summary>
        private aiVector3D mPosition;

        /// <summary>
        /// 'Up' - vector of the camera coordinate system relative to the parent.
        /// </summary>
        private aiVector3D mUp;

        /// <summary>
        /// 'LookAt' - vector of the camera coordinate system relative to the parent.
        /// </summary>
        private aiVector3D mLookAt;

        /// <summary>
        /// Half horizontal field of view angle, in radians.
        /// </summary>
        private float mHorizontalFOV;

        /// <summary>
        /// Distance of the near clipping plane from the camera.
        /// </summary>
        private float mClipPlaneNear;

        /// <summary>
        /// Distance of the far clipping plane from the camera.
        /// </summary>
        private float mClipPlaneFar;

        /// <summary>
        /// Screen aspect ratio.
        /// </summary>
        private float mAspect;
        #endregion

        /// <summary>
        /// Constructor which builds a Camera object from a pointer to an aiCamera structure.  As a user, you should not call this by hand.
        /// <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
        /// </summary>
        /// <param name="aiCamera*">A pointer to the camera structure in the low level unmanaged wrapper.</param>
        unsafe internal Camera(IntPtr p_aiCamera)//UnmanagedAssimp.aiCamera* pCamera)
        {
            // Cast the IntPtr to a pointer which contains our camera structure in the unmanaged assimp memory.
            // Note: This didn't work... :-(
            // UnmanagedAssimp.aiCamera* pCamera = (UnmanagedAssimp.aiCamera*)p_aiCamera.ToPointer();

            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiCamera tCamera = (UnmanagedAssimp.aiCamera)Marshal.PtrToStructure(p_aiCamera, typeof(UnmanagedAssimp.aiCamera));

            // Copy over the nice, simple value types into managed memory.  Value types copy the entire structure rather than just the pointer to it.
            this.mHorizontalFOV     = tCamera.mHorizontalFOV;
            this.mClipPlaneFar      = tCamera.mClipPlaneFar;
            this.mClipPlaneNear     = tCamera.mClipPlaneNear;
            this.mAspect            = tCamera.mAspect;

            this.mLookAt            = tCamera.mLookAt;
            this.mUp                = tCamera.mUp;
            this.mPosition          = tCamera.mPosition;

            // Copy the properties over into our class as marshalled managed memory.
            // Note that with strings, prepending an empty string "" forces a copy.
            this.mName              = "" + tCamera.mName.data;
        }

        /// <summary>
        /// Get the screen aspect ratio of the camera
        /// This is the ration between the width and the height of the screen.
        /// Typical values are 4/3, 1/2 or 1/1. This value is 0 if the aspect ratio
        /// is not defined in the source file. 0 is also the default value.
        /// </summary>
        public float GetAspect()
        {
		    return mAspect;
	    }

        /// <summary>
        /// Get the distance of the far clipping plane from the camera.
        /// The far clipping plane must, of course, be farer away than the near
        /// clipping plane. The default value is 1000.f. The radio between the near
        /// and the far plane should not be too large (between 1000-10000 should be
        /// ok) to avoid floating-point inaccuracies which could lead to z-fighting.
        /// </summary>
        public float GetFarClipPlane()
        {
		    return mClipPlaneFar;
	    }

        /// <summary>
        /// Get the distance of the near clipping plane from the camera.
        /// The value may not be 0.f (for arithmetic reasons to prevent a division
        /// through zero). The default value is 0.1f.
        /// </summary>
        public float GetNearClipPlane()
        {
		    return mClipPlaneNear;
	    }

        /// <summary>
        /// Half horizontal field of view angle, in radians.
        /// The field of view angle is the angle between the center line of the
        /// screen and the left or right border. The default value is 1/4PI.
        /// </summary>
        public float GetHorizontalFOV()
        {
		    return mHorizontalFOV;
	    }

        /// <summary>
        /// Returns the 'LookAt' - vector of the camera coordinate system relative to
        /// the coordinate space defined by the corresponding node.
        /// This is the viewing direction of the user. The default value is 0|0|1.
        /// The vector may be normalized, but it needn't.
        /// </summary>
        /// <returns>component order: x,y,z</returns>
        public aiVector3D GetLookAt()
        {
		    return mLookAt;
	    }

        /// <summary>
        /// Get the 'Up' - vector of the camera coordinate system relative to the
        /// coordinate space defined by the corresponding node.
        /// The 'right' vector of the camera coordinate system is the cross product
        /// of the up and lookAt vectors. The default value is 0|1|0. The vector may
        /// be normalized, but it needn't.
        /// </summary>
        /// <returns>component order: x,y,z</returns>
        public aiVector3D GetUp()
        {
		    return mUp;
	    }

        /// <summary>
        /// Get the position of the camera relative to the coordinate space defined
        /// by the corresponding node.
        /// The default value is 0|0|0.
        /// </summary>
        /// <returns>component order: x,y,z</returns>
        public aiVector3D GetPosition()
        {
		    return mPosition;
	    }

        /// <summary>
        /// Returns the name of the camera.
        /// There must be a node in the scene graph with the same name. This node
        /// specifies the position of the camera in the scene hierarchy and can be
        /// animated. The local transformation information of the camera is relative
        /// to the coordinate space defined by this node.
        /// </summary>
        public String GetName()
        {
		    return mName;
	    }
    }
}
