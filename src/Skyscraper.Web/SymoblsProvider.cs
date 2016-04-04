using Skyscraper.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace Skyscraper.Web
{
    public class SymbolsProvider : ISymbolsProvider
    {
        private bool _inited;

        public Dictionary<int, string> symbols { get; private set; }
        public SymbolsProvider()
        {
            symbols = new Dictionary<int, string>();
        }
        public void Init(string iconsPath)
        {
            if (_inited)
                return;
            _inited = true;
            var icons = Directory.GetFiles(iconsPath);
            new Random().Shuffle(icons);
            var i = 0;
            foreach (var icon in icons)
            {
                symbols.Add(i, icon);
                i++;
            }
        }

        public Symbol GetSymbol(int id)
        {
            if (symbols.ContainsKey(id))
                return new Symbol { Id = id, Path = "data:image/svg+xml;base64," + GetBase64String(symbols[id]) };
            throw new ArgumentException("Id", string.Format("id {0} not found", id));
        }

        private string GetBase64String(string imagePath)
        {
            FileStream inFile;
            byte[] binaryData;

            try
            {
                using (inFile = new FileStream(imagePath,
                                          FileMode.Open,
                                          FileAccess.Read))
                {
                    binaryData = new Byte[inFile.Length];
                    long bytesRead = inFile.Read(binaryData, 0,
                                         (int)inFile.Length);
                }

            }
            catch (Exception)
            {
                // Error creating stream or reading from it.
                return null;
            }

            // Convert the binary input into Base64 UUEncoded output.
            string base64String;
            try
            {
                base64String =
                  Convert.ToBase64String(binaryData,
                                         0,
                                         binaryData.Length);
            }
            catch (ArgumentNullException)
            {
                // log error
                return null;
            }
            return base64String;
        }
    }
}