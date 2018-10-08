// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// The blend space editor used by the animation multi blend nodes.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public abstract class BlendPointsEditor : ContainerControl
    {
        private readonly bool _is2D;
        private Vector2 _rangeX;
        private Vector2 _rangeY;
        private readonly BlendPoint[] _blendPoints = new BlendPoint[Animation.MultiBlend1D.MaxAnimationsCount];
        private readonly Guid[] _pointsAnims = new Guid[Animation.MultiBlend1D.MaxAnimationsCount];
        private readonly Vector2[] _pointsLocations = new Vector2[Animation.MultiBlend1D.MaxAnimationsCount];

        /// <summary>
        /// Represents single blend point.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.Control" />
        protected class BlendPoint : Control
        {
            private static Matrix3x3 _transform = Matrix3x3.RotationZ(45.0f * Mathf.Deg2Rad) * Matrix3x3.Translation2D(4.0f, 0.5f);
            private readonly BlendPointsEditor _editor;
            private readonly int _index;
            private bool _isMouseDown;

            /// <summary>
            /// The default size for the blend points.
            /// </summary>
            public const float DefaultSize = 8.0f;

            /// <summary>
            /// Initializes a new instance of the <see cref="BlendPoint"/> class.
            /// </summary>
            /// <param name="editor">The editor.</param>
            /// <param name="index">The blend point index.</param>
            public BlendPoint(BlendPointsEditor editor, int index)
            : base(0, 0, DefaultSize, DefaultSize)
            {
                _editor = editor;
                _index = index;
            }

            /// <inheritdoc />
            public override void OnGotFocus()
            {
                base.OnGotFocus();

                _editor.SelectedIndex = _index;
            }

            /// <inheritdoc />
            public override void Draw()
            {
                // Cache data
                var isSelected = _editor.SelectedIndex == _index;

                // Draw rotated rectangle
                Render2D.PushTransform(ref _transform);
                Render2D.FillRectangle(new Rectangle(0, 0, 5, 5), isSelected ? Color.Orange : Color.DarkGreen);
                Render2D.PopTransform();
            }

            /// <inheritdoc />
            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left)
                {
                    Focus();
                    _isMouseDown = true;
                    StartMouseCapture();
                    return true;
                }

                return base.OnMouseDown(location, buttons);
            }

            /// <inheritdoc />
            public override bool OnMouseUp(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left && _isMouseDown)
                {
                    _isMouseDown = false;
                    EndMouseCapture();
                    return true;
                }

                return base.OnMouseUp(location, buttons);
            }

            /// <inheritdoc />
            public override void OnMouseMove(Vector2 location)
            {
                if (_isMouseDown)
                {
                    _editor.SetLocation(_index, _editor.BlendPointPosToBlendSpacePos(Location + location));
                }

                base.OnMouseMove(location);
            }

            /// <inheritdoc />
            public override void OnMouseLeave()
            {
                if (_isMouseDown)
                {
                    _isMouseDown = false;
                    EndMouseCapture();
                }

                base.OnMouseLeave();
            }

            /// <inheritdoc />
            public override void OnLostFocus()
            {
                if (_isMouseDown)
                {
                    _isMouseDown = false;
                    EndMouseCapture();
                }

                base.OnLostFocus();
            }

            /// <inheritdoc />
            public override void OnEndMouseCapture()
            {
                _isMouseDown = false;

                base.OnEndMouseCapture();
            }
        }

        /// <summary>
        /// Gets a value indicating whether blend space is 2D, otherwise it is 1D.
        /// </summary>
        public bool Is2D => _is2D;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendPointsEditor"/> class.
        /// </summary>
        /// <param name="is2D">The value indicating whether blend space is 2D, otherwise it is 1D.</param>
        /// <param name="x">The X location.</param>
        /// <param name="y">The Y location.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public BlendPointsEditor(bool is2D, float x, float y, float width, float height)
        : base(x, y, width, height)
        {
            _is2D = is2D;
        }

        /// <summary>
        /// Gets the blend space data.
        /// </summary>
        /// <param name="rangeX">The space range for X axis (X-width, Y-height).</param>
        /// <param name="rangeY">The space range for Y axis (X-width, Y-height).</param>
        /// <param name="pointsAnims">The points anims (input array to fill of size equal 14).</param>
        /// <param name="pointsLocations">The points locations (input array to fill of size equal 14).</param>
        public abstract void GetData(out Vector2 rangeX, out Vector2 rangeY, Guid[] pointsAnims, Vector2[] pointsLocations);

        /// <summary>
        /// Gets or sets the index of the selected blend point.
        /// </summary>
        public abstract int SelectedIndex { get; set; }

        /// <summary>
        /// Sets the blend point location.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="location">The location.</param>
        public abstract void SetLocation(int index, Vector2 location);

        /// <summary>
        /// Gets the blend points area.
        /// </summary>
        /// <param name="pointsArea">The control-space area.</param>
        public void GetPointsArea(out Rectangle pointsArea)
        {
            pointsArea = new Rectangle(Vector2.Zero, Size);
            pointsArea.Expand(-10.0f);
        }

        /// <summary>
        /// Converts the blend point position into the blend space position (defined by min/max per axis).
        /// </summary>
        /// <param name="pos">The blend point control position.</param>
        /// <returns>The blend space position.</returns>
        public Vector2 BlendPointPosToBlendSpacePos(Vector2 pos)
        {
            GetPointsArea(out var pointsArea);
            pos += new Vector2(BlendPoint.DefaultSize * 0.5f);
            if (_is2D)
            {
                pos = new Vector2(
                    Mathf.Map(pos.X, pointsArea.Left, pointsArea.Right, _rangeX.X, _rangeX.Y),
                    Mathf.Map(pos.Y, pointsArea.Right, pointsArea.Bottom, _rangeY.X, _rangeY.Y)
                );
            }
            else
            {
                pos = new Vector2(
                    Mathf.Map(pos.X, pointsArea.Left, pointsArea.Right, _rangeX.X, _rangeX.Y),
                    0
                );
            }
            return pos;
        }

        /// <summary>
        /// Converts the blend space position into the blend point position.
        /// </summary>
        /// <param name="pos">The blend space position.</param>
        /// <returns>The blend point control position.</returns>
        public Vector2 BlendSpacePosToBlendPointPos(Vector2 pos)
        {
            GetPointsArea(out var pointsArea);
            if (_is2D)
            {
                pos = new Vector2(
                    Mathf.Map(pos.X, _rangeX.X, _rangeX.Y, pointsArea.Left, pointsArea.Right),
                    Mathf.Map(pos.Y, _rangeY.X, _rangeY.Y, pointsArea.Right, pointsArea.Bottom)
                );
            }
            else
            {
                pos = new Vector2(
                    Mathf.Map(pos.X, _rangeX.X, _rangeX.Y, pointsArea.Left, pointsArea.Right),
                    pointsArea.Center.Y
                );
            }
            return pos - new Vector2(BlendPoint.DefaultSize * 0.5f);
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Synchronize blend points collection
            GetData(out _rangeX, out _rangeY, _pointsAnims, _pointsLocations);
            for (int i = 0; i < Animation.MultiBlend1D.MaxAnimationsCount; i++)
            {
                if (_pointsAnims[i] != Guid.Empty)
                {
                    if (_blendPoints[i] == null)
                    {
                        // Create missing blend point
                        _blendPoints[i] = new BlendPoint(this, i)
                        {
                            Parent = this,
                        };
                    }

                    // Update blend point
                    _blendPoints[i].Location = BlendSpacePosToBlendPointPos(_pointsLocations[i]);
                }
                else
                {
                    if (_blendPoints[i] != null)
                    {
                        // Removed unused blend point
                        _blendPoints[i].Dispose();
                        _blendPoints[i] = null;
                    }
                }
            }

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            var rect = new Rectangle(Vector2.Zero, Size);
            var containsFocus = ContainsFocus;

            // Background
            Render2D.DrawRectangle(rect, IsMouseOver ? style.TextBoxBackgroundSelected : style.TextBoxBackground);

            // Base
            base.Draw();

            // Frame
            Render2D.DrawRectangle(new Rectangle(1, 1, rect.Width - 2, rect.Height - 2), containsFocus ? style.ProgressNormal : style.BackgroundSelected);
        }
    }

    public static partial class Animation
    {
        /// <summary>
        /// Customized <see cref="SurfaceNode" /> for the blending multiple animations in 1D.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class MultiBlend1D : SurfaceNode
        {
            private readonly Label _selectedAnimationLabel;
            private readonly ComboBox _selectedAnimation;
            private readonly AssetPicker _animationPicker;
            private readonly Label _animationSpeedLabel;
            private readonly FloatValueBox _animationSpeed;
            private readonly Label _animationXLabel;
            private readonly FloatValueBox _animationX;
            private readonly Editor _editor;
            private bool _isUpdatingUI;

            /// <summary>
            /// The maximum animations amount to blend per node.
            /// </summary>
            public const int MaxAnimationsCount = 14;

            /// <summary>
            /// Gets or sets the index of the selected animation.
            /// </summary>
            public int SelectedAnimationIndex
            {
                get => _selectedAnimation.SelectedIndex;
                set => _selectedAnimation.SelectedIndex = value;
            }

            /// <summary>
            /// The Multi Blend 1D blend space editor.
            /// </summary>
            /// <seealso cref="FlaxEditor.Surface.Archetypes.BlendPointsEditor" />
            protected class Editor : BlendPointsEditor
            {
                private MultiBlend1D _node;

                /// <summary>
                /// Initializes a new instance of the <see cref="Editor"/> class.
                /// </summary>
                /// <param name="node">The parent Visject Node node.</param>
                /// <param name="x">The X location.</param>
                /// <param name="y">The Y location.</param>
                /// <param name="width">The width.</param>
                /// <param name="height">The height.</param>
                public Editor(MultiBlend1D node, float x, float y, float width, float height)
                : base(false, x, y, width, height)
                {
                    _node = node;
                }

                /// <inheritdoc />
                public override void GetData(out Vector2 rangeX, out Vector2 rangeY, Guid[] pointsAnims, Vector2[] pointsLocations)
                {
                    var data0 = (Vector4)_node.Values[0];
                    rangeX = new Vector2(data0.X, data0.Y);
                    rangeY = Vector2.Zero;
                    for (int i = 0; i < MaxAnimationsCount; i++)
                    {
                        var dataA = (Vector4)_node.Values[4 + i * 2];
                        var dataB = (Guid)_node.Values[5 + i * 2];

                        pointsAnims[i] = dataB;
                        pointsLocations[i] = new Vector2(dataA.X, 0.0f);
                    }
                }

                /// <inheritdoc />
                public override int SelectedIndex
                {
                    get => _node.SelectedAnimationIndex;
                    set => _node.SelectedAnimationIndex = value;
                }

                /// <inheritdoc />
                public override void SetLocation(int index, Vector2 location)
                {
                    var dataA = (Vector4)_node.Values[4 + index * 2];

                    dataA.X = location.X;
                    
                    _node.Values[4 + index * 2] = dataA;

                    _node.UpdateUI();
                }
            }

            /// <inheritdoc />
            public MultiBlend1D(uint id, VisjectSurface surface, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, surface, nodeArch, groupArch)
            {
                var layoutOffsetY = FlaxEditor.Surface.Constants.LayoutOffsetY;

                _selectedAnimationLabel = new Label(300, 3 * layoutOffsetY, 120.0f, layoutOffsetY);
                _selectedAnimationLabel.HorizontalAlignment = TextAlignment.Near;
                _selectedAnimationLabel.Text = "Selected Animation:";
                _selectedAnimationLabel.Parent = this;

                _selectedAnimation = new ComboBox(_selectedAnimationLabel.X, 4 * layoutOffsetY, _selectedAnimationLabel.Width);
                _selectedAnimation.PopupShowing += OnSelectedAnimationPopupShowing;
                _selectedAnimation.SelectedIndexChanged += OnSelectedAnimationChanged;
                _selectedAnimation.Parent = this;

                var items = new List<string>(MaxAnimationsCount);
                while (items.Count < MaxAnimationsCount)
                    items.Add(string.Empty);
                _selectedAnimation.Items = items;

                _animationPicker = new AssetPicker(typeof(FlaxEngine.Animation), new Vector2(_selectedAnimation.Left, _selectedAnimation.Bottom + 4));
                _animationPicker.SelectedItemChanged += OnAnimationPickerItemChanged;
                _animationPicker.Parent = this;

                _animationSpeedLabel = new Label(_animationPicker.Left, _animationPicker.Bottom + 4, 40, TextBox.DefaultHeight);
                _animationSpeedLabel.HorizontalAlignment = TextAlignment.Near;
                _animationSpeedLabel.Text = "Speed:";
                _animationSpeedLabel.Parent = this;

                _animationSpeed = new FloatValueBox(1.0f, _animationSpeedLabel.Right + 4, _animationSpeedLabel.Y, _selectedAnimation.Right - _animationSpeedLabel.Right - 4);
                _animationSpeed.SlideSpeed = 0.01f;
                _animationSpeed.ValueChanged += OnAnimationSpeedValueChanged;
                _animationSpeed.Parent = this;

                _animationXLabel = new Label(_animationSpeedLabel.Left, _animationSpeedLabel.Bottom + 4, 40, TextBox.DefaultHeight);
                _animationXLabel.HorizontalAlignment = TextAlignment.Near;
                _animationXLabel.Text = "X:";
                _animationXLabel.Parent = this;

                _animationX = new FloatValueBox(0.0f, _animationXLabel.Right + 4, _animationXLabel.Y, _selectedAnimation.Right - _animationXLabel.Right - 4);
                _animationX.SlideSpeed = 0.01f;
                _animationX.ValueChanged += OnAnimationXChanged;
                _animationX.Parent = this;

                _editor = new Editor(this,
                                     FlaxEditor.Surface.Constants.NodeMarginX,
                                     _animationX.Bottom + 4.0f,
                                     Width - FlaxEditor.Surface.Constants.NodeMarginX * 2.0f,
                                     120.0f);
                _editor.Parent = this;
            }

            private void OnSelectedAnimationPopupShowing(ComboBox comboBox)
            {
                var items = comboBox.Items;
                items.Clear();
                for (var i = 0; i < MaxAnimationsCount; i++)
                {
                    var animId = (Guid)Values[5 + i * 2];
                    var path = string.Empty;
                    if (FlaxEngine.Content.GetAssetInfo(animId, out _, out path))
                        path = Path.GetFileNameWithoutExtension(path);
                    items.Add(string.Format("[{0}] {1}", i, path));
                }
            }

            private void OnSelectedAnimationChanged(ComboBox comboBox)
            {
                UpdateUI();
            }

            private void OnAnimationPickerItemChanged()
            {
                if (_isUpdatingUI)
                    return;

                var selectedIndex = _selectedAnimation.SelectedIndex;
                if (selectedIndex != -1)
                {
                    var index = 5 + selectedIndex * 2;
                    SetValue(index, _animationPicker.SelectedID);
                }
            }

            private void OnAnimationSpeedValueChanged()
            {
                if (_isUpdatingUI)
                    return;

                var selectedIndex = _selectedAnimation.SelectedIndex;
                if (selectedIndex != -1)
                {
                    var index = 4 + selectedIndex * 2;
                    var data0 = (Vector4)Values[index];
                    data0.W = _animationSpeed.Value;
                    SetValue(index, data0);
                }
            }

            private void OnAnimationXChanged()
            {
                if (_isUpdatingUI)
                    return;

                var selectedIndex = _selectedAnimation.SelectedIndex;
                if (selectedIndex != -1)
                {
                    var index = 4 + selectedIndex * 2;
                    var data0 = (Vector4)Values[index];
                    data0.X = _animationX.Value;
                    SetValue(index, data0);
                }
            }

            private void UpdateUI()
            {
                if (_isUpdatingUI)
                    return;
                _isUpdatingUI = true;

                var selectedIndex = _selectedAnimation.SelectedIndex;
                var isValid = selectedIndex != -1;
                if (isValid)
                {
                    var data0 = (Vector4)Values[4 + selectedIndex * 2];
                    var data1 = (Guid)Values[5 + selectedIndex * 2];

                    _animationPicker.SelectedID = data1;
                    _animationSpeed.Value = data0.W;
                    _animationX.Value = data0.X;

                    var path = string.Empty;
                    if (FlaxEngine.Content.GetAssetInfo(data1, out _, out path))
                        path = Path.GetFileNameWithoutExtension(path);
                    _selectedAnimation.Items[selectedIndex] = string.Format("[{0}] {1}", selectedIndex, path);
                }
                else
                {
                    _animationPicker.SelectedID = Guid.Empty;
                    _animationSpeed.Value = 1.0f;
                    _animationX.Value = 0.0f;
                }
                _animationPicker.Enabled = isValid;
                _animationSpeedLabel.Enabled = isValid;
                _animationSpeed.Enabled = isValid;
                _animationXLabel.Enabled = isValid;
                _animationX.Enabled = isValid;

                _isUpdatingUI = false;
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                UpdateUI();
            }

            /// <inheritdoc />
            public override void SetValue(int index, object value)
            {
                base.SetValue(index, value);

                UpdateUI();
            }
        }
    }
}