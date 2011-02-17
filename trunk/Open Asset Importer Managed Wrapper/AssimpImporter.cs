using System;
using System.Text;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Assimp.ManagedAssimp.Unmanaged;

namespace Assimp.ManagedAssimp
{
    /**
     * <summary>This class wraps the importer functionality of Assimp in a safe manner.</summary>
     * Please note that this wrapper is not finished and only implements a subset of Assimp's functionality.
     * @author John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk
     * @date 21 July 2009
     * @version 1.0
     */
    public abstract class AssimpImporter
    {
        /**
         * <summary>Loads a ship from a file using the Assimp library and returns a reference to a 'Scene' object which
         * contains all the data in that resource.  It is important to note that this will first load the mesh with 
         * Assimp into 'unmanaged' memory (allocated by the C/C++ DLL) and then copy it into the managed memory allocated by the 'Scene'.
         * As such, calling this will typically take longer and, for the duration of this method call, use double the memory of the
         * standard assimp import call.  As such, if speed or memory are of concern to you then consider using the the
         * unmanaged interface in 'Unmanaged.Assimp'.</summary>
         * @param sResourcePath The path to the mesh resource.  E.g. "data\characters\yourmum.x"
         * @param eFlags The postprocessing flags that are specified.  This is (uint) compatible with entries in the PostProcessingFlags enum.
         * @return A 'Scene' object which represents the loaded resource.  If load failed, null is returned.
         */
        public static Scene readFile(String sResourcePath, aiPostProcessSteps eFlags)
        {
            // Ensure we have a valid path.
            if (sResourcePath == null)
                return null;

            if (sResourcePath == "")
                return null;
            
            // Firstly convert the resource string to byte array since .NET uses unicode (that's 16 bit chars).
            byte[] tResourcePath = UnicodeEncoding.ASCII.GetBytes(sResourcePath);

            // Drop into unsafe mode so we can access the pointers in the Assimp library.
            unsafe
            {
                // A pointer to the start of the string resource for assimp to use to load it.
                fixed (byte* pFileName = &tResourcePath[0])
                {
                    // Declare a pointer to the scene.
                    UnmanagedAssimp.aiScene* pScene;

                    try
                    {
                        // Try and load the mesh as specfied above.
                        //UnmanagedAssimp.aiSetImportPropertyFloat(UnmanagedAssimp.AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE, 45.0f);
                        //UnmanagedAssimp.aiSetImportPropertyFloat(UnmanagedAssimp.AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE, 45.0f);
                        pScene = UnmanagedAssimp.aiImportFile(pFileName, (uint)eFlags);
                    }
                    catch (Exception pError)
                    {
                        // Something went wrong so make it the users problem!
                        throw new Exception("Assimp suffered an error when converting loading the resource into unmanaged memory.  See the inner exception for details.", pError);
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
                        // There was an error with the managed library.  Take responsibility for my bad code by forwarding the error to YOU!
                        throw new Exception("Error converting Assimp resource data to managed memory.  See the inner exception for details.", pError);
                    }
                }
            }
        }

        /**
         * <summary>Returns the error text of the last failed import process.</summary>
         * @return A textual description of the error that occurred at the last import process. Null if there was no error.
         */
        public static String getErrorString()
        {
            unsafe
            {
                return new String(UnmanagedAssimp.aiGetErrorString());
            }
        }

        /**
         * <summary>Returns whether a given file extension is supported by ASSIMP.  Must include a leading dot '.'. Example: ".3ds", ".md3".</summary>
         * @param sExtension Extension for which the function queries support.  Must include a leading dot '.'. Example: ".3ds", ".md3"
         * @return 1 if the extension is supported, 0 otherwise.
         */
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

        /**
         * <summary>Get a full list of all file extensions generally supported by ASSIMP.</summary>
         * If a file extension is contained in the list this does, of course, not mean that ASSIMP is able to load all files with this extension.
         * @return An array of strings, where each string contains a supported format.
         */
        public static String[] getExtensionList()
        {
            // Read the extensions.
            aiString tString;
            UnmanagedAssimp.aiGetExtensionList(out tString);

            // Split by ";" and return it.
            return tString.data.Split(';');
        }

        /**
         * <summary>Save a scene to a file by serialising the managed data.</summary>
         * @param sPath The path to save the file.
         * @param oScene A reference to the scene we want to save.
         */
        public static void marshalScene(String sFile, Scene oScene)
        {
            IFormatter pFormatter = new BinaryFormatter();
            Stream pOutput = new FileStream(sFile, FileMode.Create, FileAccess.Write, FileShare.None);
            pFormatter.Serialize(pOutput, oScene);
            pOutput.Close();
            pOutput.Dispose();
        }

        /**
         * <summary>Load a scene from a file that was serialised by this code.</summary>
         * @param sPath The path to load the file from.
         * @return A reference to the newly loaded scene.  Null on failure.
         */
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
