﻿// <copyright file="ResizeOptions.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp.Processing
{
    using System.Collections.Generic;
    using System.Linq;
    using SixLabors.Primitives;

    /// <summary>
    /// The resize options for resizing images against certain modes.
    /// </summary>
    public class ResizeOptions
    {
        /// <summary>
        /// Gets or sets the resize mode.
        /// </summary>
        public ResizeMode Mode { get; set; } = ResizeMode.Crop;

        /// <summary>
        /// Gets or sets the anchor position.
        /// </summary>
        public AnchorPosition Position { get; set; } = AnchorPosition.Center;

        /// <summary>
        /// Gets or sets the center coordinates.
        /// </summary>
        public IEnumerable<float> CenterCoordinates { get; set; } = Enumerable.Empty<float>();

        /// <summary>
        /// Gets or sets the target size.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Gets or sets the sampler to perform the resize operation.
        /// </summary>
        public IResampler Sampler { get; set; } = new BicubicResampler();

        /// <summary>
        /// Gets or sets a value indicating whether to compress
        /// or expand individual pixel colors the value on processing.
        /// </summary>
        public bool Compand { get; set; }
    }
}
