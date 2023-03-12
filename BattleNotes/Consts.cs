using System;
using System.IO;
using System.Numerics;

namespace BattleNotes
{
    public static class Consts
    {
        public static readonly string startupFontLocation = (String.Format(".{0}data{0}fonts{0}JetBrainsMono-Medium.ttf", Path.DirectorySeparatorChar));
        public static readonly string fontAwesomeLocation = (String.Format(".{0}data{0}fonts{0}misc{0}fontawesome-webfont.ttf", Path.DirectorySeparatorChar));

        public static readonly string themeLocation = (String.Format(".{0}data{0}themes", Path.DirectorySeparatorChar));
        public static readonly string fontsLocation = (String.Format(".{0}data{0}fonts", Path.DirectorySeparatorChar));
        public const int defaultFontSize = 20;
        
    } 
    
}
