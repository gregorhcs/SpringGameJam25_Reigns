using System.Runtime.CompilerServices;
using GameJam;

[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_EDITOR)]
[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_TESTS_EDITMODE)]
[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_TESTS_PLAYMODE)]

namespace GameJam {
    static class AssemblyInfo {
        public const string NAMESPACE_RUNTIME = "GameJam";
        public const string NAMESPACE_EDITOR = "GameJam.Editor";
        public const string NAMESPACE_TESTS_EDITMODE = "GameJam.Tests.EditMode";
        public const string NAMESPACE_TESTS_PLAYMODE = "GameJam.Tests.PlayMode";
    }
}