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

/// <summary>
/// This file contains value type structures and enumerations which are common across the managed and unmanaged wrappers.
/// Note that these structures are used by the managed assimp.
/// </summary>
namespace Assimp.ManagedAssimp
{
    #region aiPostProcessingSteps Enumeration
    /// <summary>
    /// See the massive ammount of documentation here: http://assimp.sourceforge.net/lib_html/ai_post_process_8h.html#64795260b95f5a4b3f3dc1be4f52e410
    /// </summary>
    public enum aiPostProcessSteps : uint
    {
        /// <summary>
        /// <hr>Calculates the tangents and bitangents for the imported meshes.
        /// Does nothing if a mesh does not have normals. You might want this post
        /// processing step to be executed if you plan to use tangent space calculations
        /// such as normal mapping  applied to the meshes. There's a config setting,
        /// <tt>#AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE</tt>, which allows you to specify
        /// a maximum smoothing angle for the algorithm. However, usually you'll
        /// want to leave it at the default value. Thanks.
        /// </summary>
        CalcTangentSpace = 0x1,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Identifies and joins identical vertex data sets within all
        ///  imported meshes.
        /// After this step is run each mesh does contain only unique vertices anymore,
        /// so a vertex is possibly used by multiple faces. You usually want
        /// to use this post processing step. If your application deals with
        /// indexed geometry, this step is compulsory or you'll just waste rendering
        /// time. <b>If this flag is not specified</b>, no vertices are referenced by
        /// more than one face and <b>no index buffer is required</b> for rendering.
        /// </summary>
        JoinIdenticalVertices = 0x2,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Converts all the imported data to a left-handed coordinate space.
        /// By default the data is returned in a right-handed coordinate space which
        /// for example OpenGL prefers. In this space, +X points to the right,
        /// +Z points towards the viewer and and +Y points upwards. In the DirectX
        /// coordinate space +X points to the right, +Y points upwards and +Z points
        /// away from the viewer.
        /// You'll probably want to consider this flag if you use Direct3D for
        /// rendering. The #ConvertToLeftHanded flag supersedes this
        /// setting and bundles all conversions typically required for D3D-based
        /// applications.
        /// </summary>
        MakeLeftHanded = 0x4,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Triangulates all faces of all meshes.
        /// By default the imported mesh data might contain faces with more than 3
        /// indices. For rendering you'll usually want all faces to be triangles.
        /// This post processing step splits up all higher faces to triangles.
        /// Line and point primitives are *not* modified!. If you want
        /// 'triangles only' with no other kinds of primitives, try the following
        /// solution:
        /// <ul>
        /// <li>Specify both #Triangulate and #SortByPType </li>
        /// </li>Ignore all point and line meshes when you process assimp's output</li>
        /// </ul>
        /// </summary>
        Triangulate = 0x8,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Removes some parts of the data structure (animations, materials,
        ///  light sources, cameras, textures, vertex components).
        /// The  components to be removed are specified in a separate
        /// configuration option, <tt>#AI_CONFIG_PP_RVC_FLAGS</tt>. This is quite useful
        /// if you don't need all parts of the output structure. Especially vertex
        /// colors are rarely used today ... . Calling this step to remove unrequired
        /// stuff from the pipeline as early as possible results in an increased
        /// performance and a better optimized output data structure.
        /// This step is also useful if you want to force Assimp to recompute
        /// normals or tangents. The corresponding steps don't recompute them if
        /// they're already there (loaded from the source asset). By using this
        /// step you can make sure they are NOT there.
        /// This flag is a poor one, mainly because its purpose is usually
        /// misunderstood. Consider the following case: a 3d model has been exported
        /// from a CAD app, it has per-face vertex colors. Vertex positions can't be
        /// shared, thus the #JoinIdenticalVertices step fails to
        /// optimize the data. Just because these nasty, little vertex colors.
        /// Most apps don't even process them, so it's all for nothing. By using
        /// this step, unneeded components are excluded as early as possible
        /// thus opening more room for internal optimzations.
        /// </summary>
        RemoveComponent = 0x10,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Generates normals for all faces of all meshes.
        /// This is ignored if normals are already there at the time where this flag
        /// is evaluated. Model importers try to load them from the source file, so
        /// they're usually already there. Face normals are shared between all points
        /// of a single face, so a single point can have multiple normals, which in
        /// other words, enforces the library to duplicate vertices in some cases.
        /// #JoinIdenticalVertices is *senseless* then.
        /// This flag may not be specified together with #GenSmoothNormals.
        /// </summary>
        GenNormals = 0x20,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Generates smooth normals for all vertices in the mesh.
        /// This is ignored if normals are already there at the time where this flag
        /// is evaluated. Model importers try to load them from the source file, so
        /// they're usually already there.
        /// This flag may (of course) not be specified together with
        /// #GenNormals. There's a configuration option,
        /// <tt>#AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE</tt> which allows you to specify
        /// an angle maximum for the normal smoothing algorithm. Normals exceeding
        /// this limit are not smoothed, resulting in a a 'hard' seam between two faces.
        /// Using a decent angle here (e.g. 80°) results in very good visual
        /// appearance.
        /// </summary>
        GenSmoothNormals = 0x40,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Splits large meshes into smaller submeshes
        /// This is quite useful for realtime rendering where the number of triangles
        /// which can be maximally processed in a single draw-call is usually limited
        /// by the video driver/hardware. The maximum vertex buffer is usually limited,
        /// too. Both requirements can be met with this step: you may specify both a
        /// triangle and vertex limit for a single mesh.
        /// The split limits can (and should!) be set through the
        /// <tt>#AI_CONFIG_PP_SLM_VERTEX_LIMIT</tt> and <tt>#AI_CONFIG_PP_SLM_TRIANGLE_LIMIT</tt>
        /// settings. The default values are <tt>#AI_SLM_DEFAULT_MAX_VERTICES</tt> and
        /// <tt>#AI_SLM_DEFAULT_MAX_TRIANGLES</tt>.
        /// Note that splitting is generally a time-consuming task, but not if there's
        /// nothing to split. The use of this step is recommended for most users.
        /// </summary>
        SplitLargeMeshes = 0x80,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Removes the node graph and pre-transforms all vertices with
        /// the local transformation matrices of their nodes. The output
        /// scene does still contain nodes, however, there is only a
        /// root node with children, each one referencing only one mesh,
        /// each mesh referencing one material. For rendering, you can
        /// simply render all meshes in order, you don't need to pay
        /// attention to local transformations and the node hierarchy.
        /// Animations are removed during this step.
        /// This step is intended for applications without a scenegraph.
        /// The step CAN cause some problems: if e.g. a mesh of the asset
        /// contains normals and another, using the same material index, does not,
        /// they will be brought together, but the first meshes's part of
        /// the normal list is zeroed. However, these artifacts are rare.
        /// </summary>
        PreTransformVertices = 0x100,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Limits the number of bones simultaneously affecting a single vertex
        ///  to a maximum value.
        /// If any vertex is affected by more than that number of bones, the least
        /// important vertex weights are removed and the remaining vertex weights are
        /// renormalized so that the weights still sum up to 1.
        /// The default bone weight limit is 4 (defined as <tt>#AI_LMW_MAX_WEIGHTS</tt> in
        /// aiConfig.h), but you can use the <tt>#AI_CONFIG_PP_LBW_MAX_WEIGHTS</tt> setting to
        /// supply your own limit to the post processing step.
        /// If you intend to perform the skinning in hardware, this post processing
        /// step might be of interest for you.
        /// </summary>
        LimitBoneWeights = 0x200,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Validates the imported scene data structure
        /// This makes sure that all indices are valid, all animations and
        /// bones are linked correctly, all material references are correct .. etc.
        /// It is recommended to capture Assimp's log output if you use this flag,
        /// so you can easily find ot what's actually wrong if a file fails the
        /// validation. The validator is quite rude and will find *all*
        /// inconsistencies in the data structure ... plugin developers are
        /// recommended to use it to debug their loaders. There are two types of
        /// validation failures:
        /// <ul>
        /// <li>Error: There's something wrong with the imported data. Further
        ///   postprocessing is not possible and the data is not usable at all.
        ///   The import fails. #Importer::GetErrorString() or #aiGetErrorString()
        ///   carry the error message around.</li>
        /// <li>Warning: There are some minor issues (e.g. 1000000 animation
        ///   keyframes with the same time), but further postprocessing and use
        ///   of the data structure is still safe. Warning details are written
        ///   to the log file, <tt>#AI_SCENE_FLAGS_VALIDATION_WARNING</tt> is set
        ///   in #aiScene::mFlags</li>
        /// </ul>
        /// This post-processing step is not time-consuming. It's use is not
        /// compulsory, but recommended.
        /// </summary>
        ValidateDataStructure = 0x400,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Reorders triangles for better vertex cache locality.
        /// The step tries to improve the ACMR (average post-transform vertex cache
        /// miss ratio) for all meshes. The implementation runs in O(n) and is
        /// roughly based on the 'tipsify' algorithm (see <a href="
        /// http://www.cs.princeton.edu/gfx/pubs/Sander_2007_%3ETR/tipsy.pdf">this
        /// paper</a>).
        /// If you intend to render huge models in hardware, this step might
        /// be of interest for you. The <tt>#AI_CONFIG_PP_ICL_PTCACHE_SIZE</tt>config
        /// setting can be used to fine-tune the cache optimization.
        /// </summary>
        ImproveCacheLocality = 0x800,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>Searches for redundant/unreferenced materials and removes them.
        /// This is especially useful in combination with the
        /// #PretransformVertices and #OptimizeMeshes flags.
        /// Both join small meshes with equal characteristics, but they can't do
        /// their work if two meshes have different materials. Because several
        /// material settings are always lost during Assimp's import filters,
        /// (and because many exporters don't check for redundant materials), huge
        /// models often have materials which are are defined several times with
        /// exactly the same settings ..
        /// Several material settings not contributing to the final appearance of
        /// a surface are ignored in all comparisons ... the material name is
        /// one of them. So, if you're passing additional information through the
        /// content pipeline (probably using *magic* material names), don't
        /// specify this flag. Alternatively take a look at the
        /// <tt>#AI_CONFIG_PP_RRM_EXCLUDE_LIST</tt> setting.
        /// </summary>
        RemoveRedundantMaterials = 0x1000,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>This step tries to determine which meshes have normal vectors
        /// that are facing inwards. The algorithm is simple but effective:
        /// the bounding box of all vertices + their normals is compared against
        /// the volume of the bounding box of all vertices without their normals.
        /// This works well for most objects, problems might occur with planar
        /// surfaces. However, the step tries to filter such cases.
        /// The step inverts all in-facing normals. Generally it is recommended
        /// to enable this step, although the result is not always correct.
        /// </summary>
        FixInfacingNormals = 0x2000,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>This step splits meshes with more than one primitive type in
        ///  homogeneous submeshes.
        ///  The step is executed after the triangulation step. After the step
        ///  returns, just one bit is set in aiMesh::mPrimitiveTypes. This is
        ///  especially useful for real-time rendering where point and line
        ///  primitives are often ignored or rendered separately.
        ///  You can use the <tt>#AI_CONFIG_PP_SBP_REMOVE</tt> option to specify which
        ///  primitive types you need. This can be used to easily exclude
        ///  lines and points, which are rarely used, from the import.
        /// </summary>
        SortByPType = 0x8000,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>This step searches all meshes for degenerated primitives and
        ///  converts them to proper lines or points.
        /// A face is 'degenerated' if one or more of its points are identical.
        /// To have the degenerated stuff not only detected and collapsed but
        /// also removed, try one of the following procedures:
        /// <br><b>1.</b> (if you support lines&points for rendering but don't
        ///    want the degenerates)</br>
        /// <ul>
        ///   <li>Specify the #FindDegenerates flag.
        ///   </li>
        ///   <li>Set the <tt>AI_CONFIG_PP_FD_REMOVE</tt> option to 1. This will
        ///       cause the step to remove degenerated triangles from the import
        ///       as soon as they're detected. They won't pass any further
        ///       pipeline steps.
        ///   </li>
        /// </ul>
        /// <br><b>2.</b>(if you don't support lines&points at all ...)</br>
        /// <ul>
        ///   <li>Specify the #FindDegenerates flag.
        ///   </li>
        ///   <li>Specify the #SortByPType flag. This moves line and
        ///     point primitives to separate meshes.
        ///   </li>
        ///   <li>Set the <tt>AI_CONFIG_PP_SBP_REMOVE</tt> option to
        /// </summary>
        FindDegenerates = 0x10000,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>This step searches all meshes for invalid data, such as zeroed
        ///  normal vectors or invalid UV coords and removes/fixes them. This is
        ///  intended to get rid of some common exporter errors.
        /// This is especially useful for normals. If they are invalid, and
        /// the step recognizes this, they will be removed and can later
        /// be recomputed, i.e. by the #GenSmoothNormals flag.<br>
        /// The step will also remove meshes that are infinitely small and reduce
        /// animation tracks consisting of hundreds if redundant keys to a single
        /// key. The <tt>AI_CONFIG_PP_FID_ANIM_ACCURACY</tt> config property decides
        /// the accuracy of the check for duplicate animation tracks.
        /// </summary>
        FindInvalidData = 0x20000,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>This step converts non-UV mappings (such as spherical or
        ///  cylindrical mapping) to proper texture coordinate channels.
        /// Most applications will support UV mapping only, so you will
        /// probably want to specify this step in every case. Note tha Assimp is not
        /// always able to match the original mapping implementation of the
        /// 3d app which produced a model perfectly. It's always better to let the
        /// father app compute the UV channels, at least 3ds max, maja, blender,
        /// lightwave, modo, ... are able to achieve this.
        /// </summary>
        GenUVCoords = 0x40000,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>This step applies per-texture UV transformations and bakes
        ///  them to stand-alone vtexture coordinate channelss.
        /// UV transformations are specified per-texture - see the
        /// <tt>#AI_MATKEY_UVTRANSFORM</tt> material key for more information.
        /// This step processes all textures with
        /// transformed input UV coordinates and generates new (pretransformed) UV channel
        /// which replace the old channel. Most applications won't support UV
        /// transformations, so you will probably want to specify this step.
        /// </summary>
        TransformUVCoords = 0x80000,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>This step searches for duplicate meshes and replaces duplicates
        ///  with references to the first mesh.
        ///  This step takes a while, don't use it if you have no time.
        ///  Its main purpose is to workaround the limitation that many export
        ///  file formats don't support instanced meshes, so exporters need to
        ///  duplicate meshes. This step removes the duplicates again. Please
        ///  note that Assimp does currently not support per-node material
        ///  assignment to meshes, which means that identical meshes with
        ///  differnent materials are currently *not* joined, although this is
        ///  planned for future versions.
        /// </summary>
        FindInstances = 0x100000,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>A postprocessing step to reduce the number of meshes.
        ///  In fact, it will reduce the number of drawcalls.
        ///  This is a very effective optimization and is recommended to be used
        ///  together with #OptimizeGraph, if possible. The flag is fully
        ///  compatible with both #SplitLargeMeshes and #SortByPType.
        /// </summary>
        OptimizeMeshes = 0x200000,


        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>A postprocessing step to optimize the scene hierarchy.
        ///  Nodes with no animations, bones, lights or cameras assigned are
        ///  collapsed and joined.
        ///  Node names can be lost during this step. If you use special 'tag nodes'
        ///  to pass additional information through your content pipeline, use the
        ///  <tt>#AI_CONFIG_PP_OG_EXCLUDE_LIST</tt> setting to specify a list of node
        ///  names you want to be kept. Nodes matching one of the names in this list won't
        ///  be touched or modified.
        ///  Use this flag with caution. Most simple files will be collapsed to a
        ///  single node, complex hierarchies are usually completely lost. That's not
        ///  the right choice for editor environments, but probably a very effective
        ///  optimization if you just want to get the model data, convert it to your
        ///  own format and render it as fast as possible.
        ///  This flag is designed to be used with #OptimizeMeshes for best
        ///  results.
        /// </summary>
        OptimizeGraph = 0x400000,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>This step flips all UV coordinates along the y-axis and adjusts
        /// material settings and bitangents accordingly.
        /// <br><b>Output UV coordinate system:</b>
        /// </summary>
        FlipUVs = 0x800000,

        // -------------------------------------------------------------------------
        /// <summary>
        /// <hr>This step adjusts the output face winding order to be cw.
        /// The default face winding order is counter clockwise.
        /// <br><b>Output face order:</b>
        /// </summary>
        FlipWindingOrder = 0x1000000
    }
    #endregion

    #region aiMatrix3x3 Structure
    /// <summary>
    /// A 3x3 Matrix.  These are in row-major (opengl, not directx) format.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiMatrix3x3
    {
        /// <summary>
        /// 
        /// </summary>
        public float a1;

        /// <summary>
        /// 
        /// </summary>
        public float a2;

        /// <summary>
        /// 
        /// </summary>
        public float a3;

        /// <summary>
        /// 
        /// </summary>
        public float b1;

        /// <summary>
        /// 
        /// </summary>
        public float b2;

        /// <summary>
        /// 
        /// </summary>
        public float b3;

        /// <summary>
        /// 
        /// </summary>
        public float c1;

        /// <summary>
        /// 
        /// </summary>
        public float c2;

        /// <summary>
        /// 
        /// </summary>
        public float c3;
    }
    #endregion

    #region aiMatrix4x4 Structure
    /// <summary>
    /// A 4x4 Matrix.  These are in row-major (opengl, not directx) format.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiMatrix4x4
    {
        /// <summary>
        /// 
        /// </summary>
        public float a1;

        /// <summary>
        /// 
        /// </summary>
        public float a2;

        /// <summary>
        /// 
        /// </summary>
        public float a3;

        /// <summary>
        /// 
        /// </summary>
        public float a4;

        /// <summary>
        /// 
        /// </summary>
        public float b1;

        /// <summary>
        /// 
        /// </summary>
        public float b2;

        /// <summary>
        /// 
        /// </summary>
        public float b3;

        /// <summary>
        /// 
        /// </summary>
        public float b4;

        /// <summary>
        /// 
        /// </summary>
        public float c1;

        /// <summary>
        /// 
        /// </summary>
        public float c2;

        /// <summary>
        /// 
        /// </summary>
        public float c3;

        /// <summary>
        /// 
        /// </summary>
        public float c4;

        /// <summary>
        /// 
        /// </summary>
        public float d1;

        /// <summary>
        /// 
        /// </summary>
        public float d2;

        /// <summary>
        /// 
        /// </summary>
        public float d3;

        /// <summary>
        /// 
        /// </summary>
        public float d4;
    }
    #endregion

    #region aiQuaternion Structure
    /// <summary>
    /// A quaternion as defined by assimp. See 'aiQuaternion.h' for details.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiQuaternion
    {
        /// <summary>
        /// W component of the quaternion.
        /// </summary>
        public float w;

        /// <summary>
        /// X component of the quaternion.
        /// </summary>
        public float x;

        /// <summary>
        /// Y component of the quaternion.
        /// </summary>
        public float y;

        /// <summary>
        /// Z component of the quaternion.
        /// </summary>
        public float z;
    }
    #endregion

    #region aiTexel Structure
    /// <summary>
    ///  aiTexel, or texture element (also texture pixel) is the fundamental uint of texture space , used in computer graphics.  See 'aiTexture.h' for details.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiTexel
    {
        /// <summary>
        /// Blue.
        /// </summary>
        public byte b;

        /// <summary>
        /// Green.
        /// </summary>
        public byte g;

        /// <summary>
        /// Red.
        /// </summary>
        public byte r;

        /// <summary>
        /// Alpha.
        /// </summary>
        public byte a;

    }
    #endregion

    #region aiPlane Structure
    /// <summary>
    /// A flat surface extending infinitely in all directions.  See 'aiTypes.h' for details.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiPlane
    {
        /// <summary>
        /// From the plane equation.
        /// </summary>
        public float a;

        /// <summary>
        /// From the plane equation.
        /// </summary>
        public float b;

        /// <summary>
        /// From the plane equation.
        /// </summary>
        public float c;

        /// <summary>
        /// From the plane equation.
        /// </summary>
        public float d;

    }
    #endregion

    #region aiColor3D Structure
    /// <summary>
    /// A colour with only RGB components.  See 'aiTypes.h' for details.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiColor3D
    {
        /// <summary>
        /// Red.
        /// </summary>
        public float r;

        /// <summary>
        /// Green.
        /// </summary>
        public float g;

        /// <summary>
        /// Blue.
        /// </summary>
        public float b;

        /// <summary>
        /// Return a string representation of this structure.
        /// </summary>
        public override string ToString()
        {
            return "aiColor3D(RGB)(" + r + ", " + g + ", " + b + ")";
        }
    }
    #endregion

    #region aiColor4D Structure
    /// <summary>
    /// A colour with RGBA components.  See 'aiTypes.h' for details.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiColor4D
    {
        /// <summary>
        /// Red.
        /// </summary>
        public float r;

        /// <summary>
        /// Green.
        /// </summary>
        public float g;

        /// <summary>
        /// Blue.
        /// </summary>
        public float b;

        /// <summary>
        /// Alpha.
        /// </summary>
        public float a;

        /// <summary>
        /// Return a string representation of this structure.
        /// </summary>
        public override string ToString()
        {
            return "aiColor4D(RGBA)(" + r + ", " + g + ", " + b + ", " + a + ")";
        }

    }
    #endregion

    #region aiString Structure
    /// <summary>
    /// A string structure... See 'aiTypes.h' for details.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    [Serializable]
    public struct aiString
    {
        /// <summary>
        /// Length of the string excluding the terminal 0.
        /// </summary>
        public uint length;

        /// <summary>
        /// aiString buffer.  The size limit is MAXLEN (1024).
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXLEN)]
        public string data;

        /// <summary>
        /// The maximum length of the data string.
        /// </summary>
        public const int MAXLEN = 1024;
    }
    #endregion

    #region aiMemoryInfo Structure
    /// <summary>
    /// A structure that contains memory requirements for a scene.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiMemoryInfo
    {
        /// <summary>
        /// Storage allocated for texture data, in bytes.
        /// </summary>
        public uint textures;

        /// <summary>
        /// Storage allocated for material data, in bytes.
        /// </summary>
        public uint materials;

        /// <summary>
        /// Storage allocated for mesh data, in bytes.
        /// </summary>
        public uint meshes;

        /// <summary>
        /// Storage allocated for node data, in bytes.
        /// </summary>
        public uint nodes;

        /// <summary>
        /// Storage allocated for animation data, in bytes.
        /// </summary>
        public uint animations;

        /// <summary>
        /// Storage allocated for camera data, in bytes.
        /// </summary>
        public uint cameras;

        /// <summary>
        /// Storage allocated for light data, in bytes.
        /// </summary>
        public uint lights;

        /// <summary>
        /// Storage allocated for the full import, in bytes.
        /// </summary>
        public uint total;

    }
    #endregion

    #region aiVector2D Structure
    /// <summary>
    /// A structure that represents a vector in 2 dimensional space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiVector2D
    {
        /// <summary>
        /// The x axis component.
        /// </summary>
        public float x;

        /// <summary>
        /// The y axis component.
        /// </summary>
        public float y;

        /// <summary>
        /// Return a string representation of this structure.
        /// </summary>
        public override string ToString()
        {
            return "aiVector2D(" + x + ", " + y + ")";
        }
    }
    #endregion

    #region aiVector3D Structure
    /// <summary>
    /// A structure that represents a vector in 3 dimensional space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiVector3D
    {
        /// <summary>
        /// The x axis component.
        /// </summary>
        public float x;

        /// <summary>
        /// The y axis component.
        /// </summary>
        public float y;

        /// <summary>
        /// The z axis component.
        /// </summary>
        public float z;

        /// <summary>
        /// Return a string representation of this structure.
        /// </summary>
        public override string ToString()
        {
            return "aiVector3D(" + x + ", " + y + ", " + z + ")";
        }
    }
    #endregion

    #region VectorKey Structure
    /// <summary>
    /// This class describes an animation keyframe.  See 'aiAnim.h' for details.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiVector3DKey
    {
        /// <summary>
        /// The number of ticks along the timeline this keyframe should be shown.
        /// </summary>
        public double mTime;

        /// <summary>
        /// The position or scale value (depending on the context).
        /// </summary>
        public aiVector3D mValue;

    }
    #endregion

    #region QuaternionKey Structure
    /// <summary>
    /// A quaternion which represents a rotation.  See 'aiAnim.h' for details.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiQuaternionKey
    {
        /// <summary>
        /// The number of ticks along the timeline this keyframe should be shown.
        /// </summary>
        public double mTime;

        /// <summary>
        /// The quaternion value.
        /// </summary>
        public aiQuaternion mValue;

    }
    #endregion

    #region VertexWeight Structure
    /// <summary>
    /// Defines the extent of influence a bone has over a vertex. See 'aiMesh.h' for details.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct aiVertexWeight
    {
        /// <summary>
        /// Index of the vertex which is influenced by the bone.
        /// </summary>
        public uint mVertexId;

        /// <summary>
        /// The strength of the influence in the range (0...1).  The influence from all bones at one vertex amounts to 1.
        /// </summary>
        public float mWeight;
    }
    #endregion
}
