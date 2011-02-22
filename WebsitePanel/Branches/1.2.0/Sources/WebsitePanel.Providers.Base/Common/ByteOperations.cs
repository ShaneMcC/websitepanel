using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WebsitePanel.Providers.Common
{
	sealed class ByteVector
	{
		# region Public

		public ByteVector Add(Byte[] bytes, Int32 offset, Int32 count)
		{
			Byte [] b = new Byte[ count ];
			Array.Copy( bytes, offset, b, 0, count );
			_bytes.Add( b );
			_size += count;

			return this;
		}

		public ByteVector Add (Byte[] bytes)
		{
			return Add(bytes, 0, bytes.Length);
		}

		public ByteVector Add(string s, Encoding encoding)
		{
			if ( !string.IsNullOrEmpty(s) )
			{
				Add(encoding.GetBytes(s));
			}

			return this;
		}

		public ByteVector Add(string s)
		{
			return Add(s, Encoding.ASCII);
		}



		public Byte[] Get()
		{
			Byte [] result = new Byte[ _size ];
			Int32 offset = 0;

			foreach ( Byte [] b in _bytes )
				{
				Array.Copy( b, 0, result, offset, b.Length );
				offset += b.Length;
				}

			return result;
		}

		public string GetHexString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in Get())
			{
				sb.Append(b.ToString("x2"));
			}
			return sb.ToString();
		}

		public Byte[] GetMD5Hash()
		{
			return MD5.Create().ComputeHash(Get());
		}

		public void Clear()
		{
			_bytes.Clear();
			_size = 0;
		}



		public Int64 Size
		{
			get { return _size; }
		}

		# endregion 



		# region Private

		readonly List<Byte[]> _bytes = new List<Byte[]>();
		Int64 _size;

		# endregion
	}
}
