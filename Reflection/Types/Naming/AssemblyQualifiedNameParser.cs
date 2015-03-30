using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Exceptions;

namespace Org.Edgerunner.DotSerialize.Reflection.Types.Naming
{
   public class AssemblyQualifiedNameParser
   {
      protected string _Buffer;
      protected int _Position;
      protected const char Eof = '\0';

      public AssemblyQualifiedName Parse(string name)
      {
         _Buffer = name;
         _Position = 0;
         return ReadAssemblyQualifiedName();
      }

      protected char Read()
      {
         if (_Position >= _Buffer.Length)
            return Eof;
         var result = _Buffer[_Position];
         _Position++;
         return result;
      }

      protected string ReadText(int length)
      {
         if (_Position >= _Buffer.Length)
            return string.Empty;
         string result;
         if ((_Position + length) >= _Buffer.Length)
         {
            result = _Buffer.Substring(_Position);
            _Position = _Buffer.Length;
            return result;
         }
         result = _Buffer.Substring(_Position, length);
         _Position += length;
         return result;
      }

      protected char Peek()
      {
         if (_Position >= _Buffer.Length)
            return Eof;
         return _Buffer[_Position];
      }

      public void Back()
      {
         if (_Position == 0)
            return;
         _Position--;
      }

      protected void SkipWhiteSpace()
      {
         while ((_Position < _Buffer.Length) && IsWhitespace(_Buffer[_Position]))
            _Position++;
      }

      protected bool IsSpecial(char character)
      {
         switch (character)
         {
            case ',':
            case '[':
            case ']':
            case '`':
            case '*':
               return true;
            default:
               return false;
         }
      }

      protected bool IsWhitespace(char character)
      {
         switch (character)
         {
            case '\t':
            case '\r':
            case '\n':
            case ' ':
               return true;
            default:
               return false;
         }
      }

      protected List<int> ReadArrayDimensions()
      {
         if (Peek() != '[')
            throw InvalidNameException(_Position + 1);

         var dimensions = new List<int>();
         while (Peek() == '[')
            dimensions.Add(ReadArrayDimension());

         return dimensions;
      }

      protected int ReadArrayDimension()
      {
         int dimensions = 1;
         while (true)
         {
            var d = Read();
            switch (d)
            {
               case ' ':
               case '[':
               case ']':
                  break;
               case ',':
                  dimensions++;
                  break;
               default:
                  throw InvalidNameException(_Position);
            }
            if (d == ']')
               break;
         }
         return dimensions;
      }

      protected int ReadNumber()
      {
         var sb = new StringBuilder();
         var digit = Read();
         if (!char.IsDigit(digit))
            throw InvalidNameException(_Position);
         sb.Append(digit);
         while (char.IsNumber(digit = Read()))
            sb.Append(digit);
         Back();
         int num;
         if (!int.TryParse(sb.ToString(), out num))
            throw InvalidNameException(_Position);
         return num;
      }

      protected StringBuilder ReadNonSpecialText()
      {
         char d = Read();
         if (d == Eof)
            throw InvalidNameException(_Position);
         var sb = new StringBuilder();
         var escape = false;
         while ((d != Eof) && (!IsSpecial(d) || escape))
         {
            if (escape)
               escape = false;
            sb.Append(d);
            if (d == '\\')
               escape = true;
            d = Read();
         }
         Back();
         return sb;
      }

      protected ParserException InvalidNameException(int position)
      {
         return InvalidNameException(position, null);
      }

      protected ParserException InvalidNameException(int position, Exception ex)
      {
         return new ParserException(string.Format("\"{0}\" is not a valid AssemblyQualifiedName.  Parse error at position {1}", _Buffer, position), ex);
      }

      protected AssemblyQualifiedName ReadAssemblyQualifiedName()
      {
         var result = new AssemblyQualifiedName { Type = ReadTypeName() };
         if (!ReadPartSeparator())
            return result;
         result.Assembly = ReadAssemblyName();
         if (!ReadPartSeparator())
            return result;
         result.Version = ReadVersion();
         if (!ReadPartSeparator())
            return result;
         result.Culture = ReadCulture();
         if (!ReadPartSeparator())
            return result;
         result.PublicKeyToken = ReadPublicKeyToken();
         return result;
      }

      protected bool ReadPartSeparator()
      {
         var d = Read();
         if (d == Eof)
            return false;
         if (d != ',')
            throw InvalidNameException(_Position);
         return true;
      }
      protected AssemblyQualifiedName.TypeInfo ReadTypeName()
      {
         var sb = ReadNonSpecialText();
         char d = Read();
         AssemblyQualifiedName.TypeInfo result = null;
         if (d == ',')
         {
            Back();
            result = new AssemblyQualifiedName.TypeInfo(sb.ToString());
         }
         else if (d == '*')
         {
            var dimensions = Peek() == '[' ? ReadArrayDimensions() : new List<int>();
            result = new AssemblyQualifiedName.TypeInfo(sb.ToString(), true, dimensions);
         }
         else if (d == '`')
         {
            // we have a generic
            var num = ReadNumber();
            var subs = ReadGenericDefinition(num);
            var dimensions = Peek() == '[' ? ReadArrayDimensions() : new List<int>();
            result = new AssemblyQualifiedName.TypeInfo(sb.ToString(), false, dimensions, subs);
         }
         else if (d == '[')
         {
            Back();
            result = new AssemblyQualifiedName.TypeInfo(sb.ToString(), ReadArrayDimensions());
         }
         return result;
      }

      protected List<AssemblyQualifiedName> ReadGenericDefinition(int expectedTypeDeclarations)
      {
         if (expectedTypeDeclarations < 1)
            throw new ArgumentOutOfRangeException("expectedTypeDeclarations", "expectedTypeDeclarations must be positive integer.");
         var subs = new List<AssemblyQualifiedName>(expectedTypeDeclarations);
         if (Read() != '[')
            throw InvalidNameException(_Position);
         for (int i = 1; i <= expectedTypeDeclarations; i++)
         {
            subs.Add(ReadGenericType());
            if (i != expectedTypeDeclarations)
            {
               SkipWhiteSpace();
               var d = Read();
               if (d != ',')
                  throw InvalidNameException(_Position);
               SkipWhiteSpace();
            }
         }
         if (Read() != ']')
            throw InvalidNameException(_Position);
         return subs;
      }

      protected AssemblyQualifiedName ReadGenericType()
      {
         if (Read() != '[')
            throw InvalidNameException(_Position);
         var result = ReadAssemblyQualifiedName();
         if (Read() != ']')
            throw InvalidNameException(_Position);
         return result;
      }

      protected AssemblyQualifiedName.AssemblyInfo ReadAssemblyName()
      {
         return new AssemblyQualifiedName.AssemblyInfo(ReadNonSpecialText().ToString().Trim());
      }

      private void ReadKeyForKeyValuePair(string keyToRead)
      {
         var pos = _Position + 1;
         var key = ReadText(keyToRead.Length);
         if (key != keyToRead)
            throw InvalidNameException(pos);
         if (Read() != '=')
            throw InvalidNameException(_Position);
      }
      protected Version ReadVersion()
      {
         SkipWhiteSpace();
         ReadKeyForKeyValuePair("Version");
         Version vers;
         try
         {
            vers = new Version(ReadNonSpecialText().ToString());
         }
         catch (ArgumentException ex)
         {
            throw InvalidNameException(_Position + 1, ex);
         }
         catch (FormatException ex)
         {
            throw InvalidNameException(_Position + 1, ex);
         }
         catch (OverflowException ex)
         {
            throw InvalidNameException(_Position + 1, ex);
         }
         return vers;
      }

      protected string ReadCulture()
      {
         SkipWhiteSpace();
         ReadKeyForKeyValuePair("Culture");
         return ReadNonSpecialText().ToString().Trim();
      }

      protected string ReadPublicKeyToken()
      {
         SkipWhiteSpace();
         ReadKeyForKeyValuePair("PublicKeyToken");
         return ReadNonSpecialText().ToString().Trim();
      }
   }
}
