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
    /// A mesh represents a part of the whole scene geometry. It references exactly
    /// one material and can this be drawn in a single draw call.
    /// <p/>
    /// It usually consists of a number of vertices and a series of primitives/faces
    /// referencing the vertices. In addition there might be a series of bones, each
    /// of them addressing a number of vertices with a certain weight. Vertex data is
    /// presented in channels with each channel containing a single per-vertex
    /// information such as a set of texture coordinates or a normal vector.
    /// <p/>
    /// Note that not all mesh data channels must be there. E.g. most models don't
    /// contain vertex colors so this data channel is mostly not filled.
    /// </summary>
    /// <author>John Hardy</author>
    /// <date>20 July 2009</date>
    [Serializable]
    public class Mesh
    {
        #region Constants
    /// <summary>
    /// Defines the maximum number of UV(W) channels that are available for a
    /// mesh. If a loader finds more channels in a file, it skips them.
    /// </summary>
	    public const int MAX_NUMBER_OF_TEXTURECOORDS    = UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_TEXTURECOORDS;//0x4;

    /// <summary>
    /// Defines the maximum number of vertex color channels that are available
    /// for a mesh. If a loader finds more channels in a file, it skips them.
    /// </summary>
	    public const int MAX_NUMBER_OF_COLOR_SETS       = UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_COLOR_SETS;//0x4;
        #endregion

        #region Properties
		/// <summary>
		/// The name of the mesh. May be empty.
		/// </summary>
		private String sName = null;
		
        /// <summary>
        /// Contains normal vectors in a continuous float array, xyz order. Can't be <code>null</code>
        /// </summary>
        private aiVector3D[] tVertices      = null;

        /// <summary>
        /// Contains normal vectors in a continuous float array, xyz order. Can be <code>null</code>
        /// </summary>
        private aiVector3D[] tNormals       = null;

        /// <summary>
        /// Contains tangent vectors in a continuous float array, xyz order. Can be <code>null</code>
        /// </summary>
        private aiVector3D[] tTangents      = null;

        /// <summary>
        /// Contains bitangent vectors in a continuous float array, xyz order. Can be <code>null</code>
        /// </summary>
        private aiVector3D[] tBitangents    = null;

        /// <summary>
        /// Contains UV coordinate channels in a continuous float array, uvw order. Unused channels are set to <code>null</code>.
        /// </summary>
        private aiVector3D[][] tTextures    = null;//new aiVector3D[UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_TEXTURECOORDS][];

        /// <summary>
        /// Contains vertex color channels in a continuous float array, rgba order. Unused channels are set to <code>null</code>.
        /// </summary>
        private aiColor4D[][] tColors       = null;//new aiColor4D[UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_COLOR_SETS][];

        /// <summary>
        /// Defines the number of relevant vector components for each UV channel. Typically the value is 2 or 3.
        /// </summary>
        private uint[] tNumUVComponents     = null;//new uint[UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_TEXTURECOORDS];

        /// <summary>
        /// The list of all faces for the mesh.
        /// </summary>
        private Face[] tFaces   = null;

        /// <summary>
        /// Bones influencing the mesh
        /// </summary>
        private Bone[] tBones   = null;

        /// <summary>
        /// Material index of the mesh. This is simply an index into the global material array.
        /// </summary>
        private uint iMaterialIndex     = 0;

        /// <summary>
        /// Bitwise combination of the kinds of primitives present in the mesh. The constant values are enumerated in <code>Face.Type</code>
        /// </summary>
        private uint iPrimitiveTypes    = 0;
        #endregion

        /// <summary>
        /// Constructor which builds a Mesh object from a pointer to an aiMesh structure.  As a user, you should not call this by hand.
        /// <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
        /// </summary>
        /// <param name="aiMesh*">A pointer to the face structure in the low level unmanaged wrapper.</param>
        unsafe internal Mesh(IntPtr p_aiMesh)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiMesh tMesh = (UnmanagedAssimp.aiMesh)Marshal.PtrToStructure(p_aiMesh, typeof(UnmanagedAssimp.aiMesh));

            // Copy over the nice, simple value types into managed memory.  Value types copy the entire structure rather than just the pointer to it.
            this.iPrimitiveTypes    = tMesh.mPrimitiveTypes;
            this.iMaterialIndex     = tMesh.mMaterialIndex;
			
			// Note that with strings, prepending an empty string "" forces a copy.
			this.sName = "" + tMesh.mName.data;
			
            // Marshal the arrays of vertices, 
            tVertices   = UnmanagedAssimp.MarshalArray<aiVector3D>(new IntPtr(tMesh.mVertices),     tMesh.mNumVertices);
            tNormals    = UnmanagedAssimp.MarshalArray<aiVector3D>(new IntPtr(tMesh.mNormals),      tMesh.mNumVertices);
            tTangents   = UnmanagedAssimp.MarshalArray<aiVector3D>(new IntPtr(tMesh.mTangents),     tMesh.mNumVertices);
            tBitangents = UnmanagedAssimp.MarshalArray<aiVector3D>(new IntPtr(tMesh.mBitangents),   tMesh.mNumVertices);
            
            // We can just do a straight copy of the tNumUVComponents - an array of uints of length AI_MAX_NUMBER_OF_TEXTURECOORDS.
            tNumUVComponents = new uint[UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_TEXTURECOORDS];
            for (int iChannel = 0; iChannel < UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_TEXTURECOORDS; ++iChannel)
            {
                tNumUVComponents[iChannel] = tMesh.mNumUVComponents[iChannel];
                //Console.WriteLine(iChannel + " has " + tNumUVComponents[iChannel] + " UV components");
            }

            // Now the colours and texture coords are a little tricker.
            // They are stored like this:
            // Array = {IntPtr, IntPtr, IntPtr, IntPtr}

            // Import texture coordinate sets for n channels.
            tTextures = new aiVector3D[UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_TEXTURECOORDS][];
            for (int iTextureChannel = 0; iTextureChannel < UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_TEXTURECOORDS; ++iTextureChannel)
            {
                // If this texture channel is present, use it - otherwise skip it.  Perhaps check tNumUVComponents.
                if (tMesh.mTextureCoords[iTextureChannel] != IntPtr.Zero)
                    tTextures[iTextureChannel] = (aiVector3D[])UnmanagedAssimp.MarshalArray<aiVector3D>(tMesh.mTextureCoords[iTextureChannel], tMesh.mNumVertices);
                else
                    tTextures[iTextureChannel] = null;
            }

            // Import colour sets for n channels.
            tColors = new aiColor4D[UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_COLOR_SETS][];
            for (int iColourChannel = 0; iColourChannel < UnmanagedAssimp.aiMesh.AI_MAX_NUMBER_OF_COLOR_SETS; ++iColourChannel)
            {
                // If this colour channel is present, parse it - otherwise skip.
                if (tMesh.mColors[iColourChannel] != IntPtr.Zero)
                    tColors[iColourChannel] = (aiColor4D[])UnmanagedAssimp.MarshalArray<aiColor4D>(tMesh.mColors[iColourChannel], tMesh.mNumVertices);
                else
                    tColors[iColourChannel] = null;
            }
            
            // Construct the faces array using the constructor of our high level wrapper to unpack it.
            int iStride = sizeof(UnmanagedAssimp.aiFace);
            IntPtr pPtr = tMesh.mFaces;

            tFaces      = new Face[tMesh.mNumFaces];
            for (int iFace = 0; iFace < tMesh.mNumFaces; ++iFace)
            {
                tFaces[iFace] = new Face(pPtr);
                pPtr = new IntPtr(pPtr.ToInt64() + iStride);
            }

            // Construct the bones array using the constructor of our high level wrapper to unpack it.
            iStride = sizeof(IntPtr);   // Because aiMesh defines mBones as a double pointer, our stride is IntPtr.
            pPtr    = tMesh.mBones;

            tBones  = new Bone[tMesh.mNumBones];
            for (int iBone = 0; iBone < tMesh.mNumBones; ++iBone)
            {
                tBones[iBone] = new Bone(Marshal.ReadIntPtr(pPtr));
                pPtr = new IntPtr(pPtr.ToInt64() + iStride);
            }
        }


        /// <summary>
        /// Check whether there are vertex positions in the model
        /// <code>getPosition()</code> asserts this.
        /// </summary>
        /// <returns>true if vertex positions are available. Currently this is guaranteed to be <code>true</code>.</returns>
	    public bool hasPositions()
        {
		    return tVertices != null;
	    }

        /// <summary>
        /// Check whether there are normal vectors in the model
        /// <code>getNormal()</code> asserts this.
        /// </summary>
        /// <returns>true if vertex normals are available.</returns>
	    public bool hasNormals()
        {
		    return tNormals != null;
	    }

        /// <summary>
        /// Check whether there are bones in the model. If the answer is
        /// <code>true</code>, use <code>getBone()</code> to query them.
        /// </summary>
        /// <returns>true if vertex normals are available.</returns>
	    public bool hasBones()
        {
		    return tBones != null;
	    }

        /// <summary>
        /// Check whether there are tangents/bitangents in the model If the answer is
        /// <code>true</code>, use <code>getTangent()</code> and <code>getBitangent()</code> to query them.
        /// </summary>
        /// <returns>true if vertex tangents and bitangents are available.</returns>
	    public bool hasTangentsAndBitangents()
        {
		    return (tTangents != null) && (tBitangents != null);
	    }

        /// <summary>
        /// Check whether a given UV set is existing the model <code>getUV()</code> will assert this.
        /// </summary>
        /// <param name="iIndex">UV coordinate set index</param>
        /// <returns>true the uv coordinate set is available.</returns>
	    public bool hasTextureCoords(int iIndex)
        {
		    return iIndex < this.tTextures.Length && tTextures[iIndex] != null;
	    }

        /// <summary>
        /// Check whether a given vertex color set is existing the model
        /// <code>getColor()</code> will assert this.
        /// </summary>
        /// <param name="iIndex">Vertex color set index</param>
        /// <returns>true the vertex color set is available.</returns>
        public bool hasVertexColors(int iIndex)
        {
            return iIndex < this.tColors.Length && tColors[iIndex] != null;
	    }

        /// <summary>
        /// Check whether there are faces in the model.  If the answer is
        /// <code>true</code>, use <code>getTangent()</code> and <code>getBitangent()</code> to query them.
        /// </summary>
        /// <returns>true if faces are available.</returns>
        public bool hasFaces()
        {
            return (tFaces != null) && (tFaces != null);
        }
		
		/// <summary>
		/// Get the name of this mesh.
		/// </summary>
		/// <returns>The name of this mesh. May be empty.</returns>
		public String getName()
		{
			return sName;
		}
		
        /// <summary>
        /// Get the number of vertices in the model.
        /// </summary>
        /// <returns>Number of vertices in the model. This could be 0 in some extreme cases although loaders should filter such cases out.</returns>
	    public int getNumVertices()
        {
		    return tVertices.Length;
	    }

        /// <summary>
        /// Get the number of faces in the model.
        /// </summary>
        /// <returns>Number of faces in the model. This could be 0 in some extreme cases although loaders should filter such cases out</returns>
	    public int getNumFaces()
        {
		    return tFaces.Length;
	    }

        /// <summary>
        /// Get the number of bones in the model
        /// </summary>
        /// <returns>Number of bones in the model.</returns>
	    public int getNumBones()
        {
		    return tBones.Length;
	    }

        /// <summary>
        /// Get the material index of the mesh.
        /// </summary>
        /// <returns>Zero-based index into the global material array.</returns>
	    public uint getMaterialIndex()
        {
		    return this.iMaterialIndex;
	    }

        /// <summary>
        /// Get a bitwise combination of all types of primitives which are present in the mesh.
        /// </summary>
	    public uint getPrimitiveTypes()
        {
		    return this.iPrimitiveTypes;
	    }

        /// <summary>
        /// Get a vertex position in the mesh.
        /// </summary>
        /// <param name="iIndex">Zero-based index of the vertex</param>
        /// <returns>An aiVector3D which contains our Normal.</returns>
	    public aiVector3D getPosition(int iIndex)
        {
		    return this.tVertices[iIndex];
	    }

        /// <summary>
        /// Provides direct access to the vertex position array of the mesh. This is
        /// the recommended way of accessing the data.
        /// </summary>
        /// <returns>Array of aiVector3D, size is numverts.</returns>
	    public aiVector3D[] getPositionArray()
        {
		    return this.tVertices;
	    }

        /// <summary>
        /// Get a vertex normal in the mesh.
        /// </summary>
        /// <param name="iIndex">Zero-based index of the vertex</param>
        /// <returns>An aiVector3D which contains our Normal.</returns>
	    public aiVector3D getNormal(int iIndex)
        {
		    return this.tNormals[iIndex];
	    }

        /// <summary>
        /// Provides direct access to the vertex normal array of the mesh. This is
        /// the recommended way of accessing the data.
        /// </summary>
        /// <returns>Array of aiVector3D, size is numverts.</returns>
	    public aiVector3D[] getNormalArray()
        {
		    return this.tNormals;
	    }

        /// <summary>
        /// Get a vertex tangent in the mesh.
        /// </summary>
        /// <param name="iIndex">Zero-based index of the vertex</param>
        /// <returns>An aiVector3D which contains our Tangent.</returns>
	    public aiVector3D getTangent(int iIndex)
        {
		    return this.tTangents[iIndex];
	    }

        /// <summary>
        /// Provides direct access to the vertex tangent array of the mesh. This is
        /// the recommended way of accessing the data.
        /// </summary>
        /// <returns>Array of aiVector3D, size is numverts.</returns>
	    public aiVector3D[] getTangentArray()
        {
		    return this.tTangents;
	    }

        /// <summary>
        /// Get a vertex bitangent in the mesh.
        /// </summary>
        /// <param name="iIndex">Zero-based index of the vertex</param>
        /// <returns>An aiVector3D which contains our BiTangent.</returns>
	    public aiVector3D getBitangent(int iIndex)
        {
		    return this.tBitangents[iIndex];
	    }

        /// <summary>
        /// Provides direct access to the vertex bitangent array of the mesh. This is
        /// the recommended way of accessing the data.
        /// </summary>
        /// <returns>Array of aiVector3D, size is numverts.</returns>
	    public aiVector3D[] getBiTangentArray()
        {
		    return this.tBitangents;
	    }

        /// <summary>
        /// Get a vertex texture coordinate in the mesh
        /// </summary>
        /// <param name="iChannel">Texture coordinate channel.</param>
        /// <param name="iIndex">Zero-based index of the vertex</param>
        /// <returns>An aiVector3D which contains our texture coordinate.</returns>
	    public aiVector3D getTextureCoordinate(int iChannel, int iIndex)
        {
            return tTextures[iChannel][iIndex];
	    }

        /// <summary>
        /// Provides direct access to a texture coordinate channel of the mesh This
        /// is the recommended way of accessing the data.
        /// </summary>
        /// <param name="iChannel">The texture coordinate channel.  Usually just the one.</param>
        /// <returns>Array of aiVector3D, size is numverts.</returns>
	    public aiVector3D[] getTextureCoordinateArray(int iChannel)
        {
		    return tTextures[iChannel];
	    }

        /// <summary>
        /// Get a vertex color as an unsigned integer in the mesh.
        /// </summary>
        /// <param name="channel">Vertex color channel.</param>
        /// <param name="iIndex">Zero-based index of the vertex.</param>
        /// <returns>Vertex color value packed an an integer in ARGB format.</returns>
	    public uint getVertexColorUint(int iChannel, int iIndex)
        {
            uint iOut = 0x00000000;
            aiColor4D tColour = tColors[iChannel][iIndex];
            iOut |= ((uint)tColour.a) << 24;
            iOut |= ((uint)tColour.r) << 16;
            iOut |= ((uint)tColour.g) << 8;
            iOut |= ((uint)tColour.b);

            // Return that integer.
            return iOut;
	    }

        /// <summary>
        /// Get a vertex color in the mesh.
        /// </summary>
        /// <param name="iChannel">Vertex color channel</param>
        /// <param name="iIndex">Zero-based index of the vertex.</param>
        /// <returns>An aiColour4D which contains our colour.</returns>
	    public aiColor4D getVertexColor(int iChannel, int iIndex)
        {
            return tColors[iChannel][iIndex];
	    }

        /// <summary>
        /// Provides direct access to the vertex bitangent array of the mesh This is
        /// the recommended way of accessing the data.
        /// </summary>
        /// <param name="iChannel">The colour chanel we want the array of.</param>
        /// <returns>Array of aiColor4D, size is the same as the number of verts.</returns>
	    public aiColor4D[] getVertexColorArray(int iChannel)
        {
		    return tColors[iChannel];
	    }

        /// <summary>
        /// Get a single face of the mesh.
        /// </summary>
        /// <param name="iIndex">Index of the face. Must be smaller than the value returned by <code>getNumFaces()</code></param>
	    public Face getFace(int iIndex)
        {
		    return this.tFaces[iIndex];
	    }

        /// <summary>
        /// Provides direct access to the face array of the mesh This is the
        /// recommended way of accessing the data.
        /// </summary>
        /// <returns>An array of faces which make up this mesh.</returns>
	    public Face[] getFaceArray()
        {
		    return this.tFaces;
	    }

        /// <summary>
        /// Provides access to the array of all bones influencing this mesh.
        /// </summary>
        /// <returns>Bone array</returns>
	    public Bone[] getBonesArray()
        {
		    return this.tBones;
	    }

        /// <summary>
        /// Get a bone influencing the mesh.
        /// </summary>
        /// <param name="i">Index of the bone.</param>
        /// <returns>Bone</returns>
	    public Bone getBone(int i)
        {
		    return this.tBones[i];
	    }
    }
}
