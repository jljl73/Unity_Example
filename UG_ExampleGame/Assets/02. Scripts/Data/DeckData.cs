using System.Collections.Generic;
using UniRx;

namespace UK.Data
{
    public class DeckData : DataModelBase
    {
        public ReactiveCollection<int> Decks { get; private set; } = new ReactiveCollection<int>();

        public override void Init()
        {
            Decks.Add(1);
        }

        public override void Dispose()
        {
        }
    }
}
