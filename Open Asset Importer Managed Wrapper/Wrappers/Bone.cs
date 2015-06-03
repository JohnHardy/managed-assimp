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
    /// A bone belongs to a mesh stores a list of vertex weights. It represents a joint of the skeleton. The bone hierarchy is contained in the node graph.
    /// </summary>
    /// <author>John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk</author>
    /// <date>20 July 2009</date>
    [Serializable]
    public class Bone
    {
        #region Properties
        /// <summary>
        /// The string name of the bone
        /// </summary>
        private String sName = "";

        /// <summary>
        /// List of vertex weights for the bone
        /// </summary>
        private aiVertexWeight[] tWeights = null;

        /// <summary>
        /// Matrix that transforms from mesh space to bone space in bind pose.
        /// </summary>
        private aiMatrix4x4 mOffsetMatrix;
        #endregion

        /// <summary>
        /// Constructor which builds a Bone object from a pointer to an aiFace structure.  As a user, you should not call this by hand.
        /// <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
        /// </summary>
        /// <param name="aiBone*">A pointer to the bone structure in the low level unmanaged wrapper.</param>
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

        /// <summary>
        /// Retrieves the name of the node.
        /// </summary>
        /// <returns>Normally bones are never unnamed</returns>
	    public String getName()
        {
		    return sName;
	    }

        /// <summary>
        /// Returns a reference to the array of weights.
        /// </summary>
        /// <returns><code>Weight</code> array</returns>
	    public aiVertexWeight[] getWeightsArray()
        {
		    return tWeights;
	    }

        /// <summary>
        /// Returns the number of bone weights.
        /// </summary>
        /// <returns>There should at least be one vertex weights (the validation step would complain otherwise)</returns>
	    public int getNumWeights()
        {
		    return tWeights.Length;
	    }

        /// <summary>
        /// Returns the Matrix that transforms from mesh space to bone space in bind pose.
        /// </summary>
        /// <returns>Matrix that transforms from mesh space to bone space in bind pose.</returns>
        public aiMatrix4x4 getOffsetMatrix()
        {
            return mOffsetMatrix;
        }
    }
}
