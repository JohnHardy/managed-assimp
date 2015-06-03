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
    /// A node in the imported scene hierarchy.  Each node has name, a single parent node (except for the root node), a transformation relative to its parent and possibly several child nodes.  Simple file formats don't support hierarchical structures, for these formats the imported scene does consist of only a single root node with no childs.  Multiple meshes can be assigned to a single node.
    /// </summary>
    /// <author>John Hardy - hardyj2  at the domain of  unix.lancs.ac.uk</author>
    /// <date>20 July 2009</date>
    [Serializable]
    public class Node
    {
        #region Properties
        /// <summary>
        /// The name of the node.  The name may be empty but all nodes which need to be accessed afterwards by bones and animations are usually named.  Nodes can have the same name, but nodes which are referenced by bones/animasions HAVE to be unique.
        /// </summary>
        private String sName        = null;

        /// <summary>
        /// The transformation relative to the node's parent.
        /// </summary>
        private aiMatrix4x4 mTransformation;

        /// <summary>
        /// Parent node. "Node* mParent" NULL if this node is the root node.
        /// </summary>
        private Node mParent        = null;

        /// <summary>
        /// The child nodes of this node. "Node** mChildren" NULL if mNumChildren is 0.  Double Pointer...
        /// </summary>
        private Node[] tChildren    = null;

        /// <summary>
        /// The meshes of this node. Each entry is an index into the mesh.
        /// </summary>
        private uint[] tMeshes      = null;
        #endregion

        /// <summary>
        /// Constructor which builds a Node object from a pointer to an aiNode structure.  As a user, you should not call this by hand.
        /// <remarks>This will copy the data inside the structure into managed memory.  It *DOES NOT* free the unmanaged memory.</remarks>
        /// </summary>
        /// <param name="aiNode*">A pointer to the face structure in the low level unmanaged wrapper.</param>
        unsafe internal Node(IntPtr p_aiNode, Node pParent)
        {
            // Unmarshal our structure into (what will be for us, some temporary) managed memory.  This is naturally, automatically released when we exit the function.
            UnmanagedAssimp.aiNode tNode = (UnmanagedAssimp.aiNode)Marshal.PtrToStructure(p_aiNode, typeof(UnmanagedAssimp.aiNode));

            // Copy over the nice, simple value types into managed memory.  Value types copy the entire structure rather than just the pointer to it.
            this.mTransformation = tNode.mTransformation;
            
            // Copy the properties over into our class as marshalled managed memory.
            // Note that with strings, prepending an empty string "" forces a copy.
            this.sName      = "" + tNode.mName.data;

            // Check the parent is not null.
            //this.mParent = null;
            //if (tNode.mParent != IntPtr.Zero)
            //    this.mParent = new Node(tNode.mParent);
            
            // To link the heirarchy we use a .NET reference to the parent.  This assumes we create in hierarchy order.
            this.mParent = pParent;

            // If we have children.
            if (tNode.mNumChildren > 0)
            {
                // Copy over the array of node animations into managed memory belonging to this class instance.
                int iIntPtrSize = sizeof(IntPtr);
                tChildren = new Node[tNode.mNumChildren];
                IntPtr pChildPtr = tNode.mChildren;
                for (int iElement = 0; iElement < tNode.mNumChildren; ++iElement)
                {
                    // Copy the element at this index and then increment the channel pointer (which should be sizeof(IntPtr) away.
                    tChildren[iElement] = new Node(Marshal.ReadIntPtr(pChildPtr), this);
                    pChildPtr = new IntPtr(pChildPtr.ToInt64() + iIntPtrSize);
                }
            }

            // Marshal the array of unsigned integers for each mesh.
            tMeshes = UnmanagedAssimp.MarshalUintArray(tNode.mMeshes, tNode.mNumMeshes);
        }

        /// <summary>
        /// Return a string representation fo this node.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString()
        {
            return "<node name='"+this.sName+"' numchildren='"+((this.tChildren == null) ? 0 : this.tChildren.Length)+"' parent='"+((this.mParent == null) ? "null" : this.mParent.getName())+ "' />";
        }

        /// <summary>
        /// Constructs a new node and initializes it.
        /// </summary>
        /// <param name="parentNode">Parent node or null for root nodes</param>
	    public Node(Node parentNode) {

		    this.mParent = parentNode;
	    }

        /// <summary>
        /// Returns the number of meshes of this node.
        /// </summary>
        /// <returns>Number of meshes</returns>
	    public int getNumMeshes()
        {
		    return tMeshes == null ? 0 : tMeshes.Length;
	    }

        /// <summary>
        /// Get a list of all meshes of this node
        /// </summary>
        /// <returns>Array containing indices into the Scene's mesh list. If there are no meshes, the array is <code>null</code></returns>
	    public uint[] getMeshes()
        {
		    return tMeshes;
	    }

        /// <summary>
        /// Get the local transformation matrix of the node in row-major order:
        /// <code>
        /// a1 a2 a3 a4 (the translational part of the matrix is stored
        /// b1 b2 b3 b4  in (a4|b4|c4))
        /// c1 c2 c3 c4
        /// d1 d2 d3 d4
        /// </code>
        /// </summary>
        /// <returns>Row-major transformation matrix</returns>
	    public aiMatrix4x4 getTransformRowMajor()
        {
		    return mTransformation;
	    }

        /// <summary>
        /// Get the local transformation matrix of the node in column-major order:
        /// <code>
        /// a1 b1 c1 d1 (the translational part of the matrix is stored
        /// a2 b2 c2 d2  in (a4|b4|c4))
        /// a3 b3 c3 d3
        /// a4 b4 c4 d4
        /// </code>
        /// </summary>
        /// <returns>Column-major transformation matrix</returns>
	    public aiMatrix4x4 getTransformColumnMajor()
        {
            throw new NotImplementedException("Perhaps implementing this would be a good idea..  Sorry.");
		    //Matrix4x4 m = new Matrix4x4(nodeTransform);
		    //return m.transpose();
	    }

        /// <summary>
        /// Get the name of the node. The name might be empty (length of zero) but
        /// all nodes which need to be accessed afterwards by bones or anims are
        /// usually named.
        /// </summary>
        /// <returns>The name of this node.</returns>
	    public String getName()
        {
		    return sName;
	    }

        /// <summary>
        /// Get the list of all child nodes of *this* node.
        /// </summary>
        /// <returns>List of children. May be empty.</returns>
	    public Node[] getChildren()
        {
		    return tChildren;
	    }

        /// <summary>
        /// Get the number of child nodes of *this* node.
        /// </summary>
        /// <returns>The number of child nodes of *this* node. May be 0.</returns>
	    public int getNumChildren()
        {
		    return tChildren == null ? 0 : tChildren.Length;
	    }

        /// <summary>
        /// Get the parent node of the node
        /// </summary>
        /// <returns>Parent node</returns>
	    public Node getParent()
        {
		    return mParent;
	    }

        /// <summary>
        /// Searches this node and recursively all sub nodes for a node with a specific name.
        /// </summary>
        /// <param name="_name">Name of the node to search for</param>
        /// <returns>Either a reference to the node or <code>null</code> if no node with this name was found.</returns>
	    public Node findNode(String sName)
        {
            // Are we the node you're looking for?
		    if (this.sName == sName)
			    return this;

            // Nope - lets ask the kids.
            if (tChildren == null)
                return null;

            foreach (Node oNode in tChildren)
            {
                if (sName == oNode.getName())
                    return oNode;
            }

            // Nope sorry.  Return null.
		    return null;
	    }

        /// <summary>
        /// Print all nodes to the console recursively. This is a debugging utility.
        /// This will print the nodes in the format:
        /// </summary>
        /// <param name="sPrefix">The prefix to attach for this generation.</param>
        /// Parent
        /// <code>
        public void printNodes(String sPrefix)
        {
            // Print our name.
            Console.WriteLine(sPrefix + getName());

            // Print all the meshes we have and their index.
            if (getNumMeshes() != 0)
            {
                foreach (uint iMesh in tMeshes)
                    Console.WriteLine(sPrefix + "|Mesh @ " + iMesh);
            }

            // Print out all the children recursively.
            if (getNumChildren() != 0)
            {
                foreach (Node oChild in tChildren)
                    oChild.printNodes(sPrefix + "|-- ");
            }
	    }
    }
}
