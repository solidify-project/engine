using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;

namespace SolidifyProject.Engine.Infrastructure.Services
{
    public sealed class DataService : IDataService
    {
        public const string COLLECTION_PROPERTY = "__collection";
        
        public IContentReaderService<CustomDataModel> DataReaderService { get; }

        public delegate Task LogEventHandler(string message);
        public event LogEventHandler OnLogEvent;
        
        public DataService(IContentReaderService<CustomDataModel> dataReaderService)
        {
            DataReaderService = dataReaderService;
        }

        public async Task<ExpandoObject> GetDataModelAsync()
        {
            var data = await DataReaderService.LoadContentsIdsAsync();
            var dataTasks = data.Select(GetDataByIdAsync).ToList();

            await Task.WhenAll(dataTasks);

            var dataModel = new ExpandoObject();
            
            foreach (var result in dataTasks.Select(x => x.Result))
            {
                var sections = result
                    .Key
                    .Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);

                PopulateDataObject(ref dataModel, sections, result.Value);
            }
            
            return dataModel;
        }

        private void PopulateDataObject(
            ref ExpandoObject dataObject,
            IEnumerable<string> sections,
            object value)
        {
            var sectionsArray = sections as string[] ?? sections.ToArray();
            
            var currentSection = GetCurrentSection(sectionsArray);
            var currentValue = GetCurrentValue(sectionsArray, value);

            PopulateCurrentSection(ref dataObject, currentSection, currentValue);

            PopulateSpecialCollection(ref dataObject, currentValue);
        }

        private void PopulateCurrentSection(ref ExpandoObject dataObject, string currentSection, object currentValue)
        {
            if (dataObject.Any(x => x.Key.Equals(currentSection, StringComparison.InvariantCulture)))
            {
                if (currentValue is ICollection<KeyValuePair<string, object>>)
                {
                    var newObj = new ExpandoObject();
                    ((ICollection<KeyValuePair<string, object>>)newObj)
                        .Add(new KeyValuePair<string, object>(currentSection, currentValue));
                    MergeDataObjects(ref dataObject, newObj);
                }
                else
                {
                    throw new NotSupportedException($"Value under section \"{currentSection}\" should have type \"{typeof(ExpandoObject).FullName}\", but was \"{currentValue.GetType().FullName}\"");
                }
            }
            else
            {
                ((ICollection<KeyValuePair<string, object>>)dataObject)
                    .Add(new KeyValuePair<string, object>(currentSection, currentValue));
            }
        }

        private void PopulateSpecialCollection(ref ExpandoObject dataObject, object currentValue)
        {
            ICollection<object> collection;

            if (dataObject.Any(x => x.Key.Equals(COLLECTION_PROPERTY, StringComparison.InvariantCulture)))
            {
                collection = (List<object>) dataObject
                    .Single(x => x.Key.Equals(COLLECTION_PROPERTY, StringComparison.InvariantCulture))
                    .Value;
            }
            else
            {
                collection = new List<object>();
                var collectionProperty = new KeyValuePair<string, object>(COLLECTION_PROPERTY, collection);
                ((ICollection<KeyValuePair<string, object>>)dataObject).Add(collectionProperty);
            }

            collection.Add(currentValue);
        }

        private string GetCurrentSection(IEnumerable<string> sectionsArray)
        {
            var currentSection = sectionsArray.First();

            if (currentSection.Equals(COLLECTION_PROPERTY, StringComparison.InvariantCulture))
            {
                throw new NotSupportedException($"Reserved property name: {COLLECTION_PROPERTY}");
            }
            return currentSection;
        }

        private object GetCurrentValue(IReadOnlyCollection<string> sectionsArray, object value)
        {
            object currentValue;

            if (sectionsArray.Count > 1)
            {
                var obj = new ExpandoObject();
                PopulateDataObject(ref obj, sectionsArray.Skip(1), value);
                currentValue = obj;
            }
            else
            {
                currentValue = value;
            }
            return currentValue;
        }

        private void MergeDataObjects(
            ref ExpandoObject destinationObject,
            ExpandoObject sourceObject)
        {
            foreach (var property in sourceObject.Where(x => !x.Key.Equals(COLLECTION_PROPERTY, StringComparison.InvariantCulture)))
            {
                if (destinationObject.Any(x => x.Key.Equals(property.Key, StringComparison.InvariantCulture)))
                {
                    if (property.Value is ICollection<KeyValuePair<string, object>>)
                    {
                        var pr = (ExpandoObject)destinationObject
                            .Single(x => x.Key.Equals(property.Key, StringComparison.InvariantCulture))
                            .Value;
                        MergeDataObjects(ref pr, (ExpandoObject)property.Value);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
                else
                {
                    ((ICollection<KeyValuePair<string, object>>)destinationObject).Add(property);
                }
            }
        }
        
        private async Task<KeyValuePair<string, object>> GetDataByIdAsync(string dataId)
        {
            await InvokeLogEvent($"{DateTime.Now.ToLongTimeString()}: Started retrieval of data \"{dataId}\"");
            
            var customData = await DataReaderService.LoadContentByIdAsync(dataId);

            try
            {
                return new KeyValuePair<string, object>(customData.SanitezedId, customData.CustomData);
            }
            finally
            {
                await InvokeLogEvent($"{DateTime.Now.ToLongTimeString()}: Finished data retrieval \"{dataId}\"");
            }
        }

        private async Task InvokeLogEvent(string message)
        {
            if (OnLogEvent != null)
            {
                await OnLogEvent.Invoke(message);
            }
        }
    }
}