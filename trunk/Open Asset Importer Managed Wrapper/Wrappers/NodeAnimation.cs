using System;
using System.Runtime.InteropServices;

using Assimp.ManagedAssimp.Unmanaged;

namespace Assimp.ManagedAssimp
{
    /**
     * <summary>A bone animation channel defines the animation keyframes for a single bone in the mesh hierarchy.</summary>
     * @author John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk
     * @date 20 July 2009
     * @version 1.0
     */
    [Serializable]
    public class NodeAnimation
    {
        #region Properties.
        /**
         * <summary>Defines how the animation behaves before the first key is encountered.</summary>
         * The default value is aiAnimBehaviour_DEFAULT (the origional trnsformation matrix of the affected node is used).
         */
        private uint mPreState;

        /**
         * <summary>Defines how the animation behaves after the last key is encountered.</summary>
         * The default value is aiAnimBehaviour_DEFAULT (the origional transformation matrix of the affected node is used).
         */
        private uint mPostState;

        /**
         * <summary>Rotation keyframes.</summary>
         */
        private aiQuaternionKey[] tQuatKeys     = null;

        /**
         * <summary>Position keyframes.</summary>
         */
        private aiVector3DKey[] tPosKeys        = null;

        /**
         * <summary>Scaling keyframes.</summary>
         */
        private aiVector3DKey[] tScalingKeys    = null;

        /**
         * <summary>Name of the bone affected by this animation channel.</summary>
         */
        private String sName;
        #endregion

        #region Constructor
        /**
         * <summary>Constructor which builds a NodeAnimation object from a pointer to an aiAnimation structure.  As a user, you should not call this by hand.</summary>
         * <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
         * @param p_aiNodeAnimation* A pointer to the aiNodeAnimation structure in the low level unmanaged wrapper.
         */
        internal unsafe NodeAnimation(IntPtr p_aiNodeAnimation)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiNodeAnimation tAnimation = (UnmanagedAssimp.aiNodeAnimation)Marshal.PtrToStructure(p_aiNodeAnimation, typeof(UnmanagedAssimp.aiNodeAnimation));

            // Copy over the nice, simple value types into managed memory.  Value types copy the entire structure rather than just the pointer to it.
            this.mPostState = tAnimation.mPostState;
            this.mPreState  = tAnimation.mPreState;

            // Copy the properties over into our class as marshalled managed memory.
            // Note that with strings, prepending an empty string "" forces a copy.
            this.sName      = "" + tAnimation.mNodeName.data;

            // Copy over the array of node animations into managed memory belonging to this class instance.
            tPosKeys        = UnmanagedAssimp.MarshalArray<aiVector3DKey>(new IntPtr(tAnimation.mPositionKeys),     tAnimation.mNumPositionKeys);
            tScalingKeys    = UnmanagedAssimp.MarshalArray<aiVector3DKey>(new IntPtr(tAnimation.mScalingKeys),      tAnimation.mNumScalingKeys);
            tQuatKeys       = UnmanagedAssimp.MarshalArray<aiQuaternionKey>(new IntPtr(tAnimation.mRotationKeys),   tAnimation.mNumRotationKeys);
        }
        #endregion

        /**
	     * <summary>Returns the name of the bone affected by this animation channel.</summary>
	     * @return Bone name
	     */
        public String getName()
        {
		    return sName;
	    }

	    /**
	     * <summary>Returns the number of rotation keyframes.</summary>
	     * @return This can be 0.
	     */
	    public int getNumRotationKeys()
        {
            return null == tQuatKeys ? 0 : tQuatKeys.Length;
	    }

	    /**
	     * <summary>Returns the number of position keyframes.</summary>
	     * @return This can be 0.
	     */
	    public int getNumPositionKeys()
        {
            return null == tPosKeys ? 0 : tPosKeys.Length;
	    }

	    /**
	     * <summary>Returns the number of scaling keyframes.</summary>
	     * @return This can be 0.
	     */
	    public int getNumScalingKeys()
        {
		    return null == tScalingKeys ? 0 : tScalingKeys.Length;
	    }

	    /**
	     * <summary>Get a reference to the list of all rotation keyframes.</summary>
	     * @return Could be <code>null</code> if there are no rotation keys
	     */
	    public aiQuaternionKey[] getRotationKeys()
        {
		    return tQuatKeys;
	    }

	    /**
	     * <summary>Get a reference to the list of all position keyframes.</summary>
	     * @return Could be <code>null</code> if there are no position keys
	     */
	    public aiVector3DKey[] getPositionKeys()
        {
		    return tPosKeys;
	    }

	    /**
	     * <summary>Get a reference to the list of all scaling keyframes.</summary>
	     * @return Could be <code>null</code> if there are no scaling keys
	     */
	    public aiVector3DKey[] getScalingKeys()
        {
		    return tScalingKeys;
	    }

        /**
         * <summary>Defines how the animation behaves before the first key is encountered.</summary>
         * The default value is aiAnimBehaviour_DEFAULT (the origional trnsformation matrix of the affected node is used).
         * @return The aiAnimBehaviour which describes how the node animation should look before the first key is encountered.
         */
        public uint GetPreState()
        {
            return this.mPreState;
        }

        /**
         * <summary>Defines how the animation behaves after the last key is encountered.</summary>
         * The default value is aiAnimBehaviour_DEFAULT (the origional transformation matrix of the affected node is used).
         * @return The aiAnimBehaviour which describes how the node animation should look after the last key is encountered.
         */
        public uint GetPostState()
        {
            return this.mPostState;
        }
    }
}
