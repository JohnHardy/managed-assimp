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
using System.Runtime.InteropServices;

using Assimp.ManagedAssimp.Unmanaged;


namespace Assimp.ManagedAssimp
{

    /// <summary>
    /// Class to wrap materials. Materials are represented in ASSIMP as a list of key/value pairs, the key being a <code>String</code> and the value being a binary buffer. The class provides several get methods to access material properties easily.
    /// Faces of different primitive types can occur in a single mesh. To get
    /// homogeneous meshes, try the <code>PostProcessing.SortByPType</code> flag.
    /// </summary>
    /// <author>John Hardy</author>
    /// <date>21 July 2009</date>
    [Serializable]
    public class Material
    {
        #region Constants
        /// <summary>
        /// This awesome little class binds a type to a property name.
        /// </summary>
        public class PropertyBinding<T>
        {
            /// <summary>
            /// Name of the property.
            /// </summary>
            internal String sName               = "";
            /// <summary>
            /// The texture type of the property.  Only useful for texture properies.
            /// </summary>
            internal TextureType eTextureType   = 0;
            /// <summary>
            /// The texture index of the property.  Only useful for texture properies.
            /// </summary>
            internal uint iTextureIndex         = 0;

            /// <summary>
            /// Construct a new class which stores information about a property.
            /// </summary>
            /// <param name="sName">The name of the property.</param>
            public PropertyBinding(String sName)
            {
                this.sName = sName;
            }

            /// <summary>
            /// Get the name of this property.  This corrsponds to the property "key" in a material..
            /// </summary>
            public String getName()
            {
                return sName;
            }

            /// <summary>
            /// Return the texture type of the property.  This specifies the exact usage semantic.  For non-texture properties, this member is always 0 or aiTextureType_NONE.
            /// </summary>
            /// <returns>The texture type.</returns>
            public TextureType getTextureSemantic()
            {
                return eTextureType;
            }

            /// <summary>
            /// Return the texture index. For non-texture properties, this member is always 0.
            /// </summary>
            public uint getTextureIndex()
            {
                return iTextureIndex;
            }
        }

        /*
        --#define AI_MATKEY_NAME "?mat.name",0,0
        --#define AI_MATKEY_TWOSIDED "$mat.twosided",0,0
        --#define AI_MATKEY_SHADING_MODEL "$mat.shadingm",0,0
        --#define AI_MATKEY_ENABLE_WIREFRAME "$mat.wireframe",0,0
        --#define AI_MATKEY_BLEND_FUNC "$mat.blend",0,0
        --#define AI_MATKEY_OPACITY "$mat.opacity",0,0
        --#define AI_MATKEY_BUMPSCALING "$mat.bumpscaling",0,0
        --#define AI_MATKEY_SHININESS "$mat.shininess",0,0
        ...
        */

        /// <summary>
        /// Defines the name of the material.
        /// </summary>
        public static readonly PropertyBinding<String> NAME                 = new PropertyBinding<String>("?mat.name");

    /// <summary>
    /// Defines whether the material must be rendered two-sided. This is a
    /// boolean property. n != 0 is true.
    /// </summary>
        public static readonly PropertyBinding<bool> TWOSIDED               = new PropertyBinding<bool>("$mat.twosided");

    /// <summary>
    /// Defines whether the material must be rendered in wireframe. This is a
    /// boolean property. n != 0 is true.
    /// </summary>
        public static readonly PropertyBinding<bool> WIREFRAME              = new PropertyBinding<bool>("$mat.wireframe");

    /// <summary>
    /// Defines the shading model to be used to compute the lighting for the
    /// material. This is one of the values defined in the
    /// ShadingModel 'enum'.
    /// </summary>
        public static readonly PropertyBinding<ShadingMode> SHADING_MODEL   = new PropertyBinding<ShadingMode>("$mat.shadingm");

    /// <summary>
    /// Defines the blend function to be used to combine the computed color value
    /// of a pixel with the previous value in the backbuffer. This is one of the
    /// values defined in the BlendFunc 'enum'.
    /// </summary>
        public static readonly PropertyBinding<BlendMode> BLEND_FUNC        = new PropertyBinding<BlendMode>("$mat.blend");

    /// <summary>
    /// Defines the basic opacity of the material in range 0..1 where 0 is fully
    /// transparent and 1 is fully opaque. The default value to be taken if this
    /// property is not defined is naturally 1.0f
    /// </summary>
        public static readonly PropertyBinding<float> OPACITY               = new PropertyBinding<float>("$mat.opacity");

    /// <summary>
    /// Defines the height scaling of bumpmaps/parallax maps on this material.
    /// The default value to be taken if this property is not defined is
    /// naturally 1.0f.
    /// </summary>
        public static readonly PropertyBinding<float> BUMPHEIGHT            = new PropertyBinding<float>("$mat.bumpscaling");

    /// <summary>
    /// Defines the shininess of the material. This is simply the exponent of the
    /// phong and blinn-phong shading equations. If the property is not defined,
    /// no specular lighting must be computed for the material.
    /// </summary>
        public static readonly PropertyBinding<float> SHININESS             = new PropertyBinding<float>("$mat.shininess");

        /*
         * DEFINE THE FOLLOWING... I can't find the documentation for these in assimp so have guessed at their type and meaning.
        #define AI_MATKEY_SHININESS_STRENGTH "$mat.shinpercent",0,0
        #define AI_MATKEY_REFRACTI "$mat.refracti",0,0
        #define AI_MATKEY_COLOR_DIFFUSE "$clr.diffuse",0,0
        #define AI_MATKEY_COLOR_AMBIENT "$clr.ambient",0,0
        #define AI_MATKEY_COLOR_SPECULAR "$clr.specular",0,0
        #define AI_MATKEY_COLOR_EMISSIVE "$clr.emissive",0,0
        #define AI_MATKEY_COLOR_TRANSPARENT "$clr.transparent",0,0
        #define AI_MATKEY_COLOR_REFLECTIVE "$clr.reflective",0,0
        #define AI_MATKEY_GLOBAL_BACKGROUND_IMAGE "?bg.global",0,0
        */
        /// <summary>
        /// The strength of the shininess.
        /// </summary>
        public static readonly PropertyBinding<float> SHININESS_STRENGTH    = new PropertyBinding<float>("$mat.shinpercent");

        /// <summary>
        /// Index of refraction of the material.
        /// </summary>
        public static readonly PropertyBinding<float> REFRACTI              = new PropertyBinding<float>("$mat.refracti");

        /// <summary>
        /// The diffuse colour for the material.
        /// </summary>
        public static readonly PropertyBinding<aiColor4D> COLOR_DIFFUSE     = new PropertyBinding<aiColor4D>("$clr.diffuse");

        /// <summary>
        /// The ambient colour for the material
        /// </summary>
        public static readonly PropertyBinding<aiColor4D> COLOR_AMBIENT     = new PropertyBinding<aiColor4D>("$clr.ambient");

        /// <summary>
        /// The specular colour for the material.
        /// </summary>
        public static readonly PropertyBinding<aiColor4D> COLOR_SPECULAR    = new PropertyBinding<aiColor4D>("$clr.specular");

        /// <summary>
        /// The emissive colour for the material.
        /// </summary>
        public static readonly PropertyBinding<aiColor4D> COLOR_EMISSIVE    = new PropertyBinding<aiColor4D>("$clr.emissive");

        /// <summary>
        /// The transparency colour for the material.
        /// </summary>
        public static readonly PropertyBinding<float> COLOR_TRANSPARENT     = new PropertyBinding<float>("$clr.transparent");

        /// <summary>
        /// The reflective colour for the material
        /// </summary>
        public static readonly PropertyBinding<float> COLOR_REFLECTIVE      = new PropertyBinding<float>("$clr.reflective");

        /*
         * And some more texture related stuff... again I can only guess at what this lot is for. Sorry - I'm short on time.
        #define _AI_MATKEY_TEXTURE_BASE			"$tex.file"
        #define _AI_MATKEY_UVWSRC_BASE			"$tex.uvwsrc"
        #define _AI_MATKEY_TEXOP_BASE			"$tex.op"
        #define _AI_MATKEY_MAPPING_BASE			"$tex.mapping"
        #define _AI_MATKEY_TEXBLEND_BASE		"$tex.blend"
        #define _AI_MATKEY_MAPPINGMODE_U_BASE	"$tex.mapmodeu"
        #define _AI_MATKEY_MAPPINGMODE_V_BASE	"$tex.mapmodev"
        #define _AI_MATKEY_TEXMAP_AXIS_BASE		"$tex.mapaxis"
        #define _AI_MATKEY_UVTRANSFORM_BASE		"$tex.uvtrafo"
        #define _AI_MATKEY_TEXFLAGS_BASE		"$tex.flags"
        */

        /// <summary>
        /// The string where we can find the texture file ascociated with this material. 
        /// </summary>
        public static readonly PropertyBinding<String> TEXTURE_FILE         = new PropertyBinding<string>("$tex.file");


        #endregion

        #region Enumerations
        /// <summary>
        /// This defines the enumerated values for the type properties asociated with a MaterialProperty.
        /// </summary>
        public enum PropertyTypeInfo : uint
        {
            /// <summary>
            /// Array of single-precision (32 Bit) floats.
            /// </summary>
            Float = 0x1,

            /// <summary>
            /// The material property is an aiString.
            /// </summary>
            String = 0x3,

            /// <summary>
            /// Array of (32 Bit) integers.
            /// </summary>
            Integer = 0x4,

            /// <summary>
            /// Simple binary buffer, content undefined. Not convertible to anything.
            /// </summary>
            Buffer = 0x5,
        }

        /// <summary>
        /// Defines how the Nth texture of a specific type is combined with
        ///  the result of all previous layers.
        ///  Example (left: key, right: value): <br>
        /// </summary>
        public enum TextureOperation : uint
        {
            /// <summary>
            /// T = T1 * T2
            /// </summary>
            Multiply = 0x0,

            /// <summary>
            /// T = T1 + T2
            /// </summary>
            Add = 0x1,

            /// <summary>
            /// T = T1 - T2
            /// </summary>
            Subtract = 0x2,

            /// <summary>
            /// T = T1 / T2
            /// </summary>
            Divide = 0x3,

            /// <summary>
            /// T = (T1 + T2) - (T1 * T2)
            /// </summary>
            SmoothAdd = 0x4,

            /// <summary>
            /// T = T1 + (T2-0.5)
            /// </summary>
            SignedAdd = 0x5,
        }


        /// <summary>
        /// Defines the purpose of a texture.  Related to MaterialProperty.getSemantic().
        ///  This is a very difficult topic. Different 3D packages support different
        ///  kinds of textures. For very common texture types, such as bumpmaps, the
        ///  rendering results depend on implementation details in the rendering
        ///  pipelines of these applications. Assimp loads all texture references from
        ///  the model file and tries to determine which of the predefined texture
        ///  types below is the best choice to match the original use of the texture
        ///  as closely as possible.
        ///  In content pipelines you'll usually define how textures have to be handled,
        ///  and the artists working on models have to conform to this specification,
        ///  regardless which 3D tool they're using.
        /// </summary>
        public enum TextureType : uint
        {
            /// <summary>
            /// Dummy value. No texture, but the value to be used as 'texture semantic'
            ///  <code>MaterialProperty.getSemantic()</code> for all material properties *not* related to textures.
            /// </summary>
            NONE = 0x0,

            /// <summary>
            /// The texture is combined with the result of the diffuse lighting equation.
            /// </summary>
            DIFFUSE = 0x1,

            /// <summary>
            /// The texture is combined with the result of the specular lighting equation.
            /// </summary>
            SPECULAR = 0x2,

            /// <summary>
            /// The texture is combined with the result of the ambient lighting equation.
            /// </summary>
            AMBIENT = 0x3,

            /// <summary>
            /// The texture is added to the result of the lighting calculation. It isn't influenced by incoming light.
            /// </summary>
            EMISSIVE = 0x4,

            /// <summary>
            /// The texture is a height map.
            /// By convention, higher grey-scale values stand for higher elevations from the base height.
            /// </summary>
            HEIGHT = 0x5,

            /// <summary>
            /// The texture is a (tangent space) normal-map.
            /// Again, there are several conventions for tangent-space normal maps. Assimp does (intentionally) not differenciate here.
            /// </summary>
            NORMALS = 0x6,

            /// <summary>
            /// The texture defines the glossiness of the material.
            /// The glossiness is in fact the exponent of the specular (phong) lighting equation. Usually there is a conversion function defined to map the linear color values in the texture to a suitable exponent. Have fun.
            /// </summary>
            SHININESS = 0x7,

            /// <summary>
            /// The texture defines per-pixel opacity. Usually 'white' means opaque and 'black' means 'transparency'. Or quite the opposite. Have fun.
            /// </summary>
            OPACITY = 0x8,

            /// <summary>
            /// Displacement texture.
            /// The exact purpose and format is application-dependent. Higher color values stand for higher vertex displacements.
            /// </summary>
            DISPLACEMENT = 0x9,

            /// <summary>
            /// Lightmap texture (aka Ambient Occlusion).
            /// Both 'Lightmaps' and dedicated 'ambient occlusion maps' are covered by this material property. The texture contains a
            /// scaling value for the final color value of a pixel. It's intensity is not affected by incoming light.
            /// </summary>
            LIGHTMAP = 0xA,

            /// <summary>
            /// Reflection texture. Contains the color of a perfect mirror reflection. Rarely used, almost nevery for real-time applications.
            /// </summary>
            REFLECTION = 0xB,

            /// <summary>
            /// Unknown texture. A texture reference that does not match any of the definitions above is considered to be 'unknown'. It is still imported, but is excluded from any further postprocessing.
            /// </summary>
            UNKNOWN = 0xC,
        }

        /// <summary>
        /// Defines all shading models supported by the library.
        /// The list of shading modes has been taken from Blender.
        /// See Blender documentation for more information. The API does
        /// not distinguish between "specular" and "diffuse" shaders (thus the
        /// specular term for diffuse shading models like Oren-Nayar remains
        /// undefined).
        /// Again, this value is just a hint. Assimp tries to select the shader whose
        /// most common implementation matches the original rendering results of the
        /// 3D modeller which wrote a particular model as closely as possible.
        /// </summary>
        public enum ShadingMode : uint
        {
            /// <summary>
            /// Flat shading. Shading is done on per-face base,
            ///  diffuse only. Also known as 'faceted shading'.
            /// </summary>
            Flat = 0x1,

            /// <summary>
            /// Simple Gouraud shading.
            /// </summary>
            Gouraud = 0x2,

            /// <summary>
            /// Phong-Shading.
            /// </summary>
            Phong = 0x3,

            /// <summary>
            /// Phong-Blinn-Shading
            /// </summary>
            Blinn = 0x4,

            /// <summary>
            /// Toon-Shading per pixel.
            ///  Also known as 'comic' shader.
            /// </summary>
            Toon = 0x5,

            /// <summary>
            /// OrenNayar-Shading per pixel. Extension to standard Lambertian shading, taking the
            /// roughness of the material into account.
            /// </summary>
            OrenNayar = 0x6,

            /// <summary>
            /// Minnaert-Shading per pixel.  Extension to standard Lambertian shading, taking the "darkness" of the material into account
            /// </summary>
            Minnaert = 0x7,

            /// <summary>
            /// CookTorrance-Shading per pixel
            ///  Special shader for metallic surfaces.
            /// </summary>
            CookTorrance = 0x8,

            /// <summary>
            /// No shading at all. Constant light influence of 1.0.
            /// </summary>
            NoShading = 0x9,

            /// <summary>
            /// Fresnel shading
            /// </summary>
            Fresnel = 0xa,
        }

        /// <summary>
        /// Defines some mixed flags for a particular texture.  This corresponds to the #AI_MATKEY_TEXFLAGS property.
        /// Usually you'll tell your cg artists how textures have to look like ...
        /// and hopefully the follow these rules. If they don't, restrict access
        /// to the coffee machine for them. That should help.
        /// However, if you use Assimp for completely generic loading purposes you
        /// might also need to process these flags in order to display as many
        /// 'unknown' 3D models as possible correctly.
        /// This corresponds to the #AI_MATKEY_TEXFLAGS property.
        /// </summary>
        public enum TextureFlags : uint
        {
            /// <summary>
            /// The texture's color values have to be inverted (componentwise 1-n)
            /// </summary>
            Invert = 0x1,

            /// <summary>
            /// Explicit request to the application to process the alpha channel of the texture.
            ///  Mutually exclusive with #aiTextureFlags_IgnoreAlpha. These
            ///  flags are set if the library can say for sure that the alpha
            ///  channel is used/is not used. If the model format does not
            ///  define this, it is left to the application to decide whether
            ///  the texture alpha channel - if any - is evaluated or not.
            /// </summary>
            UseAlpha = 0x2,

            /// <summary>
            /// Explicit request to the application to ignore the alpha channel of the texture.  Mutually exclusive with #aiTextureFlags_IgnoreAlpha.
            /// </summary>
            IgnoreAlpha = 0x4,
        }

        /// <summary>
        /// Defines alpha-blend flags.  This corresponds to the #AI_MATKEY_BLEND_FUNC property.
        /// If you're familiar with OpenGL or D3D, these flags aren't new to you.
        /// The define *how* the final color value of a pixel is computed, basing
        /// on the previous color at that pixel and the new color value from the
        /// material.
        /// The blend formula is:
        /// </summary>
        public enum BlendMode : uint
        {
            /// <summary>
            /// Default blending. SourceColor*SourceAlpha + DestColor*(1-SourceAlpha)
            /// </summary>
            Default = 0x0,

            /// <summary>
            /// Additive blending.  SourceColor*1 + DestColor*1
            /// </summary>
            Additive = 0x1,

            // We don't need more for the moment, but we might need them
            // in future versions ...
        }

        #endregion

        #region Properties
        /// <summary>
        /// Table of properties loaded.  This is built from a "MaterialProperty** mProperties" double pointer to MaterialProperty.
        /// </summary>
        public Dictionary<String, MaterialProperty> dProperties = null;

        /// <summary>
        /// Storage allocated.
        /// </summary>
        public uint iBytesAllocated;
        #endregion

        /// <summary>
        /// Constructor which builds a Material object from a pointer to an aiMaterial structure.  As a user, you should not call this by hand.
        /// <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
        /// </summary>
        /// <param name="aiMaterial*">A pointer to the face structure in the low level unmanaged wrapper.</param>
        unsafe internal Material(IntPtr p_aiMaterial)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiMaterial tMaterial = (UnmanagedAssimp.aiMaterial)Marshal.PtrToStructure(p_aiMaterial, typeof(UnmanagedAssimp.aiMaterial));
            
            // Copy the nice simple value types.
            this.iBytesAllocated = tMaterial.mNumAllocated;
            
            // Setup the iterator and iterator stride to marshal the array into managed memory.
            int iStride = sizeof(IntPtr);
            IntPtr pPtr = tMaterial.mProperties;

            // Now unmarshal each material property and add it to the dictionary.
            dProperties = new Dictionary<String, MaterialProperty>((int)tMaterial.mNumProperties);
            for (int iProperty = 0; iProperty < tMaterial.mNumProperties; ++iProperty)
            {
                // Parse the property.
                MaterialProperty oProperty = new MaterialProperty(Marshal.ReadIntPtr(pPtr));

                // Add it to the table using its name.
                dProperties[oProperty.getKey()] = oProperty;

                // Increment the pointer.
                pPtr = new IntPtr(pPtr.ToInt64() + iStride);
            }
        }

        /// <summary>
        /// Get the number of properties that this material has.
        /// </summary>
        /// <returns>An integer which contains the number of properties we have.</returns>
        public int getNumProperties()
        {
            return dProperties.Count;
        }

        /// <summary>
        /// Get the string array of property keys which this material has.
        /// </summary>
        /// <returns>An array of strings which is all the keys in our material property table.</returns>
        public String[] getPropertyKeys()
        {
            String[] tKeys = new String[dProperties.Count];
            int iKey = 0;
            foreach (String sKey in dProperties.Keys)
            {
                tKeys[iKey] = sKey;
            }
            return tKeys;
        }

        /// <summary>
        /// Read a property from this material as an 'object'.
        /// </summary>
        /// <param name="sKey">The property key to read as a string.</param>
        /// <returns>The "object" which contains the data.  You will have to cast by hand.  Null if not found.</returns>
        public object getProperty(String sKey)
        {
            MaterialProperty oMaterialProperty;
            if (dProperties.TryGetValue(sKey, out oMaterialProperty))
            {
                return oMaterialProperty.getData();
            }
            return null;
        }

        /// <summary>
        /// Read a property from this material as a reference to the "MaterialProperty" class.
        /// This returns a MaterialProperty which you should never really need to access to be honest.
        /// Use the constants or byte array.
        /// </summary>
        /// <param name="sKey">The property key to read as a string.</param>
        /// <returns>The MaterialProperty object which contains our property.  Null if not found.</returns>
        public MaterialProperty getMaterialProperty(String sKey)
        {
            MaterialProperty oMaterialProperty;
            if (dProperties.TryGetValue(sKey, out oMaterialProperty))
            {
                return oMaterialProperty;
            }
            return null;
        }

        /// <summary>
        /// Read the internal data of the property at this key and attempt to marshall it into a string.
        /// This returns a String and as marshalls it a strange way and as such is REALLY REALLY unsafe.
        /// </summary>
        /// <param name="sKey">The property key to read as a string.</param>
        /// <returns>The String which contains our data.</returns>
        public String getStringProperty(String sKey)
        {
            MaterialProperty oMaterialProperty;
            if (dProperties.TryGetValue(sKey, out oMaterialProperty))
            {
                // Get the buffer.
                byte[] tBuffer = oMaterialProperty.getByteData();

                // Read the first 4 bytes into a 32 bit integer.
                uint iLength = BitConverter.ToUInt32(tBuffer, 0);

                // Marshal the rest.
                return System.Text.UTF8Encoding.ASCII.GetString(tBuffer, 4, (int)iLength);
            }
            return null;
        }

        #region getProperty Structure (unsafe)
        /// <summary>
        /// Read a property from the material and return its data a a typed structure.
        /// This assumes that the property is a structure of the specified type.
        /// It uses unsafe code to create a copy of the value in the property data by marshalling it into the type.
        /// Not reccomended for use.
        /// </summary>
        /// <returns>A structure which contains the data in the byte array.</returns>
        public bool getProperty<T>(String sKey, out T tValue) where T : struct
        {
            // Get the material property.  Return null on failure.
            tValue = default(T);
            MaterialProperty oMaterialProperty;
            if (!dProperties.TryGetValue(sKey, out oMaterialProperty))
                return false;

            // Check it will fit by comparing sizes.
            //if (Marshal.SizeOf(typeof(T)) > oMaterialProperty.getNumDataBytes())
            //    throw new Exception("Requested destination structure size is larger than the length of the byte array of the material property.");

            // We have our property - so create a new T that happens to contain our data.
            // Use a little bit of C# black magic to copy the property into the appropriate type.
            unsafe
            {
                fixed (byte* pPtr = ((byte[])oMaterialProperty.getData()))
                {
                    tValue = (T)Marshal.PtrToStructure(new IntPtr(pPtr), typeof(T));
                    return true;
                }
            }
        }
        #endregion

        #region getProperty Array
        /// <summary>
        /// Get the byte array value for a specific key from the material.
        /// </summary>
        /// <param name="kKey">One of the constant key values like Material.BLEND_FUNC.</param>
        /// <param name="tValue[]">The value we want to place our data in.</param>
        /// <returns>True if the property exists, false if not.</returns>
        public bool getProperty(PropertyBinding<float[]> kKey, out byte[] tValue)
        {
            // Get the property.
            MaterialProperty oMaterialProperty;
            if (dProperties.TryGetValue(kKey.getName(), out oMaterialProperty))
            {
                // It exists, to convert it to the desired type.
                tValue = (byte[])oMaterialProperty.getData();
                return true;
            }

            // It does not exist so set it to something boring and return false.
            tValue = null;
            return false;
        }

        /// <summary>
        /// Get the floating-point array value for a specific key from the material.
        /// </summary>
        /// <param name="kKey">One of the constant key values like Material.BLEND_FUNC.</param>
        /// <param name="tValue[]">The value we want to place our data in.</param>
        /// <returns>True if the property exists, false if not.</returns>
        public bool getProperty(PropertyBinding<float[]> kKey, out float[] tValue)
        {
            // Get the property.
            MaterialProperty oMaterialProperty;
            if (dProperties.TryGetValue(kKey.getName(), out oMaterialProperty))
            {
                // It exists, to convert it to the desired type.
                tValue = (float[])oMaterialProperty.getData();
                return true;
            }

            // It does not exist so set it to something boring and return false.
            tValue = null;
            return false;
        }

        /// <summary>
        /// Get the integer array value for a specific key from the material.
        /// </summary>
        /// <param name="kKey">One of the constant key values like Material.BLEND_FUNC.</param>
        /// <param name="tValue[]">The value we want to place our data in.</param>
        /// <returns>True if the property exists, false if not.</returns>
        public bool getProperty(PropertyBinding<int[]> kKey, out int[] tValue)
        {
            // Get the property.
            MaterialProperty oMaterialProperty;
            if (dProperties.TryGetValue(kKey.getName(), out oMaterialProperty))
            {
                // It exists, to convert it to the desired type.
                tValue = (int[])oMaterialProperty.getData();
                return true;
            }

            // It does not exist so set it to something boring and return false.
            tValue = null;
            return false;
        }
        #endregion

        #region getProperty Value Types
        /// <summary>
        /// Get the floating-point value for a specific key from the material.
        /// </summary>
        /// <param name="kKey">One of the constant key values like Material.BLEND_FUNC.</param>
        /// <param name="fValue">The value we want to place our data in.</param>
        /// <returns>True if the property exists, false if not.</returns>
	    public bool getProperty(PropertyBinding<float> kKey, out float fValue)
        {
            // Get the property.
            MaterialProperty oMaterialProperty;
            if (dProperties.TryGetValue(kKey.getName(), out oMaterialProperty))
            {
                // It exists, to convert it to the desired type.
                fValue = BitConverter.ToSingle(oMaterialProperty.getByteData(), 0);
                return true;
            }

            // It does not exist so set it to something boring and return false.
            fValue = 0f;
            return false;
	    }

    /// <summary>
    /// Get the 32 bit integer value for a specific key from the material.
    /// </summary>
    /// <param name="kKey">One of the constant key values like Material.BLEND_FUNC.</param>
    /// <param name="iValue">The value we want to place our data in.</param>
    /// <returns>True if the property exists, false if not.</returns>
	    public bool getProperty(PropertyBinding<int> kKey, out int iValue)
        {
            // Get the property.
            MaterialProperty oMaterialProperty;
            if (dProperties.TryGetValue(kKey.getName(), out oMaterialProperty))
            {
                // It exists, to convert it to the desired type.
                iValue = BitConverter.ToInt32(oMaterialProperty.getByteData(), 0);
                return true;
            }

            // It does not exist so set it to something boring and return false.
            iValue = 0;
            return false;
        }
        
        #endregion

        #region getProperty String
        /// <summary>
        /// Get the string value for a specific key from the material.
        /// </summary>
        /// <param name="kKey">One of the constant key values like Material.BLEND_FUNC.</param>
        /// <param name="sValue">The value we want to place our data in.</param>
        /// <returns>True if the property exists, false if not.</returns>
        public bool getProperty(PropertyBinding<String> kKey, out String sValue)
        {
            // Use the unsafe thing to get the aiString.
            sValue = null;
            aiString tString;
            if (!getProperty<aiString>(kKey.getName(), out tString))
                return false;

            // We have the string ok so lets convert the text to a string and then be done with it!
            sValue = "" + tString.data;
            return true;
        }
        #endregion

    }
}
