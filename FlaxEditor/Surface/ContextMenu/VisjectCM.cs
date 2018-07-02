// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.ContextMenu
{
    /// <summary>
    /// The Visject Surface dedicated context menu for nodes spawning.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContextMenuBase" />
    public sealed class VisjectCM : ContextMenuBase
    {
        private readonly List<VisjectCMGroup> _groups = new List<VisjectCMGroup>(16);
        private readonly TextBox _searchBox;
        private bool _waitingForInput;
        private VisjectCMGroup _surfaceParametersGroup;
        private VerticalPanel _panel2;
        private Func<List<SurfaceParameter>> _parametersGetter;

        /// <summary>
        /// The type of the surface.
        /// </summary>
        public readonly SurfaceType Type;

        /// <summary>
        /// The currently selected group
        /// </summary>
        public VisjectCMGroup SelectedGroup;

        /// <summary>
        /// Event fired when any item in this popup menu gets clicked.
        /// </summary>
        public event Action<VisjectCMItem> OnItemClicked;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisjectCM"/> class.
        /// </summary>
        /// <param name="type">The surface type.</param>
        /// <param name="parametersGetter">The surface parameters getter callback.</param>
        public VisjectCM(SurfaceType type, Func<List<SurfaceParameter>> parametersGetter)
        {
            Type = type;
            _parametersGetter = parametersGetter;

            // Context menu dimensions
            Size = new Vector2(320, 220);

            // Search box
            _searchBox = new TextBox(false, 1, 1)
            {
                Width = Width - 3,
                WatermarkText = "Search...",
                Parent = this
            };
            _searchBox.TextChanged += OnSearchFilterChanged;

            // Create first panel (for scrollbar)
            var panel1 = new Panel(ScrollBars.Vertical)
            {
                Bounds = new Rectangle(0, _searchBox.Bottom + 1, Width, Height - _searchBox.Bottom - 2),
                Parent = this
            };

            // Create second panel (for groups arrangement)
            var panel2 = new VerticalPanel
            {
                DockStyle = DockStyle.Top,
                IsScrollable = true,
                Parent = panel1
            };
            _panel2 = panel2;

            // Init groups
            var groups = NodeFactory.Groups;
            var nodes = new List<NodeArchetype>();
            foreach (var groupArchetype in groups)
            {
                // Get valid nodes
                nodes.Clear();
                foreach (var nodeArchetype in groupArchetype.Archetypes)
                {
                    if ((nodeArchetype.Flags & NodeFlags.NoSpawnViaGUI) != 0)
                        continue;

                    if (type != SurfaceType.Material && (nodeArchetype.Flags & NodeFlags.MaterialOnly) != 0)
                        continue;

                    if (type != SurfaceType.AnimationGraph && (nodeArchetype.Flags & NodeFlags.AnimGraphOnly) != 0)
                        continue;

                    if (type != SurfaceType.Visject && (nodeArchetype.Flags & NodeFlags.VisjectOnly) != 0)
                        continue;

                    nodes.Add(nodeArchetype);
                }

                // Check if can create group for them
                if (nodes.Count > 0)
                {
                    var group = new VisjectCMGroup(this, groupArchetype);
                    group.Close(false);
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        var item = new VisjectCMItem(group, nodes[i]);
                        item.Parent = group;
                    }
                    group.SortChildren();
                    group.Parent = panel2;
                    _groups.Add(group);
                }
            }
        }

        private void OnSearchFilterChanged()
        {
            // Skip events during setup or init stuff
            if (IsLayoutLocked)
                return;

            // Update groups
            for (int i = 0; i < _groups.Count; i++)
                _groups[i].UpdateFilter(_searchBox.Text);
            SelectedGroup = _groups.Find(group => group.Visible);
            PerformLayout();
            _searchBox.Focus();
        }

        /// <summary>
        /// Called when user clicks on an item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnClickItem(VisjectCMItem item)
        {
            Hide();
            OnItemClicked?.Invoke(item);
        }

        /// <summary>
        /// Resets the view.
        /// </summary>
        public void ResetView()
        {
            bool wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            for (int i = 0; i < _groups.Count; i++)
                _groups[i].ResetView();

            _searchBox.Clear();
            SelectedGroup = _groups.Find(group => group.Visible);

            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Updates the surface parameters group.
        /// </summary>
        private void UpdateSurfaceParametersGroup()
        {
            // Remove the old one
            if (_surfaceParametersGroup != null)
            {
                _groups.Remove(_surfaceParametersGroup);
                _surfaceParametersGroup.Dispose();
                _surfaceParametersGroup = null;
            }

            // Check if surface has any parameters
            var parameters = _parametersGetter();
            int count = parameters?.Count(x => x.IsPublic) ?? 0;
            if (count > 0)
            {
                // TODO: cache the allocated memory to reduce dynamic allocations
                var archetypes = new NodeArchetype[count];
                int archetypeIndex = 0;
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (!parameters[i].IsPublic)
                        continue;

                    archetypes[archetypeIndex++] = new NodeArchetype
                    {
                        TypeID = 1,
                        Create = Archetypes.Parameters.CreateGetNode,
                        Title = "Get " + parameters[i].Name,
                        Description = "Parameter value getter",
                        Size = new Vector2(140, 60),
                        DefaultValues = new object[]
                        {
                            parameters[i].ID
                        },
                        Elements = new[]
                        {
                            NodeElementArchetype.Factory.ComboBox(2, 0, 116)
                        }
                    };
                }
                var groupArchetype = new GroupArchetype
                {
                    GroupID = 6,
                    Name = "Surface Parameters",
                    Color = new Color(52, 73, 94),
                    Archetypes = archetypes
                };
                var group = new VisjectCMGroup(this, groupArchetype);
                group.Close(false);
                archetypeIndex = 0;
                for (int i = 0; i < parameters.Count; i++)
                {
                    if (!parameters[i].IsPublic)
                        continue;

                    var item = new VisjectCMItem(group, archetypes[archetypeIndex++]);
                    item.Parent = group;
                }
                group.SortChildren();
                group.UnlockChildrenRecursive();
                group.Parent = _panel2;
                _groups.Add(group);
                _surfaceParametersGroup = group;
            }
        }

        /// <inheritdoc />
        protected override void OnShow()
        {
            // Prepare
            UpdateSurfaceParametersGroup();
            ResetView();
            Focus();
            _waitingForInput = true;

            base.OnShow();
        }

        /// <inheritdoc />
        public override void Hide()
        {
            Focus(null);

            base.Hide();
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            if (key == Keys.Escape)
            {
                Hide();
                return true;
            }
            else if (key == Keys.Return)
            {
                if (SelectedGroup?.SelectedItem != null) OnClickItem(SelectedGroup?.SelectedItem);
                return true;
            }
            else if (key == Keys.ArrowUp || key == Keys.ArrowDown)
            {
                return SelectedGroup.OnKeyDown(key);
            }

            if (_waitingForInput)
            {
                _waitingForInput = false;
                _searchBox.Focus();
                return _searchBox.OnKeyDown(key);
            }

            return base.OnKeyDown(key);
        }
    }
}
