using SkyscraperCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace Skyscraper
{
    public class SymbolsProvider : ISymbolsProvider
    {
        private bool _inited;

        public Dictionary<int, string> symbols { get; private set; }
        public SymbolsProvider()
        {
            symbols = new Dictionary<int, string>();
        }
        public void Init(string basePath, string iconsPath)
        {
            if (_inited)
                return;
            _inited = true;
            var icons = Directory.GetFiles(basePath + iconsPath);
            new Random().Shuffle(icons);
            var i = 0;
            foreach(var icon in icons)
            {
                symbols.Add(i, icon.Substring(basePath.Length));
                i++;
            }
        }

        public Symbol GetSymbol(int id)
        {
            if(symbols.ContainsKey(id))
                return new Symbol { Id = id, Path = symbols[id] };
            throw new ArgumentException("Id",string.Format("id {0} not found",id));
        }
    }
}