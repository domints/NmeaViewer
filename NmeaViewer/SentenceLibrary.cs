using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NmeaViewer.Sentences;

namespace NmeaViewer
{
    public static class SentenceLibrary
    {
        public static Dictionary<string, Type> Library = LoadLibrary();

        private static Dictionary<string, Type> LoadLibrary()
        {
            var sentenceType = typeof(NmeaSentence);
            var proprietaryType = typeof(Proprietary);
            var valueType = typeof(ValueSentence);
            return Assembly.GetAssembly(typeof(SentenceLibrary))!
                .GetTypes()
                .Where(t => sentenceType.IsAssignableFrom(t) && t != proprietaryType && t != valueType && t.GetCustomAttribute<SentenceTypeAttribute>() != null)
                .ToDictionary(t => t.GetCustomAttribute<SentenceTypeAttribute>()!.SequenceType);
        }
        
        public static NmeaSentence ParseSentence(string sentence)
        {
            var type = sentence.Substring(3, 3);
            if (Library.ContainsKey(type))
            {
                return (NmeaSentence)Library[type].GetConstructor([typeof(string)])!.Invoke([sentence]);
            }

            return new ValueSentence(sentence);
        }
    }
}