/*
 * This file is part of The Managed Assimp Wrapper.
 * 
 * The Managed Assimp Wrapper is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * The Managed Assimp Wrapper is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with The Managed Assimp Wrapper.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * If you would like to use The Managed Assimp Wrapper under another license, 
 * contact John Hardy at john at highwire-dtc dot com.
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
