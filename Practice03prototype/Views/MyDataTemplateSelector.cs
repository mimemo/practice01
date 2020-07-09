

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Practice03prototype.Views
{
    public class MyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Normal { get; set; }
        public DataTemplate Accent { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {

            if( item is Practice03prototype.ViewModels.Epage iPage)
            {

            

            if(iPage.Index % 2 == 0)
            {
                return this.Normal;
            }
            else
            {
                return this.Accent;
            }
            }

            return this.Normal;
        }
    }
}
