//
// OSC Jack - OSC Input Plugin for Unity
//
// Copyright (C) 2015 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;

namespace OscJack
{
    // OSC master directory class
    // Provides the interface for the OSC master directory.
    public static class OscMaster
    {
        // UDP listening port number list
        static int[] listenPortList = { 9000 };

        // Determines whether any data has arrived to a given address.
        public static bool HasData(string address)
        {
            return _directory.HasData(address);
        }

        // Returns a data set which was sent to a given address.
        public static Object[] GetData(string address)
        {
            return _directory.GetData(address);
        }

		public static void Remove(string address)
		{
			_directory.Remove(address);
		}

		// Returns a reference to the master directory instance.
        public static OscDirectory MasterDirectory {
            get { return _directory; }
        }

        static OscDirectory _directory = new OscDirectory(listenPortList);
    }
}
