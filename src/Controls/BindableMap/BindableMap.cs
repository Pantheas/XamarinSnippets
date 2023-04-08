using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XamarinSnippets
{
    /// <summary>
    /// Copyright by Jan Morfeld
    /// https://github.com/Pantheas/XamarinSnippets
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    /// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    /// </summary>
    public class BindableMap :
        Map
    {
        public static readonly BindableProperty PinLabelProperty =
            BindableProperty.Create(
                nameof(PinLabel),
                typeof(string),
                typeof(BindableMap));

        public string PinLabel
        {
            get => (string)GetValue(PinLabelProperty);
            set => SetValue(PinLabelProperty, value);
        }


        public static readonly BindableProperty PinPositionProperty =
            BindableProperty.Create(
                nameof(PinPosition),
                typeof(Position),
                typeof(BindableMap),
                propertyChanged: OnPinPositionChanged);

        public Position PinPosition
        {
            get => (Position)GetValue(PinPositionProperty);
            set => SetValue(PinPositionProperty, value);
        }


        private static void OnPinPositionChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (!(bindable is BindableMap map))
            {
                return;
            }

            if (!(newValue is Position position))
            {
                return;
            }
            

            map.Pins.Clear();

            if (position == null ||
                (EqualityComparer<double>.Default.Equals(
                    position.Latitude,
                    default(double))) &&
                EqualityComparer<double>.Default.Equals(
                    position.Longitude,
                    default(double)))
            {
                return;
            }


            var pin = new Pin
            {
                Label = map.PinLabel ?? string.Empty,
                Position = position
            };

            map.Pins.Add(
                pin);

            map.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromMeters(500)));
        }
    }
}