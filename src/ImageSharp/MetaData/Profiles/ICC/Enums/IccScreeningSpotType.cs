﻿// <copyright file="IccScreeningSpotType.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp
{
    /// <summary>
    /// Enumerates the screening spot types
    /// </summary>
    internal enum IccScreeningSpotType : int
    {
        /// <summary>
        /// Unknown spot type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Default printer spot type
        /// </summary>
        PrinterDefault = 1,

        /// <summary>
        /// Round stop type
        /// </summary>
        Round = 2,

        /// <summary>
        /// Diamond spot type
        /// </summary>
        Diamond = 3,

        /// <summary>
        /// Ellipse spot type
        /// </summary>
        Ellipse = 4,

        /// <summary>
        /// Line spot type
        /// </summary>
        Line = 5,

        /// <summary>
        /// Square spot type
        /// </summary>
        Square = 6,

        /// <summary>
        /// Cross spot type
        /// </summary>
        Cross = 7,
    }
}
