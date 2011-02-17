using System;
using System.Runtime.InteropServices;

using Assimp.ManagedAssimp.Unmanaged;

namespace Assimp.ManagedAssimp
{
    /**
     * <summary>An animation consists of keyframe data for a number of bones. For each bone affected by the animation a separate series of data is given.</summary>
     * There can be multiple animations in a single scene.
     * @author John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk
     * @date 20 July 2009
     * @version 1.0
     */
    [Serializable]
    public class Animation
    {
        #region Properties.
        /** <summary>The name of the animation.</summary> */
        private String sName = "";

        /** <summary>Duration of the animation in ticks.</summary> */
        private double fDuration = 0.0;

        /**  <summary>Ticks per second. 0 if not specified in the imported file.</summary> */
        private double fTicksPerSecond = 0.0;

        /** <summary>Bone animation channels.</summary> */
        private NodeAnimation[] tChannels = null;
        #endregion

        /**
         * <summary>Constructor which builds an animation object from a pointer to an aiAnimation structure.  As a user, you should not call this by hand.</summary>
         * <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
         * @param p_aiAnimation* A pointer to the animation structure in the low level unmanaged wrapper.
         */
        unsafe internal Animation(IntPtr p_aiAnimation)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiAnimation tAnimation = (UnmanagedAssimp.aiAnimation)Marshal.PtrToStructure(p_aiAnimation, typeof(UnmanagedAssimp.aiAnimation));
            
            // Copy over the nice, simple value types into managed memory.  Value types copy the entire structure rather than just the pointer to it.
            this.fTicksPerSecond    = tAnimation.mTicksPerSecond;
            this.fDuration          = tAnimation.mDuration;

            // Copy the properties over into our class as marshalled managed memory.
            // Note that with strings, prepending an empty string "" forces a copy.
            this.sName              = "" + tAnimation.mName.data;

            // Debug notify.
            //Console.WriteLine("-- Parsing Animation: " + sName + ", duration='" + fDuration + "' and tps='" + fTicksPerSecond + "'.");

            // Copy over the array of node animations into managed memory belonging to this class instance.
            IntPtr pPtr = tAnimation.mChannels;
            int iStride = sizeof(IntPtr);

            tChannels = new NodeAnimation[tAnimation.mNumChannels];
            for (int iElement = 0; iElement < tAnimation.mNumChannels; ++iElement)
            {
                // Copy the element at this index and then increment the channel pointer (which should be sizeof(IntPtr) away.
                tChannels[iElement] = new NodeAnimation(Marshal.ReadIntPtr(pPtr));
                pPtr = new IntPtr(pPtr.ToInt64() + iStride);
            }
        }

	    /**
	     * <summary>Returns the name of the animation channel.</summary>
	     * @return If the modeling package this data was exported from does support
	     * only a single animation channel, this name is usually <code>""</code>.
	     */
	    public String getName()
        {
            return sName;
	    }

	    /**
	     * <summary>Returns the total duration of the animation, in ticks.</summary>
	     * @return Total duration
	     */
	    public double getDuration()
        {
		    return fDuration;
	    }

	    /**
	     * <summary>Returns the ticks per second count.</summary>
	     * @return 0 if not specified in the imported file
	     */
	    public double getTicksPerSecond()
        {
		    return fTicksPerSecond;
	    }

	    /**
	     * <summary>Returns the number of bone animation channels.</summary>
	     * @return This value is never 0
	     */
	    public int getNumNodeAnimationChannels()
        {
		    return tChannels.Length;
	    }

	    /**
	     * <summary>Returns the list of all bone animation channels.</summary>
	     * @return This value is never <code>null</code>
	     */
        public NodeAnimation[] getNodeAnimationChannels()
        {
            return tChannels;
	    }

    }
}
