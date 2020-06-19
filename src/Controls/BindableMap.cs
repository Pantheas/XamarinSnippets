using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XamarinSnippets
{
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