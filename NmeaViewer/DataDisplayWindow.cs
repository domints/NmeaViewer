using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NmeaViewer.Sentences;
using NmeaViewer.Tree;
using Serilog;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace NmeaViewer
{
    public class DataDisplayWindow : Window
    {
        private readonly TreeView<ITreeObject> _messagesView;
        private readonly TreeRoot _root = new();

        public DataDisplayWindow()
        {
            _messagesView = new TreeView<ITreeObject>()
            {
                TreeBuilder = new MessageTreeBuilder(),
                X = 0,
                Y = 0,
                Height = Dim.Fill(),
                Width = Dim.Fill(),
                Text = "Messages",
                BorderStyle = Terminal.Gui.Drawing.LineStyle.Single
            };

            _messagesView.AddObject(_root);

            Add(_messagesView);

            NmeaReceiver.Instance.SentenceReceived += ProcessSentence;
        }

        public void ProcessSentence(NmeaSentence sentence)
        {
            if (!sentence.IsCorrect)
                return;
            Log.Debug("Processing sentence in DDW: {sentence}", JsonConvert.SerializeObject(sentence));
            ITreeObject objToRefresh = _root;
            var talker = sentence.TalkerId!;
            var type = sentence.SentenceId!;
            TreeTalker? talkerObj;
            if (_root.Talkers.ContainsKey(talker))
            {
                talkerObj = _root.Talkers[talker];
                objToRefresh = talkerObj;
            }
            else
            {
                talkerObj = new TreeTalker(talker);
                _root.Talkers.Add(talker, talkerObj);
            }

            BaseTreeSentence sentenceObj;
            var isSeqSentence = sentence is ISequenceSentence;
            if (talkerObj.Sentences.ContainsKey(type))
            {
                if (isSeqSentence != (talkerObj.Sentences[type] is SequenceTreeSentence))
                {
                    talkerObj.Sentences[type] = isSeqSentence ? new SequenceTreeSentence(type) : new TreeSentence(type);
                }

                sentenceObj = talkerObj.Sentences[type];
                objToRefresh = sentenceObj;
            }
            else
            {
                sentenceObj = isSeqSentence ? new SequenceTreeSentence(type) : new TreeSentence(type);
                talkerObj.Sentences.Add(type, sentenceObj);
            }

            if (sentence is ValueSentence valueSentence)
            {
                var o = sentenceObj as TreeSentence;
                o!.Values.Clear();
                for (int i = 0; i < valueSentence.Values.Count; i++)
                {
                    var nameString = i.ToString();
                    o.Values.Add(nameString, new TreeSentenceValue(nameString) { Value = valueSentence.Values[i] });
                }
            }
            else if (sentence is Proprietary proprietarySentence)
            {
                var o = sentenceObj as TreeSentence;
                if (o!.Values.TryGetValue("value", out TreeSentenceValue? value))
                {
                    value.Value = proprietarySentence.Data ?? string.Empty;
                }
                else
                {
                    o.Values.Add("value", new TreeSentenceValue("value") { Value = proprietarySentence.Data ?? string.Empty });
                }
            }
            else
            {
                TreeSentence? leaf = (sentenceObj as TreeSentence)!;
                if (sentence is ISequenceSentence seqSen)
                {
                    var seq = (sentenceObj as SequenceTreeSentence)!;
                    if (seq.Sequence.ContainsKey(seqSen.MessageNumber))
                    {
                        leaf = seq.Sequence[seqSen.MessageNumber];
                    }
                    else
                    {
                        leaf = new TreeSentence(seqSen.MessageNumber.ToString());
                        seq.Sequence.Add(seqSen.MessageNumber, leaf);
                    }

                    foreach (var k in seq.Sequence.Keys.ToList())
                    {
                        if (k > seqSen.TotalMessages)
                        {
                            seq.Sequence.Remove(k);
                        }
                    }
                }

                var allProps = sentence.GetType()
                    .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .Where(p => !Attribute.IsDefined(p, typeof(SkipValueAttribute)))
                    .ToList();
                foreach (var prop in allProps)
                {
                    if (leaf!.Values.TryGetValue(prop.Name, out TreeSentenceValue? value))
                    {
                        value.Value = GetString(prop, sentence);
                    }
                    else
                    {
                        leaf.Values.Add(prop.Name, new TreeSentenceValue(prop.Name)
                        {
                            Value = GetString(prop, sentence)
                        });
                    }
                }
            }

            Application.Invoke(() => _messagesView.RefreshObject(objToRefresh));
        }

        private string? GetString(PropertyInfo prop, object obj)
        {
            if (prop.PropertyType == typeof(TimeOnly))
                return ((TimeOnly)prop.GetValue(obj)!).ToLongTimeString();

            return Convert.ToString(prop.GetValue(obj))!;
        }
    }
}