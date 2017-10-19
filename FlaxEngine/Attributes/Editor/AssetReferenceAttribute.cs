////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Specifies a options for a asset reference picker in the editor. Allows to customize view or provide custom value assign policy.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class AssetReferenceAttribute : Attribute
    {
        /// <summary>
        /// The full name of the asset type to link. Use null or empty to skip it.
        /// </summary>
        public string TypeName;

        /// <summary>
        /// True if use asset picker with a smaller height (single line), otherwise will use with full icon.
        /// </summary>
        public bool UseSmallPicker;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetReferenceAttribute"/> class.
        /// </summary>
        /// <param name="typeName">The full name of the asset type to link. Use null or empty to skip it.</param>
        /// <param name="useSmallPicker">True if use asset picker with a smaller height (single line), otherwise will use with full icon.</param>
        public AssetReferenceAttribute(Type typeName = null, bool useSmallPicker = false)
        {
            TypeName = typeName?.FullName;
            UseSmallPicker = useSmallPicker;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetReferenceAttribute"/> class.
        /// </summary>
        /// <param name="typeName">The full name of the asset type to link. Use null or empty to skip it.</param>
        /// <param name="useSmallPicker">True if use asset picker with a smaller height (single line), otherwise will use with full icon.</param>
        public AssetReferenceAttribute(string typeName = null, bool useSmallPicker = false)
        {
            TypeName = typeName;
            UseSmallPicker = useSmallPicker;
        }
    }
}