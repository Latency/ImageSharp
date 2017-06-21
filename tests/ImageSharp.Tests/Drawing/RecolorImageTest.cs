﻿// <copyright file="BlendTest.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp.Tests
{
    using SixLabors.ImageSharp.Drawing.Brushes;
    using System.IO;
    using System.Linq;

    using SixLabors.ImageSharp.PixelFormats;

    using Xunit;
    using SixLabors.Primitives;

    public class RecolorImageTest : FileTestBase
    {
        [Fact]
        public void ImageShouldRecolorYellowToHotPink()
        {
            string path = this.CreateOutputDirectory("Drawing", "RecolorImage");

            RecolorBrush<Rgba32> brush = new RecolorBrush<Rgba32>(Rgba32.Yellow, Rgba32.HotPink, 0.2f);

            foreach (TestFile file in Files)
            {
                using (Image<Rgba32> image = file.CreateImage())
                {
                    using (FileStream output = File.OpenWrite($"{path}/{file.FileName}"))
                    {
                        image.Fill(brush)
                            .Save(output);
                    }
                }
            }
        }

        [Fact]
        public void ImageShouldRecolorYellowToHotPinkInARectangle()
        {
            string path = this.CreateOutputDirectory("Drawing", "RecolorImage");

            RecolorBrush<Rgba32> brush = new RecolorBrush<Rgba32>(Rgba32.Yellow, Rgba32.HotPink, 0.2f);

            foreach (TestFile file in Files)
            {
                using (Image<Rgba32> image = file.CreateImage())
                {
                    using (FileStream output = File.OpenWrite($"{path}/Shaped_{file.FileName}"))
                    {
                        int imageHeight = image.Height;
                        image.Fill(brush, new Rectangle(0, imageHeight/2 - imageHeight/4, image.Width, imageHeight/2))
                            .Save(output);
                    }
                }
            }
        }
    }
}