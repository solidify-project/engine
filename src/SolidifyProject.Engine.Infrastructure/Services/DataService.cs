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
        public const string NAMES_PROPERTY = "__names";

        public readonly static string[] RESERVED_PROPERTIES = {COLLECTION_PROPERTY, NAMES_PROPERTY};
        
        public IContentReaderService<CustomDataModel> DataReaderService { get; }

        public delegate Task LogEventHandler(string message);
        public event LogEventHandler OnLogEvent;
        
        #region constructor
        
        public DataService(IContentReaderService<CustomDataModel> dataReaderService)
        {
            DataReaderService = dataReaderService;
        }

        #endregion
        
        public async Task<ExpandoObject> GetDataModelAsync()
        {
            var data = await DataReaderService.LoadContentsIdsAsync().ConfigureAwait(false);
            var dataTasks = data.Select(GetDataByIdAsync).ToList();

            await Task.WhenAll(dataTasks).ConfigureAwait(false);

            ICollection<KeyValuePair<string, object>> dataModel = new ExpandoObject();
            
            foreach (var result in dataTasks.Select(x => x.Result))
            {
                var sections = result
                    .Key
                    .Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);

                PopulateDataObject(ref dataModel, sections, result.Value);
            }

            PopulateSpecialCollection(ref dataModel);
            
            return (ExpandoObject)dataModel;
        }

        private void PopulateDataObject(
            ref ICollection<KeyValuePair<string, object>> dataObject,
            IEnumerable<string> sections,
            object value)
        {
            var sectionsArray = sections as string[] ?? sections.ToArray();
            
            var currentSection = GetCurrentSection(sectionsArray);
            var currentValue = GetCurrentValue(sectionsArray, value);

            PopulateCurrentSection(ref dataObject, currentSection, currentValue);
        }

        private void PopulateCurrentSection(ref ICollection<KeyValuePair<string, object>> dataObject, string currentSection, object currentValue)
        {
            if (dataObject.Any(x => x.Key.Equals(currentSection, StringComparison.InvariantCulture)))
            {
                if (currentValue is ICollection<KeyValuePair<string, object>>)
                {
                    ICollection<KeyValuePair<string, object>> newObj = new ExpandoObject();
                    newObj.Add(new KeyValuePair<string, object>(currentSection, currentValue));
                    MergeDataObjects(ref dataObject, newObj);
                }
                else
                {
                    throw new NotSupportedException($"Value under section \"{currentSection}\" should have type \"{typeof(ExpandoObject).FullName}\", but was \"{currentValue.GetType().FullName}\"");
                }
            }
            else
            {
                dataObject.Add(new KeyValuePair<string, object>(currentSection, currentValue));
            }
        }

        private void PopulateSpecialCollection(ref ICollection<KeyValuePair<string, object>> dataObject)
        {
            var collection = new List<object>();
            var names = new List<string>();
            
            foreach (var obj in dataObject)
            {
                collection.Add(obj.Value);
                if (obj.Value is ICollection<KeyValuePair<string, object>> expandoObject)
                {
                    PopulateSpecialCollection(ref expandoObject);
                }
                
                names.Add(obj.Key);
            }
            
            dataObject.Add(new KeyValuePair<string, object>(COLLECTION_PROPERTY, collection));
            dataObject.Add(new KeyValuePair<string, object>(NAMES_PROPERTY, names));
        }

        private string GetCurrentSection(IEnumerable<string> sectionsArray)
        {
            var currentSection = sectionsArray.First();

            foreach (var propertyName in RESERVED_PROPERTIES)
            {
                if (currentSection.Equals(propertyName, StringComparison.InvariantCulture))
                {
                    throw new NotSupportedException($"Reserved property name: {propertyName}");
                }
            }
            
            return currentSection;
        }

        private object GetCurrentValue(IReadOnlyCollection<string> sectionsArray, object value)
        {
            object currentValue;

            if (sectionsArray.Count > 1)
            {
                ICollection<KeyValuePair<string, object>> obj = new ExpandoObject();
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
            ref ICollection<KeyValuePair<string, object>> destinationObject,
            ICollection<KeyValuePair<string, object>> sourceObject)
        {
            var properties = RESERVED_PROPERTIES
                .Aggregate<string, IEnumerable<KeyValuePair<string, object>>>(
                    sourceObject,
                    (current, propertyName) => current.Where(x => !x.Key.Equals(propertyName, StringComparison.InvariantCulture)));

            foreach (var property in properties)
            {
                if (destinationObject.Any(x => x.Key.Equals(property.Key, StringComparison.InvariantCulture)))
                {
                    if (property.Value is ICollection<KeyValuePair<string, object>>)
                    {
                        var pr = (ICollection<KeyValuePair<string, object>>)destinationObject
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
                    destinationObject.Add(property);
                }
            }
        }
        
        private async Task<KeyValuePair<string, object>> GetDataByIdAsync(string dataId)
        {
            await InvokeLogEvent($"{DateTime.Now.ToLongTimeString()}: [Data:Started] \"{dataId}\"")
                .ConfigureAwait(false);
            
            var customData = await DataReaderService.LoadContentByIdAsync(dataId)
                .ConfigureAwait(false);

            try
            {
                return new KeyValuePair<string, object>(customData.SanitezedId, customData.CustomData);
            }
            finally
            {
                await InvokeLogEvent($"{DateTime.Now.ToLongTimeString()}: [Data:Finished] \"{dataId}\"")
                    .ConfigureAwait(false);
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