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
    /// Material properties are used to store data about a given material.  This data is referenced through a string key.
    /// </summary>
    /// <author>John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk</author>
    /// <date>21 July 2009</date>
    [Serializable]
    public class MaterialProperty
    {
        #region Properties
        /// <summary>
        /// Specifies the name of the property (key).  Keys are case insenstive.
        /// </summary>
        private String sKey;

        /// <summary>
        /// Textures: Specifies the exact usage semantic.  For non-texture properties, this member is always 0 or aiTextureType_NONE.
        /// </summary>
        private Material.TextureType eSemantic;

        /// <summary>
        /// Textures: Specifies the index of the texture.  For non-texture properties, this member is always 0.
        /// </summary>
        private uint iIndex;

        /// <summary>
        /// Type information for the property.  Defines the data layout inside the data buffer. This is used by the library internally to perform debug checks and to utilize proper type conversions. (It's probably a hacky solution, but it works.)
        /// </summary>
        private Material.PropertyTypeInfo eType;

        /// <summary>
        /// Binary buffer to hold the property's value.  The size of the buffer is always mDataLength.
        /// </summary>
        //public byte[] tData;
        public object kData;
        #endregion

        /// <summary>
        /// Constructor which builds a material property object from a pointer to an aiMaterialProperty structure.  As a user, you should not call this by hand.
        /// <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
        /// </summary>
        /// <param name="aiMaterialProperty*">A pointer to the face structure in the low level unmanaged wrapper.</param>
        unsafe internal MaterialProperty(IntPtr p_aiMaterialProperty)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiMaterialProperty tProperty = (UnmanagedAssimp.aiMaterialProperty)Marshal.PtrToStructure(p_aiMaterialProperty, typeof(UnmanagedAssimp.aiMaterialProperty));

            // Copy the nice simple value types into managed memory.
            this.sKey       = "" + tProperty.mKey.data;
            this.eSemantic  = (Material.TextureType)tProperty.mSemantic;
            this.iIndex     = tProperty.mIndex;
            this.eType      = (Material.PropertyTypeInfo)tProperty.mType;
            
            // Setup the pointer and pointer stride to read the bugger.
            int iStride = sizeof(byte);
            IntPtr pPtr = new IntPtr(tProperty.mData);

            // Copy the raw bytes from assimp into the managed array.
            byte[] tData = new byte[tProperty.mDataLength];
            for (int iByte = 0; iByte < tProperty.mDataLength; ++iByte)
            {
                tData[iByte] = Marshal.ReadByte(pPtr);
                pPtr = new IntPtr(pPtr.ToInt64() + iStride);
            }

            kData = (object)tData;
        }

        /// <summary>
        /// Return the name (key) of this material property.
        /// </summary>
        /// <returns>A string which contains the key of this material property.</returns>
        public String getKey()
        {
            return sKey;
        }

        /// <summary>
        /// Get the exact usage semantic.  For non-texture properties, this member is always 0 or aiTextureType_NONE.
        /// </summary>
        /// <returns>A unit which contains the exact usage semantic.</returns>
        public Material.TextureType getSemantic()
        {
            return eSemantic;
        }

        /// <summary>
        /// Get the index of the texture.  For non-texture properties, this member is always 0.
        /// </summary>
        /// <returns>A unit which contains the texture index in the array.</returns>
        public uint getTextureIndex()
        {
            return iIndex;
        }

        /// <summary>
        /// Type information for the property.
        /// Defines the data layout inside the data buffer.
        /// This is used by the library internally to perform debug checks and to utilize proper type conversions.
        /// (It's probably a hacky solution, but it works.)
        /// </summary>
        /// <returns>The type information for this property.</returns>
        public Material.PropertyTypeInfo getType()
        {
            return eType;
        }

        /// <summary>
        /// Get a reference to the data array.
        /// </summary>
        /// <returns>A reference to the data array as bytes.</returns>
        public byte[] getByteData()
        {
            return (byte[])kData;
        }

        /// <summary>
        /// Get a reference to the data array.
        /// </summary>
        /// <returns>A reference to the data array as bytes.</returns>
        public object getData()
        {
            return kData;
        }

        /// <summary>
        /// Get the number of bytes in the data array.
        /// </summary>
        /// <returns>The number of bytes in the data array.</returns>
        public int getNumDataBytes()
        {
            return ((byte[])kData).Length;
        }
    }
}
