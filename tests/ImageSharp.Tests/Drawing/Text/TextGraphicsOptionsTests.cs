﻿
namespace SixLabors.ImageSharp.Tests.Drawing.Text
{
    using SixLabors.ImageSharp.Drawing;
    using SixLabors.Fonts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Threading.Tasks;
    using Xunit;

    public class TextGraphicsOptionsTests
    {
        [Fact]
        public void ExplicitCastOfGraphicsOptions()
        {
            GraphicsOptions opt = new GraphicsOptions(false)
            {
                AntialiasSubpixelDepth = 99
            };

            TextGraphicsOptions textOptions = opt;

            Assert.False(textOptions.Antialias);
            Assert.Equal(99, textOptions.AntialiasSubpixelDepth);
        }

        [Fact]
        public void ImplicitCastToGraphicsOptions()
        {
            TextGraphicsOptions textOptions = new TextGraphicsOptions(false)
            {
                AntialiasSubpixelDepth = 99
            };

            GraphicsOptions opt = (GraphicsOptions)textOptions;

            Assert.False(opt.Antialias);
            Assert.Equal(99, opt.AntialiasSubpixelDepth);
        }
    }
}