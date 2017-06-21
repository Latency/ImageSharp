﻿// <copyright file="IccClutDataType.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp
{
    /// <summary>
    /// Color lookup table data type
    /// </summary>
    internal enum IccClutDataType
    {
        /// <summary>
        /// 32bit floating point
        /// </summary>
        Float,

        /// <summary>
        /// 8bit unsigned integer (byte)
        /// </summary>
        UInt8,

        /// <summary>
        /// 16bit unsigned integer (ushort)
        /// </summary>
        UInt16,
    }
}
