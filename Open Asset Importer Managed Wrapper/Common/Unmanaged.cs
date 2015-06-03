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
using System.Security;

using System.Text;


namespace Assimp.ManagedAssimp.Unmanaged
{

    /// <summary>
    /// Provides raw access to unmanged structures inside of assimp which are not useful in a managed context.
    /// As an application developer, you probably won't need to work with this.  However you may choose to access the raw assimp
    /// functionality (perhaps you might not want the overhead of copying into managed memory) and you can do so through this API.
    /// </summary>
    /// <author>John Hardy</author>
    /// <date>20 July 2009</date>
    public static unsafe class UnmanagedAssimp
    {
        #region Helper Functions
        /// <summary>
        /// This method will marshall an array of type T from unmanaged memory into a managed array.
        /// T must at least be a struct.
        /// </summary>
        /// <param name="pPointer">A pointer to the element at the start of the unmanaged array.</param>
        /// <param name="iLength">The number of unmanaged elements in the array.</param>
        /// <returns>A managed array with (shallow) copied contents of the unmanaged array.</returns>
        public static unsafe T[] MarshalArray<T>(IntPtr pPointer, uint iLength) where T : struct
        {
            // Check our pointer points to something.
            if (pPointer == IntPtr.Zero)
                return null;

            // New managed array that we will fill and return.
            T[] tManagedArray   = new T[iLength];
            Type kType          = typeof(T);
            int iSize           = Marshal.SizeOf(kType);

            // Debug info.
            //Console.WriteLine("Marshalling array of '"+iLength+"' elements of type '"+kType.Name+"' with structure size of '"+iSize+"'.");

            // For iLength elements of size(T) copy them into the array.
            for (int iElement = 0; iElement < iLength; ++iElement)
            {
                // Copy the element at this index.
                //Console.WriteLine("on line " + iElement + " and ptr = " + pPointer.ToInt32());
                tManagedArray[iElement] = (T)Marshal.PtrToStructure(pPointer, kType);
                
                // Increment the pointer.
                pPointer = new IntPtr(pPointer.ToInt64() + iSize);
            }

            // Return the reference to the managed array.
            return tManagedArray;
        }

        /// <summary>
        /// This method will marshall an array of type int from unmanaged memory into a managed array.
        /// The integer refers to a 32 bit signed integer.
        /// </summary>
        /// <param name="pPointer">A pointer to the element at the start of the unmanaged array.</param>
        /// <param name="iLength">The number of unmanaged elements in the array.</param>
        /// <returns>A managed array with (shallow) copied contents of the unmanaged array.</returns>
        public static unsafe int[] MarshalIntArray(IntPtr pPointer, uint iLength)
        {
            // Check our pointer points to something.
            if (pPointer == IntPtr.Zero)
                return null;

            // New managed array that we will fill and return.
            int[] tManagedArray = new int[iLength];
            Type kType  = typeof(int);
            int iSize   = sizeof(int);

            // Debug info.
            //Console.WriteLine("Marshalling int array of '" + iLength + "' elements of type '" + kType.Name + "' with structure size of '" + iSize + "'.");

            // For iLength elements of size(int) copy them into the array.
            for (int iElement = 0; iElement < iLength; ++iElement)
            {
                // Copy the element at this index.
                tManagedArray[iElement] = (int)Marshal.ReadInt32(pPointer);

                // Increment the pointer.
                pPointer = new IntPtr(pPointer.ToInt64() + iSize);
            }

            // Return the reference to the managed array.
            return tManagedArray;
        }

        /// <summary>
        /// This method will marshall an array of type unit from unmanaged memory into a managed array.
        /// The integer refers to a 32 bit unsigned integer.
        /// </summary>
        /// <param name="pPointer">A pointer to the element at the start of the unmanaged array.</param>
        /// <param name="iLength">The number of unmanaged elements in the array.</param>
        /// <returns>A managed array with (shallow) copied contents of the unmanaged array.</returns>
        public static unsafe uint[] MarshalUintArray(IntPtr pPointer, uint iLength)
        {
            // Check our pointer points to something.
            if (pPointer == IntPtr.Zero)
                return null;

            // New managed array that we will fill and return.
            uint[] tManagedArray = new uint[iLength];
            Type kType  = typeof(uint);
            int iSize   = sizeof(uint);

            // Debug info.
            //Console.WriteLine("Marshalling uint array of '" + iLength + "' elements of type '" + kType.Name + "' with structure size of '" + iSize + "'.");

            // For iLength elements of size(int) copy them into the array.
            for (uint iElement = 0; iElement < iLength; ++iElement)
            {
                // Copy the element at this index.
                tManagedArray[iElement] = (uint)Marshal.ReadInt32(pPointer);

                // Increment the pointer.
                pPointer = new IntPtr(pPointer.ToInt64() + iSize);
            }

            // Return the reference to the managed array.
            return tManagedArray;
        }
        #endregion

        #region Essential unmanaged DLL calls.
        /*
        /// Return Type: aiScene*
        ///pFile: char*
        ///pFlags: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "aiImportFile")]
        public static extern System.IntPtr aiImportFile([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pFile, uint pFlags);
        */

        /// Return Type: void
        ///szName: char*
        ///value: int
        [System.Runtime.InteropServices.DllImportAttribute(AssimpImporter.DLLName, EntryPoint = "aiSetImportPropertyInteger")]
        public static extern void aiSetImportPropertyInteger([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string szName, int value);


        /// Return Type: void
        ///szName: char*
        ///value: float
        [System.Runtime.InteropServices.DllImportAttribute(AssimpImporter.DLLName, EntryPoint = "aiSetImportPropertyFloat")]
        public static extern void aiSetImportPropertyFloat([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string szName, float value);


        /// <summary>
        /// Reads the given file and returns its content.
        /// If the call succeeds, the imported data is returned in an aiScene structure.
        /// The data is intended to be read-only, it stays property of the ASSIMP library and will be stable until aiReleaseImport() is called.
        /// After you're done with it, call aiReleaseImport() to free the resources associated with this file. If the import fails,
        /// NULL is returned instead.
        /// Call aiGetErrorString() to retrieve a human-readable error text.
        /// </summary>
        /// <param name="sFile">An IntPtr to the character array (8bit - NOT UNICODE) which contains the path to our file.</param>
        /// <param name="eFlags">Optional post processing steps to be executed after a successful import. Provide a bitwise combination of the aiPostProcessSteps flags.</param>
        /// <returns>Pointer to the imported data or NULL if the import failed.</returns>
        [DllImport(AssimpImporter.DLLName), SuppressUnmanagedCodeSecurityAttribute]
        public static extern aiScene* aiImportFile([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]string sFile, uint eFlags);

        // @param pFile Path and filename of the file to be imported, expected to be a null-terminated c-string. NULL is not a valid value.
        // @param pFlags Optional post processing steps to be executed after a successful import. Provide a bitwise combination of the aiPostProcessSteps flags.
        //ASSIMP_API const C_STRUCT aiScene* aiImportFile( const char* pFile, unsigned int pFlags);

        /// <summary>
        /// Releases all resources associated with the given import process.
        /// Call this function after you're done with the imported data.
        /// </summary>
        /// <param name="pScene">An IntPtr to the imported data to release. NULL is a valid value.</param>
        [DllImport(AssimpImporter.DLLName), SuppressUnmanagedCodeSecurityAttribute]
        public static extern void aiReleaseImport(aiScene* pScene);
        //ASSIMP_API void aiReleaseImport( const C_STRUCT aiScene* pScene);

        /// <summary>
        /// Returns the error text of the last failed import process.
        /// </summary>
        /// <returns>A textual description of the error that occurred at the last import process. NULL if there was no error.</returns>
        [DllImport(AssimpImporter.DLLName), SuppressUnmanagedCodeSecurityAttribute]
        public static extern sbyte* aiGetErrorString();
        //ASSIMP_API const char* aiGetErrorString();

        /// <summary>
        /// Returns whether a given file extension is supported by ASSIMP
        /// </summary>
        /// <param name="sExtension">Extension for which the function queries support.  Must include a leading dot '.'. Example: ".3ds", ".md3"</param>
        /// <returns>1 if the extension is supported, 0 otherwise.</returns>
        [DllImport(AssimpImporter.DLLName), SuppressUnmanagedCodeSecurityAttribute]
        public static extern int aiIsExtensionSupported(byte* sExtension);
        //ASSIMP_API int aiIsExtensionSupported(const char* szExtension);

        /// <summary>
        /// Get a full list of all file extensions generally supported by ASSIMP.
        /// If a file extension is contained in the list this does, of course, not mean that ASSIMP is able to load all files with this extension.
        /// </summary>
        /// <param name="tOut">aiString to receive the extension list. Format of the list: "*.3ds;*.obj;*.dae". NULL is not a valid parameter.</param>
        [DllImport(AssimpImporter.DLLName), SuppressUnmanagedCodeSecurityAttribute]
        public static extern int aiGetExtensionList(out aiString tOut);
        //ASSIMP_API void aiGetExtensionList(C_STRUCT aiString* szOut);

        /// <summary>
        /// Get the storage required by an imported asset.
        /// </summary>
        /// <param name="pScene">Input asset.</param>
        /// <param name="tIn">Data structure to be filled.</param>
        [DllImport(AssimpImporter.DLLName), SuppressUnmanagedCodeSecurityAttribute]
        public static extern void aiGetMemoryRequirements(aiScene* pScene, out aiMemoryInfo tIn);
        //ASSIMP_API void aiGetMemoryRequirements(const C_STRUCT aiScene* pIn, C_STRUCT aiMemoryInfo* in);

        /// <summary>
        /// Set a string property. This is the C-version of Assimp::Importer::SetPropertyString().
        /// In the C-API properties are shared by all imports. It is not possible to specify them per asset.
        /// </summary>
        /// <param name="szName">Name of the configuration property to be set. All constants are defined in the aiConfig.h header file.</param>
        /// <param name="value">New value for the property.</param>
        [DllImport(AssimpImporter.DLLName), SuppressUnmanagedCodeSecurityAttribute]
        public static extern void aiSetImportPropertyString(IntPtr szName, out aiString mRotation);
        //ASSIMP_API void aiSetImportPropertyString(const char* szName, const C_STRUCT aiString* st);

        /// <summary>
        /// Build a quaternion from a matrix.
        /// </summary>
        /// <param name="qQuaternion">A pointer to the quaternion that contains the rotation.</param>
        /// <param name="mRotation">The output rotation matrix.</param>
        [DllImport(AssimpImporter.DLLName), SuppressUnmanagedCodeSecurityAttribute]
        public static extern void aiCreateQuaternionFromMatrix(out aiQuaternion qQuaternion, ref aiMatrix3x3 mRotation);
        //ASSIMP_API void aiCreateQuaternionFromMatrix(C_STRUCT aiQuaternion* quat,const C_STRUCT aiMatrix3x3* mat);

        /// <summary>
        /// Decompose a 4x4 row-major transform matrix into its three property matracies.
        /// </summary>
        /// <param name="mTransform">A pointer to the transform matrix.</param>
        /// <param name="vScale">A pointer to the aiVector3D we want to contain the scale.</param>
        /// <param name="qQuaternion">A pointer to the quaternion we want to contain the rotation.</param>
        /// <param name="vTranslate">A pointer to the vector we want to contain the translate.</param>
        [DllImport(AssimpImporter.DLLName), SuppressUnmanagedCodeSecurityAttribute]
        public static extern void aiDecomposeMatrix(ref aiMatrix4x4 mTransform, out aiVector3D vScale, out aiQuaternion qQuaternion, out aiVector3D vTranslate);
        //ASSIMP_API void aiDecomposeMatrix(const C_STRUCT aiMatrix4x4* mat, C_STRUCT aiVector3D* scaling, C_STRUCT aiQuaternion* rotation, C_STRUCT aiVector3D* position);
        #endregion

        #region Face Structure
        /// <summary>
        /// A structure that represents a face in a mesh. See 'aiMesh.h' for details.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiFace
        {
            /// <summary>
            /// Number of indices defining this face. 3 for a triangle, >3 for polygon.
            /// </summary>
            public uint mNumIndices;

            /// <summary>
            /// Pointer to the indices array. Size of the array is given in numIndices.
            /// </summary>
            public IntPtr mIndices;
        }
        #endregion

        #region Node Structure
        /// <summary>
        /// A node in the imported hierarchy. 
        /// <p>Each node has name, a parent node (except for the root node), a transformation relative to
        /// its parent and possibly several child nodes.</p>
        /// <p>Simple file formats don't support hierarchical structures - for these formats the imported scene does
        /// consist of only a single root node without children.</p>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiNode
        {
            /// <summary>
            /// The name of the node.  The name may be empty but all nodes which need to be accessed afterwards by bones and animations are usually named.  Nodes can have the same name, but nodes which are referenced by bones/animasions HAVE to be unique.
            /// </summary>
            public aiString mName;

            /// <summary>
            /// The transformation relative to the node's parent.
            /// </summary>
            public aiMatrix4x4 mTransformation;

            /// <summary>
            /// Parent node. "Node* mParent" NULL if this node is the root node.
            /// </summary>
            public IntPtr mParent;

            /// <summary>
            /// The number of child nodes of this node.
            /// </summary>
            public uint mNumChildren;

            /// <summary>
            /// The child nodes of this node. "Node** mChildren" NULL if mNumChildren is 0.  Double Pointer...
            /// </summary>
            public IntPtr mChildren;

            /// <summary>
            /// The number of meshes of this node.
            /// </summary>
            public uint mNumMeshes;

            /// <summary>
            /// The meshes of this node. "uint* mMeshes" Each entry is an index into the mesh.
            /// </summary>
            public IntPtr mMeshes;

        }
        #endregion

        #region NodeAnimation Structure
        /// <summary>
        /// Describes the animation of a single node.  See 'aiAnim.h' for details.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiNodeAnimation
        {
            /// <summary>
            /// The name of the node affected by this animation. The node must exist and be unique.
            /// </summary>
            public aiString mNodeName;

            /// <summary>
            /// The number of position keys.  If there are position keys there will also be at least one scaling and rotation key.
            /// </summary>
            public uint mNumPositionKeys;

            /// <summary>
            /// An array of position keys used in this animation channel.  Positions are specificed as a 3D vector.  The array is mNumPositionKeys in size.  If there are position keys there will also be at least one scaling and rotation key.
            /// </summary>
            public aiVector3DKey* mPositionKeys;

            /// <summary>
            /// The number of rotation keys.
            /// </summary>
            public uint mNumRotationKeys;

            /// <summary>
            /// An array of rotation keys used in this animation channel.  Rotations are specificed as a quaternion.  The array is mRotationKeys in size.  If there are rotation keys there will also be at least one scaling and position key.
            /// </summary>
            public aiQuaternionKey* mRotationKeys;

            /// <summary>
            /// The number of scaling keys.
            /// </summary>
            public uint mNumScalingKeys;

            /// <summary>
            /// An array of scaling keys used in this animation channel.  Scales are specificed as a 3D vector.  The array is mNumScalingKeys in size.  If there are scaling keys there will also be at least one rotation and position key.
            /// </summary>
            public aiVector3DKey* mScalingKeys;

            /// <summary>
            /// Defines how the animation behaves before the first key is encountered.  The default value is aiAnimBehaviour_DEFAULT (the origional trnsformation matrix of the affected node is used).
            /// </summary>
            public uint mPreState;

            /// <summary>
            /// Defines how the animation behaves after the last key is encountered.  The default value is aiAnimBehaviour_DEFAULT (the origional transformation matrix of the affected node is used).
            /// </summary>
            public uint mPostState;

        }
        #endregion

        #region Animation Behaviour Enumeration
        /// <summary>
        /// Defines how an animation channel behaves outside the defined time range.  This corresponds to NodeAnimation.mPreState and NodeAnimation.mPostState. 
        /// </summary>
        public enum aiAnimBehaviour
        {
            /// <summary>
            /// The value from the default node transformation is taken.
            /// </summary>
            aiAnimBehaviour_DEFAULT = 0x0,

            /// <summary>
            /// The nearest key value is used without interpolation
            /// </summary>
            aiAnimBehaviour_CONSTANT = 0x1,

            /// <summary>
            /// The value of the nearest two keys is linearly extrapolated for the current time value.
            /// </summary>
            aiAnimBehaviour_LINEAR = 0x2,

            /// <summary>
            /// The animation is repeated.  If the animation key go from n to m and the current time is t, use the value at (t-n) % (|m-n|).
            /// </summary>
            aiAnimBehaviour_REPEAT = 0x3,

            /// <summary>
            /// This value is not used, it is just here to force the the compiler to map this enum to a 32 Bit integer.
            /// </summary>
            _aiAnimBehaviour_Force32Bit = 0xFFFFFF,//0x8fffffff
        }
        #endregion

        #region Animation Structure
        /// <summary>
        /// An animation consists of keyframe data for a number of nodes.  For each node affected by the animation a seperate series of data is given.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiAnimation
        {
            /// <summary>
            /// The name of the animation.  If the modeling package this data was exported from does support only a single animation channel, this name is usually empty (length is zero).
            /// </summary>
            public aiString mName;

            /// <summary>
            /// The duration of the animation in ticks.
            /// </summary>
            public double mDuration;

            /// <summary>
            /// Ticks per second. 0 if not specified in the imported file.
            /// </summary>
            public double mTicksPerSecond;

            /// <summary>
            /// The number of bone animations channels.  Each channel affects a single node.
            /// </summary>
            public uint mNumChannels;

            /// <summary>
            /// The node animation channels array.  "NodeAnimation** mChannels" Each channel affects a single node.  The array is mNumChannels in size.  This is a double pointer..
            /// </summary>
            public IntPtr mChannels;
        }
        #endregion

        #region Camera Structure
        /// <summary>
        /// Represents a camera imported from a model.  See 'aiCamera.h' for details.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiCamera
        {
            /// <summary>
            /// The name of the camera.  There must be a node in the asset graph with the same name.  THis node specifies the position of the camera in the scene hierarchy that can be animated.
            /// </summary>
            public aiString mName;

            /// <summary>
            /// Position of the camera relative to the coordinate space defined by the corresponding node.  The default value is 0|0|0.
            /// </summary>
            public aiVector3D mPosition;

            /// <summary>
            /// 'Up' - vector of the camera coordinate system relative to the coordinate space defined by the corresponding node.  The default value is 0|1|0.
            /// </summary>
            public aiVector3D mUp;

            /// <summary>
            /// 'LookAt' - vector of the camera coordinate system relative to  the coordinate space defined by the corresponding node.  The default value is 0|0|1.
            /// </summary>
            public aiVector3D mLookAt;

            /// <summary>
            /// Half horizontal field of view angle, in radians.  The field of view angle is the angle between the center line of the screen and the left or right border.  The default value is 1/4PI.
            /// </summary>
            public float mHorizontalFOV;

            /// <summary>
            /// Distance of the near clipping plane from the camera. The value may not be 0.f (for arithmetic reasons to prevent a division through zero). The default value is 0.1f.
            /// </summary>
            public float mClipPlaneNear;

            /// <summary>
            /// Distance of the far clipping plane from the camera. The far clipping plane must, of course, be further away than the near clipping plane. The default value is 1000.f. The ratio between the near and the far plane should not be too large (between 1000-10000 should be ok) to avoid floating-point inaccuracies which could lead to z-fighting.
            /// </summary>
            public float mClipPlaneFar;

            /// <summary>
            /// Screen aspect ratio.  This is the ration between the width and the height of the screen. Typical values are 4/3, 1/2 or 1/1. This value is 0 if the aspect ratio is not defined in the source file. 0 is also the default value.
            /// </summary>
            public float mAspect;

        }
        #endregion

        #region Light Structure
        /// <summary>
        /// A structure that represents a light source imported from a mesh. See 'aiLight.h' for details.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiLight
        {
            /// <summary>
            /// The name of the light source.  There must be a node in the secegraph with the same name.  This node specifies the position of the light in the scene hierarchy and can be animated.
            /// </summary>
            public aiString mName;

            /// <summary>
            /// The type of the light source.  aiLightSource_UNDEFINED is not a valid value for this member.
            /// </summary>
            public uint mType;

            /// <summary>
            /// Position of the light source in space. Relative to the transformation of the node corresponding to the light.  The position is undefined for directional lights.
            /// </summary>
            public aiVector3D mPosition;

            /// <summary>
            /// Direction of the light source in space. Relative to the transformation of the node corresponding to the light.  The direction is undefined for point lights. The vector may be normalized, but it needn't.
            /// </summary>
            public aiVector3D mDirection;

            /// <summary>
            /// Constant light attenuation factor.  The intensity of the light source at a given distance 'd' from the light's position is <code>Atten = 1/( att0 + att1#d + att2#d*d)</code> This member corresponds to the att0 variable in the equation.  Naturally undefined for directional lights.
            /// </summary>
            public float mAttenuationConstant;

            /// <summary>
            /// Linear light attenuation factor.  The intensity of the light source at a given distance 'd' from the light's position is <code>Atten = 1/( att0 + att1#d + att2#d*d)</code> This member corresponds to the att1 variable in the equation.  Naturally undefined for directional lights.
            /// </summary>
            public float mAttenuationLinear;

            /// <summary>
            /// Quadratic light attenuation factor.  The intensity of the light source at a given distance 'd' from the light's position is <code>Atten = 1/( att0 + att1#d + att2#d*d)</code>  This member corresponds to the att2 variable in the equation.  Naturally undefined for directional lights.
            /// </summary>
            public float mAttenuationQuadratic;

            /// <summary>
            /// Diffuse color of the light source.  The diffuse light color is multiplied with the diffuse  material color to obtain the final color that contributes to the diffuse shading term.
            /// </summary>
            public aiColor3D mColorDiffuse;

            /// <summary>
            /// Specular color of the light source.  The specular light color is multiplied with the specular material color to obtain the final color that contributes to the specular shading term.
            /// </summary>
            public aiColor3D mColorSpecular;

            /// <summary>
            /// Ambient color of the light source.  The ambient light color is multiplied with the ambient material color to obtain the final color that contributes to the ambient shading term. Most renderers will ignore this value it, is just a remaining of the fixed-function pipeline that is still supported by quite many file formats.
            /// </summary>
            public aiColor3D mColorAmbient;

            /// <summary>
            /// Inner angle of a spot light's light cone.  The spot light has maximum influence on objects inside this angle. The angle is given in radians. It is 2PI for point lights and undefined for directional lights.
            /// </summary>
            public float mAngleInnerCone;

            /// <summary>
            /// Outer angle of a spot light's light cone.  The spot light does not affect objects outside this angle. The angle is given in radians. It is 2PI for point lights and undefined for directional lights. The outer angle must be greater than or equal to the inner angle. It is assumed that the application uses a smooth interpolation between the inner and the outer cone of the spot light.
            /// </summary>
            public float mAngleOuterCone;

        }
        #endregion

        #region UVTransform Structure
        /// <summary>
        /// Defines how an UV channel is transformed.  Typically you'll want to build a matrix of this information. However, we keep separate scaling/translation/rotation values to make it easier to process and optimize UV transformations internally.  This is just a helper structure for the #AI_MATKEY_UVTRANSFORM key.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiUVTransform
        {
            /// <summary>
            /// Translation on the u and v axes.  Default = (0,0)
            /// </summary>
            public aiVector2D mTranslation;

            /// <summary>
            /// Scaling on the u and v axes.  Default = (1,1)
            /// </summary>
            public aiVector2D mScaling;

            /// <summary>
            /// Rotation - in counter-clockwise direction.  The rotation angle is specified in radians. The rotation center is 0.5f|0.5f. The default value is 0.f.
            /// </summary>
            public float mRotation;

        }
        #endregion

        #region MaterialProperty Structure
        /// <summary>
        /// Data structure for a single material property.  As an user, you'll probably never need to deal with this data structure.  Just use the provided aiGetMaterialXXX() or aiMaterial::Get() family of functions to query material properties easily.  Processing them manually is faster, but it is not the recommended way. It isn't worth the effort.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiMaterialProperty
        {
            /// <summary>
            /// Specifies the name of the property (key).  Keys are case insenstive.
            /// </summary>
            public aiString mKey;

            /// <summary>
            /// Textures: Specifies the exact usage semantic.  For non-texture properties, this member is always 0 or aiTextureType_NONE.
            /// </summary>
            public uint mSemantic;

            /// <summary>
            /// Textures: Specifies the index of the texture.  For non-texture properties, this member is always 0.
            /// </summary>
            public uint mIndex;

            /// <summary>
            /// Size of the buffer mData is pointing to, in bytes.  This value may not be 0.
            /// </summary>
            public uint mDataLength;

            /// <summary>
            /// Type information for the property.  Defines the data layout inside the data buffer. This is used by the library internally to perform debug checks and to utilize proper type conversions. (It's probably a hacky solution, but it works.)
            /// </summary>
            public uint mType;

            /// <summary>
            /// Binary buffer to hold the property's value.  The size of the buffer is always mDataLength.
            /// </summary>
            public byte* mData;

        }
        #endregion

        #region Material Structure
        /// <summary>
        /// Data structure for a material.  Material data is stored using a key-value structure. A single key-value pair is called a 'material property'.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiMaterial
        {
            /// <summary>
            /// List of all material properties loaded. "MaterialProperty** mProperties" Double pointer to MaterialProperty.
            /// </summary>
            public IntPtr mProperties;

            /// <summary>
            /// Number of properties in the data base.
            /// </summary>
            public uint mNumProperties;

            /// <summary>
            /// Storage allocated.
            /// </summary>
            public uint mNumAllocated;

        }
        #endregion

        #region Bone Structure
        /// <summary>
        /// A structure that descibes a bone in a mesh.  See 'aiMesh.h' for details.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiBone
        {
            /// <summary>
            /// The name of the bone.
            /// </summary>
            public aiString mName;

            /// <summary>
            /// The number of vertices affected by this bone.
            /// </summary>
            public uint mNumWeights;

            /// <summary>
            /// The vertices affected by this bone.  Pointer to VertexWeight.
            /// </summary>
            public aiVertexWeight* mWeights;

            /// <summary>
            /// Matrix that transforms from mesh space to bone space in bind pose.
            /// </summary>
            public aiMatrix4x4 mOffsetMatrix;

        }
        #endregion

        #region Mesh Structure
        /// <summary>
        /// A mesh represents a geometry or model with a single material.  It usually consists of a number of vertices and a series of primitives/faces referencing the vertices. In addition there might be a series of bones, each of them addressing a number of vertices with a certain weight. Vertex data is presented in channels with each channel containing a single per-vertex information such as a set of texture coords or a normal vector.  If a data pointer is non-null, the corresponding data stream is present.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiMesh
        {
            /// <summary>
            /// Bitwise combination of the members of the aiPrimitiveType enum.  This specifies which types of primitives are present in the mesh.  The "SortByPrimitiveType"-Step can be used to make sure the output meshes consist of one primitive type each.
            /// </summary>
            public uint mPrimitiveTypes;

            /// <summary>
            /// The number of vertices in this mesh.  This is also the size of all of the per-vertex data arrays.
            /// </summary>
            public uint mNumVertices;

            /// <summary>
            /// The number of primitives (triangles, polygons, lines) in this  mesh.  This is also the size of the mFaces array.
            /// </summary>
            public uint mNumFaces;

            /// <summary>
            /// Vertex positions.  This array is always present in a mesh. The array is mNumVertices in size.
            /// </summary>
            public aiVector3D* mVertices;

            /// <summary>
            /// Vertex normals. An IntPtr to an array of aiVector3D.  The array contains normalized vectors, NULL if not present.  The array is mNumVertices in size. Normals are undefined for point and line primitives. A mesh consisting of points and lines only may not have normal vectors. Meshes with mixed primitive types (i.e. lines and triangles) may have normals, but the normals for vertices that are only referenced by point or line primitives are undefined and set to NaN.  Note that NaN is even uneqal to itself.. Normal vectors computed by Assimp are always uint-length.
            /// </summary>
            public aiVector3D* mNormals;

            /// <summary>
            /// Vertex tangents.  An IntPtr to an array of aiVector3D.  The tangent of a vertex points in the direction of the positive X texture axis. If the mesh contains tangents, it automatically also contains bi-tangents.  The array contains normalized vectors, NULL if not present. The array is mNumVertices in size. A mesh consisting of points and lines only may not have normal vectors. Meshes with mixed primitive types (i.e. lines and triangles) may have normals, but the normals for vertices that are only referenced by point or line primitives are undefined and set to QNaN.
            /// </summary>
            public aiVector3D* mTangents;

            /// <summary>
            /// Vertex bitangents.  An IntPtr to an array of aiVector3D.  The bitangent of a vertex points in the direction of the positive Y texture axis. The array contains normalized vectors, NULL if not present. The array is mNumVertices in size.  The bitangent is just the cross product of tangent and normal vectors.
            /// </summary>
            public aiVector3D* mBitangents;

            /// <summary>
            /// Vertex color sets.  An IntPtr to the start of an array of aiColor4D's.  A mesh may contain 0 to AI_MAX_NUMBER_OF_COLOR_SETS vertex colors per vertex. NULL if not present. Each array is mNumVertices in size if present.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AI_MAX_NUMBER_OF_COLOR_SETS)]
            public IntPtr[] mColors;//aiColor4D*[] mColors;

            /// <summary>
            /// Vertex texture coords, also known as UV channels.  An array of IntPtr's that is AI_MAX_NUMBER_OF_TEXTURECOORDS long where each IntPtr points to a aiVector3D.  A mesh may contain 0 to AI_MAX_NUMBER_OF_TEXTURECOORDS per vertex. NULL if not present. The array is mNumVertices in size.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AI_MAX_NUMBER_OF_TEXTURECOORDS)]
            public IntPtr[] mTextureCoords;//aiVector3D*[] mTextureCoords;//aiVector3D mTextureCoords;

            /// <summary>
            /// Specifies the number of components for a given UV channel.  This variable is 'uint*AI_MAX_NUMBER_OF_TEXTURECOORDS'.  Up to three channels are supported (UVW, for accessing volume or cube maps). If the value is 2 for a given channel n, the component p.z of mTextureCoords[n][p] is set to 0.0f. If the value is 1 for a given channel, p.y is set to 0.0f, too.  4D coords are not supported.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AI_MAX_NUMBER_OF_TEXTURECOORDS)]
            public uint[] mNumUVComponents;

            /// <summary>
            /// The faces the mesh is constructed from.  "aiFace* mFaces" The IntPtr points to an array of faces.  Each face refers to a number of vertices by their indices.  This array is always present in a mesh, its size is given in mNumFaces. If the AI_SCENE_FLAGS_NON_VERBOSE_FORMAT is NOT set each face references an unique set of vertices.
            /// </summary>
            public IntPtr mFaces;

            /// <summary>
            /// The number of bones this mesh contains.  Can be 0, in which case the mBones array is NULL.
            /// </summary>
            public uint mNumBones;

            /// <summary>
            /// The bones of this mesh.  "Bone** mBones" The IntPtr Points to an array of pointers to bones. A bone consists of a name by which it can be found in the frame hierarchy and a set of vertex weights.
            /// </summary>
            public IntPtr mBones;

            /// <summary>
            /// The material used by this mesh.  A mesh does use only a single material. If an imported model uses multiple materials, the import splits up the mesh. Use this value as index into the scene's material list.
            /// </summary>
            public uint mMaterialIndex;
			
			/// <summary>
			/// The name of the mesh. May be empty.
			/// </summary>
			public aiString mName;
			
            /// <summary>
            /// The maximum number of colour sets a mesh can have.
            /// </summary>
            public const int AI_MAX_NUMBER_OF_COLOR_SETS = 4;
            /// <summary>
            /// The maximum number of texture co-ordinate sets a mesh can have.
            /// </summary>
            public const int AI_MAX_NUMBER_OF_TEXTURECOORDS = 4;

        }
        #endregion

        #region Texture Structure
        /// <summary>
        /// The structure that describes an embedded texture resource.  See 'aiTexture.h' for details.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiTexture
        {
            /// <summary>
            /// Width of the texture, in pixel.  If mHeight is zero the texture is compressed in a format like JPEG. In this case mWidth specifies the size of the memory area pcData is pointing to, in bytes.
            /// </summary>
            public uint mWidth;

            /// <summary>
            /// Height of the texture, in pixels.  If this value is zero, pcData points to an compressed texture in any format (e.g. JPEG).
            /// </summary>
            public uint mHeight;

            /// <summary>
            /// A hint from the loader to make it easier for applications to determine the type of embedded compressed textures.  If mHeight != 0 this member is undefined. Otherwise it is set set to '\\0\\0\\0\\0' if the loader has no additional information about the texture file format used OR the file extension of the format without a trailing dot.  If there are multiple file extensions for a format, the shortest extension is chosen (JPEG maps to 'jpg', not to 'jpeg').  E.g. 'dds\\0', 'pcx\\0', 'jpg\\0'.  All characters are lower-case.  The fourth character will always be '\\0'.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public sbyte achFormatHint;

            /// <summary>
            /// Data of the texture.  Points to an array of mWidth#mHeight aiTexel's.  The format of the texture data is always ARGB8888 to make the implementation for user of the library as easy as possible. If mHeight = 0 this is a pointer to a memory buffer of size mWidth containing the compressed texture data. Good luck, have fun!
            /// </summary>
            public aiTexel* pcData;

        }
        #endregion

        #region Ray Structure
        /// <summary>
        /// Ray geometry, portion of a line limited at one end by a point but extending to infinity in the other.  See 'aiScene.h' for details.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiRay
        {
            /// <summary>
            /// The position of the ray.
            /// </summary>
            public aiVector3D pos;

            /// <summary>
            /// The direction of the ray.
            /// </summary>
            public aiVector3D dir;

        }
        #endregion

        #region Scene Flags
        /// <summary>
        /// Flags regarding the state of a scene.  Accessible through Scene.mFlags.
        /// </summary>
        public enum aiSceneFlags : uint
        {
            /// <summary>
            /// Specifies that the scene data structure that was imported is not complete.
            /// This flag bypasses some internal validations and allows the import
            /// of animation skeletons, material libraries or camera animation paths
            /// using Assimp. Most applications won't support such data.
            /// </summary>
            AI_SCENE_FLAGS_INCOMPLETE = 0x1,

            /// <summary>
            /// This flag is set by the validation postprocess-step (aiPostProcess_ValidateDS)
            /// if the validation is successful. In a validated scene you can be sure that
            /// any cross references in the data structure (e.g. vertex indices) are valid.
            /// </summary>
            AI_SCENE_FLAGS_VALIDATED = 0x2,

            /// <summary>
            /// This flag is set by the validation postprocess-step (aiPostProcess_ValidateDS)
            /// if the validation is successful but some issues have been found.
            /// This can for example mean that a texture that does not exist is referenced
            /// by a material or that the bone weights for a vertex don't sum to 1.0 ... .
            /// In most cases you should still be able to use the import. This flag could
            /// be useful for applications which don't capture Assimp's log output.
            /// </summary>
            AI_SCENE_FLAGS_VALIDATION_WARNING = 0x4,

            /// <summary>
            /// This flag is currently only set by the JoinIdenticalVertices step.
            /// It indicates that the vertices of the output meshes aren't in the internal
            /// verbose format anymore. In the verbose format all vertices are unique,
            /// no vertex is ever referenced by more than one face.
            /// </summary>
            AI_SCENE_FLAGS_NON_VERBOSE_FORMAT = 0x8,

            /// <summary>
            /// Denotes pure height-map terrain data. Pure terrains usually consist of quads,
            /// sometimes triangles, in a regular grid. The x,y coordinates of all vertex
            /// positions refer to the x,y coordinates on the terrain height map, the z-axis
            /// stores the elevation at a specific point.
            /// TER (Terragen) and HMP (3D Game Studio) are height map formats.
            /// </summary>
            AI_SCENE_FLAGS_TERRAIN = 0x10,
        }
        #endregion

        #region Scene Structure
        /// <summary>
        /// The root structure of the imported data.   Everything that was imported from the given file can be accessed from here.  Objects of this class are generally maintained and owned by Assimp, not by the caller. You shouldn't want to instance it, nor should you ever try to delete a given scene on your own.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct aiScene
        {
            /// <summary>
            /// Any combination of the AI_SCENE_FLAGS_XXX flags. By default this value is 0, no flags are set. Most applications will want to reject all scenes with the AI_SCENE_FLAGS_INCOMPLETE bit set.
            /// </summary>
            public uint mFlags;

            /// <summary>
            /// The root node of the hierarchy.  "Node* mRootNode" An IntPtr to the Node structure which is at the start of the hierarchy.  There will always be at least the root node if the import was successful (and no special flags have been set). Presence of further nodes depends on the format and content of the imported file.
            /// </summary>
            public IntPtr mRootNode;

            /// <summary>
            /// The number of meshes in the scene.
            /// </summary>
            public uint mNumMeshes;

            /// <summary>
            /// An array of meshes. "aiMesh** mMeshes" An IntPtr to a pointer to an array of meshes.  Use the indices given in the aiNode structure to access this array. The array is mNumMeshes in size. If the AI_SCENE_FLAGS_INCOMPLETE flag is not set there will always be at least ONE material.
            /// </summary>
            public IntPtr mMeshes;

            /// <summary>
            /// The number of matrials in the scene.
            /// </summary>
            public uint mNumMaterials;

            /// <summary>
            /// The array of materials. "aiMaterial** mMaterials" An IntPtr to a pointer to an array of materials.  Use the index given in each aiMesh structure to access this array. The array is mNumMaterials in size. If the AI_SCENE_FLAGS_INCOMPLETE flag is not set there will always be at least ONE material.
            /// </summary>
            public IntPtr mMaterials;

            /// <summary>
            /// The number of animation in the scene.
            /// </summary>
            public uint mNumAnimations;

            /// <summary>
            /// The array of animations.  "Animation** mAnimations" An IntPtr to a pointer to an animation..  All animations imported frmo the given file are listed here.  The array is mNumAnimations in size.
            /// </summary>
            public IntPtr mAnimations;

            /// <summary>
            /// The number of textures embedded into the file.
            /// </summary>
            public uint mNumTextures;

            /// <summary>
            /// The array of embedded textures.  "Texture** mTextures" An IntPtr to a pointer to a texture.  Not many file formats embed their textures into the file. An example is Quake's MDL format (which is also used by some GameStudio versions). 
            /// </summary>
            public IntPtr mTextures;

            /// <summary>
            /// The number of light sources in the scene. Light sources are fully optional, in most cases this attribute will be 0.
            /// </summary>
            public uint mNumLights;

            /// <summary>
            /// The array of light sources.  "Light** mLights" All light sources imported from the given file are listed here. The array is mNumLights in size.
            /// </summary>
            public IntPtr mLights;

            /// <summary>
            /// The number of cameras in the scene. Cameras are fully optional, in most cases this attribute will be 0.
            /// </summary>
            public uint mNumCameras;

            /// <summary>
            /// The array of cameras. "Camera** mCameras" An IntPtr to a pointer to a camera.  All cameras imported from the given file are listed here.  The array is mNumCameras in size. The first camera in the array (if existing) is the default camera view into the scene.
            /// </summary>
            public IntPtr mCameras;

        }
        #endregion

        #region A couple of important enumerations.. Not all by a long way.  See the C file for more.
        /// <summary>
        /// Enumerates all supported types of light sources.
        /// </summary>
        public enum aiLightSourceType : uint
        {
            aiLightSource_UNDEFINED = 0x0,

            /// <summary>
            /// A directional light source has a well-defined direction but is infinitely far away. That's quite a good approximation for sun light.
            /// </summary>
            aiLightSource_DIRECTIONAL = 0x1,

            /// <summary>
            /// A point light source has a well-defined position in space but no direction - it emits light in all directions. A normal bulb is a point light.
            /// </summary>
            aiLightSource_POINT = 0x2,

            /// <summary>
            /// A spot light source emits light in a specific angle. It has a position and a direction it is pointing to. A good example for a spot light is a light spot in sport arenas.
            /// </summary>
            aiLightSource_SPOT = 0x3,

            /// <summary>
            /// This value is not used. It is just there to force the compiler to map this enum to a 32 Bit integer. 
            /// </summary>
            _aiLightSource_Force32Bit = 0x9fffffff,
        };

        /// <summary>
        /// Enumerates the types of geometric primitives supported by Assimp.
        /// <see cref="aiFace Face data structure"/>
        /// <see cref="SortByPType Per-primitive sorting of meshes"/>
        /// <see cref="Triangulate Automatic triangulation"/>
        /// <see cref="AI_CONFIG_PP_SBP_REMOVE Removal of specific primitive types."/>
        /// </summary>
        public enum aiPrimitiveType : uint
        {
            /// <summary>
            /// A point primitive. This is just a single vertex in the virtual world, aiFace contains just one index for such a primitive.
            /// </summary>
            aiPrimitiveType_POINT = 0x1,

            /// <summary>
            /// A line primitive. This is a line defined through a start and an end position. aiFace contains exactly two indices for such a primitive.
            /// </summary>
            aiPrimitiveType_LINE = 0x2,

            /// <summary>
            /// A triangular primitive. A triangle consists of three indices.
            /// </summary>
            aiPrimitiveType_TRIANGLE = 0x4,

            /// <summary>
            /// A higher-level polygon with more than 3 edges. A triangle is a polygon, but polygon in this context means "all polygons that are not triangles". The "Triangulate"-Step is provided for your convenience, it splits all polygons in triangles (which are much easier to handle).
            /// </summary>
            aiPrimitiveType_POLYGON = 0x8,

            /// <summary>
            /// This value is not used. It is just here to force the compiler to map this enum to a 32 Bit integer.
            /// </summary>
            _aiPrimitiveType_Force32Bit = 0x9fffffff
        }
        #endregion

        #region Constants from aiConfig.h
        /// AI_CONFIG_GLOB_MEASURE_TIME -> "GLOB_MEASURE_TIME"
        /// <summary>
        /// Enables time measurements.
        /// If enabled, measures the time needed for each part of the loading
        /// process (i.e. IO time, importing, postprocessing, ..) and dumps
        /// </summary>
        public const string AI_CONFIG_GLOB_MEASURE_TIME = "GLOB_MEASURE_TIME";
    
        /// AI_CONFIG_PP_SBBC_MAX_BONES -> "PP_SBBC_MAX_BONES"
        public const string AI_CONFIG_PP_SBBC_MAX_BONES = "PP_SBBC_MAX_BONES";
    
        /// AI_SBBC_DEFAULT_MAX_BONES -> 60
        public const int AI_SBBC_DEFAULT_MAX_BONES = 60;

        /// <summary>
        /// Specifies the maximum angle that may be between two vertex tangents
        /// that their tangents and bitangents are smoothed.
        /// This applies to the CalcTangentSpace-Step. The angle is specified
        /// in degrees, so 180 is PI. The default value is
        /// 45 degrees. The maximum value is 175.
        /// Property type: float.
        /// </summary>
        /// AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE -> "PP_CT_MAX_SMOOTHING_ANGLE"
        public const string AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE = "PP_CT_MAX_SMOOTHING_ANGLE";

        /// <summary>
        /// Specifies the maximum angle that may be between two face normals
        /// at the same vertex position that their are smoothed together.
        /// Sometimes referred to as 'crease angle'.
        /// This applies to the GenSmoothNormals-Step. The angle is specified
        /// in degrees, so 180 is PI. The default value is 175 degrees (all vertex
        /// normals are smoothed). The maximum value is 175, too. Property type: float.
        /// Warning: setting this option may cause a severe loss of performance. The
        /// performance is unaffected if the #AI_CONFIG_FAVOUR_SPEED flag is set but
        /// the output quality may be reduced.
        /// </summary>
        /// AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE -> "PP_GSN_MAX_SMOOTHING_ANGLE"
        public const string AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE = "PP_GSN_MAX_SMOOTHING_ANGLE";
    
        /// AI_CONFIG_IMPORT_MDL_COLORMAP -> "IMPORT_MDL_COLORMAP"
        public const string AI_CONFIG_IMPORT_MDL_COLORMAP = "IMPORT_MDL_COLORMAP";
    
        /// AI_CONFIG_PP_RRM_EXCLUDE_LIST -> "PP_RRM_EXCLUDE_LIST"
        public const string AI_CONFIG_PP_RRM_EXCLUDE_LIST = "PP_RRM_EXCLUDE_LIST";
    
        /// AI_CONFIG_PP_PTV_KEEP_HIERARCHY -> "PP_PTV_KEEP_HIERARCHY"
        public const string AI_CONFIG_PP_PTV_KEEP_HIERARCHY = "PP_PTV_KEEP_HIERARCHY";
    
        /// AI_CONFIG_PP_PTV_NORMALIZE -> "PP_PTV_NORMALIZE"
        public const string AI_CONFIG_PP_PTV_NORMALIZE = "PP_PTV_NORMALIZE";
    
        /// AI_CONFIG_PP_FD_REMOVE -> "PP_FD_REMOVE"
        public const string AI_CONFIG_PP_FD_REMOVE = "PP_FD_REMOVE";
    
        /// AI_CONFIG_PP_OG_EXCLUDE_LIST -> "PP_OG_EXCLUDE_LIST"
        public const string AI_CONFIG_PP_OG_EXCLUDE_LIST = "PP_OG_EXCLUDE_LIST";
    
        /// AI_CONFIG_PP_SLM_TRIANGLE_LIMIT -> "PP_SLM_TRIANGLE_LIMIT"
        public const string AI_CONFIG_PP_SLM_TRIANGLE_LIMIT = "PP_SLM_TRIANGLE_LIMIT";
    
        /// AI_SLM_DEFAULT_MAX_TRIANGLES -> 1000000
        public const int AI_SLM_DEFAULT_MAX_TRIANGLES = 1000000;
    
        /// AI_CONFIG_PP_SLM_VERTEX_LIMIT -> "PP_SLM_VERTEX_LIMIT"
        public const string AI_CONFIG_PP_SLM_VERTEX_LIMIT = "PP_SLM_VERTEX_LIMIT";
    
        /// AI_SLM_DEFAULT_MAX_VERTICES -> 1000000
        public const int AI_SLM_DEFAULT_MAX_VERTICES = 1000000;
    
        /// AI_CONFIG_PP_LBW_MAX_WEIGHTS -> "PP_LBW_MAX_WEIGHTS"
        public const string AI_CONFIG_PP_LBW_MAX_WEIGHTS = "PP_LBW_MAX_WEIGHTS";
    
        /// AI_LMW_MAX_WEIGHTS -> 0x4
        public const int AI_LMW_MAX_WEIGHTS = 4;
    
        /// PP_ICL_PTCACHE_SIZE -> 12
        public const int PP_ICL_PTCACHE_SIZE = 12;
    
        /// AI_CONFIG_PP_ICL_PTCACHE_SIZE -> "PP_ICL_PTCACHE_SIZE"
        public const string AI_CONFIG_PP_ICL_PTCACHE_SIZE = "PP_ICL_PTCACHE_SIZE";
    
        /// Warning: Generation of Method Macros is not supported at this time
        /// aiComponent_COLORSn -> "(n) (1u << (n+20u))"
        public const string aiComponent_COLORSn = "(n) (1u << (n+20u))";
    
        /// Warning: Generation of Method Macros is not supported at this time
        /// aiComponent_TEXCOORDSn -> "(n) (1u << (n+25u))"
        public const string aiComponent_TEXCOORDSn = "(n) (1u << (n+25u))";
    
        /// AI_CONFIG_PP_RVC_FLAGS -> "PP_RVC_FLAGS"
        public const string AI_CONFIG_PP_RVC_FLAGS = "PP_RVC_FLAGS";
    
        /// AI_CONFIG_PP_SBP_REMOVE -> "PP_SBP_REMOVE"
        public const string AI_CONFIG_PP_SBP_REMOVE = "PP_SBP_REMOVE";
    
        /// AI_CONFIG_PP_FID_ANIM_ACCURACY -> "PP_FID_ANIM_ACCURACY"
        public const string AI_CONFIG_PP_FID_ANIM_ACCURACY = "PP_FID_ANIM_ACCURACY";
    
        /// AI_UVTRAFO_SCALING -> 0x1
        public const int AI_UVTRAFO_SCALING = 1;
    
        /// AI_UVTRAFO_ROTATION -> 0x2
        public const int AI_UVTRAFO_ROTATION = 2;
    
        /// AI_UVTRAFO_TRANSLATION -> 0x4
        public const int AI_UVTRAFO_TRANSLATION = 4;
    
        /// AI_UVTRAFO_ALL -> (AI_UVTRAFO_SCALING | AI_UVTRAFO_ROTATION | AI_UVTRAFO_TRANSLATION)
        public const int AI_UVTRAFO_ALL = (AI_UVTRAFO_SCALING | (AI_UVTRAFO_ROTATION | AI_UVTRAFO_TRANSLATION));
    
        /// AI_CONFIG_PP_TUV_EVALUATE -> "PP_TUV_EVALUATE"
        public const string AI_CONFIG_PP_TUV_EVALUATE = "PP_TUV_EVALUATE";
    
        /// AI_CONFIG_FAVOUR_SPEED -> "FAVOUR_SPEED"
        public const string AI_CONFIG_FAVOUR_SPEED = "FAVOUR_SPEED";
    
        /// AI_CONFIG_IMPORT_GLOBAL_KEYFRAME -> "IMPORT_GLOBAL_KEYFRAME"
        public const string AI_CONFIG_IMPORT_GLOBAL_KEYFRAME = "IMPORT_GLOBAL_KEYFRAME";
    
        /// AI_CONFIG_IMPORT_MD3_KEYFRAME -> "IMPORT_MD3_KEYFRAME"
        public const string AI_CONFIG_IMPORT_MD3_KEYFRAME = "IMPORT_MD3_KEYFRAME";
    
        /// AI_CONFIG_IMPORT_MD2_KEYFRAME -> "IMPORT_MD2_KEYFRAME"
        public const string AI_CONFIG_IMPORT_MD2_KEYFRAME = "IMPORT_MD2_KEYFRAME";
    
        /// AI_CONFIG_IMPORT_MDL_KEYFRAME -> "IMPORT_MDL_KEYFRAME"
        public const string AI_CONFIG_IMPORT_MDL_KEYFRAME = "IMPORT_MDL_KEYFRAME";
    
        /// AI_CONFIG_IMPORT_MDC_KEYFRAME -> "IMPORT_MDC_KEYFRAME"
        public const string AI_CONFIG_IMPORT_MDC_KEYFRAME = "IMPORT_MDC_KEYFRAME";
    
        /// AI_CONFIG_IMPORT_SMD_KEYFRAME -> "IMPORT_SMD_KEYFRAME"
        public const string AI_CONFIG_IMPORT_SMD_KEYFRAME = "IMPORT_SMD_KEYFRAME";
    
        /// AI_CONFIG_IMPORT_UNREAL_KEYFRAME -> "IMPORT_UNREAL_KEYFRAME"
        public const string AI_CONFIG_IMPORT_UNREAL_KEYFRAME = "IMPORT_UNREAL_KEYFRAME";
    
        /// AI_CONFIG_IMPORT_AC_SEPARATE_BFCULL -> "IMPORT_AC_SEPARATE_BFCULL"
        public const string AI_CONFIG_IMPORT_AC_SEPARATE_BFCULL = "IMPORT_AC_SEPARATE_BFCULL";
    
        /// AI_CONFIG_IMPORT_AC_EVAL_SUBDIVISION -> "IMPORT_AC_EVAL_SUBDIVISION"
        public const string AI_CONFIG_IMPORT_AC_EVAL_SUBDIVISION = "IMPORT_AC_EVAL_SUBDIVISION";
    
        /// AI_CONFIG_IMPORT_UNREAL_HANDLE_FLAGS -> "UNREAL_HANDLE_FLAGS"
        public const string AI_CONFIG_IMPORT_UNREAL_HANDLE_FLAGS = "UNREAL_HANDLE_FLAGS";
    
        /// AI_CONFIG_IMPORT_TER_MAKE_UVS -> "IMPORT_TER_MAKE_UVS"
        public const string AI_CONFIG_IMPORT_TER_MAKE_UVS = "IMPORT_TER_MAKE_UVS";
    
        /// AI_CONFIG_IMPORT_ASE_RECONSTRUCT_NORMALS -> "IMPORT_ASE_RECONSTRUCT_NORMALS"
        public const string AI_CONFIG_IMPORT_ASE_RECONSTRUCT_NORMALS = "IMPORT_ASE_RECONSTRUCT_NORMALS";
    
        /// AI_CONFIG_IMPORT_MD3_HANDLE_MULTIPART -> "IMPORT_MD3_HANDLE_MULTIPART"
        public const string AI_CONFIG_IMPORT_MD3_HANDLE_MULTIPART = "IMPORT_MD3_HANDLE_MULTIPART";
    
        /// AI_CONFIG_IMPORT_MD3_SKIN_NAME -> "IMPORT_MD3_SKIN_NAME"
        public const string AI_CONFIG_IMPORT_MD3_SKIN_NAME = "IMPORT_MD3_SKIN_NAME";
    
        /// AI_CONFIG_IMPORT_MD3_SHADER_SRC -> "IMPORT_MD3_SHADER_SRC"
        public const string AI_CONFIG_IMPORT_MD3_SHADER_SRC = "IMPORT_MD3_SHADER_SRC";
    
        /// AI_CONFIG_IMPORT_LWO_ONE_LAYER_ONLY -> "IMPORT_LWO_ONE_LAYER_ONLY"
        public const string AI_CONFIG_IMPORT_LWO_ONE_LAYER_ONLY = "IMPORT_LWO_ONE_LAYER_ONLY";
    
        /// AI_CONFIG_IMPORT_MD5_NO_ANIM_AUTOLOAD -> "IMPORT_MD5_NO_ANIM_AUTOLOAD"
        public const string AI_CONFIG_IMPORT_MD5_NO_ANIM_AUTOLOAD = "IMPORT_MD5_NO_ANIM_AUTOLOAD";
    
        /// AI_CONFIG_IMPORT_LWS_ANIM_START -> "IMPORT_LWS_ANIM_START"
        public const string AI_CONFIG_IMPORT_LWS_ANIM_START = "IMPORT_LWS_ANIM_START";
    
        /// AI_CONFIG_IMPORT_LWS_ANIM_END -> "IMPORT_LWS_ANIM_END"
        public const string AI_CONFIG_IMPORT_LWS_ANIM_END = "IMPORT_LWS_ANIM_END";
    
        /// AI_CONFIG_IMPORT_IRR_ANIM_FPS -> "IMPORT_IRR_ANIM_FPS"
        public const string AI_CONFIG_IMPORT_IRR_ANIM_FPS = "IMPORT_IRR_ANIM_FPS";
    
        /// AI_CONFIG_IMPORT_OGRE_MATERIAL_FILE -> "IMPORT_OGRE_MATERIAL_FILE"
        public const string AI_CONFIG_IMPORT_OGRE_MATERIAL_FILE = "IMPORT_OGRE_MATERIAL_FILE";

        /// <summary>
        /// Enumerates components of the aiScene and aiMesh data structures
        /// that can be excluded from the import using the #aiPrpcess_RemoveComponent step.
        /// See the documentation to #RemoveComponent for more details.
        /// </summary>
        public enum aiComponent : uint
        {
            /// aiComponent_NORMALS -> 0x2u
            aiComponent_NORMALS = 2u,
    
            /// aiComponent_TANGENTS_AND_BITANGENTS -> 0x4u
            aiComponent_TANGENTS_AND_BITANGENTS = 4u,
    
            /// aiComponent_COLORS -> 0x8
            aiComponent_COLORS = 8,
    
            /// aiComponent_TEXCOORDS -> 0x10
            aiComponent_TEXCOORDS = 16,
    
            /// aiComponent_BONEWEIGHTS -> 0x20
            aiComponent_BONEWEIGHTS = 32,
    
            /// aiComponent_ANIMATIONS -> 0x40
            aiComponent_ANIMATIONS = 64,
    
            /// aiComponent_TEXTURES -> 0x80
            aiComponent_TEXTURES = 128,
    
            /// aiComponent_LIGHTS -> 0x100
            aiComponent_LIGHTS = 256,
    
            /// aiComponent_CAMERAS -> 0x200
            aiComponent_CAMERAS = 512,
    
            /// aiComponent_MESHES -> 0x400
            aiComponent_MESHES = 1024,
    
            /// aiComponent_MATERIALS -> 0x800
            aiComponent_MATERIALS = 2048,
    
            /// _aiComponent_Force32Bit -> 0x9fffffff
            _aiComponent_Force32Bit = 0x9fffffff, //- 1610612737,
        }
        #endregion
    }
}
