using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

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
    public class Link :
        Label
    {
        private TapGestureRecognizer gestureRecognizer;


        public static BindableProperty CommandProperty =
            BindableProperty.Create(
                nameof(Command),
                typeof(ICommand),
                typeof(Link));

        public static BindableProperty CommandParameterProperty =
            BindableProperty.Create(
                nameof(CommandParameter),
                typeof(object),
                typeof(Link));

        public static BindableProperty RequiredTapsAmountProperty =
            BindableProperty.Create(
                nameof(RequiredTapsAmount),
                typeof(int),
                typeof(Link),
                defaultValue: 1);


        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }


        public int RequiredTapsAmount
        {
            get => (int)GetValue(RequiredTapsAmountProperty);
            set => SetValue(RequiredTapsAmountProperty, value);
        }


        public IList<Effect> TapEffects { get; set; } = new List<Effect>();


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Command == null)
            {
                return;
            }


            gestureRecognizer = new TapGestureRecognizer
            {
                Command = Command,
                CommandParameter = CommandParameter,
                NumberOfTapsRequired = RequiredTapsAmount
            };

            if (TapEffects.Any())
            {
                foreach (var effect in TapEffects)
                {
                    gestureRecognizer.Effects.Add(effect);
                }
            }

            GestureRecognizers.Clear();
            GestureRecognizers.Add(gestureRecognizer);
        }
    }
}