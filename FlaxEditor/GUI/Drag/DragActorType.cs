// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.SceneGraph;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    public sealed class DragActorType : DragActorType<DragEventArgs>
    {
        public DragActorType(Func<Type, bool> validateFunction) : base(validateFunction)
        {
        }
    }
    /// <summary>
    /// Helper class for handling actor type drag and drop (for spawning).
    /// </summary>
    /// <seealso cref="Actor" />
    /// <seealso cref="ActorNode" />
    public class DragActorType<U> : DragHelper<Type, U> where U : DragEventArgs
    {
        /// <summary>
        /// The default prefix for drag data used for actor type drag and drop.
        /// </summary>
        public const string DragPrefix = "ATYPE!?";

        /// <summary>
        /// Creates a new DragHelper
        /// </summary>
        /// <param name="validateFunction">The validation function</param>
        public DragActorType(Func<Type, bool> validateFunction) : base(validateFunction)
        {
        }

        /// <inheritdoc/>
        public override DragData ToDragData(Type item) => GetDragData(item);

        /// <inheritdoc/>
        public override DragData ToDragData(IEnumerable<Type> items)
        {
            throw new NotImplementedException();
        }

        public static DragData GetDragData(Type item)
        {
            if (item == null)
                throw new ArgumentNullException();

            return new DragDataText(DragPrefix + item.FullName);
        }

        /// <summary>
        /// Tries to parse the drag data to extract <see cref="Type"/> collection.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Gathered objects or empty array if cannot get any valid.</returns>
        public override IEnumerable<Type> FromDragData(DragData data)
        {
            if (data is DragDataText dataText)
            {
                if (dataText.Text.StartsWith(DragPrefix))
                {
                    // Remove prefix and parse splitted names
                    var types = dataText.Text.Remove(0, DragPrefix.Length).Split('\n');
                    var results = new List<Type>(types.Length);
                    var assembly = Utils.GetAssemblyByName("FlaxEngine");
                    if (assembly != null)
                    {
                        for (int i = 0; i < types.Length; i++)
                        {
                            // Find type
                            var obj = assembly.GetType(types[i]);
                            if (obj != null)
                                results.Add(obj);
                        }

                        return results.ToArray();
                    }
                    else
                    {
                        Editor.LogWarning("Failed to get FlaxEngine assembly to spawn actor type");
                    }
                }
            }
            return new Type[0];
        }

        public override void DragDrop(U dragEventArgs, IEnumerable<Type> item)
        {

        }
    }
}
