OSC Jack
========

OSC Jack is a class library for handling OSC over UDP input in Unity.

Setting up
----------

1. Import [the unitypackage file][unitypackage].
2. Set [the listen port list][portlist].
3. There is no step three.

[unitypackage]: https://github.com/keijiro/OscJack/raw/master/OscJack.unitypackage
[portlist]: https://github.com/keijiro/OscJack/blob/master/Assets/OscJack/OscMaster.cs#L32

API Reference
-------------

#### static bool OscMaster.HasData (string address)

Determines whether any data has arrived to a given address.

#### static Object[] OscMaster.GetData (string address)

Returns a data set which was sent to a given address.

OSC Monitor Window
------------------

OSC Jack provides the OSC Monitor window, which shows the address-value
pair list of arrived OSC messages. To open the OSC Monitor window, select
Window menu -> OSC Jack.

License
-------

Copyright (C) 2013-2015 Keijiro Takahashi

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
