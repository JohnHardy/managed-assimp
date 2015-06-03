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
using System.Runtime.InteropServices;

using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.SampleFramework;

using Assimp;
using Assimp.ManagedAssimp;


namespace ManagedAssimpSample
{
    /// <summary>
    /// A very simple model class which can be used to render a material-sorted mesh.
    /// This is in no way safe or optimised for static rendering.  Write a better one :)
    /// </summary>
    public class SimpleModel
    {
        /// <summary>
        /// The vertex format for rendering this mesh.
        /// </summary>
        public struct PositionTextured
        {
            /// <summary>
            /// Vertex point.
            /// </summary>
            public Vector3 Position;

            /// <summary>
            /// Vertex texture coordinates.
            /// </summary>
            public Vector2 UV;
        }

        #region Attribute Flag
        /// <summary>
        /// An attribute entry corresponds to a subset of the mesh and specifies the block of memory in the vertex/index buffers where the geometry for the subset resides.
        /// </summary>
        public class Attribute
        {
            /// <summary>
            /// The subset ID.
            /// </summary>
            public int iAttributeID;

            /// <summary>
            /// An offset into the index buffer (FaceStart * 3) that identifies the start of the triangles that are ascociated with this subset.
            /// </summary>
            public int iIndexStart;

            /// <summary>
            /// The number of faces (triangles) in the subset.
            /// </summary>
            public int iFaceCount;

            /// <summary>
            /// An offset into the vertex buffer that identifies the start of the verticies that are associated with this subset.
            /// </summary>
            public int iVertexStart;

            /// <summary>
            /// The number of verticies in the subset.
            /// </summary>
            public int iVertexCount;
        }
        #endregion

        #region Assimp Import Flags
        /// <summary>
        /// Target post-processing steps for the standard flat tangent mesh.
        /// </summary>
        private const aiPostProcessSteps DefaultFlags = aiPostProcessSteps.CalcTangentSpace // calculate tangents and bitangents if possible
            | aiPostProcessSteps.JoinIdenticalVertices // join identical vertices/ optimize indexing
            | aiPostProcessSteps.ValidateDataStructure // perform a full validation of the loader's output
            | aiPostProcessSteps.ImproveCacheLocality // improve the cache locality of the output vertices
            | aiPostProcessSteps.RemoveRedundantMaterials // remove redundant materials
            | aiPostProcessSteps.FindDegenerates // remove degenerated polygons from the import
            | aiPostProcessSteps.FindInvalidData // detect invalid model data, such as invalid normal vectors
            | aiPostProcessSteps.GenUVCoords // convert spherical, cylindrical, box and planar mapping to proper UVs
            | aiPostProcessSteps.TransformUVCoords // preprocess UV transformations (scaling, translation ...)
            | aiPostProcessSteps.FindInstances // search for instanced meshes and remove them by references to one master
            | aiPostProcessSteps.LimitBoneWeights // limit bone weights to 4 per vertex
            | aiPostProcessSteps.OptimizeMeshes // join small meshes, if possible;
            // | aiPostProcessSteps.FixInfacingNormals
            | aiPostProcessSteps.GenSmoothNormals // generate smooth normal vectors if not existing
            | aiPostProcessSteps.SplitLargeMeshes // split large, unrenderable meshes into submeshes
            | aiPostProcessSteps.Triangulate // triangulate polygons with more than 3 edges
            | aiPostProcessSteps.SortByPType // make 'clean' meshes which consist of a single typ of primitives
            | aiPostProcessSteps.PreTransformVertices // bake transforms, fixes most errors for Xna
            | aiPostProcessSteps.FlipUVs  // common DirectX issue (Xna also)

            | aiPostProcessSteps.MakeLeftHanded  // Makes the model import the right way round (not flipped left to right).
            | aiPostProcessSteps.FlipWindingOrder
            ;
        #endregion

        /// <summary>
        /// The vertex buffer for this mesh.  You probably want an GPU memory one.
        /// </summary>
        private PositionTextured[] tVertex;

        /// <summary>
        /// The index buffer for this mesh.
        /// </summary>
        private UInt32[] tIndex;

        /// <summary>
        /// The 'attribute buffer' which says where the materials point to in the mesh.  Note that this is *not* the same.
        /// </summary>
        private Attribute[] tAttibutes;

        /// <summary>
        /// The material buffer which stores the material names - as provided by assimp.
        /// </summary>
        private String[] tMaterials;
        
        /// <summary>
        /// The texture array.
        /// </summary>
        private SlimDX.Direct3D9.Texture[] tTextures;

        /// <summary>
        /// The radius of this mesh.
        /// </summary>
        public float Radius { get; private set; }

        /// <summary>
        /// The material count for unnamed material sorting.
        /// </summary>
        public static int iNextMaterial = 1;

        /// <summary>
        /// Construct a model class.
        /// </summary>
        /// <param name="sPath"></param>
        public SimpleModel(String sPath)
        {
            #region Material Sorted Mesh Import
            // Get the texture path.
            sPath = sPath.Replace('\\', '/');
            int iIndex = sPath.LastIndexOf("/");
            String sTexturePath = sPath.Substring(0, iIndex);
            Console.WriteLine("Texture Path = " + sTexturePath);

            // Import the file.
            Scene pScene = AssimpImporter.readFile(sPath, DefaultFlags);

            // If we had an error then throw an exception.
            if (pScene == null)
                throw new Exception(AssimpImporter.getErrorString());

            // Order the meshes into a material-mesh array table.
            Dictionary<String, List<Assimp.ManagedAssimp.Mesh>> dTable = new Dictionary<String, List<Assimp.ManagedAssimp.Mesh>>();

            // Sort the meshes into material (i.e. attribute order).
            int iVBufferSize = 0;
            int iIBufferSize = 0;
            foreach (Assimp.ManagedAssimp.Mesh pMesh in pScene.getMeshes())
            {
                // Check we have the appropriate data.
                //if (!(pMesh.hasPositions() & pMesh.hasNormals() & pMesh.hasTangentsAndBitangents() & pMesh.hasTextureCoords(0) & pMesh.hasFaces()))
                //    throw new Exception("Cannot import mesh. Required all, found: Positions=" + pMesh.hasPositions() + ", Normals=" + pMesh.hasNormals() + ", TangentsAndBitangents=" + pMesh.hasTangentsAndBitangents() + ", TextureCoords=" + pMesh.hasTextureCoords(0) + ", Faces=" + pMesh.hasFaces() + ".");

                // Convert the Assimp material to an Evolved material.
                Assimp.ManagedAssimp.Material pMat = pScene.getMaterial((int)pMesh.getMaterialIndex());
                String sMaterial = pMat.getStringProperty("$tex.file");
                if (sMaterial == null)
                    sMaterial += "UnboundMaterial_" + (iNextMaterial++);
                else
                    sMaterial = sTexturePath + "\\" + sMaterial;
 
                // Normalise the path address type.
                sMaterial = sMaterial.Replace('/', '\\');

                // If this is the first entry - add to the list.
                List<Assimp.ManagedAssimp.Mesh> lMeshes = null;
                if (!dTable.TryGetValue(sMaterial, out lMeshes))
                {
                    lMeshes = new List<Assimp.ManagedAssimp.Mesh>();
                    dTable[sMaterial] = lMeshes;
                }

                // Work out the max vertex and face count.
                iVBufferSize += (int)pMesh.getNumVertices();
                iIBufferSize += (int)pMesh.getNumFaces() * 3;

                // Insert the mesh under the appropriate material.
                lMeshes.Add(pMesh);
            }

            // Create the buffers and reset the stats.
            tVertex = new PositionTextured[iVBufferSize];
            tIndex = new UInt32[iIBufferSize];
            tAttibutes = new Attribute[dTable.Count];
            tMaterials = new String[dTable.Count];
            tTextures = new SlimDX.Direct3D9.Texture[dTable.Count];
            dTable.Keys.CopyTo(tMaterials, 0);

            Radius = 0f;

            // Monitor global poisition in the vertex, index and attribute buffers.
            int iAttribute = 0;
            int iBVertex = 0;
            int iBIndex = 0;

            // For each material.
            foreach (String sMaterial in dTable.Keys)
            {
                // Create a new attribute.
                Attribute pAttrib = new Attribute();
                tAttibutes[iAttribute] = pAttrib;

                // Store the buffer offsets into the attribute.
                pAttrib.iAttributeID = iAttribute;
                pAttrib.iVertexStart = iBVertex;
                pAttrib.iIndexStart = iBIndex;

                int iAVertex = 0;
                int iAFace = 0;

                // Copy the vertices and indicies of each mesh (updating the buffer counts).
                foreach (Assimp.ManagedAssimp.Mesh pMesh in dTable[sMaterial])
                {
                    // Copy verticies.
                    int iMeshVerts = (int)pMesh.getNumVertices();
                    for (int iVertex = 0; iVertex < iMeshVerts; ++iVertex)
                    {
                        // Create the vertex.
                        PositionTextured pVertex = new PositionTextured();

                        // Copy data.
                        pVertex.Position = toDX9(pMesh.getPosition(iVertex));
                        pVertex.UV = toDX9UV(pMesh.getTextureCoordinate(0, iVertex));

                        // Update the radius.
                        Radius = Math.Max(pVertex.Position.Length(), Radius);

                        // Store.
                        tVertex[iVertex + iBVertex] = pVertex;
                    }

                    // Increment the vertex count by the number of verticies we just looped over.
                    iAVertex += iMeshVerts;
                    iBVertex += iMeshVerts;

                    // Copy indicies.
                    int iMeshFaces = (int)pMesh.getNumFaces();
                    for (int iFace = 0; iFace < iMeshFaces; ++iFace)
                    {
                        uint[] tIndices = pMesh.getFace(iFace).getIndices();
                        if (tIndices.Length != 3)
                            throw new Exception("Cannot load a mesh which does not have only triangluar faces.");

                        tIndex[iBIndex++] = tIndices[0];
                        tIndex[iBIndex++] = tIndices[1];
                        tIndex[iBIndex++] = tIndices[2];
                    }

                    // Increment the face count by the number of faces we just looped over.
                    iAFace += iMeshFaces;
                }

                // Save the sizes.
                pAttrib.iFaceCount = iAFace;
                pAttrib.iVertexCount = iAVertex;

                // Increment the attribute counter.
                ++iAttribute;
            }
            #endregion
        }

        /// <summary>
        /// Reload the resources required for this mesh.
        /// </summary>
        /// <param name="pDevice">The graphics device we want to store the textures onto.</param>
        public void reloadTextures(Device pDevice)
        {
            // Try to load the textures from the instructions in the mesh file.
            for (int iAttr = 0; iAttr < tMaterials.Length; ++iAttr)
            {
                try
                {
                    tTextures[iAttr] = SlimDX.Direct3D9.Texture.FromFile(pDevice, tMaterials[iAttr]);
                    Console.WriteLine("Loading texture: " + tMaterials[iAttr]);
                }
                catch (Exception pError)
                {
                    Console.WriteLine("Could not load texture for '"+tMaterials[iAttr]+"'.  Error = " + pError.Message);
                    tTextures[iAttr] = null;
                }
            }
        }

        /// <summary>
        /// Free the texture resources used by this mesh.
        /// </summary>
        public void freeTextures()
        {
            foreach (SlimDX.Direct3D9.Texture pTexture in tTextures)
            {
                if (pTexture != null)
                    pTexture.Dispose();
            }
            tTextures = new SlimDX.Direct3D9.Texture[tTextures.Length];
        }

        /// <summary>
        /// Draw this model using a render context.
        /// </summary>
        /// <param name="pDevice">The render device to draw too.</param>
        public void drawMesh(Device pDevice)
        {
            // Compute the vertex stride.
            int iSize = Marshal.SizeOf(typeof(PositionTextured));

            // For each attribute.
            for (int iAttr = 0; iAttr < tAttibutes.Length; ++iAttr)
            {
                // Reference the attribute.
                Attribute pAttr = tAttibutes[iAttr];

                // Push it's material.
                pDevice.SetTexture(0, tTextures[iAttr]);

                // Draw that part of the vertex buffer.
                pDevice.DrawIndexedUserPrimitives<UInt32, PositionTextured>(PrimitiveType.TriangleList, pAttr.iVertexStart, pAttr.iVertexCount, pAttr.iFaceCount, tIndex, Format.Index32, tVertex, iSize);
            }
        }

        #region Helpers
        /// <summary>
        /// Convert an Assimp aiColour4D into an integer.
        /// </summary>
        /// <param name="vValue">The assimp value.</param>
        /// <returns>The equivilant DX value.</returns>
        public static int toDX9(aiColor4D vValue)
        {
            Color4 pColour = new Color4(vValue.a, vValue.r, vValue.g, vValue.b);
            return pColour.ToArgb();
        }

        /// <summary>
        /// Convert an Assimp aiVector3D into a DirectX Vector3D.
        /// </summary>
        /// <param name="vValue">The assimp value.</param>
        /// <param name="vOut">The DirectX value.</param>
        public static Vector3 toDX9(aiVector3D vValue)
        {
            return new Vector3(vValue.x, vValue.y, vValue.z);
        }

        /// <summary>
        /// Convert an Assimp aiVector3D into a DirectX Vector3D.
        /// </summary>
        /// <param name="vValue">The assimp value.</param>
        /// <param name="vOut">The DirectX value.</param>
        public static void toDX9(aiVector3D vValue, out Vector3 vOut)
        {
            vOut.X = vValue.x;
            vOut.Y = vValue.y;
            vOut.Z = vValue.z;
        }

        /// <summary>
        /// Convert an Assimp aiVector3D into a DirectX Vector2.
        /// </summary>
        /// <param name="vValue">The assimp value.</param>
        /// <returns>The equivilant DirectX value.</returns>
        public static Vector2 toDX9UV(aiVector3D vValue)
        {
            return new Vector2(vValue.x, vValue.y);
        }
        #endregion
    }
}
