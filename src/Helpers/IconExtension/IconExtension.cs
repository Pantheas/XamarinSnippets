using System;

using Xamarin.Forms.Xaml;

namespace XamarinSnippets
{
    public enum Icon
    {
        // add icons according to icon font here
        // e.g.:
        // Search = 0xe900
    }

    public class IconExtension :
        IMarkupExtension<string>
    {
        public Icon Icon { get; set; }


        public string ProvideValue(
            IServiceProvider serviceProvider)
        {
            return ((char)Icon).ToString();
        }


        object IMarkupExtension.ProvideValue(
            IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}