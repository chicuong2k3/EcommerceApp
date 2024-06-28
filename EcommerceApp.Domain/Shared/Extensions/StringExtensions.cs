﻿using System.Text;

namespace EcommerceApp.Domain.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateSlug(this string title, bool remapToAscii = true, int maxlength = 80)
        {
            if (title == null)
            {
                return string.Empty;
            }

            int length = title.Length;
            bool prevdash = false;
            StringBuilder stringBuilder = new StringBuilder(length);
            char c;

            for (int i = 0; i < length; ++i)
            {
                c = title[i];
                if (c >= 'a' && c <= 'z' || c >= '0' && c <= '9')
                {
                    stringBuilder.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lower-case
                    stringBuilder.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                  c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && stringBuilder.Length > 0)
                    {
                        stringBuilder.Append('-');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    int previousLength = stringBuilder.Length;

                    if (remapToAscii)
                    {
                        stringBuilder.Append(RemapInternationalCharToAscii(c));
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }

                    if (previousLength != stringBuilder.Length)
                    {
                        prevdash = false;
                    }
                }

                if (i == maxlength)
                {
                    break;
                }
            }

            if (prevdash)
            {
                return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
            }
            else
            {
                return stringBuilder.ToString();
            }
        }

        private static string RemapInternationalCharToAscii(char character)
        {
            string s = character.ToString().ToLowerInvariant();
            if ("àáạảãâầấậẩẫăằắặẳẵ".Contains(s))
            {
                return "a";
            }
            else if ("èéẹẻẽêềếệểễ".Contains(s))
            {
                return "e";
            }
            else if ("ìíịỉĩ".Contains(s))
            {
                return "i";
            }
            else if ("òóọỏõôồốộổỗơờớợởỡ".Contains(s))
            {
                return "o";
            }
            else if ("ùúụủũưừứựửữ".Contains(s))
            {
                return "u";
            }
            else if ("ỳýỵỷỹ".Contains(s))
            {
                return "y";
            }
            else if ("đ".Contains(s))
            {
                return "d";
            }
            else
            {
                return character.ToString();
            }
        }

    }
}
