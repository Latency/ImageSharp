﻿// <copyright file="IColorMatrixFilter.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp.Processing.Processors
{
    using System;
    using System.Numerics;

    using SixLabors.ImageSharp.PixelFormats;

    /// <summary>
    /// Encapsulates properties and methods for creating processors that utilize a matrix to
    /// alter the image pixels.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal interface IColorMatrixFilter<TPixel> : IImageProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// Gets the <see cref="Matrix4x4"/> used to alter the image.
        /// </summary>
        Matrix4x4 Matrix { get; }
    }
}
