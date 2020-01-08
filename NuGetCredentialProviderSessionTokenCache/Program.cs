//
// Program.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//
// Copyright (c) 2020 Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Threading;
using NuGetCredentialProvider.Logging;
using NuGetCredentialProvider.Util;

namespace NuGetCredentialProviderSessionTokenCache
{
	class MainClass
	{
		static readonly string LocalAppDataLocation = Path.Combine (
			Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create),
			"MicrosoftCredentialProvider");

		static string SessionTokenCacheLocation { get; } = Path.Combine (LocalAppDataLocation, "SessionTokenCache.dat");

		public static void Main (string[] args)
		{
			try {
				if (args.Length > 0) {
					if (args [0] == "add") {
						if (args.Length == 3) {
							AddToCache (args [1], args [2]);
						}
					} else if (args [0] == "list") {
						ListKeys ();
					}
					return;
				}

				PrintUsage ();
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
		}

		static void PrintUsage ()
		{
			Console.WriteLine ("Usage:");
			Console.WriteLine ("    add uri token");
			Console.WriteLine ("    list");
		}

		static void AddToCache (string uri, string token)
		{
			var cache = new SessionTokenCache (
				SessionTokenCacheLocation,
				new ConsoleLogger (),
				CancellationToken.None);

			cache [new Uri (uri)] = token;
		}

		static void ListKeys ()
		{
			if (File.Exists (SessionTokenCacheLocation)) {
				Console.WriteLine ("SessionTokenCache found at {0}", SessionTokenCacheLocation);
			} else {
				Console.WriteLine ("SessionTokenCache not found at {0}", SessionTokenCacheLocation);
				return;
			}

			Console.WriteLine ();
			Console.WriteLine ("Uris:");
			var cache = new SessionTokenCache (
				SessionTokenCacheLocation,
				new ConsoleLogger (),
				CancellationToken.None);

			foreach (Uri key in cache.Cache.Keys) {
				Console.WriteLine (key);
			}
		}
	}
}
