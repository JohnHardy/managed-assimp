using System;
using System.Runtime.InteropServices;

using Assimp.ManagedAssimp.Unmanaged;


namespace Assimp.ManagedAssimp
{
    /**
     * <summary>A bone belongs to a mesh stores a list of vertex weights. It represents a joint of the skeleton. The bone hierarchy is contained in the node graph.</summary>
     * @author John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk
     * @date 20 July 2009
     * @version 1.0
     */
    [Serializable]
    public class Bone
    {
        #region Properties
        /** <summary>The string name of the bone</summary> */
        private String sName = "";

        /** <summary>List of vertex weights for the bone</summary> */
        private aiVertexWeight[] tWeights = null;

        /** <summary>Matrix that transforms from mesh space to bone space in bind pose.</summary> */
        private aiMatrix4x4 mOffsetMatrix;
        #endregion

        /**
         * <summary>Constructor which builds a Bone object from a pointer to an aiFace structure.  As a user, you should not call this by hand.</summary>
         * <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
         * @param aiBone* A pointer to the bone structure in the low level unmanaged wrapper.
         */
        unsafe internal Bone(IntPtr p_aiBone)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiBone tBone = (UnmanagedAssimp.aiBone)Marshal.PtrToStructure(p_aiBone, typeof(UnmanagedAssimp.aiBone));

            // Copy over the nice, simple value types into managed memory.  Value types copy the entire structure rather than just the pointer to it.
            this.mOffsetMatrix = tBone.mOffsetMatrix;

            // Copy the properties over into our class as marshalled managed memory.
            // Note that with strings, prepending an empty string "" forces a copy.
            this.sName              = "" + tBone.mName.data;

            // Marshal the vertex weight array.
            this.tWeights = UnmanagedAssimp.MarshalArray<aiVertexWeight>(new IntPtr(tBone.mWeights), tBone.mNumWeights);
        }

	    /**
	     * <summary>Retrieves the name of the node.</summary>
	     * @return Normally bones are never unnamed
	     */
	    public String getName()
        {
		    return sName;
	    }

	    /**
	     * <summary>Returns a reference to the array of weights.</summary>
	     * @return <code>Weight</code> array
	     */
	    public aiVertexWeight[] getWeightsArray()
        {
		    return tWeights;
	    }

	    /**
	     * <summary>Returns the number of bone weights.</summary>
         * @return There should at least be one vertex weights (the validation step would complain otherwise)
	     */
	    public int getNumWeights()
        {
		    return tWeights.Length;
	    }

        /**
         * <summary>Returns the Matrix that transforms from mesh space to bone space in bind pose.<summary>
         * @return Matrix that transforms from mesh space to bone space in bind pose.
         */
        public aiMatrix4x4 getOffsetMatrix()
        {
            return mOffsetMatrix;
        }
    }
}
