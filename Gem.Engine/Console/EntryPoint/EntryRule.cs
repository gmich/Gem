using System;

namespace Gem.Console
{
    public class EntryRule : IEntryRule
    {
        private readonly Predicate<char> rule;

        public EntryRule(Predicate<char> rule)
        {
            this.rule = rule;
        }

        public bool Apply(char ch)
        {
            return rule(ch);
        }
    }
}
