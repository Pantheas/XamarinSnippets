using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace XamarinSnippets
{
    public class DynamicTemplateSelector :
        DataTemplateSelector
    {
        public DataTemplate Fallback { get; set; }

        public List<Template> Templates { get; set; } = new List<Template>();


        protected override DataTemplate OnSelectTemplate(
            object item,
            BindableObject container)
        {
            if (item == null)
            {
                return Fallback;
            }

            var templateMap = Templates.FirstOrDefault(
                template => template?.ItemType?.IsAssignableFrom(item?.GetType()));

            if (templateMap == null)
            {
                return Fallback;
            }

            return templateMap.DataTemplate;
        }
    }


    // TODO: move to separate file when using
    public class Template
    {
        public Type ItemType { get; set; }

        public DataTemplate DataTemplate { get; set; }
    }
}