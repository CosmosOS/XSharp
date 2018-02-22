using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

// Walkthrough: Creating a Language Service (MPF)
//   http://msdn.microsoft.com/en-us/library/bb165744
// Language Service Features (MPF)
//   http://msdn.microsoft.com/en-us/library/bb166215
// Syntax Colorizing
//   http://msdn.microsoft.com/en-us/library/bb165041
// Managed Babel
//   http://msdn.microsoft.com/en-us/library/bb165037.aspx

namespace XSharp.VS
{
    [Guid(LanguageServiceGuid)]
    internal sealed class XSharpLanguageService : LanguageService
    {
        public const string LanguageServiceGuid = "3fb852ed-3562-3da4-98dc-55759744328c";

        private Lazy<LanguagePreferences> _preferences;

        public XSharpLanguageService()
        {
            _preferences = new Lazy<LanguagePreferences>(
                () =>
                {
                    var preferences = new LanguagePreferences(Site, typeof(XSharpLanguageService).GUID, Name);
                    preferences.Init();

                    return preferences;
                });
        }

        public override string Name => "X#";
        public override string GetFormatFilterList() => "X# files (*.xs)\n*.xs\n";
        public override LanguagePreferences GetLanguagePreferences() => _preferences.Value;
        public override IScanner GetScanner(IVsTextLines buffer) => new Scanner(buffer);
        public override AuthoringScope ParseSource(ParseRequest req) => new Parser();
    }
}
