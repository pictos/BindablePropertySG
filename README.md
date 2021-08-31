# BindablePropertySG

- [English version](https://github.com/pictos/BindablePropertySG/new/master?readme=1#english)

## Português
Um gerador de código para criar BindableProperties para controles.

Em vez de escrever todo aquele código chato para criar BindableProperties, para Xamarin.Forms, você pode usar este Gerador de código e vc só precisa preencher
os valores que vc vai usar em um atribute e ele vai gerar todo o código chato pra vc. Não tenho certeza se essa abordagem é melhor, me diz ai.

## Uso

Lembre-se de marcar seu control como `partial` e use o attributo para preencher as propriedades que vc quer usar

Código que você digita:

```csharp
using System;
using Xamarin.Forms;

namespace BindablePropertySG
{
    [BPSourceGen.BPCreation(PropertyName = "Text", ReturnType = typeof(string), OwnerType = typeof(MyCustomView), DefaultValue = "test", PropertyChangedMethodName = "Invalidate")]
    [BPSourceGen.BPCreation(PropertyName = "TextColor", ReturnType = typeof(Color), OwnerType = typeof(MyCustomView), DefaultValue = "Color.Blue", PropertyChangedMethodName = "Invalidate")]
    partial class MyCustomView : Xamarin.Forms.View
    {
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
```
Código que o Roslyn "digita":

```csharp
using Xamarin.Forms;
using System;

namespace BindablePropertySG
{
    partial class MyCustomView
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(BindablePropertySG.MyCustomView), "test", BindingMode.OneWay, null, Invalidate, null, null, null);
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Xamarin.Forms.Color), typeof(BindablePropertySG.MyCustomView), "Color.Blue", BindingMode.OneWay, null, Invalidate, null, null, null);
        public Xamarin.Forms.Color TextColor { get => (Xamarin.Forms.Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
    }
}
```


## English
A source generator to create BindableProperties for controls.

Instead of doing all the boiler plate code to generate BindableProperties, for Xamarin.Forms, you can use this SourceGenerator and you just fill all values in an attribute
and it will generate the boiler plate code for you. Not sure if this is better with this approach, you tell me.


## Usage

Remember to mark your control as `partial` and use the attribute filling the props that you want to use

The code that you type:
```csharp
using System;
using Xamarin.Forms;

namespace BindablePropertySG
{
    [BPSourceGen.BPCreation(PropertyName = "Text", ReturnType = typeof(string), OwnerType = typeof(MyCustomView), DefaultValue = "test", PropertyChangedMethodName = "Invalidate")]
    [BPSourceGen.BPCreation(PropertyName = "TextColor", ReturnType = typeof(Color), OwnerType = typeof(MyCustomView), DefaultValue = "Color.Blue", PropertyChangedMethodName = "Invalidate")]
    partial class MyCustomView : Xamarin.Forms.View
    {
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
```
Code that Roslyn "types":

```csharp
using Xamarin.Forms;
using System;

namespace BindablePropertySG
{
    partial class MyCustomView
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(BindablePropertySG.MyCustomView), "test", BindingMode.OneWay, null, Invalidate, null, null, null);
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Xamarin.Forms.Color), typeof(BindablePropertySG.MyCustomView), "Color.Blue", BindingMode.OneWay, null, Invalidate, null, null, null);
        public Xamarin.Forms.Color TextColor { get => (Xamarin.Forms.Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
    }
}
```


