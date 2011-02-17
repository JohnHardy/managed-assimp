using System;
using System.Runtime.InteropServices;

using Assimp.ManagedAssimp.Unmanaged;

namespace Assimp.ManagedAssimp
{
    /**
     * <summary>Represents an embedded texture. Sometimes textures are not referenced with a path, instead they are directly embedded into the model file.</summary>
     * Example file formats doing this include MDL3, MDL5 and MDL7 (3D GameStudio). Embedded textures are converted to an array of color values (RGBA).
     * @author John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk
     * @date 20 July 2009
     * @version 1.0
     */
    [Serializable]
    public class Texture
    {
        #region Properties.
        /** <summary>The width of the texture.</summary> */
        private uint iWidth = 0;

        /** <summary>The height of the texture.</summary> */
        private uint iHeight = 0;

        /**  <summary>Does this texture hiave an alpha channel.  0xFFFFFFFF means already computed. </summary> */
        private uint iNeedAlpha = 0xFFFFFFFF;

        /** <summary>Texture data byte array.</summary> */
        private aiTexel[] tTexels = null;
        #endregion

        /**
         * <summary>Constructor which builds a Texture object from a pointer to an aiTexture structure.  As a user, you should not call this by hand.</summary>
         * <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
         * @param p_aiAnimation* A pointer to the texture structure in the low level unmanaged wrapper.
         */
        unsafe internal Texture(IntPtr p_aiTexture)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiTexture tTexture = (UnmanagedAssimp.aiTexture)Marshal.PtrToStructure(p_aiTexture, typeof(UnmanagedAssimp.aiTexture));

            // Copy over the nice, simple value types into managed memory.  Value types copy the entire structure rather than just the pointer to it.
            this.iWidth     = tTexture.mWidth;
            this.iHeight    = tTexture.mHeight;
            this.iNeedAlpha = 0xFFFFFFFF;

            // Copy the properties over into our class as marshalled managed memory.
            // Note that with strings, prepending an empty string "" forces a copy.

            // Copy over the array of 'texels' into managed memory belonging to this class instance.
            // - We drop the ARGB format of the texel in favour of a byte array here.
            tTexels = UnmanagedAssimp.MarshalArray<aiTexel>(new IntPtr(tTexture.pcData), tTexture.mWidth * tTexture.mHeight);
        }

	    /**
	     * <summary>Retrieve the height of the texture image.</summary>
	     * @return Height, in pixels
	     */
	    public uint getHeight()
        {
		    return iHeight;
	    }

	    /**
	     * <summary>Retrieve the width of the texture image.</summary>
	     * @return Width, in pixels
	     */
	    public uint getWidth()
        {
		    return iWidth;
	    }

	    /**
	     * <summary>Returns whether the texture uses its alpha channel.</summary>
	     * @return <code>true</code> if at least one pixel has an alpha value below 0xFF.
	     */
	    public bool hasAlphaChannel()
        {
            // Have we already computed the alpha value and have texture data?
            if (iNeedAlpha == 0xFFFFFFFF & tTexels != null)
            {
                // Nope, do so.
                for (int iTexel = 0; iTexel < tTexels.Length; ++iTexel)
                {
                    // If we find alpha, return true and set flag!
                    if (tTexels[iTexel].a < 255)
                    {
                        iNeedAlpha = 0x01;
                        return true;
                    }
                }

                // Not found any alpha.
                iNeedAlpha = 0x00;
                return false;
            }

            // It is now computed, so return if we used alpha or not.
            return (iNeedAlpha == 0x01);
	    }

	    /**
	     * <summary>Get the color at a given position of the texture.</summary>
	     * @param x X coordinate, zero based
	     * @param y Y coordinate, zero based
	     * @return aiTexel colour at this position
	     */
	    public aiTexel getPixel(int x, int y)
        {
		    if (x < iWidth && y < iHeight)
                return tTexels[y * iWidth + x];
            throw new IndexOutOfRangeException("Array index '"+y * iWidth + x+"' is less than texture bounds '"+iWidth*iHeight+"'");
	    }

	    /**
	     * <summary>Get a pointer to the color buffer of the texture.</summary>
	     * @return Array of aiTexel, size: width * height
	     */
	    public aiTexel[] getColorArray()
        {
		    return tTexels;
	    }

	    /**
	     * <summary>Convert the texture into a <code>System.Drawing.Bitmap</code>.</summary>
         * This will create a new bitmap instance.
         * This is perhaps the slowest method in the entire world ever.
	     * @return Valid <code>System.Drawing.Bitmap</code> object containing a copy of the texture image.  The bitmap is in the format ARGB.
	     */
	    public unsafe System.Drawing.Bitmap toBitmap()
        {
            // Check we have data..
            if (tTexels == null)
                throw new Exception("No texel data in Texture!");

            // Create a new bitmap with the required pixel format.
            System.Drawing.Bitmap pBitmap = new System.Drawing.Bitmap((int)iWidth, (int)iHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Set all the pixels one by one... yuck.. sorry.
            int w = 0;
            int h = 0;
            foreach (aiTexel tTexel in tTexels)
            {
                pBitmap.SetPixel(w, h, System.Drawing.Color.FromArgb(tTexel.a, tTexel.r, tTexel.g, tTexel.b));
                ++w;
                if (w > iWidth)
                {
                    w = 0;
                    ++h;
                }
            }

            // Return our new bitmap.
            return pBitmap;
	    }
    }
}
