// <copyright file="EOFException.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp.Formats.Jpg
{
    using System;

    /// <summary>
    /// The EOF (End of File exception).
    /// Thrown when the decoder encounters an EOF marker without a proceeding EOI (End Of Image) marker
    /// TODO: Rename to UnexpectedEndOfStreamException
    /// </summary>
    internal class EOFException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EOFException"/> class.
        /// </summary>
        public EOFException()
            : base("Reached end of stream before proceeding EOI marker!")
        {
        }
    }
}