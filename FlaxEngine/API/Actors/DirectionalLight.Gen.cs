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
	/// Directional Light can emmit light from direction in space
	/// </summary>
	[Serializable]
	public sealed partial class DirectionalLight : Actor
	{
		/// <summary>
		/// Creates new <see cref="DirectionalLight"/> object.
		/// </summary>
		private DirectionalLight() : base()
		{
		}

		/// <summary>
		/// Creates new instance of <see cref="DirectionalLight"/> object.
		/// </summary>
		/// <returns>Created object.</returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
		[UnmanagedCall]
		public static DirectionalLight New() 
		{
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
			return Internal_Create(typeof(DirectionalLight)) as DirectionalLight;
#endif
		}

		/// <summary>
		/// Gets or sets value indicating if visual element affects the world
		/// </summary>
		[UnmanagedCall]
		public bool AffectsWorld
		{
#if UNIT_TEST_COMPILANT
			get; set;
#else
			get { return Internal_GetAffectsWorld(unmanagedPtr); }
			set { Internal_SetAffectsWorld(unmanagedPtr, value); }
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		[UnmanagedCall]
		public Color Color
		{
#if UNIT_TEST_COMPILANT
			get; set;
#else
			get { Color resultAsRef; Internal_GetColor(unmanagedPtr, out resultAsRef); return resultAsRef; }
			set { Internal_SetColor(unmanagedPtr, ref value); }
#endif
		}

		/// <summary>
		/// Gets or sets light brightness parameter
		/// </summary>
		[UnmanagedCall]
		public float Brightness
		{
#if UNIT_TEST_COMPILANT
			get; set;
#else
			get { return Internal_GetBrightness(unmanagedPtr); }
			set { Internal_SetBrightness(unmanagedPtr, value); }
#endif
		}

		/// <summary>
		/// Gets or sets light shadows casting distance from view
		/// </summary>
		[UnmanagedCall]
		public float ShadowsDistance
		{
#if UNIT_TEST_COMPILANT
			get; set;
#else
			get { return Internal_GetShadowsDistance(unmanagedPtr); }
			set { Internal_SetShadowsDistance(unmanagedPtr, value); }
#endif
		}

		/// <summary>
		/// Gets light shadows fade off distance
		/// </summary>
		[UnmanagedCall]
		public float ShadowsFadeDistance
		{
#if UNIT_TEST_COMPILANT
			get; set;
#else
			get { return Internal_GetShadowsFadeDistance(unmanagedPtr); }
			set { Internal_SetShadowsFadeDistance(unmanagedPtr, value); }
#endif
		}

		/// <summary>
		/// Gets or sets the minimum roughness value used to clamp material surface roughness during shading pixel.
		/// </summary>
		[UnmanagedCall]
		public float MinimumRoughness
		{
#if UNIT_TEST_COMPILANT
			get; set;
#else
			get { return Internal_GetMinimumRoughness(unmanagedPtr); }
			set { Internal_SetMinimumRoughness(unmanagedPtr, value); }
#endif
		}

		/// <summary>
		/// Gets or sets value indicating if how visual element casts shadows
		/// </summary>
		[UnmanagedCall]
		public ShadowsCastingMode ShadowsMode
		{
#if UNIT_TEST_COMPILANT
			get; set;
#else
			get { return Internal_GetShadowsMode(unmanagedPtr); }
			set { Internal_SetShadowsMode(unmanagedPtr, value); }
#endif
		}

#region Internal Calls
#if !UNIT_TEST_COMPILANT
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Internal_GetAffectsWorld(IntPtr obj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_SetAffectsWorld(IntPtr obj, bool val);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_GetColor(IntPtr obj, out Color resultAsRef);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_SetColor(IntPtr obj, ref Color val);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float Internal_GetBrightness(IntPtr obj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_SetBrightness(IntPtr obj, float val);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float Internal_GetShadowsDistance(IntPtr obj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_SetShadowsDistance(IntPtr obj, float val);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float Internal_GetShadowsFadeDistance(IntPtr obj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_SetShadowsFadeDistance(IntPtr obj, float val);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float Internal_GetMinimumRoughness(IntPtr obj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_SetMinimumRoughness(IntPtr obj, float val);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ShadowsCastingMode Internal_GetShadowsMode(IntPtr obj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_SetShadowsMode(IntPtr obj, ShadowsCastingMode val);
#endif
#endregion
	}
}

