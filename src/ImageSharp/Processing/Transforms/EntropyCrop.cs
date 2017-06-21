// <copyright file="EntropyCrop.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp
{
    using System;
    using Processing.Processors;
    using SixLabors.ImageSharp.PixelFormats;

    /// <summary>
    /// Extension methods for the <see cref="Image{TPixel}"/> type.
    /// </summary>
    public static partial class ImageExtensions
    {
        /// <summary>
        /// Crops an image to the area of greatest entropy.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image to crop.</param>
        /// <param name="threshold">The threshold for entropic density.</param>
        /// <returns>The <see cref="Image{TPixel}"/></returns>
        public static Image<TPixel> EntropyCrop<TPixel>(this Image<TPixel> source, float threshold = .5f)
            where TPixel : struct, IPixel<TPixel>
        {
            EntropyCropProcessor<TPixel> processor = new EntropyCropProcessor<TPixel>(threshold);

            source.ApplyProcessor(processor, source.Bounds);
            return source;
        }
    }
}