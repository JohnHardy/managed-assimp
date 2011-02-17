using System;
using System.Runtime.InteropServices;
using System.Security;

/**
 * This file contains value type structures and enumerations which are common across the managed and unmanaged wrappers.
 * Note that these structures are used by the managed assimp.
 */
namespace Assimp.ManagedAssimp
{
    #region aiPostProcessingSteps Enumeration
    /**
     * <summary>See the massive ammount of documentation here: http://assimp.sourceforge.net/lib_html/ai_post_process_8h.html#64795260b95f5a4b3f3dc1be4f52e410</summary>
     */
    public enum aiPostProcessSteps : uint
    {
        aiProcess_CalcTangentSpace = 0x1,
        aiProcess_JoinIdenticalVertices = 0x2,
        aiProcess_MakeLeftHanded = 0x4,
        aiProcess_Triangulate = 0x8,
        aiProcess_RemoveComponent = 0x10,
        aiProcess_GenNormals = 0x20,
        aiProcess_GenSmoothNormals = 0x40,
        aiProcess_SplitLargeMeshes = 0x80,
        aiProcess_PreTransformVertices = 0x100,
        aiProcess_LimitBoneWeights = 0x200,
        aiProcess_ValidateDataStructure = 0x400,
        aiProcess_ImproveCacheLocality = 0x800,
        aiProcess_RemoveRedundantMaterials = 0x1000,
        aiProcess_FixInfacingNormals = 0x2000,
        aiProcess_SortByPType = 0x8000,
        aiProcess_FindDegenerates = 0x10000,
        aiProcess_FindInvalidData = 0x20000,
        aiProcess_GenUVCoords = 0x40000,
        aiProcess_TransformUVCoords = 0x80000,
        aiProcess_FindInstances = 0x100000,
        aiProcess_OptimizeMeshes = 0x200000,
        aiProcess_OptimizeGraph = 0x400000,
        aiProcess_FlipUVs = 0x800000,
        aiProcess_FlipWindingOrder = 0x1000000
    }
    #endregion

    #region aiMatrix3x3 Structure
    /** <summary>A 3x3 Matrix.  These are in row-major (opengl, not directx) format.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiMatrix3x3
    {
        /** <summary></summary> */
        public float a1;

        /** <summary></summary> */
        public float a2;

        /** <summary></summary> */
        public float a3;

        /** <summary></summary> */
        public float b1;

        /** <summary></summary> */
        public float b2;

        /** <summary></summary> */
        public float b3;

        /** <summary></summary> */
        public float c1;

        /** <summary></summary> */
        public float c2;

        /** <summary></summary> */
        public float c3;
    }
    #endregion

    #region aiMatrix4x4 Structure
    /** <summary>A 4x4 Matrix.  These are in row-major (opengl, not directx) format.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiMatrix4x4
    {
        /** <summary></summary> */
        public float a1;

        /** <summary></summary> */
        public float a2;

        /** <summary></summary> */
        public float a3;

        /** <summary></summary> */
        public float a4;

        /** <summary></summary> */
        public float b1;

        /** <summary></summary> */
        public float b2;

        /** <summary></summary> */
        public float b3;

        /** <summary></summary> */
        public float b4;

        /** <summary></summary> */
        public float c1;

        /** <summary></summary> */
        public float c2;

        /** <summary></summary> */
        public float c3;

        /** <summary></summary> */
        public float c4;

        /** <summary></summary> */
        public float d1;

        /** <summary></summary> */
        public float d2;

        /** <summary></summary> */
        public float d3;

        /** <summary></summary> */
        public float d4;
    }
    #endregion

    #region aiQuaternion Structure
    /** <summary>A quaternion as defined by assimp. See 'aiQuaternion.h' for details.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiQuaternion
    {
        /** <summary>W component of the quaternion.</summary> */
        public float w;

        /** <summary>X component of the quaternion.</summary> */
        public float x;

        /** <summary>Y component of the quaternion.</summary> */
        public float y;

        /** <summary>Z component of the quaternion.</summary> */
        public float z;
    }
    #endregion

    #region aiTexel Structure
    /** <summary> aiTexel, or texture element (also texture pixel) is the fundamental uint of texture space , used in computer graphics.  See 'aiTexture.h' for details.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiTexel
    {
        /** <summary>Blue.</summary> */
        public byte b;

        /** <summary>Green.</summary> */
        public byte g;

        /** <summary>Red.</summary> */
        public byte r;

        /** <summary>Alpha.</summary> */
        public byte a;

    }
    #endregion

    #region aiPlane Structure
    /** <summary>A flat surface extending infinitely in all directions.  See 'aiTypes.h' for details.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiPlane
    {
        /** <summary>From the plane equation.</summary> */
        public float a;

        /** <summary>From the plane equation.</summary> */
        public float b;

        /** <summary>From the plane equation.</summary> */
        public float c;

        /** <summary>From the plane equation.</summary> */
        public float d;

    }
    #endregion

    #region aiColor3D Structure
    /** <summary>A colour with only RGB components.  See 'aiTypes.h' for details.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiColor3D
    {
        /** <summary>Red.</summary> */
        public float r;

        /** <summary>Green.</summary> */
        public float g;

        /** <summary>Blue.</summary> */
        public float b;

        /** <summary>Return a string representation of this structure.</summary> */
        public override string ToString()
        {
            return "aiColor3D(RGB)(" + r + ", " + g + ", " + b + ")";
        }
    }
    #endregion

    #region aiColor4D Structure
    /** <summary>A colour with RGBA components.  See 'aiTypes.h' for details.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiColor4D
    {
        /** <summary>Red.</summary> */
        public float r;

        /** <summary>Green.</summary> */
        public float g;

        /** <summary>Blue.</summary> */
        public float b;

        /** <summary>Alpha.</summary> */
        public float a;

        /** <summary>Return a string representation of this structure.</summary> */
        public override string ToString()
        {
            return "aiColor4D(RGBA)(" + r + ", " + g + ", " + b + ", " + a + ")";
        }

    }
    #endregion

    #region aiString Structure
    /** <summary>A string structure... See 'aiTypes.h' for details.</summary> */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    [Serializable]
    public struct aiString
    {
        /** <summary>Length of the string excluding the terminal 0.</summary> */
        public uint length;

        /** <summary>aiString buffer.  The size limit is MAXLEN (1024).</summary> */
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXLEN)]
        public string data;

        /** <summary>The maximum length of the data string.</summary> */
        public const int MAXLEN = 1024;
    }
    #endregion

    #region aiMemoryInfo Structure
    /** <summary>A structure that contains memory requirements for a scene.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiMemoryInfo
    {
        /** <summary>Storage allocated for texture data, in bytes.</summary> */
        public uint textures;

        /** <summary>Storage allocated for material data, in bytes.</summary> */
        public uint materials;

        /** <summary>Storage allocated for mesh data, in bytes.</summary> */
        public uint meshes;

        /** <summary>Storage allocated for node data, in bytes.</summary> */
        public uint nodes;

        /** <summary>Storage allocated for animation data, in bytes.</summary> */
        public uint animations;

        /** <summary>Storage allocated for camera data, in bytes.</summary> */
        public uint cameras;

        /** <summary>Storage allocated for light data, in bytes.</summary> */
        public uint lights;

        /** <summary>Storage allocated for the full import, in bytes.</summary> */
        public uint total;

    }
    #endregion

    #region aiVector2D Structure
    /** <summary>A structure that represents a vector in 2 dimensional space.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiVector2D
    {
        /** <summary>The x axis component.</summary> */
        public float x;

        /** <summary>The y axis component.</summary> */
        public float y;

        /** <summary>Return a string representation of this structure.</summary> */
        public override string ToString()
        {
            return "aiVector2D(" + x + ", " + y + ")";
        }
    }
    #endregion

    #region aiVector3D Structure
    /** <summary>A structure that represents a vector in 3 dimensional space.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiVector3D
    {
        /** <summary>The x axis component.</summary> */
        public float x;

        /** <summary>The y axis component.</summary> */
        public float y;

        /** <summary>The z axis component.</summary> */
        public float z;

        /** <summary>Return a string representation of this structure.</summary> */
        public override string ToString()
        {
            return "aiVector3D(" + x + ", " + y + ", " + z + ")";
        }
    }
    #endregion

    #region VectorKey Structure
    /** <summary>This class describes an animation keyframe.  See 'aiAnim.h' for details.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiVector3DKey
    {
        /** <summary>The number of ticks along the timeline this keyframe should be shown.</summary> */
        public double mTime;

        /** <summary>The position or scale value (depending on the context).</summary> */
        public aiVector3D mValue;

    }
    #endregion

    #region QuaternionKey Structure
    /** <summary>A quaternion which represents a rotation.  See 'aiAnim.h' for details.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiQuaternionKey
    {
        /** <summary>The number of ticks along the timeline this keyframe should be shown.</summary> */
        public double mTime;

        /** <summary>The quaternion value.</summary> */
        public aiQuaternion mValue;

    }
    #endregion

    #region VertexWeight Structure
    /** <summary>Defines the extent of influence a bone has over a vertex. See 'aiMesh.h' for details.</summary> */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiVertexWeight
    {
        /** <summary>Index of the vertex which is influenced by the bone.</summary> */
        public uint mVertexId;

        /** <summary>The strength of the influence in the range (0...1).  The influence from all bones at one vertex amounts to 1.</summary> */
        public float mWeight;
    }
    #endregion
}
