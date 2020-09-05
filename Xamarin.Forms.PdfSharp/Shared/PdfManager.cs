using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Plugin.Xamarin.Forms.PdfSharp.Contracts;
using Plugin.Xamarin.Forms.PdfSharp.Utils;
using PdfSharpCore;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using Xamarin.Forms;
using ImageSource = MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource;

[assembly: InternalsVisibleTo("Plugin.Xamarin.Forms.PdfSharp.Droid")]
[assembly: InternalsVisibleTo("Plugin.Xamarin.Forms.PdfSharp.iOS")]
[assembly: InternalsVisibleTo("Plugin.Xamarin.Forms.PdfSharp.UWP")]
namespace Plugin.Xamarin.Forms.PdfSharp
{
	public class PDFManager
	{
		internal static PDFManager Instance { get; private set; }

		private PDFManager()
		{
		}

		internal Dictionary<Type, Type> Renderers { get; set; }

		internal static void Init(ImageSource handler, ICustomFontProvider customFontProvider = null, IList<Assembly> rendererAssemblies = null)
		{
			if (Instance == null)
				Instance = new PDFManager();
			else
				throw new InvalidOperationException("Plugin.Xamarin.Forms.PdfSharp already initialized before");

			GlobalFontSettings.FontResolver = new FontProvider(customFontProvider);
			ImageSource.ImageSourceImpl = handler;

			Instance.Renderers = new Dictionary<Type, Type>();

			if (rendererAssemblies == null)
				rendererAssemblies = new List<Assembly>();
			var thisAssembly = typeof(PDFManager).GetTypeInfo().Assembly;

			if (!rendererAssemblies.Contains(thisAssembly))
				rendererAssemblies.Add(thisAssembly);

			foreach (var asm in rendererAssemblies)
			{
				foreach (var typeInfo in asm.DefinedTypes)
				{
					if (typeInfo.IsDefined(typeof(Attributes.PdfRendererAttribute), false))
					{
						var rInfo = typeInfo.GetCustomAttribute<Attributes.PdfRendererAttribute>();
						if (Instance.Renderers.ContainsKey(rInfo.ViewType))
							Instance.Renderers[rInfo.ViewType] = typeInfo.AsType();
						else
							Instance.Renderers.Add(rInfo.ViewType, typeInfo.AsType());
					}
				}
			}
		}

		[Obsolete("use Init(,,rendererAssemblies)")]
		public static void RegisterRenderer(Type viewType, Type rendererType)
		{
			if (Instance == null)
				throw new InvalidOperationException("You must call Init firsts");

			if (!rendererType.GetTypeInfo().IsSubclassOf(typeof(Renderers.PdfRendererBase)))
				throw new ArgumentException("Renderertype Must inherit PdfRenderdererBase<View>");

			if (Instance.Renderers.ContainsKey(viewType))
				Instance.Renderers[viewType] = rendererType;
			else
				Instance.Renderers[viewType] = rendererType;
		}

		public static PdfDocument GeneratePDFFromView(View view, PageOrientation orientaiton = PageOrientation.Portrait, PageSize size = PageSize.A4, bool resizeToFit = true)
		{
			if (Instance == null)
				throw new InvalidOperationException("You must call Init first");

			PdfGenerator generator = new PdfGenerator(view, orientaiton, size, resizeToFit);
			var pdf = generator.Generate();

			return pdf;
		}

	}
}
