using System;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;

namespace InnergyTask.Tests
{
	public class XUnitBase
	{
		protected ITestOutputHelper Output { get; private set; }

		public XUnitBase(ITestOutputHelper output)
		{
			Output = output;
		}

		protected static Stream GetResourceStream(string resourceName)
		{
			var stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName);
			if (stream != null)
				return stream;

			throw new InvalidOperationException("Resource not found: " + resourceName);
		}

		protected static string GetResourceText(string resourceName)
		{
			var sb = new StringBuilder();
			using Stream stream = GetResourceStream(resourceName);
			using StreamReader reader = new StreamReader(stream);

			string line;
			while (null != (line = reader.ReadLine()))
				sb.AppendLine(line);

			return sb.ToString();
		}
	}
}