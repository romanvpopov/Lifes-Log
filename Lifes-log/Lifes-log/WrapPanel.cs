﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Lifes_log
{
    /// <summary>
    /// Represents a control that can be used to present a collection of items in wrapped layout.
    /// </summary>
    public class WrapPanel : Panel
    {
        /// <summary>Dictionary containing all children with their positions.</summary>
        private readonly IDictionary<FrameworkElement, int> elementPositions;

        /// <summary>
        /// The object source used to generate the content of the WrapPanel.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        private static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(WrapPanel), new PropertyMetadata(null, ItemsSource_Changed));

        /// <summary>
        /// The DataTemplate used to display each item.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        private static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(WrapPanel), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the WrapPanel.WrapPanel class.
        /// </summary>
        public WrapPanel()
        {
            elementPositions = new Dictionary<FrameworkElement, int>();
        }

        /// <summary>
        /// Updates the size of all children of this panel.
        /// </summary>
        /// <param name="availableSize">The available size for this panel.</param>
        /// <returns>The computed available size.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            // Take the full available width
            var finalSize = new Size { Width = double.IsInfinity(availableSize.Width) ? int.MaxValue : availableSize.Width };
            var x = 0d;
            var rowHeight = 0d;
            foreach (var child in GetChildrenOrdered())
            {
                // Determine which size the child needs
                child.Measure(availableSize);

                x += child.DesiredSize.Width;
                if (x > availableSize.Width)
                {
                    // A new row, start over with X
                    x = child.DesiredSize.Width;

                    // Adjust the size of the panel
                    finalSize.Height += rowHeight;
                    rowHeight = child.DesiredSize.Height;
                }
                else
                {
                    // Set the height based on the largest item
                    rowHeight = Math.Max(child.DesiredSize.Height, rowHeight);
                }
            }
            if (double.IsInfinity(availableSize.Width))
                finalSize.Width = x;

            // Set the final height
            finalSize.Height += rowHeight;
            return finalSize;
        }

        /// <summary>
        /// Positions and determines the size of the panel's children.
        /// </summary>
        /// <param name="finalSize">The final size computed by the parent for this panel.</param>
        /// <returns>The computed final size of the panel.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var finalRect = new Rect(0, 0, finalSize.Width, finalSize.Height);
            double rowHeight = 0;

            foreach (var child in GetChildrenOrdered())
            {
                if ((child.DesiredSize.Width + finalRect.X) > finalSize.Width)
                {
                    // Move it to the next row
                    finalRect.X = 0;
                    finalRect.Y += rowHeight;
                    rowHeight = 0;
                }
                // Place the item
                child.Arrange(new Rect(finalRect.X, finalRect.Y, child.DesiredSize.Width, child.DesiredSize.Height));

                // Adjust the location for the next items
                finalRect.X += child.DesiredSize.Width;
                rowHeight = Math.Max(child.DesiredSize.Height, rowHeight);
            }
            return finalSize;
        }

        /// <summary>
        /// Handle the ItemsSource changed.
        /// </summary>
        /// <param name="dependencyObject">The instance the ItemsSource changed on.</param>
        /// <param name="eventArgs">Data describing the changes.</param>
        private static void ItemsSource_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            var instance = dependencyObject as WrapPanel;

            if (eventArgs.OldValue != eventArgs.NewValue)
            {
                if (eventArgs.OldValue != null)
                    instance.UnregisterCollectionChanged(eventArgs.OldValue);

                if (eventArgs.NewValue != null)
                    instance.RegisterCollectionChanged(eventArgs.NewValue);
            }
        }

        /// <summary>
        /// If the ItemsSource implements the INotifyCollectionChanged
        /// it will unregister the handler from the CollectionChanged event.
        /// </summary>
        private void UnregisterCollectionChanged(object collection)
        {
            ItemsSource_CollectionChanged(collection, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, ((IEnumerable)collection).Cast<object>().ToList()));

            if (collection is INotifyCollectionChanged incc)
                incc.CollectionChanged -= ItemsSource_CollectionChanged;
        }

        /// <summary>
        /// If the ItemsSource implements the INotifyCollectionChanged 
        /// it will register a handler to the CollectionChanged event.
        /// </summary>
        private void RegisterCollectionChanged(object collection)
        {
            ItemsSource_CollectionChanged(collection, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, ((IEnumerable)collection).Cast<object>().ToList()));

            if (collection is INotifyCollectionChanged incc)
                incc.CollectionChanged += ItemsSource_CollectionChanged;
        }

        /// <summary>
        /// Handles collection changes and reflect those to the panel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments of the colection changed event.</param>
        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    {
                        Children.Clear();

                        var items = ItemsSource.Cast<object>().ToList();
                        foreach (var item in items)
                        {
                            if (Children.All(c => ((FrameworkElement)c).DataContext != item))
                                AddItem(item);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (var item in e.NewItems)
                        {
                            foreach (var uiElement in Children)
                            {
                                var child = (FrameworkElement)uiElement;
                                if (elementPositions.TryGetValue(child, out int position) && position >= e.NewStartingIndex)
                                    elementPositions[child] = ++position;
                            }
                            AddItem(item, e.NewStartingIndex);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var item in e.OldItems)
                        {
                            var childToRemove = Children.FirstOrDefault(c => ((FrameworkElement)c).DataContext == item);
                            if (childToRemove != null)
                                Children.Remove(childToRemove);

                            foreach (var uiElement in Children)
                            {
                                var child = (FrameworkElement)uiElement;
                                if (elementPositions.TryGetValue(child, out int position) && position > e.OldStartingIndex)
                                    elementPositions[child] = --position;
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    {
                        var items = ItemsSource.Cast<object>().ToList();
                        foreach (var uiElement in Children)
                        {
                            var child = (FrameworkElement)uiElement;
                            var matchingItem = items.FirstOrDefault(i => i == child.DataContext);
                            if (matchingItem != null)
                                elementPositions[child] = items.IndexOf(matchingItem);
                        }
                    }
                    break;

            }
            InvalidateArrange();
        }

        /// <summary>
        /// Returns all children of the panel ordered by position.
        /// </summary>
        /// <returns>Ordered children collection.</returns>
        private IEnumerable<FrameworkElement> GetChildrenOrdered()
        {
            return Children.OfType<FrameworkElement>().OrderBy(c =>
            {
                if (elementPositions.TryGetValue(c, out int position))
                    return position;
                else
                    return -1;
            }).ToList();
        }

        /// <summary>
        /// Adds an item to the panel.
        /// </summary>
        /// <param name="dataContext">The datacontext for the item to add.</param>
        /// <param name="position">The position for the item.</param>
        private void AddItem(object dataContext, int position = -1)
        {
            var childToAdd = new ContentControl
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                DataContext = dataContext,
                ContentTemplate = ItemTemplate
            };
            elementPositions[childToAdd] = position == -1 ? Children.Count : position;
            Children.Add(childToAdd);
        }
    }
}
