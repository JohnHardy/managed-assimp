/*
 * This file is part of The Managed Assimp Wrapper.
 * 
 * The Managed Assimp Wrapper is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * The Managed Assimp Wrapper is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with The Managed Assimp Wrapper.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * If you would like to use The Managed Assimp Wrapper under another license, 
 * contact John Hardy at john at highwire-dtc dot com.
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
