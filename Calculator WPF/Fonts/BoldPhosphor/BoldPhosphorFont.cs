using FontAwesome.Sharp;
using System;
using System.Reflection;
using System.Windows.Media;
using WpfFont = System.Windows.Media.FontFamily;

namespace Calculator_WPF.Fonts.BoldPhosphor
{
    internal static class BoldPhosphorFont
    {
        public const string FontName = "Phosphor Bold";
        public static readonly Lazy<WpfFont> Wpf = new(LoadWpfFont);

        private static readonly Assembly FontAssembly = typeof(BoldPhosphorFont).Assembly;
        private static WpfFont LoadWpfFont()
        {
            return FontAssembly.LoadFont("fonts", FontName);
        }
    }

    public class IconImage : IconImageBase<BoldPhosphorIcons>
    {
        protected override ImageSource ImageSourceFor(BoldPhosphorIcons icon)
        {
            var size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
            return BoldPhosphorFont.Wpf.Value.ToImageSource(icon, Foreground, size);
        }
    }

    public class IconBlock : IconBlockBase<BoldPhosphorIcons>
    {
        public IconBlock() : base(BoldPhosphorFont.Wpf.Value)
        {
        }
    }

    public class Icon : IconBase<IconBlock, BoldPhosphorIcons>
    {
        public Icon(BoldPhosphorIcons icon) : base(icon)
        {
        }
    }
    
}
