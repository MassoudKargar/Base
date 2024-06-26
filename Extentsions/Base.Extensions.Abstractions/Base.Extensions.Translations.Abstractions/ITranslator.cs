﻿namespace Base.Extensions.Translations.Abstractions;

public interface ITranslator
{
    string this[string name] { get; set; }

    string this[string name, params string[] arguments] { get; set; }

    string this[char separator, params string[] names] { get; set; }

    string GetString(string name);

    string GetString(string pattern, params string[] arguments);

    string GetConcatString(char separator = ' ', params string[] names);

}
