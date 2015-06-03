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
    /// Represens the asset data that has been loaded. A scene consists of multiple meshes, animations, materials and embedded textures.  It defines the 'scene graph' of the asset (the hierarchy of all meshes, bones, ...).
    /// An instance of this class is returned by <code>Importer.readFile()</code>.
    /// </summary>
    /// <author>John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk</author>
    /// <date>20 July 2009</date>
    [Serializable]
    public class Scene
    {
        #region Properties
        /// <summary>
        /// Any combination of the AI_SCENE_FLAGS_XXX flags. By default this value is 0, no flags are set. Most applications will want to reject all scenes with the AI_SCENE_FLAGS_INCOMPLETE bit set.
        /// </summary>
        private uint eFlags = 0x00;

        /// <summary>
        /// The root node of the hierarchy.  "Node* mRootNode" An IntPtr to the Node structure which is at the start of the hierarchy.  There will always be at least the root node if the import was successful (and no special flags have been set). Presence of further nodes depends on the format and content of the imported file.
        /// </summary>
        private Node pRootNode = null;

        /// <summary>
        /// An array of meshes. "aiMesh** mMeshes" An IntPtr to a pointer to an array of meshes.  Use the indices given in the aiNode structure to access this array. The array is mNumMeshes in size. If the AI_SCENE_FLAGS_INCOMPLETE flag is not set there will always be at least ONE material.
        /// </summary>
        private Mesh[] tMeshes = null;

        /// <summary>
        /// The array of materials. "aiMaterial** mMaterials" An IntPtr to a pointer to an array of materials.  Use the index given in each aiMesh structure to access this array. The array is mNumMaterials in size. If the AI_SCENE_FLAGS_INCOMPLETE flag is not set there will always be at least ONE material.
        /// </summary>
        private Material[] tMaterials = null;

        /// <summary>
        /// The array of animations.  "Animation** mAnimations" An IntPtr to a pointer to an animation..  All animations imported frmo the given file are listed here.  The array is mNumAnimations in size.
        /// </summary>
        private Animation[] tAnimations = null;

        /// <summary>
        /// The array of embedded textures.  "Texture** mTextures" An IntPtr to a pointer to a texture.  Not many file formats embed their textures into the file. An example is Quake's MDL format (which is also used by some GameStudio versions). 
        /// </summary>
        private Texture[] tTextures = null;

        /// <summary>
        /// The array of light sources.  "Light** mLights" All light sources imported from the given file are listed here. The array is mNumLights in size.
        /// </summary>
        private Light[] tLights = null;

        /// <summary>
        /// The array of cameras. "Camera** mCameras" An IntPtr to a pointer to a camera.  All cameras imported from the given file are listed here.  The array is mNumCameras in size. The first camera in the array (if existing) is the default camera view into the scene.
        /// </summary>
        private Camera[] tCameras = null;

        /// <summary>
        /// Store a structure which contains memory information about this scene.
        /// </summary>
        private aiMemoryInfo tMemoryInfo;

        /// <summary>
        /// The filename from which we were loaded.
        /// </summary>
        private String sFileName         = null;
        #endregion

        /// <summary>
        /// Constructor which builds a Scene object from a pointer to an aiScene structure.  As a user, you should not call this by hand.
        /// <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
        /// </summary>
        /// <param name="aiScene*">A pointer to the aiScene structure in the low level unmanaged wrapper.</param>
        /// <param name="sFileName">The filename from which we are loaded.  Just for posterity purposes.</param>
        unsafe internal Scene(IntPtr p_aiScene, String sFileName)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiScene tScene = (UnmanagedAssimp.aiScene)Marshal.PtrToStructure(p_aiScene, typeof(UnmanagedAssimp.aiScene));

            // Using our scene, get the memory information taken up.
            UnmanagedAssimp.aiGetMemoryRequirements((UnmanagedAssimp.aiScene*)p_aiScene.ToPointer(), out tMemoryInfo);

            // Filename.
            this.sFileName      = sFileName;

            // Define a couple of variables that we will use to do the iteration.
            int iStride         = sizeof(IntPtr);
            IntPtr pIterator    = IntPtr.Zero;

            // Copy nice simple value types into managed memory.
            this.eFlags = tScene.mFlags;

            // Create the root node.  It has a null parent because it is the root node!
            // It's important to note that by calling this - all the nodes in the heirarchy will self-create and link recursively.
            // It will NOT load any of the meshs or data into managed memory.
            pRootNode = new Node(tScene.mRootNode, null);
            
            // MESH
            // Now we need to load each mesh into managed memory.
            //Console.WriteLine("<Mesh Loading>");
            tMeshes     = new Mesh[tScene.mNumMeshes];
            pIterator   = tScene.mMeshes;
            for (int iCount = 0; iCount < tScene.mNumMeshes; ++iCount)
            {
                tMeshes[iCount] = new Mesh(Marshal.ReadIntPtr(pIterator));
                pIterator = new IntPtr(pIterator.ToInt64() + iStride);
            }
            //Console.WriteLine("</Mesh Loading>");

            // MATERIAL
            //Console.WriteLine("<Material Loading>");
            tMaterials = new Material[tScene.mNumMaterials];
            pIterator = tScene.mMaterials;
            for (int iCount = 0; iCount < tScene.mNumMaterials; ++iCount)
            {
                tMaterials[iCount] = new Material(Marshal.ReadIntPtr(pIterator));
                pIterator = new IntPtr(pIterator.ToInt64() + iStride);
            }
            //Console.WriteLine("</Material Loading>");

            // ANIMATION.
            //Console.WriteLine("<Animation Loading>");
            tAnimations = new Animation[tScene.mNumAnimations];
            pIterator   = tScene.mAnimations;
            for (int iCount = 0; iCount < tScene.mNumAnimations; ++iCount)
            {
                tAnimations[iCount] = new Animation(Marshal.ReadIntPtr(pIterator));
                pIterator = new IntPtr(pIterator.ToInt64() + iStride);
            }
            //Console.WriteLine("</Animation Loading>");

            // EMBEDDED TEXTURES.
            //Console.WriteLine("<Embedded Texture Loading>");
            tTextures = new Texture[tScene.mNumTextures];
            pIterator = tScene.mTextures;
            for (int iCount = 0; iCount < tScene.mNumTextures; ++iCount)
            {
                tTextures[iCount] = new Texture(Marshal.ReadIntPtr(pIterator));
                pIterator = new IntPtr(pIterator.ToInt64() + iStride);
            }
            //Console.WriteLine("</Embedded Texture Loading>");

            // LIGHTS.
            //Console.WriteLine("<Light Loading>");
            tLights     = new Light[tScene.mNumLights];
            pIterator   = tScene.mLights;
            for (int iCount = 0; iCount < tScene.mNumLights; ++iCount)
            {
                tLights[iCount] = new Light(Marshal.ReadIntPtr(pIterator));
                pIterator = new IntPtr(pIterator.ToInt64() + iStride);
            }
            //Console.WriteLine("</Light Loading>");

            // CAMERAS.
            //Console.WriteLine("<Camera Loading>");
            tCameras    = new Camera[tScene.mNumCameras];
            pIterator   = tScene.mCameras;
            for (int iCount = 0; iCount < tScene.mNumCameras; ++iCount)
            {
                tCameras[iCount] = new Camera(Marshal.ReadIntPtr(pIterator));
                pIterator = new IntPtr(pIterator.ToInt64() + iStride);
            }
            //Console.WriteLine("</Camera Loading>");
        }

        /// <summary>
        /// Returns a string representation of this class.
        /// </summary>
        /// <returns>A string which contains useful information about this scenegraph.</returns>
        public override string ToString()
        {
             return "Scene(size='"+this.tMemoryInfo.total/1024.0+" KB' meshes='"+getNumMeshes()+"' materials='"+getNumMaterials()+"' animations='"+getNumAnimations()+"' embeddedtextures='"+getNumTextures()+"' lights='"+getNumLights()+"' cameras='"+getNumCameras()+"' )";
        }

        /// <summary>
        /// Get the filename from which this resource was loaded.  This is only present for debugging / posterity purposes.
        /// </summary>
        /// <returns> A string file name (sometimes relative to the working directory which this assimp instance was executed).</returns>
        public String getLoadedFileName()
        {
            return sFileName;
        }

        /// <summary>
        /// Get the scene flags. This can be any combination of the FLAG_XXX constants.
        /// </summary>
        /// <returns>Scene flags.</returns>
        public uint getFlags()
        {
            return eFlags;
        }

        /// <summary>
        /// Get the root node of the scene graph.  This describes the heirarchy of the assets within that we have just imported.
        /// </summary>
        /// <returns>Root node.</returns>
        public Node getRootNode()
        {
            return pRootNode;
        }

        /// <summary>
        /// Get a reference to the memory infromation structure.
        /// </summary>
        /// <returns>A reference to the memory information structure.</returns>
        public aiMemoryInfo getMemoryInformation()
        {
            return tMemoryInfo;
        }

        /// <summary>
        /// Save this
        /// </summary>

        #region Meshes
        /// <summary>
        /// Get the mesh list of the scene.
        /// </summary>
        /// <returns>mesh list</returns>
        public Mesh[] getMeshes()
        {
            return tMeshes;
        }

        /// <summary>
        /// Get the number of meshes in the scene
        /// </summary>
        /// <returns>this value can be 0 if the <code>ANIMATION_SKELETON_ONLY</code></returns>
        /// flag is set.
        public int getNumMeshes()
        {
            return (tMeshes == null) ? 0 : tMeshes.Length;
        }

        /// <summary>
        /// Get a mesh from the scene.
        /// </summary>
        /// <param name="iIndex">Index of the mesh</param>
        /// <returns>scene.mesh[i]</returns>
        public Mesh getMesh(int iIndex)
        {
            return tMeshes[iIndex];
        }
        #endregion

        #region Embedded Textures
        /// <summary>
        /// Get the texture list.
        /// </summary>
        /// <returns>Texture list</returns>
        public Texture[] getTextures()
        {
            return tTextures;
        }

        /// <summary>
        /// Get the number of embedded textures in the scene.
        /// </summary>
        /// <returns>the number of embedded textures in the scene, usually 0.</returns>
        public int getNumTextures()
        {
            return (tTextures == null) ? 0 : tTextures.Length;
        }

        /// <summary>
        /// Get an embedded texture from the scene.
        /// </summary>
        /// <param name="iIndex">Index of the textures.</param>
        /// <returns>scene.texture[i]</returns>
        public Texture getTexture(int iIndex)
        {
            return tTextures[iIndex];
        }
        #endregion

        #region Materials
        /// <summary>
        /// Get the material list for the scene.
        /// </summary>
        /// <returns>Material list.</returns>
        public Material[] getMaterials()
        {
            return tMaterials;
        }

        /// <summary>
        /// Get the number of materials in the scene.
        /// </summary>
        /// <returns>the number of materials in the scene.</returns>
        public int getNumMaterials()
        {
            return (tMaterials == null) ? 0 : tMaterials.Length;
        }

        /// <summary>
        /// Get a material from the scene.
        /// </summary>
        /// <param name="iIndex">Index of the materials.</param>
        /// <returns>scene.material[i]</returns>
        public Material getMaterial(int iIndex)
        {
            return tMaterials[iIndex];
        }
        #endregion

        #region Animations
        /// <summary>
        /// Get the number of animations in the scene.
        /// </summary>
        /// <returns>this value could be 0, most models have no animations.</returns>
        public int getNumAnimations()
        {
            return (tAnimations == null) ? 0 : tAnimations.Length;
        }

        /// <summary>
        /// Get the animation list for the scene.
        /// </summary>
        /// <returns>Animation list</returns>
        public Animation[] getAnimations()
        {
            return tAnimations;
        }

        /// <summary>
        /// Get an animation from the scene.
        /// </summary>
        /// <param name="iIndex">Index of the animations.</param>
        /// <returns>scene.material[i]</returns>
        public Animation getAnimation(int iIndex)
        {
            return tAnimations[iIndex];
        }
        #endregion

        #region Cameras
        /// <summary>
        /// Get the number of cameras in the scene.
        /// </summary>
        /// <returns>this value could be 0, most models have no cameras.</returns>
        public int getNumCameras()
        {
            return (tCameras == null) ? 0 : tCameras.Length;
        }

        /// <summary>
        /// Get the cameras list for the scene.
        /// </summary>
        /// <returns>Cameras list.</returns>
        public Camera[] getCameras()
        {
            return tCameras;
        }

        /// <summary>
        /// Get a camera from the scene.
        /// </summary>
        /// <param name="iIndex">Index of the camera.</param>
        /// <returns>scene.camera[i]</returns>
        public Camera getCamera(int iIndex)
        {
            return tCameras[iIndex];
        }
        #endregion

        #region Lights
        /// <summary>
        /// Get the number of lights in the scene.
        /// </summary>
        /// <returns>this value could be 0, most models have no lights.</returns>
        public int getNumLights()
        {
            return (tLights == null) ? 0 : tLights.Length;
        }

        /// <summary>
        /// Get the lights list for the scene.
        /// </summary>
        /// <returns>lights list.</returns>
        public Light[] getLights()
        {
            return tLights;
        }

        /// <summary>
        /// Get a light from the scene.
        /// </summary>
        /// <param name="iIndex">Index of the light.</param>
        /// <returns>scene.light[i]</returns>
        public Light getLight(int iIndex)
        {
            return tLights[iIndex];
        }
        #endregion
    }
}
