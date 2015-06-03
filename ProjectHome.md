# Overview #
The Managed Assimp Wrapper lets you use the [Open Asset Importer (Assimp)](http://assimp.sourceforge.net) from with C# and other .NET based languages.

Unlike the supported wrapper which uses [SWIG](http://www.swig.org), this wrapper uses [P/Invoke](http://en.wikipedia.org/wiki/Platform_Invocation_Services).  It works by copying the unmanaged memory into managed memory which can be controlled by .NET.

It is compatible with VS2010 and should backport to previous versions without much trouble.

## Support ##
I will do my best to help with problems encountered using this library.  If you want a more reliably supported project, please use the [Assimp](http://assimp.sourceforge.net) project directly.

## Sample Project ##
Here is an image of the sample application provided:

![http://i54.tinypic.com/2h73tw7.png](http://i54.tinypic.com/2h73tw7.png)

## Video Usage ##
Here is a video of some models loaded using the wrapper:


<a href='http://www.youtube.com/watch?feature=player_embedded&v=gLVZcVw5LlU' target='_blank'><img src='http://img.youtube.com/vi/gLVZcVw5LlU/0.jpg' width='425' height=344 /></a>

Thanks to Mark Ward for his measure cube model (included).

## FAQ ##
**Q:** I get a PInvokeStackImbalance when I run it with the debugger attached.

**A:** This is normal and can be disabled in Debug > Exceptions > Managed Debugging Assistants > Un-check PInvokeStackImbalance.  The [PInvokeStackImbalance](http://msdn.microsoft.com/en-us/library/0htdy0k3.aspx) MDA is activated when the CLR detects that the stack depth after a P/Invoke call does not match the expected stack depth.