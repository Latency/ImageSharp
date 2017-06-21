﻿// <copyright file="ImageDataAttributeBase.cs" company="Six Labors">
// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace SixLabors.ImageSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Xunit.Sdk;

    /// <summary>
    /// Base class for Theory Data attributes which pass an instance of <see cref="TestImageProvider{TPixel}"/> to the test case.
    /// </summary>
    public abstract class ImageDataAttributeBase : DataAttribute
    {
        protected readonly object[] AdditionalParameters;

        protected readonly PixelTypes PixelTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageDataAttributeBase"/> class.
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="pixelTypes"></param>
        /// <param name="additionalParameters"></param>
        protected ImageDataAttributeBase(string memberName, PixelTypes pixelTypes, object[] additionalParameters)
        {
            this.PixelTypes = pixelTypes;
            this.AdditionalParameters = additionalParameters;
            this.MemberName = memberName;
        }

        /// <summary>
        /// Gets the member name
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets the member type
        /// </summary>
        public Type MemberType { get; set; }

        /// <summary>Returns the data to be used to test the theory.</summary>
        /// <param name="testMethod">The method that is being tested</param>
        /// <returns>One or more sets of theory data. Each invocation of the test method
        /// is represented by a single object array.</returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            IEnumerable<object[]> addedRows = Enumerable.Empty<object[]>().ToArray();
            if (!string.IsNullOrWhiteSpace(this.MemberName))
            {
                Type type = this.MemberType ?? testMethod.DeclaringType;
                Func<object> accessor = this.GetPropertyAccessor(type) ?? this.GetFieldAccessor(type);

                if (accessor != null)
                {
                    object obj = accessor();
                    if (obj is IEnumerable<object> memberItems)
                    {
                        addedRows = memberItems.Select(x => x as object[]);
                        if (addedRows.Any(x => x == null))
                        {
                            throw new ArgumentException($"Property {this.MemberName} on {this.MemberType ?? testMethod.DeclaringType} yielded an item that is not an object[]");
                        }
                    }
                }
            }

            if (!addedRows.Any())
            {
                addedRows = new[] { new object[0] };
            }

            bool firstIsprovider = this.FirstIsProvider(testMethod);
            if (firstIsprovider)
            {
                return this.InnerGetData(testMethod, addedRows);
            }

            return addedRows.Select(x => x.Concat(this.AdditionalParameters).ToArray());
        }

        /// <summary>
        /// Returns a value indicating whether the first parameter of the method is a test provider.
        /// </summary>
        /// <param name="testMethod"></param>
        /// <returns></returns>
        private bool FirstIsProvider(MethodInfo testMethod)
        {
            TypeInfo dataType = testMethod.GetParameters().First().ParameterType.GetTypeInfo();
            return dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(TestImageProvider<>);
        }

        private IEnumerable<object[]> InnerGetData(MethodInfo testMethod, IEnumerable<object[]> memberData)
        {
            foreach (KeyValuePair<PixelTypes, Type> kv in this.PixelTypes.ExpandAllTypes())
            {
                Type factoryType = typeof(TestImageProvider<>).MakeGenericType(kv.Value);

                foreach (object[] originalFacoryMethodArgs in this.GetAllFactoryMethodArgs(testMethod, factoryType))
                {
                    foreach (object[] row in memberData)
                    {
                        object[] actualFactoryMethodArgs = new object[originalFacoryMethodArgs.Length + 2];
                        Array.Copy(originalFacoryMethodArgs, actualFactoryMethodArgs, originalFacoryMethodArgs.Length);
                        actualFactoryMethodArgs[actualFactoryMethodArgs.Length - 2] = testMethod;
                        actualFactoryMethodArgs[actualFactoryMethodArgs.Length - 1] = kv.Key;

                        object factory = factoryType.GetMethod(this.GetFactoryMethodName(testMethod))
                            .Invoke(null, actualFactoryMethodArgs);

                        object[] result = new object[this.AdditionalParameters.Length + 1 + row.Length];
                        result[0] = factory;
                        Array.Copy(row, 0, result, 1, row.Length);
                        Array.Copy(this.AdditionalParameters, 0, result, 1 + row.Length, this.AdditionalParameters.Length);
                        yield return result;
                    }
                }
            }
        }

        /// <summary>
        /// Generates the collection of method arguments from the given test as a generic enumerable.
        /// </summary>
        /// <param name="testMethod">The test method</param>
        /// <param name="factoryType">The test image provider factory type</param>
        /// <returns>The <see cref="IEnumerable{T}"/></returns>
        protected virtual IEnumerable<object[]> GetAllFactoryMethodArgs(MethodInfo testMethod, Type factoryType)
        {
            object[] args = this.GetFactoryMethodArgs(testMethod, factoryType);
            return Enumerable.Repeat(args, 1);
        }

        /// <summary>
        /// Generates the collection of method arguments from the given test.
        /// </summary>
        /// <param name="testMethod">The test method</param>
        /// <param name="factoryType">The test image provider factory type</param>
        /// <returns>The <see cref="T:object[]"/></returns>
        protected virtual object[] GetFactoryMethodArgs(MethodInfo testMethod, Type factoryType)
        {
            throw new InvalidOperationException("Semi-abstract method");
        }

        /// <summary>
        /// Generates the method name from the given test method.
        /// </summary>
        /// <param name="testMethod">The test method</param>
        /// <returns>The <see cref="string"/></returns>
        protected abstract string GetFactoryMethodName(MethodInfo testMethod);

        /// <summary>
        /// Gets the field accessor for the given type.
        /// </summary>
        Func<object> GetFieldAccessor(Type type)
        {
            FieldInfo fieldInfo = null;
            for (Type reflectionType = type; reflectionType != null; reflectionType = reflectionType.GetTypeInfo().BaseType)
            {
                fieldInfo = reflectionType.GetRuntimeField(this.MemberName);
                if (fieldInfo != null)
                    break;
            }

            if (fieldInfo == null || !fieldInfo.IsStatic)
                return null;

            return () => fieldInfo.GetValue(null);
        }

        /// <summary>
        /// Gets the property accessor for the given type.
        /// </summary>
        Func<object> GetPropertyAccessor(Type type)
        {
            PropertyInfo propInfo = null;
            for (Type reflectionType = type; reflectionType != null; reflectionType = reflectionType.GetTypeInfo().BaseType)
            {
                propInfo = reflectionType.GetRuntimeProperty(this.MemberName);
                if (propInfo != null)
                {
                    break;
                }
            }

            if (propInfo?.GetMethod == null || !propInfo.GetMethod.IsStatic)
            {
                return null;
            }

            return () => propInfo.GetValue(null, null);
        }
    }
}