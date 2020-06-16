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
        private CarouselView carouselView;


        public Color IndicatorColor { get; set; }
        public int IndicatorSize { get; set; } = 4;
        public IndicatorShape IndicatorShape { get; set; } = IndicatorShape.Circle;

        public Color SelectedIndicatorColor { get; set; }
        public int SelectedIndicatorSize { get; set; } = 8;


        protected override void OnParentSet()
        {
            base.OnParentSet();


            var element = (Parent as Layout<View>)?.Children
                .FirstOrDefault(child => child.GetType() == typeof(CarouselView));

            if (!(element is CarouselView carousel))
            {
                return;
            }


            carouselView = carousel;

            carouselView.PositionChanged += OnCarouselPositionChanged;
            carouselView.PropertyChanged += OnCarouselPropertyChanged;
        }


        private void CreateIndicators()
        {
            Children.Clear();

            if (carouselView == null)
            {
                return;
            }


            if (carouselView.ItemsSource == null)
            {
                return;
            }


            foreach (var item in carouselView.ItemsSource)
            {
                AddIndicator(item);
            }
        }

        private void AddIndicator(
            object item)
        {
            BoxView indicator = CreateDefaultIndicator();

            if (carouselView.CurrentItem == item ||
                carouselView.Position == Children.Count)
            {
                indicator = CreateSelectedIndicator();
            }


            Children.Add(indicator);
        }

        private BoxView CreateDefaultIndicator()
        {
            var boxView = new BoxView
            {
                BackgroundColor = IndicatorColor,
                HeightRequest = IndicatorSize,
                WidthRequest = IndicatorSize
            };

            if (IndicatorShape == IndicatorShape.Circle)
            {
                boxView.CornerRadius = IndicatorSize / 2;
            }


            return boxView;
        }

        private BoxView CreateSelectedIndicator()
        {
            var boxView = new BoxView
            {
                BackgroundColor = SelectedIndicatorColor,
                HeightRequest = SelectedIndicatorSize,
                WidthRequest = SelectedIndicatorSize
            };

            if (IndicatorShape == IndicatorShape.Circle)
            {
                boxView.CornerRadius = SelectedIndicatorSize / 2;
            }


            return boxView;
        }

        private void SelectIndicator(BoxView indicator)
        {
            if (indicator == null)
            {
                return;
            }


            indicator.BackgroundColor = SelectedIndicatorColor;

            indicator.HeightRequest = SelectedIndicatorSize;
            indicator.WidthRequest = SelectedIndicatorSize;

            if (IndicatorShape == IndicatorShape.Circle)
            {
                indicator.CornerRadius = SelectedIndicatorSize / 2;
            }
        }

        private void DeselectIndicator(BoxView indicator)
        {
            if (indicator == null)
            {
                return;
            }

            indicator.BackgroundColor = IndicatorColor;

            indicator.HeightRequest = IndicatorSize;
            indicator.WidthRequest = IndicatorSize;

            if (IndicatorShape == IndicatorShape.Circle)
            {
                indicator.CornerRadius = IndicatorSize / 2;
            }
        }


        private void OnCarouselPropertyChanged(
            object sender,
            PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName != nameof(CarouselView.ItemsSource))
            {
                return;
            }


            CreateIndicators();
        }


        private void OnCarouselPositionChanged(
            object sender,
            PositionChangedEventArgs eventArgs)
        {
            var previousIndicator = Children.ElementAt(eventArgs.PreviousPosition) as BoxView;
            var currentIndicator = Children.ElementAt(eventArgs.CurrentPosition) as BoxView;

            DeselectIndicator(previousIndicator);
            SelectIndicator(currentIndicator);
        }
    }
}