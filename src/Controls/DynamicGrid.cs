using System.Collections;

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
    public class DynamicGrid :
        Grid
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(DynamicGrid),
                propertyChanged: OnItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }



        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(DynamicGrid));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }



        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create(
                nameof(Orientation),
                typeof(StackOrientation),
                typeof(DynamicGrid),
                defaultValue: StackOrientation.Vertical);

        public StackOrientation Orientation
        {
            get => (StackOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }



        public static readonly BindableProperty SpanProperty =
            BindableProperty.Create(
                nameof(Span),
                typeof(int),
                typeof(DynamicGrid));

        public int Span
        {
            get => (int)GetValue(SpanProperty);
            set => SetValue(SpanProperty, value);
        }


        public static readonly BindableProperty RowHeightProperty =
            BindableProperty.Create(
                nameof(RowHeight),
                typeof(GridLength),
                typeof(GridLength),
                defaultValue: new GridLength(1, GridUnitType.Star));

        public GridLength RowHeight
        {
            get => (GridLength)GetValue(RowHeightProperty);
            set => SetValue(RowHeightProperty, value);
        }



        public static readonly BindableProperty ColumnWidthProperty =
            BindableProperty.Create(
                nameof(ColumnWidth),
                typeof(GridLength),
                typeof(GridLength),
                defaultValue: new GridLength(1, GridUnitType.Star));

        public GridLength ColumnWidth
        {
            get => (GridLength)GetValue(ColumnWidthProperty);
            set => SetValue(ColumnWidthProperty, value);
        }


        protected virtual void CreateLayout()
        {
            Children.Clear();
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();


            switch (Orientation)
            {
                case StackOrientation.Horizontal:
                    CreateHorizontalLayout();
                    break;

                default:
                    CreateVerticalLayout();
                    break;
            }
        }

        protected virtual void CreateVerticalLayout()
        {
            CreateColumns();

            for (int index = 0; index < ItemsSource.Count(); index++)
            {
                int row = index / Span;
                int column = index % Span;

                if (column == 0)
                {
                    AddRowDefinition();
                }


                // ItemsSource.ElementAt(index);
                var item = ItemsSource[index];

                var view = CreateItemView(
                    item);


                SetRow(view, row);
                SetColumn(view, column);

                Children.Add(view);
            }
        }

        protected virtual void CreateHorizontalLayout()
        {
            CreateRows();

            for (int index = 0; index < ItemsSource.Count(); index++)
            {
                int row = index % Span;
                int column = index / Span;

                if (row == 0)
                {
                    AddColumnDefinition();
                }


                // ItemsSource.ElementAt(index);
                var item = ItemsSource[index];

                var view = CreateItemView(
                    item);


                PlaceItem(
                    view,
                    row,
                    column);
            }
        }

        protected void PlaceItem(
            View view,
            int row,
            int column)
        {
            SetRow(view, row);
            SetColumn(view, column);

            Children.Add(view);
        }


        protected void CreateColumns()
        {
            for (int count = 0; count < Span; count++)
            {
                AddColumnDefinition();
            }
        }

        protected void CreateRows()
        {
            for (int count = 0; count < Span; count++)
            {
                AddRowDefinition();
            }
        }

        protected void AddColumnDefinition()
        {
            var columnDefinition = new ColumnDefinition
            {
                Width = ColumnWidth
            };

            ColumnDefinitions.Add(
                columnDefinition);
        }

        protected void AddRowDefinition()
        {
            var rowDefinition = new RowDefinition
            {
                Height = RowHeight
            };

            RowDefinitions.Add(
                rowDefinition);
        }


        protected virtual View CreateItemView(
            object item)
        {
            DataTemplate template = ItemTemplate;

            if (ItemTemplate is DataTemplateSelector selector)
            {
                template = selector.SelectTemplate(item, this);
            }

            template.SetValue(
                BindableObject.BindingContextProperty,
                item);


            return (View)template.CreateContent();
        }


        private static void OnItemsSourceChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (!(bindable is DynamicGrid grid))
            {
                return;
            }


            grid.CreateLayout();
        }
    }
}