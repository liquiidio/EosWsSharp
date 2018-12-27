using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace EosWsSharp
{
    // TODO replace Types with theses types , move to seperate files

    public class Account : IEosName
    {
        private readonly string _account;

        public Account(string account)
        {
            _account = account;
        }

        private static readonly Regex AccountRegex = new Regex(@"([0-4,a-z,.]*)");

        public static implicit operator Account(string account)
        {
            account = account.ToLower();
            if (string.IsNullOrEmpty(account) || account.Length > 12 ||
                !Regex.Match(account, AccountRegex.ToString(), RegexOptions.None).Success)
                return null;

            return new Account(account);
        }

        public override string ToString() => _account;
    }

    public class Scope : IEosName
    {
        private readonly string _scope;

        public Scope(string scope)
        {
            _scope = scope;
        }

        private static readonly Regex ScopeRegex = new Regex(@"([0-4,a-z,]*)");

        public static implicit operator Scope(string scope)
        {
            scope = scope.ToLower();
            if (string.IsNullOrEmpty(scope) || scope.Length > 12 ||
                !Regex.Match(scope, ScopeRegex.ToString(), RegexOptions.None).Success)
                return null;

            return new Scope(scope);
        }

        public override string ToString() => _scope;
    }

    public class Code : IEosName
    {
        private readonly string _code;

        public Code(string code)
        {
            _code = code;
        }

        private static readonly Regex CodeRegex = new Regex(@"([0-4,a-z,]*)");

        public static implicit operator Code(string code)
        {
            code = code.ToLower();
            if (string.IsNullOrEmpty(code) || code.Length > 12 ||
                !Regex.Match(code, CodeRegex.ToString(), RegexOptions.None).Success)
                return null;

            return new Code(code);
        }

        public override string ToString() => _code;

    }

    public class EosNameConverter<T> : JsonConverter where T : IEosName
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return serializer.Deserialize<Type>(reader);
        }
    }

    public interface IEosName
    {    }
}