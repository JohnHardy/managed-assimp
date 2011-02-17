using System;
using System.Runtime.InteropServices;

using Assimp.ManagedAssimp.Unmanaged;


namespace Assimp.ManagedAssimp
{
    /**
     * <summary>Material properties are used to store data about a given material.  This data is referenced through a string key.</summary>
     * @author John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk
     * @date 21 July 2009
     * @version 1.0
     */
    [Serializable]
    public class MaterialProperty
    {
        #region Properties
        /** <summary>Specifies the name of the property (key).  Keys are case insenstive.</summary> */
        private String sKey;

        /** <summary>Textures: Specifies the exact usage semantic.  For non-texture properties, this member is always 0 or aiTextureType_NONE.</summary> */
        private Material.TextureType eSemantic;

        /** <summary>Textures: Specifies the index of the texture.  For non-texture properties, this member is always 0.</summary> */
        private uint iIndex;

        /** <summary>Type information for the property.  Defines the data layout inside the data buffer. This is used by the library internally to perform debug checks and to utilize proper type conversions. (It's probably a hacky solution, but it works.)</summary> */
        private Material.PropertyTypeInfo eType;

        /** <summary>Binary buffer to hold the property's value.  The size of the buffer is always mDataLength.</summary> */
        //public byte[] tData;
        public object kData;
        #endregion

        /**
         * <summary>Constructor which builds a material property object from a pointer to an aiMaterialProperty structure.  As a user, you should not call this by hand.</summary>
         * <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
         * @param aiMaterialProperty* A pointer to the face structure in the low level unmanaged wrapper.
         */
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

        /**
         * <summary>Return the name (key) of this material property.</summary>
         * @return A string which contains the key of this material property.
         */
        public String getKey()
        {
            return sKey;
        }

        /**
         * <summary>Get the exact usage semantic.  For non-texture properties, this member is always 0 or aiTextureType_NONE.</summary>
         * @return A unit which contains the exact usage semantic.
         */
        public Material.TextureType getSemantic()
        {
            return eSemantic;
        }

        /**
         * <summary>Get the index of the texture.  For non-texture properties, this member is always 0.</summary>
         * @return A unit which contains the texture index in the array.
         */
        public uint getTextureIndex()
        {
            return iIndex;
        }

        /**
         * <summary>Type information for the property.
         * Defines the data layout inside the data buffer.
         * This is used by the library internally to perform debug checks and to utilize proper type conversions.
         * (It's probably a hacky solution, but it works.)</summary>
         * @return The type information for this property.
         */
        public Material.PropertyTypeInfo getType()
        {
            return eType;
        }

        /**
         * <summary>Get a reference to the data array.</summary>
         * @return A reference to the data array as bytes.
         */
        public byte[] getByteData()
        {
            return (byte[])kData;
        }

        /**
         * <summary>Get a reference to the data array.</summary>
         * @return A reference to the data array as bytes.
         */
        public object getData()
        {
            return kData;
        }

        /**
         * <summary>Get the number of bytes in the data array.</summary>
         * @return The number of bytes in the data array.
         */
        public int getNumDataBytes()
        {
            return ((byte[])kData).Length;
        }
    }
}
