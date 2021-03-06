﻿using System.Collections.Generic;
using System.Reflection;
using Plugin.Xamarin.Forms.PdfSharp.Contracts;

namespace Plugin.Xamarin.Forms.PdfSharp.iOS
{
	public class Platform
	{
		public static void Init(ICustomFontProvider customFontProvider = null, IList<Assembly> rendererAssemblies = null)
		{
			PDFManager.Init(new IosImageSource(), customFontProvider);
		}
	}
}