using System;
using System.Collections.Generic;
using System.Text;

namespace RazorLightCustom.Internal
{
    public interface ICharBufferSource
    {
        char[] Rent(int bufferSize);

        void Return(char[] buffer);
    }
}
