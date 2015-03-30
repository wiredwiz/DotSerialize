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

      protected string ReadArrayDefinition()
      {
         var sb = new StringBuilder();
         if (Peek() != '[')
            throw InvalidNameException();
         while (true)
         {
            var d = Read();
            sb.Append(d);
            switch (d)
            {
               case ' ':
               case ',':
               case '[':
               case ']':
                  break;
               default:
                  throw InvalidNameException();
            }
            if (d == ']')
               break;
         }
         return sb.ToString();
      }

      protected int ReadNumber()
      {
         var sb = new StringBuilder();
         var digit = Read();
         if (!char.IsDigit(digit))
            throw InvalidNameException();
         while (char.IsNumber(digit = Read()))
            sb.Append(digit);
         if (sb.Length == 0)
            throw InvalidNameException();
         Back();
         int num;
         if (!int.TryParse(sb.ToString(), out num))
            throw InvalidNameException();
         return num;
      }

      protected ParserException InvalidNameException()
      {
         return new ParserException(string.Format("\"{0}\" is not a valid AssemblyQualifiedName", _Buffer));
      }

      protected AssemblyQualifiedName ReadAssemblyQualifiedName()
      {
         throw new NotImplementedException();
      }

      protected AssemblyQualifiedName.TypeInfo ReadTypeName()
      {
         char d = Read();
         if (d == Eof)
            throw InvalidNameException();
         var sb = new StringBuilder();
         while (!IsSpecial(d))
         {
            sb.Append(d);
         }
         AssemblyQualifiedName.TypeInfo result = null;
         if (d == ',')
         {
            result = new AssemblyQualifiedName.TypeInfo(sb.ToString());
         }         
         else if (d == '`')
         {
            // we have a generic
            var num = ReadNumber();
            var subs = ReadGenericDefinition(num);
            result = new AssemblyQualifiedName.TypeInfo(sb.ToString(), false, true, subs);
            if (Peek() == '[')
               sb.Append(ReadArrayDefinition());
         }
         else if (d == '[')
         {
            Back();
            sb.Append(ReadArrayDefinition());
            result = new AssemblyQualifiedName.TypeInfo(sb.ToString(), true);
         }
         return result;
      }

      protected List<AssemblyQualifiedName.TypeInfo> ReadGenericDefinition(int expectedTypeDeclarations)
      {
         var num = new StringBuilder();
         
         return null;
      }

      protected string ReadAssemblyName()
      {
         throw new NotImplementedException();
      }

      protected Version ReadVersion()
      {
         throw new NotImplementedException();
      }

      protected CultureInfo ReadCulture()
      {
         throw new NotImplementedException();
      }

      protected string RedPublicKeyToken()
      {
         throw new NotImplementedException();
      }
   }
}
