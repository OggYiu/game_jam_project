#define DEBUG

using System;
using System.Diagnostics;

public class DebugUtils
{
    [Conditional("DEBUG")]
    static public void Assert(bool condition, string message) {
        if (!condition) throw new Exception(message);
    }
}