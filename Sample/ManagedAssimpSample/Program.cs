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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ManagedAssimpSample
{
    /// <summary>
    /// The class which contains the application entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entrypoint into the application.
        /// </summary>
        /// <param name="args">A list of command line arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            // Ask for a file.
            OpenFileDialog pDialog = new OpenFileDialog();
            DialogResult pResult = pDialog.ShowDialog();
            if (pResult == DialogResult.Cancel)
                return;

            // If we have nothing then exit.
            if (pDialog.FileName.Length == 0)
                return;

            // Say what we are doing.
            Console.WriteLine("Attempting to open: " + pDialog.FileName);

            // Create a new window and open it.
            SampleWindow pSample = new SampleWindow(pDialog.FileName);
            pSample.Run();
        }
    }
}
