using System;

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
    public class BehaviorBase<T> :
        Behavior<T>

        where T : BindableObject
    {
        public T AssociatedObject { get; private set; }


        protected override void OnAttachedTo(
            T bindable)
        {
            base.OnAttachedTo(bindable);

            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }


            bindable.BindingContextChanged += OnBindingContextChanged;
        }


        protected override void OnDetachingFrom(
            T bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.BindingContextChanged -= OnBindingContextChanged;

            AssociatedObject = null;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            BindingContext = AssociatedObject.BindingContext;
        }


        private void OnBindingContextChanged(
            object sender,
            EventArgs eventArgs)
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}