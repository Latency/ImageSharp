﻿// <copyright file="WelchResampler.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp.Processing
{
    /// <summary>
    /// The function implements the welch algorithm.
    /// <see href="http://www.imagemagick.org/Usage/filter/"/>
    /// </summary>
    public class WelchResampler : IResampler
    {
        /// <inheritdoc/>
        public float Radius => 3;

        /// <inheritdoc/>
        public float GetValue(float x)
        {
            if (x < 0F)
            {
                x = -x;
            }

            if (x < 3F)
            {
                return MathF.SinC(x) * (1F - (x * x / 9F));
            }

            return 0F;
        }
    }
}
