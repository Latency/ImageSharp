﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Dithering;
using SixLabors.ImageSharp.Helpers;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Processing.Processors
{
    /// <summary>
    /// An <see cref="IImageProcessor{TPixel}"/> that dithers an image using error diffusion.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal class ErrorDiffusionPaletteProcessor<TPixel> : PaletteDitherProcessorBase<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDiffusionPaletteProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="diffuser">The error diffuser</param>
        public ErrorDiffusionPaletteProcessor(IErrorDiffuser diffuser)
            : this(diffuser, .5F)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDiffusionPaletteProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="diffuser">The error diffuser</param>
        /// <param name="threshold">The threshold to split the image. Must be between 0 and 1.</param>
        public ErrorDiffusionPaletteProcessor(IErrorDiffuser diffuser, float threshold)
            : this(diffuser, threshold, NamedColors<TPixel>.WebSafePalette)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDiffusionPaletteProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="diffuser">The error diffuser</param>
        /// <param name="threshold">The threshold to split the image. Must be between 0 and 1.</param>
        /// <param name="palette">The palette to select substitute colors from.</param>
        public ErrorDiffusionPaletteProcessor(IErrorDiffuser diffuser, float threshold, TPixel[] palette)
            : base(palette)
        {
            Guard.NotNull(diffuser, nameof(diffuser));
            Guard.MustBeBetweenOrEqualTo(threshold, 0, 1, nameof(threshold));

            this.Diffuser = diffuser;
            this.Threshold = threshold;
        }

        /// <summary>
        /// Gets the error diffuser.
        /// </summary>
        public IErrorDiffuser Diffuser { get; }

        /// <summary>
        /// Gets the threshold value.
        /// </summary>
        public float Threshold { get; }

        /// <inheritdoc/>
        protected override void OnApply(ImageFrame<TPixel> source, Rectangle sourceRectangle, Configuration configuration)
        {
            float threshold = this.Threshold * 255F;
            var rgba = default(Rgba32);
            bool isAlphaOnly = typeof(TPixel) == typeof(Alpha8);

            var interest = Rectangle.Intersect(sourceRectangle, source.Bounds());
            int startY = interest.Y;
            int endY = interest.Bottom;
            int startX = interest.X;
            int endX = interest.Right;

            // Collect the values before looping so we can reduce our calculation count for identical sibling pixels
            TPixel sourcePixel = source[startX, startY];
            TPixel previousPixel = sourcePixel;
            PixelPair<TPixel> pair = this.GetClosestPixelPair(ref sourcePixel, this.Palette);
            sourcePixel.ToRgba32(ref rgba);

            // Convert to grayscale using ITU-R Recommendation BT.709 if required
            byte luminance = (byte)(isAlphaOnly ? rgba.A : (.2126F * rgba.R) + (.7152F * rgba.G) + (.0722F * rgba.B)).Clamp(0, 255);

            for (int y = startY; y < endY; y++)
            {
                Span<TPixel> row = source.GetPixelRowSpan(y);

                for (int x = startX; x < endX; x++)
                {
                    sourcePixel = row[x];

                    // Check if this is the same as the last pixel. If so use that value
                    // rather than calculating it again. This is an inexpensive optimization.
                    if (!previousPixel.Equals(sourcePixel))
                    {
                        pair = this.GetClosestPixelPair(ref sourcePixel, this.Palette);
                        sourcePixel.ToRgba32(ref rgba);
                        luminance = (byte)(isAlphaOnly ? rgba.A : (.2126F * rgba.R) + (.7152F * rgba.G) + (.0722F * rgba.B)).Clamp(0, 255);

                        // Setup the previous pointer
                        previousPixel = sourcePixel;
                    }

                    TPixel transformedPixel = luminance >= threshold ? pair.First : pair.Second;
                    this.Diffuser.Dither(source, sourcePixel, transformedPixel, x, y, startX, startY, endX, endY);
                }
            }
        }
    }
}