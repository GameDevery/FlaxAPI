////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
	/// <summary>
	/// Instance of the <seealso cref="Material" /> with custom set of material parameter values.
	/// </summary>
	public sealed partial class MaterialInstance : MaterialBase
	{
		/// <summary>
		/// Creates new <see cref="MaterialInstance"/> object.
		/// </summary>
		private MaterialInstance() : base()
		{
		}

		/// <summary>
		/// Gets or sets the base material. If value gets changed parameters collection is restored to the default values of the new material.
		/// </summary>
		[UnmanagedCall]
		public Material BaseMaterial
		{
#if UNIT_TEST_COMPILANT
			get; set;
#else
			get { return Internal_GetBaseMaterial(unmanagedPtr); }
			set { Internal_SetBaseMaterial(unmanagedPtr, Object.GetUnmanagedPtr(value)); }
#endif
		}

		/// <summary>
		/// Saves asset to the file.
		/// </summary>
		/// <returns>True if cannot save data, otherwise false.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
		[UnmanagedCall]
		public bool Save() 
		{
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
			return Internal_Save(unmanagedPtr);
#endif
		}

#region Internal Calls
#if !UNIT_TEST_COMPILANT
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Material Internal_GetBaseMaterial(IntPtr obj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_SetBaseMaterial(IntPtr obj, IntPtr val);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Internal_Save(IntPtr obj);
#endif
#endregion
	}
}

