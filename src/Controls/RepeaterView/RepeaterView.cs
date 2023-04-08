using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Xamarin.Forms;

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
    public class RepeaterView :
        StackLayout
    {
        public event RepeaterViewItemAddedEventHandler ItemCreated;


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(RepeaterView),
                new List<object>(),
                BindingMode.OneWay,
                propertyChanged: ItemsChanged);

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }



        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(RepeaterView),
                default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }



        public static readonly BindableProperty SeparatorTemplateProperty =
            BindableProperty.Create(
                nameof(SeparatorTemplate),
                typeof(DataTemplate),
                typeof(RepeaterView),
                default(DataTemplate));

        public DataTemplate SeparatorTemplate
        {
            get => (DataTemplate)GetValue(SeparatorTemplateProperty);
            set => SetValue(SeparatorTemplateProperty, value);
        }



        public static readonly BindableProperty HeaderProperty =
            BindableProperty.Create(
                nameof(Header),
                typeof(View),
                typeof(RepeaterView),
                default(DataTemplate));

        public View Header
        {
            get => (View)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }



        public static readonly BindableProperty FooterProperty =
            BindableProperty.Create(
                nameof(Footer),
                typeof(View),
                typeof(RepeaterView),
                default(DataTemplate));

        public View Footer
        {
            get => (View)GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }



        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Header != null)
                Header.BindingContext = BindingContext;

            if (Footer != null)
                Footer.BindingContext = BindingContext;
        }



        private static void ItemsChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (!(bindable is RepeaterView control))
            {
                return;
            }

            if (oldValue is INotifyCollectionChanged oldObservableCollection)
            {
                oldObservableCollection.CollectionChanged -= control.OnItemsSourceCollectionChanged;
            }

            if (newValue is INotifyCollectionChanged newObservableCollection)
            {
                newObservableCollection.CollectionChanged += control.OnItemsSourceCollectionChanged;
            }

            if (!(newValue is IEnumerable newItems))
            {
                return;
            }


            control.Children.Clear();

            control.CreateHeader();


            if (newValue != null)
            {
                control.CreateViews(
                    newItems);
            }


            control.CreateFooter();

            control.UpdateChildrenLayout();
            control.InvalidateLayout();
        }


        private void CreateHeader()
        {
            if (Header == null)
                return;


            Children.Add(
                Header);

            OnItemCreated(
                Header);
        }

        private void CreateFooter()
        {
            if (Footer == null)
                return;


            Children.Add(
                Footer);

            OnItemCreated(
                Footer);
        }

        protected virtual void OnItemCreated(
            View view)
        {
            ItemCreated?.Invoke(
                this,
                new RepeaterViewItemAddedEventArgs(
                    view,
                    view.BindingContext));
        }


        private void OnItemsSourceCollectionChanged(
            object sender,
            NotifyCollectionChangedEventArgs eventArgs)
        {
            var invalidate = false;

            if (eventArgs.OldItems != null)
            {
                int elementToRemoveIndex = eventArgs.OldStartingIndex;


                if (Header != null)
                    elementToRemoveIndex += 1;


                Children.RemoveAt(
                    elementToRemoveIndex);

                invalidate = true;
            }

            if (eventArgs.NewItems != null)
            {
                CreateViews(
                    eventArgs.NewItems);


                invalidate = true;
            }


            if (invalidate)
            {
                UpdateChildrenLayout();
                InvalidateLayout();
            }
        }

        private void CreateViews(
            IEnumerable newItems)
        {
            for (int index = 0; index < newItems.Count(); index++)
            {
                var item = newItems.ElementAt(index);

                var view = CreateChildViewFor(
                    item);


                Children.Add(view);


                if (index < newItems.Count() - 1)
                {
                    var separatorView = CreateSeparatorView();

                    Children.Add(separatorView);
                }

                OnItemCreated(
                    view);
            }
        }

        private View CreateChildViewFor(
            object item)
        {
            ItemTemplate.SetValue(
                BindableObject.BindingContextProperty,
                item);

            return (View)ItemTemplate.CreateContent();
        }

        private View CreateSeparatorView()
        {
            return (View)SeparatorTemplate.CreateContent();
        }
    }


    public delegate void RepeaterViewItemAddedEventHandler(
        object sender,
        RepeaterViewItemAddedEventArgs args);


    public class RepeaterViewItemAddedEventArgs :
        EventArgs
    {
        public View View { get; }

        public object Model { get; }


        public RepeaterViewItemAddedEventArgs(
            View view,
            object model)
        {
            View = view;
            Model = model;
        }
    }
}