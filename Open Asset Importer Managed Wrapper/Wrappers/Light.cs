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
    /// Enumerates all supported types of light sources in a scene.
    /// </summary>
    public enum LightType : int
    {
        /// <summary>
        /// Undefined light type.
        /// </summary>
		UNDEFINED = 0x0,

        /// <summary>
        ///  A directional light source has a well-defined direction but is
        /// infinitely far away. That's quite a good approximation for sun light.
        /// </summary>
        DIRECTIONAL = 0x1,

        /// <summary>
        /// direction - it emmits light in all directions. A normal bulb is a
        /// point light.
        /// </summary>
        POINT = 0x2,

        /// <summary>
        /// position and a direction it is pointing to. A good example for a spot
        /// light is a light spot in sport arenas.
        /// </summary>
		SPOT = 0x3,
	}

    /// <summary>
    /// Describes a virtual light source in the scene.  Lights have a representation in the node graph and can be animated.
    /// </summary>
    /// <author>John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk</author>
    /// <date>20 July 2009</date>
    [Serializable]
    public class Light
    {
        #region Properties
        /// <summary>
        /// The name of the light source.  There must be a node in the secegraph with the same name.  This node specifies the position of the light in the scene hierarchy and can be animated.
        /// </summary>
        private String sName;

        /// <summary>
        /// The type of the light source.  aiLightSource_UNDEFINED is not a valid value for this member.
        /// </summary>
        private LightType eType;

        /// <summary>
        /// Position of the light source in space. Relative to the transformation of the node corresponding to the light.  The position is undefined for directional lights.
        /// </summary>
        private aiVector3D mPosition;

        /// <summary>
        /// Direction of the light source in space. Relative to the transformation of the node corresponding to the light.  The direction is undefined for point lights. The vector may be normalized, but it needn't.
        /// </summary>
        private aiVector3D mDirection;

        /// <summary>
        /// Constant light attenuation factor.  The intensity of the light source at a given distance 'd' from the light's position is <code>Atten = 1/( att0 + att1#d + att2#d*d)</code> This member corresponds to the att0 variable in the equation.  Naturally undefined for directional lights.
        /// </summary>
        private float mAttenuationConstant;

        /// <summary>
        /// Linear light attenuation factor.  The intensity of the light source at a given distance 'd' from the light's position is <code>Atten = 1/( att0 + att1#d + att2#d*d)</code> This member corresponds to the att1 variable in the equation.  Naturally undefined for directional lights.
        /// </summary>
        private float mAttenuationLinear;

        /// <summary>
        /// Quadratic light attenuation factor.  The intensity of the light source at a given distance 'd' from the light's position is <code>Atten = 1/( att0 + att1#d + att2#d*d)</code>  This member corresponds to the att2 variable in the equation.  Naturally undefined for directional lights.
        /// </summary>
        private float mAttenuationQuadratic;

        /// <summary>
        /// Diffuse color of the light source.  The diffuse light color is multiplied with the diffuse  material color to obtain the final color that contributes to the diffuse shading term.
        /// </summary>
        private aiColor3D mColorDiffuse;

        /// <summary>
        /// Specular color of the light source.  The specular light color is multiplied with the specular material color to obtain the final color that contributes to the specular shading term.
        /// </summary>
        private aiColor3D mColorSpecular;

        /// <summary>
        /// Ambient color of the light source.  The ambient light color is multiplied with the ambient material color to obtain the final color that contributes to the ambient shading term. Most renderers will ignore this value it, is just a remaining of the fixed-function pipeline that is still supported by quite many file formats.
        /// </summary>
        private aiColor3D mColorAmbient;

        /// <summary>
        /// Inner angle of a spot light's light cone.  The spot light has maximum influence on objects inside this angle. The angle is given in radians. It is 2PI for point lights and undefined for directional lights.
        /// </summary>
        private float mAngleInnerCone;

        /// <summary>
        /// Outer angle of a spot light's light cone.  The spot light does not affect objects outside this angle. The angle is given in radians. It is 2PI for point lights and undefined for directional lights. The outer angle must be greater than or equal to the inner angle. It is assumed that the application uses a smooth interpolation between the inner and the outer cone of the spot light.
        /// </summary>
        private float mAngleOuterCone;
        #endregion

        /// <summary>
        /// Constructor which builds a Light object from a pointer to an aiLight structure.  As a user, you should not call this by hand.
        /// <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
        /// </summary>
        /// <param name="aiLight*">A pointer to the face structure in the low level unmanaged wrapper.</param>
        unsafe internal Light(IntPtr p_aiLight)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiLight tLight = (UnmanagedAssimp.aiLight)Marshal.PtrToStructure(p_aiLight, typeof(UnmanagedAssimp.aiLight));

            // Copy over the nice, simple value types into managed memory.  Value types copy the entire structure rather than just the pointer to it.
            this.mAngleOuterCone        = tLight.mAngleOuterCone;
            this.mAngleInnerCone        = tLight.mAngleInnerCone;
            this.mColorAmbient          = tLight.mColorAmbient;
            this.mColorSpecular         = tLight.mColorSpecular;
            this.mColorDiffuse          = tLight.mColorDiffuse;
            this.mAttenuationQuadratic  = tLight.mAttenuationQuadratic;
            this.mAttenuationLinear     = tLight.mAttenuationLinear;
            this.mAttenuationConstant   = tLight.mAttenuationConstant;
            this.mDirection             = tLight.mDirection;
            this.mPosition              = tLight.mPosition;
            this.eType                  = (LightType)tLight.mType;

            // Copy the properties over into our class as marshalled managed memory.
            // Note that with strings, prepending an empty string "" forces a copy.
            this.sName                  = "" + tLight.mName.data;
        }

        /// <summary>
        /// Get the name of the light source.
        /// There must be a node in the scenegraph with the same name. This node
        /// specifies the position of the light in the scene hierarchy and can be
        /// animated.
        /// </summary>
	    public String GetName()
        {
		    return sName;
	    }

        /// <summary>
        /// Get the type of the light source.
        /// </summary>
        /// <returns>The enumerate light type.</returns>
	    public LightType GetLightType()
        {
		    return eType;
	    }

        /// <summary>
        /// Get the position of the light source in space. Relative to the
        /// transformation of the node corresponding to the light.
        /// The position is undefined for directional lights.
        /// </summary>
        /// <returns>aiVector3D with x,y,z values.</returns>
        public aiVector3D GetPosition()
        {
		    return mPosition;
	    }

        /// <summary>
        /// Get the direction of the light source in space. Relative to the
        /// transformation of the node corresponding to the light.
        /// The direction is undefined for point lights. The vector may be
        /// normalized, but it needn't.
        /// </summary>
        /// <returns>aiVector3D with x,y,z values.</returns>
        public aiVector3D GetDirection()
        {
		    return mDirection;
	    }

        /// <summary>
        /// Get the constant light attenuation factor.
        /// The intensity of the light source at a given distance 'd' from the
        /// light's position is
        /// </summary>
	    public float GetAttenuationConstant()
        {
		    return mAttenuationConstant;
	    }

        /// <summary>
        /// Get the linear light attenuation factor.
        /// The intensity of the light source at a given distance 'd' from the
        /// light's position is
        /// </summary>
	    public float GetAttenuationLinear()
        {
		    return mAttenuationLinear;
	    }

        /// <summary>
        /// Get the quadratic light attenuation factor.
        /// The intensity of the light source at a given distance 'd' from the
        /// light's position is
        /// </summary>
	    public float GetAttenuationQuadratic()
        {
		    return mAttenuationQuadratic;
	    }

        /// <summary>
        /// Get the diffuse color of the light source.
        /// </summary>
	    public aiColor3D GetColorDiffuse()
        {
		    return mColorDiffuse;
	    }

        /// <summary>
        /// Get the specular color of the light source.
        /// The specular light color is multiplied with the specular material color
        /// to obtain the final color that contributes to the specular shading term.
        /// </summary>
	    public aiColor3D GetColorSpecular() 
        {
		    return mColorSpecular;
	    }

        /// <summary>
        /// Get the ambient color of the light source.
        /// The ambient light color is multiplied with the ambient material color to
        /// obtain the final color that contributes to the ambient shading term. Most
        /// renderers will ignore this value it, is just a remaining of the
        /// fixed-function pipeline that is still supported by quite many file
        /// formats.
        /// </summary>
	    public aiColor3D GetColorAmbient()
        {
		    return mColorAmbient;
	    }

        /// <summary>
        /// Get the inner angle of a spot light's light cone.
        /// The spot light has maximum influence on objects inside this angle. The
        /// angle is given in radians. It is 2PI for point lights and undefined for
        /// directional lights.
        /// </summary>
	    public float GetAngleInnerCone()
        {
		    return mAngleInnerCone;
	    }

        /// <summary>
        /// Get the outer angle of a spot light's light cone.
        /// The spot light does not affect objects outside this angle. The angle is
        /// given in radians. It is 2PI for point lights and undefined for
        /// directional lights. The outer angle must be greater than or equal to the
        /// inner angle. It is assumed that the application uses a smooth
        /// interpolation between the inner and the outer cone of the spot light.
        /// </summary>
	    public float GetAngleOuterCone()
        {
		    return mAngleOuterCone;
	    }
    }
}
