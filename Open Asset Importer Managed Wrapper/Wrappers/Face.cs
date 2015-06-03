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
    /// Different types of faces.
    /// Faces of different primitive types can occur in a single mesh. To get
    /// homogeneous meshes, try the <code>PostProcessing.SortByPType</code> flag.
    /// </summary>
	public enum FaceType : int
    {
        /// <summary>
        /// This is just a single vertex in the virtual world. #aiFace contains just one index for such a primitive.
        /// </summary>
		POINT = 0x1,

        /// <summary>
        /// This is a line defined by start and end position. Surprise, Face defines two indices for a line.
        /// </summary>
		LINE = 0x2,

        /// <summary>
        /// A triangle, probably the only kind of primitive you wish to handle. 3 indices.
        /// </summary>
		TRIANGLE = 0x4,

        /// <summary>
        /// A simple, non-intersecting polygon defined by n points (n > 3). Can
        /// be concave or convex. Use the <code>PostProcessing.Triangulate</code>
        /// flag to have all polygons triangulated.
        /// </summary>
		 POLYGON = 0x8,
	}

    /// <summary>
    /// Represents a single face of a mesh. Faces a generally simple polygons and store n indices into the vertex streams of the mesh they belong to.
    /// </summary>
    /// <author>John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk</author>
    /// <date>20 July 2009</date>
    [Serializable]
    public class Face
    {
        #region Properties
        /// <summary>
        /// The indices of the face.
        /// </summary>
        private uint[] tIndices;
        #endregion

        /// <summary>
        /// Constructor which builds a Face object from a pointer to an aiFace structure.  As a user, you should not call this by hand.
        /// <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
        /// </summary>
        /// <param name="aiFace*">A pointer to the face structure in the low level unmanaged wrapper.</param>
        unsafe internal Face(IntPtr p_aiFace)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiFace tFace = (UnmanagedAssimp.aiFace)Marshal.PtrToStructure(p_aiFace, typeof(UnmanagedAssimp.aiFace));
            
            // Marshal the indices list.
            //IntPtr p = new IntPtr(tFace.mIndices);
            tIndices = UnmanagedAssimp.MarshalUintArray(tFace.mIndices, tFace.mNumIndices);
        }

        /// <summary>
        /// Get the indices of the face
        /// </summary>
        /// <returns>Array of n indices into the vertices of the father mesh. The return value is *never* <code>null</code></returns>
        public uint[] getIndices()
        {
            return tIndices;
        }
    }
}
