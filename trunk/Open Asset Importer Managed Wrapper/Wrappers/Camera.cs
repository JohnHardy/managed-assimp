using System;
using System.Runtime.InteropServices;

using Assimp.ManagedAssimp.Unmanaged;


namespace Assimp.ManagedAssimp
{
    /**
     * <summary>Describes a virtual camera in the scene.</summary>
     * Cameras have a representation in the node graph and can be animated.
     * @author John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk
     * @date 20 July 2009
     * @version 1.0
     */
    [Serializable]
    public class Camera
    {
        #region Properties
        /**
         * <summary>The name of the camera.</summary>
         */
        private String mName;

        /**
         * <summary>Position of the camera relative to the parent.</summary>
         */
        private aiVector3D mPosition;

        /**
         * <summary>'Up' - vector of the camera coordinate system relative to the parent.</summary>
         */
        private aiVector3D mUp;

        /**
         * <summary>'LookAt' - vector of the camera coordinate system relative to the parent.</summary>
         */
        private aiVector3D mLookAt;

        /**
         * <summary>Half horizontal field of view angle, in radians.</summary>
         */
        private float mHorizontalFOV;

        /**
         * <summary>Distance of the near clipping plane from the camera.</summary>
         */
        private float mClipPlaneNear;

        /**
         * <summary>Distance of the far clipping plane from the camera.</summary>
         */
        private float mClipPlaneFar;

        /**
         * <summary>Screen aspect ratio.</summary>
         */
        private float mAspect;
        #endregion

        /**
         * <summary>Constructor which builds a Camera object from a pointer to an aiCamera structure.  As a user, you should not call this by hand.</summary>
         * <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
         * @param aiCamera* A pointer to the camera structure in the low level unmanaged wrapper.
         */
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

        /**
	     * <summary>Get the screen aspect ratio of the camera</summary>
	     * 
	     * This is the ration between the width and the height of the screen.
	     * Typical values are 4/3, 1/2 or 1/1. This value is 0 if the aspect ratio
	     * is not defined in the source file. 0 is also the default value.
	     */
        public float GetAspect()
        {
		    return mAspect;
	    }

        /**
         * <summary>Get the distance of the far clipping plane from the camera.</summary>
         * 
         * The far clipping plane must, of course, be farer away than the near
         * clipping plane. The default value is 1000.f. The radio between the near
         * and the far plane should not be too large (between 1000-10000 should be
         * ok) to avoid floating-point inaccuracies which could lead to z-fighting.
         */
        public float GetFarClipPlane()
        {
		    return mClipPlaneFar;
	    }

        /**
         * <summary>Get the distance of the near clipping plane from the camera.</summary>
         * 
         * The value may not be 0.f (for arithmetic reasons to prevent a division
         * through zero). The default value is 0.1f.
         */
        public float GetNearClipPlane()
        {
		    return mClipPlaneNear;
	    }

        /**
         * <summary>Half horizontal field of view angle, in radians.</summary>
         * 
         * The field of view angle is the angle between the center line of the
         * screen and the left or right border. The default value is 1/4PI.
         */
        public float GetHorizontalFOV()
        {
		    return mHorizontalFOV;
	    }

        /**
         * <summary>Returns the 'LookAt' - vector of the camera coordinate system relative to
         * the coordinate space defined by the corresponding node.</summary>
         * 
         * This is the viewing direction of the user. The default value is 0|0|1.
         * The vector may be normalized, but it needn't.
         * 
         * @return component order: x,y,z
         */
        public aiVector3D GetLookAt()
        {
		    return mLookAt;
	    }

        /**
         * <summary>Get the 'Up' - vector of the camera coordinate system relative to the
         * coordinate space defined by the corresponding node.</summary>
         * 
         * The 'right' vector of the camera coordinate system is the cross product
         * of the up and lookAt vectors. The default value is 0|1|0. The vector may
         * be normalized, but it needn't.
         * 
         * @return component order: x,y,z
         */
        public aiVector3D GetUp()
        {
		    return mUp;
	    }

        /**
         * <summary>Get the position of the camera relative to the coordinate space defined
         * by the corresponding node.</summary>
         * 
         * The default value is 0|0|0.
         * 
         * @return component order: x,y,z
         */
        public aiVector3D GetPosition()
        {
		    return mPosition;
	    }

        /**
         * <summary>Returns the name of the camera.</summary>
         * 
         * There must be a node in the scene graph with the same name. This node
         * specifies the position of the camera in the scene hierarchy and can be
         * animated. The local transformation information of the camera is relative
         * to the coordinate space defined by this node.
         */
        public String GetName()
        {
		    return mName;
	    }
    }
}
