﻿// <copyright file="BinaryThreshold.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp
{
    using System;
    using Processing.Processors;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.Primitives;

    /// <summary>
    /// Extension methods for the <see cref="Image{TPixel}"/> type.
    /// </summary>
    public static partial class ImageExtensions
    {
        /// <summary>
        /// Applies binarization to the image splitting the pixels at the given threshold.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="threshold">The threshold to apply binarization of the image. Must be between 0 and 1.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> BinaryThreshold<TPixel>(this Image<TPixel> source, float threshold)
            where TPixel : struct, IPixel<TPixel>
        {
            return BinaryThreshold(source, threshold, source.Bounds);
        }

        /// <summary>
        /// Applies binarization to the image splitting the pixels at the given threshold.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="threshold">The threshold to apply binarization of the image. Must be between 0 and 1.</param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> structure that specifies the portion of the image object to alter.
        /// </param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> BinaryThreshold<TPixel>(this Image<TPixel> source, float threshold, Rectangle rectangle)
            where TPixel : struct, IPixel<TPixel>
        {
            source.ApplyProcessor(new BinaryThresholdProcessor<TPixel>(threshold), rectangle);
            return source;
        }
    }
}
