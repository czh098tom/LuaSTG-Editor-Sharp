using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace LuaSTGEditorSharp.Toolbox
{
    public class BindableStaticResource : StaticResourceExtension
    {
        private static readonly DependencyProperty DummyProperty;

        static BindableStaticResource()
        {
            DummyProperty = DependencyProperty.RegisterAttached("Dummy",
                                                                typeof(Object),
                                                                typeof(DependencyObject),
                                                                new UIPropertyMetadata(null));
        }

        public Binding MyBinding { get; set; }

        public BindableStaticResource()
        {
        }

        public BindableStaticResource(Binding binding)
        {
            MyBinding = binding;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetObject = (FrameworkElement)target.TargetObject;

            MyBinding.Source = targetObject.DataContext;
            var DummyDO = new DependencyObject();
            BindingOperations.SetBinding(DummyDO, DummyProperty, MyBinding);

            ResourceKey = DummyDO.GetValue(DummyProperty);

            return ResourceKey != null ? base.ProvideValue(serviceProvider) : null;
        }
        public new object ResourceKey
        {
            get
            {
                return base.ResourceKey;
            }
            set
            {
                if (value != null)
                {
                    base.ResourceKey = value;
                }
            }
        }
    }
}
