#Managed-Assimp 

#This project is no longer supported.  Please use the newer and better: https://code.google.com/p/assimp-net/

I have kept a reference here for posterity, plus it was referenced by a few older [ST Excalibur](stexcalibur.com) builds.

#Overview
The Managed Assimp Wrapper lets you use the [http://assimp.sourceforge.net Open Asset Importer (Assimp)] from with C# and other .NET based languages.

Unlike the supported wrapper which uses [http://www.swig.org SWIG], this wrapper uses [http://en.wikipedia.org/wiki/Platform_Invocation_Services P/Invoke].  It works by copying the unmanaged memory into managed memory which can be controlled by .NET.

It is compatible with VS2010 and should backport to previous versions without much trouble.

#Sample Project
Here is an image of the sample application provided:

![Picture](http://i54.tinypic.com/2h73tw7.png)

#Video Usage
Here is a video of some models loaded using the wrapper:


<a href="http://www.youtube.com/watch?feature=player_embedded&v=gLVZcVw5LlU
" target="_blank"><img src="http://img.youtube.com/vi/gLVZcVw5LlU/0.jpg" 
alt="Watch a preview" width="640" height="480" border="10" /></a>



Thanks to Mark Ward for his measure cube model (included).

== FAQ ==
*Q:* I get a PInvokeStackImbalance when I run it with the debugger attached.

*A:* This is normal and can be disabled in Debug > Exceptions > Managed Debugging Assistants > Un-check PInvokeStackImbalance.  The [http://msdn.microsoft.com/en-us/library/0htdy0k3.aspx PInvokeStackImbalance] MDA is activated when the CLR detects that the stack depth after a P/Invoke call does not match the expected stack depth.

