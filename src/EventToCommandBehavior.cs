using System;
using System.Reflection;
using System.Windows.Input;

using Xamarin.Forms;

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
    public class EventToCommandBehavior :
        BehaviorBase<View>
    {
        private Delegate eventHandler;


        public static readonly BindableProperty EventNameProperty =
            BindableProperty.Create(
                nameof(EventName),
                typeof(string),
                typeof(EventToCommandBehavior),
                propertyChanged: OnEventNameChanged);

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }


        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                nameof(Command),
                typeof(ICommand),
                typeof(EventToCommandBehavior));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }


        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(
                nameof(CommandParameter),
                typeof(object),
                typeof(EventToCommandBehavior));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }


        public static readonly BindableProperty InputConverterProperty =
            BindableProperty.Create(
                nameof(Converter),
                typeof(IValueConverter),
                typeof(EventToCommandBehavior));

        public IValueConverter Converter
        {
            get => (IValueConverter)GetValue(InputConverterProperty);
            set => SetValue(InputConverterProperty, value);
        }



        protected override void OnAttachedTo(
            View bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }


        protected override void OnDetachingFrom(
            View bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }


        private void RegisterEvent(
            string name)
        {
            if (string.IsNullOrWhiteSpace(
                name))
            {
                return;
            }


            var eventInfo = AssociatedObject
                .GetType()
                .GetRuntimeEvent(name);


            if (eventInfo == null)
            {
                throw new ArgumentException(
                    $"{nameof(EventToCommandBehavior)}: Can't register the '{EventName}' event.");
            }


            var methodInfo = typeof(EventToCommandBehavior)
                .GetTypeInfo()
                .GetDeclaredMethod("OnEvent");

            eventHandler = methodInfo
                .CreateDelegate(
                    eventInfo.EventHandlerType,
                    this);

            eventInfo.AddEventHandler(
                AssociatedObject,
                eventHandler);
        }


        private void DeregisterEvent(
            string name)
        {
            if (string.IsNullOrWhiteSpace(
                name))
            {
                return;
            }

            if (eventHandler == null)
            {
                return;
            }


            var eventInfo = AssociatedObject
                .GetType()
                .GetRuntimeEvent(name);


            if (eventInfo == null)
            {
                throw new ArgumentException(
                    $"{nameof(EventToCommandBehavior)}: Can't de-register the '{EventName}' event.");
            }


            eventInfo.RemoveEventHandler(
                AssociatedObject,
                eventHandler);

            eventHandler = null;
        }

        private void OnEvent(
            object sender,
            object eventArgs)
        {
            if (Command == null)
            {
                return;
            }


            object parameter;

            if (CommandParameter != null)
            {
                parameter = CommandParameter;
            }
            else if (Converter != null)
            {
                parameter = Converter.Convert(
                    eventArgs, 
                    typeof(object), 
                    null, 
                    null);
            }
            else
            {
                parameter = eventArgs;
            }


            if (Command.CanExecute(
                parameter))
            {
                Command.Execute(
                    parameter);
            }
        }


        private static void OnEventNameChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (!(bindable is EventToCommandBehavior behavior))
            {
                return;
            }

            if (behavior.AssociatedObject == null)
            {
                return;
            }


            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;


            behavior.DeregisterEvent(
                oldEventName);

            behavior.RegisterEvent(
                newEventName);
        }
    }
}
