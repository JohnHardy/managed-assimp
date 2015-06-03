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
using System.Text;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Assimp.ManagedAssimp;
using Assimp.ManagedAssimp.Unmanaged;

namespace Assimp
{
    /// <summary>
    /// This class wraps the importer functionality of Assimp in a safe manner.
    /// Please note that this wrapper is not finished and only implements a subset of Assimp's functionality.
    /// 
    /// You may need to disable the pInvokeStackImbalance MDA.  It will appear in the Managed Debugging Assistants
    /// list in the Exceptions dialog box (which is displayed when you click Exceptions on the Debug menu).
    /// </summary>
    /// <author>John Hardy - john at highwire-dtc dot com</author>
    /// <date>21 July 2009</date>
    public abstract class AssimpImporter
    {
        /// <summary>
        /// The path and name of the DLL file.
        /// </summary>
        public const String DLLName = "assimp.dll";

        /// <summary>
        /// Loads a ship from a file using the Assimp library and returns a reference to a 'Scene' object which
        /// contains all the data in that resource.  It is important to note that this will first load the mesh with
        /// Assimp into 'unmanaged' memory (allocated by the C/C++ DLL) and then copy it into the managed memory allocated by the 'Scene'.
        /// As such, calling this will typically take longer and, for the duration of this method call, use double the memory of the
        /// standard assimp import call.  As such, if speed or memory are of concern to you then consider using the the
        /// unmanaged interface in 'Unmanaged.Assimp'.
        /// </summary>
        /// <param name="sResourcePath">The path to the mesh resource.  E.g. "data\characters\yourmum.x"</param>
        /// <param name="eFlags">The postprocessing flags that are specified.  This is (uint) compatible with entries in the PostProcessingFlags enum.</param>
        /// <returns>A 'Scene' object which represents the loaded resource.  If load failed, null is returned.</returns>
        public static Scene readFile(String sResourcePath, aiPostProcessSteps eFlags)
        {
            // Ensure we have a valid path.
            if (sResourcePath == null)
                throw new ArgumentNullException("The resource path cannot be null.");

            if (sResourcePath == "")
                throw new ArgumentNullException("The resource path cannot be null.");

            // Drop into unsafe mode so we can access the pointers in the Assimp library.
            unsafe
            {
                // Declare a pointer to the scene.
                UnmanagedAssimp.aiScene* pScene;

                try
                {
                    // Try and load the mesh as specfied above.
                    //UnmanagedAssimp.aiSetImportPropertyFloat(UnmanagedAssimp.AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE, 80.0f);
                    //UnmanagedAssimp.aiSetImportPropertyFloat(UnmanagedAssimp.AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE, 80.0f);
                        
                    //UnmanagedAssimp.aiSetImportPropertyInteger(UnmanagedAssimp.AI_CONFIG_IMPORT_ASE_RECONSTRUCT_NORMALS, 1);

                    // This is commented out because we want the pre-transform vertices step to flatten the mesh.
                    //UnmanagedAssimp.aiSetImportPropertyInteger(UnmanagedAssimp.AI_CONFIG_PP_PTV_KEEP_HIERARCHY, 1);

                    // This is commented out because we do not normalise the mesh.
                    //UnmanagedAssimp.aiSetImportPropertyInteger(UnmanagedAssimp.AI_CONFIG_PP_PTV_NORMALIZE, 0);

                    // Force Assimp to ignore all lines and points on import.
                    int iIgnoreFaceTypes = (int)(UnmanagedAssimp.aiPrimitiveType.aiPrimitiveType_LINE | UnmanagedAssimp.aiPrimitiveType.aiPrimitiveType_POINT);
                    UnmanagedAssimp.aiSetImportPropertyInteger(UnmanagedAssimp.AI_CONFIG_PP_SBP_REMOVE, iIgnoreFaceTypes);

                    pScene = UnmanagedAssimp.aiImportFile(sResourcePath, (uint)eFlags);
                }
                catch (Exception pError)
                {
                    throw pError;
                    // Something went wrong so make it the users problem!
                    //throw new Exception("Assimp suffered an error when converting loading the resource into unmanaged memory.  See the inner exception for details.", pError);
                }

                // A C# pointer to the scene.
                IntPtr pScenePtr = new IntPtr(pScene);

                // If the mesh did not load correctly, return null.
                if (pScenePtr == IntPtr.Zero)
                    return null;

                try
                {
                    // Now we want to parse the scene with the managed wrapper.
                    Scene oScene = new Scene(pScenePtr, sResourcePath);

                    // Now that is done we can release the Assimp stuff.
                    UnmanagedAssimp.aiReleaseImport(pScene);

                    // Success - return a reference to our newly created scene!
                    return oScene;
                }
                catch (Exception pError)
                {
                    throw pError;
                    // There was an error with the managed library.  Take responsibility for my bad code by forwarding the error to YOU!
                    //throw new Exception("Error converting Assimp resource data to managed memory.  See the inner exception for details.", pError);
                }
            }
        }

        /// <summary>
        /// Returns the error text of the last failed import process.
        /// </summary>
        /// <returns>A textual description of the error that occurred at the last import process. Null if there was no error.</returns>
        public static String getErrorString()
        {
            unsafe
            {
                return new String(UnmanagedAssimp.aiGetErrorString());
            }
        }

        /// <summary>
        /// Returns whether a given file extension is supported by ASSIMP.  Must include a leading dot '.'. Example: ".3ds", ".md3".
        /// </summary>
        /// <param name="sExtension">Extension for which the function queries support.  Must include a leading dot '.'. Example: ".3ds", ".md3"</param>
        /// <returns>1 if the extension is supported, 0 otherwise.</returns>
        public static bool isExtensionSupported(String sExtension)
        {
            // Firstly convert the string to byte array since .NET uses unicode.
            byte[] tExtension = UnicodeEncoding.ASCII.GetBytes(sExtension);

            // Then make the call!
            unsafe
            {
                fixed (byte* pPtr = &tExtension[0])
                {
                    return (UnmanagedAssimp.aiIsExtensionSupported(pPtr) == 1);
                }
            }
            
        }

        /// <summary>
        /// Get a full list of all file extensions generally supported by ASSIMP.
        /// If a file extension is contained in the list this does, of course, not mean that ASSIMP is able to load all files with this extension.
        /// </summary>
        /// <returns>An array of strings, where each string contains a supported format.</returns>
        public static String[] getExtensionList()
        {
            // Read the extensions.
            aiString tString;
            UnmanagedAssimp.aiGetExtensionList(out tString);

            // Split by ";" and return it.
            return tString.data.Split(';');
        }

        /// <summary>
        /// Save a scene to a file by serialising the managed data.
        /// </summary>
        /// <param name="sPath">The path to save the file.</param>
        /// <param name="oScene">A reference to the scene we want to save.</param>
        public static void marshalScene(String sFile, Scene oScene)
        {
            IFormatter pFormatter = new BinaryFormatter();
            Stream pOutput = new FileStream(sFile, FileMode.Create, FileAccess.Write, FileShare.None);
            pFormatter.Serialize(pOutput, oScene);
            pOutput.Close();
            pOutput.Dispose();
        }

        /// <summary>
        /// Load a scene from a file that was serialised by this code.
        /// </summary>
        /// <param name="sPath">The path to load the file from.</param>
        /// <returns>A reference to the newly loaded scene.  Null on failure.</returns>
        public static Scene unmarshalScene(String sFile)
        {
            IFormatter pFormatter = new BinaryFormatter();
            Stream pInput = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            Scene pScene = pFormatter.Deserialize(pInput) as Scene;
            pInput.Close();
            pInput.Dispose();
            return pScene;
        }
    }
}
