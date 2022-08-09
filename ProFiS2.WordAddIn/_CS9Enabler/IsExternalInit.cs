// This allows to use C# record type in .Net Framework and Standard 2.0
// DO NOT EDIT, DO NOT CHANGE NAMESPACES

using System.ComponentModel;

namespace ClassLibrary
{
    public record Class(string Str)
    {
        internal int Int { get; init; }
    }
}

namespace System.Runtime.CompilerServices
{
    /// <summary>
    ///     Reserved to be used by the compiler for tracking metadata.
    ///     This class should not be used by developers in source code.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit
    {
    }
}