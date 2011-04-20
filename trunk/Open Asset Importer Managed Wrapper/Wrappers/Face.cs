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
