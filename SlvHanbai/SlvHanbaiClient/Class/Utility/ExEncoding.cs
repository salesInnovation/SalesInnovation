using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace SlvHanbaiClient.Class.Utility
{
    public class ExUTF8Encoding : System.Text.UTF8Encoding
	{
        public string OnGetString(byte[] bytes, int index, int count)
        {
            return base.GetString(bytes, index, count);
        }

        public override int GetMaxByteCount(int charCount)
        {
            throw new NotImplementedException();
        }

        public override char[] GetChars(byte[] bytes, int index, int count)
        {
            return base.GetChars(bytes, index, count);
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override int GetMaxCharCount(int byteCount)
        {
            throw new NotImplementedException();
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            throw new NotImplementedException();
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            throw new NotImplementedException();
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            throw new NotImplementedException();
        }

	}
}
