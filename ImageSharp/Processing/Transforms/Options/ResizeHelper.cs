﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Linq;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Processing
{
    /// <summary>
    /// Provides methods to help calculate the target rectangle when resizing using the
    /// <see cref="ResizeMode"/> enumeration.
    /// </summary>
    internal static class ResizeHelper
    {
        /// <summary>
        /// Calculates the target location and bounds to perform the resize operation against.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The source image.</param>
        /// <param name="options">The resize options.</param>
        /// <returns>
        /// The <see cref="Rectangle"/>.
        /// </returns>
        public static Rectangle CalculateTargetLocationAndBounds<TPixel>(ImageFrame<TPixel> source, ResizeOptions options)
            where TPixel : struct, IPixel<TPixel>
        {
            switch (options.Mode)
            {
                case ResizeMode.Crop:
                    return CalculateCropRectangle(source, options);
                case ResizeMode.Pad:
                    return CalculatePadRectangle(source, options);
                case ResizeMode.BoxPad:
                    return CalculateBoxPadRectangle(source, options);
                case ResizeMode.Max:
                    return CalculateMaxRectangle(source, options);
                case ResizeMode.Min:
                    return CalculateMinRectangle(source, options);

                // Last case ResizeMode.Stretch:
                default:
                    return new Rectangle(0, 0, options.Size.Width, options.Size.Height);
            }
        }

        /// <summary>
        /// Calculates the target rectangle for crop mode.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The source image.</param>
        /// <param name="options">The resize options.</param>
        /// <returns>
        /// The <see cref="Rectangle"/>.
        /// </returns>
        private static Rectangle CalculateCropRectangle<TPixel>(ImageFrame<TPixel> source, ResizeOptions options)
            where TPixel : struct, IPixel<TPixel>
        {
            int width = options.Size.Width;
            int height = options.Size.Height;

            if (width <= 0 || height <= 0)
            {
                return new Rectangle(0, 0, source.Width, source.Height);
            }

            float ratio;
            int sourceWidth = source.Width;
            int sourceHeight = source.Height;

            int destinationX = 0;
            int destinationY = 0;
            int destinationWidth = width;
            int destinationHeight = height;

            // Fractional variants for preserving aspect ratio.
            float percentHeight = Math.Abs(height / (float)sourceHeight);
            float percentWidth = Math.Abs(width / (float)sourceWidth);

            if (percentHeight < percentWidth)
            {
                ratio = percentWidth;

                if (options.CenterCoordinates.Any())
                {
                    float center = -(ratio * sourceHeight) * options.CenterCoordinates.ToArray()[1];
                    destinationY = (int)Math.Round(center + (height / 2F));

                    if (destinationY > 0)
                    {
                        destinationY = 0;
                    }

                    if (destinationY < (int)Math.Round(height - (sourceHeight * ratio)))
                    {
                        destinationY = (int)Math.Round(height - (sourceHeight * ratio));
                    }
                }
                else
                {
                    switch (options.Position)
                    {
                        case AnchorPosition.Top:
                        case AnchorPosition.TopLeft:
                        case AnchorPosition.TopRight:
                            destinationY = 0;
                            break;
                        case AnchorPosition.Bottom:
                        case AnchorPosition.BottomLeft:
                        case AnchorPosition.BottomRight:
                            destinationY = (int)Math.Round(height - (sourceHeight * ratio));
                            break;
                        default:
                            destinationY = (int)Math.Round((height - (sourceHeight * ratio)) / 2F);
                            break;
                    }
                }

                destinationHeight = (int)Math.Ceiling(sourceHeight * percentWidth);
            }
            else
            {
                ratio = percentHeight;

                if (options.CenterCoordinates.Any())
                {
                    float center = -(ratio * sourceWidth) * options.CenterCoordinates.First();
                    destinationX = (int)Math.Round(center + (width / 2F));

                    if (destinationX > 0)
                    {
                        destinationX = 0;
                    }

                    if (destinationX < (int)Math.Round(width - (sourceWidth * ratio)))
                    {
                        destinationX = (int)Math.Round(width - (sourceWidth * ratio));
                    }
                }
                else
                {
                    switch (options.Position)
                    {
                        case AnchorPosition.Left:
                        case AnchorPosition.TopLeft:
                        case AnchorPosition.BottomLeft:
                            destinationX = 0;
                            break;
                        case AnchorPosition.Right:
                        case AnchorPosition.TopRight:
                        case AnchorPosition.BottomRight:
                            destinationX = (int)Math.Round(width - (sourceWidth * ratio));
                            break;
                        default:
                            destinationX = (int)Math.Round((width - (sourceWidth * ratio)) / 2F);
                            break;
                    }
                }

                destinationWidth = (int)Math.Ceiling(sourceWidth * percentHeight);
            }

            return new Rectangle(destinationX, destinationY, destinationWidth, destinationHeight);
        }

        /// <summary>
        /// Calculates the target rectangle for pad mode.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The source image.</param>
        /// <param name="options">The resize options.</param>
        /// <returns>
        /// The <see cref="Rectangle"/>.
        /// </returns>
        private static Rectangle CalculatePadRectangle<TPixel>(ImageFrame<TPixel> source, ResizeOptions options)
            where TPixel : struct, IPixel<TPixel>
        {
            int width = options.Size.Width;
            int height = options.Size.Height;

            if (width <= 0 || height <= 0)
            {
                return new Rectangle(0, 0, source.Width, source.Height);
            }

            float ratio;
            int sourceWidth = source.Width;
            int sourceHeight = source.Height;

            int destinationX = 0;
            int destinationY = 0;
            int destinationWidth = width;
            int destinationHeight = height;

            // Fractional variants for preserving aspect ratio.
            float percentHeight = Math.Abs(height / (float)sourceHeight);
            float percentWidth = Math.Abs(width / (float)sourceWidth);

            if (percentHeight < percentWidth)
            {
                ratio = percentHeight;
                destinationWidth = (int)Math.Round(sourceWidth * percentHeight);

                switch (options.Position)
                {
                    case AnchorPosition.Left:
                    case AnchorPosition.TopLeft:
                    case AnchorPosition.BottomLeft:
                        destinationX = 0;
                        break;
                    case AnchorPosition.Right:
                    case AnchorPosition.TopRight:
                    case AnchorPosition.BottomRight:
                        destinationX = (int)Math.Round(width - (sourceWidth * ratio));
                        break;
                    default:
                        destinationX = (int)Math.Round((width - (sourceWidth * ratio)) / 2F);
                        break;
                }
            }
            else
            {
                ratio = percentWidth;
                destinationHeight = (int)Math.Round(sourceHeight * percentWidth);

                switch (options.Position)
                {
                    case AnchorPosition.Top:
                    case AnchorPosition.TopLeft:
                    case AnchorPosition.TopRight:
                        destinationY = 0;
                        break;
                    case AnchorPosition.Bottom:
                    case AnchorPosition.BottomLeft:
                    case AnchorPosition.BottomRight:
                        destinationY = (int)Math.Round(height - (sourceHeight * ratio));
                        break;
                    default:
                        destinationY = (int)Math.Round((height - (sourceHeight * ratio)) / 2F);
                        break;
                }
            }

            return new Rectangle(destinationX, destinationY, destinationWidth, destinationHeight);
        }

        /// <summary>
        /// Calculates the target rectangle for box pad mode.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The source image.</param>
        /// <param name="options">The resize options.</param>
        /// <returns>
        /// The <see cref="Rectangle"/>.
        /// </returns>
        private static Rectangle CalculateBoxPadRectangle<TPixel>(ImageFrame<TPixel> source, ResizeOptions options)
            where TPixel : struct, IPixel<TPixel>
        {
            int width = options.Size.Width;
            int height = options.Size.Height;

            if (width <= 0 || height <= 0)
            {
                return new Rectangle(0, 0, source.Width, source.Height);
            }

            int sourceWidth = source.Width;
            int sourceHeight = source.Height;

            // Fractional variants for preserving aspect ratio.
            float percentHeight = Math.Abs(height / (float)sourceHeight);
            float percentWidth = Math.Abs(width / (float)sourceWidth);

            int boxPadHeight = height > 0 ? height : (int)Math.Round(sourceHeight * percentWidth);
            int boxPadWidth = width > 0 ? width : (int)Math.Round(sourceWidth * percentHeight);

            // Only calculate if upscaling.
            if (sourceWidth < boxPadWidth && sourceHeight < boxPadHeight)
            {
                int destinationX;
                int destinationY;
                int destinationWidth = sourceWidth;
                int destinationHeight = sourceHeight;
                width = boxPadWidth;
                height = boxPadHeight;

                switch (options.Position)
                {
                    case AnchorPosition.Left:
                        destinationY = (height - sourceHeight) / 2;
                        destinationX = 0;
                        break;
                    case AnchorPosition.Right:
                        destinationY = (height - sourceHeight) / 2;
                        destinationX = width - sourceWidth;
                        break;
                    case AnchorPosition.TopRight:
                        destinationY = 0;
                        destinationX = width - sourceWidth;
                        break;
                    case AnchorPosition.Top:
                        destinationY = 0;
                        destinationX = (width - sourceWidth) / 2;
                        break;
                    case AnchorPosition.TopLeft:
                        destinationY = 0;
                        destinationX = 0;
                        break;
                    case AnchorPosition.BottomRight:
                        destinationY = height - sourceHeight;
                        destinationX = width - sourceWidth;
                        break;
                    case AnchorPosition.Bottom:
                        destinationY = height - sourceHeight;
                        destinationX = (width - sourceWidth) / 2;
                        break;
                    case AnchorPosition.BottomLeft:
                        destinationY = height - sourceHeight;
                        destinationX = 0;
                        break;
                    default:
                        destinationY = (height - sourceHeight) / 2;
                        destinationX = (width - sourceWidth) / 2;
                        break;
                }

                return new Rectangle(destinationX, destinationY, destinationWidth, destinationHeight);
            }

            // Switch to pad mode to downscale and calculate from there.
            return CalculatePadRectangle(source, options);
        }

        /// <summary>
        /// Calculates the target rectangle for max mode.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The source image.</param>
        /// <param name="options">The resize options.</param>
        /// <returns>
        /// The <see cref="Rectangle"/>.
        /// </returns>
        private static Rectangle CalculateMaxRectangle<TPixel>(ImageFrame<TPixel> source, ResizeOptions options)
            where TPixel : struct, IPixel<TPixel>
        {
            int width = options.Size.Width;
            int height = options.Size.Height;
            int destinationWidth = width;
            int destinationHeight = height;

            // Fractional variants for preserving aspect ratio.
            float percentHeight = Math.Abs(height / (float)source.Height);
            float percentWidth = Math.Abs(width / (float)source.Width);

            // Integers must be cast to floats to get needed precision
            float ratio = options.Size.Height / (float)options.Size.Width;
            float sourceRatio = source.Height / (float)source.Width;

            if (sourceRatio < ratio)
            {
                destinationHeight = (int)Math.Round(source.Height * percentWidth);
                height = destinationHeight;
            }
            else
            {
                destinationWidth = (int)Math.Round(source.Width * percentHeight);
                width = destinationWidth;
            }

            // Replace the size to match the rectangle.
            options.Size = new Size(width, height);
            return new Rectangle(0, 0, destinationWidth, destinationHeight);
        }

        /// <summary>
        /// Calculates the target rectangle for min mode.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The source image.</param>
        /// <param name="options">The resize options.</param>
        /// <returns>
        /// The <see cref="Rectangle"/>.
        /// </returns>
        private static Rectangle CalculateMinRectangle<TPixel>(ImageFrame<TPixel> source, ResizeOptions options)
            where TPixel : struct, IPixel<TPixel>
        {
            int width = options.Size.Width;
            int height = options.Size.Height;
            int sourceWidth = source.Width;
            int sourceHeight = source.Height;
            int destinationWidth;
            int destinationHeight;

            // Don't upscale
            if (width > sourceWidth || height > sourceHeight)
            {
                options.Size = new Size(sourceWidth, sourceHeight);
                return new Rectangle(0, 0, sourceWidth, sourceHeight);
            }

            // Fractional variants for preserving aspect ratio.
            float percentHeight = Math.Abs(height / (float)sourceHeight);
            float percentWidth = Math.Abs(width / (float)sourceWidth);

            float sourceRatio = (float)sourceHeight / sourceWidth;

            // Find the shortest distance to go.
            int widthDiff = sourceWidth - width;
            int heightDiff = sourceHeight - height;

            if (widthDiff < heightDiff)
            {
                destinationHeight = (int)Math.Round(width * sourceRatio);
                height = destinationHeight;
                destinationWidth = width;
            }
            else if (widthDiff > heightDiff)
            {
                destinationWidth = (int)Math.Round(height / sourceRatio);
                destinationHeight = height;
                width = destinationWidth;
            }
            else
            {
                if (height > width)
                {
                    destinationWidth = width;
                    destinationHeight = (int)Math.Round(sourceHeight * percentWidth);
                    height = destinationHeight;
                }
                else
                {
                    destinationHeight = height;
                    destinationWidth = (int)Math.Round(sourceWidth * percentHeight);
                    width = destinationWidth;
                }
            }

            // Replace the size to match the rectangle.
            options.Size = new Size(width, height);
            return new Rectangle(0, 0, destinationWidth, destinationHeight);
        }
    }
}