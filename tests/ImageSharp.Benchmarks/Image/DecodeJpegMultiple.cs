﻿// <copyright file="DecodeJpegMultiple.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp.Benchmarks.Image
{
    using System.Collections.Generic;
    using BenchmarkDotNet.Attributes;

    using CoreImage = ImageSharp.Image;

    [Config(typeof(Config.Short))]
    public class DecodeJpegMultiple : MultiImageBenchmarkBase
    {
        protected override IEnumerable<string> InputImageSubfoldersOrFiles => new[]
        {
            "Jpg/"
        };

        protected override IEnumerable<string> SearchPatterns => new[] { "*.jpg" };

        [Benchmark(Description = "DecodeJpegMultiple - ImageSharp")]
        public void DecodeJpegImageSharp()
        {
            this.ForEachStream(
                ms => CoreImage.Load<Rgba32>(ms)
                );
        }

        [Benchmark(Baseline = true, Description = "DecodeJpegMultiple - System.Drawing")]
        public void DecodeJpegSystemDrawing()
        {
            this.ForEachStream(
                System.Drawing.Image.FromStream
                );
        }

    }
}