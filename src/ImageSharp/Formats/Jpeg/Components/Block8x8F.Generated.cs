﻿// <copyright file="Block8x8F.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>
// ReSharper disable InconsistentNaming
// <auto-generated />
#pragma warning disable
namespace SixLabors.ImageSharp.Formats.Jpg
{
	using System.Numerics;
	using System.Runtime.CompilerServices;

	internal partial struct Block8x8F
    {
        private static readonly Vector4 CMin4 = new Vector4(0F);
        private static readonly Vector4 CMax4 = new Vector4(255F);
        private static readonly Vector4 COff4 = new Vector4(128F);

		/// <summary>
        /// Transpose the block into the destination block.
        /// </summary>
        /// <param name="d">The destination block</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TransposeInto(ref Block8x8F d)
        {
            d.V0L.X = V0L.X;
            d.V1L.X = V0L.Y;
            d.V2L.X = V0L.Z;
            d.V3L.X = V0L.W;
            d.V4L.X = V0R.X;
            d.V5L.X = V0R.Y;
            d.V6L.X = V0R.Z;
            d.V7L.X = V0R.W;

            d.V0L.Y = V1L.X;
            d.V1L.Y = V1L.Y;
            d.V2L.Y = V1L.Z;
            d.V3L.Y = V1L.W;
            d.V4L.Y = V1R.X;
            d.V5L.Y = V1R.Y;
            d.V6L.Y = V1R.Z;
            d.V7L.Y = V1R.W;

            d.V0L.Z = V2L.X;
            d.V1L.Z = V2L.Y;
            d.V2L.Z = V2L.Z;
            d.V3L.Z = V2L.W;
            d.V4L.Z = V2R.X;
            d.V5L.Z = V2R.Y;
            d.V6L.Z = V2R.Z;
            d.V7L.Z = V2R.W;

            d.V0L.W = V3L.X;
            d.V1L.W = V3L.Y;
            d.V2L.W = V3L.Z;
            d.V3L.W = V3L.W;
            d.V4L.W = V3R.X;
            d.V5L.W = V3R.Y;
            d.V6L.W = V3R.Z;
            d.V7L.W = V3R.W;

            d.V0R.X = V4L.X;
            d.V1R.X = V4L.Y;
            d.V2R.X = V4L.Z;
            d.V3R.X = V4L.W;
            d.V4R.X = V4R.X;
            d.V5R.X = V4R.Y;
            d.V6R.X = V4R.Z;
            d.V7R.X = V4R.W;

            d.V0R.Y = V5L.X;
            d.V1R.Y = V5L.Y;
            d.V2R.Y = V5L.Z;
            d.V3R.Y = V5L.W;
            d.V4R.Y = V5R.X;
            d.V5R.Y = V5R.Y;
            d.V6R.Y = V5R.Z;
            d.V7R.Y = V5R.W;

            d.V0R.Z = V6L.X;
            d.V1R.Z = V6L.Y;
            d.V2R.Z = V6L.Z;
            d.V3R.Z = V6L.W;
            d.V4R.Z = V6R.X;
            d.V5R.Z = V6R.Y;
            d.V6R.Z = V6R.Z;
            d.V7R.Z = V6R.W;

            d.V0R.W = V7L.X;
            d.V1R.W = V7L.Y;
            d.V2R.W = V7L.Z;
            d.V3R.W = V7L.W;
            d.V4R.W = V7R.X;
            d.V5R.W = V7R.Y;
            d.V6R.W = V7R.Z;
            d.V7R.W = V7R.W;
        }

		/// <summary>
        /// Level shift by +128, clip to [0, 255]
        /// </summary>
        /// <param name="d">The destination block</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void TransformByteConvetibleColorValuesInto(ref Block8x8F d)
        {
            d.V0L = Vector4.Clamp(V0L + COff4, CMin4, CMax4);
            d.V0R = Vector4.Clamp(V0R + COff4, CMin4, CMax4);
            d.V1L = Vector4.Clamp(V1L + COff4, CMin4, CMax4);
            d.V1R = Vector4.Clamp(V1R + COff4, CMin4, CMax4);
            d.V2L = Vector4.Clamp(V2L + COff4, CMin4, CMax4);
            d.V2R = Vector4.Clamp(V2R + COff4, CMin4, CMax4);
            d.V3L = Vector4.Clamp(V3L + COff4, CMin4, CMax4);
            d.V3R = Vector4.Clamp(V3R + COff4, CMin4, CMax4);
            d.V4L = Vector4.Clamp(V4L + COff4, CMin4, CMax4);
            d.V4R = Vector4.Clamp(V4R + COff4, CMin4, CMax4);
            d.V5L = Vector4.Clamp(V5L + COff4, CMin4, CMax4);
            d.V5R = Vector4.Clamp(V5R + COff4, CMin4, CMax4);
            d.V6L = Vector4.Clamp(V6L + COff4, CMin4, CMax4);
            d.V6R = Vector4.Clamp(V6R + COff4, CMin4, CMax4);
            d.V7L = Vector4.Clamp(V7L + COff4, CMin4, CMax4);
            d.V7R = Vector4.Clamp(V7R + COff4, CMin4, CMax4);
        }
	}
}
