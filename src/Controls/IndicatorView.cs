using DID.Infrastructure.Extensions;

using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
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
    public class IndicatorView :
        StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(IndicatorView),
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
                typeof(IndicatorView),
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: OnSelectedItemChanged);

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }



        public static readonly BindableProperty IndicatorColorProperty =
            BindableProperty.Create(
                nameof(IndicatorColor),
                typeof(Color),
                typeof(IndicatorView),
                defaultValue: Color.Default);

        public Color IndicatorColor
        {
            get => (Color)GetValue(IndicatorColorProperty);
            set => SetValue(IndicatorColorProperty, value);
        }


        public static readonly BindableProperty IndicatorWidthProperty =
            BindableProperty.Create(
                nameof(IndicatorWidth),
                typeof(double),
                typeof(IndicatorView),
                defaultValue: 4d);

        public double IndicatorWidth
        {
            get => (double)GetValue(IndicatorWidthProperty);
            set => SetValue(IndicatorWidthProperty, value);
        }


        public static readonly BindableProperty IndicatorHeightProperty =
            BindableProperty.Create(
                nameof(IndicatorHeight),
                typeof(double),
                typeof(IndicatorView),
                defaultValue: 4d);

        public double IndicatorHeight
        {
            get => (double)GetValue(IndicatorHeightProperty);
            set => SetValue(IndicatorHeightProperty, value);
        }



        public static readonly BindableProperty SelectedIndicatorWidthProperty =
            BindableProperty.Create(
                nameof(SelectedIndicatorWidth),
                typeof(double),
                typeof(IndicatorView),
                defaultValue: 8d);

        public double SelectedIndicatorWidth
        {
            get => (double)GetValue(SelectedIndicatorWidthProperty);
            set => SetValue(SelectedIndicatorWidthProperty, value);
        }


        public static readonly BindableProperty SelectedIndicatorHeightProperty =
            BindableProperty.Create(
                nameof(SelectedIndicatorHeight),
                typeof(double),
                typeof(IndicatorView),
                defaultValue: 8d);

        public double SelectedIndicatorHeight
        {
            get => (double)GetValue(SelectedIndicatorHeightProperty);
            set => SetValue(SelectedIndicatorHeightProperty, value);
        }


        public static readonly BindableProperty SelectedIndicatorColorProperty =
            BindableProperty.Create(
                nameof(SelectedIndicatorColor),
                typeof(Color),
                typeof(IndicatorView),
                defaultValue: Color.Default);

        public Color SelectedIndicatorColor
        {
            get => (Color)GetValue(SelectedIndicatorColorProperty);
            set => SetValue(SelectedIndicatorColorProperty, value);
        }



        public static readonly BindableProperty IndicatorShapeProperty =
            BindableProperty.Create(
                nameof(IndicatorShape),
                typeof(IndicatorShape),
                typeof(IndicatorView),
                defaultValue: IndicatorShape.Circle);

        public IndicatorShape IndicatorShape
        {
            get => (IndicatorShape)GetValue(IndicatorShapeProperty);
            set => SetValue(IndicatorShapeProperty, value);
        }




        private void CreateIndicators()
        {
            Children.Clear();

            if (ItemsSource == null ||
                !ItemsSource.Any())
            {
                return;
            }


            foreach (var item in ItemsSource)
            {
                AddIndicator(item);
            }
        }

        private void AddIndicator(
            object item)
        {
            BoxView indicator;


            if (SelectedItem == item)
            {
                indicator = CreateSelectedIndicator();
            }
            else
            {
                indicator = CreateDefaultIndicator();
            }


            Children.Add(indicator);
        }

        private BoxView CreateDefaultIndicator()
        {
            var boxView = CreateIndicator();
            boxView.Color = IndicatorColor;

            boxView.HeightRequest = IndicatorHeight;
            boxView.WidthRequest =
                IndicatorShape == IndicatorShape.Circle ?
                    IndicatorHeight :
                    IndicatorWidth;

            if (IndicatorShape == IndicatorShape.Circle)
            {
                boxView.CornerRadius = IndicatorHeight / 2;
            }


            return boxView;
        }

        private BoxView CreateSelectedIndicator()
        {
            var boxView = CreateIndicator();
            boxView.Color = SelectedIndicatorColor;

            boxView.HeightRequest = SelectedIndicatorHeight;
            boxView.WidthRequest =
                IndicatorShape == IndicatorShape.Circle ?
                    SelectedIndicatorHeight :
                    SelectedIndicatorWidth;

            if (IndicatorShape == IndicatorShape.Circle)
            {
                boxView.CornerRadius = SelectedIndicatorHeight / 2;
            }


            return boxView;
        }

        private BoxView CreateIndicator()
        {
            return new BoxView
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
        }

        private void SelectIndicator(
            BoxView indicator)
        {
            if (indicator == null)
            {
                return;
            }


            indicator.HeightRequest = SelectedIndicatorHeight;
            indicator.WidthRequest =
                IndicatorShape == IndicatorShape.Circle ?
                    SelectedIndicatorHeight :
                    SelectedIndicatorWidth;

            if (IndicatorShape == IndicatorShape.Circle)
            {
                indicator.CornerRadius = SelectedIndicatorHeight / 2;
            }


            indicator.Color = SelectedIndicatorColor;
        }


        private void DeselectIndicator(
            BoxView indicator)
        {
            if (indicator == null)
            {
                return;
            }


            indicator.HeightRequest = IndicatorHeight;
            indicator.WidthRequest =
                IndicatorShape == IndicatorShape.Circle ?
                    IndicatorHeight :
                    IndicatorWidth;


            if (IndicatorShape == IndicatorShape.Circle)
            {
                indicator.CornerRadius = IndicatorHeight / 2;
            }


            indicator.Color = IndicatorColor;
            //indicator.BackgroundColor = IndicatorColor;
        }


        private void OnItemsSourceCollectionChanged(
            object sender,
            NotifyCollectionChangedEventArgs eventArgs)
        {
            CreateIndicators();
        }

        private void HandleSelectedItemChanged(
            object previousSelected,
            object selectedItem)
        {
            if (ItemsSource == null)
            {
                return;
            }

            if (previousSelected != null &&
                ItemsSource.Contains(previousSelected))
            {
                int previousIndex = ItemsSource.IndexOf(
                    previousSelected);

                var previousIndicator = Children.ElementAt(
                    previousIndex) as BoxView;

                DeselectIndicator(previousIndicator);
            }

            if (selectedItem != null &&
                ItemsSource.Contains(selectedItem))
            {
                int selectedIndex = ItemsSource.IndexOf(
                    selectedItem);

                var selectedIndicator = Children.ElementAt(
                    selectedIndex) as BoxView;

                SelectIndicator(selectedIndicator);
            }


            InvalidateLayout();
        }



        private static void OnItemsSourceChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (!(bindable is IndicatorView indicatorView))
            {
                return;
            }

            if (oldValue is INotifyCollectionChanged oldObservableCollection)
            {
                oldObservableCollection.CollectionChanged -= indicatorView.OnItemsSourceCollectionChanged;
            }

            if (newValue is INotifyCollectionChanged observableCollection)
            {
                observableCollection.CollectionChanged += indicatorView.OnItemsSourceCollectionChanged;
            }


            indicatorView.CreateIndicators();
        }

        private static void OnSelectedItemChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (!(bindable is IndicatorView indicatorView))
            {
                return;
            }


            indicatorView.HandleSelectedItemChanged(
                oldValue,
                newValue);
        }
    }
}