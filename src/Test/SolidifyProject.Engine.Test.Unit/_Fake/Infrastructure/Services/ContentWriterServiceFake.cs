﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Markdig.Extensions.TaskLists;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Test.Unit._Fake.Infrastructure.Services
{
    public class ContentWriterServiceFake<T> : IContentWriterService<T> where T : ModelBase
    {
        private readonly List<T> _storage;
        public List<T> Storage => _storage;
        
        public ContentWriterServiceFake(List<T> storage = null)
        {
            _storage = storage ?? new List<T>();
        }
        
        public Task SaveContentAsync(string id, T content)
        {
            var elements = _storage
                .Where(x => x.Id.Equals(id, StringComparison.Ordinal))
                .ToList();
            
            foreach (var element in elements)
            {
                _storage.Remove(element);
            }

            content.Id = id;
            _storage.Add(content);

            return Task.FromResult<object>(null);
        }

        public Task CleanFolderAsync(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var elements = _storage
                    .Where(x => x.Id.StartsWith($"{path}{Path.DirectorySeparatorChar}"));
                
                foreach (var element in elements)
                {
                    _storage.Remove(element);
                }
            }
            
            _storage.Clear();

            return Task.CompletedTask;
        }
    }
}