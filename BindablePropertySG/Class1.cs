using System;
using Xamarin.Forms;

namespace BindablePropertySG
{
    [BPSourceGen.BPCreation(PropertyName = "Text", ReturnType = typeof(string), OwnerType = typeof(MyCustomView), DefaultValue = "test", PropertyChangedMethodName = "Invalidate")]
    [BPSourceGen.BPCreation(PropertyName = "TextColor", ReturnType = typeof(Color), OwnerType = typeof(MyCustomView), DefaultValue = "Color.Blue", PropertyChangedMethodName = "Invalidate")]
    partial class MyCustomView : Xamarin.Forms.View
    {
        //public static readonly BindableProperty TextProperty = BindableProperty.Create(
        //    nameof(Text), typeof(string), typeof(MyCustomView), default, BindingMode.TwoWay, propertyChanged: Invalidate);

        //public string Text
        //{
        //    get => (string)GetValue(TextProperty);
        //    set => SetValue(TextProperty, value);
        //}


        static void Invalidate(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MyCustomView customView)
            {
                customView.Text = "";
                customView.TextColor = Color.Brown;

            }
        }


    }

}
