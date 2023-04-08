using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace XamarinSnippets.Controls
{
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
                    gestureRecognizer.Effects.Add(
                        effect);
                }
            }

            GestureRecognizers.Add(
                gestureRecognizer);
        }
    }
}