﻿////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        /// <summary>
        /// Shows the primary menu.
        /// </summary>
        /// <param name="location">The location in teh Surface Space.</param>
        public void ShowPrimaryMenu(Vector2 location)
        {
            _cmPrimaryMenu.Show(this, location);
        }

        /// <summary>
        /// Shows the secondary context menu for the given node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="location">The location in the Surface Space.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ShowSecondaryCM(SurfaceNode node, Vector2 location)
        {
            throw new NotImplementedException();
        }
    }
}
