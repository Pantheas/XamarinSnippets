using System;
using System.Collections;

using Xamarin.Forms;

// uses IEnumerableExtensions

namespace XamarinSnippets.Controls
{
    /// <summary>
    /// Copyright by Jan Morfeld
    /// https://github.com/Pantheas/XamarinSnippets
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    /// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    /// </summary>
    public class JumpMarkView :
        AbsoluteLayout
    {
        private static readonly object _syncLock = new object();

        private int selectedItemIndex = -1;


        public static readonly BindableProperty ItemsSourceProperty =
               BindableProperty.Create(
                   nameof(ItemsSource),
                   typeof(IEnumerable),
                   typeof(JumpMarkView),
                   propertyChanged: OnItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }



        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(
                nameof(SelectedItem),
                typeof(object),
                typeof(JumpMarkView),
                propertyChanged: OnSelectedItemChanged);

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }



        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(JumpMarkView));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }


        public static readonly BindableProperty StripeColorProperty =
            BindableProperty.Create(
                nameof(StripeColor),
                typeof(Color),
                typeof(JumpMarkView),
                defaultValue: Color.Black,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if (!(bindable is JumpMarkView control))
                    {
                        return;
                    }


                    control.ResetStripeView();
                });

        public Color StripeColor
        {
            get => (Color)GetValue(StripeColorProperty);
            set => SetValue(StripeColorProperty, value);
        }


        public static readonly BindableProperty StripeHeightProperty =
            BindableProperty.Create(
                nameof(StripeHeight),
                typeof(double),
                typeof(JumpMarkView),
                3.0,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if (!(bindable is JumpMarkView control))
                    {
                        return;
                    }


                    control.ResetStripeView();
                });

        public double StripeHeight
        {
            get => (double)GetValue(StripeHeightProperty);
            set => SetValue(StripeHeightProperty, value);
        }


        public static readonly BindableProperty ScrollToPositionProperty =
            BindableProperty.Create(
                nameof(ScrollToPosition),
                typeof(ScrollToPosition),
                typeof(JumpMarkView),
                defaultValue: ScrollToPosition.Start);

        public ScrollToPosition ScrollToPosition
        {
            get => (ScrollToPosition)GetValue(ScrollToPositionProperty);
            set => SetValue(ScrollToPositionProperty, value);
        }


        public static readonly BindableProperty SeparatorProperty =
            BindableProperty.Create(
                nameof(Separator),
                typeof(View),
                typeof(JumpMarkView),
                propertyChanged: OnSeparatorPropertyChanged);

        public View Separator
        {
            get => (View)GetValue(SeparatorProperty);
            set => SetValue(SeparatorProperty, value);
        }



        public static readonly BindableProperty ItemsViewProperty =
            BindableProperty.Create(
                nameof(ItemsView),
                typeof(ItemsView),
                typeof(JumpMarkView),
                propertyChanged: OnItemsViewChanged);

        public ItemsView ItemsView
        {
            get => (ItemsView)GetValue(ItemsViewProperty);
            set => SetValue(ItemsViewProperty, value);
        }


        public static readonly BindableProperty AnimateScrollProperty =
            BindableProperty.Create(
                nameof(AnimateScroll),
                typeof(bool),
                typeof(JumpMarkView),
                defaultValue: false);

        public bool AnimateScroll
        {
            get => (bool)GetValue(AnimateScrollProperty);
            set => SetValue(AnimateScrollProperty, value);
        }




        public double Spacing { get; set; }
        public Thickness FirstJumpMarkMargin { get; set; }
        public Thickness LastJumpMarkMargin { get; set; }



        private StackLayout ItemsStackLayout { get; } = new StackLayout
        {
            Orientation = StackOrientation.Horizontal
        };

        private BoxView StripeView { get; set; } = new BoxView();



        private bool isScrolling;
        private bool IsScrolling
        {
            get
            {
                lock (_syncLock)
                {
                    return isScrolling;
                }
            }
            set
            {
                lock (_syncLock)
                {
                    isScrolling = value;
                }
            }
        }



        public JumpMarkView()
        {
            Children.Add(ItemsStackLayout, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Children.Add(StripeView, new Rectangle(0, 1, 0, 0), AbsoluteLayoutFlags.YProportional);

            SetLayoutBounds(this, new Rectangle(.5, 1, -1, -1));
            SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);
        }

        protected override void OnSizeAllocated(
            double width,
            double height)
        {
            base.OnSizeAllocated(width, height);


            if (SelectedItem != null)
            {
                BringSelectedItemIntoView(-1);
            }
        }


        private void SetupLayout()
        {
            if (Parent == null)
            {
                return;
            }

            try
            {
                BatchBegin();

                if (ItemsSource == null)
                {
                    return;
                }


                CreateJumpMarks();

                ResetStripeViewNonBatch();
            }
            finally
            {
                BatchCommit();
            }
        }


        private void CreateJumpMarks()
        {
            if (ItemsSource?.Any() != true ||
                ItemTemplate == null)
            {
                return;
            }


            ItemsStackLayout.Spacing = Spacing;
            ItemsStackLayout.Children.Clear();

            foreach (var item in ItemsSource)
            {
                var view = CreateJumpMarkView(
                    item);


                ItemsStackLayout.Children.Add(
                    view);
            }


            if (ItemsStackLayout.Children.Count > 0)
            {
                var firstView = ItemsStackLayout.Children[0];
                firstView.Margin = FirstJumpMarkMargin;
            }

            if (ItemsStackLayout.Children.Count > 1)
            {
                int index = ItemsStackLayout.Children.Count - 1;
                var lastView = ItemsStackLayout.Children[index];
                lastView.Margin = LastJumpMarkMargin;
            }
        }

        private View CreateJumpMarkView(
            object item)
        {
            var template = ItemTemplate;

            if (template is DataTemplateSelector templateSelector)
            {
                template = templateSelector.SelectTemplate(
                    item,
                    this);
            }

            template.SetValue(
                BindableObject.BindingContextProperty,
                item);

            var view = (View)template.CreateContent();

            var tapGestureRecognizer = new TapGestureRecognizer
            {
                CommandParameter = item,
                Command = new Command(parameter =>
                {
                    if (ItemsSource?.Any() != true ||
                        ItemsSource.IndexOf(parameter) == -1)
                    {
                        return;
                    }


                    SelectedItem = parameter;
                })
            };

            view.GestureRecognizers.Add(
                tapGestureRecognizer);


            return view;
        }


        private void ResetStripeView()
        {
            try
            {
                BatchBegin();
                ResetStripeViewNonBatch();
            }
            finally
            {
                BatchCommit();
            }
        }

        private void UpdateStripePosition(
            int oldIndex)
        {
            try
            {
                BatchBegin();

                UpdateStripePositionNonBatch(
                    oldIndex);
            }
            finally
            {
                BatchCommit();
            }
        }

        private void ResetStripeViewNonBatch()
        {
            ItemsStackLayout.Margin = new Thickness(0, 0, 0, StripeHeight);
            StripeView.Color = StripeColor;
        }


        private void UpdateStripePositionNonBatch(
            int oldIndex)
        {
            View oldItemView = null;
            View selectedItemView = null;

            if (oldIndex >= 0 &&
                oldIndex < ItemsSource.Count())
            {
                oldItemView = ItemsStackLayout.Children[oldIndex];
            }

            if (selectedItemIndex >= 0 &&
                selectedItemIndex < ItemsSource.Count())
            {
                selectedItemView = ItemsStackLayout.Children[selectedItemIndex];
            }


            MoveStripe(
                oldItemView,
                selectedItemView,
                selectedItemIndex - oldIndex);
        }

        private void MoveStripe(
            View oldView,
            View newView,
            int direction)
        {
            if (oldView != null)
            {
                MoveOutStripeView(
                    oldView,
                    direction);
            }

            if (newView != null)
            {
                MoveInStripeView(
                    newView,
                    direction);
            }
        }


        private void BringSelectedItemIntoView(
            int oldIndex)
        {
            if (!(Parent is Xamarin.Forms.ScrollView scrollView))
            {
                return;
            }

            try
            {
                BatchBegin();



                UpdateStripePosition(
                    oldIndex);

                if (selectedItemIndex < 0 ||
                    selectedItemIndex >= ItemsSource.Count())
                {
                    return;
                }


                scrollView.ScrollToAsync(
                    ItemsStackLayout.Children[selectedItemIndex],
                    position: ScrollToPosition,
                    animated: AnimateScroll);


                if (IsScrolling)
                {
                    return;
                }


                IsScrolling = true;
                if (SelectedItem is IEnumerable group &&
                    group.Any())
                {
                    ItemsView.ScrollTo(
                        group.ElementAt(0),
                        group,
                        position: ScrollToPosition.Start,
                        animate: AnimateScroll);
                }
                else
                {
                    ItemsView.ScrollTo(
                        SelectedItem,
                        position: ScrollToPosition.Start,
                        animate: AnimateScroll);
                }
            }
            finally
            {
                BatchCommit();
                IsScrolling = false;
            }
        }

        private void OnItemsViewScrolled(
            object sender,
            ItemsViewScrolledEventArgs eventArgs)
        {
            if (IsScrolling)
            {
                return;
            }


            try
            {
                if (!(sender is ItemsView itemsView) ||
                    itemsView != ItemsView)
                {
                    return;
                }


                IsScrolling = true;
                int index = eventArgs.FirstVisibleItemIndex;

                if (sender is Xamarin.Forms.CollectionView collectionView &&
                    collectionView.IsGrouped)
                {
                    int groupIndex = GetGroupIndexFromItemIndex(
                        index,
                        collectionView);

                    if (groupIndex == -1)
                    {
                        return;
                    }


                    index = groupIndex;
                }


                if (ItemsSource == null ||
                    index < 0 ||
                    index >= ItemsSource.Count() ||
                    index == selectedItemIndex)
                {
                    return;
                }


                var item = ItemsSource.ElementAt(
                    index);

                if (SelectedItem == item)
                {
                    return;
                }


                SelectedItem = item;
            }
            finally
            {
                IsScrolling = false;
            }
        }

        private int GetGroupIndexFromItemIndex(
            int index,
            Xamarin.Forms.CollectionView collectionView)
        {
            int itemCount = 0;

            foreach (var group in ItemsSource.OfType<IEnumerable>())
            {
                itemCount += group.Count();

                if (collectionView.GroupHeaderTemplate != null)
                {
                    index--;
                }

                if (collectionView.GroupFooterTemplate != null)
                {
                    index--;
                }

                if (itemCount <= index)
                {
                    continue;
                }


                return ItemsSource.IndexOf(
                    group);
            }


            return -1;
        }


        private void MoveOutStripeView(
            View view,
            int direction)
        {
            if (direction > 0)
            {
                MoveOutStripeToRightSide(
                    view);
            }
            else
            {
                MoveOutStripeToLeftSide(
                    view);
            }
        }

        private void MoveInStripeView(
            View view,
            int direction)
        {
            if (direction > 0)
            {
                MoveInStripeFromLeftSide(
                    view);
            }
            else
            {
                MoveInStripeFromRightSide(
                    view);
            }
        }


        private void MoveOutStripeToRightSide(
            View view)
        {
            double destination = view.X + view.Width;

            var animation = new Animation(
                value =>
                {
                    var bounds = new Rectangle(
                        value,
                        1,
                        destination - value,
                        StripeHeight);

                    SetLayoutBounds(
                        StripeView,
                        bounds);
                },
                view.X,
                destination);


            StripeView.Animate(
                view.Id.ToString(),
                animation);
        }

        private void MoveOutStripeToLeftSide(
            View view)
        {
            var animation = new Animation(
                value =>
                {
                    var bounds = new Rectangle(
                        view.X,
                        1,
                        value,
                        StripeHeight);

                    SetLayoutBounds(
                        StripeView,
                        bounds);
                },
                view.Width,
                0);


            StripeView.Animate(
                view.Id.ToString(),
                animation);
        }

        private void MoveInStripeFromRightSide(
            View view)
        {
            var animation = new Animation(
                value =>
                {
                    var bounds = new Rectangle(
                        value,
                        1,
                        view.Width,
                        StripeHeight);

                    SetLayoutBounds(
                        StripeView,
                        bounds);
                },
                view.X + view.Width,
                view.X);


            StripeView.Animate(
                view.Id.ToString(),
                animation);
        }

        private void MoveInStripeFromLeftSide(
            View view)
        {
            var animation = new Animation(
                value =>
                {
                    var bounds = new Rectangle(
                        view.X,
                        1,
                        value,
                        StripeHeight);

                    SetLayoutBounds(
                        StripeView,
                        bounds);
                },
                0,
                view.Width);


            StripeView.Animate(
                view.Id.ToString(),
                animation);
        }



        private static void OnItemsSourceChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (!(bindable is JumpMarkView control) ||
                !(newValue is IEnumerable))
            {
                return;
            }


            control.SetupLayout();
        }


        private static void OnSelectedItemChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (!(bindable is JumpMarkView control) ||
                !(control.Parent is Xamarin.Forms.ScrollView))
            {
                return;
            }


            int oldIndex = control.ItemsSource.IndexOf(
                oldValue);

            control.selectedItemIndex = control.ItemsSource.IndexOf(
                newValue);

            control.BringSelectedItemIntoView(
                oldIndex);
        }


        private static void OnSeparatorPropertyChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (!(bindable is JumpMarkView control))
            {
                return;
            }

            if (oldValue is View oldSeparator)
            {
                if (control.Children.Contains(oldSeparator))
                {
                    control.Children.Remove(oldSeparator);
                }
            }

            if (newValue is View separator)
            {
                control.Children.Add(
                    separator,
                    new Rectangle(0, 1, 1, 1),
                    AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
            }
        }


        private static void OnItemsViewChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (!(bindable is JumpMarkView control))
            {
                return;
            }

            if (oldValue is ItemsView oldItemsView)
            {
                oldItemsView.Scrolled -= control.OnItemsViewScrolled;
            }

            if (newValue is ItemsView itemsView)
            {
                itemsView.Scrolled += control.OnItemsViewScrolled;
            }
        }
    }
}