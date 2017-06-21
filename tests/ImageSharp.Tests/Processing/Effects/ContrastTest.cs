﻿// <copyright file="ContrastTest.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp.Tests.Processing.Effects
{
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.Primitives;
    using Xunit;

    public class ContrastTest : FileTestBase
    {
        public static readonly TheoryData<int> ContrastValues
        = new TheoryData<int>
        {
            50,
           -50
        };

        [Theory]
        [WithFileCollection(nameof(DefaultFiles), nameof(ContrastValues), DefaultPixelType)]
        public void ImageShouldApplyContrastFilter<TPixel>(TestImageProvider<TPixel> provider, int value)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                image.Contrast(value)
                    .DebugSave(provider, value, Extensions.Bmp);
            }
        }

        [Theory]
        [WithFileCollection(nameof(DefaultFiles), nameof(ContrastValues), DefaultPixelType)]
        public void ImageShouldApplyContrastFilterInBox<TPixel>(TestImageProvider<TPixel> provider, int value)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> source = provider.GetImage())
            using (var image = new Image<TPixel>(source))
            {
                var bounds = new Rectangle(10, 10, image.Width / 2, image.Height / 2);

                image.Contrast(value, bounds)
                    .DebugSave(provider, value, Extensions.Bmp);

                ImageComparer.EnsureProcessorChangesAreConstrained(source, image, bounds);
            }
        }
    }
}